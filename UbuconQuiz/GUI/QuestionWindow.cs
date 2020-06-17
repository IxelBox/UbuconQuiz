	using System;
using Gtk;
using Hyena.Data.Sqlite;

namespace UbuconQuiz
{
	public class QuestionWindow : Window
	{
		QuestitionNodeView qnv;
		
		public QuestionWindow () : base(Gtk.WindowType.Toplevel)
		{
			build ();
			this.ShowAll ();
		}
		
		private void build ()
		{
			var mainBox = new VBox ();
			qnv = new QuestitionNodeView ();
			
			mainBox.PackStart (qnv, true, true, 2);
			
			var buttonBox = new HButtonBox {Layout = ButtonBoxStyle.End, Spacing = 6};
			Button buttonAdd = new Button (Stock.Add);
			buttonAdd.Clicked += ButtonAddClicked;
			buttonBox.Add (buttonAdd);
			
			Button buttonRemove = new Button (Stock.Remove);
			buttonRemove.Clicked += ButtonRemoveClicked;
			buttonBox.Add (buttonRemove);
			
			Button buttonRefresh = new Button (Stock.Refresh);
			buttonRefresh.Clicked += ButtonRefreshClicked;
			buttonBox.Add (buttonRefresh);
			
			Button buttonClose = new Button (Stock.Close);
			buttonClose.Clicked += ButtonCloseClicked;
			buttonBox.Add (buttonClose);
			
			mainBox.PackEnd (buttonBox, false, false, 2);
			
			
			
			this.Add (mainBox);
			
		}
		
		void ButtonRefreshClicked (object sender, EventArgs e)
		{
			qnv.RefreshView ();
			
		}

		void ButtonRemoveClicked (object sender, EventArgs e)
		{
			if (qnv.QuestionNodeView.NodeSelection.SelectedNode == null)
				return;
			var questionNode = (QuestionNode)qnv.QuestionNodeView.NodeSelection.SelectedNode;
			var question = questionNode.GetQuestion ();
			qnv.QuestionNodeView.NodeStore.RemoveNode (questionNode);
			using (var connection = new HyenaSqliteConnection (QuestionProvider.dbPath)) {
				var provider = new QuestionProvider (connection);
				provider.Delete (question);
			}
			
		}

		void ButtonAddClicked (object sender, EventArgs e)
		{
			using (var connection = new HyenaSqliteConnection (QuestionProvider.dbPath)) {
				var provider = new QuestionProvider (connection);
				Question question = new Question ("Frage", "Antwort", "Nothing", 0, 0);
				provider.Save (question);
				qnv.QuestionNodeView.NodeStore.AddNode (new QuestionNode (question));
			}
		}

		void ButtonCloseClicked (object sender, EventArgs e)
		{
			this.HideAll ();
		}
	}
}

