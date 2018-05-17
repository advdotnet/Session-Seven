using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using SessionSeven.Components;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using System;
using System.Linq;
using Tree = SessionSeven.Entities.Tree;

namespace SessionSeven.Functional.Test
{
    [TestClass]
    public class Playthrough
    {
        /// <summary>
        /// This test solves the game and creates savegames after each session.
        /// </summary>
        [TestMethod, TestCategory("GPU")]
        public void SolveGame()
        {
            var ScoreTarget = ScoreType.Freedom;

            SessionSevenTestEngine.Execute((runner) =>
            {
                SolveSessionTwo(runner, ScoreTarget);
                SolveSessionThree(runner, ScoreTarget);
                SolveSessionFour(runner, ScoreTarget);
                SolveSessionFive(runner, ScoreTarget);
                SolveSessionSix(runner, ScoreTarget);
                SolveSessionSeven(runner, ScoreTarget);

                AssertAllSessionsAreFinished();
            });
        }

        /// <summary>
        /// This test solves the game, creates and loads savegames after each session.
        /// </summary>
        [TestMethod, TestCategory("GPU")]
        public void SolveGameWithLoadingSaveGames()
        {
            var ScoreTarget = ScoreType.Freedom;

            SessionSevenTestEngine.Execute((runner) =>
            {
                SolveSessionTwo(runner, ScoreTarget);
                var State = runner.SaveState("[Playthrough] After Session Two");
                runner.LoadState(State);
                SolveSessionThree(runner, ScoreTarget);
                State = runner.SaveState("[Playthrough] After Session Three");
                runner.LoadState(State);
                SolveSessionFour(runner, ScoreTarget);
                State = runner.SaveState("[Playthrough] After Session Four");
                runner.LoadState(State);
                SolveSessionFive(runner, ScoreTarget);
                State = runner.SaveState("[Playthrough] After Session Five");
                runner.LoadState(State);
                SolveSessionSix(runner, ScoreTarget);
                State = runner.SaveState("[Playthrough] After Session Six");
                runner.LoadState(State);
                SolveSessionSeven(runner, ScoreTarget);

                AssertAllSessionsAreFinished();
            });
        }


        public void SolveGameWithSaveGames(ScoreType targetscore)
        {
            SessionSevenTestEngine.Execute((runner) =>
            {
                SolveSessionTwo(runner, targetscore);
                runner.SaveState("[Playthrough] After Session Two");
                SolveSessionThree(runner, targetscore);
                runner.SaveState("[Playthrough] After Session Three");
                SolveSessionFour(runner, targetscore);
                runner.SaveState("[Playthrough] After Session Four");
                SolveSessionFive(runner, targetscore);
                runner.SaveState("[Playthrough] After Session Five");
                SolveSessionSix(runner, targetscore);
                runner.SaveState("[Playthrough] After Session Six");
                SolveSessionSeven(runner, targetscore);

                AssertAllSessionsAreFinished();
            });
        }

        [TestMethod, TestCategory("GPU")]
        public void FinishSessionTwo()
        {
            var ScoreTarget = ScoreType.Freedom;

            SessionSevenTestEngine.Execute((runner) =>
            {
                SolveSessionTwo(runner, ScoreTarget);

                Assert.IsTrue(Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Two));
            });
        }

        [TestMethod, TestCategory("GPU")]
        public void BatteryIssue()
        {
            SessionSevenTestEngine.Execute((runner) =>
            {
                var OldBatteryA = Game.Ego.Inventory.AddItem<InventoryItems.BatteryA>();
                Assert.AreEqual(Game.Ego.Inventory.GetItemById(OldBatteryA.ID), OldBatteryA);
                Assert.AreEqual(Tree.InventoryItems.BatteryA, OldBatteryA);
                Game.Ego.Inventory.RemoveItem<InventoryItems.BatteryA>();

                var NewBatteryA = Game.Ego.Inventory.AddItem<InventoryItems.BatteryA>();
                Assert.AreEqual(Game.Ego.Inventory.GetItemById(NewBatteryA.ID), NewBatteryA);
                Assert.AreEqual(Tree.InventoryItems.BatteryA, NewBatteryA);
            });
        }

        [TestMethod, TestCategory("GPU")]
        public void FinishSessionThree()
        {
            var ScoreTarget = ScoreType.Freedom;

            SessionSevenTestEngine.Execute((runner) =>
            {
                SolveSessionTwo(runner, ScoreTarget);
                SolveSessionThree(runner, ScoreTarget);

                Assert.IsTrue(Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Three));
            });
        }

        [TestMethod, TestCategory("GPU")]
        public void FinishSessionFour()
        {
            var ScoreTarget = ScoreType.Freedom;

            SessionSevenTestEngine.Execute((runner) =>
            {
                SolveSessionTwo(runner, ScoreTarget);
                SolveSessionFour(runner, ScoreTarget);

                Assert.IsTrue(Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Four));
            });
        }

        [TestMethod, TestCategory("GPU")]
        public void FinishSessionFive()
        {
            var ScoreTarget = ScoreType.Freedom;

            SessionSevenTestEngine.Execute((runner) =>
            {
                SolveSessionTwo(runner, ScoreTarget);
                SolveSessionThree(runner, ScoreTarget);
                SolveSessionFour(runner, ScoreTarget);
                SolveSessionFive(runner, ScoreTarget);

                Assert.IsTrue(Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Five));
            });
        }

        [TestMethod, TestCategory("GPU")]
        public void FinishSessionSix()
        {
            var ScoreTarget = ScoreType.Freedom;

            SessionSevenTestEngine.Execute((runner) =>
            {
                SolveSessionTwo(runner, ScoreTarget);
                SolveSessionThree(runner, ScoreTarget);
                SolveSessionFour(runner, ScoreTarget);
                SolveSessionFive(runner, ScoreTarget);
                SolveSessionSix(runner, ScoreTarget);

                Assert.IsTrue(Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Six));
            });
        }

        [TestMethod, TestCategory("GPU")]
        public void FinishSessionSeven()
        {
            var ScoreTarget = ScoreType.Freedom;

            SessionSevenTestEngine.Execute((runner) =>
            {
                SolveSessionTwo(runner, ScoreTarget);
                SolveSessionThree(runner, ScoreTarget);
                SolveSessionFour(runner, ScoreTarget);
                SolveSessionFive(runner, ScoreTarget);
                SolveSessionSix(runner, ScoreTarget);
                SolveSessionSeven(runner, ScoreTarget);

                Assert.IsTrue(Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Seven));
            });
        }

        private void AssertAllSessionsAreFinished()
        {
            var AllSessions = Enum.GetValues(typeof(Cutscenes.Sessions)).Cast<Cutscenes.Sessions>();
            foreach (var Session in AllSessions)
            {
                Assert.IsTrue(Tree.Cutscenes.Director.FinishedSession(Session));
            }
        }

        private void SolveSessionTwo(SessionSevenTestEngine runner, ScoreType targetScore)
        {
            runner.Interact(Tree.Basement.MedicalCabinet, Verbs.Open);
            runner.Interact(Tree.InventoryItems.Scissors, Tree.InventoryItems.Bandages, Verbs.Use);
            runner.Interact(Tree.InventoryItems.BandageStrip, Verbs.Use);
            runner.Interact(Tree.Basement.WindowLeft, Verbs.Open);

            runner.AnswerDialog(targetScore);
        }

        private void SolveSessionThree(SessionSevenTestEngine runner, ScoreType targetScore)
        {
            Assert.IsTrue(Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Two));

            runner.Interact(Tree.Basement.LandonPortrait, Verbs.Pick);
            runner.Interact(Tree.InventoryItems.Portrait, Verbs.Look);
            runner.Interact(Tree.InventoryItems.DrawerKey, Tree.Basement.DrawerRight, Verbs.Use);
            runner.Interact(Tree.Basement.DrawerRight, Verbs.Open, false);
            runner.AdvanceToNonInteractive();
            runner.AdvanceToInteractive();
            runner.MouseClick();
            runner.AdvanceToInteractive();
            runner.Interact(Tree.Basement.Shelf, Verbs.Look);
            runner.Interact(Tree.Basement.ShelfBlanketFlashlight, Verbs.Pick);

            runner.AnswerDialog(targetScore);
        }

        private void SolveSessionFour(SessionSevenTestEngine runner, ScoreType scoreType)
        {
            Assert.IsTrue(Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Two));

            runner.Interact(Tree.Basement.GuitarCase, Verbs.Open);
            runner.Interact(Tree.Basement.GuitarStrings, Verbs.Pick);
            runner.Interact(Tree.Basement.Shelf, Verbs.Look);
            runner.Interact(Tree.Basement.ShelfBox, Verbs.Open);
            runner.Interact(Tree.Basement.ShelfBox, Verbs.Open);

            runner.AnswerDialog(scoreType);
        }

        private void SolveSessionFive(SessionSevenTestEngine runner, ScoreType targetScore)
        {
            Assert.IsTrue(Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Two));
            Assert.IsTrue(Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Three));
            Assert.IsTrue(Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Four));

            runner.Interact(Tree.Basement.ToolBar, Verbs.Look);
            runner.Interact(Tree.Basement.Hammer, Verbs.Pick);
            runner.Interact(Tree.Basement.Screwdriver, Verbs.Pick);

            // get RFID antenna on shelf
            runner.Interact(Tree.Basement.Shelf, Verbs.Look);
            runner.Interact(Tree.Basement.RFIDAntennaShelf, Verbs.Pick);
            runner.Interact(Tree.InventoryItems.RFIDAntennaBoxShelf, Verbs.Open);

            // get drone
            runner.Interact(Tree.Basement.CabinetRightDoor, Verbs.Open);
            runner.Interact(Tree.Basement.DronePackage, Verbs.Pick);
            runner.Interact(Tree.InventoryItems.DronePackage, Verbs.Open);

            // get second battery
            runner.Interact(Tree.Basement.ShelfBox, Verbs.Open);

            Assert.IsTrue(Game.Ego.Inventory.HasItem<InventoryItems.BatteryA>());
            Assert.IsTrue(Game.Ego.Inventory.HasItem<InventoryItems.BatteryB>());

            runner.Interact(Tree.InventoryItems.BatteryA, Tree.InventoryItems.RemoteControl, Verbs.Use);
            runner.Interact(Tree.InventoryItems.BatteryB, Tree.InventoryItems.RemoteControl, Verbs.Use);

            // get RFID antenna on cabinet
            BumpDroneAgainstCardBoard(runner);

            runner.Interact(Tree.InventoryItems.RemoteControl, Verbs.Open);
            runner.Interact(Tree.InventoryItems.RemoteControl, Verbs.Open);

            Assert.IsTrue(Game.Ego.Inventory.HasItem<InventoryItems.BatteryA>());
            Assert.IsTrue(Game.Ego.Inventory.HasItem<InventoryItems.BatteryB>());

            runner.Interact(Tree.Basement.RFIDAntennaCabinet, Verbs.Pick);
            runner.Interact(Tree.InventoryItems.RFIDAntennaBoxCabinet, Verbs.Open);
            runner.Interact(Tree.Basement.CabinetRightDoor, Verbs.Close);
            runner.Interact(Tree.InventoryItems.RFIDAntennaCabinet, Verbs.Use);

            SetupRFIDAntenna(runner);

            runner.Interact(Tree.InventoryItems.Screwdriver, Tree.Basement.WoodenPanel, Verbs.Use);
            runner.Interact(Tree.InventoryItems.Hammer, Tree.Basement.WoodenPanel, Verbs.Use);
            runner.Interact(Tree.Basement.WoodenBox, Verbs.Pick);
            runner.Interact(Tree.InventoryItems.WoodenBox, Verbs.Open);

            runner.AnswerDialog(targetScore);
        }

        private void SolveSessionSix(SessionSevenTestEngine runner, ScoreType targetScore)
        {
            Assert.IsTrue(Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Two));
            Assert.IsTrue(Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Three));
            Assert.IsTrue(Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Four));
            Assert.IsTrue(Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Five));

            runner.Interact(Tree.Basement.Hazelnuts, Verbs.Pick);
            runner.Interact(Tree.Basement.ShelfBlanketFlashlight, Verbs.Pick);
            runner.Interact(Tree.InventoryItems.Hazelnuts, Tree.Basement.MouseHole, Verbs.Give);

            runner.Interact(Tree.InventoryItems.Baton, Verbs.Pull);
            runner.Interact(Tree.InventoryItems.GuitarStrings, Tree.InventoryItems.Baton, Verbs.Use);

            runner.Interact(Tree.InventoryItems.BatteryA, Tree.InventoryItems.Flashlight, Verbs.Use);
            runner.Interact(Tree.InventoryItems.BatteryB, Tree.InventoryItems.Flashlight, Verbs.Use);

            // Place nuts            
            PlaceNutInWatchArea(runner);
            PlaceNutsLeftRight(runner);
            PlaceNutInWatchArea(runner);

            runner.Interact(Tree.InventoryItems.Flashlight, Tree.Basement.MouseHole, Verbs.Use);
            runner.Interact(Tree.InventoryItems.BatonWithString, Tree.Basement.MouseHole, Verbs.Use);
            runner.Interact(Tree.InventoryItems.Paperclips, Tree.Basement.CabinetLock, Verbs.Use);

            runner.Interact(Tree.Basement.CabinetLeftDoor, Verbs.Open);

            runner.AnswerDialog(targetScore);
        }

        private void SolveSessionSeven(SessionSevenTestEngine runner, ScoreType targetScore)
        {
            Assert.IsTrue(Tree.Cutscenes.Director.FinishedSession(Cutscenes.Sessions.Six));

            runner.Interact(Tree.Basement.DrillingMachineCable, Tree.Basement.SocketsLeft, Verbs.Use);
            runner.Interact(Tree.Basement.CabinetSawKit, Verbs.Pick);
            runner.Interact(Tree.InventoryItems.SawKit, Tree.Basement.DrillingMachine, Verbs.Use);
            runner.Interact(Tree.Basement.DrillingMachine, Tree.Basement.Door, Verbs.Use);
            runner.Interact(Tree.Basement.Door, Verbs.Open);
            runner.SaveState("BeforeSessionSeven");
            runner.Interact(Tree.Basement.Door, Verbs.Walk);
        }

        private void PlaceNutInWatchArea(SessionSevenTestEngine runner)
        {
            var Target = Tree.Basement.MouseHole.WatchArea.GetClosestPoint(new Vector2(788, 279));

            PlaceNut(runner, Target);
        }

        private void PlaceNutsLeftRight(SessionSevenTestEngine runner)
        {
            PlaceNut(runner, new Vector2(0, 250));
            PlaceNut(runner, new Vector2(999, 250));
        }

        private void PlaceNut(SessionSevenTestEngine runner, Vector2 absolutePosition)
        {
            runner.Interact(Tree.InventoryItems.Hazelnuts, Verbs.Use);

            var ClickPos = Tree.Basement.Scene.Get<Camera>().Transform(absolutePosition);

            runner.MouseClick(ClickPos);
            runner.AdvanceToInteractive();
        }

        private void SetupRFIDAntenna(SessionSevenTestEngine runner)
        {
            var ClickPos = Tree.Basement.Scene.Get<Camera>().Transform(new Vector2(422, 334));

            runner.MouseClick(ClickPos);
            runner.AdvanceToInteractive();

            ClickPos = Tree.Basement.Scene.Get<Camera>().Transform(Basement.WoodenPanel.Collider.XYToVector2());

            runner.MouseClick(ClickPos);
            runner.AdvanceToInteractive();

            ClickPos = Tree.Basement.Scene.Get<Camera>().Transform(new Vector2(620, 333));

            runner.MouseClick(ClickPos);
            runner.AdvanceToInteractive();

            ClickPos = Tree.Basement.Scene.Get<Camera>().Transform(Basement.WoodenPanel.Collider.XYToVector2());

            runner.MouseClick(ClickPos);
            runner.AdvanceToInteractive();
        }

        private void BumpDroneAgainstCardBoard(SessionSevenTestEngine runner)
        {
            int Tries = 0;
            while (!Tree.Basement.RFIDAntennaCabinet.FellDown)
            {
                Assert.AreEqual(2, Tree.InventoryItems.RemoteControl.Get<BatteryCompartment>().GetBatteriesCount());
                runner.Interact(Tree.InventoryItems.Drone, Tree.Basement.RFIDAntennaCabinet, Verbs.Use);
                var Script = runner.Interact(Tree.InventoryItems.RemoteControl, Verbs.Use);

                runner.SelectDialogOption((int)Basement.DroneCommand.On, true);
                runner.SelectDialogOption((int)Basement.DroneCommand.Right, true);
                runner.SelectDialogOption((int)Basement.DroneCommand.Forward, true);
                runner.SelectDialogOption((int)Basement.DroneCommand.Forward, true);
                runner.SelectDialogOption((int)Basement.DroneCommand.Forward, true);
                runner.SelectDialogOption((int)Basement.DroneCommand.Right, true);
                runner.SelectDialogOption((int)Basement.DroneCommand.Forward, true);
                runner.SelectDialogOption((int)Basement.DroneCommand.Forward, true);

                while (!Script.Done)
                {
                    runner.SelectDialogOption((int)Basement.DroneCommand.Forward, true);
                }
                Tries++;
            }

            Console.WriteLine("Drone fly tries: " + Tries);
        }
    }
}
