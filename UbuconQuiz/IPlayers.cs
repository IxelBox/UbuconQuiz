using System;

namespace UbuconQuiz
{
	public interface IPlayers
	{
		event EventHandler<PlayerChangeEventArgs> ChangePlayers;
		
		IPlayer Player1{ get; }

		IPlayer Player2{ get; }

		void Reset ();
	}
}

