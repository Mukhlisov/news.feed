using news.feed.models.Exceptions;

namespace news.feed.models.Models.Secret;

public record Secret<T>(T? Data, SecretStatus Status)
{
    public void EnsureCreated()
    {
        if (Status != SecretStatus.Created)
            throw new FailedToCreateSecretException($"Secret is not created. Status: {Status}");
    }

    public bool IsExpired => Status == SecretStatus.Expired;
}