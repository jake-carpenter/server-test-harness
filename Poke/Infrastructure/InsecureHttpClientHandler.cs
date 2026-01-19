namespace Poke.Infrastructure;

/// <summary>
/// A <see cref="HttpClientHandler"/> that accepts any server certificate.
/// </summary>
public class InsecureHttpClientHandler : HttpClientHandler
{
    public InsecureHttpClientHandler()
    {
        ServerCertificateCustomValidationCallback = DangerousAcceptAnyServerCertificateValidator;
    }
}
