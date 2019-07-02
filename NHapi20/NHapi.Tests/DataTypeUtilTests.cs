using System;
using NHapi.Base.Model;
using Xunit;

namespace NHapi.Tests
{
	public class DataTypeUtilTests
	{
		[Fact]
		public void GetGMTOffset_PST()
		{
			var offset = DataTypeUtil.GetGMTOffset(TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"),
				new DateTime(2014, 12, 1));
			Assert.Equal(-800, offset);
		}

		[Fact]
		public void GetGMTOffset_PDT()
		{
			var offset = DataTypeUtil.GetGMTOffset(TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"),
				new DateTime(2014, 10, 1));
			Assert.Equal(-700, offset);
		}

		[Fact]
		public void GetGMTOffset_For_TimeZone_With_Non_Zero_Minutes()
		{
			var offset = DataTypeUtil.GetGMTOffset(TimeZoneInfo.FindSystemTimeZoneById("Myanmar Standard Time"),
				new DateTime(2014, 11, 1));
			Assert.Equal(630, offset);
		}

		[Fact]
		public void GetGMTOffset_For_TimeZone_With_Offset_Greater_Than_12()
		{
			var offset = DataTypeUtil.GetGMTOffset(TimeZoneInfo.FindSystemTimeZoneById("Line Islands Standard Time"),
				new DateTime(2014, 11, 1));
			Assert.Equal(1400, offset);
		}
	}
}