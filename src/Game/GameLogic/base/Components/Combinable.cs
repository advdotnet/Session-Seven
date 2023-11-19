using STACK;
using System;

namespace SessionSeven.Components
{
	/// <summary>
	/// Entity can be combined with other entities by the "USE" verb.
	/// </summary>
	[Serializable]
	public class Combinable : Component
	{
		private bool _isCombinable;

		public bool IsCombinable
		{
			get => _isCombinable;
			set => _isCombinable = value;
		}

		public Combinable()
		{
			_isCombinable = true;
		}

		public static Combinable Create(Entity entity)
		{
			return entity.Add<Combinable>();
		}
	}
}
