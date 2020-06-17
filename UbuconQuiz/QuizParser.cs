using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UbuconQuiz
{
	public class QuizParser
	{
		string _path;
		public QuizParser (string path)
		{
			_path = path;
		}

		public IEnumerable<Question> Questions {
			get {
				var questionStream = new StreamReader (_path);
				
				string cat = "";
				
				while (!questionStream.EndOfStream) {
					var line = questionStream.ReadLine ();
					if (!line.StartsWith (" ")) {
						cat = line;
						continue;
					}
					
					var questionAnswer = line.Split ('?');
					string answerLevel = String.Empty;
					string answer = String.Empty;
					string question = String.Empty;
					int level = 0;
					
					for (int i = 0; i < questionAnswer.Length; i++) {
						if (i == questionAnswer.Length - 1 && i != 0) {
							answerLevel = questionAnswer [i];
							
							answer = answerLevel.Split ('#') [0];
							
							string match = Regex.Match (answerLevel, @"##\d+\W*$").Value;
							if (match.Length != 0) {
								level = int.Parse (match.TrimStart ('#').Trim());
							}
						} else {
							question += questionAnswer [i];
							question += "?";
						}
					}
					yield return new Question (question.Trim(), answer.Trim(), cat.Trim(), level, 0);
				}
				questionStream.Close ();
			}
		}
		
		
	}
}

