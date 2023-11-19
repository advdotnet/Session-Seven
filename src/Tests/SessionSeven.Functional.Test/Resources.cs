using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using STACK;
using STACK.TestBase;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace SessionSeven.Functional.Test
{
	public class TestGame : StackGame
	{
		public const string SCENE_ID = "s";
		public const string ENTITY_OUTLINE_ID = "e";
		public const string ENTITY_ID = "e";

		protected override List<Scene> GetScenes()
		{
			var outlineEntity = new Entity(ENTITY_OUTLINE_ID);
			var entity = new Entity(ENTITY_ID);

			Text
				.Create(outlineEntity)
				.SetFont(content.fonts.pixeloperator_outline_BMF);

			Text
				.Create(entity)
				.SetFont(content.fonts.pixeloperator_BMF);

			var scene = new Scene(SCENE_ID);

			scene.Push(outlineEntity, entity);

			return new List<Scene> { scene };
		}

		protected override void OnStart()
		{
			StartWorld();
		}
	}

	[TestClass]
	public class Resources
	{
		[TestMethod]
		public void AllResourceCharactersCanBeResolvedBySpriteFont()
		{
			var supportedLanguages = new[] { "en-US", "de-DE", "es-ES", "pl" };

			foreach (var language in supportedLanguages)
			{
				var gameSettings = new GameSettings()
				{
					Culture = language
				};

				using (var graphicsDevice = Mock.CreateGraphicsDevice())
				using (var runner = new SessionSevenTestEngine(new TestGame(), Mock.Wrap(graphicsDevice), Mock.Input, gameSettings))
				{
					runner.StartGame();
					runner.AdvanceToInteractive();

					var text = runner.Game.World.GetScene(TestGame.SCENE_ID).GetObject(TestGame.ENTITY_OUTLINE_ID).Get<Text>();
					var outlineText = runner.Game.World.GetScene(TestGame.SCENE_ID).GetObject(TestGame.ENTITY_OUTLINE_ID).Get<Text>();

					foreach (var resourceString in GetResourceStrings())
					{
						// this throws if the sprite font cannot resolve a character
						text.Set(resourceString, TextDuration.Persistent, Vector2.Zero);
						outlineText.Set(resourceString, TextDuration.Persistent, Vector2.Zero);
					}
				}
			}
		}

		private IEnumerable<string> GetResourceStrings()
		{
			var gameLogicAssembly = Assembly.GetAssembly(typeof(Properties.Basement_Resources));
			var manifestResourceNames = gameLogicAssembly.GetManifestResourceNames();

			foreach (var manifestResourceName in manifestResourceNames)
			{
				var resourceBaseName = System.IO.Path.GetFileNameWithoutExtension(manifestResourceName);
				var resourceManager = new ResourceManager(resourceBaseName, gameLogicAssembly);
				var resourceSet = resourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

				foreach (DictionaryEntry entry in resourceSet)
				{
					yield return entry.Value.ToString();
				}
			}
		}
	}
}
