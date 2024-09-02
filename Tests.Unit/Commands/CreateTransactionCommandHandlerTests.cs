using System.Linq.Expressions;
using Application.exceptions;
using Application.handlers.transactions.commands.CreateTransaction;
using Application.kafka.producer;
using Application.specs;
using Domain.Entities;
using Domain.Entities.Budget;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Repositories.interfaces;
using Moq;

namespace Tests.Unit.Commands;

public class CreateTransactionCommandHandlerTests
{
    private Mock<IBudgetRepository> _budgetRepository;
    private Mock<IEventsProducer> _eventsProducer;
    private BudgetSpecs _budgetSpecs;

    [SetUp]
    public void Setup()
    {
        ;
        _budgetRepository = new Mock<IBudgetRepository>();
        _eventsProducer = new Mock<IEventsProducer>();
        _budgetSpecs = new BudgetSpecs();
    }

    [Test]
    public async Task CreateTransaction_BudgetFound_Success()
    {
        // Arrange
        _budgetRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => BudgetEntity.Create(Limit.Create(100.10, 10), Guid.NewGuid(), Guid.NewGuid())
            );

        var command = new CreateTransactionCommand
        {
            Amount = 100,
            BudgetId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
        };

        var handler =
            new CreateTransactionCommandHandler(_budgetRepository.Object, _eventsProducer.Object, _budgetSpecs);

        // Act
        var result = await handler.Handle(command);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task CreateTransaction_CategoryNotFound_Throws()
    {
        // Arrange
        _budgetRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        var command = new CreateTransactionCommand
        {
            Amount = 100,
            BudgetId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
        };

        var handler =
            new CreateTransactionCommandHandler(_budgetRepository.Object, _eventsProducer.Object, _budgetSpecs);

        // Act
        // Assert
        Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }
}