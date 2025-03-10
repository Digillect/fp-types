namespace Digillect.FP.Types;

/// <summary>
/// Provides utility methods for functional programming constructs, such as creating results, options,
/// and working with unit type.
/// </summary>
public static class Prelude
{
	/// <summary>
	/// Gets the unique instance of the <see cref="Unit"/> type,
	/// representing a singular value that holds no meaningful data.
	/// </summary>
	/// <remarks>
	/// The <see cref="Unit"/> type is useful in scenarios where a function's return value
	/// or parameter does not convey any additional information or context.
	/// </remarks>
	// ReSharper disable once InconsistentNaming
	public static Unit unit => Unit.Default;

	/// <summary>
	/// Returns a successful result containing the specified value.
	/// </summary>
	/// <param name="value">The value to include in the successful result.</param>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <returns>A successful result containing the specified value.</returns>
	public static Result<T> Success<T>(T value)
	{
		return Result.Success(value);
	}

	/// <summary>
	/// Returns a failed result containing the specified error.
	/// </summary>
	/// <param name="error">The error object to include in the failed result.</param>
	/// <typeparam name="T">The type of the value associated with the result.</typeparam>
	/// <returns>A failed result containing the specified error.</returns>
	public static Result<T> Err<T>(Error error)
	{
		return Result.Error<T>(error);
	}

	/// <summary>
	/// Returns a failed result containing a <see cref="GenericError"/> with the specified message.
	/// </summary>
	/// <param name="message">The message describing the error.</param>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <returns>A failed result containing an error with the specified message.</returns>
	public static Result<T> Err<T>(string message)
	{
		return Result.Error<T>(Error.Generic(message));
	}

	/// <summary>
	/// Creates an option containing the specified value.
	/// </summary>
	/// <param name="value">The value to include in the option.</param>
	/// <typeparam name="T">The type of the value. Must be a non-nullable type.</typeparam>
	/// <returns>An option representing the specified value.</returns>
	public static Option<T> Some<T>(T value)
		where T : notnull
	{
		return Option<T>.Some(value);
	}

	/// <summary>
	/// Creates an option that represents an optional value.
	/// </summary>
	/// <param name="value">The value to wrap in an option, or <c>null</c> to create a None option.</param>
	/// <typeparam name="T">The type of the optional value.</typeparam>
	/// <returns>An option containing the specified value if it is not <c>null</c>, or a None option otherwise.</returns>
	public static Option<T> Optional<T>(T? value) where T : notnull
	{
		return value is not null
			? Option<T>.Some(value)
			: Option<T>.None;
	}

	/// <summary>
	/// Executes the specified action and returns a successful result if no exceptions occur.
	/// If an exception is thrown, captures the exception and returns it as an error.
	/// </summary>
	/// <param name="action">The action to execute.</param>
	/// <returns>A successful result if the action completes without exceptions, or an error with the captured exception otherwise.</returns>
	public static Result<Unit> WithExceptionHandling(Action action)
	{
		try
		{
			action();
		}
		catch (Exception ex)
		{
			return new Exceptional(ex);
		}

		return unit;
	}

	/// <summary>
	/// Executes the specified operation and encapsulates its result in a successful result. If an exception is thrown during the operation, captures the exception as an error in a failed result.
	/// </summary>
	/// <param name="action">The operation to execute.</param>
	/// <typeparam name="T">The type of the result value.</typeparam>
	/// <returns>A successful result if the operation completes without exceptions; otherwise, a failed result containing the captured exception.</returns>
	public static Result<T> WithExceptionHandling<T>(Func<T> action)
	{
		try
		{
			return action();
		}
		catch (Exception ex)
		{
			return new Exceptional(ex);
		}
	}

	/// <summary>
	/// Executes the specified asynchronous action with exception handling.
	/// </summary>
	/// <param name="action">The asynchronous action to execute.</param>
	/// <returns>A task that represents the result of the operation, containing a successful result if the action completes successfully, or an exceptional result if an exception is thrown during execution.</returns>
	public static async Task<Result<Unit>> WithExceptionHandlingAsync(Func<Task> action)
	{
		try
		{
			await action();
		}
		catch (Exception ex)
		{
			return new Exceptional(ex);
		}

		return unit;
	}

	/// <summary>
	/// Executes the specified asynchronous action with exception handling, accepting a cancellation token.
	/// </summary>
	/// <param name="action">The asynchronous action to execute, which accepts a cancellation token.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
	/// <returns>
	/// A task that represents the asynchronous operation, returning a successful result if the action completes without exceptions,
	/// or an exceptional result if an exception occurs during execution.
	/// </returns>
	/// <remarks>
	/// This method provides a way to safely execute asynchronous operations that can be canceled
	/// while automatically handling any exceptions that might occur during execution.
	/// </remarks>
	public static async Task<Result<Unit>> WithExceptionHandlingAsync(Func<CancellationToken, Task> action, CancellationToken cancellationToken = default)
	{
		try
		{
			await action(cancellationToken);
		}
		catch (Exception ex)
		{
			return new Exceptional(ex);
		}

		return unit;
	}

	/// <summary>
	/// Executes an asynchronous action with exception handling, returning an error result if an exception occurs.
	/// </summary>
	/// <param name="action">The asynchronous function to execute.</param>
	/// <typeparam name="T">The type of the result's value.</typeparam>
	/// <returns>A successful result containing the action's returned value, or an error result if an exception occurs.</returns>
	public static async Task<Result<T>> WithExceptionHandlingAsync<T>(Func<Task<T>> action)
	{
		try
		{
			return await action();
		}
		catch (Exception ex)
		{
			return new Exceptional(ex);
		}
	}

	/// <summary>
	/// Executes the provided asynchronous action with exception handling.
	/// </summary>
	/// <param name="action">The asynchronous action to execute, which accepts a cancellation token and returns a task with the result.</param>
	/// <param name="cancellationToken">The cancellation token used to cancel the asynchronous operation if required.</param>
	/// <typeparam name="T">The type of the result.</typeparam>
	/// <returns>A result containing the outcome of the action, or an exception if an error occurred.</returns>
	public static async Task<Result<T>> WithExceptionHandlingAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken cancellationToken = default)
	{
		try
		{
			return await action(cancellationToken);
		}
		catch (Exception ex)
		{
			return new Exceptional(ex);
		}
	}
}
