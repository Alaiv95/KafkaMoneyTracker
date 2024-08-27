using Application.budget.queries;
using Application.exceptions;
using Application.mappers;
using Application.specs;
using Domain.Models;
using FluentAssertions;
using Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Tests.Unit.Queries;

public class GetBudgetListQueryHandlerTests
{
    private BudgetMapper _mapper;
    private BudgetSpecs _spec;
    private Mock<IBudgetRepository> _repository;
    private GetBudgetListQuery _query;

    [SetUp]
    public void SetUp()
    {
        _repository = new Mock<IBudgetRepository>();
        _mapper = new BudgetMapper();
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
        var budgetList = new List<Budget>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                BudgetLimit = 100,
                DurationInDays = 30,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            },
            new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                BudgetLimit = 100,
                DurationInDays = 30,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            },
            new()
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                CategoryId = Guid.NewGuid(),
                BudgetLimit = 100,
                DurationInDays = 30,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            },
        };

        var expectedResult = budgetList.Select(_mapper.EntityToDto);

        _repository
          .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Budget, bool>>>()))
          .ReturnsAsync(() => budgetList);

        var queryHandler = new GetBudgetListQueryHandler(_repository.Object, _spec, _mapper);

        // Act
        var result = await queryHandler.Handle(_query);

        // Assert
        result.Should().BeOfType<GetBudgetListDto>();
        result.Budgets.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task GetBudgets_DataNotReturned_Success()
    {
        // Arrange
        var budgetList = new List<Budget>();
        var expectedResult = new List<BudgetLookUpDto>();

        _repository
          .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Budget, bool>>>()))
          .ReturnsAsync(() => budgetList);

        var queryHandler = new GetBudgetListQueryHandler(_repository.Object, _spec, _mapper);

        // Act
        var result = await queryHandler.Handle(_query);

        // Assert
        result.Should().BeOfType<GetBudgetListDto>();
        result.Budgets.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public async Task GetBudgets_InvalidDates_Fail()
    {
        // Arrange
        _query = new GetBudgetListQuery()
        {
            DateFrom = DateTime.Now.AddDays(10),
            DateTo = DateTime.Now,
            UserId = Guid.NewGuid(),
        };

        var queryHandler = new GetBudgetListQueryHandler(_repository.Object, _spec, _mapper);

        // Act
        // Assert
        Assert.ThrowsAsync<DateFromCantBeLessThenDateToException>(() => queryHandler.Handle(_query));
    }
}
