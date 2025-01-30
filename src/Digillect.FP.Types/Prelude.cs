using System.Runtime.CompilerServices;

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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Result<T> Failure<T>(Error error)
	{
		return Result.Failure<T>(error);
	}

	/// <summary>
	/// Creates an option containing the specified value.
	/// </summary>
	/// <param name="value">The value to include in the option.</param>
	/// <typeparam name="T">The type of the value. Must be a non-nullable type.</typeparam>
	/// <returns>An option representing the specified value.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Option<T> Optional<T>(T? value) where T : notnull
	{
		return value is not null
			? Option<T>.Some(value)
			: Option<T>.None;
	}
}
