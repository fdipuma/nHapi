using NHapi.Base.Parser;
using NHapi.Model.V231.Message;
using Xunit;

namespace NHapi.Tests
{

	public class PipeParsingORM
	{
		private const string Message_ORMSample =
			"MSH|^~\\&|HIS|MedCenter|LIS|MedCenter|20060307110114||ORM^O01|MSGID20060307110114|P|2.3.1\r\n" +
			"PID|||12001||Jones^John^^^Mr.||19670824|M|||123 West St.^^Denver^CO^80020^USA|||||||\r\n" +
			"PV1||O|OP^PAREG^||||2342^Jones^Bob|||OP|||||||||2|||||||||||||||||||||||||20060307110111|\r\n" +
			"ORC|NW|20060307110114\r\n" +
			"OBR|1|20060307110114||003038^Urinalysis^L|||20060307110114\r\n";

		[Fact]
		public void TestORMDescriptionExtract()
		{
			var parser = new PipeParser();
			var results = parser.Parse(Message_ORMSample);
			var typed = results as ORM_O01;

			Assert.Equal(@"Date/Time Of Birth", typed.PATIENT.PID.DateTimeOfBirth.Description);
		}
	}
}
