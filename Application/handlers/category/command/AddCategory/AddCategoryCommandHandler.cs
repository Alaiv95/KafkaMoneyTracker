using Application.Dtos;
using Application.exceptions;
using Application.mediator.interfaces;
using Domain.Enums;
using Infrastructure.Models;
using Infrastructure.Repositories;

namespace Application.handlers.category.command.AddCategory;

public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, CategoryLookupDto>
{
    private readonly ICategoryRepository _repository;
    
    public AddCategoryCommandHandler(ICategoryRepository repository) => _repository = repository;
    
    public async Task<CategoryLookupDto> Handle(AddCategoryCommand command)
    {
        if (await IsCategoryWithNameExists(command.Name))
        {
            throw new CategoryAlreadyExistsException($"Category {command.Name} already exists.");
        } 
        
        var category = new Category
        {
            CategoryType = CategoryType.Custom,
            CategoryName = command.Name,
            Id = Guid.NewGuid(),
            IsCustom = true,
            CreatedBy = command.UserId
        };

        await _repository.AddAsync(category);

        return new CategoryLookupDto
        {
            CategoryType = CategoryType.Custom,
            CategoryName = category.CategoryName,
            Id = category.Id
        };
    }
    
    private async Task<bool> IsCategoryWithNameExists(string name)
    {
        return await _repository.GetByNameAsync(name) != null;
    }
}