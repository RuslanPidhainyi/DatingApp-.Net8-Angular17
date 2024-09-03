using System;
using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
{
    /* RequestDelegate next - представляє наступний middleware в конвеєрі обробки запитів. Якщо в даному middleware не виникне винятків, управління буде передано наступному middleware.
    
    делегат, який представляє наступний middleware в конвеєрі обробки HTTP-запитів. Коли InvokeAsync викликається, цей делегат використовується для виклику наступного компонента в конвеєрі.
    */



    /* ILogger<ExceptionMiddleware> logger - об'єкт для логування, який використовується для запису помилок. 

    об'єкт для логування, який використовується для запису інформації про винятки або інші важливі події, що відбуваються в додатку
    */


    /* IHostEnvironment env - об'єкт, який надає інформацію про середовище виконання додатку (Production, Development) 
    
    об'єкт, який надає інформацію про середовище виконання додатку (наприклад, Development, Production тощо). Використовується, щоб визначити, чи є додаток в режимі розробки або у виробничому середовищі.
    */


    //func: InvokeAsync — це асинхронний метод, який обробляє вхідні HTTP-запити. Він виконує свою роботу наступним чином
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            /*
                Виконує виклик наступного middleware в конвеєрі через await next(context). Якщо цей виклик проходить успішно, управління переходить до наступного middleware. Якщо виникає помилка (виняток), виконується код в блоці catch.
            */
            await next(context);
        }
        catch (Exception ex)
        {   
            //Якщо виникає виняток, він перехоплюється і обробляється тут.

            logger.LogError(ex, ex.Message); //записує інформацію про виняток у журнал (лог), включаючи саме виключення ex і його повідомлення ex.Message.
            context.Response.ContentType = "application/json"; //встановлює тип вмісту відповіді в JSON, що зручно для клієнтської сторони (наприклад, веб-браузера), щоб зрозуміти формат відповіді.
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; //встановлює статусний код відповіді HTTP на 500 (Internal Server Error), що вказує на помилку сервера.

            var response = env.IsDevelopment() 
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace) //Zwroci nam jeden typ odp, gdy jestesmy w fazie Development
                : new ApiException(context.Response.StatusCode, ex.Message, "Internal server error"); //Zwroci nam mniejszy typ odp, gdy jestesmy w fazie Production
            

            /*
                Налаштовуються параметри серіалізації для JSON (JsonSerializerOptions), щоб використовувати camelCase для назв властивостей (наприклад, statusCode замість StatusCode).
            */
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options); //серіалізує об'єкт response в JSON формат з використанням зазначених опцій.

            await context.Response.WriteAsync(json); //Надсилає серіалізовану JSON відповідь клієнту.
        }
    }
}

/*
Підсумок
Цей ExceptionMiddleware обробляє будь-які винятки, що виникають під час обробки HTTP-запитів, записує їх у журнал і повертає клієнту відповідну відповідь у форматі JSON. Це забезпечує:

    - Уніфіковану обробку винятків на всіх рівнях програми.
    - Відмінності між відповідями в середовищі розробки та виробництва, забезпечуючи належну безпеку інформації та зручність відлагодження.
*/
