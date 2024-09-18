using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService) : BaseApiController
{
   //Metoda do zwracania odpowiedzi HTTP  do klienta 
   [HttpGet] //Ządania HTTP Get 
   public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers() /*publiczna metoda.

                                                         Result action - jako typ rzeczy, ktore zamierzamy zwrocic z tego  punktu koncowego API

                                                         Następnie okreslamy typ  zwracanej rzeczy  - teraz zwrocimy liste użytkownikow (Istneje  wiele róznych  rodzajów list w C#, jedna z metod, ktorej mozemy do tego użyc jest enumarable - uzywane tylko dla kolekcji okreslonego typu) - zwrocimy IEnumereble typu appUser*/
   {
      //Stąd możemy zwracac odpowiedz HTTP
      var users = await userRepository.GetMembersAsync(); //w ten sposob otrzymamy liste user'ow z naszej bazy

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
      if (user == null) return NotFound();
      return user;
   }

   [HttpPut]
   public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
   {

      var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());//Pobieramy naszego usera z Repository

      if (user is null) return BadRequest("Could not found user");

      mapper.Map(memberUpdateDto, user);//Func mapowania bierze naszą memberUpdateDto, która zawiera w siebie rozne wlasciwosci

      if (await userRepository.SaveAllASync()) return NoContent(); //Sprawdza ile zmian zostawo zapisano w bd to wysyla - NoContent() 204, W przeciwnym przepadku jezeli tak samo, czyli nic nie zmienilismy wysli BadRequest("Failed to update the user");

      return BadRequest("Failed to update the user");
   }

   [HttpPost("add-photo")]
   public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
   {
      var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

      if (user == null) return BadRequest("Cannot update user");

      var result = await photoService.AddPhotoAsync(file);

      if (result.Error != null) return BadRequest(result.Error.Message);

      var photo = new Photo
      {
         Url = result.SecureUrl.AbsoluteUri,
         PublicId = result.PublicId
      };

      user.Photos.Add(photo);

      if (await userRepository.SaveAllASync()) return mapper.Map<PhotoDto>(photo);

      return BadRequest("Problem adding photo");
   }

}
