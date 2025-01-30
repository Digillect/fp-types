namespace Digillect.FP.Types;

/// <summary>
/// Provides extension methods for the <see cref="Result{T}"/> type that allow for asynchronous operations
/// and enhancements in functional programming style for result handling.
/// </summary>
public static class ResultExtensions
{
	/// <summary>
	/// Transforms the success value of the current asynchronous <see cref="Result{T}"/> instance using the specified
	/// mapping function <paramref name="map"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the original <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the success result in the resultant <see cref="Result{TResult}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="map">The mapping function to apply to the success value.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to a new <see cref="Result{TResult}"/> containing
	/// the mapped value if the original result is successful; otherwise, the original error is returned.
	/// </returns>
	public static async Task<Result<TResult>> Map<T, TResult>(this Task<Result<T>> result, Func<T, TResult> map)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.Map(map);
	}

	/// <summary>
	/// Transforms the success value of the current asynchronous <see cref="Result{T}"/> instance using the specified
	/// asynchronous mapping function <paramref name="map"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the original <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the success result in the resultant <see cref="Result{TResult}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="map">The asynchronous mapping function to apply to the success value.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to a new <see cref="Result{TResult}"/> containing
	/// the mapped value if the original result is successful; otherwise, the original error is returned.
	/// </returns>
	public static async Task<Result<TResult>> MapAsync<T, TResult>(this Task<Result<T>> result, Func<T, Task<TResult>> map)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.MapAsync(map).ConfigureAwait(false);
	}

	/// <summary>
	/// Chains the current asynchronous <see cref="Result{T}"/> instance to another <see cref="Result{TResult}"/> using
	/// the specified binding function <paramref name="bind"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the original <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the success result in the resultant <see cref="Result{TResult}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="bind">The binding function that returns a new <see cref="Result{TResult}"/> based on the success value.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to a new <see cref="Result{TResult}"/>. If the original result
	/// is a success, the binding function is applied to generate a new result. If the original result is a failure, the original error is returned.
	/// </returns>
	public static async Task<Result<TResult>> Bind<T, TResult>(this Task<Result<T>> result, Func<T, Result<TResult>> bind)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.Bind(bind);
	}

	/// <summary>
	/// Chains the current asynchronous <see cref="Result{T}"/> instance to another asynchronous <see cref="Result{TResult}"/> using
	/// the specified asynchronous binding function <paramref name="bind"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the original <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the success result in the resultant <see cref="Result{TResult}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="bind">The asynchronous binding function that returns a new <see cref="Result{TResult}"/> based on the success value.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to a new <see cref="Result{TResult}"/>. If the original result
	/// is a success, the asynchronous binding function is applied to generate a new result. If the original result is a failure, the original error is returned.
	/// </returns>
	public static async Task<Result<TResult>> BindAsync<T, TResult>(this Task<Result<T>> result, Func<T, Task<Result<TResult>>> bind)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.BindAsync(bind).ConfigureAwait(false);
	}

	/// <summary>
	/// Executes a side effect when the current asynchronous <see cref="Result{T}"/> instance contains a success value.
	/// Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the current <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="action">The action to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to the original <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async Task<Result<T>> Tap<T>(this Task<Result<T>> result, Action<T> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		awaitedResult.Tap(action);

		return awaitedResult;
	}

	/// <summary>
	/// Executes an asynchronous side effect when the current asynchronous <see cref="Result{T}"/> instance contains
	/// a success value. Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the current <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="action">The asynchronous action to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to the original <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> result, Func<T, Task> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.TapAsync(action).ConfigureAwait(false);
	}

	/// <summary>
	/// Executes an asynchronous side effect when the current asynchronous <see cref="Result{T}"/> instance contains
	/// a success value. Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the current <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="action">The asynchronous action to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to the original <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async Task<Result<T>> TapAsync<T>(this Task<Result<T>> result, Func<Task> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.TapAsync(action).ConfigureAwait(false);
	}

	/// <summary>
	/// Returns a new <see cref="Result{T}"/> with the specified alternative success value <paramref name="value"/>
	/// if the original asynchronous <see cref="Result{T}"/> contains an error.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="value">The alternative success value to return if the original result contains an error.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to a <see cref="Result{T}"/> with either the original success value
	/// or the provided alternative if the original result contains an error.
	/// </returns>
	public static async Task<Result<T>> OrElse<T>(this Task<Result<T>> result, T value)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.OrElse(value);
	}

	/// <summary>
	/// Returns a new <see cref="Result{T}"/> with an alternative success value generated by <paramref name="valueFactory"/>
	/// if the original asynchronous <see cref="Result{T}"/> contains an error.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="valueFactory">The factory function to generate an alternative success value if the original result contains an error.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to a <see cref="Result{T}"/> with either the original success value
	/// or the generated alternative.
	/// </returns>
	public static async Task<Result<T>> OrElse<T>(this Task<Result<T>> result, Func<T> valueFactory)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.OrElse(valueFactory);
	}

	/// <summary>
	/// Returns a new <see cref="Result{T}"/> asynchronously with an alternative success value generated by
	/// <paramref name="valueFactory"/> if the original asynchronous <see cref="Result{T}"/> contains an error.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="valueFactory">The asynchronous factory function to generate an alternative success value if the original
	/// result contains an error.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to a <see cref="Result{T}"/> with either the original success value
	/// or the generated alternative.
	/// </returns>
	public static async Task<Result<T>> OrElseAsync<T>(this Task<Result<T>> result, Func<Task<T>> valueFactory)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.OrElseAsync(valueFactory).ConfigureAwait(false);
	}

	/// <summary>
	/// Recovers from the error state of the current asynchronous <see cref="Result{T}"/> instance by providing a fallback success
	/// value using the specified recovery function <paramref name="map"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="map">The function to generate a fallback success value when the current result contains an error.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to a new <see cref="Result{T}"/>. If the original result is successful,
	/// the result is returned unchanged. If the original result is a failure, the recovery function generates a new success value.
	/// </returns>
	public static async Task<Result<T>> Recover<T>(this Task<Result<T>> result, Func<Error, T> map)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.Recover(map);
	}

	/// <summary>
	/// Recovers from the error state of the current asynchronous <see cref="Result{T}"/> instance by providing
	/// a fallback success value asynchronously through the specified recovery function <paramref name="map"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="map">
	/// The asynchronous recovery function to generate a fallback success value. This function is invoked if the current result
	/// contains an error.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to a new <see cref="Result{T}"/>. If the current result
	/// is successful, it is returned unchanged. If the current result contains an error, the recovery function generates
	/// a fallback success value asynchronously.
	/// </returns>
	public static async Task<Result<T>> RecoverAsync<T>(this Task<Result<T>> result, Func<Error, Task<T>> map)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.RecoverAsync(map).ConfigureAwait(false);
	}

	/// <summary>
	/// Recovers from the error state of the current asynchronous <see cref="Result{T}"/> instance by providing a new
	/// <see cref="Result{T}"/> using the specified recovery function <paramref name="map"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="map">The function to generate a new <see cref="Result{T}"/> when the current result contains an error.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to a new <see cref="Result{T}"/>. If the original result is successful,
	/// it is returned unchanged; otherwise, the recovery function generates a new result.
	/// </returns>
	public static async Task<Result<T>> RecoverWith<T>(this Task<Result<T>> result, Func<Error, Result<T>> map)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.RecoverWith(map);
	}

	/// <summary>
	/// Recovers from the error state of the current asynchronous <see cref="Result{T}"/> instance by providing another
	/// asynchronous <see cref="Result{T}"/> through the specified recovery function <paramref name="map"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="map">
	/// The asynchronous recovery function that generates a new <see cref="Result{T}"/> if the current result contains an error.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to a new <see cref="Result{T}"/>. If the current result is successful,
	/// it is returned unchanged. If the current result contains an error, the recovery function generates and returns another
	/// asynchronous <see cref="Result{T}"/>.
	/// </returns>
	public static async Task<Result<T>> RecoverWithAsync<T>(this Task<Result<T>> result, Func<Error, Task<Result<T>>> map)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.RecoverWithAsync(map).ConfigureAwait(false);
	}

	/// <summary>
	/// Executes the specified action <paramref name="action"/> if the current asynchronous <see cref="Result{T}"/>
	/// instance contains an error. Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="action">The action to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="Task"/> that, when awaited, evaluates to the current <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async Task<Result<T>> OnFailure<T>(this Task<Result<T>> result, Action<Error> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.OnFailure(action);
	}

	/// <summary>
	/// Executes the specified asynchronous action <paramref name="action"/> if the current asynchronous <see cref="Result{T}"/>
	/// instance contains an error. Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="action">The asynchronous action to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="Task"/> that, when awaited, evaluates to the current <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async Task<Result<T>> OnFailureAsync<T>(this Task<Result<T>> result, Func<Error, Task> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.OnFailureAsync(action).ConfigureAwait(false);
	}

	/// <summary>
	/// Executes the specified function <paramref name="onSuccess"/> if the current asynchronous <see cref="Result{T}"/>
	/// instance contains a success value, or <paramref name="onError"/> if it contains an error, and returns the produced result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the result produced by the functions.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="onSuccess">The function to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <param name="onError">The function to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> that, when awaited, evaluates to the result produced by either
	/// <paramref name="onSuccess"/> or <paramref name="onError"/>.
	/// </returns>
	public static async Task<Result<TResult>> Match<T, TResult>(this Task<Result<T>> result, Func<T, TResult> onSuccess, Func<Error, TResult> onError)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.Match(onSuccess, onError);
	}

	/// <summary>
	/// Asynchronously executes the specified function <paramref name="onSuccess"/> if the current asynchronous
	/// <see cref="Result{T}"/> instance contains a success value, or <paramref name="onError"/> if it contains an error,
	/// and returns the produced result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the result produced by the asynchronous functions.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="onSuccess">The asynchronous function to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <param name="onError">The asynchronous function to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="Task{TResult}"/> that, when awaited, evaluates to the result produced by either
	/// <paramref name="onSuccess"/> or <paramref name="onError"/>.
	/// </returns>
	public static async Task<TResult> MatchAsync<T, TResult>(
		this Task<Result<T>> result,
		Func<T, Task<TResult>> onSuccess,
		Func<Error, Task<TResult>> onError)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.MatchAsync(onSuccess, onError).ConfigureAwait(false);
	}

	/// <summary>
	/// Executes the specified action <paramref name="onSuccess"/> if the current asynchronous <see cref="Result{T}"/>
	/// instance contains a success value, or <paramref name="onError"/> if it contains an error. Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="onSuccess">The action to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <param name="onError">The action to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="Task"/> that, when awaited, evaluates to the current <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async Task<Result<T>> Switch<T>(this Task<Result<T>> result, Action<T> onSuccess, Action<Error> onError)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.Switch(onSuccess, onError);
	}

	/// <summary>
	/// Asynchronously executes the specified action <paramref name="onSuccess"/> if the current asynchronous
	/// <see cref="Result{T}"/> instance contains a success value, or <paramref name="onError"/> if it contains an error.
	/// Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="onSuccess">The asynchronous action to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <param name="onError">The asynchronous action to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="Task"/> that, when awaited, evaluates to the current <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async Task<Result<T>> SwitchAsync<T>(this Task<Result<T>> result, Func<T, Task> onSuccess, Func<Error, Task> onError)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.SwitchAsync(onSuccess, onError).ConfigureAwait(false);
	}

	/// <summary>
	/// Returns a failure result with the specified error <paramref name="error"/> if the current success value satisfies
	/// the given predicate <paramref name="predicate"/>; otherwise, returns the original result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="predicate">The predicate to evaluate on the success value of the result.</param>
	/// <param name="error">The error to return if the predicate evaluates to <c>true</c>.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to either the original result or a failure result.
	/// </returns>
	public static async Task<Result<T>> FailWhen<T>(this Task<Result<T>> result, Func<T, bool> predicate, Error error)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.FailWhen(predicate, error);
	}

	/// <summary>
	/// Returns a failure result when the specified predicate <paramref name="predicate"/> evaluates to <c>true</c>.
	/// Uses the provided factory <paramref name="errorFactory"/> to generate the error dynamically.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="predicate">The predicate to evaluate the success value of the result.</param>
	/// <param name="errorFactory">A factory function to dynamically generate an error if the predicate evaluates to <c>true</c>.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to either the original result or a failure result.
	/// </returns>
	public static async Task<Result<T>> FailWhenWith<T>(this Task<Result<T>> result, Func<T, bool> predicate, Func<Error> errorFactory)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.FailWhenWith(predicate, errorFactory);
	}

	/// <summary>
	/// Returns a failure result when the specified predicate <paramref name="predicate"/> evaluates to <c>true</c>.
	/// Uses the provided factory <paramref name="errorFactory"/> to generate the error dynamically.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="predicate">The predicate to evaluate the success value of the result.</param>
	/// <param name="errorFactory">A factory function to dynamically generate an error if the predicate evaluates to <c>true</c>.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to either the original result or a failure result.
	/// </returns>
	public static async Task<Result<T>> FailWhenWith<T>(this Task<Result<T>> result, Func<T, bool> predicate, Func<T, Error> errorFactory)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return awaitedResult.FailWhenWith(predicate, errorFactory);
	}

	/// <summary>
	/// Asynchronously returns a failure result with the error generated by <paramref name="errorFactory"/>
	/// if the success value satisfies the given predicate <paramref name="predicate"/>.
	/// Otherwise, returns the original result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="predicate">An asynchronous predicate to evaluate the success value of the result.</param>
	/// <param name="errorFactory">
	/// An asynchronous factory function that generates the error if the predicate evaluates to <c>true</c>.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to either the original result or a failure result.
	/// </returns>
	public static async Task<Result<T>> FailWhenWithAsync<T>(this Task<Result<T>> result, Func<T, bool> predicate, Func<Task<Error>> errorFactory)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.FailWhenWithAsync(predicate, errorFactory).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously returns a failure result with the error generated asynchronously by <paramref name="errorFactory"/>
	/// using the current success value if the predicate <paramref name="predicate"/> evaluates to <c>true</c>.
	/// Otherwise, returns the original result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="predicate">
	/// An asynchronous predicate to evaluate whether the failure condition is met.
	/// </param>
	/// <param name="errorFactory">
	/// An asynchronous factory function that uses the success value to generate the error if the predicate evaluates to <c>true</c>.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to either the original result or a failure result.
	/// </returns>
	public static async Task<Result<T>> FailWhenWithAsync<T>(this Task<Result<T>> result, Func<T, bool> predicate, Func<T, Task<Error>> errorFactory)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.FailWhenWithAsync(predicate, errorFactory).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously returns a failure result with the specified error <paramref name="error"/> if the success value satisfies
	/// the asynchronous predicate <paramref name="predicate"/>.
	/// Otherwise, returns the original result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="predicate">
	/// An asynchronous predicate to evaluate the success value of the result.
	/// </param>
	/// <param name="error">
	/// The error to return as the failure result if the predicate evaluates to <c>true</c>.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to either the original result or a failure result.
	/// </returns>
	public static async Task<Result<T>> FailWhenAsync<T>(this Task<Result<T>> result, Func<T, Task<bool>> predicate, Error error)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.FailWhenAsync(predicate, error).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously returns a failure result with the error generated by <paramref name="errorFactory"/>
	/// if the success value satisfies the asynchronous predicate <paramref name="predicate"/>.
	/// Otherwise, returns the original result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="predicate">An asynchronous predicate to evaluate the success value of the result.</param>
	/// <param name="errorFactory">
	/// A factory function that generates the error if the predicate evaluates to <c>true</c>.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to either the original result or a failure result.
	/// </returns>
	public static async Task<Result<T>> FailWhenWithAsync<T>(this Task<Result<T>> result, Func<T, Task<bool>> predicate, Func<Error> errorFactory)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.FailWhenWithAsync(predicate, errorFactory).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously returns a failure result with the error generated by <paramref name="errorFactory"/>
	/// that uses the current success value if the asynchronous predicate <paramref name="predicate"/> evaluates to <c>true</c>.
	/// Otherwise, returns the original result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="predicate">An asynchronous predicate to evaluate the success value of the result.</param>
	/// <param name="errorFactory">
	/// A factory function that uses the success value to generate the error if the predicate evaluates to <c>true</c>.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to either the original result or a failure result.
	/// </returns>
	public static async Task<Result<T>> FailWhenWithAsync<T>(this Task<Result<T>> result, Func<T, Task<bool>> predicate, Func<T, Error> errorFactory)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.FailWhenWithAsync(predicate, errorFactory).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously returns a failure result with the error generated by <paramref name="errorFactory"/>
	/// if the asynchronous predicate <paramref name="predicate"/> evaluates to <c>true</c>.
	/// Otherwise, returns the original result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="predicate">An asynchronous predicate to evaluate the success value of the result.</param>
	/// <param name="errorFactory">
	/// An asynchronous factory function that generates the error if the predicate evaluates to <c>true</c>.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to either the original result or a failure result.
	/// </returns>
	public static async Task<Result<T>> FailWhenWithAsync<T>(this Task<Result<T>> result, Func<T, Task<bool>> predicate, Func<Task<Error>> errorFactory)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.FailWhenWithAsync(predicate, errorFactory).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously returns a failure result with the error generated by <paramref name="errorFactory"/>
	/// that uses the current success value if the asynchronous predicate <paramref name="predicate"/> evaluates to <c>true</c>.
	/// Otherwise, returns the original result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="Task{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="predicate">An asynchronous predicate to evaluate the success value of the result.</param>
	/// <param name="errorFactory">
	/// An asynchronous factory function that uses the success value to generate the error if the predicate evaluates to <c>true</c>.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to either the original result or a failure result.
	/// </returns>
	public static async Task<Result<T>> FailWhenWithAsync<T>(this Task<Result<T>> result, Func<T, Task<bool>> predicate, Func<T, Task<Error>> errorFactory)
	{
		var awaitedResult = await result.ConfigureAwait(false);

		return await awaitedResult.FailWhenWithAsync(predicate, errorFactory).ConfigureAwait(false);
	}

	/// <summary>
	/// Wraps the given synchronous <see cref="Result{T}"/> into a <see cref="Task"/> to make it asynchronous.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The synchronous <see cref="Result{T}"/> to be wrapped as asynchronous.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to the same <see cref="Result{T}"/> instance.
	/// </returns>
	public static Task<Result<T>> ToAsync<T>(this Result<T> result)
	{
		return Task.FromResult(result);
	}

	/// <summary>
	/// Wraps the provided value into an asynchronous successful <see cref="Result{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="value">The value to be wrapped as a successful result.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to a successful <see cref="Result{T}"/> containing the provided value.
	/// </returns>
	public static Task<Result<T>> ToAsyncResult<T>(this T value)
		where T : notnull
	{
		return Task.FromResult(Result<T>.Success(value));
	}

	/// <summary>
	/// Wraps the specified nullable value into an asynchronous <see cref="Result{T}"/>.
	/// If the value is <c>null</c>, returns an asynchronous failure result with the provided error.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="value">The nullable value to wrap as a success result, or to return as an error if <c>null</c>.</param>
	/// <param name="notFoundError">The error to return if the supplied value is <c>null</c>.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to either a successful or failed <see cref="Result{T}"/>
	/// depending on the value.
	/// </returns>
	public static Task<Result<T>> ToAsyncResult<T>(this T? value, Error notFoundError)
		where T : notnull
	{
		return Task.FromResult(value is not null ? Result<T>.Success(value) : Result<T>.Failure(notFoundError));
	}

	/// <summary>
	/// Wraps the specified nullable value into an asynchronous <see cref="Result{T}"/>.
	/// If the value is <c>null</c>, the provided factory function <paramref name="notFoundError"/> is used to generate an error.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="value">The nullable value to wrap as a success result, or return a failure result if <c>null</c>.</param>
	/// <param name="notFoundError">A factory function to generate an error result if <paramref name="value"/> is <c>null</c>.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, evaluates to either a successful or failed <see cref="Result{T}"/>
	/// depending on the value.
	/// </returns>
	public static Task<Result<T>> ToAsyncResult<T>(this T? value, Func<Error> notFoundError)
		where T : notnull
	{
		return Task.FromResult(value is not null ? Result<T>.Success(value) : Result<T>.Failure(notFoundError()));
	}
}
