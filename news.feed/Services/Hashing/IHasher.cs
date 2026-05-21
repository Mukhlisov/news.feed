namespace news.feed.Services.Hashing;

public interface IHasher
{
    bool Verify(string input, string hash);
    string Hash(string input);
}