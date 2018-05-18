﻿using Microsoft.Xna.Framework;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven.Letter
{

    [Serializable]
    public class Scene : Location
    {
        public Scene() : base(content.rooms.letter.scene)
        {
            Background.Get<Sprite>().SetRenderStage(RenderStage.PostBloom);

            DrawOrder = 500;

            InputDispatcher
                .Create(this)
                .SetOnMouseUpFn(OnMouseUp);
        }

        void OnMouseUp(Vector2 position, MouseButton button)
        {
            Enabled = false;
            Visible = false;
        }
    }
}