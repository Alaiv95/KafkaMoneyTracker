using Application.Dtos;
using Application.handlers.transactions.commands.CancelTransactions;
using FluentAssertions;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Moq;

namespace Tests.Unit.Commands;

public class CancelTransactionCommandHandlerTests
{
    private Mock<ITransactionRepository> _transactionRepository;

    [SetUp]
    public void Setup()
    {
        _transactionRepository = new Mock<ITransactionRepository>();
    }

    [Test]
    public async Task CancelTransactions_WithData_Success()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var transactionsResponse = new List<Transaction>
        {
            new()
            {
                Amount = 100,
                CategoryId = Guid.NewGuid(),
                UserId = userId,
                Id = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new()
            {
                Amount = 200,
                CategoryId = Guid.NewGuid(),
                UserId = userId,
                Id = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new()
            {
                Amount = 300,
                CategoryId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Id = Guid.NewGuid(),
                IsActive = true,
                CreatedAt = DateTime.Now
            },
        };

        var expectedResult = transactionsResponse
            .Where(t => t.UserId == userId)
            .Select(t =>
                new TransactionLookupDto
                {
                    CategoryId = t.CategoryId,
                    IsActive = false,
                    Amount = t.Amount
                });

        var command = new CancelTransactionsCommand
        {
            UserId = userId,
            TransactionIds = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
            }
        };

        _transactionRepository
            .Setup(repository => repository.GetByIdsAsync(It.IsAny<List<Guid>>()))
            .ReturnsAsync(() => transactionsResponse);

        var handler = new CancelTransactionsCommandHandler(_transactionRepository.Object);

        // Act
        var result = await handler.Handle(command);

        // Assert
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    [Test]
    public async Task CreateBudget_WithoutData_ReturnsEmpty()
    {
        // Arrange
        var command = new CancelTransactionsCommand
        {
            TransactionIds = new List<Guid>()
        };

        var handler = new CancelTransactionsCommandHandler(_transactionRepository.Object);

        // Act
        var result = await handler.Handle(command);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
    
    [Test]
    public async Task CreateBudget_WithNull_ReturnsEmpty()
    {
        // Arrange
        var command = new CancelTransactionsCommand
        {
            TransactionIds = null
        };

        var handler = new CancelTransactionsCommandHandler(_transactionRepository.Object);

        // Act
        var result = await handler.Handle(command);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}