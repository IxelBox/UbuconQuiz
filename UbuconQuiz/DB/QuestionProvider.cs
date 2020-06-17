using System;
using Hyena.Data.Sqlite;

namespace UbuconQuiz
{
	public class QuestionProvider : SqliteModelProvider<Question>
	{
		
		public static string dbPath = System.IO.Path.Combine (Environment.CurrentDirectory, "quest.db");

		public QuestionProvider (HyenaSqliteConnection connection) : base (connection)
		{
			Init ();
		}

		public override string TableName {
			get { return "question"; }
		}

		protected override int ModelVersion {
			get { return 1; }
		}

		protected override int DatabaseVersion {
			get { return 1; }
		}

		protected override void MigrateTable (int old_version)
		{
		}

		protected override void MigrateDatabase (int old_version)
		{
		}

		protected override Question MakeNewObject ()
		{
			return new Question ();
		}

		public static void RemoveDb ()
		{
			System.IO.File.Delete (dbPath);
		}
	}
}

