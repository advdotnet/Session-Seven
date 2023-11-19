using Microsoft.Xna.Framework;
using STACK;
using STACK.Components;
using System;
using GlblRes = global::SessionSeven.Properties.Resources;

namespace SessionSeven.Letter
{

	[Serializable]
	public class Scene : Location
	{
		public Scene() : base(GlblRes.RoomsLetterScene)
		{
			Background
				.Get<Sprite>()
				.SetRenderStage(RenderStage.PostBloom);

			DrawOrder = 500;

			InputDispatcher
				.Create(this)
				.SetOnMouseUpFn(OnMouseUp);
		}

		private void OnMouseUp(Vector2 position, MouseButton button)
		{
			Enabled = false;
			Visible = false;
		}
	}
}
