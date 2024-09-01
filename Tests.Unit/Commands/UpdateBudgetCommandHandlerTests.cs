using System.Linq.Expressions;
using Application.exceptions;
using Application.handlers.budget.commands.UpdateBudget;
using Application.specs;
using Domain.Models;
using FluentAssertions;
using Infrastructure.Repositories;
using Moq;

namespace Tests.Unit.Commands;

public class UpdateBudgetCommandHandlerTests : TestBase
{
    private Mock<IBudgetRepository> _budgetRepository;
    private BudgetSpecs _spec;

    [SetUp]
    public void Setup()
    {
        _budgetRepository = new Mock<IBudgetRepository>();
        _spec = new BudgetSpecs();
    }

    [Test]
    public async Task UpdateBudget_WithPassedParams_Success()
    {
        // Arrange
        var updateCommand = new UpdateBudgetCommand
        {
            CategoryId = Guid.NewGuid(),
            BudgetLimit = 50,
            DurationInDays = 10,
            UserId = Guid.NewGuid()
        };
        
        var dbResponse = new List<Budget>
        {
            new()
            {
                UserId = Guid.NewGuid(),
                BudgetLimit = 100,
                CategoryId = Guid.NewGuid(),
                DurationInDays = 20,
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
                Id = Guid.NewGuid()
            }
        };

        _budgetRepository
            .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Budget, bool>>>()))
            .ReturnsAsync(() => dbResponse);

        var handler = new UpdateBudgetCommandHandler(_budgetRepository.Object, _spec, Mapper);

        // Act
        var result = await handler.Handle(updateCommand);

        // Assert
        result.BudgetLimit.Should().Be(updateCommand.BudgetLimit);
        result.DurationInDays.Should().Be(updateCommand.DurationInDays);
        result.Id.Should().Be(dbResponse.First().Id);
    }
    
    [Test]
    public async Task UpdateBudget_NotFoundBudget_Fail()
    {
        // Arrange
        var updateCommand = new UpdateBudgetCommand
        {
            CategoryId = Guid.NewGuid(),
            BudgetLimit = 50,
            DurationInDays = 10,
            UserId = Guid.NewGuid()
        };
        

        _budgetRepository
            .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Budget, bool>>>()))
            .ReturnsAsync(() => new List<Budget>());

        var handler = new UpdateBudgetCommandHandler(_budgetRepository.Object, _spec, Mapper);

        // Act
        // Assert
        Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(updateCommand));
    }
    
    [Test]
    public async Task UpdateBudget_NoPassedParams_Fail()
    {
        // Arrange
        var updateCommand = new UpdateBudgetCommand
        {
            CategoryId = Guid.NewGuid(),
            UserId = Guid.NewGuid()
        };
        var dbResponse = new List<Budget>
        {
            new()
            {
                UserId = Guid.NewGuid(),
                BudgetLimit = 100,
                CategoryId = Guid.NewGuid(),
                DurationInDays = 20,
                CreatedAt = DateTime.Now,
                UpdatedAt = null,
                Id = Guid.NewGuid()
            }
        };

        _budgetRepository
            .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Budget, bool>>>()))
            .ReturnsAsync(() => dbResponse);

        var handler = new UpdateBudgetCommandHandler(_budgetRepository.Object, _spec, Mapper);

        // Act
        // Assert
        Assert.ThrowsAsync<AllParamsNullException>(() => handler.Handle(updateCommand));
    }
}