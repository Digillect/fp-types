namespace Digillect.FP.Types;

/// <summary>
/// Represents a type with a single, unique value. Often used to signify the absence of meaningful information.
/// </summary>
/// <remarks>
/// The <see cref="Unit"/> type is commonly utilized in functional programming to denote the result of operations
/// or methods that do not return any meaningful value.
/// </remarks>
public readonly struct Unit : IEquatable<Unit>
{
	/// <summary>
	/// Represents the default and sole instance of the <see cref="Unit"/> type.
	/// </summary>
	/// <remarks>
	/// The <see cref="Default"/> field is used to signify the unique value of the <see cref="Unit"/> type,
	/// commonly employed in scenarios where operations or methods do not return any meaningful value.
	/// </remarks>
	public static readonly Unit Default = new();

	/// <inheritdoc />
	public override bool Equals(object? obj)
	{
		return obj is Unit other && Equals(other);
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		return 0;
	}

	/// <inheritdoc />
	public override string ToString()
	{
		return "()";
	}

	/// <inheritdoc />
	public bool Equals(Unit other)
	{
		return true;
	}

	public static bool operator ==(Unit _, Unit __)
	{
		return true;
	}

	public static bool operator !=(Unit _, Unit __)
	{
		return false;
	}
}
