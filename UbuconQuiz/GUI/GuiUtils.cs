using System;
using Gtk;

namespace UbuconQuiz
{
	public static class GuiUtils
	{
		public static Label CreateBigLabel (string text, int fontSize)
		{
			
			if (fontSize <= 0) {
				throw new ArgumentOutOfRangeException ("font size must be over 0");
			}
			
			return new Label ("<span font=\"" + fontSize + "\">" + text + "</span>"){UseMarkup = true};
		}

		public static Label CreateBigLabel (string text)
		{
			return CreateBigLabel (text, 50);
		}
		
	}
}

