using SessionSeven.Entities;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Basement
{
    [Serializable]
    public class WoodenPanelSoundTrigger : Entity
    {
        public WoodenPanelSoundTrigger()
        {
            Enabled = true;
            Visible = false;
        }

        private bool PlayerWasOnCollider = false;

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (Game.Ego.DrawScene != this.DrawScene ||
                (Tree.Basement.WoodenPanel.Visible && !Tree.Basement.WoodenPanel.Enabled))
            {
                return;
            }

            // check if player steps on collider and play sound
            var PlayerPosition = Game.Ego.Get<Transform>().Position;
            var PlayerOnCollider = WoodenPanel.Collider.Contains(PlayerPosition);

            if (!PlayerWasOnCollider && PlayerOnCollider)
            {
                Game.PlaySoundEffect(content.audio._path_ + "squeak_" + World.Get<Randomizer>().CreateInt(1, 4));
            }

            PlayerWasOnCollider = PlayerOnCollider;
        }
    }
}
