﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeachBoard.Gateway.Application;
using TeachBoard.Gateway.Application.Exceptions;
using TeachBoard.Gateway.Application.Refit.Clients;
using TeachBoard.Gateway.Application.Refit.RequestModels.Education;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Education;
using TeachBoard.Gateway.WebApi.ActionResults;
using TeachBoard.Gateway.WebApi.Models;

namespace TeachBoard.Gateway.WebApi.Controllers;

[Route("api/teacher")]
[Authorize(Roles = "Teacher")]
public class TeacherController : BaseController
{
    private readonly IIdentityClient _identityClient;
    private readonly IMembersClient _membersClient;
    private readonly IEducationClient _educationClient;

    private readonly ILogger<TeacherController> _logger;

    public TeacherController(IIdentityClient identityClient, IMembersClient membersClient, IEducationClient educationClient, ILogger<TeacherController> logger)
    {
        _identityClient = identityClient;
        _membersClient = membersClient;
        _educationClient = educationClient;
        _logger = logger;
    }

    /// <summary>
    /// Check completed student's homeworks
    /// </summary>
    /// 
    /// <param name="model">Completed homework id, grade and comment</param>
    ///
    /// <response code="200">
    /// Success / teacher_not_found / completed_homework_not_found / completed_homework_invalid_teacher
    /// </response>
    /// <response code="422">Invalid model state</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpPost("check-homework")]
    public async Task<ActionResult<CompletedHomework>> CheckHomework([FromBody] CheckHomeworkRequestModel model)
    {
        var teacherResponse = await _membersClient.GetTeacherByUserId(UserId);
        var teacher = teacherResponse.Data;

        if (teacher is null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.TeacherNotFound,
                PublicErrorMessage = "Teacher data bound to your user not found",
                LogErrorMessage = $"CheckHomework at teacher controller error. No teacher found by user id [{UserId}]"
            };

        var internalRequest = new CheckHomeworkInternalRequestModel
        {
            TeacherId = teacher.Id,
            CompletedHomeworkId = model.CompletedHomeworkId,
            Grade = model.Grade,
            Comment = model.Comment
        };
        var checkHomeworkResponse = await _educationClient.CheckHomework(internalRequest);
        var checkedCompletedHomework = checkHomeworkResponse.Data;

        return new WebApiResult(checkedCompletedHomework);
    }
}