using Chirp.Infrastructure.Entities;

namespace Chirp.Infrastructure.Data;

public static class DbInitializer2
{
public static void SeedDatabase2(ChirpContext chirpContext)
{
    //? This data is made for UI-Testing with Playwright
    //? New data with authors other than one self following another author
    if (!(chirpContext.Authors.Any() && chirpContext.Cheeps.Any()))
    {
        // Authors
        var a1 = new Author() { AuthorId = new Guid(), Name = "PhiVaLo",  Email = "user1@chirp.com", Cheeps = new List<Cheep>() };
        var a2 = new Author() { AuthorId = new Guid(), Name = "Tien197",  Email = "user2@chirp.com", Cheeps = new List<Cheep>() };
        var a3 = new Author() { AuthorId = new Guid(), Name = "HelgeCPH", Email = "user3@chirp.com", Cheeps = new List<Cheep>() };
        var a4 = new Author() { AuthorId = new Guid(), Name = "ondfisk",  Email = "user4@chirp.com", Cheeps = new List<Cheep>() };

        var authors = new List<Author>() { a1, a2, a3, a4 };

        // Cheeps
        var c100 = new Cheep() { CheepId = new Guid(), Author = a1, Text = "Test Data for UI-Test with Playwright", TimeStamp = DateTime.Parse("1990-12-01 08:00:00") };
        var c101 = new Cheep() { CheepId = new Guid(), Author = a1, Text = "Test Data for UI-Test with Playwright", TimeStamp = DateTime.Parse("1990-12-01 07:00:00") };

        var c200 = new Cheep() { CheepId = new Guid(), Author = a2, Text = "Follows HelgeCPH & ondfisk", TimeStamp = DateTime.Parse("1991-12-01 06:00:00") };
        var c201 = new Cheep() { CheepId = new Guid(), Author = a2, Text = "Follows HelgeCPH & ondfisk", TimeStamp = DateTime.Parse("1990-12-01 05:00:00") };
        var c202 = new Cheep() { CheepId = new Guid(), Author = a2, Text = "Follows HelgeCPH & ondfisk", TimeStamp = DateTime.Parse("1990-12-01 05:00:00") };

        var c300 = new Cheep() { CheepId = new Guid(), Author = a3, Text = "Follows ondfisk", TimeStamp = DateTime.Parse("1992-12-01 04:00:00") };
        var c301 = new Cheep() { CheepId = new Guid(), Author = a3, Text = "Follows ondfisk", TimeStamp = DateTime.Parse("1991-12-01 03:00:00") };
        var c302 = new Cheep() { CheepId = new Guid(), Author = a3, Text = "Follows ondfisk", TimeStamp = DateTime.Parse("1990-12-01 03:00:00") };
        
        var c400 = new Cheep() { CheepId = new Guid(), Author = a4, Text = "3", TimeStamp = DateTime.Parse("1992-12-01 02:00:00") };
        var c401 = new Cheep() { CheepId = new Guid(), Author = a4, Text = "2", TimeStamp = DateTime.Parse("1991-12-01 01:00:00") };
        var c402 = new Cheep() { CheepId = new Guid(), Author = a4, Text = "1", TimeStamp = DateTime.Parse("1990-12-01 00:00:00") };

        var cheeps = new List<Cheep>() { 
            c100, c101, 
            c200, c201, c202,
            c300, c301, c302,
            c400, c401, c402 };

        chirpContext.Authors.AddRange(authors);
        chirpContext.Cheeps.AddRange(cheeps);

        // Followers
        var follower2 = new Follower { FollowerId = Guid.NewGuid(), FolloweeId = Guid.NewGuid(), FollowerAuthor = a2, FolloweeAuthor = a3 };
        var follower3 = new Follower { FollowerId = Guid.NewGuid(), FolloweeId = Guid.NewGuid(), FollowerAuthor = a2, FolloweeAuthor = a4 };
        var follower4 = new Follower { FollowerId = Guid.NewGuid(), FolloweeId = Guid.NewGuid(), FollowerAuthor = a3, FolloweeAuthor = a4 };

        var followers = new List<Follower> { follower2, follower3, follower4};

        chirpContext.Followers.AddRange(followers);

        chirpContext.SaveChanges();
    }
}

}