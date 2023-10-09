namespace CheepRepository.Model;

public class AuthorDetailDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public IEnumerable<int> CheepIds { get; set; } = new List<int>();
}