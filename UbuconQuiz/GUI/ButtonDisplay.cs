using System;
using System.Collections.Generic;
using Gtk;
using Hyena.Gui;
using Hyena.Data.Sqlite;

namespace UbuconQuiz
{
    public enum Buttons
    {
        Nothing,
        Question,
        Answer,
        Category,
        End
    }

    public class ButtonDisplay : Gtk.HBox
    {
        HButtonBox buttonBox;
        Buttons currentButtons;
        IPlayers players;
        MessageBus bus;

        public ButtonDisplay (Buttons buttons)
        {
            bus = MessageBus.Instance;
            players = bus.Players;
            CurrentButtons = buttons;
            this.Show ();
        }

        private void FillButtonBoxes ()
        {
            buttonBox = new HButtonBox ();
            //buttonBox.Layout = ButtonBoxStyle.Spread;
            //buttonBox.LayoutStyle = ButtonBoxStyle.Spread;
            //buttonBox.Spacing = 6;
            switch (CurrentButtons) {
            case Buttons.Answer:
                CreateAnswerButtons ();
                break;
            case Buttons.Category:
                CreateCategoryButtons ();
                break;
            case Buttons.Question:
                CreateQuestionButtons ();
                break;
            case Buttons.End:
                CreateEndButtons ();
                break;
            default:
                break;
            }
        }

        void CreateEndButtons ()
        {
            var b = new Button ();
            b.Label = "Neues Spiel";
            b.Clicked += HandleNewGameClicked;
            buttonBox.Add (b);
        }

        void HandleNewGameClicked (object sender, EventArgs e)
        {
            bus.CurrentState = QuizState.Begin;
        }

        void CreateQuestionButtons ()
        {

            var quest = MessageBus.Instance.CurrentQuestion;
			
            var b = new Button ();
            b.Label = players.Player1.Name + " +" + quest.DifficultyLevel;
            ;
            b.Clicked += HandlePlayer1AddPointClicked;
            buttonBox.Add (b);
			
            b = new Button ();
            b.Label = "Keine Antwort";
            b.Clicked += NoAnswerClicked;
            buttonBox.Add (b);
			
            b = new Button ();
            b.Label = players.Player2.Name + " +" + quest.DifficultyLevel;
            b.Clicked += HandlePlayer2AddPointClicked;
            buttonBox.Add (b);
        }

        void NoAnswerClicked (object sender, EventArgs e)
        {
            players.Player1.Score = players.Player1.Score;
            bus.CurrentState = QuizState.Answer;
        }

        #region Create CategoryButtons

        void CreateCategoryButtons ()
        {
            var b = new Button ();
            b.Label = "Ende";
            b.Clicked += HandleEndClicked;
            buttonBox.Add (b);
        }

        #endregion

        #region Create Answer Button

        void CreateAnswerButtons ()
        {
            var b = new Button ();
            b.Label = "Nächste Frage";
            b.Clicked += HandleNextClicked;
            buttonBox.Add (b);
			
            b = new Button ();
            b.Label = "Ende";
            b.Clicked += HandleEndClicked;
            buttonBox.Add (b);
			
            var quest = MessageBus.Instance.CurrentQuestion;
            var level = quest.DifficultyLevel;
				
            b = new Button ();
            b.Label = players.Player1.Name + " +" + level;
            b.Clicked += HandlePlayer1AddPointClicked;
            buttonBox.Add (b);
			
            b = new Button ();
            b.Label = players.Player1.Name + " -" + level;
            b.Clicked += HandlePlayer1RemovePointClicked;
            buttonBox.Add (b);
			
            b = new Button ();
            b.Label = players.Player2.Name + " +" + level;
            b.Clicked += HandlePlayer2AddPointClicked;
            buttonBox.Add (b);
			
            b = new Button ();
            b.Label = players.Player2.Name + " -" + level;
            b.Clicked += HandlePlayer2RemovePointClicked;
            buttonBox.Add (b);
        }

        void HandlePlayer2RemovePointClicked (object sender, EventArgs e)
        {
            players.Player2.Score -= 
			bus.CurrentQuestion.DifficultyLevel;
        }

        void HandlePlayer2AddPointClicked (object sender, EventArgs e)
        {
            players.Player2.Score += 
			bus.CurrentQuestion.DifficultyLevel;
            if (bus.CurrentState == QuizState.Question) {
                bus.CurrentState = QuizState.Answer;
            }
        }

        void HandlePlayer1RemovePointClicked (object sender, EventArgs e)
        {
            players.Player1.Score -= 
			bus.CurrentQuestion.DifficultyLevel;
        }

        void HandlePlayer1AddPointClicked (object sender, EventArgs e)
        {
            players.Player1.Score += 
			bus.CurrentQuestion.DifficultyLevel;
            if (bus.CurrentState == QuizState.Question) {
                bus.CurrentState = QuizState.Answer;
            }
        }

        public event EventHandler<EventArgs> EndClicked;

        void HandleEndClicked (object sender, EventArgs e)
        {
            bus.CurrentState = QuizState.Finish;
			
            if (EndClicked != null) {
                EndClicked (this, e);
            }
        }

        public event EventHandler<EventArgs> NextClicked;

        void HandleNextClicked (object sender, EventArgs e)
        {
            bus.CurrentState = QuizState.Category;
            if (NextClicked != null) {
                NextClicked (this, e);
            }
        }

        #endregion

        public Buttons CurrentButtons {
            get {
                return currentButtons;
            }
            set {
                currentButtons = value;
                if (buttonBox != null)
                    this.Remove (buttonBox);
                FillButtonBoxes ();
                this.Add (buttonBox);
                this.ShowAll ();
            }
        }
    }
}

