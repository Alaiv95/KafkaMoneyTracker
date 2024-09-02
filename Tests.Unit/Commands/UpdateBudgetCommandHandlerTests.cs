using System.Linq.Expressions;
using Application.exceptions;
using Application.handlers.budget.commands.UpdateBudget;
using Application.specs;
using Domain.Entities.Budget;
using FluentAssertions;
using Infrastructure.Models;
using Infrastructure.Repositories.interfaces;
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

        var limit = Limit.Create(50, 10);

        var resp = new List<BudgetEntity>
        {
            BudgetEntity.Create(
                Limit.Create(222.22, 10),
                Guid.NewGuid(),
                Guid.NewGuid()
            )
        };

        _budgetRepository
            .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Budget, bool>>>()))
            .ReturnsAsync(() => resp);

        var handler = new UpdateBudgetCommandHandler(_budgetRepository.Object, _spec);

        // Act
        var result = await handler.Handle(updateCommand);

        // Assert
        result.Should().BeEquivalentTo(limit);
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
            .ReturnsAsync(() => new List<BudgetEntity>());

        var handler = new UpdateBudgetCommandHandler(_budgetRepository.Object, _spec);

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
        
        var resp = new List<BudgetEntity>
        {
            BudgetEntity.Create(
                Limit.Create(222.22, 10),
                Guid.NewGuid(),
                Guid.NewGuid()
            )
        };

        _budgetRepository
            .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Budget, bool>>>()))
            .ReturnsAsync(() => resp);

        var handler = new UpdateBudgetCommandHandler(_budgetRepository.Object, _spec);

        // Act
        // Assert
        Assert.ThrowsAsync<AllParamsNullException>(() => handler.Handle(updateCommand));
    }
}