using SessionSeven.Components;
using STACK;
using STACK.Components;
using System;

namespace SessionSeven.InventoryItems
{
    [Serializable]
    public abstract class ItemBase : Entity
    {
        public ItemBase(string image, string caption, bool givable = true, bool combinable = true)
        {

            Sprite
                .Create(this)
                .SetRenderStage(RenderStage.PostBloom)
                .SetImage(image);

            Transform
                .Create(this);

            Interaction
                .Create(this)
                .SetGetInteractionsFn(GetInteractions);                

            HotspotSprite
                .Create(this)
                .SetCaption(caption);

            if (givable)
            {
                Givable
                    .Create(this);
            }

            if (combinable)
            {
                Combinable
                    .Create(this);
            }
        }

        protected virtual Interactions GetInteractions()
        {
            return Interactions.None;
        }        
    }
}
