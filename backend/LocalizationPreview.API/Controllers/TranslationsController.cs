using LocalizationPreview.API.Features.Translations.CreateTranslation;
using LocalizationPreview.API.Features.Translations.GetListTranslations;
using LocalizationPreview.API.Features.Translations.GetTranslation;
using LocalizationPreview.API.Features.Translations.GetTranslationById;
using LocalizationPreview.API.Features.Translations.UpdateTranslation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LocalizationPreview.API.Controllers;

[Route("api/translations")]
[ApiController]
public class TranslationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TranslationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTranslation([FromBody] CreateTranslationCommand request)
    {
        var result = await _mediator.Send(request);
        return new JsonResult(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateTranslation([FromBody] UpdateTranslationCommand request)
    {
        var result = await _mediator.Send(request);
        return new JsonResult(result);
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById([FromRoute] long id)
    {
        var request = new GetTranslationByIdQuery() { Id = id };
        var viewModel = await _mediator.Send(request);
        if (viewModel == null)
        {
            return BadRequest("Translation not found.");
        }

        return new JsonResult(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetTranslationQuery request)
    {
        var viewModel = await _mediator.Send(request);
        if (viewModel == null)
        {
            return BadRequest("Translation not found.");
        }

        return new JsonResult(viewModel);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetList([FromQuery] GetListTranslationsQuery request)
    {
        return new JsonResult(await _mediator.Send(request));
    }
}