using System;
using Gtk;
using Hyena.Data.Gui;

namespace UbuconQuiz
{
	public class ListViewTest : HBox
	{
		public ListViewTest ()
		{
			build ();
		}
		
		private void build ()
		{
			var listview = new ListView<Question> ();
			this.Add (listview);
		}
	}
	
}

