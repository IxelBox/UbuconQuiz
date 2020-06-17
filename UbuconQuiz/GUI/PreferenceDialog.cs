using System;
using System.Linq;
using Gtk;
using Hyena.Data.Sqlite;
using Hyena.Data;
using Hyena.Data.Gui;

namespace UbuconQuiz
{
	public class PreferenceDialog : Gtk.Dialog
	{
		
		Label dbInfoLabel;
		Label dbLoadLabel;
		FileChooserButton newDbButton;

		public PreferenceDialog (Window w, DialogFlags f) : base("Einstellungen", w, f)
		{
			this.Modal = true;
			build ();
		}

		void build ()
		{
			this.Title = "Einstellungen";
			
			var mainBox = new VBox ();
			
			dbInfoLabel = new Label ();
			
			dbInfoLabel.Text = string.Format ("Exestiert die Datenbank: {0}", System.IO.File.Exists (QuestionProvider.dbPath));
			mainBox.Add (dbInfoLabel);
			
			newDbButton = new FileChooserButton ("Question File Open", FileChooserAction.Open);
			mainBox.Add (newDbButton);
			
			dbLoadLabel = new Label ();
			mainBox.Add (dbLoadLabel);
			
			var createDbButton = new Button (new Label ("Datenbank erstellen"));
			createDbButton.Clicked += HandleCreateDbButtonClicked;
			mainBox.Add (createDbButton);
			
			var removeDbButton = new Button (new Label ("Datenbank löschen"));
			removeDbButton.Clicked += delegate {
				QuestionProvider.RemoveDb ();
				dbInfoLabel.Text = string.Format ("Exestiert die Datenbank: {0}", System.IO.File.Exists (QuestionProvider.dbPath));
			};
			mainBox.Add (removeDbButton);
			
			this.VBox.Add (mainBox);
			
			this.AddButton ("Schließen", ResponseType.Close);
			
			this.ShowAll ();
		}

		void HandleCreateDbButtonClicked (object sender, EventArgs e)
		{
			string file = newDbButton.Filename;
			if (string.IsNullOrEmpty (file)) {
				dbLoadLabel.Text = "richtigen Pfad angeben";
				this.ShowAll ();
				return;
			}
			try {
				QuizParser qp = new QuizParser (file);
				
				using (var connection = new HyenaSqliteConnection (QuestionProvider.dbPath)) {
					var provider = new QuestionProvider (connection);
					var questions = qp.Questions.ToList();
					foreach (var quest in questions) {
						provider.Save (quest);
					}
				}
				dbLoadLabel.Text = "erstellen erfolgreich";
				this.ShowAll ();
			} catch (Exception exc) {
				dbLoadLabel.Text = exc.Message;
				this.ShowAll ();
			}
			dbInfoLabel.Text = string.Format ("Exestiert die Datenbank: {0}", System.IO.File.Exists (QuestionProvider.dbPath));
		}
	}
}

