using System;
using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapi.Model.V25.Message;
using Xunit;

namespace NHapi.Tests
{
	/// <summary>
	/// This test case is in response to BUG 1807858 on source forge.  This BUG really isn't a bug, but the expected functionality.
	/// </summary>
	
	public class PipeParsingFixture25
	{
		[Fact]
		public void TestAdtA28MappingFromHl7()
		{
			string hl7Data = @"MSH|^~\&|CohieCentral|COHIE|Clinical Data Provider|TCH|20060228155525||ADT^A28^ADT_A05|1|P|2.5|
EVN|
PID|1|12345
PV1|1";
			PipeParser parser = new PipeParser();
			IMessage msg = parser.Parse(hl7Data);

			Assert.NotNull(msg);
			ADT_A05 a05 = (ADT_A05) msg;

			Assert.Equal("A28", a05.MSH.MessageType.TriggerEvent.Value);
			Assert.Equal("1", a05.PID.SetIDPID.Value);
		}

		[Fact]
		public void TestAdtA28MappingToHl7()
		{
			ADT_A05 a05 = new ADT_A05();

			a05.MSH.MessageType.MessageCode.Value = "ADT";
			a05.MSH.MessageType.TriggerEvent.Value = "A28";
			a05.MSH.MessageType.MessageStructure.Value = "ADT_A05";
			PipeParser parser = new PipeParser();
			string msg = parser.Encode(a05);

			string[] data = msg.Split('|');
			Assert.Equal("ADT^A28^ADT_A05", data[8]);
		}

		[Fact]
		public void TestAdtA04AndA01MessageStructure()
		{
			var result = PipeParser.GetMessageStructureForEvent("ADT_A04", "2.5");
			bool isSame = string.Compare("ADT_A01", result, StringComparison.InvariantCultureIgnoreCase) == 0;
			Assert.True(isSame, "ADT_A04 returns ADT_A01");

			result = PipeParser.GetMessageStructureForEvent("ADT_A13", "2.5");
			isSame = string.Compare("ADT_A01", result, StringComparison.InvariantCultureIgnoreCase) == 0;
			Assert.True(isSame, "ADT_A13 returns ADT_A01");

			result = PipeParser.GetMessageStructureForEvent("ADT_A08", "2.5");
			isSame = string.Compare("ADT_A01", result, StringComparison.InvariantCultureIgnoreCase) == 0;
			Assert.True(isSame, "ADT_A08 returns ADT_A01");

			result = PipeParser.GetMessageStructureForEvent("ADT_A01", "2.5");
			isSame = string.Compare("ADT_A01", result, StringComparison.InvariantCultureIgnoreCase) == 0;
			Assert.True(isSame, "ADT_A01 returns ADT_A01");
		}
	}
}