namespace Digillect.FP.Types;

/// <summary>
/// Provides extension methods for the <see cref="Result{T}"/> type.
/// Note: Task and ValueTask related extension methods have been moved to TaskResultExtensions and ValueTaskResultExtensions classes respectively.
/// </summary>
public static class ResultExtensions
{
	/// <summary>
	/// Extracts the underlying success value from the current <see cref="Result{T}"/> instance.
	/// </summary>
	/// <typeparam name="T">The type of the success result contained in the <see cref="Result{T}"/>.</typeparam>
	/// <param name="result">The current <see cref="Result{T}"/> instance.</param>
	/// <returns>
	/// The success value of the <see cref="Result{T}"/> if it is successful; otherwise, an exception is thrown.
	/// </returns>
	public static T Unwrap<T>(this Result<T> result) => result.Value;
}