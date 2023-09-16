using System.Reflection;
using CommandLine;

namespace ITU_BDSA23_GROUP27.Chirp.CLI;

class CommandLineOptions
{
    [Verb("read", aliases:new []{"r"}, HelpText = "Read cheeps from database")]
    internal class ReadOptions
    {
        [Option("limit", Required = false, HelpText = "The amount of cheeps to read")]
        public int? Limit { get; set; }
    }

    [Verb("cheep", aliases:new []{"c"}, HelpText = "Create new cheep")]
    internal class CheepOptions
    {
        [Option('m', "message", Required = true, HelpText = "The message to cheep")]
        public string Message { get; set; }
    }
    
    internal class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }
    }

    internal static Type[] GetVerbs()
    {
        return typeof(CommandLineOptions).GetNestedTypes(BindingFlags.NonPublic)
            .Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray();  
    }
}