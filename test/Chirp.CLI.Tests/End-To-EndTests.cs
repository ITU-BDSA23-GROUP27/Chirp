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
        // var db = CSVDatabase<Cheep>.Instance;


        // Get the absolute path to your CLI project's directory
        string cliProjectDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "..", "src", "ITU-BDSA23-GROUP27.Chirp.CLI");


        // Act
        string output = "";
        
        using (var process = new Process())
        {
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "run read";
            process.StartInfo.WorkingDirectory = cliProjectDirectory;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            
            // Read std output of spawned process
            var reader = process.StandardOutput;
            output = reader.ReadToEnd();
            process.WaitForExit();
        }
        _testOutputHelper.WriteLine("TEEEEEEEEEEEEEEEEEEEEEEEEEEEST");
        _testOutputHelper.WriteLine(output);
        _testOutputHelper.WriteLine("TEEEEEEEEEEEEEEEEEEEEEEEEEEEST");

        string fstCheep = output.Split("\n")[0];

        fstCheep = fstCheep.TrimEnd('\r', '\n');
 
        _testOutputHelper.WriteLine(fstCheep);


        // Assert
        Assert.StartsWith("ropf", fstCheep);
        Assert.EndsWith("Hello, BDSA students!", fstCheep);
    }
    
}

