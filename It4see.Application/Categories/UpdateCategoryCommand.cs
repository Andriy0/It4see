using It4see.Domain;
using MediatR;

namespace It4see.Application.Categories;

public class UpdateCategoryCommand : IRequest<Category>
{
    public int Id { get; set; }

    public string Title { get; set; }
}