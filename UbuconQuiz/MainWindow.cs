using System;
using System.Linq;
using Gtk;
using Hyena.Widgets;

namespace UbuconQuiz
{
    public class MainWindow : Gtk.Window
    {
        public const string ProgrammName = "Ubucon Quiz";
        bool IsFullscreen = false;
        VBox mainContainer;
        ButtonDisplay buttonField;
        PlayerDisplay playerDisplay;

        public MainWindow () : base (Gtk.WindowType.Toplevel)
        {
            MainWindowBuild ();
            Configuration ();
        }

        void Configuration ()
        {

            MessageBus.Instance.BuzzHelper.LoadDevice ();

            mainContainer = new VBox ();
            var bus = MessageBus.Instance;
            bus.ChangeState += MessageBusChangeState;
            buttonField = new ButtonDisplay (Buttons.Nothing);
			
            mainContainer.PackStart (new NameEntryDisplay (), true, true, 4);
			
            mainContainer.PackEnd (buttonField, false, false, 4);
            playerDisplay = new PlayerDisplay ();
            mainContainer.PackEnd (playerDisplay, false, false, 4);

            this.Add (mainContainer);
            this.ShowAll ();
        }

        void MessageBusChangeState (object sender, QuizStateEventArgs e)
        {
            var removeWidgets = mainContainer.Children.Where (c => c is IMainContent);
            foreach (var widget in removeWidgets) {
                mainContainer.Remove (widget);
            }

            switch (e.NewState) {
            case QuizState.Begin:
                playerDisplay.ResetPressHighlight ();
                mainContainer.PackStart (new NameEntryDisplay (), true, true, 4);
                buttonField.CurrentButtons = Buttons.Category;
                break;
            case QuizState.Category:
                playerDisplay.ResetPressHighlight ();
                mainContainer.PackStart (new CategoryDisplay (), true, true, 4);
                buttonField.CurrentButtons = Buttons.Category;
                break;
            case QuizState.Question:
                MessageBus.Instance.BuzzHelper.WaitForPress ();
                mainContainer.PackStart (new QuestionDisplay (), true, true, 4);
                buttonField.CurrentButtons = Buttons.Question;
                break;
            case QuizState.Answer:
                MessageBus.Instance.BuzzHelper.CancelWait ();
                playerDisplay.ResetPressHighlight ();
                mainContainer.PackStart (new QuestionDisplay (), true, true, 4);
                buttonField.CurrentButtons = Buttons.Answer;
                break;
            case QuizState.Finish:
                playerDisplay.ResetPressHighlight ();
                mainContainer.PackStart (new FinishDisplay (), true, true, 4);
                buttonField.CurrentButtons = Buttons.End;
                break;
            default:
                break;
            }

            this.ShowAll ();
        }

        void MainWindowBuild ()
        {
            // Widget MainWindow
            this.Name = ProgrammName;
            this.Title = ProgrammName;
            this.WindowPosition = ((global::Gtk.WindowPosition)(4));
            if ((this.Child != null)) {
                this.Child.ShowAll ();
            }
            this.DefaultWidth = 400;
            this.DefaultHeight = 300;
            this.Show ();
            this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
            this.KeyPressEvent += HandleHandleKeyPressEvent;
        }

        void HandleHandleKeyPressEvent (object o, KeyPressEventArgs args)
        {
            if (args.Event.Key == Gdk.Key.F11) {
                if (IsFullscreen) {
                    this.Unfullscreen ();
                    IsFullscreen = false;
                } else {
                    this.Fullscreen ();
                    IsFullscreen = true;
                }
            }
			
            if (args.Event.Key == Gdk.Key.F6) {
                new QuestionWindow ().Show ();
            }
			
            if (args.Event.Key == Gdk.Key.F5) {
                var dialog = new PreferenceDialog (this, Gtk.DialogFlags.DestroyWithParent);
                dialog.Run ();
                dialog.Destroy ();
            }
        }

        protected void OnDeleteEvent (object sender, DeleteEventArgs a)
        {
            Application.Quit ();
            a.RetVal = true;
        }
	
    }
}

