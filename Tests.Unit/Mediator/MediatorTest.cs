using Application.exceptions;
using Application.mediator;
using Application.mediator.interfaces;
using Application.mediatorHandlers.budget.commands;
using Application.specs;
using Domain.Models;
using FluentAssertions;
using Infrastructure.Repositories;
using Moq;

namespace Tests.Unit.Commands;

public class MediatorTest
{
    private Mock<IBudgetRepository> _budgetRepository;
    private Mock<IGenericRepository<User>> _userRepository;
    private Mock<IGenericRepository<Category>> _categoryRepository;
    private CreateBudgetCommand _command;
    private BudgetSpecs _spec;

    [SetUp]
    public void Setup()
    {
        _budgetRepository = new Mock<IBudgetRepository>();
        _userRepository = new Mock<IGenericRepository<User>>();
        _categoryRepository = new Mock<IGenericRepository<Category>>();
        _spec = new BudgetSpecs();
    }

    [Test]
    public async Task TryAddCommand_WhenAlreadyAdded()
    {
        // Arrange
        var mediator = new Mediator();
        var handler = new CreateBudgetCommandHandler(_budgetRepository.Object, _userRepository.Object, _categoryRepository.Object, _spec);
        mediator.Register(handler);

        // Act
        // Assert
        Assert.Throws<CommandAlreadyRegisteredException>(() => mediator.Register(handler));
    }

    [Test]
    public async Task TestHandleAddedCommand_Success()
    {
        // Arrange
        var mediator = new Mediator();
        var handler = new Mock<IRequestHandler<CreateBudgetCommand, Guid>>();
        var result = Guid.NewGuid();


        handler
            .Setup(handler => handler.Handle(It.IsAny<CreateBudgetCommand>()))
            .ReturnsAsync(() => result);

        mediator.Register(handler.Object);

        var c = new CreateBudgetCommand
        {
            UserId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            BudgetLimit = 100.23,
            DurationInDays = 30
        };

        // Act

        var res = await mediator.HandleRequest(c);

        // Assert
        result.Should().NotBeEmpty();
    }

    [Test]
    public async Task TestHandleNotAddedCommand_Fail()
    {
        // Arrange
        var mediator = new Mediator();

        var c = new CreateBudgetCommand
        {
            UserId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            BudgetLimit = 100.23,
            DurationInDays= 30
        };

        // Act
        // Assert
        Assert.ThrowsAsync<CommandNotRegisteredException>(() => mediator.HandleRequest(c));
    }
}