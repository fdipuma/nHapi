using System;
using NHapi.Base.Model;
using NHapi.Base.Util;
using NHapi.Model.V231.Datatype;
using NHapi.Model.V231.Message;
using NHapi.Base.Parser;
using Xunit;
using Xunit.Abstractions;

namespace NHapi.Tests
{
	public class ParserTest
	{
		private readonly ITestOutputHelper _output;

		public ParserTest(ITestOutputHelper output)
		{
			_output = output;
		}

		public string GetMessage()
		{
			return "MSH|^~\\&|XPress Arrival||||200610120839||ORU^R01|EBzH1711114101206|P|2.3.1|||AL|||ASCII\r\n" +
			       "PID|1||1711114||Appt^Test||19720501||||||||||||001020006\r\n" +
			       "ORC|||||F\r\n" +
			       "OBR|1|||ehipack^eHippa Acknowlegment|||200610120839|||||||||00002^eProvider^Electronic|||||||||F\r\n" +
			       "OBX|1|FT|||This\\.br\\is\\.br\\A Test~MoreText~SomeMoreText||||||F\r\n";
		}

		[Fact]
		public void TestOBR5RepeatingValuesMessage()
		{
			var parser = new PipeParser();
			var oru = new ORU_R01();
			oru = (ORU_R01) parser.Parse(GetMessage());

			foreach (var obs in oru.GetPATIENT_RESULT(0).GetORDER_OBSERVATION(0).GetOBSERVATION().OBX.GetObservationValue())
			{
				Assert.IsType<FT>(obs.Data);
			}
		}

		[Fact]
		public void TestSpecialCharacterEncoding()
		{
			PipeParser parser = new PipeParser();
			ORU_R01 oru = new ORU_R01();
			oru = (ORU_R01) parser.Parse(GetMessage());

			FT data = (FT) oru.GetPATIENT_RESULT(0).GetORDER_OBSERVATION(0).GetOBSERVATION(0).OBX.GetObservationValue(0).Data;
			Assert.Equal(@"This\.br\is\.br\A Test", data.Value);
		}

		[Fact]
		public void TestSpecialCharacterEntry()
		{
			PipeParser parser = new PipeParser();
			ORU_R01 oru = new ORU_R01();
			oru.MSH.MessageType.MessageType.Value = "ORU";
			oru.MSH.MessageType.TriggerEvent.Value = "R01";
			oru.MSH.EncodingCharacters.Value = @"^~\&";
			oru.MSH.VersionID.VersionID.Value = "2.3.1";
			oru.GetPATIENT_RESULT(0).GetORDER_OBSERVATION(0).GetOBSERVATION(0).OBX.ValueType.Value = "FT";
			oru.GetPATIENT_RESULT(0).GetORDER_OBSERVATION(0).OBR.SetIDOBR.Value = "1";
			Varies v =
				oru.GetPATIENT_RESULT(0).GetORDER_OBSERVATION(0).GetOBSERVATION(0).OBX.GetObservationValue(0);
			ST text = new ST(oru);
			text.Value = @"This\.br\is\.br\A Test";
			v.Data = text;


			string encodedData = parser.Encode(oru);
			_output.WriteLine(encodedData);
			IMessage msg = parser.Parse(encodedData);
			_output.WriteLine(msg.GetStructureName());
			oru = (ORU_R01) msg;
			FT data = (FT) oru.GetPATIENT_RESULT(0).GetORDER_OBSERVATION(0).GetOBSERVATION(0).OBX.GetObservationValue(0).Data;
			Assert.Equal(@"This\.br\is\.br\A Test", data.Value);
		}

		[Fact]
		public void TestSpecialCharacterEntryEndingSlash()
		{
			PipeParser parser = new PipeParser();
			ORU_R01 oru = new ORU_R01();
			oru.MSH.MessageType.MessageType.Value = "ORU";
			oru.MSH.MessageType.TriggerEvent.Value = "R01";
			oru.MSH.EncodingCharacters.Value = @"^~\&";
			oru.MSH.VersionID.VersionID.Value = "2.3.1";
			oru.GetPATIENT_RESULT(0).GetORDER_OBSERVATION(0).GetOBSERVATION(0).OBX.ValueType.Value = "FT";
			oru.GetPATIENT_RESULT(0).GetORDER_OBSERVATION(0).OBR.SetIDOBR.Value = "1";
			Varies v =
				oru.GetPATIENT_RESULT(0).GetORDER_OBSERVATION(0).GetOBSERVATION(0).OBX.GetObservationValue(0);
			ST text = new ST(oru);
			text.Value = @"This\.br\is\.br\A Test~";
			v.Data = text;


			string encodedData = parser.Encode(oru);
			IMessage msg = parser.Parse(encodedData);
			oru = (ORU_R01) msg;
			FT data = (FT) oru.GetPATIENT_RESULT(0).GetORDER_OBSERVATION(0).GetOBSERVATION(0).OBX.GetObservationValue(0).Data;
			Assert.Equal(@"This\.br\is\.br\A Test~", data.Value);
		}

		[Fact]
		public void TestSpecialCharacterEntryWithAllSpecialCharacters()
		{
			PipeParser parser = new PipeParser();
			ORU_R01 oru = new ORU_R01();
			oru.MSH.MessageType.MessageType.Value = "ORU";
			oru.MSH.MessageType.TriggerEvent.Value = "R01";
			oru.MSH.EncodingCharacters.Value = @"^~\&";
			oru.MSH.VersionID.VersionID.Value = "2.3.1";
			oru.GetPATIENT_RESULT(0).GetORDER_OBSERVATION(0).GetOBSERVATION(0).OBX.ValueType.Value = "FT";
			oru.GetPATIENT_RESULT(0).GetORDER_OBSERVATION(0).OBR.SetIDOBR.Value = "1";
			Varies v =
				oru.GetPATIENT_RESULT(0).GetORDER_OBSERVATION(0).GetOBSERVATION(0).OBX.GetObservationValue(0);
			ST text = new ST(oru);
			text.Value = @"Th&is\.br\is\.br\A T|e\H\st\";
			v.Data = text;


			string encodedData = parser.Encode(oru);
			_output.WriteLine(encodedData);
			IMessage msg = parser.Parse(encodedData);
			oru = (ORU_R01) msg;
			FT data = (FT) oru.GetPATIENT_RESULT(0).GetORDER_OBSERVATION(0).GetOBSERVATION(0).OBX.GetObservationValue(0).Data;
			Assert.Equal(@"Th&is\.br\is\.br\A T|e\H\st\", data.Value);
		}

		[Fact]
		public void TestValidHl7Data()
		{
			PipeParser parser = new PipeParser();
			ORU_R01 oru = new ORU_R01();
			oru.MSH.MessageType.MessageType.Value = "ORU";
			oru.MSH.MessageType.TriggerEvent.Value = "R01";
			oru.MSH.EncodingCharacters.Value = @"^~\&";
			oru.MSH.VersionID.VersionID.Value = "2.3.1";
			oru.GetPATIENT_RESULT(0).GetORDER_OBSERVATION(0).GetOBSERVATION(0).OBX.ValueType.Value = "FT";
			oru.GetPATIENT_RESULT(0).GetORDER_OBSERVATION(0).OBR.SetIDOBR.Value = "1";
			Varies v =
				oru.GetPATIENT_RESULT(0).GetORDER_OBSERVATION(0).GetOBSERVATION(0).OBX.GetObservationValue(0);
			ST text = new ST(oru);
			text.Value = @"Th&is\.br\is\.br\A T|est\";
			v.Data = text;


			string encodedData = parser.Encode(oru);

			//Console.WriteLine(encodedData);
			string[] segs = encodedData.Split('\r');
			string[] fields = segs[2].Split('|');
			string data = fields[5];

			Assert.Equal(@"Th\T\is\.br\is\.br\A T\F\est\E\", data);
		}

		[Fact]
		public void UnEscapesData()
		{
			// Arrange
			const string content =
				"MSH|^~\\&|TestSys|432^testsys practice|TEST||201402171537||MDM^T02|121906|P|2.3.1||||||||\r\n" +
				@"OBX|1|TX|PROBLEM FOCUSED^PROBLEM FOCUSED^test|1|\T\#39;Thirty days have September,\X000d\April\X0A\June,\X0A\and November.\X0A\When short February is done,\E\X0A\E\all the rest have\T\nbsp;31.\T\#39" +
				"\r\n";

			var parser = new PipeParser();
			var msg = parser.Parse(content);


			// Act
			var segment = msg.GetStructure("OBX") as ISegment;
			var idx = Terser.getIndices("OBX-5");
			var segmentData = Terser.Get(segment, idx[0], idx[1], idx[2], idx[3]);

			// Assert

			// verify that data was properly unescaped by NHapi	
			// \E\X0A\E\ should be escaped to \X0A\
			// \X0A\ should be unescaped to \n
			// \X000d\ should be unescaped to \r
			// \t\ should be unescaped to &


			const string expectedResult =
				"&#39;Thirty days have September,\rApril\nJune,\nand November.\nWhen short February is done,\\X0A\\all the rest have&nbsp;31.&#39";
			Assert.Equal(expectedResult, segmentData);
		}
	}
}