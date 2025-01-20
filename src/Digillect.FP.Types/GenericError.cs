namespace Digillect.FP.Types;

/// <summary>
/// Represents a generic implementation of an error.
/// </summary>
/// <param name="message">The error message describing the generic error.</param>
public sealed class GenericError(string message) : Error(message), IEquatable<GenericError>
{
	/// <inheritdoc />
	public bool Equals(GenericError? other)
	{
		return other != null && Message == other.Message;
	}

	/// <inheritdoc />
	public override bool Equals(object? obj)
	{
		return ReferenceEquals(this, obj) || obj is GenericError other && Equals(other);
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		return Message.GetHashCode();
	}

	/// <summary>
	/// Determines whether two specified <see cref="GenericError"/> instances are equal.
	/// </summary>
	/// <param name="left">The first <see cref="GenericError"/> instance to compare.</param>
	/// <param name="right">The second <see cref="GenericError"/> instance to compare.</param>
	/// <returns>
	/// <c>true</c> if the specified <see cref="GenericError"/> instances are equal; otherwise, <c>false</c>.
	/// </returns>
	public static bool operator ==(GenericError? left, GenericError? right)
	{
		return Equals(left, right);
	}

	/// <summary>
	/// Determines whether two specified <see cref="GenericError"/> instances are not equal.
	/// </summary>
	/// <param name="left">The first <see cref="GenericError"/> instance to compare.</param>
	/// <param name="right">The second <see cref="GenericError"/> instance to compare.</param>
	/// <returns>
	/// <c>true</c> if the specified <see cref="GenericError"/> instances are not equal; otherwise, <c>false</c>.
	/// </returns>
	public static bool operator !=(GenericError? left, GenericError? right)
	{
		return !Equals(left, right);
	}
}
