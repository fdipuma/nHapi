using System;
using System.Collections.Generic;
using System.Text;
using NHapi.Base.Parser;
using Xunit;

namespace NHapi.Tests
{
	public class EscapeTest
	{
		[Theory]
		[MemberData(nameof(testData))]
		public void TestEscapes(string input, string expected)
		{
			var encodingChars = new EncodingCharacters('|', null);
			var actual = Escape.escape(input, encodingChars);

			Assert.Equal(expected, actual);
		}

		public static TheoryData<string, string> testData = new TheoryData<string, string>
		{ 
			// control characters should be escaped
			{@"\", @"\E\"}, 
			{@"|", @"\F\"}, 
			{@"^", @"\S\"},
			{@"&", @"\T\"},
			{@"~", @"\R\"}, 

			// command sequences should not be escaped
			{@"\H\", @"\H\"},
			{@"\N\", @"\N\"},
			{@"\XAB\", @"\XAB\"},
			{@"\CAB\", @"\CAB\"},
			{@"\MAB\", @"\MAB\"},
			{@"\ZAB\", @"\E\ZAB\E\"}, // .. but Z sequences are not supported

			// FT commands should not be escaped
			{@"\.sp\", @"\.sp\"},
			{@"\.sp4\", @"\.sp4\"},
			{@"\.sp+4\", @"\.sp+4\"},
			{@"\.br\", @"\.br\"},
			{@"\.fi\", @"\.fi\"},
			{@"\.nf\", @"\.nf\"},
			{@"\.in4\", @"\.in4\"},
			{@"\.in+4\", @"\.in+4\"},
			{@"\.in-4\", @"\.in-4\"},
			{@"\.ti4\", @"\.ti4\"},
			{@"\.ti+4\", @"\.ti+4\"},
			{@"\.ti-4\", @"\.ti-4\"},
			{@"\.sk8\", @"\.sk8\"},
			{@"\.sk+8\", @"\.sk+8\"},
			{@"\.ce\", @"\.ce\"},

			// invalid FT commands should be escaped
			{@"\.sp-4\", @"\E\.sp-4\E\"},
			{@"\.spA\", @"\E\.spA\E\"},
			{@"\.in+d\", @"\E\.in+d\E\"},
			{@"\.ti-d\", @"\E\.ti-d\E\"},
			{@"\.sk\", @"\E\.sk\E\"},

			// unclosed escapes should be escaped
			{@".br\", @".br\E\"},
			{@".sp\", @".sp\E\"},
			{@".sp+4\", @".sp+4\E\"},
			{@".fi\", @".fi\E\"},
			{@".nf\", @".nf\E\"},
			{@".in+4\", @".in+4\E\"},
			{@".ti-4\", @".ti-4\E\"},
			{@".ce\", @".ce\E\"},
			{@"\.br", @"\E\.br"},
			{@"\.sp", @"\E\.sp"},
			{@"\.sp+4", @"\E\.sp+4"},
			{@"\.fi", @"\E\.fi"},
			{@"\.nf", @"\E\.nf"},
			{@"\.in+4", @"\E\.in+4"},
			{@"\.ti-4", @"\E\.ti-4"},
			{@"\.ce", @"\E\.ce"},
		};
	}
}
