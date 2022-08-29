using System.Dynamic;
using AutoMapper;
using CommandAPISolution.Data;
using CommandAPISolution.Dtos;
using CommandAPISolution.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CommandAPISolution.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommandsController: ControllerBase
{
    private readonly ICommandAPIRepo commandApiRepo;
    private readonly IMapper mapper;

    public CommandsController(ICommandAPIRepo commandApiRepo, IMapper mapper)
    {
        this.commandApiRepo = commandApiRepo;
        this.mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<IEnumerable<CommandReadDto>> Get()
    {
        return Ok(mapper.Map<IEnumerable<CommandReadDto>>( this.commandApiRepo.GetAllCommands()));
    }

    [HttpGet("{id:int}", Name = "GetCommandById")]
    public ActionResult<CommandReadDto> GetCommandById(int id)
    {
        var commandItem = this.commandApiRepo.GetCommandById(id);
        if (commandItem == null)
        {
            return NotFound();
        }
        return Ok(mapper.Map<CommandReadDto>(commandItem));
    }
    [HttpPost]
    public ActionResult <CommandReadDto> CreateCommand
        (CommandCreateDto commandCreateDto)
    {
        var commandModel = mapper.Map<Command>(commandCreateDto);
        commandApiRepo.CreateCommand(commandModel);
        commandApiRepo.SaveChanges();
        var commandReadDto = mapper.Map<CommandReadDto>(commandModel);
        return CreatedAtRoute(nameof(GetCommandById),
            new {Id = commandReadDto.Id}, commandReadDto);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateCommand(int id, CommandUpdateDto
        commandUpdateDto)
    {
        var commandModelFromRepo = commandApiRepo.GetCommandById(id);
        if (commandModelFromRepo == null)
        {
            return NotFound();
        }

        mapper.Map(commandUpdateDto, commandModelFromRepo);
        commandApiRepo.UpdateCommand(commandModelFromRepo);
        commandApiRepo.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public ActionResult PartialCommandUpdate(int id,
        JsonPatchDocument<CommandUpdateDto> patchDoc)
    {
        var commandModelFromRepo = commandApiRepo.GetCommandById(id);
        if (commandModelFromRepo == null)
        {
            return NotFound();
        }

        var commandToPatch = mapper.Map<CommandUpdateDto>(commandModelFromRepo);
        patchDoc.ApplyTo(commandToPatch, ModelState);
        if (!TryValidateModel(commandToPatch))
        {
            return ValidationProblem(ModelState);
        }

        mapper.Map(commandToPatch, commandModelFromRepo);
        commandApiRepo.UpdateCommand(commandModelFromRepo);
        commandApiRepo.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCommand(int id)
    {
        var commandModelFromRepo = commandApiRepo.GetCommandById(id);
        if (commandModelFromRepo == null)
        {
            return NotFound();
        }
        commandApiRepo.DeleteCommand(commandModelFromRepo);
        commandApiRepo.SaveChanges();
        return NoContent();
    }
}