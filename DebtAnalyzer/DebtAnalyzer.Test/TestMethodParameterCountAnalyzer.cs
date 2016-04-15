﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using DebtAnalyzer.MethodDebt;
using TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DebtAnalyzer.Test
{
	[TestClass]
    public class TestMethodParameterCountAnalyzer : CodeFixVerifier
	{
		public TestMethodParameterCountAnalyzer()
		{
			MethodParameterCountAnalyzer.DefaultMaximumParameterCount = 5;
		}

		[TestMethod]
		public void TestFix()
		{
			VerifyCSharpFix(TestProgramInput, FixedProgram, allowNewCompilerDiagnostics: true);
		}

		//No diagnostics expected to show up
		[TestMethod]
        public void TestEmptyProgramHasNoDiagnostics()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

		[TestMethod]
		public void TestDiagnostic()
		{
			var test = TestProgramInput;
			var expected = new DiagnosticResult
			{
				Id = "MaxParameterCount",
				Message = String.Format("Method MyBadMethod2443 has 6 parameters while it should not have more than 4."),
				Severity = DiagnosticSeverity.Info,
				Locations =
					new[] {
							new DiagnosticResultLocation("Test0.cs", 17, 14)
						}
			};

			VerifyCSharpDiagnostic(new[] { test, DebtAnalyzerTestUtil.DebtMethodAnnotation, MaxParametersAnnotation }, expected);
		}

		[TestMethod]
        public void TestDiagnosticAsError()
        {
            var test = TestProgramInput;
            var expected = new DiagnosticResult
            {
                Id = "MaxParameterCount",
                Message = "Method MyBadMethod2443 has 6 parameters while it should not have more than 5.",
                Severity = DiagnosticSeverity.Error,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 17, 14)
                        }
            };

            VerifyCSharpDiagnostic(new [] { test, DebtAnalyzerTestUtil.DebtMethodAnnotation, DebtAnalyzerTestUtil.DebtAsError }, expected);
        }

		[TestMethod]
		public void TestDiagnosticAnnotation()
		{
			VerifyCSharpDiagnostic(new[] {DebtAnalyzerTestUtil.DebtMethodAnnotation, FixedProgram });
		}
		
		public static string MaxParametersAnnotation => @"

using System;
using DebtAnalyzer;

[assembly: MaxParameters(4)]
namespace DebtAnalyzer
{
	[AttributeUsage(AttributeTargets.Assembly)]
	class MaxParameters : Attribute
	{
		public MaxParameters(int parameterCount)
		{
			ParameterCount = parameterCount;
		}

		public int ParameterCount { get; }
	}
}";

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

		public static string TestProgramInput => @"
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
        void MyBadMethod2443(int a, int b, int c, int d, int e, int g)
        {

        }
    }
}
";

		[TestMethod]
		public void TestConstructorFix()
		{
			VerifyCSharpFix(WithConstructor, FixedWithConstructor, allowNewCompilerDiagnostics: true);
		}

		public static string FixedWithConstructor => @"
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
        [DebtMethod(LineCount = 1, ParameterCount = 6)]
        TypeName(int a, int b, int c, int d, int e, int g) {
        }
    }
}
";

		public static string WithConstructor => @"
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
        TypeName(int a, int b, int c, int d, int e, int g) {
        }
    }
}
";
		protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new MethodDebtAnnotationProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new MethodDebtAnalyzer();
        }
    }
}