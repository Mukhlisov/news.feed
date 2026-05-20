using news.feed.models.Models.Secret;

namespace news.feed.Services.Auth;

public interface ISecretProvider
{
    Secret<string> CreateSecretFor(string issuer);
    Secret<string> GetSecret(string issuer);
}