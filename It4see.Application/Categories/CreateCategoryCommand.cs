using It4see.Domain;
using MediatR;

namespace It4see.Application.Categories;

public class CreateCategoryCommand : IRequest<Category>
{
    public string Title { get; set; }
}
