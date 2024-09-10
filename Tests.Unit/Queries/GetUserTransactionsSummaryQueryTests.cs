using System.Linq.Expressions;
using Application.Dtos;
using Application.handlers.transactions.queries.Transactions.GetUserTransactionsSummary;
using Core.common;
using Domain.Entities.Budget;
using Domain.Entities.Transaction;
using FluentAssertions;
using Infrastructure.Models;
using Infrastructure.Repositories.interfaces;
using Infrastructure.specs;
using Moq;

namespace Tests.Unit.Queries;

public class GetUserTransactionsSummaryQueryTests : TestBase
{
    private Mock<ITransactionRepository> _transactionRepository;
    private Mock<IBudgetRepository> _budgetRepository;
    private TransactionSpecs _spec;

    [SetUp]
    public void Setup()
    {
        _transactionRepository = new Mock<ITransactionRepository>();
        _budgetRepository = new Mock<IBudgetRepository>();
        _spec = new TransactionSpecs();
    }

    [Test]
    public async Task GetSummary_DataExists_ReturnSummarySuccess()
    {
        // Arrange
        var budget = BudgetEntity.Create(
            Limit.Create(100.12, 12),
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        var info = new List<TransactionInfo>
        {
            new(
                100, 100, true, "eur",
                new BudgetInfo { BudgetLimit = 200, CategoryName = "First", DurationInDays = 90 }
            ),
            new(
                -200, -200,true, "eur",
                new BudgetInfo { BudgetLimit = 200, CategoryName = "First", DurationInDays = 90 }
            ),
            new(
                200, 200, true, "eur",
                new BudgetInfo { BudgetLimit = 200, CategoryName = "First", DurationInDays = 90 }
            ),
            new(
                -500, -500, true, "eur",
                new BudgetInfo { BudgetLimit = 200, CategoryName = "Second", DurationInDays = 90 }
            ),
        };

        var expectedResult = new List<TransactionSummaryDto>
        {
            new()
            {
                CategoryName = "First",
                Expenses = -200,
                Income = 300
            },
            new()
            {
                CategoryName = "Second",
                Expenses = -500,
                Income = 0
            }
        };

        var container = new PaginationContainer<TransactionSummaryDto>
        {
            Data = expectedResult,
            PageNumber = 1,
            TotalPages = 1
        };


        _budgetRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => budget);

        _transactionRepository
            .Setup(repository => repository.SearchWithIncludeAsync(
                    It.IsAny<Expression<Func<Transaction, bool>>>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()
                )
            )
            .ReturnsAsync(() => info);
        
        _transactionRepository
            .Setup(repository => repository.CountTransactionsAsync(It.IsAny<Expression<Func<Transaction, bool>>>())
            )
            .ReturnsAsync(() => 10);

        var handler = new GetUserTransactionsSummaryQueryHandler(
            _transactionRepository.Object,
            _spec
        );


        var query = new GetUserTransactionsSummaryQuery
        {
            BudgetId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            PageNumber = 1,
            DisplayLimit = 10
        };

        // Act
        var result = await handler.Handle(query);

        // Assert
        result.Should().BeEquivalentTo(container);
    }

    [Test]
    public async Task GetSummary_DataNotReturned_EmptyResultSuccess()
    {
        // Arrange
        _budgetRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        _transactionRepository
            .Setup(repository => repository.SearchWithIncludeAsync(
                    It.IsAny<Expression<Func<Transaction, bool>>>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()
                )
            ).ReturnsAsync(() => null);

        var handler = new GetUserTransactionsSummaryQueryHandler(
            _transactionRepository.Object,
            _spec
        );


        var query = new GetUserTransactionsSummaryQuery
        {
            BudgetId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            DisplayLimit = 1,
            PageNumber = 10
        };

        // Act
        var result = await handler.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().BeEmpty();
    }
}