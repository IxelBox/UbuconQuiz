using System;
using System.Text;
using Gtk;
using Hyena.Widgets;

namespace UbuconQuiz
{
	public class FinishDisplay : Gtk.Alignment, IMainContent
	{
		IPlayers players;

		public FinishDisplay () : base(0.5f, 0.5f,1,1)
		{
			players = MessageBus.Instance.Players;
			Label label = new Label ();
			
			StringBuilder sb = new StringBuilder ();
			sb.Append ("<span font=\"50\">");
			if (players.Player1.Score == players.Player2.Score) {
				sb.Append ("Unentschieden");
			} else if (players.Player1.Score > players.Player2.Score) {
				sb.Append (players.Player1.Name);
				sb.Append (" Win");
			} else {
				sb.Append (players.Player2.Name);
				sb.Append (" Win");
			}
			sb.Append ("</span>");
			label.Markup = sb.ToString ();
			this.Add (label);
			this.ShowAll ();
		}
		
	}
}

