using System;
using System.Linq;
using System.Collections.Generic;
using Gtk;
using Hyena.Data.Sqlite;

namespace UbuconQuiz
{
	public class DifficultLevelButtons : VBox
	{
		
		string m_Category;
		
		public DifficultLevelButtons (string category) : base(false, 0)
		{
			m_Category = category;
			build ();
			this.ShowAll ();
		}

		bool generateButton (IEnumerable<Question> question)
		{
			if (question.Count () != 0) {
				var rand = new Random ();
				var index = rand.Next (question.Count ());
				var quest = (question.ToList ()) [index];
				var button = new CategoryButton (new Label{Markup ="<big>"+quest.DifficultyLevel+"</big>", UseMarkup = true}){Quest = quest};
				this.PackStart (button, true, true, 0);
				button.Clicked += HandleBClicked;
				button.Relief = ReliefStyle.None;
				return true;
			}
			return false;
		}
		
		private void build ()
		{
			this.PackStart (new Label{Markup= "<b><big>"+m_Category+"</big></b>", UseMarkup = true}, false, false, 35);
			
			int buttonCount = 0;
			
			
			using (var connection = new HyenaSqliteConnection (QuestionProvider.dbPath)) {
				var provider = new QuestionProvider (connection);
				var levels = provider.FetchAll ().Where (q => q.HowOftenAsk == 0 && q.Category == m_Category).Select (q => q.DifficultyLevel).Distinct ().ToList ();
				levels.Sort ();
				
				foreach (var level in levels) {
					var questions = provider.FetchAll ().Where (q => q.HowOftenAsk == 0 && q.DifficultyLevel == level && q.Category == m_Category);
						
					if (generateButton (questions))
						buttonCount++;
				}
			}
		}
		
		void HandleBClicked (object sender, EventArgs e)
		{
			CategoryButton b = sender as CategoryButton;
			
			if (b != null) {
				var bus = MessageBus.Instance;
				bus.CurrentQuestion = b.Quest;
				bus.CurrentState = QuizState.Question;
			}
		}
	}
}

