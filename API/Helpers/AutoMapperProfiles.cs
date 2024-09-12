using System;
using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

//Narzedzia mapper lub AutoMapper jest pomocnym, zebys nie tworzyc recznie za kazdym razem nowy objekt, i mappowac recznie kazdego czlonka w objekcie, dla tego mamy AutoMapper.

/*
щоб не  робить ось таке MAppowania в ручну:
var someObject = new MemberDto 
{
  id = id
  UserName = Username
  ...
}
*/

public class AutoMapperProfiles : Profile // Profile належить бібліотеці AutoMapper і використовується для створення конфігурацій мапінгу. Тобто, ми створюємо профіль, який буде містити налаштування мапінгу.
{
    //ctor - construktor
    //Конструктор — це спеціальний метод, який викликається при створенні екземпляра класу. У конструкторі ми визначаємо, як відбуватиметься мапінг між об'єктами.
    public AutoMapperProfiles()
    {
        //Цей блок налаштовує мапінг між об'єктами типу AppUser (сутність користувача в базі даних) та MemberDto (об'єкт для передачі даних).
        CreateMap<AppUser, MemberDto>()
        .ForMember(d => d .Age, o => o.MapFrom(s => s.DateOfBirth.CalculateAge()))
        .ForMember(d => d.PhotoUrl, 
                o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.Url));
        //Цей блок налаштовує мапінг між об'єктами типу Photo (сутність користувача в базі даних) та PhotoDto (об'єкт для передачі даних).
        CreateMap<Photo, PhotoDto>();
    }
}
