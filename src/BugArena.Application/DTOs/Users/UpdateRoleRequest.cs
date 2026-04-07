using System.ComponentModel.DataAnnotations;

namespace BugArena.Application.DTOs.Users;

public record UpdateRoleRequest(
    [Required]
    [RegularExpression("^(Admin|User)$", ErrorMessage = "Role must be either Admin or User.")]
    string Role
);