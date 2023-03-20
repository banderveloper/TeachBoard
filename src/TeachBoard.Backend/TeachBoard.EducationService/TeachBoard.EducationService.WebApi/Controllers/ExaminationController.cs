using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.EducationService.Application.Exceptions;
using TeachBoard.EducationService.Application.Features.Examination;
using TeachBoard.EducationService.Domain.Entities;
using TeachBoard.EducationService.WebApi.ActionResults;
using TeachBoard.EducationService.WebApi.Models.Examination;
using TeachBoard.EducationService.WebApi.Validation;

namespace TeachBoard.EducationService.WebApi.Controllers;

[Route("examinations")]
[ApiController]
[Produces("application/json")]
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
    /// <param name="model">New examination data</param>
    /// <returns>Created examination</returns>
    ///
    /// <response code="200">Success / subject_not_found</response>
    /// <response code="400">invalid_date_time</response>
    /// <response code="422">Invalid model</response>
    [HttpPost]
    [ProducesResponseType(typeof(Examination), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IBadRequestApiException), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<Examination>> CreateExamination([FromBody] CreateExaminationRequestModel model)
    {
        var command = _mapper.Map<CreateExaminationCommand>(model);
        var createdExamination = await _mediator.Send(command);

        return new WebApiResult(createdExamination);
        
        
    }

    /// <summary>
    /// Create/update student examination activity
    /// </summary>
    /// 
    /// <param name="model">Examination activity data</param>
    /// <returns>Created/updated examination student activity</returns>
    ///
    /// <response code="200">Success / examination_not_found</response>
    /// <response code="422">Invalid model</response>
    [HttpPost("student-activity")]
    [ProducesResponseType(typeof(StudentExaminationActivity), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<StudentExaminationActivity>> SetStudentExaminationActivity(
        [FromBody] SetStudentExaminationActivityRequestModel model)
    {
        var command = _mapper.Map<SetStudentExaminationActivityCommand>(model);
        var studentExaminationActivity = await _mediator.Send(command);

        return new WebApiResult(studentExaminationActivity);
    }
    
    /// <summary>
    /// Get student's examination activities presentation models
    /// </summary>
    /// 
    /// <param name="studentId">Student id</param>
    /// <returns>List of public examination activities data</returns>
    ///
    /// <response code="200">Success</response>
    [HttpGet("student-activities/{studentId:int}")]
    [ProducesResponseType(typeof(IList<StudentExaminationActivityPresentationDataModel>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IList<StudentExaminationActivityPresentationDataModel>>> GetStudentExaminationActivities(int studentId)
    {
        var query = new GetStudentExaminationsActivityPresentationDataModelQuery { StudentId = studentId };
        var activities = await _mediator.Send(query);

        return new WebApiResult(activities);
    }
}