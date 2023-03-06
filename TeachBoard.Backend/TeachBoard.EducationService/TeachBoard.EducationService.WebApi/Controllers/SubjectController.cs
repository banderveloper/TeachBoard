using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Features.Subject;
using TeachBoard.EducationService.Domain.Entities;
using TeachBoard.EducationService.WebApi.ActionResults;
using TeachBoard.EducationService.WebApi.Models.Subject;
using TeachBoard.EducationService.WebApi.Validation;

namespace TeachBoard.EducationService.WebApi.Controllers;

[ApiController]
[Route("subjects")]
[Produces("application/json")]
public class SubjectController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SubjectController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Create new subject
    /// </summary>
    /// 
    /// <param name="model">Create subject model</param>
    /// <returns>Created subject</returns>
    ///
    /// <response code="200">Success. Subject created and returned</response>
    /// <response code="409">Subject with given name already exists (subject_already_exists)</response>
    /// <response code="422">Invalid model</response>
    [HttpPost]
    [ProducesResponseType(typeof(Subject), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<Subject>> CreateSubject([FromBody] CreateSubjectRequestModel model)
    {
        var command = _mapper.Map<CreateSubjectCommand>(model);

        var createdSubject = await _mediator.Send(command);
        return new WebApiResult(createdSubject);
    }

    /// <summary>
    /// Get subject by id
    /// </summary>
    /// 
    /// <param name="id">Subject id</param>
    /// <returns>Subject with given id</returns>
    ///
    /// <response code="200">Success. Subject returned</response>
    /// <response code="404">Subject with given id not found (subject_not_found)</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Subject), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Subject>> GetSubjectById(int id)
    {
        var query = new GetSubjectByIdQuery { SubjectId = id };
        var subject = await _mediator.Send(query);

        return new WebApiResult(subject);
    }

    /// <summary>
    /// Get all subjects
    /// </summary>
    /// 
    /// <returns>All subjects list</returns>
    ///
    /// <response code="200">Success. Subjects returned</response>
    /// <response code="404">No subjects found (subjects_not_found)</response>
    [HttpGet]
    [ProducesResponseType(typeof(IList<Subject>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IList<Subject>>> GetAllSubjects()
    {
        var query = new GetAllSubjectsQuery();
        var subjectsModel = await _mediator.Send(query);

        return new WebApiResult(subjectsModel);
    }

    /// <summary>
    /// Delete subject by id
    /// </summary>
    /// 
    /// <response code="200">Success. Subject deleted</response>
    /// <response code="404">Subject with given id not found (subject_not_found)</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IExpectedApiException), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSubjectById(int id)
    {
        var query = new DeleteSubjectByIdCommand { SubjectId = id };
        await _mediator.Send(query);

        return new WebApiResult();
    }
}