using System;
using System.Collections.Generic;
using System.Linq;
using Hyena.Data.Sqlite;
using Hyena.Widgets;

using Gtk;

namespace UbuconQuiz
{
    public class QuestionDisplay : Alignment, IMainContent
    {
        MessageBus bus;
        WrapLabel label;

        public QuestionDisplay () : base (0.5f, 0.5f, 1, 1)
        {
            bus = MessageBus.Instance;
            label = new WrapLabel ();
            this.SetPadding (10, 10, 50, 50);
            this.Add (label);
            if (bus.CurrentState == QuizState.Question) {
                ShowQuestion ();
                HowOftenAskFill ();
            } else if (bus.CurrentState == QuizState.Answer) {
                ShowQuestion ();
                ShowAnswer ();
            }
			
        }

        void HowOftenAskFill ()
        {
            using (var connection = new HyenaSqliteConnection (QuestionProvider.dbPath)) {
                var provider = new QuestionProvider (connection);
                var cmd = new HyenaSqliteCommand ("UPDATE ? SET HowOftenAsk = 1 WHERE Id = ?;", provider.TableName, bus.CurrentQuestion.Id);
                connection.Execute (cmd);
            }
        }

        void ShowQuestion ()
        {

            if (bus.CurrentState == QuizState.Question) {
                label.Markup = "<span font=\"40\">" + bus.CurrentQuestion.Quest.Trim () + "</span>";
            } else {
                label.Markup = "<span font=\"40\" fgcolor=\"grey\">" + bus.CurrentQuestion.Quest.Trim () + "</span>";
            }
            this.ShowAll ();
        }

        void ShowAnswer ()
        {
            string answer = bus.CurrentQuestion.Answer;
            if (string.IsNullOrEmpty (bus.CurrentQuestion.Answer)) {
                answer = "Noch keine Antwort vorhanden";
            }
            label.Markup += "<b><span font=\"30\">\n\n" + answer + "</span></b>";
            this.ShowAll ();
        }
		
    }
}

