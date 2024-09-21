using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{


   [Required]
   public string Username { get; set; } = string.Empty; //Ініціалізація string.Empty: Значення цієї властивості ініціалізується порожнім рядком (string.Empty). Це означає, що за замовчуванням властивість Username буде мати порожній рядок як початкове значення.
   [Required]
   public string? KnownAs { get; set; }
   [Required]
   public string? Gender { get; set; }
   [Required]
   public string? DateOfBirth { get; set; }
   [Required]
   public string? City { get; set; }
   [Required]
   public string? Country { get; set; }
   [Required]
   [StringLength(8, MinimumLength = 4)]
   public string Password { get; set; } = string.Empty;
}
