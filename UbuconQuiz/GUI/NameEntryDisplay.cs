using System;
using Gtk;
using Hyena.Widgets;

namespace UbuconQuiz
{
    public class NameEntryDisplay : Hyena.Widgets.AnimatedVBox, IMainContent
    {

        public NameEntryDisplay ()
        {
            MessageBus.Instance.Players.Reset ();
            this.Spacing = 12;
            build ();
            MessageBus.Instance.BuzzHelper.PressBuzz += BuzzHelper_PressBuzz;
        }

        void BuzzHelper_PressBuzz (object sender, PressBuzzBuzzerEventArgs e)
        {
            if (WaitForPlayer != null && CurrentDialog != null) {
                WaitForPlayer.Button = e.PressButton;
                CurrentDialog.Destroy ();
            }
        }

        Entry entryUserOne;
        Entry entryUserTwo;
        Label LabelButtonUserOne;
        Label LabelButtonUserTwo;
        IPlayer WaitForPlayer;
        Gtk.MessageDialog CurrentDialog;

        void build ()
        {
            var b = new Button ();
            b.HeightRequest = 200;
            b.Label = "Quiz starten!";
            b.Clicked += delegate {
                MessageBus.Instance.Players.Player1.Name = entryUserOne.Text;
                MessageBus.Instance.Players.Player2.Name = entryUserTwo.Text;
                MessageBus.Instance.CurrentState = QuizState.Category;
            };
            this.Add (b);

            //Label, Entry, Device User 2
            var box = new HBox (false, 4);
            box.Add (new Label ("User 2:"));
            entryUserTwo = new Entry ();
            LabelButtonUserTwo = new Label ();
            box.Add (entryUserTwo);
            box.Add (BuzzerToPlayer (MessageBus.Instance.Players.Player2, LabelButtonUserTwo));
            box.Add (LabelButtonUserTwo);
            this.Add (box);

            //Label, Entry, Device User 1
            box = new HBox (false, 4);
            box.Add (new Label ("User 1:"));
            entryUserOne = new Entry ();
            LabelButtonUserOne = new Label ();
            box.Add (entryUserOne);
            box.Add (BuzzerToPlayer (MessageBus.Instance.Players.Player1, LabelButtonUserOne));
            box.Add (LabelButtonUserOne);
            this.Add (box);

            this.ShowAll ();
        }

        private Button BuzzerToPlayer (IPlayer player, Label infoLabel)
        {
            var b = new Button ();
            b.Label = "Buzzer zuweisen";
            b.Clicked += delegate {
                CurrentDialog = new Gtk.MessageDialog (null, Gtk.DialogFlags.Modal, Gtk.MessageType.Question, Gtk.ButtonsType.Cancel, "Bitte Gewünschten Button drücken!");
                WaitForPlayer = player;

                MessageBus.Instance.BuzzHelper.WaitForPress ();

                var result = (ResponseType)CurrentDialog.Run ();

                if (result == ResponseType.None) {
                    infoLabel.Text = WaitForPlayer.Button.ToString ();
                } else {
                    infoLabel.Text = String.Empty;
                }

                CurrentDialog.Dispose ();
                CurrentDialog = null;
                WaitForPlayer = null;
                MessageBus.Instance.BuzzHelper.CancelWait ();
            };
            return b;
        }
    }
}

