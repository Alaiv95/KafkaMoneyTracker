﻿using Application.exceptions;
using Application.handlers.budget.commands.CreateBudget;
using Application.mediator;
using Application.mediator.exceptions;
using Application.mediator.interfaces;
using Domain.Entities;
using Domain.Entities.User;
using FluentAssertions;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Repositories.interfaces;
using Infrastructure.specs;
using Moq;

namespace Tests.Unit.Commands;

public class MediatorTest
{
    private Mock<IBudgetRepository> _budgetRepository;
    private Mock<IGenericRepository<User, UserEntity>> _userRepository;
    private Mock<IGenericRepository<Category, CategoryEntity>> _categoryRepository;
    private CreateBudgetCommand _command;
    private BudgetSpecs _spec;

    [SetUp]
    public void Setup()
    {
        _budgetRepository = new Mock<IBudgetRepository>();
        _userRepository = new Mock<IGenericRepository<User, UserEntity>>();
        _categoryRepository = new Mock<IGenericRepository<Category, CategoryEntity>>();
        _spec = new BudgetSpecs();
    }

    [Test]
    public async Task TryAddCommand_WhenAlreadyAdded()
    {
        // Arrange
        var mediator = new Mediator();
        var handler = new CreateBudgetCommandHandler(_budgetRepository.Object, _categoryRepository.Object, _spec);
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