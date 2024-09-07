//Ми можемо поділити цей плік на 2 секції - (Послуги)
using System.Text;
using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Middleware;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
 
// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentetiService(builder.Configuration);

var app = builder.Build(); 

//app.UseDeveloperExceptionPage(); //To jest dla trybu developerskim

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(x => x.AllowAnyHeader()
.AllowAnyMethod()
.WithOrigins("http://localhost:4200", "https://localhost:4200"));
/*
Mowimy ze naszej polityce chemy zezwolic na dowolny naglowek i dowolny metod, więc pobierz, opublikuj, umieść, usuń z których żródeł chcemy zezwolić na te ządanie. 

WithOrigins("http://localhost:4200") - mozemy Okreslic naszą aplikacje ktora odpowiada za klienta. 
Wkrotce jednak dodamy mozliwosc uruchamienia naszej app Angulara przez Https(Na tej podstawie mozemy dodac wiele żrodeł w tem samym czasie.)

.WithOrigins("http://localhost:4200", "https://localhost:4200")) - Cors, mowi ze te pochodzenia dla tych dwoch URL jest dozwolony
*/

app.UseAuthentication();
app.UseAuthorization();

// 2  А потім мами app аби сконфігуровач Поток запитів HTTP
// Configure the HTTP request pipeline.
app.MapControllers();

//Wzorzec "Lokator usług" - aby uzyskać dostęp do usługi której chcemy użyć poza wstzrykiwaniem zaleznosci
using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}   
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();


// //Ми можемо поділити цей плік на 2 секції - (Послуги)

// using System.Text;
// using API.Data;
// using API.Interfaces;
// using API.Services;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;

// var builder = WebApplication.CreateBuilder(args);


// // 1 Реєструєми любу послугу, яку потребує наша Аплікація 
// // Add services to the container.
// builder.Services.AddControllers();
// builder.Services.AddDbContext<DataContext>(opt => 
// {
//    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
// });
// //Linijka kta odpowiada za metode CORS
// builder.Services.AddCors();

// //Okreslamy czas życia usługi:

// //builder.Services.AddSingleton()
// //builder.Services.AddTransient() //AddTransient - Transient w czasie zycia, ktore są tworzone za kazdym razem, gdy są wymagane z kontenera usług. I to działa dla lekkich, bezstanowych usług. (ten jest zwykle uwazany za zbyt krotki)  
// builder.Services.AddScoped<ITokenService, TokenService>(); //Uslugę te są tworzone raz na żądanie klienta, a kiedy mówimy o ządaniach, tak naprawdę mówimy tutaj o żądaniu HTTP.Jezeli User loguje się bedzie to liczone jako żądanie. Tak więc dla naszej usługi tokena, poniewaz chcemy, aby wygenerowała token, gdy to ządanie zostanie odebrane przez nasz kontroler API i wewnetrz nasszego kontrolera konta, kiedy uzyjemy wstrzykniecie zaleznosci, bedziemy wstrzykiwać tutaj naszą usługę tokena. Tak więc żadanie przychodzi do punktu końcowego rejestracji lub logowania Zostaje utworzona nasza nowa usluga. 

// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options => {
//       var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("TokenKey not found"); 
//       //odszyfrowania tokena
//       options.TokenValidationParameters = new TokenValidationParameters
//       {
//          ValidateIssuerSigningKey = true,
//          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
//          ValidateIssuer = false,
//          ValidateAudience = false
//       }; 
//    });

// var app = builder.Build(); 

// app.UseCors(x => x.AllowAnyHeader()
// .AllowAnyMethod()
// .WithOrigins("http://localhost:4200", "https://localhost:4200"));
// /*
// Mowimy ze naszej polityce chemy zezwolic na dowolny naglowek i dowolny metod, więc pobierz, opublikuj, umieść, usuń z których żródeł chcemy zezwolić na te ządanie. 

// WithOrigins("http://localhost:4200") - mozemy Okreslic naszą aplikacje ktora odpowiada za klienta. 
// Wkrotce jednak dodamy mozliwosc uruchamienia naszej app Angulara przez Https(Na tej podstawie mozemy dodac wiele żrodeł w tem samym czasie.)

// .WithOrigins("http://localhost:4200", "https://localhost:4200")) - Cors, mowi ze te pochodzenia dla tych dwoch URL jest dozwolony
// */

// app.UseAuthentication();
// app.UseAuthorization();

// // 2  А потім мами app аби сконфігуровач Поток запитів HTTP
// // Configure the HTTP request pipeline.
// app.MapControllers();

// app.Run();

/////////////////////////////////////////////////////////////////////////////////////////////////////
         /***Tak był ten program file na poczętku - i jest to tłumaczone czym jest co.***/


// //Ми можемо поділити цей плік на 2 секції - (Послуги)

// var builder = WebApplication.CreateBuilder(args);


// // 1 Реєструєми любу послугу, яку потребує наша Аплікація 
// // Add services to the container.

// builder.Services.AddControllers(); //Usluga do stworzenia kontrolera i zarejestrowania je, abyśmy mogli korzystac z punktow koncowyc API
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();


// var app = builder.Build(); //Створюється наша аплікація


// // 2  А потім мами app аби сконфігуровач Поток запитів HTTP
// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

// app.UseHttpsRedirection();

// app.UseAuthorization();

// app.MapControllers(); //Opragromowania posredniczące do mapowania punktów końcowych kontrolera

// app.Run();//Запускає нашу программу