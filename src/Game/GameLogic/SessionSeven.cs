using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SessionSeven.Components;
using SessionSeven.Entities;
using STACK;
using STACK.Components;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SessionSeven
{
    /// <summary>
    /// The game class
    /// </summary>
    public class Game : StackGame
    {
        public const int VIRTUAL_WIDTH = 640;
        public const int VIRTUAL_HEIGHT = 400;
        public const string TITLE = "Session Seven";
        public const string SAVEGAMEFOLDER = TITLE;

        public Game()
        {
            VirtualResolution = new Point(VIRTUAL_WIDTH, VIRTUAL_HEIGHT);
            Title = TITLE;
            SaveGameFolder = SAVEGAMEFOLDER;
        }

        /// <summary>
        /// Intended for use within a using statement. Makes the game world non-interactive 
        /// during the code block spanning the using statement.
        /// </summary>
        /// <param name="resetInteractive">Set the interactive flag to true in the end</param>
        /// <param name="resetGUI">Reset the GUI in the end</param>
        /// <returns></returns>
        public static IDisposable CutsceneBlock(bool resetInteractive = true, bool resetGUI = true)
        {
            return new CutsceneDisposeControl(Tree.World, Tree.GUI.Interaction.Scene.Reset, resetInteractive, resetGUI);
        }

        /// <summary>
        /// Screen padding in px
        /// </summary>
        public const int SCREEN_PADDING = 5;

        public static Actors.Ryan Ego
        {
            get
            {
                return Tree.Actors.Ryan;
            }
        }

        public static Actors.Mouse Mouse
        {
            get
            {
                return Tree.Actors.Mouse;
            }
        }

        /// <summary>
        /// Plays a sound effect and returns the SoundEffectInstance.        
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static SoundEffectInstance PlaySoundEffect(string fileName, bool looped = false, STACK.Components.AudioEmitter emitter = null, STACK.Components.AudioListener listener = null)
        {
            return Tree.World.Get<AudioManager>().PlaySoundEffect(fileName, looped, emitter, listener);
        }

        public static IEnumerator WaitForSoundEffect(string fileName)
        {
            using (var SoundEffect = PlaySoundEffect(fileName))
            {
                yield return Script.WaitFor(SoundEffect);
            }
        }

        /// <summary>
        /// Plays a song.        
        /// </summary>
        /// <param name="fileName"></param>
        public static void PlaySong(string fileName)
        {
            Tree.World.Get<AudioManager>().PlaySong(fileName);
        }

        public static void PlayBasementEndSong()
        {
            StopSong();
            PlaySong(content.audio.basementend);
            Tree.World.Get<AudioManager>().RepeatSong = false;
        }

        /// <summary>
        /// Enqueues a song.        
        /// </summary>
        /// <param name="fileName"></param>
        public static void EnqueueSong(string fileName)
        {
            Tree.World.Get<AudioManager>().EnqueueSong(fileName);
        }

        public static void PauseSong()
        {
            Tree.World.Get<AudioManager>().PauseSong();
        }

        public static void ResumeSong()
        {
            Tree.World.Get<AudioManager>().ResumeSong();
        }

        public static void StopSong()
        {
            Tree.World.Get<AudioManager>().StopSong();
        }

        protected override List<Scene> GetScenes()
        {
            var EgoInv = new Inventory();

            return new List<Scene>()
            {
                EgoInv.Scene,
                new Cutscenes.Scene(),
                new Basement.Scene(),
                new JailCell.Scene(),
                new PaddedCell.Scene(),
                new SunSet.Scene(),
                new Office.Scene(),
                new Title.Scene(),
                new Letter.Scene(),
                new Actors.Scene(EgoInv),
                new GUI.Scene(),
                new GUI.Interaction.Scene(),
                new GUI.Dialog.Scene(),
                new GUI.PositionSelection.Scene()
            };
        }

        public static Menu MainMenu;

        protected override void OnStart()
        {
            MainMenu = new Menu(Engine);
        }

        public override void OnExit()
        {
            if (null != MainMenu)
            {
                MainMenu.Dispose();
            }
        }

        protected override void OnWorldInitialized(bool restore)
        {
            if (restore)
            {
                Tree.Reset(World);
                return;
            }

            ShadowRectangle
                .Create(World);

            Engine.Renderer.GUIManager.ShowSoftwareCursor = false;
            Tree.Reset(World);
            World.Interactive = false;
            Ego.EnterScene(Tree.Basement.SceneID);

            Tree.Actors.Ryan.EnterScene(Tree.Basement.SceneID);
            Tree.Actors.RyanVoice.EnterScene(Tree.Basement.SceneID);
            Tree.Actors.Mouse.EnterScene(Tree.Basement.SceneID);

            Tree.Cutscenes.Director.StartSession(Cutscenes.Sessions.One);
        }

        /// <summary>
        /// Fast forwards (updates are called as fast as possible, no frames are drawn,
        /// no sounds or music is played) the game until StopSkipping is called.
        /// </summary>
        public static void EnableSkipping()
        {
            var SkipContent = Tree.World.Get<SkipContent>();

            if (null != SkipContent && null != SkipContent.SkipCutscene)
            {
                SkipContent.SkipCutscene.Possible = true;
            }
        }

        /// <summary>
        /// Disables fast forwarding.
        /// </summary>
        public static void StopSkipping()
        {
            var SkipContent = Tree.World.Get<SkipContent>();

            if (null != SkipContent && null != SkipContent.SkipCutscene)
            {
                SkipContent.SkipCutscene.Possible = false;
                SkipContent.SkipCutscene.Stop();
            }
        }

        public static void EnableTextSkipping(bool enabled = true)
        {
            var SkipContent = Tree.World.Get<SkipContent>();

            if (null != SkipContent && null != SkipContent.SkipText)
            {
                SkipContent.SkipText.Possible = enabled;
            }
        }
    }
}
