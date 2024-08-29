using Application.handlers.budget.queries.CheckSpentBudget;
using Application.kafka.producer;
using Application.mappers;
using Application.specs;
using Domain.Models;
using FluentAssertions;
using Infrastructure.Repositories;
using Moq;
using System.Linq.Expressions;

namespace Tests.Unit.Queries;

public class CheckSpenBudgetListQueryHandlerTests
{
    private Mock<IBudgetRepository> _budgetRepository;
    private Mock<IGenericRepository<Transaction>> _transactionRepository;
    private Mock<IEventsProducer> _eventsProducer;
    private BudgetMapper _budgetMapper;
    private BudgetSpecs _spec;
    private TransactionSpecs _transactionSpec;
    private CheckSpentBudgetQuery _query;

    [SetUp]
    public void Setup()
    {
        _budgetRepository = new Mock<IBudgetRepository>();
        _transactionRepository = new Mock<IGenericRepository<Transaction>>();
        _eventsProducer = new Mock<IEventsProducer>();
        _budgetMapper = new BudgetMapper();
        _spec = new BudgetSpecs();
        _transactionSpec = new TransactionSpecs();

        var budgets = new List<Budget>
        {
            new()
            {
                UserId = Guid.NewGuid(),
                BudgetLimit = 1000,
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                Id = Guid.NewGuid(),
            }
        };

        _budgetRepository
            .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Budget, bool>>>()))
            .ReturnsAsync(() => budgets);

        _query = new CheckSpentBudgetQuery
        {
            CategoryId = Guid.NewGuid(),
            UserId = Guid.NewGuid()
        };
    }

    [Test]
    public async Task CheckBudget_NotExceededLimit_ReturnsFalse()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new()
            {
                Amount = -500,
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
            }
        };

        _transactionRepository
            .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Transaction, bool>>>()))
            .ReturnsAsync(() => transactions);

        var query = new CheckSpentBudgetQuery
        {
            CategoryId = Guid.NewGuid(),
            UserId = Guid.NewGuid()
        };

        var handler = new CheckSpentBudgetQueryHandler(
            _transactionRepository.Object,
            _budgetRepository.Object, 
            _eventsProducer.Object, _spec,
            _transactionSpec
        );

        // Act
        var result = await handler.Handle(_query);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public async Task CheckBudget_ExceededLimit_ReturnsTrue()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new()
            {
                Amount = -1500,
                CategoryId = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
            }
        };

        _transactionRepository
            .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Transaction, bool>>>()))
            .ReturnsAsync(() => transactions);


        var handler = new CheckSpentBudgetQueryHandler(
            _transactionRepository.Object,
            _budgetRepository.Object,
            _eventsProducer.Object, _spec,
            _transactionSpec
        );

        // Act
        var result = await handler.Handle(_query);

        // Assert
        result.Should().BeTrue();
    }
}