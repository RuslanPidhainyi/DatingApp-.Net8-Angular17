using System;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class BuggyController(DataContext context) : BaseApiController
{
    [Authorize]
    [HttpGet("auth")]
    public ActionResult<string> GetAuth()
    {
        return "secret text";
    }

    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
        var thing = context.Users.Find(-1);

        if (thing == null) return NotFound();

        return thing;
    }

    [HttpGet("server-error")]
    public ActionResult<AppUser> GetServerError()
    {
        // try
        // {
        //     // Find(-1) - Znajdzi z index -1
        //     //  a operator - null -"??" sprawdza czy ma wartosci null, Jesli tak to wzuc wyjątek

        //     var thing = context.Users.Find(-1) ?? throw new Exception("A bad thing has happened"); //Z najdz index -1 w w bd tabele User, a jesli wartosc ma null, to wzuc wyjątek

        //     return thing;
        // }
        // catch (Exception ex)
        // {
        //     return StatusCode(500, "Computer says no!");
        // }


        // Find(-1) - Znajdzi z index -1
        //  a operator - null -"??" sprawdza czy ma wartosci null, Jesli tak to wzuc wyjątek

        var thing = context.Users.Find(-1) ?? throw new Exception("A bad thing has happened"); //Z najdz index -1 w w bd tabele User, a jesli wartosc ma null, to wzuc wyjątek

        return thing;
    }

    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("This was not a good request");
    }
}
