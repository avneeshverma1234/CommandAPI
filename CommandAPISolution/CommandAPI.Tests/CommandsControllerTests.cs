using System;
using System.Collections.Generic;
using AutoMapper;
using CommandAPISolution.Controllers;
using CommandAPISolution.Data;
using CommandAPISolution.Dtos;
using CommandAPISolution.Models;
using CommandAPISolution.Profiles;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CommandAPI.Tests;

public class CommandsControllerTests : IDisposable
{
    Mock<ICommandAPIRepo> mockRepo;
    CommandsProfile realProfile;
    MapperConfiguration configuration;
    IMapper mapper;

    public CommandsControllerTests()
    {
        mockRepo = new Mock<ICommandAPIRepo>();
        realProfile = new CommandsProfile();
        configuration = new MapperConfiguration(cfg => cfg.
            AddProfile(realProfile));
        mapper = new Mapper(configuration);
    }
    
    [Fact]
    public void GetCommandItems_ReturnsZeroItems_WhenDBIsEmpty()
    {
       
        var controller = new CommandsController(mockRepo.Object, mapper);
        //Act
        var result = controller.GetAllCommands();
        //Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public void GetAllCommands_ReturnsOneItem_WhenDBHasOneResource()
    { 
//Arrange
        mockRepo.Setup(repo =>
            repo.GetAllCommands()).Returns(GetCommands(1));
        var controller = new CommandsController(mockRepo.Object, mapper);
//Act
        var result = controller.GetAllCommands();
//Assert
        var okResult = result.Result as OkObjectResult;
        var commands = okResult.Value as List<CommandReadDto>;
        Assert.Single(commands);
    }

    [Fact]
    public void GetAllCommands_Returns200OK_WhenDBHasOneResource()
    {
//Arrange
        mockRepo.Setup(repo =>
            repo.GetAllCommands()).Returns(GetCommands(1));
        var controller = new CommandsController(mockRepo.Object, mapper);
//Act
        var result = controller.GetAllCommands();
//Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }
    [Fact]
    public void GetAllCommands_ReturnsCorrectType_WhenDBHasOneResource()
    {
//Arrange
        mockRepo.Setup(repo =>
            repo.GetAllCommands()).Returns(GetCommands(1));
        var controller = new CommandsController(mockRepo.Object, mapper);
//Act
        var result = controller.GetAllCommands();
        //Assert
        Assert.IsType<ActionResult<IEnumerable<CommandReadDto>>>(result);
    }
    
    private List<Command> GetCommands(int num)
    {
        var commands = new List<Command>();
        if (num > 0)
        {
            commands.Add(new Command
            {
                Id = 0,
                HowTo = "How to generate a migration",
                CommandLine = "dotnet ef migrations add <Name of Migration>",
                Platform = ".Net Core EF"
            });
        }

        return commands;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            mockRepo = null;
            mapper = null;
            configuration = null;
            realProfile = null;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}