using STACK;
using System;

namespace SessionSeven.Components
{
	/// <summary>
	/// Entity can be given to other entities by using the "GIVE" verb.
	/// </summary>
	[Serializable]
	public class Givable : Component
	{
		private bool _isGivable;

		public bool IsGivable
		{
			get => _isGivable;
			set => _isGivable = value;
		}

		public Givable()
		{
			_isGivable = true;
		}

		public static Givable Create(Entity entity)
		{
			return entity.Add<Givable>();
		}
	}
}
