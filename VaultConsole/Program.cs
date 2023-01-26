using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.AuthMethods;
using VaultSharp;
using VaultSharp.V1.Commons;

//conect
IAuthMethodInfo authMethod = new TokenAuthMethodInfo(vaultToken: "myroot");
VaultClientSettings vaultClientSettings = new
VaultClientSettings("http://vault:8200", authMethod);
IVaultClient vaultClient = new VaultClient(vaultClientSettings);

Console.WriteLine($"Conectou {DateTime.Now}");

//write secret
var secretData = new Dictionary<string, object> { { "ConnectionString", "mongodb://localhost:27017" } };
vaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(
    path: "/mongo",
    data: secretData,
    mountPoint: "secret"
).Wait();

Console.WriteLine("Secret criado.");

//read secret
Secret<SecretData> secret = vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
    path: "/mongo",
    mountPoint: "secret"
).Result;
var conexao = secret.Data.Data["ConnectionString"];

Console.WriteLine(conexao.ToString());