namespace API.Entities;

public class AppUser
{
   public int Id {get; set;}
   public required string UserName { get; set; } //required nie mozemy utworzyc usera, bez podanej jego nazwy 
}
