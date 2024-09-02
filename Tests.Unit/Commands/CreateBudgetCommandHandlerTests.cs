using Application.exceptions;
using Application.handlers.budget.commands.CreateBudget;
using Application.specs;
using FluentAssertions;
using Infrastructure.Repositories;
using Moq;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Entities.Budget;
using Domain.Entities.User;
using Domain.Enums;
using Infrastructure.Models;
using Infrastructure.Repositories.interfaces;

namespace Tests.Unit.Commands;

public class CreateBudgetCommandHandlerTests
{
    private Mock<IBudgetRepository> _budgetRepository;
    private Mock<IGenericRepository<User, UserEntity>> _userRepository;
    private Mock<IGenericRepository<Category, CategoryEntity>> _categoryRepository;
    private CreateBudgetCommand _command;
    private BudgetSpecs _spec;
    private UserEntity _user;
    private CategoryEntity _category;
    private BudgetEntity _budget;

    [SetUp]
    public void Setup()
    {
        _budgetRepository = new Mock<IBudgetRepository>();
        _userRepository = new Mock<IGenericRepository<User, UserEntity>>();
        _categoryRepository = new Mock<IGenericRepository<Category, CategoryEntity>>();
        _spec = new BudgetSpecs();

        _command = new CreateBudgetCommand
        {
            UserId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            BudgetLimit = 100.23,
            DurationInDays = 30
        };
        
        _user = UserEntity.Create("TEST", "test");
        _category = CategoryEntity.Create(CategoryValue.Create(CategoryType.Custom, "alal"), _user.Id);
        _budget = BudgetEntity.Create(Limit.Create(102.11, 33), Guid.NewGuid(),_user.Id);
    }

    [Test]
    public async Task CreateBudget_WithValidData_Success()
    {
        // Arrange
        _userRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => _user);

        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => _category);

        _budgetRepository
            .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Budget, bool>>>()))
            .ReturnsAsync(() => new List<BudgetEntity>());

        var handler = new CreateBudgetCommandHandler(_budgetRepository.Object, _categoryRepository.Object, _spec);

        // Act
        var result = await handler.Handle(_command);

        // Assert
        result.Should().NotBeEmpty();
    }

    [Test]
    public async Task CreateBudget_NotExistingCategoryId_Fail()
    {
        // Arrange
        _userRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => _user);

        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        _budgetRepository
            .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Budget, bool>>>()))
            .ReturnsAsync(() => new List<BudgetEntity>());

        var handler = new CreateBudgetCommandHandler(_budgetRepository.Object, _categoryRepository.Object, _spec);

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
            .ReturnsAsync(() => _user);

        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => _category);

        _budgetRepository
            .Setup(repository => repository.SearchAsync(It.IsAny<Expression<Func<Budget, bool>>>()))
            .ReturnsAsync(() => new List<BudgetEntity>() { _budget });

        var handler = new CreateBudgetCommandHandler(_budgetRepository.Object, _categoryRepository.Object, _spec);

        // Act
        // Assert
        Assert.ThrowsAsync<BudgetForCategoryAlreadyExistsException>(async () => await handler.Handle(_command));
    }
}