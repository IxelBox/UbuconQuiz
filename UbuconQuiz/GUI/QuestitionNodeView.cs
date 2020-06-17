using System;
using System.Linq;
using Gtk;
using Mono.Data.Sqlite;
using Hyena.Data.Sqlite;

namespace UbuconQuiz
{
	public class QuestitionNodeView : ScrolledWindow
	{
		public QuestitionNodeView ()
		{
			build ();
			
		}
		
		private void build ()
		{
			this.SetSizeRequest (1600, 400);
			CreateNodeView ();
		}

		/// <summary>
		/// Creates the node view.
		/// </summary>
		void CreateNodeView ()
		{
			QuestionNodeView = new NodeView (Store);
			QuestionNodeView.Reorderable = true;
			this.Add (QuestionNodeView);
			
			QuestionNodeView.AppendColumn ("Frage", new CellRendererTextEditable (Store, Question.Constants.Quest), "text", 0);
			QuestionNodeView.AppendColumn ("Antwort", new CellRendererTextEditable (Store, Question.Constants.Answer), "text", 1);
			QuestionNodeView.AppendColumn ("Category", new CellRendererTextEditable (Store, Question.Constants.Category), "text", 2);
			QuestionNodeView.AppendColumn ("Anzahl", new CellRendererTextEditable (Store, Question.Constants.HowOftenAsk), "text", 3);
			QuestionNodeView.AppendColumn ("Schwierigkeit", new CellRendererTextEditable (Store, Question.Constants.DifficultyLevel), "text", 4);
			
			
			foreach (var col in QuestionNodeView.Columns) {
				//col.SortIndicator = true;
				//col.SortOrder = SortType.Descending;
				col.Clickable = true;
				col.Resizable = true;
				col.Clicked += HandleColumnClicked;
				//col.Reorderable = true;
			}
			
			QuestionNodeView.ShowAll ();
		}

		void HandleColumnClicked (object sender, EventArgs e)
		{
			
			var column = sender as TreeViewColumn;
			if (column != null) {
				int columnNumber = 0;
				SortType columnSortType = SortType.Ascending;
				
				if (SortType.Ascending == column.SortOrder) {
					if (column == QuestionNodeView.Columns [0]) {
						columnNumber = 0;
						comparer = (arg1, arg2) => String.Compare (arg1.Quest, arg2.Quest);
					} else if (column == QuestionNodeView.Columns [1]) {
						columnNumber = 1;
						comparer = (arg1, arg2) => String.Compare (arg1.Answer, arg2.Answer);
					} else if (column == QuestionNodeView.Columns [2]) {
						columnNumber = 2;
						comparer = (arg1, arg2) => String.Compare (arg1.Category, arg2.Category);
					} else if (column == QuestionNodeView.Columns [3]) {
						columnNumber = 3;
						comparer = (arg1, arg2) => String.Compare (arg1.HowOftenAsk.ToString (), arg2.HowOftenAsk.ToString ());
					} else if (column == QuestionNodeView.Columns [4]) {
						columnNumber = 4;
						comparer = (arg1, arg2) => String.Compare (arg1.DifficultyLevel.ToString (), arg2.DifficultyLevel.ToString ());
					}
					columnSortType = SortType.Ascending;
				} else {
					if (column == QuestionNodeView.Columns [0]) {
						columnNumber = 0;
						comparer = (arg1, arg2) => String.Compare (arg2.Quest, arg1.Quest);
					} else if (column == QuestionNodeView.Columns [1]) {
						columnNumber = 1;
						comparer = (arg1, arg2) => String.Compare (arg2.Answer, arg1.Answer);
					} else if (column == QuestionNodeView.Columns [2]) {
						columnNumber = 2;
						comparer = (arg1, arg2) => String.Compare (arg2.Category, arg1.Category);
					} else if (column == QuestionNodeView.Columns [3]) {
						columnNumber = 3;
						comparer = (arg1, arg2) => String.Compare (arg2.HowOftenAsk.ToString (), arg1.HowOftenAsk.ToString ());
					} else if (column == QuestionNodeView.Columns [4]) {
						columnNumber = 4;
						comparer = (arg1, arg2) => String.Compare (arg2.DifficultyLevel.ToString (), arg1.DifficultyLevel.ToString ());
					}
					columnSortType = SortType.Descending;
				}
				
				
				RefreshView ();
				column = QuestionNodeView.Columns [columnNumber];
				
				column.SortIndicator = true;
				column.SortOrder = columnSortType;
				
				if (SortType.Descending == column.SortOrder) {
					column.SortOrder = SortType.Ascending;
				} else if (SortType.Ascending == column.SortOrder) {
					column.SortOrder = SortType.Descending;
				}

				foreach (var col in QuestionNodeView.Columns.Where(c => c != column)) {
					col.SortIndicator = false;
					//column.SortOrder = SortType.Descending;
				}
				
			}
			
		}
		
		public NodeView QuestionNodeView{ get; private set; }
		
		NodeStore m_Store;
		
		NodeStore Store {
			get {
				if (m_Store == null) {
				
					m_Store = new NodeStore (typeof(QuestionNode));
				
					using (var connection = new HyenaSqliteConnection (QuestionProvider.dbPath)) {
						var provider = new QuestionProvider (connection);
						var questions = provider.FetchAll ().ToList ();
						
						//comparer = ((arg1, arg2) => String.Compare (arg1.Category, arg2.Category));
						
						if (comparer != null) {
							questions.Sort ((x, y) => comparer (x, y));
						}
						
					
						foreach (var quest in questions) {
							m_Store.AddNode (new QuestionNode (quest));
						}
					}
				}
				return m_Store;
			}
		}
		
		private Func<Question, Question, int> comparer;
		
		public void RefreshView ()
		{
			m_Store = null;
			this.Remove (QuestionNodeView);
			CreateNodeView ();
		}
		
	}
	
	[TreeNode()]
	public class QuestionNode : TreeNode
	{
		
		Question m_Question;
		
		public QuestionNode (Question question)
		{
			this.m_Question = question;
		}
		
		[TreeNodeValue(Column=0)]
		public string Quest {
			get { return m_Question.Quest;}
		}
		
		[TreeNodeValue(Column=1)]
		public string Answer {
			get { return m_Question.Answer;}
		}
		
		[TreeNodeValue(Column=2)]
		public string Category {
			get { return m_Question.Category;}
		}
		
		[TreeNodeValue(Column=3)]
		public int HowOftenAsk {
			get { return m_Question.HowOftenAsk;}
		}
		
		[TreeNodeValue(Column=4)]
		public int DifficultyLevel {
			get { return m_Question.DifficultyLevel;}
		}
		
		public Question GetQuestion ()
		{
			return m_Question;
		}
	}
	
	
	
}