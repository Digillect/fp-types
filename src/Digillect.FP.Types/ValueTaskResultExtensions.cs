namespace Digillect.FP.Types;

/// <summary>
/// Provides extension methods for <see cref="ValueTask{TResult}"/> where TResult is <see cref="Result{T}"/>,
/// and cross-type extensions for <see cref="Result{T}"/> with <see cref="ValueTask"/> functions,
/// enabling comprehensive asynchronous functional programming operations.
/// </summary>
public static class ValueTaskResultExtensions
{
	/// <summary>
	/// Wraps the result of a <see cref="ValueTask{T}"/> into a successful <see cref="Result{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the value returned by the task.</typeparam>
	/// <param name="taskWithValue">The task that produces a value to wrap in a successful result.</param>
	/// <returns>
	/// A <see cref="ValueTask{TResult}"/> that, when awaited, returns a successful <see cref="Result{T}"/> containing the value from the original task.
	/// </returns>
	public static async ValueTask<Result<T>> ToResult<T>(this ValueTask<T> taskWithValue)
	{
		var value = await taskWithValue;
		return Result.Success(value);
	}

	/// <summary>
	/// Extracts the underlying success value from the current <see cref="ValueTask{TResult}"/> where TResult is <see cref="Result{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result contained in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="resultTask">The current <see cref="ValueTask{TResult}"/> instance.</param>
	/// <returns>
	/// A <see cref="ValueTask{TResult}"/> that represents the asynchronous operation,
	/// containing the success value of the <see cref="Result{T}"/> if it is successful; otherwise, an exception is thrown.
	/// </returns>
	public static async ValueTask<T> Unwrap<T>(this ValueTask<Result<T>> resultTask)
	{
		var result = await resultTask.ConfigureAwait(false);
		return result.Unwrap();
	}

	/// <summary>
	/// Transforms the success value of the current asynchronous <see cref="Result{T}"/> instance using the specified
	/// mapping function <paramref name="map"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the original <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the success result in the resultant <see cref="Result{TResult}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="map">The mapping function to apply to the success value.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a new <see cref="Result{TResult}"/> containing
	/// the mapped value if the original result is successful; otherwise, the original error is returned.
	/// </returns>
	public static async ValueTask<Result<TResult>> Map<T, TResult>(this ValueTask<Result<T>> result, Func<T, TResult> map)
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
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="map">The asynchronous mapping function to apply to the success value.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a new <see cref="Result{TResult}"/> containing
	/// the mapped value if the original result is successful; otherwise, the original error is returned.
	/// </returns>
	public static async ValueTask<Result<TResult>> MapAsync<T, TResult>(this ValueTask<Result<T>> result, Func<T, Task<TResult>> map)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return await awaitedResult.MapAsync(map).ConfigureAwait(false);
	}

	/// <summary>
	/// Transforms the success value of the current asynchronous <see cref="Result{T}"/> instance using the specified
	/// asynchronous mapping function that returns a <see cref="ValueTask{TResult}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the original <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the success result in the resultant <see cref="Result{TResult}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="map">The asynchronous mapping function that returns a <see cref="ValueTask{TResult}"/>.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a new <see cref="Result{TResult}"/> containing
	/// the mapped value if the original result is successful; otherwise, the original error is returned.
	/// </returns>
	public static async ValueTask<Result<TResult>> MapAsync<T, TResult>(this ValueTask<Result<T>> result, Func<T, ValueTask<TResult>> map)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		if (awaitedResult.IsError)
		{
			return Result.Error<TResult>(awaitedResult.Error);
		}

		var mappedValue = await map(awaitedResult.Value).ConfigureAwait(false);
		return Result.Success(mappedValue);
	}

	/// <summary>
	/// Chains the current asynchronous <see cref="Result{T}"/> instance to another <see cref="Result{TResult}"/> using
	/// the specified binding function <paramref name="bind"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the original <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the success result in the resultant <see cref="Result{TResult}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="bind">The binding function that returns a new <see cref="Result{TResult}"/> based on the success value.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a new <see cref="Result{TResult}"/>. If the original result
	/// is a success, the binding function is applied to generate a new result. If the original result is a failure, the original error is returned.
	/// </returns>
	public static async ValueTask<Result<TResult>> Bind<T, TResult>(this ValueTask<Result<T>> result, Func<T, Result<TResult>> bind)
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
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="bind">The asynchronous binding function that returns a new <see cref="Result{TResult}"/> based on the success value.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a new <see cref="Result{TResult}"/>. If the original result
	/// is a success, the asynchronous binding function is applied to generate a new result. If the original result is a failure, the original error is returned.
	/// </returns>
	public static async ValueTask<Result<TResult>> BindAsync<T, TResult>(this ValueTask<Result<T>> result, Func<T, Task<Result<TResult>>> bind)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return await awaitedResult.BindAsync(bind).ConfigureAwait(false);
	}

	/// <summary>
	/// Chains the current asynchronous <see cref="Result{T}"/> instance to another <see cref="Result{TResult}"/> using
	/// the specified asynchronous binding function that returns a <see cref="ValueTask{TResult}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the original <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the success result in the resultant <see cref="Result{TResult}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="bind">The asynchronous binding function that returns a <see cref="ValueTask"/> containing a new <see cref="Result{TResult}"/>.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a new <see cref="Result{TResult}"/>. If the original result
	/// is a success, the asynchronous binding function is applied to generate a new result. If the original result is a failure, the original error is returned.
	/// </returns>
	public static async ValueTask<Result<TResult>> BindAsync<T, TResult>(this ValueTask<Result<T>> result, Func<T, ValueTask<Result<TResult>>> bind)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		if (awaitedResult.IsError)
		{
			return Result.Error<TResult>(awaitedResult.Error);
		}

		return await bind(awaitedResult.Value).ConfigureAwait(false);
	}

	/// <summary>
	/// Executes a side effect when the current asynchronous <see cref="Result{T}"/> instance contains a success value.
	/// Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the current <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="action">The action to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to the original <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async ValueTask<Result<T>> Tap<T>(this ValueTask<Result<T>> result, Action<T> action)
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
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="action">The asynchronous action to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to the original <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async ValueTask<Result<T>> TapAsync<T>(this ValueTask<Result<T>> result, Func<T, Task> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return await awaitedResult.TapAsync(action).ConfigureAwait(false);
	}

	/// <summary>
	/// Executes an asynchronous side effect when the current asynchronous <see cref="Result{T}"/> instance contains
	/// a success value. Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the current <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="action">The asynchronous action to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to the original <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async ValueTask<Result<T>> TapAsync<T>(this ValueTask<Result<T>> result, Func<Task> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return await awaitedResult.TapAsync(action).ConfigureAwait(false);
	}

	/// <summary>
	/// Executes an asynchronous side effect using a <see cref="ValueTask"/> when the current asynchronous <see cref="Result{T}"/> instance contains
	/// a success value. Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the current <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="action">The asynchronous action that returns a <see cref="ValueTask"/> to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to the original <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async ValueTask<Result<T>> TapAsync<T>(this ValueTask<Result<T>> result, Func<T, ValueTask> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		if (awaitedResult.IsSuccess)
		{
			await action(awaitedResult.Value).ConfigureAwait(false);
		}
		return awaitedResult;
	}

	/// <summary>
	/// Executes an asynchronous side effect using a <see cref="ValueTask"/> when the current asynchronous <see cref="Result{T}"/> instance contains
	/// a success value. Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the current <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="action">The asynchronous action that returns a <see cref="ValueTask"/> to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to the original <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async ValueTask<Result<T>> TapAsync<T>(this ValueTask<Result<T>> result, Func<ValueTask> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		if (awaitedResult.IsSuccess)
		{
			await action().ConfigureAwait(false);
		}
		return awaitedResult;
	}

	/// <summary>
	/// Returns a new <see cref="Result{T}"/> with the specified alternative success value <paramref name="value"/>
	/// if the original asynchronous <see cref="Result{T}"/> contains an error.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="value">The alternative success value to return if the original result contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a <see cref="Result{T}"/> with either the original success value
	/// or the provided alternative if the original result contains an error.
	/// </returns>
	public static async ValueTask<Result<T>> OrElse<T>(this ValueTask<Result<T>> result, T value)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return awaitedResult.OrElse(value);
	}

	/// <summary>
	/// Returns a new <see cref="Result{T}"/> with an alternative success value generated by <paramref name="valueFactory"/>
	/// if the original asynchronous <see cref="Result{T}"/> contains an error.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="valueFactory">The factory function to generate an alternative success value if the original result contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a <see cref="Result{T}"/> with either the original success value
	/// or the generated alternative.
	/// </returns>
	public static async ValueTask<Result<T>> OrElse<T>(this ValueTask<Result<T>> result, Func<T> valueFactory)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return awaitedResult.OrElse(valueFactory);
	}

	/// <summary>
	/// Returns a new <see cref="Result{T}"/> asynchronously with an alternative success value generated by
	/// <paramref name="valueFactory"/> if the original asynchronous <see cref="Result{T}"/> contains an error.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="valueFactory">The asynchronous factory function to generate an alternative success value if the original
	/// result contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a <see cref="Result{T}"/> with either the original success value
	/// or the generated alternative.
	/// </returns>
	public static async ValueTask<Result<T>> OrElseAsync<T>(this ValueTask<Result<T>> result, Func<Task<T>> valueFactory)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return await awaitedResult.OrElseAsync(valueFactory).ConfigureAwait(false);
	}

	/// <summary>
	/// Returns a new <see cref="Result{T}"/> asynchronously with an alternative success value generated by
	/// <paramref name="valueFactory"/> that returns a <see cref="ValueTask{T}"/> if the original asynchronous <see cref="Result{T}"/> contains an error.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="valueFactory">The asynchronous factory function that returns a <see cref="ValueTask{T}"/> to generate an alternative success value if the original
	/// result contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a <see cref="Result{T}"/> with either the original success value
	/// or the generated alternative.
	/// </returns>
	public static async ValueTask<Result<T>> OrElseAsync<T>(this ValueTask<Result<T>> result, Func<ValueTask<T>> valueFactory)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		if (awaitedResult.IsSuccess)
		{
			return awaitedResult;
		}

		var value = await valueFactory().ConfigureAwait(false);
		return Result.Success(value);
	}

	/// <summary>
	/// Recovers from the error state of the current asynchronous <see cref="Result{T}"/> instance by providing a fallback success
	/// value using the specified recovery function <paramref name="map"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="map">The function to generate a fallback success value when the current result contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a new <see cref="Result{T}"/>. If the original result is successful,
	/// the result is returned unchanged. If the original result is a failure, the recovery function generates a new success value.
	/// </returns>
	public static async ValueTask<Result<T>> Recover<T>(this ValueTask<Result<T>> result, Func<Error, T> map)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return awaitedResult.Recover(map);
	}

	/// <summary>
	/// Recovers from the error state of the current asynchronous <see cref="Result{T}"/> instance by providing
	/// a fallback success value asynchronously through the specified recovery function <paramref name="map"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="map">
	/// The asynchronous recovery function to generate a fallback success value. This function is invoked if the current result
	/// contains an error.
	/// </param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a new <see cref="Result{T}"/>. If the current result
	/// is successful, it is returned unchanged. If the current result contains an error, the recovery function generates
	/// a fallback success value asynchronously.
	/// </returns>
	public static async ValueTask<Result<T>> RecoverAsync<T>(this ValueTask<Result<T>> result, Func<Error, Task<T>> map)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return await awaitedResult.RecoverAsync(map).ConfigureAwait(false);
	}

	/// <summary>
	/// Recovers from the error state of the current asynchronous <see cref="Result{T}"/> instance by providing
	/// a fallback success value asynchronously through the specified recovery function that returns a <see cref="ValueTask{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="map">
	/// The asynchronous recovery function that returns a <see cref="ValueTask{T}"/> to generate a fallback success value. This function is invoked if the current result
	/// contains an error.
	/// </param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a new <see cref="Result{T}"/>. If the current result
	/// is successful, it is returned unchanged. If the current result contains an error, the recovery function generates
	/// a fallback success value asynchronously.
	/// </returns>
	public static async ValueTask<Result<T>> RecoverAsync<T>(this ValueTask<Result<T>> result, Func<Error, ValueTask<T>> map)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		if (awaitedResult.IsSuccess)
		{
			return awaitedResult;
		}

		var recoveredValue = await map(awaitedResult.Error).ConfigureAwait(false);
		return Result.Success(recoveredValue);
	}

	/// <summary>
	/// Recovers from the error state of the current asynchronous <see cref="Result{T}"/> instance by providing a new
	/// <see cref="Result{T}"/> using the specified recovery function <paramref name="map"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="map">The function to generate a new <see cref="Result{T}"/> when the current result contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a new <see cref="Result{T}"/>. If the original result is successful,
	/// it is returned unchanged; otherwise, the recovery function generates a new result.
	/// </returns>
	public static async ValueTask<Result<T>> RecoverWith<T>(this ValueTask<Result<T>> result, Func<Error, Result<T>> map)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return awaitedResult.RecoverWith(map);
	}

	/// <summary>
	/// Recovers from the error state of the current asynchronous <see cref="Result{T}"/> instance by providing another
	/// asynchronous <see cref="Result{T}"/> through the specified recovery function <paramref name="map"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="map">
	/// The asynchronous recovery function that generates a new <see cref="Result{T}"/> if the current result contains an error.
	/// </param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a new <see cref="Result{T}"/>. If the current result is successful,
	/// it is returned unchanged. If the current result contains an error, the recovery function generates and returns another
	/// asynchronous <see cref="Result{T}"/>.
	/// </returns>
	public static async ValueTask<Result<T>> RecoverWithAsync<T>(this ValueTask<Result<T>> result, Func<Error, Task<Result<T>>> map)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return await awaitedResult.RecoverWithAsync(map).ConfigureAwait(false);
	}

	/// <summary>
	/// Recovers from the error state of the current asynchronous <see cref="Result{T}"/> instance by providing another
	/// <see cref="Result{T}"/> through the specified recovery function that returns a <see cref="ValueTask"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="map">
	/// The asynchronous recovery function that returns a <see cref="ValueTask"/> containing a new <see cref="Result{T}"/> if the current result contains an error.
	/// </param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a new <see cref="Result{T}"/>. If the current result is successful,
	/// it is returned unchanged. If the current result contains an error, the recovery function generates and returns another
	/// <see cref="Result{T}"/> asynchronously.
	/// </returns>
	public static async ValueTask<Result<T>> RecoverWithAsync<T>(this ValueTask<Result<T>> result, Func<Error, ValueTask<Result<T>>> map)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		if (awaitedResult.IsSuccess)
		{
			return awaitedResult;
		}

		return await map(awaitedResult.Error).ConfigureAwait(false);
	}

	/// <summary>
	/// Executes the specified action <paramref name="action"/> if the current asynchronous <see cref="Result{T}"/>
	/// instance contains an error. Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="action">The action to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask"/> that, when awaited, evaluates to the current <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async ValueTask<Result<T>> OnError<T>(this ValueTask<Result<T>> result, Action<Error> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return awaitedResult.OnError(action);
	}

	/// <summary>
	/// Executes the specified asynchronous action <paramref name="action"/> if the current asynchronous <see cref="Result{T}"/>
	/// instance contains an error. Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="action">The asynchronous action to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask"/> that, when awaited, evaluates to the current <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async ValueTask<Result<T>> OnErrorAsync<T>(this ValueTask<Result<T>> result, Func<Error, Task> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return await awaitedResult.OnErrorAsync(action).ConfigureAwait(false);
	}

	/// <summary>
	/// Executes the specified asynchronous action if the current asynchronous <see cref="Result{T}"/>
	/// instance contains an error. Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="action">The asynchronous action to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask"/> that, when awaited, evaluates to the current <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async ValueTask<Result<T>> OnErrorAsync<T>(this ValueTask<Result<T>> result, Func<Task> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return await awaitedResult.OnErrorAsync(action).ConfigureAwait(false);
	}

	/// <summary>
	/// Executes the specified asynchronous action that returns a <see cref="ValueTask"/> if the current asynchronous <see cref="Result{T}"/>
	/// instance contains an error. Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="action">The asynchronous action that returns a <see cref="ValueTask"/> to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask"/> that, when awaited, evaluates to the current <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async ValueTask<Result<T>> OnErrorAsync<T>(this ValueTask<Result<T>> result, Func<Error, ValueTask> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		if (awaitedResult.IsError)
		{
			await action(awaitedResult.Error).ConfigureAwait(false);
		}
		return awaitedResult;
	}

	/// <summary>
	/// Executes the specified asynchronous action that returns a <see cref="ValueTask"/> if the current asynchronous <see cref="Result{T}"/>
	/// instance contains an error. Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="action">The asynchronous action that returns a <see cref="ValueTask"/> to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask"/> that, when awaited, evaluates to the current <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async ValueTask<Result<T>> OnErrorAsync<T>(this ValueTask<Result<T>> result, Func<ValueTask> action)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		if (awaitedResult.IsError)
		{
			await action().ConfigureAwait(false);
		}
		return awaitedResult;
	}

	/// <summary>
	/// Handles an error of the specified type if the result contains an error of that type.
	/// </summary>
	/// <typeparam name="T">The type of the result value.</typeparam>
	/// <typeparam name="TError">The type of error to handle.</typeparam>
	/// <param name="result">The <see cref="ValueTask"/> returning the result to process.</param>
	/// <param name="handler">The handler for the specified error type.</param>
	/// <returns>A <see cref="ValueTask"/> returning the original result.</returns>
	public static async ValueTask<Result<T>> OnErrorOfType<T, TError>(this ValueTask<Result<T>> result, Action<TError> handler)
		where TError : Error
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return awaitedResult.OnErrorOfType(handler);
	}

	/// <summary>
	/// Handles an error of the specified type if the result contains an error of that type.
	/// </summary>
	/// <typeparam name="T">The type of the result value.</typeparam>
	/// <typeparam name="TError">The type of error to handle.</typeparam>
	/// <param name="result">The <see cref="ValueTask"/> returning the result to process.</param>
	/// <param name="handler">The asynchronous handler for the specified error type.</param>
	/// <returns>A <see cref="ValueTask"/> returning the original result.</returns>
	public static async ValueTask<Result<T>> OnErrorOfTypeAsync<T, TError>(this ValueTask<Result<T>> result, Func<TError, Task> handler)
		where TError : Error
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return await awaitedResult.OnErrorOfTypeAsync(handler).ConfigureAwait(false);
	}

	/// <summary>
	/// Handles an error of the specified type if the result contains an error of that type using a <see cref="ValueTask"/>.
	/// </summary>
	/// <typeparam name="T">The type of the result value.</typeparam>
	/// <typeparam name="TError">The type of error to handle.</typeparam>
	/// <param name="result">The <see cref="ValueTask"/> returning the result to process.</param>
	/// <param name="handler">The asynchronous handler that returns a <see cref="ValueTask"/> for the specified error type.</param>
	/// <returns>A <see cref="ValueTask"/> returning the original result.</returns>
	public static async ValueTask<Result<T>> OnErrorOfTypeAsync<T, TError>(this ValueTask<Result<T>> result, Func<TError, ValueTask> handler)
		where TError : Error
	{
		var awaitedResult = await result.ConfigureAwait(false);
		if (awaitedResult is { IsError: true, Error: TError specificError })
		{
			await handler(specificError).ConfigureAwait(false);
		}
		return awaitedResult;
	}

	/// <summary>
	/// Executes the specified function <paramref name="onSuccess"/> if the current asynchronous <see cref="Result{T}"/>
	/// instance contains a success value, or <paramref name="onError"/> if it contains an error, and returns the produced result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the result produced by the functions.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="onSuccess">The function to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <param name="onError">The function to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask{TResult}"/> that, when awaited, evaluates to the result produced by either
	/// <paramref name="onSuccess"/> or <paramref name="onError"/>.
	/// </returns>
	public static async ValueTask<TResult> Match<T, TResult>(this ValueTask<Result<T>> result, Func<T, TResult> onSuccess, Func<Error, TResult> onError)
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
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="onSuccess">The asynchronous function to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <param name="onError">The asynchronous function to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask{TResult}"/> that, when awaited, evaluates to the result produced by either
	/// <paramref name="onSuccess"/> or <paramref name="onError"/>.
	/// </returns>
	public static async ValueTask<TResult> MatchAsync<T, TResult>(
		this ValueTask<Result<T>> result,
		Func<T, Task<TResult>> onSuccess,
		Func<Error, Task<TResult>> onError)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return await awaitedResult.MatchAsync(onSuccess, onError).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously executes the specified function using <see cref="ValueTask"/> if the current asynchronous
	/// <see cref="Result{T}"/> instance contains a success value, or <paramref name="onError"/> if it contains an error,
	/// and returns the produced result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the result produced by the asynchronous functions.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="onSuccess">The asynchronous function that returns a <see cref="ValueTask{TResult}"/> to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <param name="onError">The asynchronous function that returns a <see cref="ValueTask{TResult}"/> to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask{TResult}"/> that, when awaited, evaluates to the result produced by either
	/// <paramref name="onSuccess"/> or <paramref name="onError"/>.
	/// </returns>
	public static async ValueTask<TResult> MatchAsync<T, TResult>(
		this ValueTask<Result<T>> result,
		Func<T, ValueTask<TResult>> onSuccess,
		Func<Error, ValueTask<TResult>> onError)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return awaitedResult.IsSuccess
			? await onSuccess(awaitedResult.Value).ConfigureAwait(false)
			: await onError(awaitedResult.Error).ConfigureAwait(false);
	}

	/// <summary>
	/// Executes the specified action <paramref name="onSuccess"/> if the current asynchronous <see cref="Result{T}"/>
	/// instance contains a success value, or <paramref name="onError"/> if it contains an error. Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="onSuccess">The action to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <param name="onError">The action to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask"/> that, when awaited, evaluates to the current <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async ValueTask<Result<T>> Switch<T>(this ValueTask<Result<T>> result, Action<T> onSuccess, Action<Error> onError)
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
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="onSuccess">The asynchronous action to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <param name="onError">The asynchronous action to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask"/> that, when awaited, evaluates to the current <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async ValueTask<Result<T>> SwitchAsync<T>(this ValueTask<Result<T>> result, Func<T, Task> onSuccess, Func<Error, Task> onError)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return await awaitedResult.SwitchAsync(onSuccess, onError).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously executes the specified action using <see cref="ValueTask"/> if the current asynchronous
	/// <see cref="Result{T}"/> instance contains a success value, or <paramref name="onError"/> if it contains an error.
	/// Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{T}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="onSuccess">The asynchronous action that returns a <see cref="ValueTask"/> to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <param name="onError">The asynchronous action that returns a <see cref="ValueTask"/> to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask"/> that, when awaited, evaluates to the current <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async ValueTask<Result<T>> SwitchAsync<T>(this ValueTask<Result<T>> result, Func<T, ValueTask> onSuccess, Func<Error, ValueTask> onError)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		if (awaitedResult.IsSuccess)
		{
			await onSuccess(awaitedResult.Value).ConfigureAwait(false);
		}
		else
		{
			await onError(awaitedResult.Error).ConfigureAwait(false);
		}
		return awaitedResult;
	}

	/// <summary>
	/// Transforms the successful value of the asynchronous result using the specified mapping function.
	/// This method provides LINQ query syntax support for <see cref="ValueTask{TResult}"/> where TResult is <see cref="Result{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the original success value.</typeparam>
	/// <typeparam name="TResult">The type of the transformed value.</typeparam>
	/// <param name="result">The asynchronous <see cref="ValueTask{TResult}"/> that represents the current <see cref="Result{T}"/>.</param>
	/// <param name="selector">The function to apply to the successful value.</param>
	/// <returns>A new asynchronous result with the transformed value if the original result was successful; otherwise, the original error.</returns>
	public static async ValueTask<Result<TResult>> Select<T, TResult>(this ValueTask<Result<T>> result, Func<T, TResult> selector)
	{
		var awaitedResult = await result.ConfigureAwait(false);
		return awaitedResult.Select(selector);
	}

	/// <summary>
	/// Wraps the given synchronous <see cref="Result{T}"/> into a <see cref="ValueTask"/> to make it asynchronous.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The synchronous <see cref="Result{T}"/> to be wrapped as asynchronous.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to the same <see cref="Result{T}"/> instance.
	/// </returns>
	public static ValueTask<Result<T>> ToValueTask<T>(this Result<T> result)
	{
		return new ValueTask<Result<T>>(result);
	}

	/// <summary>
	/// Wraps the provided value into an asynchronous successful <see cref="Result{T}"/> using <see cref="ValueTask"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="value">The value to be wrapped as a successful result.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a successful <see cref="Result{T}"/> containing the provided value.
	/// </returns>
	public static ValueTask<Result<T>> ToValueTaskResult<T>(this T value)
		where T : notnull
	{
		return new ValueTask<Result<T>>(Result.Success(value));
	}

	/// <summary>
	/// Wraps the specified nullable value into an asynchronous <see cref="Result{T}"/> using <see cref="ValueTask"/>.
	/// If the value is <c>null</c>, returns an asynchronous failure result with the provided error.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="value">The nullable value to wrap as a success result, or to return as an error if <c>null</c>.</param>
	/// <param name="notFoundError">The error to return if the supplied value is <c>null</c>.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to either a successful or failed <see cref="Result{T}"/>
	/// depending on the value.
	/// </returns>
	public static ValueTask<Result<T>> ToValueTaskResult<T>(this T? value, Error notFoundError)
		where T : notnull
	{
		return new ValueTask<Result<T>>(value is not null ? Result.Success(value) : Result.Error<T>(notFoundError));
	}

	/// <summary>
	/// Wraps the specified nullable value into an asynchronous <see cref="Result{T}"/> using <see cref="ValueTask"/>.
	/// If the value is <c>null</c>, the provided factory function <paramref name="notFoundError"/> is used to generate an error.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="value">The nullable value to wrap as a success result, or return a failure result if <c>null</c>.</param>
	/// <param name="notFoundError">A factory function to generate an error result if <paramref name="value"/> is <c>null</c>.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to either a successful or failed <see cref="Result{T}"/>
	/// depending on the value.
	/// </returns>
	public static ValueTask<Result<T>> ToValueTaskResult<T>(this T? value, Func<Error> notFoundError)
		where T : notnull
	{
		return new ValueTask<Result<T>>(value is not null ? Result.Success(value) : Result.Error<T>(notFoundError()));
	}

	/// <summary>
	/// Transforms the success value of the current <see cref="Result{T}"/> instance using the specified
	/// asynchronous mapping function that returns a <see cref="ValueTask{TResult}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the original <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the success result in the resultant <see cref="Result{TResult}"/>.</typeparam>
	/// <param name="result">The <see cref="Result{T}"/> instance.</param>
	/// <param name="map">The asynchronous mapping function that returns a <see cref="ValueTask{TResult}"/>.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a new <see cref="Result{TResult}"/> containing
	/// the mapped value if the original result is successful; otherwise, the original error is returned.
	/// </returns>
	public static async ValueTask<Result<TResult>> MapAsync<T, TResult>(this Result<T> result, Func<T, ValueTask<TResult>> map)
	{
		if (result.IsError)
		{
			return Result.Error<TResult>(result.Error);
		}

		var mappedValue = await map(result.Value).ConfigureAwait(false);
		return Result.Success(mappedValue);
	}

	/// <summary>
	/// Chains the current <see cref="Result{T}"/> instance to another <see cref="Result{TResult}"/> using
	/// the specified asynchronous binding function that returns a <see cref="ValueTask{TResult}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the original <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the success result in the resultant <see cref="Result{TResult}"/>.</typeparam>
	/// <param name="result">The <see cref="Result{T}"/> instance.</param>
	/// <param name="bind">The asynchronous binding function that returns a <see cref="ValueTask"/> containing a new <see cref="Result{TResult}"/>.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a new <see cref="Result{TResult}"/>. If the original result
	/// is a success, the asynchronous binding function is applied to generate a new result. If the original result is a failure, the original error is returned.
	/// </returns>
	public static async ValueTask<Result<TResult>> BindAsync<T, TResult>(this Result<T> result, Func<T, ValueTask<Result<TResult>>> bind)
	{
		if (result.IsError)
		{
			return Result.Error<TResult>(result.Error);
		}

		return await bind(result.Value).ConfigureAwait(false);
	}

	/// <summary>
	/// Executes an asynchronous side effect using a <see cref="ValueTask"/> when the current <see cref="Result{T}"/> instance contains
	/// a success value. Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the current <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The <see cref="Result{T}"/> instance.</param>
	/// <param name="action">The asynchronous action that returns a <see cref="ValueTask"/> to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to the original <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async ValueTask<Result<T>> TapAsync<T>(this Result<T> result, Func<T, ValueTask> action)
	{
		if (result.IsSuccess)
		{
			await action(result.Value).ConfigureAwait(false);
		}
		return result;
	}

	/// <summary>
	/// Executes an asynchronous side effect using a <see cref="ValueTask"/> when the current <see cref="Result{T}"/> instance contains
	/// a success value. Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the current <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The <see cref="Result{T}"/> instance.</param>
	/// <param name="action">The asynchronous action that returns a <see cref="ValueTask"/> to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to the original <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async ValueTask<Result<T>> TapAsync<T>(this Result<T> result, Func<ValueTask> action)
	{
		if (result.IsSuccess)
		{
			await action().ConfigureAwait(false);
		}
		return result;
	}

	/// <summary>
	/// Returns a new <see cref="Result{T}"/> asynchronously with an alternative success value generated by
	/// <paramref name="valueFactory"/> that returns a <see cref="ValueTask{T}"/> if the original <see cref="Result{T}"/> contains an error.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The <see cref="Result{T}"/> instance.</param>
	/// <param name="valueFactory">The asynchronous factory function that returns a <see cref="ValueTask{T}"/> to generate an alternative success value if the original
	/// result contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a <see cref="Result{T}"/> with either the original success value
	/// or the generated alternative.
	/// </returns>
	public static async ValueTask<Result<T>> OrElseAsync<T>(this Result<T> result, Func<ValueTask<T>> valueFactory)
	{
		if (result.IsSuccess)
		{
			return result;
		}

		var value = await valueFactory().ConfigureAwait(false);
		return Result.Success(value);
	}

	/// <summary>
	/// Recovers from the error state of the current <see cref="Result{T}"/> instance by providing
	/// a fallback success value asynchronously through the specified recovery function that returns a <see cref="ValueTask{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The <see cref="Result{T}"/> instance.</param>
	/// <param name="map">
	/// The asynchronous recovery function that returns a <see cref="ValueTask{T}"/> to generate a fallback success value. This function is invoked if the current result
	/// contains an error.
	/// </param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a new <see cref="Result{T}"/>. If the current result
	/// is successful, it is returned unchanged. If the current result contains an error, the recovery function generates
	/// a fallback success value asynchronously.
	/// </returns>
	public static async ValueTask<Result<T>> RecoverAsync<T>(this Result<T> result, Func<Error, ValueTask<T>> map)
	{
		if (result.IsSuccess)
		{
			return result;
		}

		var recoveredValue = await map(result.Error).ConfigureAwait(false);
		return Result.Success(recoveredValue);
	}

	/// <summary>
	/// Recovers from the error state of the current <see cref="Result{T}"/> instance by providing another
	/// <see cref="Result{T}"/> through the specified recovery function that returns a <see cref="ValueTask"/>.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The <see cref="Result{T}"/> instance.</param>
	/// <param name="map">
	/// The asynchronous recovery function that returns a <see cref="ValueTask"/> containing a new <see cref="Result{T}"/> if the current result contains an error.
	/// </param>
	/// <returns>
	/// A <see cref="ValueTask{T}"/> that, when awaited, evaluates to a new <see cref="Result{T}"/>. If the current result is successful,
	/// it is returned unchanged. If the current result contains an error, the recovery function generates and returns another
	/// <see cref="Result{T}"/> asynchronously.
	/// </returns>
	public static async ValueTask<Result<T>> RecoverWithAsync<T>(this Result<T> result, Func<Error, ValueTask<Result<T>>> map)
	{
		if (result.IsSuccess)
		{
			return result;
		}

		return await map(result.Error).ConfigureAwait(false);
	}

	/// <summary>
	/// Executes the specified asynchronous action that returns a <see cref="ValueTask"/> if the current <see cref="Result{T}"/>
	/// instance contains an error. Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The <see cref="Result{T}"/> instance.</param>
	/// <param name="action">The asynchronous action that returns a <see cref="ValueTask"/> to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask"/> that, when awaited, evaluates to the current <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async ValueTask<Result<T>> OnErrorAsync<T>(this Result<T> result, Func<Error, ValueTask> action)
	{
		if (result.IsError)
		{
			await action(result.Error).ConfigureAwait(false);
		}
		return result;
	}

	/// <summary>
	/// Executes the specified asynchronous action that returns a <see cref="ValueTask"/> if the current <see cref="Result{T}"/>
	/// instance contains an error. Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The <see cref="Result{T}"/> instance.</param>
	/// <param name="action">The asynchronous action that returns a <see cref="ValueTask"/> to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask"/> that, when awaited, evaluates to the current <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async ValueTask<Result<T>> OnErrorAsync<T>(this Result<T> result, Func<ValueTask> action)
	{
		if (result.IsError)
		{
			await action().ConfigureAwait(false);
		}
		return result;
	}

	/// <summary>
	/// Handles an error of the specified type if the result contains an error of that type using a <see cref="ValueTask"/>.
	/// </summary>
	/// <typeparam name="T">The type of the result value.</typeparam>
	/// <typeparam name="TError">The type of error to handle.</typeparam>
	/// <param name="result">The result to process.</param>
	/// <param name="handler">The asynchronous handler that returns a <see cref="ValueTask"/> for the specified error type.</param>
	/// <returns>A <see cref="ValueTask"/> returning the original result.</returns>
	public static async ValueTask<Result<T>> OnErrorOfTypeAsync<T, TError>(this Result<T> result, Func<TError, ValueTask> handler)
		where TError : Error
	{
		if (result is { IsError: true, Error: TError specificError })
		{
			await handler(specificError).ConfigureAwait(false);
		}
		return result;
	}

	/// <summary>
	/// Asynchronously executes the specified function using <see cref="ValueTask"/> if the current
	/// <see cref="Result{T}"/> instance contains a success value, or <paramref name="onError"/> if it contains an error,
	/// and returns the produced result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the result produced by the asynchronous functions.</typeparam>
	/// <param name="result">The <see cref="Result{T}"/> instance.</param>
	/// <param name="onSuccess">The asynchronous function that returns a <see cref="ValueTask{TResult}"/> to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <param name="onError">The asynchronous function that returns a <see cref="ValueTask{TResult}"/> to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask{TResult}"/> that, when awaited, evaluates to the result produced by either
	/// <paramref name="onSuccess"/> or <paramref name="onError"/>.
	/// </returns>
	public static async ValueTask<TResult> MatchAsync<T, TResult>(
		this Result<T> result,
		Func<T, ValueTask<TResult>> onSuccess,
		Func<Error, ValueTask<TResult>> onError)
	{
		return result.IsSuccess
			? await onSuccess(result.Value).ConfigureAwait(false)
			: await onError(result.Error).ConfigureAwait(false);
	}

	/// <summary>
	/// Asynchronously executes the specified action using <see cref="ValueTask"/> if the current
	/// <see cref="Result{T}"/> instance contains a success value, or <paramref name="onError"/> if it contains an error.
	/// Does not modify the result.
	/// </summary>
	/// <typeparam name="T">The type of the success result in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The <see cref="Result{T}"/> instance.</param>
	/// <param name="onSuccess">The asynchronous action that returns a <see cref="ValueTask"/> to execute if the <see cref="Result{T}"/> contains a success value.</param>
	/// <param name="onError">The asynchronous action that returns a <see cref="ValueTask"/> to execute if the <see cref="Result{T}"/> contains an error.</param>
	/// <returns>
	/// A <see cref="ValueTask"/> that, when awaited, evaluates to the current <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public static async ValueTask<Result<T>> SwitchAsync<T>(this Result<T> result, Func<T, ValueTask> onSuccess, Func<Error, ValueTask> onError)
	{
		if (result.IsSuccess)
		{
			await onSuccess(result.Value).ConfigureAwait(false);
		}
		else
		{
			await onError(result.Error).ConfigureAwait(false);
		}
		return result;
	}
}
