﻿using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Refit;
using TeachBoard.Gateway.Application;
using TeachBoard.Gateway.Application.Exceptions;
using TeachBoard.Gateway.Application.Refit.Clients;
using TeachBoard.Gateway.Application.Refit.RequestModels.Education;
using TeachBoard.Gateway.Application.Refit.RequestModels.Identity;
using TeachBoard.Gateway.Application.Refit.RequestModels.Members;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Education;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Identity;
using TeachBoard.Gateway.Application.Refit.ResponseModels.Members;
using TeachBoard.Gateway.Application.Validation;
using TeachBoard.Gateway.WebApi.ActionResults;
using TeachBoard.Gateway.WebApi.Models;

namespace TeachBoard.Gateway.WebApi.Controllers;

[Route("api/administrator")]
// [Authorize(Roles = "Administrator")]
public class AdministratorController : BaseController
{
    private readonly IIdentityClient _identityClient;
    private readonly IMembersClient _membersClient;
    private readonly IEducationClient _educationClient;

    public AdministratorController(IIdentityClient identityClient, IMembersClient membersClient,
        IEducationClient educationClient)
    {
        _identityClient = identityClient;
        _membersClient = membersClient;
        _educationClient = educationClient;
    }

    /// <summary>
    /// Create pending user
    /// </summary>
    /// 
    /// <param name="model">New pending user data</param>
    ///
    /// <response code="200">Success / set_role_forbidden / email_already_exists / phoneNumber_already_exists</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="422">Invalid model</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpPost("pending-user")]
    [ProducesResponseType(typeof(RegisterCodeModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<RegisterCodeModel>> CreatePendingUser(
        [FromBody] CreatePendingUserRequestModel model)
    {
        // If role not student or teacher, error. Admin can create only teachers and 
        if (model.Role != UserRole.Student && model.Role != UserRole.Teacher)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.SetRoleForbidden,
                PublicErrorMessage = "Administrator can set only 'Student' and 'Teacher' role to pending users",
                ReasonField = "role"
            };

        var identityResponse = await _identityClient.CreatePendingUser(model);
        var registerCodeModel = identityResponse.Data;

        return new WebApiResult(registerCodeModel);
    }

    /// <summary>
    /// Set student group
    /// </summary>
    /// 
    /// <param name="model">Student id and group id</param>
    ///
    /// <response code="200">Success / student_not_found / group_not_found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="422">Invalid model</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpPut("student-group")]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> SetStudentGroup([FromBody] SetStudentGroupRequestModel model)
    {
        await _membersClient.SetStudentGroup(model);
        return new WebApiResult();
    }

    /// <summary>
    /// Create lesson
    /// </summary>
    /// 
    /// <param name="model">Group id, subject id, teacher id, scheduled time</param>
    ///
    /// <response code="200">Success / teacher_not_found / subject_not_found / group_not_found</response>
    /// <response code="400">invalid_datetime</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="422">Invalid model</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpPost("lesson")]
    [ProducesResponseType(typeof(Teacher), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IBadRequestApiException), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<Lesson>> CreateLesson([FromBody] CreateLessonAsAdministratorRequestModel model)
    {
        // get teacher by id and throw ex if it is not found
        var getTeacherResponse = await _membersClient.GetTeacherById(model.TeacherId);
        var teacher = getTeacherResponse.Data;
        if (teacher is null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.TeacherNotFound,
                PublicErrorMessage = "Teacher not found",
                LogErrorMessage = $"Teacher with id [{model.TeacherId}] not found"
            };

        // get group by id and throw ex if it is not found
        var getGroupResponse = await _membersClient.GetGroupById(model.GroupId);
        var group = getGroupResponse.Data;
        if (group is null)
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.GroupNotFound,
                PublicErrorMessage = "Group not found",
                LogErrorMessage = $"Group with id [{model.GroupId}] not found"
            };

        var createLessonResponse = await _educationClient.CreateLessonAsAdministrator(model);
        var createdLesson = createLessonResponse.Data;

        return new WebApiResult(createdLesson);
    }

    /// <summary>
    /// Get all users with given name (or part)
    /// </summary>
    /// 
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("users-presentations/{partialName}")]
    [ProducesResponseType(typeof(IList<UserPresentationDataModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<IList<UserPresentationDataModel>>> GetUsersByPartialName(string partialName)
    {
        var response = await _identityClient.GetUserPresentationDataModelsByPartialName(partialName);
        var users = response.Data;

        return new WebApiResult(users);
    }

    /// <summary>
    /// Get user's full data as member (user data + special data (student/teacher data)
    /// </summary>
    /// <response code="200">Success</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpGet("memberData/{userId:int}")]
    [ProducesResponseType(typeof(MemberFullDataResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<MemberFullDataResponseModel>> GetFullMemberData(int userId)
    {
        var userPublicDataResponse = await _identityClient.GetUserPublicData(userId);
        var user = userPublicDataResponse.Data;

        // if user by id does not exists client will return null - return empty result to client
        if (user is null)
            return new WebApiResult();

        // admin can check only students and teachers, so throw ex if user is not student or teacher 
        if (user.Role != UserRole.Student && user.Role != UserRole.Teacher)
        {
            throw new ExpectedApiException
            {
                ErrorCode = ErrorCode.GetUserInfoForbidden,
                PublicErrorMessage = "Administrator can get info only about students and teachers",
                LogErrorMessage =
                    $"GetFullMemberData in controller error. Admin is allowed to check students and teachers, but requested user is {user.Role}"
            };
        }

        // get special data (if student - student data, if teacher - teacher data)
        object? memberData = null;

        switch (user.Role)
        {
            case UserRole.Student:
                var studentPresentationResponse = await _membersClient.GetStudentPresentation(user.Id);
                memberData = studentPresentationResponse.Data;
                break;

            case UserRole.Teacher:
                var getTeacherResponse = await _membersClient.GetTeacherByUserId(user.Id);
                memberData = getTeacherResponse.Data;
                break;

            case UserRole.Unspecified:
            case UserRole.Administrator:
            case UserRole.Director:
            default:
                throw new ExpectedApiException
                {
                    ErrorCode = ErrorCode.UnexpectedRole,
                    PublicErrorMessage = "Got user has unexpected role, operation cancelled",
                    LogErrorMessage = $"GetFullMemberData in controller error. Requested user has {user.Role} role"
                };
        }

        return new WebApiResult(
            new MemberFullDataResponseModel
            {
                User = user,
                Member = memberData
            }
        );
    }

    /// <summary>
    /// Update user public data
    /// </summary>
    /// <response code="200">Success / user_not_found</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="422">Invalid model</response>
    /// <response code="503">One of the needed services is unavailable now</response>
    [HttpPut("user-public")]
    [ProducesResponseType(typeof(UserPublicData), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ValidationResultModel), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(void), StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<UserPublicData>> UpdateUserPublicData([FromBody] UpdateUserRequestModel model)
    {
        var updateResponse = await _identityClient.UpdateUser(model);
        var userPublicData = updateResponse.Data;

        return new WebApiResult(userPublicData);
    }
}