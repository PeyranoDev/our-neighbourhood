
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Infrastructure.ExternalsApis.Security.Interface;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.ExternalsApis.Security.Implementations;

public class AzureKeyVaultSecretProvider : ISecretProvider
{
    private readonly SecretClient _secretClient;

    public AzureKeyVaultSecretProvider(IConfiguration configuration)
    {
        var keyVaultEndpoint = configuration["AzureKeyVault:Endpoint"];
        var credential = new DefaultAzureCredential();
        _secretClient = new SecretClient(new Uri(keyVaultEndpoint), credential);
    }

    public string GetSecret(string secretName)
    {
        KeyVaultSecret secret = _secretClient.GetSecret(secretName);
        return secret.Value;
    }
}