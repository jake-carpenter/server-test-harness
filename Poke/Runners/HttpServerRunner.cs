using Poke.Commands;
using Poke.Infrastructure;
using Poke.Models;

namespace Poke.Runners;

/// <summary>
/// Executes HTTP Server connectivity checks.
/// </summary>
public class HttpServerRunner : IRunner
{
    /// <summary>
    /// The formatter for HTTP Server output.
    /// </summary>
    public IRunnerFormatter Formatter => new HttpServerFormatter();

    /// <summary>
    /// Executes the HTTP Server connectivity check.
    /// </summary>
    /// <param name="server">The server to execute.</param>
    /// <param name="settings">The run settings.</param>
    /// <returns>The run result.</returns>
    public async Task<RunResult> Execute(Server server, RunSettings settings)
    {
        if (server is not HttpServer httpServer)
            throw new InvalidOperationException(
                $"Expected {nameof(HttpServer)} but got {server.GetType().Name}"
            );

        if (settings.DryRun)
            return RunResult.Success();

        try
        {
            using var handler = CreateHandler(httpServer);

            // ReSharper disable once ShortLivedHttpClient - It will only ever be used once.
            using var client = new HttpClient(handler, disposeHandler: false);
            using var request = new HttpRequestMessage(HttpMethod.Get, httpServer.Uri);
            using var response = await client.SendAsync(request);

            return response.IsSuccessStatusCode
                ? RunResult.Success()
                : RunResult.Failure(
                    new HttpRequestException(
                        $"HTTP request failed with status code {(int)response.StatusCode} ({response.StatusCode})"
                    )
                );
        }
        catch (Exception ex)
        {
            return RunResult.Failure(ex);
        }
    }

    private static HttpMessageHandler CreateHandler(HttpServer httpServer)
    {
        return httpServer.Insecure ? new InsecureHttpClientHandler() : new HttpClientHandler();
    }
}
