namespace CheepRepository.Model;

public class AuthorDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public IEnumerable<Guid> CheepIds { get; set; } = new List<Guid>();
}