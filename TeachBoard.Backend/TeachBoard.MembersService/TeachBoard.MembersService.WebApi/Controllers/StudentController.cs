﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.MembersService.Application.Exceptions;
using TeachBoard.MembersService.Application.Features.Students;
using TeachBoard.MembersService.Application.Validation;
using TeachBoard.MembersService.Domain.Entities;

namespace TeachBoard.MembersService.WebApi.Controllers;

[ValidateModel]
[ApiController]
[Route("members/students")]
[Produces("application/json")]
public class StudentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public StudentController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Get student by id
    /// </summary>
    /// <param name="id">Student id</param>
    /// <response code="200">Success. Student returns.</response>
    /// <response code="404">Student with given id not found (student_not_found)</response>
    [HttpGet("getbyid/{id:int}")]
    [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Student>> GetStudentById(int id)
    {
        var query = new GetStudentByIdQuery { StudentId = id };
        var student = await _mediator.Send(query);

        return Ok(student);
    }

    /// <summary>
    /// Get students by group id
    /// </summary>
    /// <param name="id">Group id</param>
    /// <response code="200">Success. Array of students returns</response>
    /// <response code="404">Students with given group id not found (students_not_found)</response>
    [HttpGet("getbygroupid/{id:int}")]
    [ProducesResponseType(typeof(StudentsListModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IApiException), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentsListModel>> GetStudentsByGroupId(int id)
    {
        var query = new GetStudentsByGroupIdQuery { GroupId = id };
        var students = await _mediator.Send(query);

        return Ok(students);
    }
}