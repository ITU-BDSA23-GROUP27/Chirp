using System.Globalization;
using System.Reflection;

if (args.Length < 1)
{
    Console.WriteLine("Usage: \n  " +
                      " - dotnet <read> \n  " +
                      " - dotnet <cheep> [message]");
    return;
}

ExecuteArgumentExecutableMethods(args);

void ExecuteArgumentExecutableMethods(string[] args)
{
    var obj = Activator.CreateInstance(typeof(Chirp)); // Instantiate the class
    
    foreach (MethodInfo methodInfo in typeof(Chirp).GetMethods())
    {
        var attr = methodInfo.GetCustomAttributes(typeof(ArgumentExecutableAttribute), false);
        
        if (attr.FirstOrDefault() == null) continue;
        
        var attrNames = (attr[0] as ArgumentExecutableAttribute)?.Names;
        
        if (attrNames != null && attrNames.Any(attrName => args.Contains(attrName) || args.Contains(attrName?.ToLower())))
        {
            methodInfo.Invoke(obj, null);
        }
    }
}

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
internal sealed class ArgumentExecutableAttribute : Attribute
{
    public IEnumerable<string> Names
    {
        get;
    }

    public ArgumentExecutableAttribute(params string[] _names)
    {
        this.Names = _names;
    }
}

internal sealed class Chirp
{
    const string FILE = "chirp_cli_db.csv";
    const string DATE_FORMAT = "MM/dd/yy HH:mm:ss";

    private static string[] Args
    {
        get => Environment.GetCommandLineArgs();
    }
    
    #region Commands

    [ArgumentExecutable("Read")]
    public void ReadChirps()
    {
        string[] lines = File.ReadAllLines(FILE);

        for (int i = 1; i < lines.Length; i++) // skip header line
        {
            string[] data = lines[i].Split('"');

            string author = data[0].TrimEnd(',');
            string message = data[1];
            string timestamp = data[2].TrimStart(',');

            string datetime = TimestampToDate(timestamp);

            string chirp = $"{author} @ {datetime}: {message}";
            Console.WriteLine(chirp);
        }
    }

    [ArgumentExecutable("Cheep", "New")]
    public void Cheep()
    {
        if (Args.Length < 2)
        {
            Console.WriteLine("When cheeping a message is required");
            return;
        }
    
        string message = Args[1];
        string username = Environment.UserName;
        long timestamp = DateToTimestamp(DateTime.Now.ToString(DATE_FORMAT, CultureInfo.InvariantCulture));

        string chirp = $"{username},\"{message}\",{timestamp}";
        File.AppendAllText(FILE, chirp + Environment.NewLine);

        Console.WriteLine("Text appended successfully.");
    }

    [ArgumentExecutable("Hello", "Test")]
    public void HelloWorldTest()
    {
        Console.WriteLine("Hello World!");
    }

    #endregion
    #region Utility Methods

    string TimestampToDate(string timestamp)
    {
        if (int.TryParse(timestamp, out int unixTimestamp))
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(unixTimestamp);
            string formattedDateTime = dateTimeOffset.ToLocalTime().ToString(DATE_FORMAT, CultureInfo.InvariantCulture);
            return formattedDateTime;
        }

        return "Invalid Timestamp";
    }

    long DateToTimestamp(string datetime)
    {
        if (DateTime.TryParseExact(datetime, DATE_FORMAT, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datetimeParse))
        {
            long timestamp = new DateTimeOffset(datetimeParse).ToUnixTimeSeconds();
            return timestamp;
        }

        return 0;
    }

    #endregion
}