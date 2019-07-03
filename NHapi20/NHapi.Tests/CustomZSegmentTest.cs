using NHapi.Base;
using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapi.Model.V22_ZSegments;
using NHapi.Model.V22_ZSegments.Message;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace NHapi.Tests
{
	public class CustomZSegmentTest
	{
		private readonly ITestOutputHelper _output;

		public CustomZSegmentTest(ITestOutputHelper output)
		{
			_output = output;
		}

		[Fact]
		public void ParseADT_A08()
		{
			PackageManager.Instance.AddCustomVersion(typeof(NHapi.Model.V22_ZSegments.Constants).Namespace, "2.2.CustomZ");

			//this is some fictive data
			string message = "MSH|^~\\&|SUNS1|OVI02|AZIS|CMD|200606221348||ADT^A08|1049691900|P|2.2\r\n" +
			                 "EVN|A08|200601060800\r\n" +
			                 "PID||8912716038^^^51276|0216128^^^51276||BARDOUN^LEA SACHA||19981201|F|||AVENUE FRANC GOLD 8^^LUXEMBOURGH^^6780^150||053/12456789||N|S|||99120162652||^^^|||||B\r\n" +
			                 "PV1||O|^^|U|||07632^MORTELO^POL^^^DR.|^^^^^|||||N||||||0200001198\r\n" +
			                 "PV2|||^^AZIS||N|||200601060800\r\n" +
			                 "IN1|0001|2|314000|||||||||19800101|||1|BARDOUN^LEA SACHA|1|19981201|AVENUE FRANC GOLD 8^^LUXEMBOURGH^^6780^150|||||||||||||||||\r\n" +
			                 "ZIN|0164652011399|0164652011399|101|101|45789^Broken bone\r\n";

			var parser = new PipeParser();

			IMessage m = parser.Parse(message, Constants.VERSION);

			Assert.NotNull(m);

			_output.WriteLine("Type: " + m.GetType());

			var adtA08 = m as ADT_A08;
			//verify some Z segment data
			Assert.Equal("45789", adtA08.ZIN.AccidentData.Id.Value);
			Assert.Equal("Broken bone", adtA08.ZIN.AccidentData.Text.Value);
		}
	}
}