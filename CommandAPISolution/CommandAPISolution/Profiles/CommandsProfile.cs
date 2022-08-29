using AutoMapper;
using CommandAPISolution.Dtos;
using CommandAPISolution.Models;

namespace CommandAPISolution.Profiles;

public class CommandsProfile: Profile
{
    public CommandsProfile()
    {
        CreateMap<Command, CommandReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<CommandUpdateDto, Command>();
        CreateMap<Command, CommandUpdateDto>();
    }
}