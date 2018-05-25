using Microsoft.Xna.Framework.Audio;
using TomShane.Neoforce.Controls;

namespace SessionSeven
{
    /// <summary>
    /// Button with hover and click sounds
    /// </summary>
    public class MenuButton : Button
    {
        public MenuButton(Manager manager, SoundEffect clickSound, SoundEffect focusSound) : base(manager)
        {
            Click += (a, e) => PlaySoundIfAvaiable(clickSound);
            MouseOver += (a, e) => PlaySoundIfAvaiable(focusSound);
        }

        private void PlaySoundIfAvaiable(SoundEffect sound)
        {
            if (null != sound && !sound.IsDisposed)
            {
                sound.Play();
            }
        }
    }
}
