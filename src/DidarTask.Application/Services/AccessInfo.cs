namespace DidarTask.Application.Services;

/// <summary>
/// Represents the subscription access information returned by the Packaging service.
/// </summary>
/// <param name="SubscriptionLevel">Human readable name of the user's plan.</param>
/// <param name="AllowedFeatures">Number of features the plan allows.</param>
public record AccessInfo(string SubscriptionLevel, int AllowedFeatures);
