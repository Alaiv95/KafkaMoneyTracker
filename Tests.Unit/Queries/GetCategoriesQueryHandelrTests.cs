using Application.Dtos;
using Application.handlers.category.queries;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Repositories.interfaces;
using Microsoft.AspNetCore.Components.Web;
using Moq;
using Org.BouncyCastle.Crypto.Digests;

namespace Tests.Unit.Queries;

public class GetCategoriesQueryHandelrTests
{
    private Mock<ICategoryRepository> _categoryRepository;
    private List<CategoryEntity> _categories;

    [SetUp]
    public void Setup()
    {
        _categoryRepository = new Mock<ICategoryRepository>();
        
        _categories = new List<CategoryEntity>
        {
            CategoryEntity.Create(CategoryValue.Create(CategoryType.Entertainment, "test"), Guid.NewGuid()),
            CategoryEntity.Create(CategoryValue.Create(CategoryType.Custom, "test123"), Guid.NewGuid())
        };
        
        _categoryRepository
            .Setup(repository => repository.GetAllAsync())
            .ReturnsAsync(() => _categories);
    }

    [Test]
    public async Task GetCategory_IncludeCustom_Success()
    {
        // Arrange
        var query = new GetCategoriesQuery()
        {
            IncludeCustom = true
        };
        

        var expectedResult = _categories.Select(c => new CategoryLookupDto
        {
            CategoryType = c.CategoryValue.CategoryType,
            CategoryName = c.CategoryValue.CategoryName,
            Id = c.Id,
        });

        var handler = new GetCategoriesQueryHandler(_categoryRepository.Object);

        // Act
        var result = await handler.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
    }
    
    [Test]
    public async Task GetCategory_NotIncludeCustom_Success()
    {
        // Arrange
        var query = new GetCategoriesQuery()
        {
            IncludeCustom = false
        };

        var expectedResult = _categories
            .Where(c => !c.IsCustom)
            .Select(c => new CategoryLookupDto
        {
            CategoryType = c.CategoryValue.CategoryType,
            CategoryName = c.CategoryValue.CategoryName,
            Id = c.Id,
        });

   

        var handler = new GetCategoriesQueryHandler(_categoryRepository.Object);

        // Act
        var result = await handler.Handle(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedResult);
    }
}