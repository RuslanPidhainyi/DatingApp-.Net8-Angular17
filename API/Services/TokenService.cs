using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config, UserManager<AppUser> userManager) : ITokenService
{
   public async Task<string> CreateToken(AppUser user)
   {  
      //Nasz klucz ktory jest wlasciwoscio "TokenKey"
      var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access tokenKey from appsettings"); //?? - jezeli null to zrob cos (w moim wypadku mam Exception)
      if(tokenKey.Length < 64) throw new Exception("Your tokenKey needs to be longer");

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));//Teraz nasz token będzie zawierał informacje o użytkowniku

      /*Istnieje 2 rodzaje klucza: Symetryczny i Asymetryczny.
      
      SymmetricSecurityKey - symetryczny klucz bezpiczenstwa  szyfrowania i odszyfrowania informacji. 
      
      AsymetrycznySecurityKey - asymetryczny klucz  to proces z ktorym mamy więc jeden klucz do szyfrowania i drugi do odszyfrowania, ale w naszym przypadku wybieramy jeden klucz, aby rządzić nimi wszystkimi oraz szyfrować  i odszyfrować. ale kiedy używamy tego systemu, musimy upewnić się, że klucz jest bezpiecznie przechowywany na naszym serwerze i nikt nie może uzyskać do niego dostęp i nikt nie może go po prostu zobaczyć*/

       if (user.UserName == null) throw new Exception("No username for user");

      //claims - Twierdzenie o userze - czymś co user moze powiedzić o siebie. (np. Mogą powiedzić, że moja data urodzenia jest taka. Twierdzenie, że mam na imię Bob).

      //To jest standartna definicja claims/Twierdzenie o userze ich wewnętrz tokena
      var claims = new List<Claim>
      {
         new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
         new Claim(ClaimTypes.Name, user.UserName)
      };

      var roles = await userManager.GetRolesAsync(user);
      
      claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);//Haszowania 512 algorytm ktory sluzy do podpisywania naszego klucza -  part signature/ czesc Podpis klucza

      //opis tokena
      var tokenDescriptor = new SecurityTokenDescriptor
      {
         Subject = new ClaimsIdentity(claims), //Nasze nowy padmioty/temy zotsną zrownane z nową tozsamoscią rozszezen/claims, nastepnie przekaze nasze rozszezen/claims jako parametry
         Expires = DateTime.UtcNow.AddDays(7), //Data wygaszania 
         SigningCredentials = creds //Signature/Podpis
      };

      //Obsugę tokenów 
      var tokenHandler = new JwtSecurityTokenHandler();// sluzy do zapisaniu naszego tokena 
      var token = tokenHandler.CreateToken(tokenDescriptor);//tworzymy nasz token i przekazujemy mu tokenDescriptor/ Opis tokena

      return tokenHandler.WriteToken(token); //zapisujemy token w odpowieDZ i zwracamy token 
   }
}