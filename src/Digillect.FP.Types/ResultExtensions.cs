using System.Runtime.CompilerServices;

namespace Digillect.FP.Types;

/// <summary>
/// Provides extension methods for the <see cref="Result{T}"/> type that allow for asynchronous operations
/// and enhancements in functional programming style for result handling.
/// </summary>
public static class ResultExtensions
{
	/// <summary>
	/// Applies a synchronous transformation function to the value of a successful <see cref="Result{T}"/>
	/// wrapped in a <see cref="Task{TResult}"/>, returning a new <see cref="Result{T}"/> with the transformed value,
	/// also wrapped in a <see cref="Task{TResult}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the input <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value to be produced by the transformation function.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to transform.</param>
	/// <param name="next">The synchronous transformation function to apply to the value of the successful result.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a <see cref="Result{TResult}"/> with the transformed value
	/// if the original result was successful, or the original error if the result was unsuccessful.
	/// </returns>
	public static async Task<Result<TResult>> Then<T, TResult>(this Task<Result<T>> result, Func<T, TResult> next)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.Then(next);
	}

	/// <summary>
	/// Applies an asynchronous transformation function to the value of a successful <see cref="Result{T}"/>
	/// wrapped in a <see cref="Task{TResult}"/>, returning a new <see cref="Result{T}"/> with the transformed value,
	/// also wrapped in a <see cref="Task{TResult}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the input <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value to be produced by the asynchronous transformation function.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to transform.</param>
	/// <param name="next">The asynchronous transformation function to apply to the value of the successful result.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a <see cref="Result{TResult}"/> with the transformed value
	/// if the original result was successful, or the original error if the result was unsuccessful.
	/// </returns>
	public static async Task<Result<TResult>> ThenAsync<T, TResult>(this Task<Result<T>> result, Func<T, Task<TResult>> next)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.ThenAsync(next).ConfigureAwait(false);
	}

	/// <summary>
	/// Applies an asynchronous transformation function that returns a <see cref="Result{T}"/> to the value
	/// of a successful <see cref="Result{T}"/> wrapped in a <see cref="Task{TResult}"/>, resulting in a new
	/// <see cref="Result{T}"/> with the transformed value, also wrapped in a <see cref="Task{TResult}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the input <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value to be produced by the transformation function.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to transform.</param>
	/// <param name="next">
	/// The asynchronous transformation function that takes the value of the successful result
	/// and produces a new <see cref="Result{T}"/>.
	/// </param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a <see cref="Result{TResult}"/> with the transformed value
	/// if the original result was successful, or the original error if the result was unsuccessful.
	/// </returns>
	public static async Task<Result<TResult>> ThenAsync<T, TResult>(this Task<Result<T>> result, Func<T, Result<TResult>> next)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.Then(next);
	}

	/// <summary>
	/// Applies an asynchronous transformation function to the value of a successful <see cref="Result{T}"/>
	/// wrapped in a <see cref="Task{TResult}"/>, returning a new <see cref="Result{T}"/> with the transformed value,
	/// also wrapped in a <see cref="Task{TResult}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the input <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value to be produced by the asynchronous transformation function.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to transform.</param>
	/// <param name="next">The asynchronous transformation function to apply to the value of the successful result.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a <see cref="Result{TResult}"/> with the transformed value
	/// if the original result was successful, or the original error if the result was unsuccessful.
	/// </returns>
	public static async Task<Result<TResult>> ThenAsync<T, TResult>(this Task<Result<T>> result, Func<T, Task<Result<TResult>>> next)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.ThenAsync(next).ConfigureAwait(false);
	}

	/// <summary>
	/// Executes the specified action on the value of a successful <see cref="Result{T}"/>
	/// wrapped in a <see cref="Task{TResult}"/>, returning the original <see cref="Result{T}"/>
	/// wrapped in a <see cref="Task{TResult}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to process.</param>
	/// <param name="action">The action to execute on the value of the successful result.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing the original <see cref="Result{T}"/> after executing the action,
	/// if the result was successful. If the result is unsuccessful, it is returned unchanged.
	/// </returns>
	public static async Task<Result<T>> Do<T>(this Task<Result<T>> result, Action<T> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		awaitedResult.Do(action);

		return awaitedResult;
	}

	/// <summary>
	/// Executes an asynchronous action on the successful result of a <see cref="Task{TResult}"/>
	/// containing a <see cref="Result{T}"/>, without altering the result value.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task containing the result to process.</param>
	/// <param name="action">An asynchronous action to execute if the result is successful.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing the original <see cref="Result{T}"/>
	/// after executing the provided action, or the original error if the result was unsuccessful.
	/// </returns>
	public static async Task<Result<T>> DoAsync<T>(this Task<Result<T>> result, Func<T, Task> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.DoAsync(action).ConfigureAwait(false);
	}

	/// <summary>
	/// Executes an asynchronous action if the task represents a successful <see cref="Result{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to process.</param>
	/// <param name="action">The asynchronous action to execute on the successful result.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing the original <see cref="Result{T}"/> if the action is executed,
	/// or the original error if the result is unsuccessful.
	/// </returns>
	public static async Task<Result<T>> DoAsync<T>(this Task<Result<T>> result, Func<Task> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.DoAsync(action).ConfigureAwait(false);
	}

	/// <summary>
	/// Provides an alternative value to the original result if it represents a failure,
	/// returning a successful <see cref="Result{T}"/> with the given value.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the original <see cref="Result{T}"/> to evaluate.</param>
	/// <param name="value">The value to use if the original result represents a failure.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a successful <see cref="Result{T}"/> with the provided value
	/// if the original result was unsuccessful, or the original result if it was successful.
	/// </returns>
	public static async Task<Result<T>> Else<T>(this Task<Result<T>> result, T value)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.Else(value);
	}

	/// <summary>
	/// Returns a new <see cref="Result{T}"/> with the provided fallback value
	/// if the original result was unsuccessful, maintaining its asynchronous context.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to evaluate.</param>
	/// <param name="value">A function returning the fallback value to use if the result is unsuccessful.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a <see cref="Result{T}"/> with the original value if successful,
	/// or the specified fallback value if unsuccessful.
	/// </returns>
	public static async Task<Result<T>> Else<T>(this Task<Result<T>> result, Func<T> value)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.Else(value);
	}

	/// <summary>
	/// Provides an alternative value using a mapping function when the asynchronous <see cref="Result{T}"/> is unsuccessful.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the result.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to process.</param>
	/// <param name="value">A function that maps the <see cref="Error"/> from the failed result to a new value.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a <see cref="Result{T}"/> that holds the original value
	/// if the result was successful, or the mapped value if the result was unsuccessful.
	/// </returns>
	public static async Task<Result<T>> Else<T>(this Task<Result<T>> result, Func<Error, T> value)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.Else(value);
	}

	/// <summary>
	/// Transforms the result of a failed asynchronous <see cref="Result{T}"/> using a mapping function, returning
	/// a new <see cref="Result{T}"/> provided by the mapping function, or the original successful result if it exists.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to be evaluated.</param>
	/// <param name="map">A function that accepts an <see cref="Error"/> from the failed result and returns a new <see cref="Result{T}"/>.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing either the original successful result or the transformed
	/// result provided by the mapping function if the original result was unsuccessful.
	/// </returns>
	public static async Task<Result<T>> Else<T>(this Task<Result<T>> result, Func<Error, Result<T>> map)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.Else(map);
	}

	/// <summary>
	/// Transforms the error of an unsuccessful <see cref="Result{T}"/> wrapped in a <see cref="Task{TResult}"/>
	/// using an asynchronous mapping function, returning a new <see cref="Result{T}"/> wrapped in a <see cref="Task{TResult}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to process.</param>
	/// <param name="map">The asynchronous mapping function to transform the error when the result is unsuccessful.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a <see cref="Result{T}"/>
	/// with the original value if the result was successful or a new value transformed from the error if the result was unsuccessful.
	/// </returns>
	public static async Task<Result<T>> ElseAsync<T>(this Task<Result<T>> result, Func<Error, Task<T>> map)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.ElseAsync(map).ConfigureAwait(false);
	}

	/// <summary>
	/// Executes the specified action when the <see cref="Result{T}"/> represents a failure,
	/// and returns the original result.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to evaluate.</param>
	/// <param name="action">The action to execute with the <see cref="Error"/> from the failed result.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> containing the same <see cref="Result{T}"/> instance passed as input.
	/// </returns>
	public static async Task<Result<T>> ElseDo<T>(this Task<Result<T>> result, Action<Error> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.ElseDo(action);
	}

	/// <summary>
	/// Executes an asynchronous action if the <see cref="Result{T}"/> encapsulates an error,
	/// returning the original result unchanged.
	/// </summary>
	/// <typeparam name="T">The type of the value contained in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to evaluate.</param>
	/// <param name="action">The asynchronous action to execute when the result contains an error.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing the original <see cref="Result{T}"/> after the action has been executed
	/// if an error is present, or the original successful result.
	/// </returns>
	public static async Task<Result<T>> ElseDoAsync<T>(this Task<Result<T>> result, Func<Error, Task> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.ElseDoAsync(action).ConfigureAwait(false);
	}

	/// <summary>
	/// Evaluates an asynchronous <see cref="Result{T}"/> and applies either the <paramref name="onSuccess"/> function
	/// to the successful value or the <paramref name="onFailure"/> function to the error, returning a value of type <typeparamref name="TResult"/>.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the value to be returned by the match operation.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to match.</param>
	/// <param name="onSuccess">The function to invoke with the successful value of the result.</param>
	/// <param name="onFailure">The function to invoke with the error if the result is unsuccessful.</param>
	/// <returns>
	/// A <typeparamref name="TResult"/> produced by invoking either the <paramref name="onSuccess"/> function if the result
	/// was successful, or the <paramref name="onFailure"/> function if the result was unsuccessful.
	/// </returns>
	public static async Task<Result<TResult>> Match<T, TResult>(this Task<Result<T>> result, Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.Match(onSuccess, onFailure);
	}

	/// <summary>
	/// Matches a <see cref="Result{T}"/> wrapped in a <see cref="Task{TResult}"/> asynchronously,
	/// by applying one of two asynchronous functions depending on whether the result is successful or unsuccessful.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the result produced by the provided asynchronous functions.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to match against.</param>
	/// <param name="onSuccess">The asynchronous function to apply to the value of a successful result.</param>
	/// <param name="onFailure">The asynchronous function to apply to the error of an unsuccessful result.</param>
	/// <returns>
	/// An asynchronous task that, upon completion, yields the result of the function applied either to the value
	/// of the successful result or to the error of the unsuccessful result.
	/// </returns>
	public static async Task<TResult> MatchAsync<T, TResult>(
		this Task<Result<T>> result,
		Func<T, Task<TResult>> onSuccess,
		Func<Error, Task<TResult>> onFailure)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.MatchAsync(onSuccess, onFailure).ConfigureAwait(false);
	}

	/// <summary>
	/// Executes the specified actions based on the state of the asynchronous <see cref="Result{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the value contained in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/>.</param>
	/// <param name="onSuccess">
	/// An action to execute if the <see cref="Result{T}"/> represents a successful state. The value contained in the result will be passed to this action.
	/// </param>
	/// <param name="onFailure">
	/// An action to execute if the <see cref="Result{T}"/> represents a failure state. The error representing the failure will be passed to this action.
	/// </param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing the original <see cref="Result{T}"/> after the appropriate action has been executed.
	/// </returns>
	public static async Task<Result<T>> Switch<T>(this Task<Result<T>> result, Action<T> onSuccess, Action<Error> onFailure)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.Switch(onSuccess, onFailure);
	}

	/// <summary>
	/// Executes asynchronous actions based on the outcome of a <see cref="Result{T}"/> wrapped in a <see cref="Task{TResult}"/>.
	/// If the result is successful, the <paramref name="onSuccess"/> action is invoked with the value.
	/// If the result is unsuccessful, the <paramref name="onFailure"/> action is invoked with the error.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to process.</param>
	/// <param name="onSuccess">The asynchronous action to execute when the result is successful, using the value of the successful result.</param>
	/// <param name="onFailure">The asynchronous action to execute when the result is unsuccessful, using the error of the failed result.</param>
	/// <returns>
	/// An asynchronous task containing the original <see cref="Result{T}"/> after the appropriate action has been executed.
	/// </returns>
	public static async Task<Result<T>> SwitchAsync<T>(this Task<Result<T>> result, Func<T, Task> onSuccess, Func<Error, Task> onFailure)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.SwitchAsync(onSuccess, onFailure).ConfigureAwait(false);
	}

	/// <summary>
	/// Evaluates a predicate against the value of a successful <see cref="Result{T}"/> wrapped in a <see cref="Task{TResult}"/>.
	/// If the predicate evaluates to true, returns a failed <see cref="Result{T}"/> with the specified error.
	/// Otherwise, returns the original result.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the input <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to be evaluated.</param>
	/// <param name="predicate">The synchronous function that evaluates the value of the successful result to determine if the operation should fail.</param>
	/// <param name="error">The error to return in case the predicate evaluates to true.</param>
	/// <returns>
	/// A task containing the original <see cref="Result{T}"/> if the predicate returns false, or a failed <see cref="Result{T}"/>
	/// with the specified error if the predicate returns true.
	/// </returns>
	public static async Task<Result<T>> FailIf<T>(this Task<Result<T>> result, Func<T, bool> predicate, Error error)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.FailIf(predicate, error);
	}

	/// <summary>
	/// Checks the value of a successful <see cref="Result{T}"/> wrapped in a <see cref="Task{TResult}"/>
	/// against a condition specified by the predicate. If the predicate evaluates to true, transforms the result into a failure
	/// by assigning the provided error generated by the specified function.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to evaluate.</param>
	/// <param name="predicate">The function that defines the condition to evaluate the value.</param>
	/// <param name="error">The function to generate the error to assign if the condition is true.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a <see cref="Result{T}"/>.
	/// If the predicate evaluates to true, the returned result will be a failure with the error produced by the factory function;
	/// otherwise, it will contain the original value.
	/// </returns>
	public static async Task<Result<T>> FailIf<T>(this Task<Result<T>> result, Func<T, bool> predicate, Func<Error> error)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.FailIf(predicate, error);
	}

	/// <summary>
	/// Applies a failure transformation to the result if the specified condition is met.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the input <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to evaluate.</param>
	/// <param name="predicate">The function that evaluates the value to determine whether the condition for failure is met.</param>
	/// <param name="error">The function that generates an <see cref="Error"/> based on the value that satisfies the predicate.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a <see cref="Result{T}"/>.
	/// If the predicate evaluates to true, the returned result will be a failure with the error produced by the factory function;
	/// otherwise, it will contain the original value.
	/// </returns>
	public static async Task<Result<T>> FailIf<T>(this Task<Result<T>> result, Func<T, bool> predicate, Func<T, Error> error)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.FailIf(predicate, error);
	}

	/// <summary>
	/// Creates a new <see cref="Result{T}"/> representing a failure if the given asynchronous predicate evaluates to true,
	/// using the specified asynchronous error factory function to produce the error object.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to evaluate.</param>
	/// <param name="predicate">The asynchronous function that determines whether the result value should trigger a failure.</param>
	/// <param name="error">The asynchronous function to produce the <see cref="Error"/> object when the predicate evaluates to true.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a <see cref="Result{T}"/>.
	/// If the predicate evaluates to true, the returned result will be a failure with the error produced by the factory function;
	/// otherwise, it will contain the original value.
	/// </returns>
	public static async Task<Result<T>> FailIfAsync<T>(this Task<Result<T>> result, Func<T, bool> predicate, Func<Task<Error>> error)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.FailIfAsync(predicate, error).ConfigureAwait(false);
	}

	/// <summary>
	/// Evaluates a condition on the value of a successful <see cref="Result{T}"/> wrapped in a <see cref="Task{TResult}"/>.
	/// If the condition is true, an asynchronous function generates an error to produce a failed result.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the input <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to evaluate.</param>
	/// <param name="predicate">A function that determines whether to fail the result, based on the value of the result.</param>
	/// <param name="error">An asynchronous function that generates an <see cref="Error"/> when the predicate evaluates to true.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a <see cref="Result{T}"/>.
	/// If the predicate evaluates to true, the returned result will be a failure with the error produced by the factory function;
	/// otherwise, it will contain the original value.
	/// </returns>
	public static async Task<Result<T>> FailIfAsync<T>(this Task<Result<T>> result, Func<T, bool> predicate, Func<T, Task<Error>> error)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.FailIfAsync(predicate, error).ConfigureAwait(false);
	}

	/// <summary>
	/// Conditionally transforms a successful <see cref="Result{T}"/> wrapped in a <see cref="Task{TResult}"/>
	/// into a failed result if an asynchronous predicate is satisfied. Returns the original result if the predicate is not satisfied.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to evaluate and potentially transform.</param>
	/// <param name="predicate">An asynchronous predicate function applied to the value of the successful result.
	/// If the predicate evaluates to true, the result is transformed into a failed result.</param>
	/// <param name="error">The error to associate with the failed result if the predicate evaluates to true.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing the original <see cref="Result{T}"/> if the predicate is not satisfied,
	/// or a failed <see cref="Result{T}"/> containing the specified error if the predicate evaluates to true.
	/// </returns>
	public static async Task<Result<T>> FailIfAsync<T>(this Task<Result<T>> result, Func<T, Task<bool>> predicate, Error error)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.FailIfAsync(predicate, error).ConfigureAwait(false);
	}

	/// <summary>
	/// Evaluates an asynchronous predicate on the value within a successful <see cref="Result{T}"/> wrapped in
	/// a <see cref="Task{TResult}"/>. If the predicate evaluates to true, it generates an error using the provided
	/// function and returns a failed <see cref="Result{T}"/>; otherwise, it returns the original result.
	/// </summary>
	/// <typeparam name="T">The type of the value contained within the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task containing the <see cref="Result{T}"/> to be evaluated.</param>
	/// <param name="predicate">
	/// A function that receives the value of the successful result and evaluates a <see cref="Task{TResult}"/>
	/// returning a <see cref="bool"/> to determine whether the error should be generated.
	/// </param>
	/// <param name="error">
	/// A function to generate an <see cref="Error"/> if the predicate evaluates to true.
	/// </param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a <see cref="Result{T}"/>.
	/// If the predicate evaluates to true, the returned result will be a failure with the error produced by the factory function;
	/// otherwise, it will contain the original value.
	/// </returns>
	public static async Task<Result<T>> FailIfAsync<T>(this Task<Result<T>> result, Func<T, Task<bool>> predicate, Func<Error> error)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.FailIfAsync(predicate, error).ConfigureAwait(false);
	}

	/// <summary>
	/// Applies an asynchronous failure condition to the value of a successful <see cref="Result{T}"/> wrapped in a <see cref="Task{TResult}"/>.
	/// If the condition specified by the predicate is met, a failure result is returned using the error provided by the function.
	/// Otherwise, the original successful result is returned.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to evaluate.</param>
	/// <param name="predicate">The asynchronous function to apply to the value of the successful result to determine if a failure should be triggered.</param>
	/// <param name="error">The function to generate the failure <see cref="Error"/> based on the value of the result.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a <see cref="Result{T}"/>.
	/// If the predicate evaluates to true, the returned result will be a failure with the error produced by the factory function;
	/// otherwise, it will contain the original value.
	/// </returns>
	public static async Task<Result<T>> FailIfAsync<T>(this Task<Result<T>> result, Func<T, Task<bool>> predicate, Func<T, Error> error)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.FailIfAsync(predicate, error).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously applies a failure condition to the value of a successful <see cref="Result{T}"/>,
	/// and returns a failed result if the predicate is satisfied, using the provided asynchronous error factory.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to evaluate.</param>
	/// <param name="predicate">An asynchronous function that evaluates the value to determine if the result should fail.</param>
	/// <param name="error">An asynchronous function that generates the <see cref="Error"/> if the predicate is satisfied.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a <see cref="Result{T}"/>.
	/// If the predicate evaluates to true, the returned result will be a failure with the error produced by the factory function;
	/// otherwise, it will contain the original value.
	/// </returns>
	public static async Task<Result<T>> FailIfAsync<T>(this Task<Result<T>> result, Func<T, Task<bool>> predicate, Func<Task<Error>> error)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.FailIfAsync(predicate, error).ConfigureAwait(false);
	}

	/// <summary>
	/// Evaluates an asynchronous predicate against the successful value of a <see cref="Result{T}"/> wrapped in a <see cref="Task{TResult}"/>.
	/// If the predicate evaluates to true, transforms the result into a failure with an asynchronous error factory.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous task representing the <see cref="Result{T}"/> to evaluate.</param>
	/// <param name="predicate">The asynchronous predicate to evaluate against the successful value of the result.</param>
	/// <param name="error">An asynchronous factory function to generate an <see cref="Error"/> if the predicate evaluates to true.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> containing a <see cref="Result{T}"/>.
	/// If the predicate evaluates to true, the returned result will be a failure with the error produced by the factory function;
	/// otherwise, it will contain the original value.
	/// </returns>
	public static async Task<Result<T>> FailIfAsync<T>(this Task<Result<T>> result, Func<T, Task<bool>> predicate, Func<T, Task<Error>> error)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.FailIfAsync(predicate, error).ConfigureAwait(false);
	}

	/// <summary>
	/// Converts a synchronous <see cref="Result{T}"/> instance into an asynchronous <see cref="Task{TResult}"/>
	/// representing the same result.
	/// </summary>
	/// <typeparam name="T">The type of the value held by the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The synchronous <see cref="Result{T}"/> to convert.</param>
	/// <returns>A <see cref="Task{TResult}"/> representing the provided <see cref="Result{T}"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<T>> ToAsync<T>(this Result<T> result)
		=> Task.FromResult(result);

	/// <summary>
	/// Converts a value of type <typeparamref name="T"/> into an asynchronous <see cref="Result{T}"/> instance representing success.
	/// </summary>
	/// <typeparam name="T">The type of the value to be encapsulated in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="value">The value to convert into a successful <see cref="Result{T}"/>.</param>
	/// <returns>A <see cref="Task{TResult}"/> containing a <see cref="Result{T}"/> that represents success.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<T>> ToAsyncResult<T>(this T value)
		where T : notnull
		=> Task.FromResult(Result<T>.Success(value));

	/// <summary>
	/// Converts a nullable value of type <typeparamref name="T"/> into an asynchronous <see cref="Task{TResult}"/>
	/// containing a <see cref="Result{T}"/>. If the value is not null, it succeeds with the value;
	/// otherwise, it fails with the provided <paramref name="notFoundError"/>.
	/// </summary>
	/// <typeparam name="T">The type of the value to wrap in a <see cref="Result{T}"/>.</typeparam>
	/// <param name="value">The nullable value to convert into a <see cref="Result{T}"/>.</param>
	/// <param name="notFoundError">The error instance to include in the result when the value is null.</param>
	/// <returns>A <see cref="Task{TResult}"/> containing the resulting <see cref="Result{T}"/> based on the provided value.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<T>> ToAsyncResult<T>(this T? value, Error notFoundError)
		where T : notnull
		=> Task.FromResult(value is not null ? Result<T>.Success(value) : Result<T>.Failure(notFoundError));

	/// <summary>
	/// Converts the specified value into an asynchronous <see cref="Result{T}"/> instance, where the result represents
	/// a success containing the provided value.
	/// </summary>
	/// <typeparam name="T">The type of the value to be converted into a <see cref="Result{T}"/>.</typeparam>
	/// <param name="value">The value to wrap in a successful <see cref="Result{T}"/>.</param>
	/// <param name="notFoundError">The function that returns an error instance to include in the result when the value is null.</param>
	/// <returns>A <see cref="Task{TResult}"/> representing a successful <see cref="Result{T}"/> containing the provided value.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Task<Result<T>> ToAsyncResult<T>(this T? value, Func<Error> notFoundError)
		where T : notnull
		=> Task.FromResult(value is not null ? Result<T>.Success(value) : Result<T>.Failure(notFoundError()));
}
