using System;
using Gtk;

namespace UbuconQuiz
{
	public class CategoryButton : Button
	{
		public CategoryButton (Widget widget) : base (widget)
		{
		}

		public Question Quest{ get; set; }
	}
}

