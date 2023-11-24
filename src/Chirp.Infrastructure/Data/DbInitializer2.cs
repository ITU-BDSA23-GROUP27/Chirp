using Chirp.Infrastructure.Entities;

namespace Chirp.Infrastructure.Data;

public static class DbInitializer2
{
public static void SeedDatabase2(ChirpContext chirpContext)
{
    //? This data is made for UI-Testing with Playwright
    //? New data with authors other than one self following another author
    if (!(chirpContext.Users.Any() && chirpContext.Cheeps.Any()))
    {
        // Authors
        var a1 = new User() { Name = "PhiVaLo",  Email = "user1@chirp.com", Cheeps = new List<Cheep>() };
        var a2 = new User() { Name = "Chirp27",  Email = "user2@chirp.com", Cheeps = new List<Cheep>() };
        var a3 = new User() { Name = "Happy", Email = "user3@chirp.com", Cheeps = new List<Cheep>() };
        var a4 = new User() { Name = "Smiley",  Email = "user4@chirp.com", Cheeps = new List<Cheep>() };

        var authors = new List<User>() { a1, a2, a3, a4 };

        // Cheeps
        var c100 = new Cheep() { CheepId = new Guid(), User = a1, Text = "UI-Test with Playwright", TimeStamp = DateTime.Parse("1990-12-01 08:00:00") };

        var c200 = new Cheep() { CheepId = new Guid(), User = a2, Text = "Follows → Happy & Smiley & PhiVaLo", TimeStamp = DateTime.Parse("1991-12-01 06:00:00") };
        var c201 = new Cheep() { CheepId = new Guid(), User = a2, Text = "Follows → Happy & Smiley & PhiVaLo", TimeStamp = DateTime.Parse("1990-12-01 05:00:00") };
        var c202 = new Cheep() { CheepId = new Guid(), User = a2, Text = "Follows → Happy & Smiley & PhiVaLo", TimeStamp = DateTime.Parse("1990-12-01 05:00:00") };

        var c300 = new Cheep() { CheepId = new Guid(), User = a3, Text = "Follows → Smiley", TimeStamp = DateTime.Parse("1992-12-01 04:00:00") };
        var c301 = new Cheep() { CheepId = new Guid(), User = a3, Text = "Follows → Smiley", TimeStamp = DateTime.Parse("1991-12-01 03:00:00") };
        var c302 = new Cheep() { CheepId = new Guid(), User = a3, Text = "Follows → Smiley", TimeStamp = DateTime.Parse("1990-12-01 03:00:00") };
        
        var c400 = new Cheep() { CheepId = new Guid(), User = a4, Text = "Hello BDSA GR27", TimeStamp = DateTime.Parse("1992-12-01 02:00:00") };
        var c401 = new Cheep() { CheepId = new Guid(), User = a4, Text = "Hello BDSA GR27", TimeStamp = DateTime.Parse("1991-12-01 01:00:00") };
        var c402 = new Cheep() { CheepId = new Guid(), User = a4, Text = "Hello BDSA GR27", TimeStamp = DateTime.Parse("1990-12-01 00:00:00") };

        var cheeps = new List<Cheep>() { 
            c100,
            c200, c201, c202,
            c300, c301, c302,
            c400, c401, c402 };

        chirpContext.Users.AddRange(authors);
        chirpContext.Cheeps.AddRange(cheeps);

        // Followers
        var follower2 = new Follower { FollowerId = Guid.NewGuid(), FolloweeId = Guid.NewGuid(), FollowerUser = a2, FolloweeUser = a3 };
        var follower3 = new Follower { FollowerId = Guid.NewGuid(), FolloweeId = Guid.NewGuid(), FollowerUser = a2, FolloweeUser = a4 };
        var follower4 = new Follower { FollowerId = Guid.NewGuid(), FolloweeId = Guid.NewGuid(), FollowerUser = a3, FolloweeUser = a4 };
        var follower5 = new Follower { FollowerId = Guid.NewGuid(), FolloweeId = Guid.NewGuid(), FollowerUser = a2, FolloweeUser = a1 };

        var followers = new List<Follower> { follower2, follower3, follower4, follower5 };

        chirpContext.Followers.AddRange(followers);

        chirpContext.SaveChanges();
    }
}

}