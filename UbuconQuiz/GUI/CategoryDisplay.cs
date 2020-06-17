using System;
using System.Collections.Generic;
using System.Linq;
using Gtk;
using Hyena.Data.Sqlite;

namespace UbuconQuiz
{
	public class CategoryDisplay : Gtk.HBox, IMainContent
	{
		public CategoryDisplay () : base(true, 12)
		{
			build ();
			
		}

		void build ()
		{
			using (var connection = new HyenaSqliteConnection (QuestionProvider.dbPath)) {
				var provider = new QuestionProvider (connection);
				
				var questions = provider.FetchAll ();
				var questCategories = questions.Where (q => q.HowOftenAsk == 0).Select (q => q.Category).Distinct ().ToList ();
				int questCategoriesCount = questCategories.Count ();
				var categories = new List<string> ();
				
				if (questCategoriesCount > 3) {
					do {
						var rand = new Random (DateTime.Now.Millisecond);
						int r = 0;
						
						if (questCategoriesCount > 1) {
							r = rand.Next (0, questCategoriesCount);
						}
						categories.Add (questCategories [r]);
						categories = categories.Distinct ().ToList ();
					} while (categories.Count < 3);
				} else {
					categories = questCategories.ToList ();
				}
				
				foreach (var cat in categories) {
					this.Add (new DifficultLevelButtons (cat));
				}
				
				if (questCategoriesCount == 0) {
					this.Add (new Label ("Keine Fragen mehr vorhanden"));
				}
			}
		}
	}
}

