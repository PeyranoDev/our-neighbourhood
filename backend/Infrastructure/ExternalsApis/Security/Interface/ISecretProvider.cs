namespace Infrastructure.ExternalsApis.Security.Interface
{
    public interface ISecretProvider
    {
        string GetSecret(string secretName);
    }
}