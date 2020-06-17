using System;
using Gtk;
using Hyena.Data.Sqlite;

namespace UbuconQuiz
{
	public class CellRendererTextEditable : CellRendererText
	{
		string m_Column;
		NodeStore m_Store;
		
		public CellRendererTextEditable (NodeStore store, string column)
		{
			m_Column = column;
			m_Store = store;
			
			this.Editable = true;
		}
		
		protected override void OnEdited (string path, string new_text)
		{
			
			using (var connection = new HyenaSqliteConnection (QuestionProvider.dbPath)) {
				var node = (QuestionNode)m_Store.GetNode (new TreePath (path));
				var quest = node.GetQuestion ();
				switch (m_Column) {
				case Question.Constants.Quest:
					quest.Quest = new_text;
					break;
				case Question.Constants.Answer:
					quest.Answer = new_text;
					break;
				case Question.Constants.Category:
					quest.Category = new_text;
					break;
				case Question.Constants.HowOftenAsk:
					quest.HowOftenAsk = Convert.ToInt32 (new_text);
					break;
				case Question.Constants.DifficultyLevel:
					quest.DifficultyLevel = Convert.ToInt32 (new_text);
					break;
				default:
					throw new ApplicationException ("falscher Spaltenname");
				}
				var provider = new QuestionProvider (connection);
				var cmd = new HyenaSqliteCommand ("UPDATE ? SET ? = ? WHERE Id = ?;", provider.TableName, m_Column, new_text, quest.Id);
				connection.Execute (cmd);
			}
			base.OnEdited (path, new_text);
		}
	}
}

