using System.Runtime.CompilerServices;

namespace Digillect.FP.Types;

/// <summary>
/// Represents the result of an operation, encapsulating either a successful value of type <typeparamref name="T"/>
/// or an error. Provides methods to handle success and error cases in a functional programming style.
/// </summary>
public readonly struct Result<T> : IResult
{
	private readonly T _value;
	private readonly Error? _error;

	private Result(T value)
	{
		_value = value;
		_error = null;
	}

	private Result(Error error)
	{
		_value = default!;
		_error = error;
	}

	/// <summary>
	/// Creates a successful result with the provided value.
	/// </summary>
	/// <param name="value">The value associated with the success result.</param>
	/// <returns>The created successful result.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Result<T> Success(T value) => new(value);

	/// <summary>
	/// Creates a failed result with the provided error.
	/// </summary>
	/// <param name="error">The error associated with the failure result.</param>
	/// <returns>The created failed result.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Result<T> Failure(Error error) => new(error);

	/// <summary>
	/// Indicates whether the result represents a successful operation.
	/// </summary>
	public bool IsSuccess => _error is null;

	/// <summary>
	/// Indicates whether the result represents a failed operation.
	/// </summary>
	public bool IsFailure => _error is not null;

	/// <summary>
	/// Gets the encapsulated value of a successful result or throws an exception if the result represents a failure.
	/// </summary>
	/// <exception cref="InvalidOperationException">
	/// Thrown if the result is a failure and there is no value to retrieve.
	/// </exception>
	public T Value => _error is null ? _value : throw new InvalidOperationException("Attempt to access value of the failed result.");

	/// <summary>
	/// Gets the error associated with a failed result. Throws an exception if the result represents a success.
	/// </summary>
	/// <exception cref="InvalidOperationException">
	/// Thrown when attempting to access the error of a successful result.
	/// </exception>
	public Error Error => _error ?? throw new InvalidOperationException("Attempt to access error of the successful result.");

	/// <summary>
	/// Allows implicit conversion from a value of type <typeparamref name="T"/> to a successful result of type <see cref="Result{T}"/>.
	/// </summary>
	/// <param name="value">The value to wrap in a successful result.</param>
	/// <returns>A successful result containing the provided value.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Result<T>(T value) => new(value);

	/// <summary>
	/// Allows implicit conversion from an <see cref="Error"/> to a failed result of type <see cref="Result{T}"/>.
	/// </summary>
	/// <param name="error">The error to wrap in a failed result.</param>
	/// <returns>A failed result containing the provided error.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Result<T>(Error error) => new(error);

	/// <summary>
	/// Provides an implicit operator to convert a result of type <typeparamref name="T"/>
	/// to a result of type <see cref="Unit"/>. If the original result is successful,
	/// it creates a successful <see cref="Result{T}"/> with <see cref="Unit.Default"/> as the value.
	/// If the original result is a failure, it creates a failed <see cref="Result{T}"/> with the same error.
	/// </summary>
	/// <param name="result">The result instance of type <typeparamref name="T"/>.</param>
	/// <returns>
	/// A new <see cref="Result{T}"/> instance where the type is <see cref="Unit"/>.
	/// The returned result is successful if the original is successful, otherwise it propagates the failure.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static implicit operator Result<Unit>(Result<T> result)
		=> result.IsSuccess ? Result<Unit>.Success(Unit.Default) : Result<Unit>.Failure(result.Error);

	/// <summary>
	/// Executes a continuation function on the result's value if the result is successful.
	/// If the result is failed, propagates the existing error.
	/// </summary>
	/// <typeparam name="TResult">The type of the value produced by the continuation function.</typeparam>
	/// <param name="next">The continuation function to execute on the result's value.</param>
	/// <returns>
	/// A new result containing the value produced by the continuation function
	/// if the original result is successful, or the original error if it is failed.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Result<TResult> Then<TResult>(Func<T, TResult> next)
	{
		return _error is not null ? Result<TResult>.Failure(_error) : Result<TResult>.Success(next(_value));
	}

	/// <summary>
	/// Executes an asynchronous continuation function if the current result represents a successful outcome.
	/// </summary>
	/// <param name="next">The asynchronous function to invoke on the value of the successful result.</param>
	/// <typeparam name="TResult">The type of the value returned by the continuation function.</typeparam>
	/// <returns>
	/// A task that represents the result of asynchronously applying the continuation function.
	/// If the current result is successful, returns a successful result with the mapped value.
	/// If the current result is a failure, propagates the failure without invoking the continuation.
	/// </returns>
	public async Task<Result<TResult>> ThenAsync<TResult>(Func<T, Task<TResult>> next)
	{
		if (_error is not null)
		{
			return Result<TResult>.Failure(_error);
		}

		var result = await next(_value).ConfigureAwait(false);

		return Result<TResult>.Success(result);
	}

	/// <summary>
	/// Chains the execution of a continuation function if the current result is successful.
	/// </summary>
	/// <typeparam name="TResult">The type of the value in the resulting result.</typeparam>
	/// <param name="next">The function to invoke if the current result is successful.</param>
	/// <returns>
	/// A new result containing the value from the provided continuation function if the current result is successful,
	/// or a failed result containing the current error if the result is unsuccessful.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Result<TResult> Then<TResult>(Func<T, Result<TResult>> next)
	{
		return _error is not null ? Result<TResult>.Failure(_error) : next(_value);
	}

	/// <summary>
	/// Asynchronously chains the current successful result to a continuation function that returns a task of a result.
	/// </summary>
	/// <param name="next">A function that takes the value of the current result and returns a task wrapping the next result.</param>
	/// <typeparam name="TResult">The type of the result produced by the continuation function.</typeparam>
	/// <returns>A task representing the result of the continuation if the current result was successful; otherwise, the failed result.</returns>
	public async Task<Result<TResult>> ThenAsync<TResult>(Func<T, Task<Result<TResult>>> next)
	{
		if (_error is not null)
		{
			return Result<TResult>.Failure(_error);
		}

		var result = await next(_value).ConfigureAwait(false);

		return result;
	}

	/// <summary>
	/// Performs an action on the successful value of the current result if it represents success.
	/// </summary>
	/// <param name="action">The action to be performed on the successful value.</param>
	/// <returns>The current result.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Result<T> Do(Action<T> action)
	{
		if (_error is null)
		{
			action(_value);
		}

		return this;
	}

	/// <summary>
	/// Asynchronously performs the given action if the result represents a successful state.
	/// </summary>
	/// <param name="action">
	/// The asynchronous action to execute on the encapsulated value if the result is successful.
	/// </param>
	/// <returns>
	/// A task that completes with the original result.
	/// If the result is a failure, no action is performed, and the failure state is preserved.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<Result<T>> DoAsync(Func<T, Task> action)
	{
		if (_error is null)
		{
			await action(_value).ConfigureAwait(false);
		}

		return this;
	}

	/// <summary>
	/// Executes the provided asynchronous action if the result is successful.
	/// </summary>
	/// <param name="action">The asynchronous action to perform.</param>
	/// <returns>A task that represents the asynchronous operation, containing the result.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<Result<T>> DoAsync(Func<Task> action)
	{
		if (_error is null)
		{
			await action().ConfigureAwait(false);
		}

		return this;
	}

	/// <summary>
	/// Returns the current result if it represents a success; otherwise, returns a successful result with the specified value.
	/// </summary>
	/// <param name="value">The value to return as a successful result if the current result is a failure.</param>
	/// <returns>The current result if successful; otherwise, a successful result with the provided value.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Result<T> Else(T value)
	{
		return _error is null ? this : Success(value);
	}

	/// <summary>
	/// Provides an alternative value to be used if the current result represents a failure.
	/// </summary>
	/// <param name="value">A function that returns the alternative value to be used when the result is a failure.</param>
	/// <returns>
	/// A new result containing the original success value or the alternative value if the current result is a failure.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Result<T> Else(Func<T> value)
	{
		return _error is null ? this : Success(value());
	}

	/// <summary>
	/// Transforms the error of a failed result using the specified mapping function, while preserving the original result if it is successful.
	/// </summary>
	/// <param name="map">A function that maps the error to a value of type <typeparamref name="T"/>.</param>
	/// <returns>The original successful result if no error exists, otherwise a transformed result containing the mapped value.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Result<T> Else(Func<Error, T> map)
	{
		return _error is null ? this : Success(map(_error));
	}

	/// <summary>
	/// Returns the current result if it is successful; otherwise, evaluates the provided mapping function
	/// on the error and returns the resulting <see cref="Result{T}"/>.
	/// </summary>
	/// <param name="map">A function that takes an <see cref="Error"/> and returns a new <see cref="Result{T}"/>.</param>
	/// <returns>The current result if it is successful; otherwise, the result of the mapping function.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Result<T> Else(Func<Error, Result<T>> map)
	{
		return _error is null ? this : map(_error);
	}

	/// <summary>
	/// Provides an asynchronous fallback mechanism by applying the specified function when the result is a failure.
	/// </summary>
	/// <param name="map">A function to map the error to a new success value asynchronously.</param>
	/// <returns>
	/// A task that represents the asynchronous operation, returning a successful result if the fallback mapping is applied,
	/// or the original result if it is successful.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<Result<T>> ElseAsync(Func<Error, Task<T>> map)
	{
		if (_error is null)
		{
			return this;
		}

		var result = await map(_error).ConfigureAwait(false);

		return Success(result);
	}

	/// <summary>
	/// Executes the specified action if the result represents a failure.
	/// </summary>
	/// <param name="action">An action to perform on the error associated with the failed result.</param>
	/// <returns>The current result instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Result<T> ElseDo(Action<Error> action)
	{
		if (_error is not null)
		{
			action(_error);
		}

		return this;
	}

	/// <summary>
	/// Executes an asynchronous action if the result represents a failure.
	/// </summary>
	/// <param name="action">The asynchronous action to execute, receiving the error from the failed result.</param>
	/// <returns>The current result instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public async Task<Result<T>> ElseDoAsync(Func<Error, Task> action)
	{
		if (_error is not null)
		{
			await action(_error).ConfigureAwait(false);
		}

		return this;
	}

	/// <summary>
	/// Invokes the specified functions based on the state of the result and returns the result of the invoked function.
	/// </summary>
	/// <param name="onSuccess">The function to invoke if the result represents a success.</param>
	/// <param name="onFailure">The function to invoke if the result represents a failure.</param>
	/// <returns>The result of invoking the appropriate function.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
		=> _error is null ? onSuccess(_value) : onFailure(_error);

	/// <summary>
	/// Handles the result asynchronously based on its state, invoking the appropriate function for success or failure.
	/// </summary>
	/// <param name="onSuccess">
	/// The function to invoke if the result is successful. Takes the value of the result as input and produces a Task with a result.
	/// </param>
	/// <param name="onFailure">
	/// The function to invoke if the result is a failure. Takes the error of the result as input and produces a Task with a result.
	/// </param>
	/// <returns>A Task containing the result of the invoked function based on the result state.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> onSuccess, Func<Error, Task<TResult>> onFailure)
	{
		return _error is null ? onSuccess(_value) : onFailure(_error);
	}

	/// <summary>
	/// Invokes the provided actions based on the result's state and returns the current result.
	/// </summary>
	/// <param name="onSuccess">The action to execute if the result is successful.</param>
	/// <param name="onFailure">The action to execute if the result is a failure.</param>
	/// <returns>The current result instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Result<T> Switch(Action<T> onSuccess, Action<Error> onFailure)
	{
		if (_error is null)
		{
			onSuccess(_value);
		}
		else
		{
			onFailure(_error);
		}

		return this;
	}

	/// <summary>
	/// Asynchronously executes the provided functions depending on the result state.
	/// </summary>
	/// <param name="onSuccess">The function to execute if the result is successful.</param>
	/// <param name="onFailure">The function to execute if the result is a failure.</param>
	/// <returns>The current result instance.</returns>
	public async Task<Result<T>> SwitchAsync(Func<T, Task> onSuccess, Func<Error, Task> onFailure)
	{
		if (_error is null)
		{
			await onSuccess(_value).ConfigureAwait(false);
		}
		else
		{
			await onFailure(_error).ConfigureAwait(false);
		}

		return this;
	}

	/// <summary>
	/// Returns a failed result if the provided predicate evaluates to true, using the specified error.
	/// </summary>
	/// <param name="predicate">A function that takes the current value and returns a boolean indicating whether the condition for failure is met.</param>
	/// <param name="error">The error to associate with the failure result if the predicate is true.</param>
	/// <returns>
	/// A new failed result if the predicate returns true.
	/// Otherwise, the original result is returned unchanged.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Result<T> FailIf(Func<T, bool> predicate, Error error)
	{
		if (_error is null && predicate(_value))
		{
			return Failure(error);
		}

		return this;
	}

	/// <summary>
	/// Returns a failed result if the specified predicate evaluates to true; otherwise, returns the current result.
	/// </summary>
	/// <param name="predicate">A function that defines the failure condition based on the value.</param>
	/// <param name="error">A function that generates the error to associate with the failure result.</param>
	/// <returns>
	/// A new failed result if the predicate returns true.
	/// Otherwise, the original result is returned unchanged.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Result<T> FailIf(Func<T, bool> predicate, Func<Error> error)
	{
		if (_error is null && predicate(_value))
		{
			return Failure(error());
		}

		return this;
	}

	/// <summary>
	/// Evaluates the result's value against a given predicate and returns a failure result if the predicate is met.
	/// </summary>
	/// <param name="predicate">A function that takes the value and returns true if the condition for failure is met.</param>
	/// <param name="error">A function that generates an error when the predicate condition is met, using the current value.</param>
	/// <returns>
	/// A new failed result if the predicate returns true.
	/// Otherwise, the original result is returned unchanged.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Result<T> FailIf(Func<T, bool> predicate, Func<T, Error> error)
	{
		if (_error is null && predicate(_value))
		{
			return Failure(error(_value));
		}

		return this;
	}

	/// <summary>
	/// Returns a failed result if the specified predicate evaluates to true for the value of a successful result,
	/// using the error provided by the asynchronous error-generating function.
	/// </summary>
	/// <param name="predicate">A function that determines whether the result should fail based on the current value.</param>
	/// <param name="error">An asynchronous function that returns the error to be associated with the failure.</param>
	/// <returns>
	/// A failed result with the provided error if the predicate evaluates to true;
	/// otherwise, the current result.
	/// </returns>
	public async Task<Result<T>> FailIfAsync(Func<T, bool> predicate, Func<Task<Error>> error)
	{
		if (_error is null && predicate(_value))
		{
			return Failure(await error().ConfigureAwait(false));
		}

		return this;
	}

	/// <summary>
	/// Asynchronously transforms the result into a failure if the provided predicate evaluates to true and produces an error.
	/// </summary>
	/// <param name="predicate">A function that determines whether the result should fail based on its value.</param>
	/// <param name="error">An asynchronous function that generates an error if the predicate evaluates to true.</param>
	/// <returns>
	/// A task representing the asynchronous operation. The result will be a failure if the predicate is met and the error is produced,
	/// otherwise, it will return the original result.
	/// </returns>
	public async Task<Result<T>> FailIfAsync(Func<T, bool> predicate, Func<T, Task<Error>> error)
	{
		if (_error is null && predicate(_value))
		{
			return Failure(await error(_value).ConfigureAwait(false));
		}

		return this;
	}

	/// <summary>
	/// Asynchronously evaluates a predicate against the result's value and returns a failure result
	/// with the specified error if the predicate evaluates to true.
	/// </summary>
	/// <param name="predicate">An asynchronous function that assesses the result's value and returns a boolean.</param>
	/// <param name="error">The error to return if the predicate evaluates to true.</param>
	/// <returns>A failed result with the provided error if the predicate evaluates to true; otherwise, the original result.</returns>
	public async Task<Result<T>> FailIfAsync(Func<T, Task<bool>> predicate, Error error)
	{
		if (_error is null)
		{
			bool result = await predicate(_value).ConfigureAwait(false);
			if (result)
			{
				return Failure(error);
			}
		}

		return this;
	}

	/// <summary>
	/// Asynchronously fails the result if the provided predicate evaluates to true.
	/// </summary>
	/// <param name="predicate">A function to evaluate the condition that determines whether the result should fail.</param>
	/// <param name="error">A function to produce the error if the predicate evaluates to true.</param>
	/// <returns>A failed result with the provided error if the predicate evaluates to true; otherwise, the original result.</returns>
	public async Task<Result<T>> FailIfAsync(Func<T, Task<bool>> predicate, Func<Error> error)
	{
		if (_error is null)
		{
			bool result = await predicate(_value).ConfigureAwait(false);
			if (result)
			{
				return Failure(error());
			}
		}

		return this;
	}

	/// <summary>
	/// Asynchronously fails the result with the provided error if the specified predicate evaluates to true.
	/// </summary>
	/// <param name="predicate">The asynchronous predicate function to evaluate the current result value.</param>
	/// <param name="error">The function to generate an error based on the current result value.</param>
	/// <returns>A failed result with the provided error if the predicate evaluates to true; otherwise, the original result.</returns>
	public async Task<Result<T>> FailIfAsync(Func<T, Task<bool>> predicate, Func<T, Error> error)
	{
		if (_error is null)
		{
			bool result = await predicate(_value).ConfigureAwait(false);
			if (result)
			{
				return Failure(error(_value));
			}
		}

		return this;
	}

	/// <summary>
	/// Evaluates a predicate asynchronously on the result's value, and if it returns true, creates a failure result with the specified error.
	/// </summary>
	/// <param name="predicate">An asynchronous function that evaluates a condition on the value of the result.</param>
	/// <param name="error">An asynchronous function that provides the error to be used in case of failure.</param>
	/// <returns>A failed result with the provided error if the predicate evaluates to true; otherwise, the original result.</returns>
	public async Task<Result<T>> FailIfAsync(Func<T, Task<bool>> predicate, Func<Task<Error>> error)
	{
		if (_error is null)
		{
			bool result = await predicate(_value).ConfigureAwait(false);
			if (result)
			{
				return Failure(await error().ConfigureAwait(false));
			}
		}

		return this;
	}

	/// <summary>
	/// Creates a failure result if the provided asynchronous predicate evaluates to true.
	/// </summary>
	/// <param name="predicate">An asynchronous function that takes the current value and returns a boolean indicating whether the failure condition is met.</param>
	/// <param name="error">An asynchronous function that takes the current value and returns the error to associate with the failure result.</param>
	/// <returns>A failed result with the provided error if the predicate evaluates to true; otherwise, the original result.</returns>
	public async Task<Result<T>> FailIfAsync(Func<T, Task<bool>> predicate, Func<T, Task<Error>> error)
	{
		if (_error is null)
		{
			bool result = await predicate(_value).ConfigureAwait(false);
			if (result)
			{
				return Failure(await error(_value).ConfigureAwait(false));
			}
		}

		return this;
	}

	/// <summary>
	/// Transforms the successful value of the result using the specified mapping function.
	/// </summary>
	/// <param name="map">The function to apply to the successful value.</param>
	/// <returns>A new result with the transformed value if the original result was successful; otherwise, the original error.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Result<TResult> Select<TResult>(Func<T, TResult> map)
		=> _error is null ? Result<TResult>.Success(map(_value)) : Result<TResult>.Failure(_error);

	/// <summary>
	/// Projects each value of a result to another result and transforms the combined value using the specified projection function.
	/// </summary>
	/// <typeparam name="TResult">The type of the intermediate result produced by the bind function.</typeparam>
	/// <typeparam name="TProjection">The type of the final projected result.</typeparam>
	/// <param name="bind">A function to transform the current value into a result of type <typeparamref name="TResult"/>.</param>
	/// <param name="project">A function to transform the initial value and the result of <paramref name="bind"/> into the final projected value.</param>
	/// <returns>
	/// A new result containing the projected value if both the current and intermediate results are successful; otherwise,
	/// a failed result with the appropriate error.
	/// </returns>
	public Result<TProjection> SelectMany<TResult, TProjection>(Func<T, Result<TResult>> bind, Func<T, TResult, TProjection> project)
	{
		if (_error is not null)
		{
			return Result<TProjection>.Failure(_error);
		}

		var intermediate = bind(_value);
		if (intermediate.IsFailure)
		{
			return Result<TProjection>.Failure(intermediate.Error);
		}

		return project(_value, intermediate.Value);
	}
}

/// <summary>
/// Represents a factory for creating instances of <see cref="Result{T}"/>.
/// Provides static methods to create successful or failed results, encapsulating
/// values or errors respectively.
/// </summary>
public static class Result
{
	/// <summary>
	/// Creates a successful result with the specified value.
	/// </summary>
	/// <param name="value">The value that represents the successful result.</param>
	/// <returns>A successful result containing the given value.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Result<T> Success<T>(T value) => Result<T>.Success(value);

	/// <summary>
	/// Creates a failed result with the provided error.
	/// </summary>
	/// <param name="error">The error representing the reason for the failure.</param>
	/// <returns>The created failed result.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Result<T> Failure<T>(Error error) => Result<T>.Failure(error);
}
