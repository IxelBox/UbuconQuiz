using System;

namespace UbuconQuiz
{
    public interface IPlayer
    {
		
        string Name{ get; set; }

        int Score{ get; set; }

        BuzzButton Button { get; set; }

        event EventHandler<EventArgs> ChangePlayer;
    }
}

