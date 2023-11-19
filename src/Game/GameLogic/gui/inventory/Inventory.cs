using Microsoft.Xna.Framework;
using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using SessionSeven.InventoryItems;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven
{
	[Serializable]
	public class Inventory
	{
		private readonly Vector2 _topLeft = new Vector2(328, Verbs.OFFSET + 18);
		private readonly Vector2 _itemDimension = new Vector2(78, 47);

		public const int ItemsPerRow = 4;
		public const int Rows = 2;
		public const int ItemWidth = 78;

		public int ScrollIndex { get; private set; }

		[Serializable]
		public class InventoryScene : STACK.Scene
		{
			public InventoryScene()
			{
				DrawOrder = 123;
			}
		}

		public InventoryScene Scene { get; private set; }

		public Inventory()
		{
			Scene = new InventoryScene();
			ScrollIndex = 0;
		}

		public void Show()
		{
			Scene.Visible = true;
			Scene.Enabled = true;
			Tree.GUI.Interaction.Scene.Show();
		}

		public void Hide()
		{
			Scene.Visible = false;
			Scene.Enabled = true;
			Tree.GUI.Interaction.Scene.Hide();
		}

		public bool Visible => Scene.Visible;

		public bool HasItem(string id)
		{
			return null != Scene.GetObject(id);
		}

		public bool HasItem<T>() where T : ItemBase
		{
			foreach (var item in Scene.GameObjectCache.Entities)
			{
				if (typeof(T).IsAssignableFrom(item.GetType()))
				{
					return true;
				}
			}

			return false;
		}

		public Entity GetItemById(string id)
		{
			return (ItemBase)Scene.GetObject(id);
		}

		private void AddItem(Entity item)
		{
			Scene.Push(item);
			ScrollIndex++;
			Update();
		}

		public ItemBase AddItem<T>() where T : ItemBase
		{
			var item = Activator.CreateInstance<T>();
			AddItem(item);
			return item;
		}

		public void RemoveItem(Entity item)
		{
			Scene.Pop(item);
			Update();
		}

		public void RemoveItem<T>() where T : ItemBase
		{
			foreach (var item in Scene.GameObjectCache.Entities)
			{
				if (typeof(T).IsAssignableFrom(item.GetType()))
				{
					RemoveItem(item);
					return;
				}
			}
		}

		public void ScrollBy(int value)
		{
			ScrollIndex += value;
			Update();
		}

		public bool CanScrollUp => ScrollIndex > 0;

		public bool CanScrollDown => Scene.GameObjectCache.Entities.Count - (ScrollIndex * ItemsPerRow) > ItemsPerRow * Rows;

		private void Update()
		{
			var itemsCount = Scene.GameObjectCache.Entities.Count;

			ScrollIndex = Math.Max(0, ScrollIndex);
			ScrollIndex = Math.Min(((itemsCount - 1) / ItemsPerRow) - 1, ScrollIndex);

			if (itemsCount - (ItemsPerRow * Rows) <= 0)
			{
				ScrollIndex = 0;
			}

			var real = 0;

			for (var i = 0; i < itemsCount; i++)
			{
				if (!(Scene.GameObjectCache.Entities[i] is ItemBase))
				{
					continue;
				}

				var inVisibleRange = ScrollIndex * ItemsPerRow <= i && (ScrollIndex + 1) * ItemsPerRow * Rows > i;

				Scene.GameObjectCache.Entities[i].Visible = inVisibleRange;
				Scene.GameObjectCache.Entities[i].Enabled = inVisibleRange;

				if (inVisibleRange)
				{
					var xIndex = real % ItemsPerRow;
					var yIndex = real / ItemsPerRow;
					var position = _topLeft + new Vector2(xIndex * _itemDimension.X, yIndex * _itemDimension.Y);

					Scene.GameObjectCache.Entities[i].Get<Transform>().Position = position;
					real++;
				}
			}
		}
	}
}
