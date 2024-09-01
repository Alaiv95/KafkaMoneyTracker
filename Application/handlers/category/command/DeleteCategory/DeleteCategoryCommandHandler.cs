using Application.Dtos;
using Application.exceptions;
using Application.mediator.interfaces;
using AutoMapper;
using Infrastructure.Repositories;

namespace Application.handlers.category.command.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, CategoryLookupDto>
{
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;
    
    public DeleteCategoryCommandHandler(ICategoryRepository repository, IMapper mapper)  {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<CategoryLookupDto> Handle(DeleteCategoryCommand command)
    {
        var category = await _repository.GetByIdAsync(command.CategoryId);

        if (category is null)
        {
            throw new NotFoundException($"Category with id {command.CategoryId} not found");
        }

        if (!category.IsCustom || category.CreatedBy != command.UserId)
        {
            throw new CategoryCantBeDeletedException($"Category with id {category.Id} is not custom or created by other user");
        }

        await _repository.DeleteAsync(category);

        return _mapper.Map<CategoryLookupDto>(category);
    }
}