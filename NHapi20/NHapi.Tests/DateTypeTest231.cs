using System;
using NHapi.Model.V231.Message;
using NHapi.Base.Parser;
using Xunit;

namespace NHapi.Tests
{
	/// <summary>
	/// Summary description for DateTypeTest.
	/// </summary>
	public class DateTypeTest231
	{
		[Fact]
		public void ConvertToDate()
		{
			DateTime checkDate = DateTime.Now;
			PipeParser parser = new PipeParser();
			ADT_A01 a01 = new ADT_A01();
			a01.PV1.AdmitDateTime.TimeOfAnEvent.Set(checkDate, "yyyyMMdd");
			Assert.Equal(a01.PV1.AdmitDateTime.TimeOfAnEvent.Value, checkDate.ToString("yyyyMMdd"));
		}

		[Fact]
		public void ConvertToLongDate()
		{
			DateTime checkDate = DateTime.Now;
			ACK ack = new ACK();
			ack.MSH.DateTimeOfMessage.TimeOfAnEvent.SetLongDate(checkDate);
			Assert.Equal(ack.MSH.DateTimeOfMessage.TimeOfAnEvent.Value, checkDate.ToString("yyyyMMddHHmm"));
		}

		[Fact]
		public void ConvertToLongDateWithSecond()
		{
			DateTime checkDate = DateTime.Now;
			ACK ack = new ACK();
			ack.MSH.DateTimeOfMessage.TimeOfAnEvent.SetLongDateWithSecond(checkDate);
			Assert.Equal(ack.MSH.DateTimeOfMessage.TimeOfAnEvent.Value, checkDate.ToString("yyyyMMddHHmmss"));
		}

		[Fact]
		public void ConvertToLongDateWithFractionOfSecond()
		{
			DateTime checkDate = DateTime.Now;
			ACK ack = new ACK();
			ack.MSH.DateTimeOfMessage.TimeOfAnEvent.SetLongDateWithFractionOfSecond(checkDate);
			Assert.Equal(ack.MSH.DateTimeOfMessage.TimeOfAnEvent.Value, checkDate.ToString("yyyyMMddHHmmss.FFFF"));
		}

		[Fact]
		public void ConvertToShortDate()
		{
			DateTime checkDate = DateTime.Now;
			ACK ack = new ACK();
			ack.MSH.DateTimeOfMessage.TimeOfAnEvent.SetShortDate(checkDate);
			Assert.Equal(ack.MSH.DateTimeOfMessage.TimeOfAnEvent.Value, checkDate.ToString("yyyyMMdd"));
		}

		[Fact]
		public void ConvertBackToShortDate()
		{
			DateTime checkDate = DateTime.Now;
			ACK ack = new ACK();
			ack.MSH.DateTimeOfMessage.TimeOfAnEvent.SetShortDate(checkDate);

			DateTime checkDate2 = ack.MSH.DateTimeOfMessage.TimeOfAnEvent.GetAsDate();

			Assert.Equal(checkDate.ToShortDateString(), checkDate2.ToShortDateString());
		}

		[Fact]
		public void ConvertBackToLongDate()
		{
			DateTime checkDate = DateTime.Now;
			ACK ack = new ACK();
			ack.MSH.DateTimeOfMessage.TimeOfAnEvent.SetLongDate(checkDate);

			DateTime checkDate2 = ack.MSH.DateTimeOfMessage.TimeOfAnEvent.GetAsDate();

			Assert.Equal(checkDate.ToLongDateString(), checkDate2.ToLongDateString());
		}
	}
}