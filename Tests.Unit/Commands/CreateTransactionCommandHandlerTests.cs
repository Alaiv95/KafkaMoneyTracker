using Application.exceptions;
using Application.handlers.transactions.commands.CreateTransaction;
using Application.kafka.producer;
using Domain.Models;
using FluentAssertions;
using Infrastructure.Repositories;
using Moq;

namespace Tests.Unit.Commands;

public class CreateTransactionCommandHandlerTests
{
    private Mock<IGenericRepository<Category>> _categoryRepository;
    private Mock<IEventsProducer> _eventsProducer;

    [SetUp]
    public void Setup()
    {;
        _categoryRepository = new Mock<IGenericRepository<Category>>();
        _eventsProducer = new Mock<IEventsProducer>();
    }

    [Test]
    public async Task CreateTransaction_CategoryFound_Success()
    {
        // Arrange
        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => new Category());

        var command = new CreateTransactionCommand
        {
            Amount = 100,
            CategoryId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
        };

        var handler = new CreateTransactionCommandHandler(_categoryRepository.Object, _eventsProducer.Object);

        // Act
        var result = await handler.Handle(command);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public async Task CreateTransaction_CategoryNotFound_Throws()
    {
        // Arrange
        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        var command = new CreateTransactionCommand
        {
            Amount = 100,
            CategoryId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
        };

        var handler = new CreateTransactionCommandHandler(_categoryRepository.Object, _eventsProducer.Object);

        // Act
        // Assert
        Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }
}
