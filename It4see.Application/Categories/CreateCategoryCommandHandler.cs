using It4see.Domain;
using It4see.Persistence.Base;

using MediatR;

namespace It4see.Application.Categories;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Category>
{
    private readonly ICategoryRepository categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        this.categoryRepository = categoryRepository;
    }

    public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Title = request.Title
        };

        return await categoryRepository.AddAsync(category);
    }
}
