using API.Extensions;

namespace API.Entities;

public class AppUser
{
   public int Id { get; set; }
   public required string UserName { get; set; } //required nie mozemy utworzyc usera, bez podanej jego nazwy 
   public byte[] PasswordHash { get; set; } = [];
   public byte[] PasswordSalt { get; set; } = [];//passSalt - zmieni nam nasz passHash, zrobione dlatego jezli user wpisze słabe chaslo
   public DateOnly DateOfBirth { get; set; }
   public required string KnownAs { get; set; }

   //Format Daty "UTC" SQLite nie rozumiem! Rozumie np: PostqruesSQL
   public DateTime Created { get; set; } = DateTime.UtcNow;
   public DateTime LastActive { get; set; } = DateTime.UtcNow;
   public required string Gender { get; set; }
   public string? Introduction { get; set; }
   public string? Interests { get; set; }
   public string? LookingFor { get; set; }
   public required string City { get; set; }
   public required string Country { get; set; }
   public List<Photo> Photos { get; set; } = [];

   public int GetAge()
   {
      return DateOfBirth.CalculateAge();
   }

}
