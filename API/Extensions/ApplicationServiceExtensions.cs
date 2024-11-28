﻿using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using API.SignalR;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ApplicationServiceExtensions
{
   public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
   {  
      //Реєстрація контролерів: - Додає служби, необхідні для роботи контролерів MVC. Це потрібно для обробки HTTP-запитів у вашому API.
      services.AddControllers();

      /*
      Реєстрація контексту бази даних: 
      Додає DataContext як службу до контейнера впровадження залежностей.
      UseSqlite — вказує, що використовуватиметься база даних SQLite, і підключається до неї за допомогою рядка підключення, який витягується з конфігураційного файлу (appsettings.json) під ключем "DefaultConnection".  
      */
      services.AddDbContext<DataContext>(opt =>
      {
         opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
      });


      services.AddCors();//Додає конфігурацію CORS (Cross-Origin Resource Sharing). Це дозволяє вашому API приймати запити з інших доменів (наприклад, з фронтенду на Angular або React).


      
      
      //Реєструє сервіси з типом Scoped. Це означає, що для кожного HTTP-запиту створюється новий екземпляр цих сервісів:

      //ITokenService, TokenService — інтерфейс ITokenService і його реалізація TokenService, яка, ймовірно, відповідає за генерацію та валідацію JWT-токенів.
      services.AddScoped<ITokenService, TokenService>();

      //IUserRepository, UserRepository — інтерфейс IUserRepository і його реалізація UserRepository, яка використовується для роботи з даними користувачів.
      services.AddScoped<IUserRepository, UserRepository>();

      services.AddScoped<ILikesRepository, LikesRepository>();

      services.AddScoped<IMessageRepository, MessageRepository>();

      services.AddScoped<IUnitOfWork, UnitOfWork>();

      services.AddScoped<IPhotoService, PhotoService>();

      services.AddScoped<LogUserActivity>();

     //Додає AutoMapper до контейнера служб і автоматично завантажує всі конфігурації маппінгу, що визначені в поточному додатку. Це дозволяє легко використовувати AutoMapper для перетворення об'єктів між різними типами.
      services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); //Zarejestrowalismy AutoMapper

      services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

      services.AddSignalR();

      services.AddSingleton<PresenceTracker>();

      return services;//Метод повертає колекцію служб після того, як всі необхідні служби були додані до контейнера.
   }
}
