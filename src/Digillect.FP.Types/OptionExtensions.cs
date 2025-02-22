namespace Digillect.FP.Types;

public static class OptionExtensions
{
	public static Result<T> ToResult<T>(this Option<T> option, Error errorIfNone) where T : notnull
	{
		return option.Match(
			Result.Success,
			() => Result.Error<T>(errorIfNone));
	}

	public static Result<T> ToResult<T>(this Option<T> option, Func<Error> errorIfNone) where T : notnull
	{
		return option.Match(
			Result.Success,
			() => Result.Error<T>(errorIfNone()));
	}

	public static async Task<Result<T>> ToResult<T>(this Task<Option<T>> option, Error errorIfNone) where T : notnull
	{
		return (await option).Match(
			Result.Success,
			() => Result.Error<T>(errorIfNone));
	}

	public static async Task<Result<T>> ToResult<T>(this Task<Option<T>> option, Func<Error> errorIfNone) where T : notnull
	{
		return (await option).Match(
			Result.Success,
			() => Result.Error<T>(errorIfNone()));
	}
}
