﻿using SessionSeven.Components;
using SessionSeven.Entities;
using SessionSeven.GUI.Dialog;
using SessionSeven.GUI.Interaction;
using STACK;
using STACK.Components;
using STACK.TestBase;
using System;

namespace SessionSeven.Functional.Test
{
	/// <summary>
	/// Game specific extension of the TestEngine.
	/// </summary>
	public class SessionSevenTestEngine : TestEngine
	{

		public SessionSevenTestEngine(StackGame game, IServiceProvider services, TestInputProvider input, GameSettings settings = null)
			: base(game, services, input, settings) { }

		/// <summary>
		/// Creates a new SessionSevenTestEngine and executes an action with it.
		/// </summary>
		/// <param name="action"></param>
		public static void Execute(Action<SessionSevenTestEngine> action)
		{
			using (var graphicsDevice = Mock.CreateGraphicsDevice())
			using (var runner = new SessionSevenTestEngine(new Game(), Mock.Wrap(graphicsDevice), Mock.Input))
			{
				runner.StartGame();
				runner.AdvanceToInteractive();

				action(runner);
			}
		}

		/// <summary>
		/// Executes an interaction using the game's main actor.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="verb"></param>
		/// <param name="advanceToInteractive"></param>
		/// <returns></returns>
		public Script Interact(Entity obj, LockedVerb verb, bool advanceToInteractive = true)
		{
			return Interact(SessionSeven.Game.Ego, obj, verb, advanceToInteractive);
		}

		/// <summary>
		/// Executes an interaction and optionally advances the resulting script until the game is interactive again.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="obj"></param>
		/// <param name="verb"></param>
		/// <param name="advanceToInteractive"></param>
		/// <returns></returns>
		public Script Interact(Entity sender, Entity obj, LockedVerb verb, bool advanceToInteractive = true)
		{
			AssertThatGetAllInteractionsDoesNotThrow();

			if (!Game.World.Interactive)
			{
				throw new InvalidOperationException(string.Format("World cannot be interacted with."));
			}

			if (!obj.Enabled || !obj.Visible)
			{
				throw new InvalidOperationException(string.Format("Entity {0} cannot be interacted with.", obj));
			}

			var interactions = obj.Get<Interaction>().GetInteractions();
			var interactionsForObject = interactions.GetFor(sender);
			var interactionForVerb = interactionsForObject[verb];
			var script = interactionForVerb(new InteractionContext(sender, obj, null, verb));

			if (advanceToInteractive)
			{
				Advance(() => script.Done || script.WaitsforSelection() || null == Game.World);
				AdvanceToInteractive();
			}

			return script;
		}

		private void AssertThatGetAllInteractionsDoesNotThrow()
		{
			foreach (var scene in Game.World.Scenes)
			{
				foreach (var entity in scene.GameObjectCache.Entities)
				{
					var interaction = entity.Get<Interaction>();
					interaction?.GetInteractions();
				}
			}
		}

		/// <summary>
		/// Selects a dialog option with the given ID.
		/// </summary>
		/// <param name="optionId"></param>
		/// <param name="advanceToInteractive"></param>
		public void SelectDialogOption(int optionId, bool advanceToInteractive = true)
		{
			while (Tree.GUI.Dialog.Menu.State == DialogMenuState.Opening)
			{
				Tick();
			}

			if (Tree.GUI.Dialog.Menu.State != DialogMenuState.Open)
			{
				return;
			}

			Tree.GUI.Dialog.Menu.SelectOption(optionId);
			Tick();

			if (advanceToInteractive)
			{
				AdvanceToInteractive();
			}
		}

		/// <summary>
		/// Selects any (score) dialog option with the given score. If no such option was found,
		/// a random option is selected.
		/// </summary>
		/// <param name="scoreType"></param>
		/// 
		public void SelectScoreDialogOption(ScoreType scoreType)
		{
			for (var i = 0; i <= Tree.GUI.Dialog.Menu.Options.Count; i++)
			{
				var option = Tree.GUI.Dialog.Menu.Options[i] as ScoreOption;
				if (option.ScoreSet.ContainsKey(scoreType))
				{
					SelectDialogOption(option.ID);
					return;
				}
			}

			SelectDialogOption(new Random().Next(0, Tree.GUI.Dialog.Menu.Options.Count));
		}

		/// <summary>
		/// Answers all upcoming dialog choices using the given score type.
		/// </summary>
		/// <param name="scoreType"></param>
		/// <param name="advanceToInteractive"></param>
		public void AnswerDialog(ScoreType scoreType, bool advanceToInteractive = true)
		{
			while (Tree.GUI.Dialog.Menu.State == DialogMenuState.Opening)
			{
				Tick();
			}

			while (Tree.GUI.Dialog.Menu.State == DialogMenuState.Open)
			{
				SelectScoreDialogOption(scoreType);
			}

			if (advanceToInteractive)
			{
				AdvanceToInteractive();
			}
		}
	}
}
