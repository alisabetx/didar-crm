using System;

namespace DidarTask.Application.Services;

/// <summary>
/// Provides a very small coordination mechanism that applies a local change and
/// verifies the remote service. If the verification fails or throws, the local
/// change is rolled back using the supplied rollback action.
/// </summary>
public static class TransactionCoordinator
{
    public static bool Execute(Action apply, Func<bool> remoteCheck, Action rollback)
    {
        try
        {
            apply();
            if (!remoteCheck())
            {
                rollback();
                return false;
            }

            return true;
        }
        catch
        {
            rollback();
            throw;
        }
    }
}
