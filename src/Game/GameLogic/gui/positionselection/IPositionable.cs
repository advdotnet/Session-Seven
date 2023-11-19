using Microsoft.Xna.Framework;

namespace SessionSeven.GUI.PositionSelection
{
	public interface IPositionable
	{
		void BeginPosition(int mode);
		void SetPosition(Vector2 position, int mode);
		void EndPosition(int mode);
	}
}
