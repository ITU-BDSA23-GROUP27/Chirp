using System.Diagnostics;
using SimpleDB;
using Xunit.Abstractions;

namespace Chirp.CLI.Tests;

public class End_To_EndTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public End_To_EndTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    // checks if Chirp returns the correct cheeps when using the command "read {limit}"
    [Fact]
    public void Commandline_Read_ReturnsCheeps() 
    {
        // Arrange
        // TODO Change to just use the singleton implicitly
        var db = CSVDatabase<Cheep>.Instance;

        // Act
        string output = "";
        string commandLineArguments = "--read";
        
        using (var process = new Process())
        {
            process.StartInfo.FileName = "C:/Program Files/dotnet/dotnet.exe";
            process.StartInfo.Arguments = $"./src/ITU-BDSA23-GROUP27.Chirp.CLI/bin/Debug/net7.0/ITU-BDSA23-GROUP27.Chirp.CLI.dll {commandLineArguments}";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WorkingDirectory = "../../../../../";
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            
            // Read std output of spawned process
            var reader = process.StandardOutput;
            output = reader.ReadToEnd();
            process.WaitForExit();
        }

        string fstCheep = output.Split("\n")[0];

        _testOutputHelper.WriteLine(fstCheep);
        
        // Assert
        Assert.StartsWith("ropf", fstCheep);
        Assert.EndsWith("Hello, World!", fstCheep);
    }

    // checks if Chirp returns the same amount of Cheeps as limit
    public void Commandline_Read_ReturnsLimit()
    {
        // Arrange
        
        // Act
        
        // Assert
    }
}