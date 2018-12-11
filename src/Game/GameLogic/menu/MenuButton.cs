using Microsoft.Xna.Framework.Audio;
using STACK;
using STACK.Components;
using TomShane.Neoforce.Controls;

namespace SessionSeven
{
    /// <summary>
    /// Button with hover and click sounds
    /// </summary>
    public class MenuButton : Button
    {
        public MenuButton(Manager manager, SoundEffect clickSound, SoundEffect focusSound, GameSettings gameSettings) : base(manager)
        {
            Click += (a, e) => PlaySoundIfAvaiable(clickSound, gameSettings);
            MouseOver += (a, e) => PlaySoundIfAvaiable(focusSound, gameSettings);
        }

        private void PlaySoundIfAvaiable(SoundEffect sound, GameSettings gameSettings)
        {
            if (null != sound && !sound.IsDisposed && !AudioManager.SoundDisabled)
            {
                sound.Play(gameSettings.SoundEffectVolume, 0f, 0f);
            }
        }
    }
}
