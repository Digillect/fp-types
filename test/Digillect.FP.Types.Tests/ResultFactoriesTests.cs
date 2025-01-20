using static Digillect.FP.Types.Prelude;

namespace Digillect.FP.Types.Tests;

public sealed class ResultFactoriesTests
{
	[Test]
	public async Task Result_Success_method_creates_successful_result()
	{
		var result = Result.Success(1);

		await Assert.That(result.IsSuccess).IsTrue();
		await Assert.That(result.IsFailure).IsFalse();
		await Assert.That(result.Value).IsEqualTo(1);
	}

	[Test]
	public async Task Result_Failure_method_creates_failed_result()
	{
		var result = Result.Failure<int>(Error.Generic("Error"));

		await Assert.That(result.IsSuccess).IsFalse();
		await Assert.That(result.IsFailure).IsTrue();
		await Assert.That(result.Error).IsEqualTo(Error.Generic("Error"));
	}

	[Test]
	public async Task Prelude_Success_method_creates_successful_result()
	{
		var result = Success(1);

		await Assert.That(result.IsSuccess).IsTrue();
		await Assert.That(result.IsFailure).IsFalse();
		await Assert.That(result.Value).IsEqualTo(1);
	}

	[Test]
	public async Task Prelude_Failure_method_creates_failed_result()
	{
		var result = Failure<int>(Error.Generic("Error"));

		await Assert.That(result.IsSuccess).IsFalse();
		await Assert.That(result.IsFailure).IsTrue();
		await Assert.That(result.Error).IsEqualTo(Error.Generic("Error"));
	}
}
