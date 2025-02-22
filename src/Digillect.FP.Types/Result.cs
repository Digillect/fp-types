namespace Digillect.FP.Types;

/// <summary>
/// Represents the result of an operation, encapsulating either a successful value of type <typeparamref name="T"/>
/// or an error. Provides methods to handle success and error cases in a functional programming style.
/// </summary>
public readonly struct Result<T> : IResult
{
	private readonly T _value;
	private readonly Error? _error;

	internal Result(T value)
	{
		_value = value;
		_error = null;
	}

	internal Result(Error error)
	{
		_value = default!;
		_error = error;
	}

	/// <summary>
	/// Indicates whether the result represents a successful operation.
	/// </summary>
	public bool IsSuccess => _error is null;

	/// <summary>
	/// Indicates whether the result represents a failed operation.
	/// </summary>
	public bool IsError => _error is not null;

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
	public static implicit operator Result<T>(T value)
	{
		return new Result<T>(value);
	}

	/// <summary>
	/// Allows implicit conversion from an <see cref="Error"/> to a failed result of type <see cref="Result{T}"/>.
	/// </summary>
	/// <param name="error">The error to wrap in a failed result.</param>
	/// <returns>A failed result containing the provided error.</returns>
	public static implicit operator Result<T>(Error error)
	{
		return new Result<T>(error);
	}

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
	public static implicit operator Result<Unit>(Result<T> result)
	{
		return result.IsSuccess ? Result.Success(Unit.Default) : Result.Error<Unit>(result.Error);
	}

	/// <summary>
	/// Transforms the successful result using the provided mapping function.
	/// </summary>
	/// <typeparam name="TResult">The type of the mapped result returned by the mapping function.</typeparam>
	/// <param name="map">
	/// A function that transforms the successful result into a value of type <typeparamref name="TResult" />.
	/// </param>
	/// <returns>
	/// A <see cref="Result{TResult}"/> containing the transformed success value or the original error.
	/// </returns>
	public Result<TResult> Map<TResult>(Func<T, TResult> map)
	{
		return _error is not null ? Result.Error<TResult>(_error) : Result.Success(map(_value));
	}

	/// <summary>
	/// Asynchronously transforms the successful result using the provided asynchronous mapping function.
	/// </summary>
	/// <typeparam name="TResult">The type of the mapped result returned by the asynchronous mapping function.</typeparam>
	/// <param name="map">
	/// An asynchronous function that transforms the successful result into a value of type <typeparamref name="TResult" />.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, returns a <see cref="Result{TResult}"/> containing the transformed success value or the original error.
	/// </returns>
	public async Task<Result<TResult>> MapAsync<TResult>(Func<T, Task<TResult>> map)
	{
		if (_error is not null)
		{
			return Result.Error<TResult>(_error);
		}

		var result = await map(_value).ConfigureAwait(false);

		return Result.Success(result);
	}

	/// <summary>
	/// Chains the successful result to another operation that returns a <see cref="Result{TResult}"/> using the provided binding function.
	/// </summary>
	/// <typeparam name="TResult">The type of the result returned by the binding function.</typeparam>
	/// <param name="bind">
	/// A function that maps the successful result into a new <see cref="Result{TResult}"/>.
	/// </param>
	/// <returns>
	/// A <see cref="Result{TResult}"/> containing the new success value or the original error.
	/// </returns>
	public Result<TResult> Bind<TResult>(Func<T, Result<TResult>> bind)
	{
		return _error is not null ? Result.Error<TResult>(_error) : bind(_value);
	}

	/// <summary>
	/// Asynchronously chains the successful result to another operation that returns a <see cref="Result{TResult}"/> using the provided asynchronous binding function.
	/// </summary>
	/// <typeparam name="TResult">The type of the result returned by the binding function.</typeparam>
	/// <param name="bind">
	/// An asynchronous function that maps the successful result into a new <see cref="Result{TResult}"/>.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, returns a <see cref="Result{TResult}"/> containing the new success value or the original error.
	/// </returns>
	public async Task<Result<TResult>> BindAsync<TResult>(Func<T, Task<Result<TResult>>> bind)
	{
		if (_error is not null)
		{
			return Result.Error<TResult>(_error);
		}

		var result = await bind(_value).ConfigureAwait(false);

		return result;
	}

	/// <summary>
	/// Performs a side effect using the successful result of the current <see cref="Result{T}" /> instance.
	/// Does not modify the result.
	/// </summary>
	/// <param name="action">
	/// An action to execute with the successful result.
	/// This action cannot modify the original result.
	/// </param>
	/// <returns>
	/// The original <see cref="Result{T}" /> instance, unchanged.
	/// </returns>
	public Result<T> Tap(Action<T> action)
	{
		if (_error is null)
		{
			action(_value);
		}

		return this;
	}

	/// <summary>
	/// Asynchronously performs a side effect using the successful result of the current <see cref="Result{T}" /> instance.
	/// Does not modify the result.
	/// </summary>
	/// <param name="action">
	/// An asynchronous action to execute with the successful result.
	/// This action cannot modify the original result.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}" /> that, when awaited, returns the original <see cref="Result{T}" /> instance, unchanged.
	/// </returns>
	public async Task<Result<T>> TapAsync(Func<T, Task> action)
	{
		if (_error is null)
		{
			await action(_value).ConfigureAwait(false);
		}

		return this;
	}

	/// <summary>
	/// Asynchronously performs a side effect using the successful result of the current <see cref="Result{T}" /> instance.
	/// Does not modify the result.
	/// </summary>
	/// <param name="action">
	/// An asynchronous action to execute with the successful result.
	/// This action cannot modify the original result.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}" /> that, when awaited, returns the original <see cref="Result{T}" /> instance, unchanged.
	/// </returns>
	public async Task<Result<T>> TapAsync(Func<Task> action)
	{
		if (_error is null)
		{
			await action().ConfigureAwait(false);
		}

		return this;
	}

	/// <summary>
	/// Provides an alternative value in case the current <see cref="Result{T}"/> instance contains an error.
	/// </summary>
	/// <param name="alternative">
	/// A value to return as an alternative if the current instance contains an error.
	/// </param>
	/// <returns>
	/// A <see cref="Result{T}"/> containing the original success value or the provided alternative.
	/// </returns>
	public Result<T> OrElse(T alternative)
	{
		return _error is null ? this : Result.Success(alternative);
	}

	/// <summary>
	/// Provides a factory for alternative value in case the current <see cref="Result{T}"/> instance contains an error.
	/// </summary>
	/// <param name="alternativeFactory">
	/// A factory for a value to return as an alternative if the current instance contains an error.
	/// </param>
	/// <returns>
	/// A <see cref="Result{T}"/> containing the original success value or the provided alternative.
	/// </returns>
	public Result<T> OrElse(Func<T> alternativeFactory)
	{
		return _error is null ? this : Result.Success(alternativeFactory());
	}

	/// <summary>
	/// Asynchronously provides an alternative value in case the current <see cref="Result{T}"/> instance contains an error.
	/// </summary>
	/// <param name="alternativeFactory">
	/// An asynchronous factory that provides an alternative value if the current instance contains an error.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, returns a <see cref="Result{T}"/> containing the original success value or the provided alternative.
	/// </returns>
	public async Task<Result<T>> OrElseAsync(Func<Task<T>> alternativeFactory)
	{
		if (_error is null)
		{
			return this;
		}

		var value = await alternativeFactory();

		return Result.Success(value);
	}

	/// <summary>
	/// Handles the current error by mapping it to an alternative value.
	/// </summary>
	/// <param name="recover">
	/// A function that maps the error to an alternative value.
	/// </param>
	/// <returns>
	/// A <see cref="Result{T}"/> containing the new success value or the original result.
	/// </returns>
	public Result<T> Recover(Func<Error, T> recover)
	{
		return _error is null ? this : Result.Success(recover(_error));
	}

	/// <summary>
	/// Asynchronously handles the current error by mapping it to an alternative value.
	/// </summary>
	/// <param name="recover">
	/// An asynchronous function that maps the error to an alternate <see cref="Result{T}"/> containing a success result.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, returns a <see cref="Result{T}"/>, containing the new success value or the original result.
	/// </returns>
	public async Task<Result<T>> RecoverAsync(Func<Error, Task<T>> recover)
	{
		if (_error is null)
		{
			return this;
		}

		var result = await recover(_error).ConfigureAwait(false);

		return Result.Success(result);
	}

	/// <summary>
	/// Handles the current error iby providing an alternate <see cref="Result{T}"/> object.
	/// </summary>
	/// <param name="recover">
	/// A function that maps the error to an alternate <see cref="Result{T}"/> object containing a new success value or a different error.
	/// </param>
	/// <returns>
	/// A <see cref="Result{T}"/> containing the original success value, or the result of the <paramref name="recover"/> function if an error exists.
	/// </returns>
	public Result<T> RecoverWith(Func<Error, Result<T>> recover)
	{
		return _error is null ? this : recover(_error);
	}

	/// <summary>
	/// Asynchronously handles the current error by providing an alternate <see cref="Result{T}"/> object.
	/// </summary>
	/// <param name="recover">
	/// An asynchronous function that maps the error to an alternate <see cref="Result{T}"/> object containing a new success value or a different error.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, returns a <see cref="Result{T}"/> containing the original success value, or the result of the <paramref name="recover"/> function if an error exists.
	/// </returns>
	public async Task<Result<T>> RecoverWithAsync(Func<Error, Task<Result<T>>> recover)
	{
		if (_error is null)
		{
			return this;
		}

		var result = await recover(_error);

		return result;
	}

	/// <summary>
	/// Performs a side effect when the current <see cref="Result{T}"/> instance contains an error.
	/// Does not modify the result.
	/// </summary>
	/// <param name="action">
	/// An action to execute when the instance contains an error. The action receives the error value as its parameter.
	/// </param>
	/// <returns>
	/// The original <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public Result<T> OnFailure(Action<Error> action)
	{
		if (_error is not null)
		{
			action(_error);
		}

		return this;
	}

	/// <summary>
	/// Asynchronously performs a side effect when the current <see cref="Result{T}"/> instance contains an error.
	/// Does not modify the result.
	/// </summary>
	/// <param name="action">
	/// An asynchronous action to execute when the instance contains an error.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, returns the original <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public async Task<Result<T>> OnFailureAsync(Func<Task> action)
	{
		if (_error is not null)
		{
			await action().ConfigureAwait(false);
		}

		return this;
	}

	/// <summary>
	/// Asynchronously performs a side effect when the current <see cref="Result{T}"/> instance contains an error.
	/// Does not modify the result.
	/// </summary>
	/// <param name="action">
	/// An asynchronous action to execute when the instance contains an error. The action receives the error value as its parameter.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, returns the original <see cref="Result{T}"/> instance, unchanged.
	/// </returns>
	public async Task<Result<T>> OnFailureAsync(Func<Error, Task> action)
	{
		if (_error is not null)
		{
			await action(_error).ConfigureAwait(false);
		}

		return this;
	}

	/// <summary>
	/// Matches the current <see cref="Result{T}"/> instance to one of two specified functions: one for a successful result
	/// and another for an error. Returns the result of the corresponding function.
	/// </summary>
	/// <typeparam name="TResult">The type of the value returned by the match functions.</typeparam>
	/// <param name="onSuccess">
	/// A function to execute if the instance contains a successful result.
	/// The function takes the success value as input and returns a value of type <typeparamref name="TResult"/>.
	/// </param>
	/// <param name="onError">
	/// A function to execute if the instance contains an error.
	/// The function takes the error value as input and returns a value of type <typeparamref name="TResult"/>.
	/// </param>
	/// <returns>
	/// A value of type <typeparamref name="TResult"/> returned by either <paramref name="onSuccess"/> or <paramref name="onError"/>.
	/// </returns>
	public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onError)
	{
		return _error is null ? onSuccess(_value) : onError(_error);
	}

	/// <summary>
	/// Asynchronously matches the current <see cref="Result{T}"/> instance to one of two specified asynchronous functions:
	/// one for a successful result and another for an error. Returns the result of the corresponding function.
	/// </summary>
	/// <typeparam name="TResult">The type of the value returned by the asynchronous match functions.</typeparam>
	/// <param name="onSuccess">
	/// An asynchronous function to execute if the instance contains a successful result.
	/// The function takes the success value as input and returns a value of type <typeparamref name="TResult"/>.
	/// </param>
	/// <param name="onError">
	/// An asynchronous function to execute if the instance contains an error.
	/// The function takes the error value as input and returns a value of type <typeparamref name="TResult"/>.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, returns a value of type <typeparamref name="TResult"/>
	/// produced by either the <paramref name="onSuccess"/> or <paramref name="onError"/> function.
	/// </returns>
	public Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> onSuccess, Func<Error, Task<TResult>> onError)
	{
		return _error is null ? onSuccess(_value) : onError(_error);
	}

	/// <summary>
	/// Executes one of two specified actions depending on whether the current <see cref="Result{T}"/> instance
	/// contains a success or an error.
	/// </summary>
	/// <param name="onSuccess">
	/// An action to execute if the instance contains a successful result.
	/// The action takes the success value as its parameter.
	/// </param>
	/// <param name="onError">
	/// An action to execute if the instance contains an error.
	/// The action takes the error value as its parameter.
	/// </param>
	/// <remarks>
	/// This method is used primarily for executing side effects (e.g., logging) without transforming the result.
	/// </remarks>
	public Result<T> Switch(Action<T> onSuccess, Action<Error> onError)
	{
		if (_error is null)
		{
			onSuccess(_value);
		}
		else
		{
			onError(_error);
		}

		return this;
	}

	/// <summary>
	/// Asynchronously executes one of two specified actions depending on whether the current <see cref="Result{T}"/> instance
	/// contains a success or an error.
	/// </summary>
	/// <param name="onSuccess">
	/// An asynchronous action to execute if the instance contains a successful result.
	/// The action takes the success value as its parameter.
	/// </param>
	/// <param name="onError">
	/// An asynchronous action to execute if the instance contains an error.
	/// The action takes the error value as its parameter.
	/// </param>
	/// <returns>
	/// A <see cref="Task"/> that completes execution after one of the actions has been performed.
	/// </returns>
	/// <remarks>
	/// This method is used primarily for handling side effects (e.g., logging, asynchronous notifications),
	/// without modifying or transforming the result.
	/// </remarks>
	public async Task<Result<T>> SwitchAsync(Func<T, Task> onSuccess, Func<Error, Task> onError)
	{
		if (_error is null)
		{
			await onSuccess(_value).ConfigureAwait(false);
		}
		else
		{
			await onError(_error).ConfigureAwait(false);
		}

		return this;
	}

	/// <summary>
	/// Returns an error if the specified condition is met.
	/// </summary>
	/// <param name="predicate">The condition that determines whether a failure occurs.</param>
	/// <param name="error">The error to return if the condition is met.</param>
	/// <returns>
	/// A <see cref="Result{T}"/> containing the successful result or the specified error.
	/// </returns>
	public Result<T> FailWhen(Func<T, bool> predicate, Error error)
	{
		if (_error is null && predicate(_value))
		{
			return Result.Error<T>(error);
		}

		return this;
	}

	/// <summary>
	/// Asynchronously returns an error if the specified condition is met.
	/// </summary>
	/// <param name="predicate">An asynchronous function that defines the condition to determine if a failure occurs.</param>
	/// <param name="error">The error to return if the condition is met.</param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, returns a <see cref="Result{T}"/> containing the successful result or the specified error.
	/// </returns>
	public async Task<Result<T>> FailWhenAsync(Func<T, Task<bool>> predicate, Error error)
	{
		if (_error is null)
		{
			bool result = await predicate(_value).ConfigureAwait(false);
			if (result)
			{
				return Result.Error<T>(error);
			}
		}

		return this;
	}

	/// <summary>
	/// Returns an error if the specified condition is met.
	/// </summary>
	/// <param name="predicate">The condition that determines whether a failure occurs.</param>
	/// <param name="errorFactory">The factory that produces an error to return if the condition is met.</param>
	/// <returns>
	/// A <see cref="Result{T}"/> containing the successful result or the produced error.
	/// </returns>
	public Result<T> FailWhenWith(Func<T, bool> predicate, Func<Error> errorFactory)
	{
		if (_error is null && predicate(_value))
		{
			return Result.Error<T>(errorFactory());
		}

		return this;
	}

	/// <summary>
	/// Returns an error if the specified condition is met.
	/// </summary>
	/// <param name="predicate">The condition that determines whether a failure occurs.</param>
	/// <param name="errorFactory">The factory that maps a result value to an error to return if the condition is met.</param>
	/// <returns>
	/// A <see cref="Result{T}"/> containing the successful result or the produced error.
	/// </returns>
	public Result<T> FailWhenWith(Func<T, bool> predicate, Func<T, Error> errorFactory)
	{
		if (_error is null && predicate(_value))
		{
			return Result.Error<T>(errorFactory(_value));
		}

		return this;
	}

	/// <summary>
	/// Transforms the current <see cref="Result{T}"/> instance into a failure if the specified predicate returns true.
	/// The error is generated asynchronously using the provided <paramref name="errorFactory"/> function.
	/// </summary>
	/// <param name="predicate">
	/// A predicate function to evaluate the current result. If the predicate returns true, the result is transformed into a failure.
	/// </param>
	/// <param name="errorFactory">
	/// An asynchronous function that generates an <see cref="Error"/> if the predicate returns true.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, returns a <see cref="Result{T}"/>.
	/// If the predicate returns true, the task result contains a failure; otherwise, it contains the original result.
	/// </returns>
	public async Task<Result<T>> FailWhenWithAsync(Func<T, bool> predicate, Func<Task<Error>> errorFactory)
	{
		if (_error is null && predicate(_value))
		{
			return Result.Error<T>(await errorFactory().ConfigureAwait(false));
		}

		return this;
	}

	/// <summary>
	/// Transforms the current <see cref="Result{T}"/> instance into a failure if the specified predicate returns true.
	/// The error is generated asynchronously using the provided <paramref name="errorFactory"/> function,
	/// which takes the successful result as a parameter.
	/// </summary>
	/// <param name="predicate">
	/// A predicate function to evaluate the current result. If the predicate returns true, the result is transformed into a failure.
	/// </param>
	/// <param name="errorFactory">
	/// An asynchronous function that generates an <see cref="Error"/> based on the successful result,
	/// if the predicate returns true.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, returns a <see cref="Result{T}"/>.
	/// If the predicate returns true, the task result contains a failure; otherwise, it contains the original result.
	/// </returns>
	public async Task<Result<T>> FailWhenWithAsync(Func<T, bool> predicate, Func<T, Task<Error>> errorFactory)
	{
		if (_error is null && predicate(_value))
		{
			return Result.Error<T>(await errorFactory(_value).ConfigureAwait(false));
		}

		return this;
	}

	/// <summary>
	/// Transforms the current <see cref="Result{T}"/> instance into a failure if the specified asynchronous predicate returns true.
	/// The error is generated using the provided <paramref name="errorFactory"/> function.
	/// </summary>
	/// <param name="predicate">
	/// An asynchronous predicate function to evaluate the current result. If it returns true, the result is transformed into a failure.
	/// </param>
	/// <param name="errorFactory">
	/// A function that generates an <see cref="Error"/> if the predicate returns true.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, returns a <see cref="Result{T}"/>.
	/// If the predicate returns true, the task result contains a failure; otherwise, it contains the original result.
	/// </returns>
	public async Task<Result<T>> FailWhenWithAsync(Func<T, Task<bool>> predicate, Func<Error> errorFactory)
	{
		if (_error is null)
		{
			bool result = await predicate(_value).ConfigureAwait(false);
			if (result)
			{
				return Result.Error<T>(errorFactory());
			}
		}

		return this;
	}

	/// <summary>
	/// Transforms the current <see cref="Result{T}"/> instance into a failure if the specified asynchronous predicate returns true.
	/// The error is generated using the provided <paramref name="errorFactory"/> function, which takes the successful result as a parameter.
	/// </summary>
	/// <param name="predicate">
	/// An asynchronous predicate function to evaluate the current result. If it returns true, the result is transformed into a failure.
	/// </param>
	/// <param name="errorFactory">
	/// A function that generates an <see cref="Error"/> based on the successful result, if the predicate returns true.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, returns a <see cref="Result{T}"/>.
	/// If the predicate returns true, the task result contains a failure; otherwise, it contains the original result.
	/// </returns>
	public async Task<Result<T>> FailWhenWithAsync(Func<T, Task<bool>> predicate, Func<T, Error> errorFactory)
	{
		if (_error is null)
		{
			bool result = await predicate(_value).ConfigureAwait(false);
			if (result)
			{
				return Result.Error<T>(errorFactory(_value));
			}
		}

		return this;
	}

	/// <summary>
	/// Transforms the current <see cref="Result{T}"/> instance into a failure if the specified asynchronous predicate returns true.
	/// The error is generated asynchronously using the provided <paramref name="errorFactory"/> function.
	/// </summary>
	/// <param name="predicate">
	/// An asynchronous predicate function to evaluate the current result. If it returns true, the result is transformed into a failure.
	/// </param>
	/// <param name="errorFactory">
	/// An asynchronous function that generates an <see cref="Error"/> if the predicate returns true.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, returns a <see cref="Result{T}"/>.
	/// If the predicate returns true, the task result contains a failure; otherwise, it contains the original result.
	/// </returns>
	public async Task<Result<T>> FailWhenWithAsync(Func<T, Task<bool>> predicate, Func<Task<Error>> errorFactory)
	{
		if (_error is null)
		{
			bool result = await predicate(_value).ConfigureAwait(false);
			if (result)
			{
				return Result.Error<T>(await errorFactory().ConfigureAwait(false));
			}
		}

		return this;
	}

	/// <summary>
	/// Transforms the current <see cref="Result{T}"/> instance into a failure if the specified asynchronous predicate returns true.
	/// The error is generated asynchronously using the provided <paramref name="errorFactory"/> function,
	/// which takes the successful result as a parameter.
	/// </summary>
	/// <param name="predicate">
	/// An asynchronous predicate function to evaluate the current result. If it returns true, the result is transformed into a failure.
	/// </param>
	/// <param name="errorFactory">
	/// An asynchronous function that generates an <see cref="Error"/> based on the successful result,
	/// if the predicate returns true.
	/// </param>
	/// <returns>
	/// A <see cref="Task{T}"/> that, when awaited, returns a <see cref="Result{T}"/>.
	/// If the predicate returns true, the task result contains a failure; otherwise, it contains the original result.
	/// </returns>
	public async Task<Result<T>> FailWhenWithAsync(Func<T, Task<bool>> predicate, Func<T, Task<Error>> errorFactory)
	{
		if (_error is null)
		{
			bool result = await predicate(_value).ConfigureAwait(false);
			if (result)
			{
				return Result.Error<T>(await errorFactory(_value).ConfigureAwait(false));
			}
		}

		return this;
	}

	/// <summary>
	/// Transforms the successful value of the result using the specified mapping function.
	/// </summary>
	/// <param name="map">The function to apply to the successful value.</param>
	/// <returns>A new result with the transformed value if the original result was successful; otherwise, the original error.</returns>
	public Result<TResult> Select<TResult>(Func<T, TResult> map)
	{
		return _error is null ? Result.Success(map(_value)) : Result.Error<TResult>(_error);
	}

	/// <summary>
	/// Projects the result value into a new <see cref="Result{T}"/> according to a projection function,
	/// and then flattens the results into a final result.
	/// </summary>
	/// <typeparam name="TResult">The type of the intermediate result.</typeparam>
	/// <typeparam name="TProjection">The type of the final projection result.</typeparam>
	/// <param name="bind">A function that transforms the value into an intermediate result.</param>
	/// <param name="project">A projection function that combines the original value and intermediate result.</param>
	/// <returns>
	/// A result containing the projected value if both operations succeed;
	/// otherwise, a result containing the error from the first failed operation.
	/// </returns>
	/// <remarks>
	/// This method is part of the LINQ support for monadic operations over results.
	/// </remarks>

	public Result<TProjection> SelectMany<TResult, TProjection>(Func<T, Result<TResult>> bind, Func<T, TResult, TProjection> project)
	{
		if (_error is not null)
		{
			return Result.Error<TProjection>(_error);
		}

		var intermediate = bind(_value);
		if (intermediate.IsError)
		{
			return Result.Error<TProjection>(intermediate.Error);
		}

		return project(_value, intermediate.Value);
	}

	/// <summary>
	/// Asynchronously projects the result value into a new <see cref="Result{T}"/> according to a projection function,
	/// and then flattens the results into a final result.
	/// </summary>
	/// <typeparam name="TResult">The type of the intermediate result.</typeparam>
	/// <typeparam name="TProjection">The type of the final projection result.</typeparam>
	/// <param name="bind">An asynchronous function that transforms the value into an intermediate result.</param>
	/// <param name="project">A projection function that combines the original value and intermediate result.</param>
	/// <returns>
	/// A task representing a result containing the projected value if both operations succeed;
	/// otherwise, a result containing the error from the first failed operation.
	/// </returns>
	/// <remarks>
	/// This method is part of the asynchronous LINQ support for monadic operations over results.
	/// </remarks>
	public async Task<Result<TProjection>> SelectMany<TResult, TProjection>(Func<T, Task<Result<TResult>>> bind, Func<T, TResult, TProjection> project)
	{
		if (_error is not null)
		{
			return Result.Error<TProjection>(_error);
		}

		var intermediate = await bind(_value);
		if (intermediate.IsError)
		{
			return Result.Error<TProjection>(intermediate.Error);
		}

		return project(_value, intermediate.Value);
	}

	/// <summary>
	/// Asynchronously projects the result value into a new <see cref="Result{T}"/> according to a projection function,
	/// and then asynchronously flattens the results into a final result.
	/// </summary>
	/// <typeparam name="TResult">The type of the intermediate result.</typeparam>
	/// <typeparam name="TProjection">The type of the final projection result.</typeparam>
	/// <param name="bind">An asynchronous function that transforms the value into an intermediate result.</param>
	/// <param name="project">An asynchronous projection function that combines the original value and intermediate result.</param>
	/// <returns>
	/// A task representing a result containing the projected value if both operations succeed;
	/// otherwise, a result containing the error from the first failed operation.
	/// </returns>
	/// <remarks>
	/// This method is part of the asynchronous LINQ support for monadic operations over results.
	/// Both binding and projection operations are performed asynchronously.
	/// </remarks>
	public async Task<Result<TProjection>> SelectMany<TResult, TProjection>(Func<T, Task<Result<TResult>>> bind, Func<T, TResult, Task<TProjection>> project)
	{
		if (_error is not null)
		{
			return Result.Error<TProjection>(_error);
		}

		var intermediate = await bind(_value).ConfigureAwait(false);
		if (intermediate.IsError)
		{
			return Result.Error<TProjection>(intermediate.Error);
		}

		return await project(_value, intermediate.Value).ConfigureAwait(false);
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
	public static Result<T> Success<T>(T value)
	{
		return new Result<T>(value);
	}

	/// <summary>
	/// Creates an error result with the provided error.
	/// </summary>
	/// <param name="error">The error representing the reason for the failure.</param>
	/// <returns>The created failed result.</returns>
	public static Result<T> Error<T>(Error error)
	{
		return new Result<T>(error);
	}
}
