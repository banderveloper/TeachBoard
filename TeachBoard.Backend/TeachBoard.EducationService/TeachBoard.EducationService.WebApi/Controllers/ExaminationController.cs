using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Features.Examination;
using TeachBoard.EducationService.Domain.Entities;
using TeachBoard.EducationService.WebApi.Models.Examination;
using TeachBoard.EducationService.WebApi.Models.Validation;

namespace TeachBoard.EducationService.WebApi.Controllers;

[Route("examinations")]
[ApiController]
[Produces("application/json")]
[ValidateModel]
public class ExaminationController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public ExaminationController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    /// <summary>
    /// Create new examination
    /// </summary>
    /// 
    /// <param name="model">Create examination model</param>
    /// <returns>Created examination</returns>
    ///
    /// <response code="200">Success. Examination created and returned</response>
    /// <response code="400">Examination end time cannot be later than start (invalid_datetime)</response>
    /// <response code="404">Subject with given id not found (subject_not_found)</response>
    /// <response code="422">Invalid model</response>
    [HttpPost("create")]
    [ProducesResponseType(typeof(Examination), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<Examination>> CreateExamination([FromBody] CreateExaminationRequestModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);

        var command = _mapper.Map<CreateExaminationCommand>(model);
        var createdExamination = await _mediator.Send(command);

        return createdExamination;
    }

    /// <summary>
    /// Create/update student examination activity
    /// </summary>
    /// 
    /// <param name="model">Create/update student examination activity model</param>
    /// <returns>Created/updated examination student activity</returns>
    ///
    /// <response code="200">Success. Student examination activity created/updated and returned</response>
    /// <response code="404">Student examination activity with given id not found (examination_not_found)</response>
    /// <response code="422">Invalid model</response>
    [HttpPost("setStudentActivity")]
    [ProducesResponseType(typeof(StudentExaminationActivity), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<StudentExaminationActivity>> SetStudentExaminationActivity(
        [FromBody] SetStudentExaminationActivityRequestModel model)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(model);

        var command = _mapper.Map<SetStudentExaminationActivityCommand>(model);
        var studentExaminationActivity = await _mediator.Send(command);

        return studentExaminationActivity;
    }
    
    /// <summary>
    /// Get student examination activities public data
    /// </summary>
    /// 
    /// <param name="studentId">Student id</param>
    /// <returns>List of public examination activities data</returns>
    ///
    /// <response code="200">Success. List of Student examination activities returned</response>
    /// <response code="404">Student examination activies with given id student id not found (student_examination_activities_not_found)</response>
    /// <response code="422">Invalid model</response>
    [HttpGet("getStudentActivities/{studentId:int}")]
    [ProducesResponseType(typeof(StudentExaminationsPublicDataListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<StudentExaminationsPublicDataListModel>> GetStudentExaminationActivities(int studentId)
    {
        var query = new GetStudentExaminationsActivitiesQuery { StudentId = studentId };
        var model = await _mediator.Send(query);

        return model;
    }
}