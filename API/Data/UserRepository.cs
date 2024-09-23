using System;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public async Task<MemberDto?> GetMemberAsync(string username)
    {
        return await context.Users
        .Where(x => x.UserName == username)//Використовує LINQ-запит для пошуку користувача з бази даних (_context.Users), де UserName відповідає значенню username.
        .ProjectTo<MemberDto>(mapper.ConfigurationProvider)// Перетворює\проеціює знайдений об'єкт User на MemberDto за допомогою конфігурації AutoMapper.
        .SingleOrDefaultAsync();//асинхронно повертає єдиний об'єкт або null, якщо об'єкт не знайдено.
    }

    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
    {
        var query =  context.Users.AsQueryable();

        query = query.Where(x => x.UserName != userParams.CurrentUsername);

        if(userParams.Gender != null )
        {
            query = query.Where(x => x.Gender == userParams.Gender);
        }

        return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
    }

    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        return await context.Users
        .Include(x => x.Photos)//включає навігаційну властивість Photos, щоб завантажити дані про фотографії користувача разом з основними даними.
        .SingleOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await context.Users
        .Include(x => x.Photos)
        .ToListAsync();// асинхронно повертає всі результати у вигляді списку.
    }

    public async Task<bool> SaveAllASync()
    {
        return await context.SaveChangesAsync() > 0;//Jezeli wartosc jest wieksza od zera to wartosc jest zapisana
    }

    public void Update(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
    }

    /*
    Ціль: Оновити інформацію про користувача.
    Кроки виконання:
        Змінює стан об'єкта user на Modified, щоб позначити, що він був змінений.
        Зміни будуть збережені при наступному виклику SaveChanges або SaveChangesAsync.
    */
}
