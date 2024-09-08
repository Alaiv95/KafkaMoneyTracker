using Application.exceptions;
using Application.handlers.budget.queries.GetBudgetList;
using Application.mappers;
using FluentAssertions;
using Infrastructure.Repositories;
using Moq;
using System.Linq.Expressions;
using Core.common;
using Domain.Entities.Budget;
using Infrastructure.HttpClientS;
using Infrastructure.Models;
using Infrastructure.Repositories.interfaces;
using Infrastructure.specs;

namespace Tests.Unit.Queries;

public class GetBudgetListQueryHandlerTests : TestBase
{
    private BudgetSpecs _spec;
    private Mock<IBudgetRepository> _repository;
    private GetBudgetListQuery _query;

    [SetUp]
    public void SetUp()
    {
        _repository = new Mock<IBudgetRepository>();
        _spec = new BudgetSpecs();

        _query = new GetBudgetListQuery()
        {
            DateFrom = DateTime.Now,
            DateTo = DateTime.Now.AddDays(10),
            UserId = Guid.NewGuid(),
        };
    }

    [Test]
    public async Task GetBudgets_DataReturned_Success()
    {
        // Arrange
        var budgets = new List<BudgetEntity>
        {
            BudgetEntity.Create(
                Limit.Create(100.12, 12),
                Guid.NewGuid(),
                Guid.NewGuid()
            ),
            BudgetEntity.Create(
                Limit.Create(102.12, 14),
                Guid.NewGuid(),
                Guid.NewGuid()
            ),
            BudgetEntity.Create(
                Limit.Create(-100.12, 2),
                Guid.NewGuid(),
                Guid.NewGuid()
            )
        };
        
        var expectedResult = Mapper.Map<List<BudgetLookUpVm>>(budgets);

        _repository
          .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Budget, bool>>>()))
          .ReturnsAsync(() => budgets);

        var queryHandler = new GetBudgetListQueryHandler(_repository.Object, _spec, Mapper);

        // Act
        var result = await queryHandler.Handle(_query);

        // Assert
        result.Should().BeOfType<BudgetListVm>();
        result.Budgets.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task GetBudgets_DataNotReturned_Success()
    {
        // Arrange
        var budgetList = new List<BudgetEntity>();
        var expectedResult = new List<BudgetLookUpVm>();

        _repository
          .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Budget, bool>>>()))
          .ReturnsAsync(() => budgetList);

        var queryHandler = new GetBudgetListQueryHandler(_repository.Object, _spec, Mapper);

        // Act
        var result = await queryHandler.Handle(_query);

        // Assert
        result.Should().BeOfType<BudgetListVm>();
        result.Budgets.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task GetBudgets_InvalidDates_Throws()
    {
        // Arrange
        _query = new GetBudgetListQuery()
        {
            DateFrom = DateTime.Now.AddDays(10),
            DateTo = DateTime.Now,
            UserId = Guid.NewGuid(),
        };

        var queryHandler = new GetBudgetListQueryHandler(_repository.Object, _spec, Mapper);

        // Act
        // Assert
        Assert.ThrowsAsync<DateFromCantBeLessThenDateToException>(() => queryHandler.Handle(_query));
    }
}
