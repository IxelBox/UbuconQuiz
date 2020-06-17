using System;
using Gtk;
using Hyena.Widgets;
using Hyena.Gui.Theatrics;

namespace UbuconQuiz
{
    public class PlayerDisplay : HBox
    {
        IPlayers players;

        public PlayerDisplay ()
        {
            players = MessageBus.Instance.Players;
			
            Build ();
            players.ChangePlayers += HandlePlayersChange;
            MessageBus.Instance.BuzzHelper.PressBuzz += BuzzerHelper_PressBuzz;
        }

        void BuzzerHelper_PressBuzz (object sender, PressBuzzBuzzerEventArgs e)
        {
            if (players.Player1.Button == e.PressButton) {
                foreach (var c in ((Container)PlayerOne).Children) {
                    c.ModifyFg (StateType.Normal, new Gdk.Color (0, 125, 0));
                }
            } else if (players.Player2.Button == e.PressButton) {
                foreach (var c in ((Container)PlayerTwo).Children) {
                    c.ModifyFg (StateType.Normal, new Gdk.Color (0, 125, 0));
                }
            } else {
                MessageBus.Instance.BuzzHelper.WaitForPress ();
            }
        }

        public void ResetPressHighlight ()
        {
            foreach (var c in ((Container)PlayerOne).Children) {
                c.ModifyFg (StateType.Normal);
            }

            foreach (var c in ((Container)PlayerTwo).Children) {
                c.ModifyFg (StateType.Normal);
            }
        }

        Widget PlayerOne;
        Widget PlayerTwo;

        void HandlePlayersChange (object sender, EventArgs e)
        {
            foreach (var child in this.Children) {
                var box = child as VBox;
                if (box != null) {
                    foreach (var c in box.Children) {
                        box.Remove (c);
                    }
                }
            }
            this.Remove (PlayerOne);
            this.Remove (PlayerTwo);
            Build ();
        }

        void Build ()
        {
            PlayerOne = CreatePlayerDisplay (players.Player1.Name, players.Player1.Score);
            PlayerTwo = CreatePlayerDisplay (players.Player2.Name, players.Player2.Score);
            this.Add (PlayerOne);
            this.Add (PlayerTwo);
            this.ShowAll ();
        }

        Widget CreatePlayerDisplay (string name, int points)
        {
            var box = new VBox ();
            box.Add (GuiUtils.CreateBigLabel (name, 30));
            var ani = new AnimatedVBox ();
            ani.Add (GuiUtils.CreateBigLabel (points.ToString (), 30));
            box.Add (ani);
            return box;
        }
    }
}

