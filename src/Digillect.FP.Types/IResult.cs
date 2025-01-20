namespace Digillect.FP.Types;

/// <summary>
/// Defines a contract for handling the result of an operation.
/// The result encapsulates either a successful outcome or an error
/// and provides information to distinguish between success and failure.
/// </summary>
public interface IResult
{
	/// <summary>
	/// Indicates whether the result represents a successful operation.
	/// </summary>
	bool IsSuccess { get; }

	/// <summary>
	/// Indicates whether the result represents a failed operation.
	/// </summary>
	bool IsFailure { get; }

	/// <summary>
	/// Gets the error associated with a failed result. Throws an exception if the result represents a success.
	/// </summary>
	/// <exception cref="InvalidOperationException">
	/// Thrown when attempting to access the error of a successful result.
	/// </exception>
	Error Error { get; }
}
