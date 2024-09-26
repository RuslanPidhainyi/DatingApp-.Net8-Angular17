using System;

namespace API.DTOs;

public class CreateMassageDto
{
    public required string RecipientUsername { get; set; }
    public required string Content { get; set; }

}
