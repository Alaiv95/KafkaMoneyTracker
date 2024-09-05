using System.Linq.Expressions;
using Application.handlers.budget.queries.CheckSpentBudget;
using Application.kafka.producer;
using Domain.Entities.Budget;
using Domain.Entities.Transaction;
using FluentAssertions;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Repositories.interfaces;
using Infrastructure.specs;
using Moq;

namespace Tests.Unit.Queries;

public class CheckSpenBudgetListQueryHandlerTests
{
    private Mock<IBudgetRepository> _budgetRepository;
    private Mock<IGenericRepository<Transaction, TransactionEntity>> _transactionRepository;
    private Mock<IEventsProducer> _eventsProducer;
    private BudgetSpecs _spec;
    private TransactionSpecs _transactionSpec;
    private CheckSpentBudgetQuery _query;

    [SetUp]
    public void Setup()
    {
        _budgetRepository = new Mock<IBudgetRepository>();
        _transactionRepository = new Mock<IGenericRepository<Transaction, TransactionEntity>>();
        _eventsProducer = new Mock<IEventsProducer>();
        _spec = new BudgetSpecs();
        _transactionSpec = new TransactionSpecs();

        var budgets = new List<BudgetEntity>
        {
            BudgetEntity.Create(
                Limit.Create(100.12, 12),
                Guid.NewGuid(),
                Guid.NewGuid()
            )
        };

        _budgetRepository
            .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Budget, bool>>>()))
            .ReturnsAsync(() => budgets);

        _query = new CheckSpentBudgetQuery
        {
            BudgetId = Guid.NewGuid(),
            UserId = Guid.NewGuid()
        };
    }

    [Test]
    public async Task CheckBudget_NotExceededLimit_ReturnsFalse()
    {
        // Arrange
        var transactions = new List<TransactionEntity>
        {
            TransactionEntity.Create(
                Guid.NewGuid(),
                Money.Create(123.23, "RUB"),
                Guid.NewGuid()
            )
        };

        _transactionRepository
            .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Transaction, bool>>>()))
            .ReturnsAsync(() => transactions);
        
        var handler = new CheckSpentBudgetQueryHandler(
            _transactionRepository.Object,
            _budgetRepository.Object,
            _eventsProducer.Object,
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
        var transactions = new List<TransactionEntity>
        {
            TransactionEntity.Create(
                Guid.NewGuid(),
                Money.Create(-1500, "RUB"),
                Guid.NewGuid()
            )
        };

        _transactionRepository
            .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Transaction, bool>>>()))
            .ReturnsAsync(() => transactions);
        _budgetRepository
            .Setup(rep => rep.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => BudgetEntity.Create(
                Limit.Create(100, 10),
                Guid.NewGuid(),
                Guid.NewGuid()
            ));

        var handler = new CheckSpentBudgetQueryHandler(
            _transactionRepository.Object,
            _budgetRepository.Object,
            _eventsProducer.Object,
            _transactionSpec
        );

        // Act
        var result = await handler.Handle(_query);

        // Assert
        result.Should().BeTrue();
    }
}