using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ServiceFilter(typeof(LogUserActivity))]
[ApiController]
[Route("api/[controller]")]// api/users - aby uzyskac dostęp do punktow kocnowych w kontrollerze. Urzytkownik biedzie przegladał localhost 5001, a następnie użytkownicy zostaną przekierowany do tego kontrolera i znajdujący się w nim punktów końcowych 
public class BaseApiController : ControllerBase //dzidziczenia bazowego 
{

}
