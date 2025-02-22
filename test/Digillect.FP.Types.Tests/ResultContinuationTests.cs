namespace Digillect.FP.Types.Tests;

public sealed class ResultContinuationTests
{
	[Test]
	public async Task Map_executes_continuation_when_result_is_successful()
	{
		var result = Result.Success(1).Map(x => x + 1);

		await Assert.That(result.IsSuccess).IsTrue();
		await Assert.That(result.Value).IsTypeOf<int>();
		await Assert.That(result.Value).IsEqualTo(2);
	}

	[Test]
	public async Task Map_does_not_execute_continuation_when_result_is_failed()
	{
		var result = Result.Error<int>(Error.Generic("Error")).Map(x => x + 1);

		await Assert.That(result.IsError).IsTrue();
		await Assert.That(result.Error).IsEqualTo(Error.Generic("Error"));
	}

	[Test]
	public async Task Bind_executes_continuation_when_result_is_successful()
	{
		var result = Result.Success(1).Bind(x => Result.Success(x + 1));

		await Assert.That(result.IsSuccess).IsTrue();
		await Assert.That(result.Value).IsTypeOf<int>();
		await Assert.That(result.Value).IsEqualTo(2);
	}

	[Test]
	public async Task Bind_does_not_execute_continuation_when_result_is_failed()
	{
		var result = Result.Error<int>(Error.Generic("Error")).Bind(x => Result.Success(x + 1));

		await Assert.That(result.IsError).IsTrue();
		await Assert.That(result.Error).IsEqualTo(Error.Generic("Error"));
	}

	[Test]
	public async Task Synchronous_Map_and_Bind_methods_chains()
	{
		var result = Result.Success(1)
			.Map(x => x + 1)
			.Bind(x => Result.Success(x + 1))
			.Map(x => x.ToString());

		await Assert.That(result.IsSuccess).IsTrue();
		await Assert.That(result.Value).IsTypeOf<string>();
		await Assert.That(result.Value).IsEqualTo("3");
	}

	[Test]
	public async Task MapAsync_executes_continuation_with_task_when_result_is_successful()
	{
		var result = await Result.Success(1).MapAsync(x => Task.FromResult(x + 1));

		await Assert.That(result.IsSuccess).IsTrue();
		await Assert.That(result.Value).IsTypeOf<int>().And.IsEqualTo(2);
	}

	[Test]
	public async Task MapAsync_does_not_execute_continuation_with_task_when_result_is_failed()
	{
		var result = await Result.Error<int>(Error.Generic("Error")).MapAsync(x => Task.FromResult(x + 1));

		await Assert.That(result.IsError).IsTrue();
		await Assert.That(result.Error).IsEqualTo(Error.Generic("Error"));
	}

	[Test]
	public async Task BindAsync_executes_continuation_when_result_is_successful()
	{
		var result = await Result.Success(1).BindAsync(x => Task.FromResult(Result.Success(x + 1)));

		await Assert.That(result.IsSuccess).IsTrue();
		await Assert.That(result.Value).IsTypeOf<int>().And.IsEqualTo(2);
	}

	[Test]
	public async Task BindAsync_does_not_execute_continuation_when_result_is_failed()
	{
		var result = await Result.Error<int>(Error.Generic("Error")).BindAsync(x => Task.FromResult(Result.Success(x + 1)));

		await Assert.That(result.IsError).IsTrue();
		await Assert.That(result.Error).IsEqualTo(Error.Generic("Error"));
	}

	[Test]
	public async Task Map_MapAsync_Bind_and_BindAsync_methods_can_be_chained()
	{
		var result = await Result.Success(1)
			.Map(x => x + 1)
			.MapAsync(x => Task.FromResult(x + 1))
			.Map(x => x + 1)
			.BindAsync(x => Task.FromResult(Result.Success(x + 1)));

		await Assert.That(result.IsSuccess).IsTrue();
		await Assert.That(result.Value).IsTypeOf<int>().And.IsEqualTo(5);
	}

	[Test]
	public async Task FailWhen_returns_failure_when_predicate_is_satisfied()
	{
		var result = Result.Success(1).FailWhen(x => x == 1, Error.Generic("Error"));

		await Assert.That(result.IsError).IsTrue();
		await Assert.That(result.Error).IsEqualTo(Error.Generic("Error"));
	}

	[Test]
	public async Task FailWhen_returns_success_when_predicate_is_not_satisfied()
	{
		var result = Result.Success(1).FailWhen(x => x == 2, Error.Generic("Error"));

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
			from x2 in Result.Error<int>(Error.Generic("Error"))
			let x3 = x1 + x2
			select x3;

		await Assert.That(result.IsError).IsTrue();
		await Assert.That(result.Error).IsEqualTo(Error.Generic("Error"));
	}
}
