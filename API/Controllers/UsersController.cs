using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository) : BaseApiController
{
   //Metoda do zwracania odpowiedzi HTTP  do klienta 
   [HttpGet] //Ządania HTTP Get 
   public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers() /*publiczna metoda.

                                                         Result action - jako typ rzeczy, ktore zamierzamy zwrocic z tego  punktu koncowego API

                                                         Następnie okreslamy typ  zwracanej rzeczy  - teraz zwrocimy liste użytkownikow (Istneje  wiele róznych  rodzajów list w C#, jedna z metod, ktorej mozemy do tego użyc jest enumarable - uzywane tylko dla kolekcji okreslonego typu) - zwrocimy IEnumereble typu appUser*/
   {
      //Stąd możemy zwracac odpowiedz HTTP
      var users =  await userRepository.GetMembersAsync(); //w ten sposob otrzymamy liste user'ow z naszej bazy

      return Ok(users); //Ok - nie dba o to co umiescili. Bo to co umiscili nie jest az tak dobre, przez to ze uzywamy kolekcje "IEnumerable"
   }


   //W tym przypadku chcemy uzysjac indywidualnego uzytkownaka
   //oprocz user'a API, chelibysmy wiedzic id user'a np w URL musi byc cos  takiego
   //[HttpGet("{id:int}")]  //np w URL musi byc cos  takiego - api/user/1 //:int - to jest bezpeczenstwo typu i okreslic ograniczenie - mowi nam ze nasz identyfikator biedzie typu integer.  
   
   //[HttpGet("{username:string}")] //parametr trasy jest domyslnie stringiem
   [HttpGet("{username}")]
   public async Task<ActionResult<MemberDto>> GetUser(string username) 
   {
      var user = await userRepository.GetMemberAsync(username);
      if(user == null) return NotFound();   
      return user;
   }

}
