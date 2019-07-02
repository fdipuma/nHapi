using NHapi.Model.V23.Datatype;
using NHapi.Model.V23.Group;
using NHapi.Model.V23.Message;
using Xunit;

namespace NHapi.Tests
{
	public class Test23Orc
	{
		[Fact]
		public void Test()
		{
			ORM_O01 msg = new ORM_O01();
			ORM_O01_ORDER order = msg.GetORDER(0);
			EI placerNumber = order.ORC.GetPlacerOrderNumber(0);
			placerNumber.EntityIdentifier.Value = "123";
		}
	}
}