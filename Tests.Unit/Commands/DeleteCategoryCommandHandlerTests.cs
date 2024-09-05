using Application.Dtos;
using Application.Dtos.category;
using Application.exceptions;
using Application.handlers.category.command.DeleteCategory;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Repositories.interfaces;
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

        var category = CategoryEntity.Create(
            CategoryValue.Create(CategoryType.Custom, "asd"),
            userId
        );
        
        var command = new DeleteCategoryCommand()
        {
            CategoryId = category.Id,
            UserId = userId
        };


        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => category);

        var handler = new DeleteCategoryCommandHandler(_categoryRepository.Object, Mapper);

        // Act
        var result = await handler.Handle(command);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(Mapper.Map<CategoryLookupDto>(category));
    }

    [Test]
    public async Task TryDeleteCategory_WrongUser_Fail()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user2Id = Guid.NewGuid();
        
        var category = CategoryEntity.Create(
            CategoryValue.Create(CategoryType.Custom, "asd"),
            user2Id
        );
        
        var command = new DeleteCategoryCommand()
        {
            CategoryId = category.Id,
            UserId = userId
        };
        

        _categoryRepository
            .Setup(repository => repository.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(() => category);

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

        var dbResponse = CategoryEntity.Create(CategoryValue.Create(CategoryType.Entertainment, "alal"), userId);
        
        var command = new DeleteCategoryCommand()
        {
            CategoryId = dbResponse.Id,
            UserId = userId
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