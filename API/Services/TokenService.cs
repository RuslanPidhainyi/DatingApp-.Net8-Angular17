using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
   public string CreateToken(AppUser user)
   {
      var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access tokenKey from appsettings"); //?? - jezeli null to zrob cos
      if(tokenKey.Length < 64) throw new Exception("Your tokenKey needs to be longer");

      var key = new SymmetricSecurityKey //Istnieje 2 rodzaje klucza: Symetryczny i Asymetryczny.SymmetricSecurityKey - symetryczny klucz bezpiczenstwa  szyfrowania i odszyfrowania informacji. Asymetryczny to proces z ktrym 
   }
}
