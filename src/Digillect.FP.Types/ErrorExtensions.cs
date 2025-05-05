namespace Digillect.FP.Types;

public static class ErrorExtensions
{
	/// <summary>
	/// Executes the specified handler action if the error is of the specified concrete type.
	/// </summary>
	/// <typeparam name="TConcreteError">The specific error type to check for.</typeparam>
	/// <param name="error">The error to check.</param>
	/// <param name="handler">The action to execute if the error matches the specified type.</param>
	/// <returns>The original error instance.</returns>
	public static Error When<TConcreteError>(this Error error, Action<TConcreteError> handler)
		where TConcreteError : Error
	{
		if (error is TConcreteError concreteError)
		{
			handler(concreteError);
		}

		return error;
	}

	/// <summary>
	/// Executes the specified handler action with the error.
	/// </summary>
	/// <param name="error">The error to pass to the handler.</param>
	/// <param name="handler">The action to execute with the error.</param>
	/// <returns>The original error instance.</returns>
	public static Error Else(this Error error, Action<Error> handler)
	{
		handler(error);

		return error;
	}
}
