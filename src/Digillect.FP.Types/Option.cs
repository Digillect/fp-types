namespace Digillect.FP.Types;

/// <summary>
/// Represents an optional value, either containing a value of type <typeparamref name="T"/> (Some) or no value at all (None).
/// </summary>
/// <typeparam name="T">The type of the value contained in the option. Must be a non-nullable type.</typeparam>
public readonly struct Option<T> : IEquatable<Option<T>>
	where T : notnull
{
	/// <summary>
	/// Represents the absence of a value in the context of an <see cref="Option{T}" />.
	/// Used to indicate that an <see cref="Option{T}" /> is in the 'None' state, containing no value.
	/// </summary>
	public static readonly Option<T> None = default;

	/// <summary>
	/// Creates an option containing a value.
	/// </summary>
	/// <typeparam name="T">The type of the value to be wrapped in an option. Must be a non-nullable type.</typeparam>
	/// <param name="value">The value to wrap. Must be non-null.</param>
	/// <returns>An option instance containing the provided value.</returns>
	public static Option<T> Some(T value) => new(value);

	private readonly T? _value;
	private readonly bool _isSome;

	/// <summary>
	/// Indicates whether the current <see cref="Option{T}"/> instance contains a value.
	/// Returns <c>true</c> if the instance is in the 'Some' state, containing a value; otherwise, returns <c>false</c>.
	/// </summary>
	public bool IsSome => _isSome;

	/// <summary>
	/// Indicates whether the current <see cref="Option{T}"/> instance represents the 'None' state,
	/// meaning it does not contain a value.
	/// </summary>
	public bool IsNone => !_isSome;

	private Option(T value)
	{
		_value = value;
		_isSome = true;
	}

	/// <summary>
	/// Determines whether the current <see cref="Option{T}"/> is equal to another <see cref="Option{T}"/>.
	/// </summary>
	/// <param name="other">The other <see cref="Option{T}"/> to compare with the current instance.</param>
	/// <returns><c>true</c> if the current instance is equal to the other instance; otherwise, <c>false</c>.</returns>
	public bool Equals(Option<T> other) => _isSome == other._isSome && EqualityComparer<T>.Default.Equals(_value!, other._value!);

	/// <inheritdoc />
	public override bool Equals(object? obj)
	{
		if (obj is Option<T> other)
		{
			return Equals(other);
		}

		return false;
	}

	/// <inheritdoc />
	public override int GetHashCode() => _isSome ? _value?.GetHashCode() ?? 0 : 0;

	/// <inheritdoc />
	public override string ToString() => _isSome ? $"Some({_value})" : "None";

	public static explicit operator T(Option<T> option) => option._isSome ? option._value! : throw new InvalidCastException("Option is not in a Some state");

	public static implicit operator Option<T>(T? value) => value is not null ? Some(value) : None;

	public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);

	public static bool operator !=(Option<T> left, Option<T> right) => !(left == right);

	public static Option<T> operator |(Option<T> left, Option<T> right) => left._isSome ? left : right;

	public static bool operator true(Option<T> value) => value._isSome;

	public static bool operator false(Option<T> value) => !value._isSome;

	/// <summary>
	/// Transforms the value contained in the option using the specified mapping function.
	/// </summary>
	/// <typeparam name="TResult">The type of the value resulting from the mapping function. Must be a non-nullable type.</typeparam>
	/// <param name="map">A function to transform the value contained in the option.</param>
	/// <returns>
	/// An option containing the result of applying the mapping function if the current option is in the Some state,
	/// or an empty option (None) if the current option is in the None state.
	/// </returns>
	public Option<TResult> Map<TResult>(Func<T, TResult> map)
		where TResult : notnull =>
		_isSome
			? Option<TResult>.Some(map(_value!))
			: default;

	/// <summary>
	/// Transforms the current option by applying a function that returns another option.
	/// If the current option is in the 'Some' state, the provided function is applied to its value.
	/// If it is in the 'None' state, the result will also be a 'None'.
	/// </summary>
	/// <typeparam name="TResult">The type of the value that the resulting option will contain. Must be a non-nullable type.</typeparam>
	/// <param name="bind">A function that maps the current option's value to another option value of type <typeparamref name="TResult"/>.</param>
	/// <returns>An option containing the transformed value if the current option is 'Some', or 'None' if the current option is 'None'.</returns>
	public Option<TResult> Bind<TResult>(Func<T, Option<TResult>> bind)
		where TResult : notnull =>
		_isSome ? bind(_value!) : default;

	/// <summary>
	/// Projects the value of the current option using a specified binding and mapping function,
	/// returning a new option containing the result of the projection if both values are present.
	/// </summary>
	/// <typeparam name="TIntermediate">The type of the intermediate value generated by the binding function. Must be a non-nullable type.</typeparam>
	/// <typeparam name="TResult">The type of the result produced by the projection function. Must be a non-nullable type.</typeparam>
	/// <param name="bind">A function that maps the value in the current option to another option.</param>
	/// <param name="project">A function that combines the current value and the bound intermediate value to produce the final result.</param>
	/// <returns>An option containing the result of the projection if both the current option and the intermediate option contain values; otherwise, an empty option.</returns>
	public Option<TResult> SelectMany<TIntermediate, TResult>(
		Func<T, Option<TIntermediate>> bind,
		Func<T, TIntermediate, TResult> project)
		where TIntermediate : notnull
		where TResult : notnull
	{
		if (IsNone)
		{
			return default;
		}

		var intermediate = bind(_value!);

		if (intermediate.IsNone)
		{
			return default;
		}

		return project(_value!, intermediate._value!);
	}

	/// <summary>
	/// Executes the provided functions based on the state of the <see cref="Option{T}"/>.
	/// </summary>
	/// <typeparam name="TResult">The type of the result returned by the provided functions.</typeparam>
	/// <param name="ifSome">The function to execute if the option is in the 'Some' state, containing a value.</param>
	/// <param name="ifNone">The function to execute if the option is in the 'None' state, containing no value.</param>
	/// <returns>The result of the executed function, depending on the state of the option.</returns>
	public TResult Match<TResult>(Func<T, TResult> ifSome, Func<TResult> ifNone) =>
		_isSome
			? ifSome(_value!)
			: ifNone();

	/// <summary>
	/// Matches the current state of the option to a specified result based on the presence or absence of a value.
	/// </summary>
	/// <typeparam name="TResult">The type of the result to return.</typeparam>
	/// <param name="ifSome">A function to invoke when the option is in the 'Some' state.</param>
	/// <param name="ifNone">A function to invoke when the option is in the 'None' state.</param>
	/// <returns>The result of invoking the corresponding function, based on the current state of the option.</returns>
	public TResult Match<TResult>(Func<T, TResult> ifSome, TResult ifNone) =>
		_isSome
			? ifSome(_value!)
			: ifNone;

	/// <summary>
	/// Evaluates the current <see cref="Option{T}"/> state and returns a value based on whether the option is in the 'Some' or 'None' state.
	/// </summary>
	/// <typeparam name="TResult">The type of the result to return from the match operation.</typeparam>
	/// <param name="ifSome">The value to return if the option is in the 'Some' state.</param>
	/// <param name="ifNone">The value to return if the option is in the 'None' state.</param>
	/// <returns>The value of <paramref name="ifSome"/> if the option is in the 'Some' state; otherwise, the value of <paramref name="ifNone"/>.</returns>
	public TResult Match<TResult>(TResult ifSome, TResult ifNone) =>
		_isSome
			? ifSome
			: ifNone;
}
