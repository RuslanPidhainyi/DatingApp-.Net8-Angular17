using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    // public static async Task SeedUsers(DataContext context)
    // {
    //     if (await context.Users.AnyAsync()) return;

    //     var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

    //     var options = new JsonSerializerOptions{PropertyNameCaseInsensitive = true};

    //     var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

    //     if (users == null) return;

    //     foreach (var user in users)
    //     {
    //         using var hmac = new HMACSHA512();

    //         user.UserName = user.UserName.ToLower();
    //         user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
    //         user.PasswordSalt = hmac.Key;

    //         context.Users.Add(user);
    //     }

    //     await context.SaveChangesAsync();
    // }

    public static async Task SeedUsers(DataContext context)
    {
        Console.WriteLine("Checking if users already exist...");

        if (await context.Users.AnyAsync())
        {
            Console.WriteLine("Users already exist. Skipping seed.");
            return; //перевіряє, чи є вже користувачі в базі даних. Якщо є, метод виходить.
        }

        Console.WriteLine("Reading user data from JSON file...");

        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json"); //Зчитування  початкових даних із файлу, здійснюється асинхронно за допомогою методу File.ReadAllTextAsync, що підвищує продуктивність програми.

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        if (users == null)
        {
            Console.WriteLine("No user data found in JSON file.");
            return;
        }

        Console.WriteLine("Seeding users to database...");

        foreach (var user in users)
        {
            using var hmac = new HMACSHA512();

            // user.UserName = user.UserName.ToLower();
            // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            // user.PasswordSalt = hmac.Key;

            context.Users.Add(user);
            Console.WriteLine($"User {user.UserName} added.");
        }

        /*
        Ключове слово using тут використовується для автоматичного управління ресурсами. Це означає, що об'єкт, створений за допомогою using, буде автоматично звільнений після завершення його використання.
        У даному випадку hmac — це екземпляр класу HMACSHA512, який реалізує інтерфейс IDisposable. Об'єкти, що реалізують IDisposable, вимагають виклику методу Dispose() після завершення їх використання для звільнення ресурсів, які вони займають (наприклад, пам'ять або системні ресурси).
        */

        await context.SaveChangesAsync();
        Console.WriteLine("User seeding completed.");
    }


}