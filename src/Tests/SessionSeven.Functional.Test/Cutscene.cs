using Microsoft.VisualStudio.TestTools.UnitTesting;
using SessionSeven.Entities;
using STACK;
using STACK.TestBase;

namespace SessionSeven.Functional.Test
{
	[TestClass]
	public class Cutscene
	{
		[TestMethod]
		public void CutsceneDisposeControlTestResetGUIAndWorldInteractive()
		{
			var world = new World(new TestServiceProvider());
			Tree.World = world;
			var reset = false;
			void ResetFn() => reset = true;

			using (new CutsceneDisposeControl(world, ResetFn))
			{
				Assert.IsFalse(world.Interactive);
				Assert.IsFalse(reset);
			}
			Assert.IsTrue(reset);
			Assert.IsTrue(world.Interactive);
		}

		[TestMethod]
		public void CutsceneDisposeControlTestResetWorldInteractive()
		{
			var world = new World(new TestServiceProvider());
			Tree.World = world;
			var reset = false;
			void ResetFn() => reset = true;

			using (new CutsceneDisposeControl(world, ResetFn, true, false))
			{
				Assert.IsFalse(world.Interactive);
				Assert.IsFalse(reset);
			}
			Assert.IsFalse(reset);
			Assert.IsTrue(world.Interactive);
		}

		[TestMethod]
		public void CutsceneDisposeControlTestResetGUI()
		{
			var world = new World(new TestServiceProvider());
			Tree.World = world;
			var reset = false;
			void ResetFn() => reset = true;

			using (new CutsceneDisposeControl(world, ResetFn, false, true))
			{
				Assert.IsFalse(world.Interactive);
				Assert.IsFalse(reset);
			}
			Assert.IsTrue(reset);
			Assert.IsFalse(world.Interactive);
		}
	}
}
