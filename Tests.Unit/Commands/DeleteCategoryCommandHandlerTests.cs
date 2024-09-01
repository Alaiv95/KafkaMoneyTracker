using Application.Dtos;
using Application.exceptions;
using Application.handlers.category.command.DeleteCategory;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Moq;

namespace Tests.Unit.Commands;

public class DeleteCategoryCommandHandlerTests : TestBase
{
    private Mock<ICategoryRepository> _categoryRepository;

    [SetUp]
    public void Setup()
    {
        _categoryRepository = new Mock<ICategoryRepository>();
    }

    [Test]
    public async Task DeleteCategory_ValidData_Success()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var command = new DeleteCategoryCommand()
        {
            CategoryId = categoryId,
            UserId = userId
        };

        var dbResponse = new Category()
        {
            CategoryName = "asd",
            CategoryType = CategoryType.Custom,
            IsCustom = true,
            Id = categoryId,
            CreatedBy = userId
        };

        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => dbResponse);

        var handler = new DeleteCategoryCommandHandler(_categoryRepository.Object, Mapper);

        // Act
        var result = await handler.Handle(command);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(Mapper.Map<CategoryLookupDto>(dbResponse));
    }

    [Test]
    public async Task TryDeleteCategory_WrongUser_Fail()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user2Id = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var command = new DeleteCategoryCommand()
        {
            CategoryId = categoryId,
            UserId = userId
        };

        var dbResponse = new Category()
        {
            CategoryName = "asd",
            CategoryType = CategoryType.Custom,
            IsCustom = true,
            Id = categoryId,
            CreatedBy = user2Id
        };

        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => dbResponse);

        var handler = new DeleteCategoryCommandHandler(_categoryRepository.Object, Mapper);

        // Act
        // Assert
        Assert.ThrowsAsync<CategoryCantBeDeletedException>(() => handler.Handle(command));
    }

    [Test]
    public async Task TryDeleteCategory_NotCustom_Fail()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var command = new DeleteCategoryCommand()
        {
            CategoryId = categoryId,
            UserId = userId
        };

        var dbResponse = new Category()
        {
            CategoryName = "asd",
            CategoryType = CategoryType.Food,
            IsCustom = false,
            Id = categoryId,
            CreatedBy = userId
        };

        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => dbResponse);

        var handler = new DeleteCategoryCommandHandler(_categoryRepository.Object, Mapper);

        // Act
        // Assert
        Assert.ThrowsAsync<CategoryCantBeDeletedException>(() => handler.Handle(command));
    }

    [Test]
    public async Task TryDeleteCategory_CategoryNotExists_Fail()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var categoryId = Guid.NewGuid();

        var command = new DeleteCategoryCommand()
        {
            CategoryId = categoryId,
            UserId = userId
        };

        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => null);

        var handler = new DeleteCategoryCommandHandler(_categoryRepository.Object, Mapper);

        // Act
        // Assert
        Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(command));
    }
}