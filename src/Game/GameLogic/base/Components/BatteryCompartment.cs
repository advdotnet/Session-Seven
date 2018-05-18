using STACK;
using STACK.Components;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.Components
{
    /// <summary>
    /// Entity can be combined with other entities by the "USE" verb.
    /// </summary>
    [Serializable]
    public class BatteryCompartment : Component
    {
        public bool BatteryAInstalled { get; private set; }
        public bool BatteryBInstalled { get; private set; }

        public IEnumerator InstallBatteryAScript(bool install)
        {
            Game.Ego.Turn(Directions4.Up);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.StartUse();

                BatteryAInstalled = install;

                if (install)
                {
                    Game.PlaySoundEffect(content.audio.battery_insert);
                    Game.Ego.Inventory.RemoveItem<InventoryItems.BatteryA>();
                }
                else
                {
                    Game.PlaySoundEffect(content.audio.battery_remove);
                    Game.Ego.Inventory.AddItem<InventoryItems.BatteryA>();
                }

                yield return Game.Ego.StopUse();
            }
        }

        public IEnumerator InstallBatteryBScript(bool install)
        {
            Game.Ego.Turn(Directions4.Up);
            using (Game.CutsceneBlock())
            {
                yield return Game.Ego.StartUse();

                BatteryBInstalled = install;

                if (install)
                {
                    Game.PlaySoundEffect(content.audio.battery_insert);
                    Game.Ego.Inventory.RemoveItem<InventoryItems.BatteryB>();
                }
                else
                {
                    Game.PlaySoundEffect(content.audio.battery_remove);
                    Game.Ego.Inventory.AddItem<InventoryItems.BatteryB>();
                }

                yield return Game.Ego.StopUse();
            }
        }

        public IEnumerator OpenScript()
        {
            if (BatteryAInstalled)
            {
                yield return Game.Ego.StartScript(InstallBatteryAScript(false));
            }
            else if (BatteryBInstalled)
            {
                yield return Game.Ego.StartScript(InstallBatteryBScript(false));
            }
            else
            {
                using (Game.CutsceneBlock())
                {
                    yield return Game.Ego.Say(Items_Res.The_battery_compartment_is_empty);
                }
            }
        }

        public string GetDescriptionString()
        {
            switch (GetBatteriesCount())
            {
                case 0:
                    return Items_Res.It_is_empty;

                case 1:
                    return Items_Res.One_battery_is_installed;

                default:
                    return Items_Res.Two_batteries_are_installed;
            }
        }

        public int GetBatteriesCount()
        {
            int Result = 0;

            if (BatteryAInstalled)
            {
                Result++;
            }
            if (BatteryBInstalled)
            {
                Result++;
            }

            return Result;
        }

        public static BatteryCompartment Create(Entity entity)
        {
            return entity.Add<BatteryCompartment>();
        }
    }
}
