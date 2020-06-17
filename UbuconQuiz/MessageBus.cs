using System;

namespace UbuconQuiz
{
    public enum QuizState
    {
        Begin,
        Category,
        Question,
        Answer,
        Finish,
        Exit
    }

    public class MessageBus
    {
        private static MessageBus instance;

        public static MessageBus Instance {
            get {
                if (instance == null)
                    instance = new MessageBus ();
                return instance;
            }
        }

        private MessageBus ()
        {
            CurrentState = QuizState.Begin;
            CurrentQuestion = null;
            Players = new ImplPlayers ();
            BuzzHelper = new BuzzBuzzer ();
        }

        QuizState currentState;

        public QuizState CurrentState {
            get {
                return this.currentState;
            }
            set {
#if DEBUG
                System.Console.WriteLine ("old state: {0}\nnew state: {1}", currentState, value);
#endif
                currentState = value;
                if (currentState == QuizState.Category) {
                    CurrentQuestion = null;
                }
                if (ChangeState != null) {
                    ChangeState (this, new QuizStateEventArgs (currentState));
                }
            }
        }

        public event EventHandler<QuizStateEventArgs> ChangeState;

        public Question CurrentQuestion {
            get;
            set;
        }

        public BuzzBuzzer BuzzHelper{ get; }

        public IPlayers Players { get; private set; }

        private class ImplPlayers : IPlayers
        {

            public event EventHandler<PlayerChangeEventArgs> ChangePlayers;

            public ImplPlayers ()
            {
                Reset ();
            }

            public IPlayer Player1 {
                get;
                private set;
            }

            public IPlayer Player2 {
                get;
                private set;
            }

            public void Reset ()
            {
                Player1 = new ImplPlayer ();
                Player1.ChangePlayer += PlayerChangeHandler;
				
                Player2 = new ImplPlayer ();
                Player2.ChangePlayer += PlayerChangeHandler;
            }

            void PlayerChangeHandler (object sender, EventArgs e)
            {
                if (ChangePlayers != null) {
                    ChangePlayers (this, new PlayerChangeEventArgs (sender as IPlayer));
                }
            }
        }

        private class ImplPlayer : IPlayer
        {
			
            public ImplPlayer ()
            {
                Name = "???";
                Score = 0;
                Button = BuzzButton.None;
            }

            #region IPlayer implementation

            public event EventHandler<EventArgs> ChangePlayer;

            string m_Name;
            int m_Score;

            public string Name {
                get {
                    return this.m_Name;
                }
                set {
                    m_Name = value;
                    if (ChangePlayer != null) {
                        ChangePlayer (this, new EventArgs ());
                    }
                }
            }

            public int Score {
                get {
                    return this.m_Score;
                }
                set {
                    m_Score = value;
                    if (ChangePlayer != null) {
                        ChangePlayer (this, new EventArgs ());
                    }
                }
            }


            public BuzzButton Button { get; set; }

            #endregion
			
        }
		
    }

    [Serializable]
    public sealed class QuizStateEventArgs : EventArgs
    {

        public QuizStateEventArgs (QuizState newState)
        {
            this.NewState = newState;
        }

        public QuizState NewState{ get; private set; }
    }

    [Serializable]
    public sealed class PlayerChangeEventArgs : EventArgs
    {
		
        public PlayerChangeEventArgs (IPlayer player)
        {
            this.Player = player;
        }

        public IPlayer Player{ get; private set; }
    }
}