using Application.exceptions;
using Application.handlers.category.command.AddCategory;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Repositories.interfaces;
using Moq;

namespace Tests.Unit.Commands;

public class AddCategoryCommandHandlerTests
{
    private Mock<ICategoryRepository> _categoryRepository;

    [SetUp]
    public void Setup()
    {
        _categoryRepository = new Mock<ICategoryRepository>();
    }

    [Test]
    public async Task AddCategory_UniqueName_Success()
    {
        // Arrange
        var command = new AddCategoryCommand
        {
            Name = "Category-test",
            UserId = Guid.NewGuid()
        };

        _categoryRepository
            .Setup(repository => repository.GetByNameAsync(It.IsAny<String>()))
            .ReturnsAsync(() => null);

        var handler = new AddCategoryCommandHandler(_categoryRepository.Object);

        // Act
        var result = await handler.Handle(command);

        // Assert
        result.Should().NotBeNull();
        result.CategoryName.Should().Be(command.Name);
        result.CategoryType.Should().Be(CategoryType.Custom);
    }

    [Test]
    public async Task AddCategory_NameExists_Throws()
    {
        // Arrange
        var command = new AddCategoryCommand
        {
            Name = "Category-test"
        };

        var category = CategoryEntity.Create(
            CategoryValue.Create(CategoryType.Custom, command.Name),
            Guid.NewGuid()
        );

        _categoryRepository
            .Setup(repository => repository.GetByNameAsync(It.IsAny<String>()))
            .ReturnsAsync(() => category);

        var handler = new AddCategoryCommandHandler(_categoryRepository.Object);

        // Act
        // Assert
        Assert.ThrowsAsync<CategoryAlreadyExistsException>(() => handler.Handle(command));
    }
}