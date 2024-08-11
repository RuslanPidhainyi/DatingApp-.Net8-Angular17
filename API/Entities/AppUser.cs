namespace API.Entities;

public class AppUser
{
   public int Id {get; set;}
   public required string UserName { get; set; } //required nie mozemy utworzyc usera, bez podanej jego nazwy 
   public required byte[] PasswordHash {get; set;}
   public required byte[] PasswordSalt {get; set;}//passSalt - zmieni nam nasz passHash, zrobione dlatego jezli user wpisze słabe chaslo
}
