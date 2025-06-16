namespace AzureTools.Secrets.Provider
{
    using Azure;
    using Azure.Security.KeyVault.Certificates;
    using Azure.Security.KeyVault.Secrets;
    using AzureTools.Authentication.Provider;
    using AzureTools.Secrets.Settings;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;

    public class KeyVaultSecretProvider
    {
        private readonly ITokenCredentialProvider _tokenCredentialProvider;
        private readonly SecretProviderSettings _providerSettings;
        private readonly ConcurrentDictionary<Uri, SecretClient> _secretClients = new ();
        private readonly ConcurrentDictionary<Uri, CertificateClient> _certificateClients = new ();

        public KeyVaultSecretProvider
            (ITokenCredentialProvider tokenCredentialProvider,
            IOptions<SecretProviderSettings> providerOptions)
        {
            _tokenCredentialProvider = tokenCredentialProvider
                ?? throw new ArgumentNullException(nameof(tokenCredentialProvider));
            _providerSettings = providerOptions?.Value ?? throw new ArgumentNullException(nameof(providerOptions));
            if (_providerSettings.KeyVaultUri == null)
            {
                throw new ArgumentException("KeyVault URI must be provided in settings.", nameof(providerOptions));
            }
            if (_providerSettings.AuthenticationSettings == null)
            {
                throw new ArgumentException("Authentication settings must be provided in settings.", nameof(providerOptions));
            }
        }

        public async Task<string> GetSecretAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Secret name cannot be null or empty.", nameof(name));
            }

#pragma warning disable CS8604 // Possible null reference argument. This is null checked in the ctor.
            var client = InitializeClient(_providerSettings.KeyVaultUri);
#pragma warning restore CS8604 // Possible null reference argument.
            var secret = await client.GetSecretAsync(name);

            if (secret == null)
            {
                throw new KeyNotFoundException($"Secret '{name}' not found in Key Vault at {_providerSettings.KeyVaultUri}.");
            }

            KeyVaultSecret secretValue = secret.Value;

            if (secretValue == null || string.IsNullOrWhiteSpace(secretValue.Value))
            {
                throw new KeyNotFoundException($"Secret '{name}' has no value in Key Vault at {_providerSettings.KeyVaultUri}.");
            }

            return secretValue.Value;
        }

        public async Task SetSecretAsync(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Secret name cannot be null or empty.", nameof(name));
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Secret value cannot be null or empty.", nameof(value));
            }

            var vaultUri = _providerSettings.KeyVaultUri;
            var client = InitializeClient(vaultUri);

            var secret = await client.SetSecretAsync(name, value);

            if (secret == null)
            {
                throw new Exception($"Failed to set secret '{name}' in Key Vault at {vaultUri}.");
            }

            var rawResponse = secret.GetRawResponse(); // Ensure the secret is set by accessing the response

            if (rawResponse == null)
            {
                throw new Exception($"Failed to set secret '{name}' in Key Vault at {vaultUri}.");
            }

            if (rawResponse.IsError)
            {
                throw new RequestFailedException(rawResponse.Status, $"Failed to set secret '{name}' in Key Vault at {vaultUri}.", rawResponse.ReasonPhrase, null);
            }
        }

        public async Task<X509Certificate2> GetCertificateAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Certificate name cannot be null or empty.", nameof(name));
            }

#pragma warning disable CS8604 // Possible null reference argument. This is null checked in the ctor.
            var client = InitializeCertificateClient(_providerSettings.KeyVaultUri);
#pragma warning restore CS8604 // Possible null reference argument.
            var certificate = await client.GetCertificateAsync(name);
            if (certificate == null)
            {
                throw new KeyNotFoundException($"Certificate '{name}' not found in Key Vault at {_providerSettings.KeyVaultUri}.");
            }
            
            var certificateValue = certificate.Value;

            if (certificateValue == null || string.IsNullOrWhiteSpace(certificateValue.Name))
            {
                throw new KeyNotFoundException($"Certificate '{name}' has no value in Key Vault at {_providerSettings.KeyVaultUri}.");
            }

            var x509Certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2(certificateValue.Cer);

            return x509Certificate;
        }

        private SecretClient InitializeClient(Uri vaultUri)
        {
            if (vaultUri == null)
            {
                throw new ArgumentException("Vault URI cannot be null or empty.", nameof(vaultUri));
            }
            if (_secretClients.TryGetValue(vaultUri, out var client))
            {
                return client;
            }

#pragma warning disable CS8604 // Possible null reference argument. This was null checked in the ctor.
            var secretClient = new SecretClient(vaultUri, _tokenCredentialProvider.GetCredential(_providerSettings.AuthenticationSettings));
#pragma warning restore CS8604 // Possible null reference argument.
            _secretClients[vaultUri] = secretClient;
            return secretClient;
        }
        
        private CertificateClient InitializeCertificateClient(Uri vaultUri)
        {
            if (vaultUri == null)
            {
                throw new ArgumentException("Vault URI cannot be null or empty.", nameof(vaultUri));
            }
            if (_certificateClients.TryGetValue(vaultUri, out var client))
            {
                return client;
            }

#pragma warning disable CS8604 // Possible null reference argument. This was null checked in the ctor.
            var certClient = new CertificateClient(vaultUri, _tokenCredentialProvider.GetCredential(_providerSettings.AuthenticationSettings));
#pragma warning restore CS8604 // Possible null reference argument.
            _certificateClients[vaultUri] = certClient;
            return certClient;
        }

    }
}
