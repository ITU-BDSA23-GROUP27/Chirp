using System.Diagnostics;
using Microsoft.VisualBasic;
using SimpleDB;
using Xunit.Abstractions;

namespace Chirp.CLI.Tests;

public class End_To_EndTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    // Debugger
    public End_To_EndTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    // TODO - run on a test.csv
    // 

    //Executes the program with the given command
    private string CLIRun(string args)
    {
        //! ChatGPT - Get the absolute path to your CLI project's directory
        string cliProjectDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "..", "src", "ITU-BDSA23-GROUP27.Chirp.CLI");
        
        // Act
        using (Process process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = $"run {args}";
            process.StartInfo.WorkingDirectory = cliProjectDirectory;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            
             // Synchronously read the standard output of the spawned process.
            var reader = process.StandardOutput;
            var output = reader.ReadToEnd();
            process.WaitForExit();

            return output;
        }
    }
    
    [Fact]
    public void Test_ReadCheep_ReturnsFirstCheep() 
    {
        // Arrange
        var output = CLIRun("read");
    
        // Act
        string firstCheep = output.Split("\n")[0].TrimEnd('\r', '\n');

        // Assert
        Assert.StartsWith("ropf", firstCheep);
        Assert.EndsWith("Hello, BDSA students!", firstCheep);
    }

    [Theory]
    [InlineData("read 2", 2)]
    [InlineData("read 4", 4)]
    public void Test_ReadNumberOfTotalCheeps_ReturnsTotalCheeps(string args, int expected) 
    {
        // Arrange
        var output = CLIRun(args);
    
        // Act
        var actual = output.TrimEnd('\r', '\n').Split("\n").Length;

        // Assert
        Assert.Equal(expected, actual);
    }

    public void Test_CheepAndReadLastMessage() 
    {
        // Arrange
        var msg = "cheep Hi";
        CLIRun(msg);

        var output = CLIRun("read");
    
        // Act
        var lastCheep = output.TrimEnd('\r', '\n').Split("\n")[^1];

        // Assert
        Assert.EndsWith(msg, lastCheep);
    }
}