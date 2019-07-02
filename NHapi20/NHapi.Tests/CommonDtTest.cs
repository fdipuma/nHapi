using NHapi.Base;
using NHapi.Base.Model.Primitive;
using Xunit;

namespace NHapi.Tests
{
	public class CommonDtTest
	{
		[Fact]
		public void Constructor_Sets_Value()
		{
			var commonDt = new CommonDT("20010203");
			Assert.Equal("20010203", commonDt.Value);
		}

		[Fact]
		public void Value__Set_to_Null()
		{
			var commonDt = new CommonDT();
			commonDt.Value = null;
			Assert.Null(commonDt.Value);
		}

		[Fact]
		public void Value__Set_to_Empty_String()
		{
			var commonDt = new CommonDT();
			commonDt.Value = "";
			Assert.Equal("", commonDt.Value);
		}

		[Fact]
		public void Value__Set_to_Empty_Quoted_String()
		{
			var commonDt = new CommonDT();
			commonDt.Value = "\"\"";
			Assert.Equal("\"\"", commonDt.Value);
		}

		[Fact]
		public void Value__Set_to_Invalid_Length()
		{
			var commonDt = new CommonDT();
			Assert.Throws<DataTypeException>(
				() => commonDt.Value = "20010");
		}

		[Fact]
		public void Value__Set_to_Valid_Year()
		{
			var commonDt = new CommonDT();
			commonDt.Value = "2001";
			Assert.Equal("2001", commonDt.Value);
			Assert.Equal(2001, commonDt.Year);
		}

		[Fact]
		public void Value__Set_to_Valid_Year_and_Month()
		{
			var commonDt = new CommonDT();
			commonDt.Value = "200102";
			Assert.Equal("200102", commonDt.Value);
			Assert.Equal(2001, commonDt.Year);
			Assert.Equal(2, commonDt.Month);
		}

		[Fact]
		public void Value__Set_to_Valid_Year_and_Month_and_Day()
		{
			var commonDt = new CommonDT();
			commonDt.Value = "20010203";
			Assert.Equal("20010203", commonDt.Value);
			Assert.Equal(2001, commonDt.Year);
			Assert.Equal(2, commonDt.Month);
			Assert.Equal(3, commonDt.Day);
		}

		[Fact]
		public void Value__Set_to_Invalid_Year()
		{
			var commonDt = new CommonDT();

			Assert.Throws<DataTypeException>(
				() => commonDt.Value = "200a");
        }

		[Fact]
		public void Value__Set_to_Invalid_Month()
		{
			var commonDt = new CommonDT();

			Assert.Throws<DataTypeException>(
				() => commonDt.Value = "20010a");
        }

		[Fact]
		public void Value__Set_to_Invalid_Day()
		{
			var commonDt = new CommonDT();

			Assert.Throws<DataTypeException>(
				() => commonDt.Value = "2001020a");
        }


		[Fact]
		public void YearPrecision__Set_to_Valid_Year()
		{
			var commonDt = new CommonDT();
			commonDt.YearPrecision = 2001;
			Assert.Equal("2001", commonDt.Value);
			Assert.Equal(2001, commonDt.Year);
			Assert.Equal(0, commonDt.Month);
			Assert.Equal(0, commonDt.Day);
		}

		[Fact]
		public void YearPrecision__Set_to_Invalid_Year()
		{
			var commonDt = new CommonDT();

			Assert.Throws<DataTypeException>(
				() => commonDt.YearPrecision = 20010);
        }


		[Fact]
		public void setYearMonthPrecision_With_Valid_Year_and_Month()
		{
			var commonDt = new CommonDT();
			commonDt.setYearMonthPrecision(2001, 02);
			Assert.Equal("200102", commonDt.Value);
			Assert.Equal(2001, commonDt.Year);
			Assert.Equal(2, commonDt.Month);
		}

		[Fact]
		public void setYearMonthPrecision_With_Invalid_Year()
		{
			var commonDt = new CommonDT();

			Assert.Throws<DataTypeException>(
				() => commonDt.setYearMonthPrecision(20010, 02));
        }

		[Fact]
		public void setYearMonthPrecision_With_Invalid_Month()
		{
			var commonDt = new CommonDT();

			Assert.Throws<DataTypeException>(
				() => commonDt.setYearMonthPrecision(2001, 13));
        }


		[Fact]
		public void setYearMonthDayPrecision_With_Valid_Year_and_Month_and_Day()
		{
			var commonDt = new CommonDT();
			commonDt.setYearMonthDayPrecision(2001, 2, 3);
			Assert.Equal("20010203", commonDt.Value);
			Assert.Equal(2001, commonDt.Year);
			Assert.Equal(2, commonDt.Month);
			Assert.Equal(3, commonDt.Day);
		}

		[Fact]
		public void setYearMonthDayPrecision_With_Invalid_Year()
		{
			var commonDt = new CommonDT();

			Assert.Throws<DataTypeException>(
				() => commonDt.setYearMonthDayPrecision(20010, 2, 3));
        }

		[Fact]
		public void setYearMonthDayPrecision_With_Invalid_Month()
		{
			var commonDt = new CommonDT();

			Assert.Throws<DataTypeException>(
				() => commonDt.setYearMonthDayPrecision(2001, 13, 3));
        }

		[Fact]
		public void setYearMonthDayPrecision_With_Invalid_Day()
		{
			var commonDt = new CommonDT();

			Assert.Throws<DataTypeException>(
				() => commonDt.setYearMonthDayPrecision(2001, 2, 29));
        }
	}
}