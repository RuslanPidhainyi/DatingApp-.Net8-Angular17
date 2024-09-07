using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(DataContext context)
    {
        if (await context.Users.AnyAsync()) return;  //перевіряє, чи є вже користувачі в базі даних. Якщо є, метод виходить.

        var useData = await File.ReadAllTextAsync("Data/UserSeedData.json"); //Зчитування  початкових даних із файлу, здійснюється асинхронно за допомогою методу File.ReadAllTextAsync, що підвищує продуктивність програми.

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var users = JsonSerializer.Deserialize<List<AppUser>>(useData, options);

        if (users == null) return;

        foreach (var user in users)
        {
            using var hmac = new HMACSHA512();

            user.UserName = user.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$word"));
            user.PasswordSalt = hmac.Key;

            context.Users.Add(user);
        }

        /*
        Ключове слово using тут використовується для автоматичного управління ресурсами. Це означає, що об'єкт, створений за допомогою using, буде автоматично звільнений після завершення його використання.

        У даному випадку hmac — це екземпляр класу HMACSHA512, який реалізує інтерфейс IDisposable. Об'єкти, що реалізують IDisposable, вимагають виклику методу Dispose() після завершення їх використання для звільнення ресурсів, які вони займають (наприклад, пам'ять або системні ресурси).
        */

        await context.SaveChangesAsync();
    }
}
