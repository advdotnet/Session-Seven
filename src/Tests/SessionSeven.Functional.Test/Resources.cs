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
        public const string ENTITY_ID = "e";

        protected override List<Scene> GetScenes()
        {
            var Entity = new Entity(ENTITY_ID);

            Text
                .Create(Entity)
                .SetFont(content.fonts.pixeloperator_outline_BMF);

            var Scene = new Scene(SCENE_ID);

            Scene.Push(Entity);

            return new List<Scene> { Scene };
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
            var SupportedLanguages = new[] { "en-US", "de-DE", "es-ES" };

            foreach (var Language in SupportedLanguages)
            {
                var GameSettings = new GameSettings()
                {
                    Culture = Language
                };

                using (var GraphicsDevice = Mock.CreateGraphicsDevice())
                using (var Runner = new SessionSevenTestEngine(new TestGame(), Mock.Wrap(GraphicsDevice), Mock.Input, GameSettings))
                {
                    Runner.StartGame();
                    Runner.AdvanceToInteractive();

                    var Text = Runner.Game.World.GetScene(TestGame.SCENE_ID).GetObject(TestGame.ENTITY_ID).Get<Text>();

                    foreach (var ResourceString in GetResourceStrings())
                    {
                        // this throws if the sprite font cannot resolve a character
                        Text.Set(ResourceString, TextDuration.Persistent, Vector2.Zero);
                    }
                }
            }
        }

        private IEnumerable<string> GetResourceStrings()
        {
            var GameLogicAssembly = Assembly.GetAssembly(typeof(Properties.Basement_Resources));
            var ManifestResourceNames = GameLogicAssembly.GetManifestResourceNames();

            foreach (var ManifestResourceName in ManifestResourceNames)
            {
                var ResourceBaseName = System.IO.Path.GetFileNameWithoutExtension(ManifestResourceName);
                var ResourceManager = new ResourceManager(ResourceBaseName, GameLogicAssembly);
                var ResourceSet = ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

                foreach (DictionaryEntry Entry in ResourceSet)
                {
                    yield return Entry.Value.ToString();
                }
            }
        }
    }
}
