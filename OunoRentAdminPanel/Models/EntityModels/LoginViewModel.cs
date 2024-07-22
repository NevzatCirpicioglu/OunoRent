using System.ComponentModel.DataAnnotations;

namespace OunoRentAdminPanel.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Şifre alanı boş bırakılamaz")]
    public required string Password { get; set; }
}
