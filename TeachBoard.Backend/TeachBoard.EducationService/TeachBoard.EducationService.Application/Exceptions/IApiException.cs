﻿namespace TeachBoard.EducationService.Application.Exceptions;

public interface IApiException
{
    public string? Error { get; set; }
    public string? ErrorDescription { get; set; }
    public string? ReasonField { get; set; }
}