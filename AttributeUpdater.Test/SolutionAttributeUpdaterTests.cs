﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ÀttributeUpdater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebtAnalyzer.ParameterCount;
using TestHelper;

namespace AttributeUpdater.Test
{
	[TestClass]
	public class SolutionAttributeUpdaterTests
	{
		public SolutionAttributeUpdaterTests()
		{
			MethodParameterCountAnalyzer.DefaultMaximumParameterCount = 5;
		}

		[TestMethod]
		public void TestUpdate()
		{
			var project = DiagnosticVerifier.CreateProject(new[] { OutdatedAnnotationProgram, OutdatedAnnotationProgram, ProgramWithUnnecessaryAnnotation });
			var newSolution = SolutionAttributeUpdater.UpdateAttributes(project.Solution).Result;
			var document = newSolution.Projects.SelectMany(newProject => newProject.Documents).First();
			Assert.AreEqual(FixedProgram, CodeFixVerifier.GetStringFromDocument(document));

			var document2 = newSolution.Projects.SelectMany(newProject => newProject.Documents).Skip(1).First();
			Assert.AreEqual(FixedProgram, CodeFixVerifier.GetStringFromDocument(document2));

			var document3 = newSolution.Projects.SelectMany(newProject => newProject.Documents).Skip(2).First();
			Assert.AreEqual(ProgramWithoutAnnotationNeeded, CodeFixVerifier.GetStringFromDocument(document3));
		}

		public static string ProgramWithUnnecessaryAnnotation => @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DebtAnalyzer;

namespace ConsoleApplication1
{
    [DebtType(LineCount = 100, FieldCount = 8)]
    class TypeName
    {
        /// <summary>
        /// 
        /// </summary>
        [DebtMethod(LineCount = 1, ParameterCount = 8)]
        void MyBadMethod2443(int a, int b, int c)
        {

        }
    }
}
";

		public static string ProgramWithoutAnnotationNeeded => @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DebtAnalyzer;

namespace ConsoleApplication1
{
    class TypeName
    {
        /// <summary>
        /// 
        /// </summary>
        void MyBadMethod2443(int a, int b, int c)
        {

        }
    }
}
";

		public static string OutdatedAnnotationProgram => @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DebtAnalyzer;

namespace ConsoleApplication1
{
    class TypeName
    {
        /// <summary>
        /// 
        /// </summary>
        [DebtMethod(LineCount = 1, ParameterCount = 8)]
        void MyBadMethod2443(int a, int b, int c, int d, int e, int g)
        {

        }
    }
}
";

		public static string FixedProgram => @"
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using DebtAnalyzer;

namespace ConsoleApplication1
{
    class TypeName
    {
        /// <summary>
        /// 
        /// </summary>
        [DebtMethod(LineCount = 1, ParameterCount = 6)]
        void MyBadMethod2443(int a, int b, int c, int d, int e, int g)
        {

        }
    }
}
";
	}
}