using Application.exceptions;
using Application.handlers.transactions.commands.CreateTransaction;
using Application.kafka.producer;
using Application.utils.CurrencyConverter;
using Domain.Entities.Budget;
using Domain.Entities.Transaction;
using FluentAssertions;
using Infrastructure.Repositories.interfaces;
using Infrastructure.specs;
using Moq;

namespace Tests.Unit.Commands;

public class CreateTransactionCommandHandlerTests
{
    private Mock<IBudgetRepository> _budgetRepository;
    private Mock<IEventsProducer> _eventsProducer;
    private Mock<IAuthRepository> _authRepository;
    private Mock<IConverter<Money>> _converter;
    private BudgetSpecs _budgetSpecs;

    [SetUp]
    public void Setup()
    {
        _budgetRepository = new Mock<IBudgetRepository>();
        _eventsProducer = new Mock<IEventsProducer>();
        _authRepository = new Mock<IAuthRepository>();
        _converter = new Mock<IConverter<Money>>();
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

        var handler = new CreateTransactionCommandHandler(
            _budgetRepository.Object,
            _eventsProducer.Object,
            _authRepository.Object,
            _converter.Object
        );

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
            new CreateTransactionCommandHandler(
                _budgetRepository.Object,
                _eventsProducer.Object,
                _authRepository.Object,
                _converter.Object
            );

        // Act
        // Assert
        Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }
}