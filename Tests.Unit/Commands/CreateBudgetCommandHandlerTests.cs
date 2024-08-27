using Application.exceptions;
using Application.mediatorHandlers.budget.commands;
using Application.specs;
using Domain.Models;
using FluentAssertions;
using Infrastructure.Repositories;
using Moq;
using System.Linq.Expressions;

namespace Tests.Unit.Commands;

public class CreateBudgetCommandHandlerTests
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

        _command = new CreateBudgetCommand
        {
            UserId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            BudgetLimit = 100.23,
            DurationInDays = 30
        };
    }

    [Test]
    public async Task CreateBudget_WithValidData_Success()
    {
        // Arrange
        _userRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => new User());

        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => new Category());

        _budgetRepository
            .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Budget, bool>>>()))
            .ReturnsAsync(() => new List<Budget>());

        var handler = new CreateBudgetCommandHandler(_budgetRepository.Object, _userRepository.Object, _categoryRepository.Object, _spec);

        // Act
        var result = await handler.Handle(_command);

        // Assert
        result.Should().NotBeEmpty();
    }

    [Test]
    public async Task CreateBudget_NotExistingUserId_Fail()
    {
        // Arrange
        _userRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => new Category());

        _budgetRepository
            .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Budget, bool>>>()))
            .ReturnsAsync(() => new List<Budget>());

        var handler = new CreateBudgetCommandHandler(_budgetRepository.Object, _userRepository.Object, _categoryRepository.Object, _spec);

        // Act
        // Assert
        Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(_command));
    }

    [Test]
    public async Task CreateBudget_NotExistingCategoryId_Fail()
    {
        // Arrange
        _userRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => new User());

        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        _budgetRepository
            .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Budget, bool>>>()))
            .ReturnsAsync(() => new List<Budget>());

        var handler = new CreateBudgetCommandHandler(_budgetRepository.Object, _userRepository.Object, _categoryRepository.Object, _spec);

        // Act
        // Assert
        Assert.ThrowsAsync<NotFoundException>(async () => await handler.Handle(_command));
    }

    [Test]
    public async Task CreateBudget_AlreadyExists_Fail()
    {
        // Arrange
        _userRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => new User());

        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => new Category());

        _budgetRepository
            .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Budget, bool>>>()))
            .ReturnsAsync(() => new List<Budget>() { new Budget()});

        var handler = new CreateBudgetCommandHandler(_budgetRepository.Object, _userRepository.Object, _categoryRepository.Object, _spec);

        // Act
        // Assert
        Assert.ThrowsAsync<BudgetForCategoryAlreadyExistsException>(async () => await handler.Handle(_command));
    }
}