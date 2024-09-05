using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDto
{
   [Required]
   public string Username { get; set; } = string.Empty; //Ініціалізація string.Empty: Значення цієї властивості ініціалізується порожнім рядком (string.Empty). Це означає, що за замовчуванням властивість Username буде мати порожній рядок як початкове значення.

   [Required]
   [StringLength(8, MinimumLength = 4)]
   public string Password { get; set; } = string.Empty;
}
