using Microsoft.VisualStudio.TestTools.UnitTesting;
using SessionSeven.Entities;
using STACK;
using STACK.Test;
using System;

namespace SessionSeven.Functional.Test
{
    [TestClass]
    public class Cutscene
    {
        [TestMethod]
        public void CutsceneDisposeControlTestResetGUIAndWorldInteractive()
        {
            var World = new World(new TestServiceProvider());
            Tree.World = World;
            var Reset = false;
            Action ResetFn = () => Reset = true;

            using (new CutsceneDisposeControl(World, ResetFn))
            {
                Assert.IsFalse(World.Interactive);
                Assert.IsFalse(Reset);
            }
            Assert.IsTrue(Reset);
            Assert.IsTrue(World.Interactive);
        }

        [TestMethod]
        public void CutsceneDisposeControlTestResetWorldInteractive()
        {
            var World = new World(new TestServiceProvider());
            Tree.World = World;
            var Reset = false;
            Action ResetFn = () => Reset = true;

            using (new CutsceneDisposeControl(World, ResetFn, true, false))
            {
                Assert.IsFalse(World.Interactive);
                Assert.IsFalse(Reset);
            }
            Assert.IsFalse(Reset);
            Assert.IsTrue(World.Interactive);
        }

        [TestMethod]
        public void CutsceneDisposeControlTestResetGUI()
        {
            var World = new World(new TestServiceProvider());
            Tree.World = World;
            var Reset = false;
            Action ResetFn = () => Reset = true;

            using (new CutsceneDisposeControl(World, ResetFn, false, true))
            {
                Assert.IsFalse(World.Interactive);
                Assert.IsFalse(Reset);
            }
            Assert.IsTrue(Reset);
            Assert.IsFalse(World.Interactive);
        }
    }
}
