using System;
using Mono.Data.Sqlite;
using Hyena.Data.Sqlite;

namespace UbuconQuiz
{
	public class Question
	{
		public static class Constants
		{
			public const string Id = "Id";
			public const string Quest = "Quest";
			public const string Answer = "Answer";
			public const string Category = "Category";
			public const string HowOftenAsk = "HowOftenAsk";
			public const string DifficultyLevel = "DifficultyLevel";
			
			
		}
		public Question ()
		{

		}

		public Question (string question, string answer, string category, int level, int howOftenAsk)
		{
			this.Quest = question;
			this.Answer = answer;
			this.Category = category;
			this.HowOftenAsk = howOftenAsk;
			this.DifficultyLevel = level;
		}

		[DatabaseColumn(Constants.Id, Constraints = DatabaseColumnConstraints.PrimaryKey)]
		public int Id;
		[DatabaseColumn(Constants.Quest)]
		public string Quest;
		[DatabaseColumn(Constants.Answer)]
		public string Answer;
		[DatabaseColumn(Constants.Category)]
		public string Category;
		[DatabaseColumn(Constants.HowOftenAsk, DefaultValue="0")]
		public int HowOftenAsk;
		[DatabaseColumn(Constants.DifficultyLevel, DefaultValue="0")]
		public int DifficultyLevel;
		
	}
}

