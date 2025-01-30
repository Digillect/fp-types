namespace Digillect.FP.Types;

/// <summary>
/// An error reporting an unexpected exception.
/// </summary>
/// <param name="exception">The exception that caused the error.</param>
public sealed class Exceptional(Exception exception) : Error
{
	/// <summary>
	/// The exception that caused the error.
	/// </summary>
	public Exception Exception { get; } = exception;

	public override string ToString()
	{
		return Exception.Message;
	}
}
