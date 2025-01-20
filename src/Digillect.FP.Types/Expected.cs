namespace Digillect.FP.Types;

/// <summary>
/// Base class for expected errors.
/// </summary>
/// <param name="message">Error message.</param>
public abstract class Expected(string message) : Error(message);
