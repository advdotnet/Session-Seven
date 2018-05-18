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
        private readonly Vector2 TopLeft = new Vector2(328, Verbs.OFFSET + 18);
        private readonly Vector2 ItemDimension = new Vector2(78, 47);

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

        public bool Visible
        {
            get
            {
                return Scene.Visible;
            }
        }

        public bool HasItem(string id)
        {
            return (null != Scene.GetObject(id));
        }

        public bool HasItem<T>() where T : ItemBase
        {
            foreach (var Item in Scene.GameObjectCache.Entities)
            {
                if (typeof(T).IsAssignableFrom(Item.GetType()))
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
            var Item = Activator.CreateInstance<T>();
            AddItem(Item);
            return Item;
        }

        public void RemoveItem(Entity item)
        {
            Scene.Pop(item);
            Update();
        }

        public void RemoveItem<T>() where T : ItemBase
        {
            foreach (var Item in Scene.GameObjectCache.Entities)
            {
                if (typeof(T).IsAssignableFrom(Item.GetType()))
                {
                    RemoveItem(Item);
                    return;
                }
            }
        }

        public void ScrollBy(int value)
        {
            ScrollIndex += value;
            Update();
        }

        public bool CanScrollUp
        {
            get { return ScrollIndex > 0; }
        }

        public bool CanScrollDown
        {
            get { return (Scene.GameObjectCache.Entities.Count - ScrollIndex * ItemsPerRow > ItemsPerRow * Rows); }
        }

        void Update()
        {
            int ItemsCount = Scene.GameObjectCache.Entities.Count;

            ScrollIndex = Math.Max(0, ScrollIndex);
            ScrollIndex = Math.Min(((ItemsCount - 1) / ItemsPerRow) - 1, ScrollIndex);

            if (ItemsCount - ItemsPerRow * Rows <= 0) ScrollIndex = 0;

            int real = 0;

            for (int i = 0; i < ItemsCount; i++)
            {
                if (!(Scene.GameObjectCache.Entities[i] is ItemBase))
                {
                    continue;
                }

                bool InVisibleRange = ScrollIndex * ItemsPerRow <= i && (ScrollIndex + 1) * ItemsPerRow * Rows > i;

                Scene.GameObjectCache.Entities[i].Visible = InVisibleRange;
                Scene.GameObjectCache.Entities[i].Enabled = InVisibleRange;

                if (InVisibleRange)
                {
                    var xIndex = real % ItemsPerRow;
                    var yIndex = real / ItemsPerRow;
                    var Position = TopLeft + new Vector2(xIndex * ItemDimension.X, yIndex * ItemDimension.Y);

                    Scene.GameObjectCache.Entities[i].Get<Transform>().Position = Position;
                    real++;
                }
            }
        }
    }
}
