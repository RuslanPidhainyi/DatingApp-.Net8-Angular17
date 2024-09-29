using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{   
    //UserManager - daje nam dostep do db
    public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {

        if (await userManager.Users.AnyAsync()) return; //перевіряє, чи є вже користувачі в базі даних. Якщо є, метод виходить.

        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json"); //Зчитування  початкових даних із файлу, здійснюється асинхронно за допомогою методу File.ReadAllTextAsync, що підвищує продуктивність програми.

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        if (users == null) return;

        var roles = new List<AppRole>
        {
            //new AppRole{Name ="Member"}
            new() {Name ="Member"},
            new() {Name ="Admin"},
            new() {Name ="Moderator"},

        };

        foreach(var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        foreach (var user in users)
        {
            //using var hmac = new HMACSHA512();
            // user.UserName = user.UserName.ToLower();
            // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));
            // user.PasswordSalt = hmac.Key;
            //context.Users.Add(user);
            
            user.UserName = user.UserName!.ToLower();
            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Member");
        }

        /*
        Ключове слово using тут використовується для автоматичного управління ресурсами. Це означає, що об'єкт, створений за допомогою using, буде автоматично звільнений після завершення його використання.
        У даному випадку hmac — це екземпляр класу HMACSHA512, який реалізує інтерфейс IDisposable. Об'єкти, що реалізують IDisposable, вимагають виклику методу Dispose() після завершення їх використання для звільнення ресурсів, які вони займають (наприклад, пам'ять або системні ресурси).
        */

        //await context.SaveChangesAsync();

        var admin = new AppUser 
        {
            UserName = "admin",
            KnownAs = "Admin",
            Gender = "",
            City = "",
            Country = "",
        };

        await userManager.CreateAsync(admin, "Pa$$w0rd");
        await userManager.AddToRolesAsync(admin, ["Admin", "Moderator"]);

    }


}