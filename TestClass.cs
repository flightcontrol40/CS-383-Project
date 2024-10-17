namespace GdMUT;
using Chicken;
using Godot;
using System;

/// <summary>
/// This is a test class for GDMUT. This is purely for demonstration. If you added
/// this into your project, feel free to delete it =).
/// </summary>
/*
public static class TestClass
{

private const string TEST_CHICKEN = "res://src/Clayton/Enemy/BaseChicken.tscn";
#if TOOLS
	/// <summary>
	/// An example of a result that will pass.
	/// </summary>
	/// <returns>The result.</returns>
	[CSTestFunction]
	public static Result ExamplePass()
	{
		int x = 0;
		x *= 100;
		return (x == 0) ? Result.Success : Result.Failure;
	}

	/// <summary>
	/// An example of a result that will fail.
	/// </summary>
	/// <returns>The result.</returns>
	[CSTestFunction]
	public static Result ExampleFail()
	{
		int x = 0;
		x *= 100;
		return (x != 0) ? Result.Success : Result.Failure;
	}

	/// <summary>
	/// An example of a result that will fail with a custom message.
	/// </summary>
	/// <returns>The result.</returns>
	[CSTestFunction]
	public static Result ExampleCustomFail()
	{
		int x = 0;
		x *= 100;
		return (x != 0)
			? Result.Success
			: new Result(false, "You can't multiply 0 and expect anything else than 0!");
	}

	/// <summary>
	/// An example of a result that will pass with a custom message.
	/// </summary>
	/// <returns>The result.</returns>
	[CSTestFunction]
	public static Result ExampleCustomSuccess()
	{
		int x = 0;
		x *= 100;
		return (x == 0) ? new Result(true, "Proved that 0 * 100 = 0") : Result.Failure;
	}
	
[CSTestFunction]
public static Result ChickenTest()
{
	try
	{
		var testResulScene = GD.Load<PackedScene>(TEST_CHICKEN);
	
		var testResul = (BaseChicken)testResulScene.Instantiate();
	
		int x = testResul.Health;

		return (x == 100) ? new Result(true, "Proved that 100 = 100") : Result.Failure;
	}
	catch (Exception ex)
	{
		return new Result(false, $"Exception occurred: {ex.Message}");
	}
}




	
#endif
}
*/
