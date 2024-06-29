//Ми можемо поділити цей плік на 2 секції - (Послуги)

using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// 1 Реєструєми любу послугу, яку потребує наша Аплікація 
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt => 
{
   opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
//Linijka kta odpowiada za metode CORS
builder.Services.AddCors();
 

var app = builder.Build(); 

app.UseCors(x => x.AllowAnyHeader()
.AllowAnyMethod()
.WithOrigins("http://localhost:4200", "https://localhost:4200"));
/*
Mowimy ze naszej polityce chemy zezwolic na dowolny naglowek i dowolny metod, więc pobierz, opublikuj, umieść, usuń z których żródeł chcemy zezwolić na te ządanie. 

WithOrigins("http://localhost:4200") - mozemy Okreslic naszą aplikacje ktora odpowiada za klienta. 
Wkrotce jednak dodamy mozliwosc uruchamienia naszej app Angulara przez Https(Na tej podstawie mozemy dodac wiele żrodeł w tem samym czasie.)

.WithOrigins("http://localhost:4200", "https://localhost:4200")) - Cors, mowi ze te pochodzenia dla tych dwoch URL jest dozwolony
*/


// 2  А потім мами app аби сконфігуровач Поток запитів HTTP
// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();

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


