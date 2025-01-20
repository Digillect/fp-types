namespace Digillect.FP.Types.Tests;

public sealed class ResultContinuationTests
{
	[Test]
	public async Task Then_method_in_map_mode_executes_continuation_when_result_is_successful()
	{
		var result = Result.Success(1).Then(x => x + 1);

		await Assert.That(result.IsSuccess).IsTrue();
		await Assert.That(result.Value).IsTypeOf<int>();
		await Assert.That(result.Value).IsEqualTo(2);
	}

	[Test]
	public async Task Then_method_in_map_mode_does_not_execute_continuation_when_result_is_failed()
	{
		var result = Result.Failure<int>(Error.Generic("Error")).Then(x => x + 1);

		await Assert.That(result.IsFailure).IsTrue();
		await Assert.That(result.Error).IsEqualTo(Error.Generic("Error"));
	}

	[Test]
	public async Task Then_method_in_bind_mode_executes_continuation_when_result_is_successful()
	{
		var result = Result.Success(1).Then(x => Result.Success(x + 1));

		await Assert.That(result.IsSuccess).IsTrue();
		await Assert.That(result.Value).IsTypeOf<int>();
		await Assert.That(result.Value).IsEqualTo(2);
	}

	[Test]
	public async Task Then_method_in_bind_mode_does_not_execute_continuation_when_result_is_failed()
	{
		var result = Result.Failure<int>(Error.Generic("Error")).Then(x => Result.Success(x + 1));

		await Assert.That(result.IsFailure).IsTrue();
		await Assert.That(result.Error).IsEqualTo(Error.Generic("Error"));
	}

	[Test]
	public async Task Synchronous_Then_method_chains()
	{
		var result = Result.Success(1)
			.Then(x => x + 1)
			.Then(x => Result.Success(x + 1))
			.Then(x => x.ToString());

		await Assert.That(result.IsSuccess).IsTrue();
		await Assert.That(result.Value).IsTypeOf<string>();
		await Assert.That(result.Value).IsEqualTo("3");
	}

	[Test]
	public async Task ThenAsync_method_in_map_mode_executes_continuation_when_result_is_successful()
	{
		var result = await Result.Success(1).ThenAsync(x => Task.FromResult(x + 1));

		await Assert.That(result.IsSuccess).IsTrue();
		await Assert.That(result.Value).IsTypeOf<int>().And.IsEqualTo(2);
	}

	[Test]
	public async Task ThenAsync_method_in_map_mode_does_not_execute_continuation_when_result_is_failed()
	{
		var result = await Result.Failure<int>(Error.Generic("Error")).ThenAsync(x => Task.FromResult(x + 1));

		await Assert.That(result.IsFailure).IsTrue();
		await Assert.That(result.Error).IsEqualTo(Error.Generic("Error"));
	}

	[Test]
	public async Task ThenAsync_method_in_bind_mode_executes_continuation_when_result_is_successful()
	{
		var result = await Result.Success(1).ThenAsync(x => Task.FromResult(Result.Success(x + 1)));

		await Assert.That(result.IsSuccess).IsTrue();
		await Assert.That(result.Value).IsTypeOf<int>().And.IsEqualTo(2);
	}

	[Test]
	public async Task ThenAsync_method_in_bind_mode_does_not_execute_continuation_when_result_is_failed()
	{
		var result = await Result.Failure<int>(Error.Generic("Error")).ThenAsync(x => Task.FromResult(Result.Success(x + 1)));

		await Assert.That(result.IsFailure).IsTrue();
		await Assert.That(result.Error).IsEqualTo(Error.Generic("Error"));
	}

	[Test]
	public async Task Then_and_ThenAsync_methods_can_be_chained()
	{
		var result = await Result.Success(1)
			.Then(x => x + 1)
			.ThenAsync(x => Task.FromResult(x + 1))
			.Then(x => x + 1)
			.ThenAsync(x => Task.FromResult(Result.Success(x + 1)));

		await Assert.That(result.IsSuccess).IsTrue();
		await Assert.That(result.Value).IsTypeOf<int>().And.IsEqualTo(5);
	}

	[Test]
	public async Task FailIf_method_returns_failure_when_predicate_is_satisfied()
	{
		var result = Result.Success(1).FailIf(x => x == 1, Error.Generic("Error"));

		await Assert.That(result.IsFailure).IsTrue();
		await Assert.That(result.Error).IsEqualTo(Error.Generic("Error"));
	}

	[Test]
	public async Task FailIf_method_returns_success_when_predicate_is_not_satisfied()
	{
		var result = Result.Success(1).FailIf(x => x == 2, Error.Generic("Error"));

		await Assert.That(result.IsSuccess).IsTrue();
		await Assert.That(result.Value).IsTypeOf<int>().And.IsEqualTo(1);
	}

	[Test]
	public async Task LINQ_style_chaining_with_suceessful_results_works()
	{
		var result =
			from x1 in Result.Success(1)
			from x2 in Result.Success(2)
			let x3 = x1 + x2
			select x3;

		await Assert.That(result.IsSuccess).IsTrue();
		await Assert.That(result.Value).IsTypeOf<int>().And.IsEqualTo(3);
	}

	[Test]
	public async Task LINQ_style_chaining_with_failed_results_works()
	{
		var result =
			from x1 in Result.Success(1)
			from x2 in Result.Failure<int>(Error.Generic("Error"))
			let x3 = x1 + x2
			select x3;

		await Assert.That(result.IsFailure).IsTrue();
		await Assert.That(result.Error).IsEqualTo(Error.Generic("Error"));
	}
}
