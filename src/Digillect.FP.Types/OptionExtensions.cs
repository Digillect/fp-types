namespace Digillect.FP.Types;

/// <summary>
/// Provides extension methods for the <see cref="Option{T}"/> type.
/// </summary>
public static class OptionExtensions
{
	/// <summary>
	/// Converts an <see cref="Option{T}"/> to a <see cref="Result{T}"/>, using the specified error if the option is None.
	/// </summary>
	/// <typeparam name="T">The type of the value contained in the option.</typeparam>
	/// <param name="option">The option to convert.</param>
	/// <param name="errorIfNone">The error to use if the option is None.</param>
	/// <returns>
	/// A <see cref="Result{T}"/> containing the option's value if it is Some; otherwise, a result with the specified error.
	/// </returns>
	public static Result<T> ToResult<T>(this Option<T> option, Error errorIfNone) where T : notnull
	{
		return option.Match(
			Result.Success,
			() => Result.Error<T>(errorIfNone));
	}

	/// <summary>
	/// Converts an <see cref="Option{T}"/> to a <see cref="Result{T}"/>, using the result of the specified function as the error if the option is None.
	/// </summary>
	/// <typeparam name="T">The type of the value contained in the option.</typeparam>
	/// <param name="option">The option to convert.</param>
	/// <param name="errorIfNone">A function that returns the error to use if the option is None.</param>
	/// <returns>
	/// A <see cref="Result{T}"/> containing the option's value if it is Some; otherwise, a result with the error returned by the function.
	/// </returns>
	public static Result<T> ToResult<T>(this Option<T> option, Func<Error> errorIfNone) where T : notnull
	{
		return option.Match(
			Result.Success,
			() => Result.Error<T>(errorIfNone()));
	}

	/// <summary>
	/// Converts a <see cref="Task{TResult}"/> containing an <see cref="Option{T}"/> to a <see cref="Task{TResult}"/> containing a <see cref="Result{T}"/>, using the specified error if the option is None.
	/// </summary>
	/// <typeparam name="T">The type of the value contained in the option.</typeparam>
	/// <param name="option">The task containing the option to convert.</param>
	/// <param name="errorIfNone">The error to use if the option is None.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a <see cref="Result{T}"/> with the option's value if it is Some; otherwise, a result with the specified error.
	/// </returns>
	public static async Task<Result<T>> ToResult<T>(this Task<Option<T>> option, Error errorIfNone) where T : notnull
	{
		return (await option).Match(
			Result.Success,
			() => Result.Error<T>(errorIfNone));
	}

	/// <summary>
	/// Converts a <see cref="Task{TResult}"/> containing an <see cref="Option{T}"/> to a <see cref="Task{TResult}"/> containing a <see cref="Result{T}"/>, using the result of the specified function as the error if the option is None.
	/// </summary>
	/// <typeparam name="T">The type of the value contained in the option.</typeparam>
	/// <param name="option">The task containing the option to convert.</param>
	/// <param name="errorIfNone">A function that returns the error to use if the option is None.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a <see cref="Result{T}"/> with the option's value if it is Some; otherwise, a result with the error returned by the function.
	/// </returns>
	public static async Task<Result<T>> ToResult<T>(this Task<Option<T>> option, Func<Error> errorIfNone) where T : notnull
	{
		return (await option).Match(
			Result.Success,
			() => Result.Error<T>(errorIfNone()));
	}

	/// <summary>
	/// Converts a <see cref="ValueTask{TResult}"/> containing an <see cref="Option{T}"/> to a <see cref="ValueTask{TResult}"/> containing a <see cref="Result{T}"/>, using the specified error if the option is None.
	/// </summary>
	/// <typeparam name="T">The type of the value contained in the option.</typeparam>
	/// <param name="option">The value task containing the option to convert.</param>
	/// <param name="errorIfNone">The error to use if the option is None.</param>
	/// <returns>
	/// A <see cref="ValueTask{TResult}"/> containing a <see cref="Result{T}"/> with the option's value if it is Some; otherwise, a result with the specified error.
	/// </returns>
	public static async ValueTask<Result<T>> ToResult<T>(this ValueTask<Option<T>> option, Error errorIfNone) where T : notnull
	{
		return (await option).Match(
			Result.Success,
			() => Result.Error<T>(errorIfNone));
	}

	/// <summary>
	/// Converts a <see cref="ValueTask{TResult}"/> containing an <see cref="Option{T}"/> to a <see cref="ValueTask{TResult}"/> containing a <see cref="Result{T}"/>, using the result of the specified function as the error if the option is None.
	/// </summary>
	/// <typeparam name="T">The type of the value contained in the option.</typeparam>
	/// <param name="option">The value task containing the option to convert.</param>
	/// <param name="errorIfNone">A function that returns the error to use if the option is None.</param>
	/// <returns>
	/// A <see cref="ValueTask{TResult}"/> containing a <see cref="Result{T}"/> with the option's value if it is Some; otherwise, a result with the error returned by the function.
	/// </returns>
	public static async ValueTask<Result<T>> ToResult<T>(this ValueTask<Option<T>> option, Func<Error> errorIfNone) where T : notnull
	{
		return (await option).Match(
			Result.Success,
			() => Result.Error<T>(errorIfNone()));
	}
}
