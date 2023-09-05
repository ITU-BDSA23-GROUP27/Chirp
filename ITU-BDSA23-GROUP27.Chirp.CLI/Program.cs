if (args.Length == 0)
{
    Console.WriteLine("Missing argument \n  " +
                      "-read : to read all messages \n  " +
                      "-cheep 'Your Message' : post a message");
    return;
}

switch (args[0])
{
    case "-read":
        break;
    case "-cheep":
        break;
    default:
        Console.WriteLine("Invalid Argument");
        return;
}