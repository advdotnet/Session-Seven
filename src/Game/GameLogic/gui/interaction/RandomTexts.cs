using System.Collections.Generic;
using GlblRes = global::SessionSeven.Properties.Resources;

namespace SessionSeven.GUI.Interaction
{
	public static class RandomTexts
	{
		public static readonly List<string> Default = new List<string>()
		{
			GlblRes.Cant_do_that
		};

		public static readonly List<string> Pick = new List<string>()
		{
			GlblRes.There_is_no_need_to_carry_that_around,
			GlblRes.I_dont_need_that,
			GlblRes.I_dont_see_the_point_in_taking_that
		};

		public static readonly List<string> PickInventory = new List<string>()
		{
			GlblRes.I_have_got_this_already,
			GlblRes.I_have_it_already,
			GlblRes.Im_carrying_that
		};

		public static readonly List<string> Move = new List<string>()
		{
			GlblRes.I_dont_want_to_move_that,
			GlblRes.There_is_no_need_to_move_that
		};

		public static readonly List<string> Use = new List<string>()
		{
			GlblRes.Cant_use_that,
			GlblRes.I_cant_do_that,
			GlblRes.That_does_not_work
		};

		public static readonly List<string> Look = new List<string>()
		{
			GlblRes.I_dont_see_anything_special,
			GlblRes.Nothing_to_see_here
		};

		public static readonly List<string> Talk = new List<string>()
		{
			GlblRes.I_guess_there_wont_be_an_answer,
			GlblRes.I_cant_talk_to_that
		};

		public static readonly List<string> Close = new List<string>()
		{
			GlblRes.I_cant_close_that,
			GlblRes.This_cant_be_closed,
			GlblRes.That_does_not_need_to_be_closed
		};

		public static readonly List<string> Open = new List<string>()
		{
			GlblRes.I_cant_open_that,
			GlblRes.This_cant_be_opened,
			GlblRes.That_does_not_need_to_be_opened
		};

		public static readonly List<string> Give = new List<string>()
		{
			GlblRes.I_cant_give_that_to_somebody,
			GlblRes.No,
			GlblRes.How
		};
	}
}
