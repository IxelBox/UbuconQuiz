using System;

namespace UbuconQuiz
{
	public class Players
	{
		
		private static Players instance;

		public static Players Instance {
			get {
				if (instance == null)
					instance = new Players ();
				return instance;
			}
		}

		public event EventHandler<EventArgs> ChangePlayers;

		private Players ()
		{
			Reset ();
		}

		string player1Name;
		string player2Name;
		int player1Score;
		int player2Score;

		public string Player1Name {
			get {
				return this.player1Name;
			}
			set {
				player1Name = value;
				if (ChangePlayers != null)
					ChangePlayers (this, new EventArgs ());
			}
		}

		public string Player2Name {
			get {
				return this.player2Name;
			}
			set {
				player2Name = value;
				if (ChangePlayers != null)
					ChangePlayers (this, new EventArgs ());
			}
		}

		public int Player1Score {
			get {
				return this.player1Score;
			}
			set {
				player1Score = value;
				if (ChangePlayers != null)
					ChangePlayers (this, new EventArgs ());
			}
		}

		public int Player2Score {
			get {
				return this.player2Score;
			}
			set {
				player2Score = value;
				if (ChangePlayers != null)
					ChangePlayers (this, new EventArgs ());
			}
		}

		public void Reset ()
		{
			Player1Name = "???";
			Player1Score = 0;
			Player2Name = "???";
			Player2Score = 0;
			
		}
	}
}

