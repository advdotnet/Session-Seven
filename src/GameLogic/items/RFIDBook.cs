using SessionSeven.GUI.Interaction;
using STACK;
using System;
using System.Collections;
using Items_Res = global::SessionSeven.Properties.Items_Resources;

namespace SessionSeven.InventoryItems
{
    [Serializable]
    public class RFIDBook : ItemBase
    {
        public RFIDBook()
            : base(content.inventory.rfidbook, Items_Res.RFIDBook_RFIDBook_RFIDBook, true, false)
        {

        }

        protected override Interactions GetInteractions()
        {
            return Interactions
                .Create()
                .For(Game.Ego)
                    .Add(Verbs.Open, LookScript())
                    .Add(Verbs.Use, LookScript())
                    .Add(Verbs.Look, LookScript());
        }

        IEnumerator LookScript()
        {
            using (Game.CutsceneBlock())
            {
                var HasAnyAntenna = Game.Ego.Inventory.HasItem<RFIDAntennaCabinet>() ||
                    Game.Ego.Inventory.HasItem<RFIDAntennaShelf>();

                if (HasAnyAntenna)
                {
                    yield return Game.Ego.Say(Items_Res.I_wonder_what_Landon_used_those_antennas_for);
                    yield return Game.Ego.Say(Items_Res.Here_is_a_summary_in_the_book_describing_RFID_antennas);

                    yield return Game.Ego.Say(Items_Res._Radio_Frequency_Identification_allows_to_identify_objects_using_data_transmitted_via_radio_waves_);
                    yield return Game.Ego.Say(Items_Res._RFID_antennas_send_the_RF_energy_to_those_objects_called_RFID_tags_and_receive_the_tags_reply_);
                    yield return Game.Ego.Say(Items_Res._No_line_of_sight_is_required_between_the_antenna_and_the_tag_Directional_antennas_allow_for_a_greater_coverage_distance_);
                    yield return Game.Ego.Say(Items_Res._Using_two_directional_RFID_antennas_the_actual_position_of_a_RFID_tag_can_be_triangulated_);
                }
                else
                {
                    yield return Game.Ego.Say(Items_Res.Its_a_book_about_RFID_technology_I_wrote_a_research_paper_about_it_in_university);
                }
            }
        }
    }

}
