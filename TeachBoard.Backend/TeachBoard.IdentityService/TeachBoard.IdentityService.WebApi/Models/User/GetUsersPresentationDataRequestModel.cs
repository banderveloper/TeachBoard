using System.ComponentModel.DataAnnotations;

namespace TeachBoard.IdentityService.WebApi.Models.User;

public class GetUsersPresentationDataRequestModel
{
    [Required] public IList<int> Ids { get; set; }
}