using NHapi.Base.Model;
using NHapi.Base.Parser;
using NHapi.Model.V21.Message;
using Xunit;

namespace NHapi.Tests
{
	public class PipeParsingFixture21
	{
		[Fact]
		public void ParseADTA01()
		{
			string message = "MSH|^~\\&|ADT|Admitting|RADIO|ARTEFACT|200710061035||ADT^A01|00000040|P|2.1\r\n" +
			                 "EVN|A01|200710061035\r\n" +
			                 "PID||1144270^4^M10|0699999^2^M10||XXXXXX|XXXCXCXX|20071006|F|||10 THE ADDRESS||(450)999-9999|||S||||||||||||||N\r\n" +
			                 "NK1|1\r\n" +
			                 "PV1||I|19^D415^01P|05|07008496||180658^DOCTOR NAME|||81|||||||180658^DOCTOR NAME|NN||01||||||||||||||||||||||||200710061018\r\n" +
			                 "DG1|1|I9|412|NAISSANCE||01\r\n" +
			                 "Z01|1||S|NOUVEAU-NE||FATHER NAME^D|||||0||||A||||||N|||1|GFATHER NAME|G-PERE||(450)432-9999|21||S||20071006101800||N||0||||0000000000||||||||00000000000000|00000000||||||||||01|00000000|00000000000000|05|00|75017|00|00|||||||||||||||||||000000000|000000000|||00000000000000|||01|0\r\n";

			PipeParser parser = new PipeParser();

			IMessage m = parser.Parse(message);

			ADT_A01 parsedMessage = m as ADT_A01;

			Assert.NotNull(parsedMessage);
			Assert.Equal("1144270", parsedMessage.PID.PATIENTIDEXTERNALEXTERNALID.IDNumber.Value);
		}
	}
}