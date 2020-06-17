using System;
using System.IO;
using System.Linq;
using UbuconQuiz;
/*
using NUnit.Framework;

namespace UbuconQuizTests
{
	[TestFixture()]
	public class ParserFixture
	{
		
		string path;
		QuizParser parser;

		[TestFixtureSetUp]
		public void InitTestFixture ()
		{
			path = Path.Combine (Environment.CurrentDirectory,"Questions.txt");
		}

		[SetUp]
		public void InitTest ()
		{
			parser = new QuizParser (path);
		}

		[TearDown]
		public void StopTest ()
		{
			parser.Questions.GetEnumerator ().Dispose ();
		}

		[Test]
		public void GiveMoreThenOneElement ()
		{
			Assert.IsTrue (parser.Questions.Any ());
		}

		[Test]
		public void CheckCategoryCount ()
		{
			Assert.AreEqual (2,parser.Questions.GroupBy (q => q.Category).Count ());
		}

		[Test]
		public void CheckQuestionCount ()
		{
			Assert.AreEqual (6,parser.Questions.Count ());
		}

		[Test, ExpectedException(typeof(FileNotFoundException))]
		public void ExpectFileNotFoundExceptionWithInvalidPath ()
		{
			var pars2 = new QuizParser ("foobar.txt");
			pars2.Questions.Any ();
		}
	}
}
 */
