namespace Digillect.FP.Types;

/// <summary>
/// Base error class.
/// </summary>
public abstract class Error
{
	public override string ToString()
	{
		string message = GetType().Name;

		if (message.EndsWith("Error"))
		{
			message = message.Substring(0, message.Length - 5);
		}

		return message.Length > 0 ? message : "Unknown error";
	}

	/// <summary>
	/// Creates a new instance of the <see cref="GenericError"/> using the provided error message.
	/// </summary>
	/// <param name="message">The error message associated with the error.</param>
	/// <returns>A <see cref="GenericError"/> object initialized with the specified message.</returns>
	public static GenericError Generic(string message)
	{
		return new GenericError(message);
	}
}
