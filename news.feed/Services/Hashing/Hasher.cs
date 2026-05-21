using BC = BCrypt.Net.BCrypt;

namespace news.feed.Services.Hashing;

public class Hasher : IHasher
{
    public bool Verify(string input, string hash) => BC.Verify(input, hash);
    public string Hash(string input) => BC.HashPassword(input);
}