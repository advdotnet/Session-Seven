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

		private bool _playerWasOnCollider = false;

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (Game.Ego.DrawScene != DrawScene ||
				(Tree.Basement.WoodenPanel.Visible && !Tree.Basement.WoodenPanel.Enabled))
			{
				return;
			}

			// check if player steps on collider and play sound
			var playerPosition = Game.Ego.Get<Transform>().Position;
			var playerOnCollider = WoodenPanel.Collider.Contains(playerPosition);

			if (!_playerWasOnCollider && playerOnCollider)
			{
				Game.PlaySoundEffect(content.audio._path_ + "squeak_" + World.Get<Randomizer>().CreateInt(1, 4));
			}

			_playerWasOnCollider = playerOnCollider;
		}
	}
}
