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
 

var app = builder.Build(); 


// 2  А потім мами app аби сконфігуровач Поток запитів HTTP
// Configure the HTTP request pipeline.
app.MapControllers();

app.Run();

/////////////////////////////////////////////////////////////////////////////////////////////////////

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


