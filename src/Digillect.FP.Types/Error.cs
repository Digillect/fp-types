namespace Digillect.FP.Types;

/// <summary>
/// Base error class.
/// </summary>
/// <param name="message">Error message.</param>
public abstract class Error(string message)
{
	/// <summary>
	/// Error message.
	/// </summary>
	public string Message { get; } = message;

	/// <summary>
	/// Creates a new instance of the <see cref="GenericError"/> using the provided error message.
	/// </summary>
	/// <param name="message">The error message associated with the error.</param>
	/// <returns>A <see cref="GenericError"/> object initialized with the specified message.</returns>
	public static GenericError Generic(string message) => new(message);
}
