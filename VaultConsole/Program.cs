using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods;
using VaultSharp;
using VaultSharp.V1.Commons;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using IHost host = Host.CreateDefaultBuilder(args).Build();
IConfiguration config = host.Services.GetRequiredService<IConfiguration>();

var vault_host = config.GetValue<string>("Vault:host");
var vault_token = config.GetValue<string>("Vault:token");
var vault_secret_path = config.GetValue<string>("Vault:secret_path");

try
{
    //conect
    IAuthMethodInfo authMethod = new TokenAuthMethodInfo(vaultToken: vault_token);
    VaultClientSettings vaultClientSettings = new
    VaultClientSettings(vault_host, authMethod);
    IVaultClient vaultClient = new VaultClient(vaultClientSettings);

    //write secret
    var secretData = new Dictionary<string, object> { { "ConnectionString", "mongodb://localhost:27017" } };
    vaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(
        path: vault_secret_path,
        data: secretData,
        mountPoint: "secret"
    ).Wait();
    Console.WriteLine($"Secret created - {DateTime.Now}");

    //read secret
    Secret<SecretData> secret = vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
        path: vault_secret_path,
        mountPoint: "secret"
    ).Result;
    var conexao = secret.Data.Data["ConnectionString"];
    Console.WriteLine($"Secret: {conexao.ToString()}");
}
catch(Exception error)
{
    Console.WriteLine(error.ToString());
}