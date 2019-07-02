using NHapi.Base.Parser;
using NHapi.Model.V23.Message;
using Xunit;

namespace NHapi.Tests
{
	/// <summary>
	/// This test case was created from BUG 1812261 on the SourceForge project site
	/// Chad Chenoweth
	/// </summary>
	
	public class TestMSH3
	{
		[Fact]
		public void TestMSH3Set()
		{
			ADT_A01 a01 = new ADT_A01();
			a01.MSH.SendingApplication.UniversalID.Value = "TEST";

			PipeParser parser = new PipeParser();
			string hl7 = parser.Encode(a01);

			string[] data = hl7.Split('|');
			Assert.Equal("ADT^A01", data[8]);
		}
	}
}