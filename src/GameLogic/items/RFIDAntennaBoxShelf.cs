using SessionSeven.Entities;
using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
    [Serializable]
    public class RFIDAntennaBoxShelf : ItemBase
    {
        public RFIDAntennaBoxShelf() : base(content.inventory.cardboard, Items_Res.RFIDAntennaBoxCabinet_RFIDAntennaBoxCabinet_Cardboard)
        {
        }

        protected override Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Look, LookScript())
                    .Add(Verbs.Open, OpenScript())
                    .Add(Verbs.Use, OpenScript());
        }

        IEnumerator OpenScript()
        {
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.StartUse();
                yield return Game.WaitForSoundEffect(content.audio.cardboard_open);

                Game.Ego.Inventory.RemoveItem(this);
                Game.Ego.Inventory.AddItem<RFIDAntennaShelf>();
                yield return Game.Ego.StopUse();

                if (Game.Ego.Inventory.HasItem<RFIDAntennaCabinet>())
                {
                    yield return Game.Ego.Say(Items_Res.Another_longrange_RFID_antenna);
                }
                else
                {
                    yield return Game.Ego.StartScript(Tree.InventoryItems.RFIDAntennaShelf.LookScript(false));
                }
            }
        }

        IEnumerator LookScript()
        {
            using (Game.CutsceneBlock())
            {
                var HasOtherAntenna = Game.Ego.Inventory.HasItem<RFIDAntennaShelf>();

                yield return Game.Ego.Say(Items_Res.A_cardboard_without_any_inscription);

                if (Game.Ego.Inventory.HasItem<RFIDAntennaBoxCabinet>())
                {
                    yield return Game.Ego.Say(Items_Res.Looks_extactly_like_the_other_cardboard_I_got);
                }

                if (HasOtherAntenna)
                {
                    yield return Game.Ego.Say(Items_Res.Looks_extactly_like_the_other_cardboard_which_contained_the_RFID_antenna);
                }

                yield return Game.Ego.Say(Items_Res.Must_be_Cynthias_or_Landons);

                if (!HasOtherAntenna)
                {
                    yield return Game.Ego.Say(Items_Res.I_wonder_what_is_inside);
                }
            }
        }
    }
}
