using System.ComponentModel.DataAnnotations;

namespace TeachBoard.FileService.Models;

public class UploadHomeworkSolutionRequestModel
{
    [Required] public int StudentId { get; set; }
    [Required] public int HomeworkId { get; set; }
    [Required] public IFormFile HomeworkFile { get; set; }
}