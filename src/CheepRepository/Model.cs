namespace CheepRepository;

public class Model
{
}

public class Cheep
{
    public string Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public Author Author { get; set; }
}

public class Author
{
    public string Name { get; set; }
    public string Email { get; set; }
    public List<Cheep> Cheeps { get; set; } = new();
}