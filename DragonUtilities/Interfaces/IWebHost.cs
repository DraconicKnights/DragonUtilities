namespace DragonUtilities.Interfaces;

/// <summary>
/// Web Host Service
/// A service that manages an HTTP server to listen for and handle incoming requests.
/// </summary>
public interface IWebHost
{
    /// <summary>
    /// Starts the HTTP listener to begin accepting incoming requests.
    /// </summary>
    public void Start();

    /// <summary>
    /// Stops the HTTP listener to stop accepting incoming requests.
    /// </summary>
    public void Stop();

    /// <summary>
    /// Restarts the HTTP server by stopping and starting it again.
    /// </summary>
    public void Restart();
}