using It4see.Domain;
using It4see.Persistence.Base;

using MediatR;

namespace It4see.Application.Categories;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Category>
{
    private readonly ICategoryRepository categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        this.categoryRepository = categoryRepository;
    }

    public async Task<Category> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Id = request.Id,
            Title = request.Title
        };

        return await categoryRepository.UpdateAsync(category);
    }
}