using System;
using System.Data;
using System.Linq;
using Mono.Data.Sqlite;
using Hyena.Data.Sqlite;

namespace UbuconQuiz
{
	public class DatabaseManager
	{
		string path;

		public DatabaseManager (string path)
		{
			this.path = path;
		}

		/* many thinks not implement
		public void CreateDB ()
		{
			var db = new QuestionTable (connectionString);
			if (db.DatabaseExists ()) {
				//Console.WriteLine ("Deleting old database...");
				db.DeleteDatabase ();
			}
			db.CreateDatabase ();
		}
*/
		public void CreateProvider ()
		{

			using (var conn = new HyenaSqliteConnection (path)) {
				var qp = new QuestionProvider (conn);
				
				/*
				dbConn.Open ();
			
				using (var dbCommand = dbConn.CreateCommand ()) {
					dbCommand.CommandText = @"CREATE TABLE question (id INTEGER PRIMARY KEY, question TEXT, answer TEXT, category TEXT, howOftenAsk INTEGER);";
					dbCommand.ExecuteNonQuery ();
				}
				*/
			}
		}

		public bool CheckForDB ()
		{
			string check = "Select * from question;";
			bool existDB = false;
			/*
			using (SqliteConnection dbConn = new SqliteConnection (connectionString)) {
				dbConn.Open ();
			
				using (var dbCommand = dbConn.CreateCommand ()) {
					try {
						dbCommand.CommandText = check;
						dbCommand.ExecuteReader ();
						existDB = true;
					} catch (SqliteException) {
						existDB = false;
					}
				}
			}
			*/
			return existDB;
		}

		public void RemoveDB ()
		{
			System.IO.File.Delete (path);
		}

		public void ResetDB ()
		{
			/*
			using (SqliteConnection dbConn = new SqliteConnection (connectionString)) {
				dbConn.Open ();
			
				using (var dbCommand = dbConn.CreateCommand ()) {
					dbCommand.CommandText = @"DROP TABLE question;";
					dbCommand.ExecuteNonQuery ();
				}
			}
			

			CreateDB ();
			*/
		}

		public void AddQuestion (Question question)
		{

			//string insertCommand = "insert into question (question,answer,category,howOftenAsk) values (" + question.Quest + "," + question.Answer + "," + question.Category + ",0);";

		}

		public void AddOneHowAsk (Question question)
		{
		
		}
	}
}

