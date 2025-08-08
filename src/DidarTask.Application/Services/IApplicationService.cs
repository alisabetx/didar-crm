namespace DidarTask.Application.Services;

/// <summary>
/// Minimal interface exposing availability of the Application service. This allows
/// other services to coordinate changes and roll them back if the application is
/// unreachable.
/// </summary>
public interface IApplicationService
{
    /// <summary>
    /// Returns true when the Application service is reachable.
    /// </summary>
    bool Ping();
}
