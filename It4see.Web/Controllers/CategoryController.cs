using AutoMapper;

using It4see.Application.Categories;
using It4see.Domain;
using It4see.Web.ViewModels.Category;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace It4see.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    public CategoryController(IMediator mediator, IMapper mapper)
    {
        this.mediator = mediator;
        this.mapper = mapper;
    }

    [HttpGet("list")]
    public async Task<IActionResult> Get()
    {
        var categories = await mediator.Send(new GetAllCategoriesQuery());

        var categoryListViewModels = mapper.Map<List<CategoryListViewModel>>(categories);

        return Ok(categoryListViewModels);
    }

    [HttpGet]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await this.mediator.Send(new GetCategoryQuery { Id = id });
        if (category == null)
        {
            return NotFound();
        }

        return Ok(mapper.Map<CategoryDetailsViewModel>(category));
    }

    [HttpGet("byTitle")]
    public async Task<IActionResult> GetByTitle(string title)
    {
        var category = await this.mediator.Send(new GetCategoryByTitleQuery { Title = title });
        if (category == null)
        {
            return NotFound();
        }

        return Ok(mapper.Map<CategoryDetailsViewModel>(category));
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateCategoryViewModel categoryDto)
    {
        var createCategoryCommand = new CreateCategoryCommand
        {
            Title = categoryDto.Title
        };

        var createdCategory = await mediator.Send(createCategoryCommand);

        return Ok(mapper.Map<CategoryDetailsViewModel>(createdCategory));
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateCategoryViewModel categoryDto)
    {
        var updateCategoryCommand = new UpdateCategoryCommand
        {
            Id = categoryDto.Id,
            Title = categoryDto.Title
        };

        var category = await mediator.Send(updateCategoryCommand);

        return Ok(mapper.Map<CategoryDetailsViewModel>(category));
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        await mediator.Send(new DeleteCategoryCommand { Id = id });

        return NoContent();
    }
}
