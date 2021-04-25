using System;
using System.Globalization;

namespace CalendarLib
{
	/// <summary>
	/// Ethiopian Calendar which is similar to the Julian calendar
	/// It has 13 months where 12 of the have 30 days and a 13th
	/// month which has 5 days on non-leap years and 6 on leap years.
	/// Leyu Sisay @ 2005
	/// </summary>
	public sealed class EthiopianCalendar : Calendar
	{
		#region Member Vars.

		private int					[]eras = new int[]{0};
		private int					twoDigiYearMax=2069;
		
		#endregion Member Vars.

		#region Constructor

		/// <summary>
		/// Constructs the Ethiopian Calendar
		/// </summary>
		public EthiopianCalendar()
		{
			
		}

		
		#endregion Constructor

		#region Properties
		
		/// <summary>
		/// gets An array of integers that represents the eras in the current calendar
		/// </summary>
		public override int[] Eras 
		{
			get
			{
				return eras;
			}
		
		}

		
		/// <summary>
		/// gets/sets The last year of a 100-year range that can be represented by a 2-digit year.
		/// default is 2069 that is 1970-2069 69 will be interprated as 2069 and 73 will be 
		/// interpreted as 1973.
		/// </summary>
		public override int TwoDigitYearMax
		{
			get
			{
				return twoDigiYearMax;
			}
			
			set
			{
				twoDigiYearMax=value;
			}
		}
		
		
		#endregion Properties

		#region Methods
	
		/// <summary>
		/// Returns a DateTime that is the specified number of days away from the specified DateTime
		/// </summary>
		/// <param name="time">The DateTime to which to add days</param>
		/// <param name="days">The number of days to add</param>
		/// <returns>The DateTime that results from adding the specified number of days to the specified DateTime</returns>
		public	override DateTime AddDays(DateTime time,int days)
		{
			return time.AddDays(days);

			#region Unnecessary (Commented) 
			//			//No days to add just exit
			//			if (days == 0)
			//				return time;
			//
			//			//Get the year month & day values form datetime
			//			int year	=time.Year;
			//			int month	=time.Month; 
			//			int day		=time.Day;
			//			int hour	=time.Hour;
			// 
			//			//Convert the gregorian date to ethiopian
			//			ConversionHelper.GregorianToEthiopian(ref year,ref month,ref day);
			//			
			//			//Change the hour for consistency we just convert it back
			//			hour=ConversionHelper.GregorianToEthiopian(hour);
			//
			//			if( (day + days) > 0)
			//			{
			//				while( day + days > GetDaysInMonth(year,month))
			//				{
			//					days-=GetDaysInMonth(year,month)-day; 
			//
			//					time=AddMonths(new DateTime(year,month,1,hour,time.Minute,time.Second,this),1);
			//
			//					year	=time.Year;
			//					month	=time.Month; 
			//					day		=time.Day;
			//
			//					//Convert the gregorian date to ethiopian
			//					ConversionHelper.GregorianToEthiopian(ref year,ref month,ref day);
			//				}
			//
			//				day+=days;
			//			}
			//			else
			//			{
			//				days+=day;
			//
			//				do
			//				{
			//					time=AddMonths(new DateTime(year,month,1,hour,time.Minute,time.Second,this),-1);
			//
			//					year	=time.Year;
			//					month	=time.Month; 
			//					day		=time.Day;
			//
			//					//Convert the gregorian date to ethiopian
			//					ConversionHelper.GregorianToEthiopian(ref year,ref month,ref day);
			//					
			//					days+=GetDaysInMonth(year,month);   
			//
			//				}while(days < -GetDaysInMonth(year,month));
			//				
			//				day+=days;
			//			}
			//
			//			return new DateTime(year,month,day,hour,time.Minute,time.Second,this);

			#endregion Unnecessary (Commented) 
		}

		
		/// <summary>
		/// Returns a DateTime that is the specified number of hours away from the specified DateTime
		/// </summary>
		/// <param name="time">The DateTime to which to add hours</param>
		/// <param name="hours">The number of hours to add.</param>
		/// <returns>The DateTime that results from adding the specified number of hours to the specified DateTime</returns>
		public	override DateTime AddHours(DateTime time,int hours)
		{            
			return time.AddHours(hours);	//Now just add the remaining hours
		}

			
		/// <summary>
		/// Returns a DateTime that is the specified number of milliseconds away from the specified DateTime
		/// </summary>
		/// <param name="time">The DateTime to which to add milliseconds</param>
		/// <param name="milliseconds">The number of milliseconds to add</param>
		/// <returns>The DateTime that results from adding the specified number of milliseconds to the specified DateTime</returns>
		public	override DateTime AddMilliseconds(DateTime time,double milliseconds)
		{
			return time.AddMilliseconds(milliseconds); 
		}
		
	
		/// <summary>
		/// Returns a DateTime that is the specified number of minutes away from the specified DateTime
		/// </summary>
		/// <param name="time">The DateTime to which to add month</param>
		/// <param name="months">The number of months to add</param>
		/// <returns>The DateTime that results from adding the specified number of months to the specified DateTime</returns>
		public	override DateTime AddMonths(DateTime time,int months)
		{
			//Months to be added is zero just return
			if(months==0)
				return time;

			//Months to be added not zero then convert given date to ethiopian
			int year	=time.Year;
			int month	=time.Month;
			int day		=time.Day;
			int hour	=time.Hour;
 
			//Convert the gregorian date to ethiopian
			ConversionHelper.GregorianToEthiopian(ref year,ref month,ref day);
			
			//Change the hour just convert it back
			hour=ConversionHelper.GregorianToEthiopian(hour);

			//Are there any number of years to be added or subtracted
			int years=0;
			//What is the new month to be set
			int newMonth=0;
			
			//If we have -ve months
			if((month+months) <= 0)		//Note that GetMonthsInYear always returns 13 months
			{
				years		= -1 + (month+months)	/ GetMonthsInYear(year);
				newMonth	= GetMonthsInYear(year) + ((month+months) % GetMonthsInYear(year));
			}
			else
			{				
				years	+= (((float) month+ (float) months)/ (float) GetMonthsInYear(year)) != (int)((month+months)/GetMonthsInYear(year)) ? ((month+months)/GetMonthsInYear(year)) : ((month+months)/GetMonthsInYear(year))-1;
				newMonth	=(month+months)%GetMonthsInYear(year)== 0 ? 13:(month+months)%GetMonthsInYear(year) ;
			}

			//Make sure that years to be added are greater than or less than 0
			if (years!=0)
			{
				//Add the years to the datetime object
				time=AddYears(time,years);
				
				year			=time.Year;
				month			=time.Month; 
				int dummyDay	=time.Day;

				//Convert the updated datetime object
				ConversionHelper.GregorianToEthiopian(ref year,ref month,ref dummyDay);
			}
			
			month=newMonth;			
			
			//In order to prevent the case where the month is pagume and the day is greater than 5 || 6
			//When the year is added the date won't be valid !
			if(day > GetDaysInMonth(year,month))
			{
				day=GetDaysInMonth(year,month);
			}

			return new DateTime(year,month,day,hour,time.Minute,time.Second,this); 
		}

		
		/// <summary>
		/// returns a DateTime that is the specified number of years away from the specified DateTime.
		/// </summary>
		/// <param name="time">The DateTime to which to add years</param>
		/// <param name="years">The number of years to add</param>
		/// <returns>The DateTime that results from adding the specified number of years to the specified DateTime</returns>
		public	override DateTime AddYears(DateTime time,int years)
		{
			//No years to add then exit
			if(years==0)
				return time;

			int year	=time.Year;
			int month	=time.Month; 
			int day		=time.Day;
			int hour	=time.Hour;
 
			//Convert the gregorian datetime to ethiopian date
			ConversionHelper.GregorianToEthiopian(ref year,ref month,ref day);
			hour=ConversionHelper.GregorianToEthiopian(hour);
			
			//Add or subtract the years
			year+=years;

			//In order to prevent the case where the month is pagume and the day is 6
			//When the year is added the date won't be valid !
			if(day > GetDaysInMonth(year,month))
			{
				day=GetDaysInMonth(year,month);
			}

			return new DateTime(year,month,day,hour,time.Minute,time.Second,this); 
		}
				
		
		/// <summary>
		/// returns the day of the month in the specified DateTime.
		/// </summary>
		/// <param name="time">the DateTime to read</param>
		/// <returns>A 1-based integer that represents the day of the month in time</returns>
		public	override int GetDayOfMonth(DateTime time)
		{
			int year	=time.Year;
			int month	=time.Month;
			int day		=time.Day;

			ConversionHelper.GregorianToEthiopian(ref year,ref month,ref day);
			return day;
		}

	
		/// <summary>
		///  returns the day of the week in the specified DateTime.
		/// </summary>
		/// <param name="time">The DateTime to read.</param>
		/// <returns>A DayOfWeek value that represents the day of the week in time</returns>
		public	override DayOfWeek GetDayOfWeek(DateTime time)
		{
			//Get the yer, month and day of the given DateTime in Gregorian
			
			int year	=time.Year;
			int month	=time.Month;
			int day		=time.Day;

			//Change the given gregorian date to ethiopian date
			ConversionHelper.GregorianToEthiopian(ref year,ref month,ref day);
			return ConversionHelper.EthiopianDayOfWeek(year,month,day);
		}

		
		/// <summary>
		/// returns the day of the year in the specified DateTime.
		/// </summary>
		/// <param name="time">The DateTime to read.</param>
		/// <returns>A 1-based integer that represents the day of the year in time</returns>
		public	override int GetDayOfYear(DateTime time)
		{
			//Get the gregorian dates
			int year	=time.Year;
			int month	=time.Month;
			int day		=time.Day;

			//Change the given gregorian date to ethiopian date
			ConversionHelper.GregorianToEthiopian(ref year,ref month,ref day);
			
			return (month-1)*30 + day;	//30 is always true from Meskerem-Nehase
		}


		/// <summary>
		/// returns the number of days in the specified month in the specified year in the specified era.
		/// </summary>
		/// <param name="year">An integer that represents the year</param>
		/// <param name="month">A 1-based integer that represents the month</param>
		/// <param name="era">An integer that represents the era</param>
		/// <returns>The number of days in the specified month in the specified year in the specified era</returns>
		public	override int GetDaysInMonth(int year,int month,int era)
		{
			try
			{
				if(year <1)
				{
					throw new ArgumentOutOfRangeException("year",year,"Year is outside bounds");
				}
				if(month <1 || month > GetMonthsInYear(year,era))
				{
					throw new ArgumentOutOfRangeException("month",month,"Month is outside bounds");
				}
			}
			catch (ArgumentOutOfRangeException ex)
			{
				throw ex;
			}

			
			//If the month is not Pagume
			if(month!=13)
			{
				return 30;	//Always 30 days Meskerem->Nehase
			}
			else			//The Month is Pagume
			{
				if(IsLeapYear(year,era))//If its a leap year
				{
					return 6;			//We have 6 days
				}
				else
				{
					return 5;			//Else on regular bases return 5;
				}
			}		 
		}


		/// <summary>
		/// returns the number of days in the specified year in the specified era
		/// </summary>
		/// <param name="year">An integer that represents the year</param>
		/// <param name="era">An integer that represents the era</param>
		/// <returns>The number of days in the specified year in the specified era</returns>
		public	override int GetDaysInYear(int year,int era)
		{
			//Check date is valid
			try
			{
				if(year <1)
				{
					throw new ArgumentOutOfRangeException("year",year,"Year is outside bounds");
				}
			}
			catch(ArgumentOutOfRangeException ex)
			{
				throw ex;			
			}
			
			//If it's a leap year then the total number of days is 366
			if(IsLeapYear(year,era))
			{
				return 366;
			}
			else	//Else it's 365
			{
				return 365;		
			}
		}

		
		/// <summary>
		/// returns the era in the specified DateTime.
		/// </summary>
		/// <param name="time">The DateTime to read.</param>
		/// <returns>An integer that represents the era in time</returns>
		public	override int GetEra(DateTime time)
		{
			return CurrentEra;		//Default era
		}

			
		/// <summary>
		/// Returns the hours value in the specified DateTime
		/// </summary>
		/// <param name="time">The DateTime to read</param>
		/// <returns>An integer from 0 to 23 that represents the hour in time</returns>
		public	override int GetHour(DateTime time)
		{
			return ConversionHelper.GregorianToEthiopian(time.Hour);
		}


		/// <summary>
		/// returns the month in the specified DateTime.
		/// </summary>
		/// <param name="time">The DateTime to read.</param>
		/// <returns>A 1-based integer that represents the month in time</returns>
		public	override int GetMonth(DateTime time)
		{
			int year	=time.Year;
			int month	=time.Month;
			int day		=time.Day;

			ConversionHelper.GregorianToEthiopian(ref year,ref month,ref day);
			return month;
		}

		
		/// <summary>
		/// returns the number of months in the specified year in the specified era
		/// </summary>
		/// <param name="year">An integer that represents the year</param>
		/// <param name="era">An integer that represents the era</param>
		/// <returns>The number of months in the specified year in the specified era</returns>
		public	override int GetMonthsInYear(int year,int era)
		{
			//Check for the validity of date
			try
			{
				if(year <1)
				{
					throw new ArgumentOutOfRangeException("year",year,"Year is outside bounds");
				}					
			}
			catch(ArgumentOutOfRangeException ex)
			{
				throw ex;
			}
						
			return 13;		//No matter the case we always have 13 months
		}
		

		/// <summary>
		/// returns the year in the specified DateTime.
		/// </summary>
		/// <param name="time">The DateTime to read.</param>
		/// <returns>An integer that represents the year in time</returns>
		public	override int GetYear(DateTime time)
		{
			int year	=time.Year;
			int month	=time.Month;
			int day		=time.Day;

			ConversionHelper.GregorianToEthiopian(ref year,ref month,ref day);
			return year;
		}

		
		/// <summary>
		/// determines whether the specified date in the specified era is a leap day
		/// </summary>
		/// <param name="year">An integer that represents the year</param>
		/// <param name="month">A 1-based integer that represents the month</param>
		/// <param name="day">A 1-based integer that represents the day</param>
		/// <param name="era">An integer that represents the era</param>
		/// <returns>true if the specified day is a leap day; otherwise, false</returns>
		public	override bool IsLeapDay(int year,int month,int day,int era)
		{
			try
			{
				if(year <1)
				{
					throw new ArgumentOutOfRangeException("year",year,"Year is outside bounds");
				}
				if(month <1 || month > GetMonthsInYear(year,era))
				{
					throw new ArgumentOutOfRangeException("month",month,"Month is outside bounds");
				}
				if(day <1 || day > GetDaysInMonth(year,month,era))
				{
					throw new ArgumentOutOfRangeException("day",day,"Day is outside bounds");
				}			
			
			}
			catch(ArgumentOutOfRangeException ex)
			{
				throw ex;
			}

			//Only if the month given is leap and the day is 6
			if(IsLeapMonth(year,month,era)&& day==6)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		
		/// <summary>
		/// whether the specified month in the specified year in the specified era is a leap month
		/// </summary>
		/// <param name="year">An integer that represents the year</param>
		/// <param name="month">A 1-based integer that represents the month</param>
		/// <param name="era">An integer that represents the era</param>
		/// <returns>true if the specified month is a leap month; otherwise, false</returns>
		public	override bool IsLeapMonth(int year,int month,int era)
		{
			try
			{
				if(year <1)
				{
					throw new ArgumentOutOfRangeException("year",year,"Year is outside bounds");
				}
				if(month <1 || month > GetMonthsInYear(year,era))
				{
					throw new ArgumentOutOfRangeException("month",month,"Month is outside bounds");
				}
			}
			catch(ArgumentOutOfRangeException ex)
			{
				throw ex;
			}

			//Only if the year is a leap year and the month is Pagume
			if(IsLeapYear(year,era) && month==13)
			{
				return true;
			}
			else
			{
				return false;
			}			
		}

		
		/// <summary>
		/// determines whether the specified year in the specified era is a leap year
		/// </summary>
		/// <param name="year">An integer that represents the year</param>
		/// <param name="era">An integer that represents the era</param>
		/// <returns>true if the specified year is a leap year; otherwise, false</returns>
		public	override bool IsLeapYear(int year,int era)
		{
			//Check wheter year is valid
			try
			{
				if(year <1)
				{
					throw new ArgumentOutOfRangeException("year",year,"Year is outside bounds");
				}
			}
			catch(ArgumentOutOfRangeException ex)
			{
				throw ex;
			}
						
			//Leap year ( is it (year%4)==3 || (year+5500)%4==3 ) ????
			return (year %4) == 3 ;
		}

	
		/// <summary>
		/// returns a DateTime that is set to the specified date and time in the specified era
		/// </summary>
		/// <param name="year">An integer that represents the year</param>
		/// <param name="month">A 1-based integer that represents the month</param>
		/// <param name="day">A 1-based integer that represents the day</param>
		/// <param name="hour">An integer from 0 to 23 that represents the hour</param>
		/// <param name="minute">An integer from 0 to 59 that represents the minute</param>
		/// <param name="second">An integer from 0 to 59 that represents the second</param>
		/// <param name="millisecond">An integer from 0 to 999 that represents the millisecond</param>
		/// <param name="era">An integer that represents the era</param>
		/// <returns>The DateTime that is set to the specified date and time in the current era</returns>
		public	override DateTime ToDateTime(int year,int month,int day,int hour,int minute,int second,int millisecond,int era)
		{
			try
			{
				if(year <1)
				{
					throw new ArgumentOutOfRangeException("year",year,"Year is outside bounds");
				}
				if(month <1 || month > GetMonthsInYear(year,era))
				{
					throw new ArgumentOutOfRangeException("month",month,"Month is outside bounds");
				}
				if(day <1 || day > GetDaysInMonth(year,month,era))
				{
					throw new ArgumentOutOfRangeException("day",day,"Day is outside bounds");
				}				
			}
			catch(ArgumentOutOfRangeException ex)
			{
				throw ex;
			}
			
			//Converts the Ethiopian Date to Gregorian Date
			ConversionHelper.EthiopianToGregorian(ref year,ref month,ref day);
			
			//Change hour representation of ethiopian to gregorian hour
			hour=ConversionHelper.EthiopianToGregorian(hour);

			return new DateTime(year,month,day,hour,minute,second,millisecond);
		}

		
		#endregion Methods		

		#region Static Methods
		
		/// <summary>
		/// returns the string representation of DayOfWeek value
		/// </summary>
		/// <param name="dayOfWeek">The DayOfWeek value to read</param>
		/// <returns>returns the string representation of DayOfWeek value</returns>
		public static string ToString(DayOfWeek dayOfWeek)
		{		
			return DateTimeFormatEtInfo.GetDayName(dayOfWeek);				
		}

		
		/// <summary>
		/// returns the string representation of the specified DateTime in Ethiopian Calendar
		/// </summary>
		/// <param name="time">DateTime to read</param>
		/// <returns>string representation of sepcified DateTime in the general format</returns>
		public static string ToString(DateTime time)
		{
			return ToString(time,"g");
		}		
		
	
		/// <summary>
		/// returns the string representation of the specified DateTime in Ethiopian Calendar
		/// </summary>
		/// <param name="time">DateTime to read</param>
		/// <param name="format">A format string (no support for format characters "r","R","s","u","U" and format patterns "f","ff","fff","ffff","fffff","ffffff","fffffff","z","zz","zzz","\c") the other formats are as described in the System.Globalization.DateTimeFormatInfo documentation in msdn.(Added format pattern "ttt" more descriptive than the AM/PM paradigm)</param>
		/// <returns>string representation of sepcified DateTime in the given format</returns>
		public static string ToString(DateTime time, string format)
		{
			
			int year	=time.Year;
			int month	=time.Month;
			int day		=time.Day;
			int hour	=time.Hour;
			int minute	=time.Minute;
			int second	=time.Second;

			ConversionHelper.GregorianToEthiopian( ref year, ref month,ref day);
			
			DayOfWeek dayOfWeek	=ConversionHelper.EthiopianDayOfWeek(year,month,day); 
			hour				=ConversionHelper.GregorianToEthiopian(hour);

			return DateTimeFormatEtInfo.ToString(year,month,day,dayOfWeek,hour,minute,second,format);   
		}


		/// <summary>
		/// Returns the Months Names arry of strings
		/// </summary>
		public static string[] MonthNames
		{
			get
			{
				return DateTimeFormatEtInfo.GetMonthNames;  
			}	
		}
				
		
		#endregion Static Methods

		#region private class ConversionHelper
		
		private class ConversionHelper
		{			
			#region Constants

			private const int E_EPOCH = 2796;		//Ethiopian Epoch

			#endregion Constants

			#region Member Vars

			private static GregorianCalendar	gregorianCalendar=new GregorianCalendar();  

			#endregion Member Vars

			#region Constructor

			/// <summary>
			/// Forbid the instance of Conversion Helper
			/// </summary>
			private ConversionHelper()
			{
			
			}
			
			#endregion Constructor

			#region Functions

			/// <summary>
			/// returns the converetd year, month and day values
			/// from Gregorian to Ethiopian
			/// </summary>
			/// <param name="year">An integer that represents the year</param>
			/// <param name="month">A 1-based integer that represents the month</param>
			/// <param name="day">A 1-based integer that represents the day</param>
			public static void GregorianToEthiopian(ref int year,ref int month,ref int day)
			{
				int fixedDate=GregorianToFixed(year,month,day);				//Change the Gregorian Calendar to the fxed date
				FixedToEthiopian(fixedDate,ref year,ref month,ref day);		
			}


			/// <summary>
			/// returns the DayOfWeek of the given year,month,day
			/// </summary>
			/// <param name="year">An integer that represents the year</param>
			/// <param name="month">A 1-based integer that represents the month</param>
			/// <param name="day">A 1-based integer that represents the day</param>
			/// <returns>A DayOfWeek value that represents the day of the week in time</returns>
			public static DayOfWeek EthiopianDayOfWeek(int year,int month,int day)
			{
				//The integer representation of the day Of week
				int dayOfWeek = Math.Abs(ConversionHelper.EthiopianToFixed(year,month,day)) % 7;
			
				//Now Change the integer representation in DayOfWeek enum values
				switch(dayOfWeek)
				{
					case 0:
						return DayOfWeek. Sunday;
					case 1:
						return DayOfWeek. Monday;
					case 2:
						return DayOfWeek. Tuesday;
					case 3:
						return DayOfWeek. Wednesday;  
					case 4:
						return DayOfWeek. Thursday;  
					case 5:
						return DayOfWeek. Friday ;  
					case 6:
						return DayOfWeek. Saturday ;
					default:
						return DayOfWeek. Sunday; 
				}
			}
		
		
			/// <summary>
			/// returns the converetd hour
			/// from Gregorian to Ethiopian
			/// </summary>
			/// <param name="hour">An integer from 0 to 23 that represents the hour</param>
			/// <returns>An integer from 0 to 23 that represents the hour.</returns>
			public static int  GregorianToEthiopian(int hour)
			{
				return (( 24+ hour - 6) % 24 );			
			}

		
			/// <summary>
			/// returns the fixed date out of the Gregorian
			/// year, month and day values
			/// </summary>
			/// <param name="year">An integer that represents the year</param>
			/// <param name="month">A 1-based integer that represents the month</param>
			/// <param name="day">A 1-based integer that represents the day</param>
			/// <returns>A fixed date representation</returns>
			public static  int GregorianToFixed( int year, int month, int day)
			{			
				return ( 365 * (year - 1)
					+ (int)Math.Floor((double)(year - 1)	/	(double)4) 
					- (int)Math.Floor((double)(year - 1)	/	(double)100) 
					+ (int)Math.Floor((double)(year - 1)	/	(double)400) 
					+ (int)Math.Floor((double)(367 * month - 362)	/	(double)12) 
					+ (month <= 2 ? 0 : (gregorianCalendar.IsLeapYear(year) ? -1 : -2))
					+ day );
			}

			
			/// <summary>
			/// returns the converetd fixed date, year, month and day values
			/// from Gregorian to Ethiopian
			/// </summary>
			/// <param name="fixedDate">The value of the gregoria calendar in fixed date</param>
			/// <param name="year">An integer that represents the year</param>
			/// <param name="month">A 1-based integer that represents the month</param>
			/// <param name="day">A 1-based integer that represents the day</param>
			private static void FixedToEthiopian(int fixedDate,ref int year,ref int month,ref int day)
			{
				year  = (int)Math.Floor((double)( 4*(fixedDate - E_EPOCH) + 1463)/ (double)1461 );
				month = (int)Math.Floor((double)(fixedDate - EthiopianToFixed(year,1, 1))/(double) 30 ) + 1;
				day	  =  (int)( fixedDate +1 - EthiopianToFixed(year,month,1));
			}
	
		
			/// <summary>
			/// returns the converetd year, month and day values
			/// from Ethiopian to Gregorian
			/// </summary>
			/// <param name="year">An integer that represents the year</param>
			/// <param name="month">A 1-based integer that represents the month</param>
			/// <param name="day">A 1-based integer that represents the day</param>
			public static  void EthiopianToGregorian(ref int year,ref int month,ref int day)
			{
				int fixedDate=EthiopianToFixed(year,month,day);
				FixedToGregorian(fixedDate,ref year,ref month,ref day);		
			}

					
			/// <summary>
			/// returns the converetd hour
			/// from Ethiopian to Gregorian
			/// </summary>
			/// <param name="hour">An integer from 0 to 23 that represents the hour</param>
			/// <returns>An integer from 0 to 23 that represents the hour.</returns>
			public static int EthiopianToGregorian(int hour)
			{
				return 	(( 24+ hour + 6) % 24 );
			}


			/// <summary>
			/// returns the fixed date out of the Ethiopian
			/// year, month and day values
			/// </summary>
			/// <param name="year">An integer that represents the year</param>
			/// <param name="month">A 1-based integer that represents the month</param>
			/// <param name="day">A 1-based integer that represents the day</param>
			/// <returns>A fixed date representation</returns>
			public static  int EthiopianToFixed(int year,int month,int day)
			{
				return ( E_EPOCH - 1 + 365 * (year - 1)	+ (int)Math.Floor((double)year / (double)4 )
					+ 30 * (month - 1)	+ day );		
			}


			/// <summary>
			/// returns the converetd fixed date, year, month and day values
			/// from Gregorian to Ethiopian
			/// </summary>
			/// <param name="fixedDate">The value of the ethiopian calendar in fixed date</param>
			/// <param name="year">An integer that represents the year</param>
			/// <param name="month">A 1-based integer that represents the month</param>
			/// <param name="day">A 1-based integer that represents the day</param>
			private static void FixedToGregorian(int fixedDate,ref int year,ref int month,ref int day)
			{		
				int correction;
				int priorDays;

				year  = GregorianYearFromFixed( fixedDate );
				priorDays = (int)( fixedDate - GregorianToFixed(year, 1, 1) );

				correction = ( fixedDate < GregorianToFixed(year,3,1) ) ? 0 : (gregorianCalendar.IsLeapYear(year) ? 1 : 2 ) ;

				month	= (int)Math.Floor( (double)(12 * (priorDays + correction) + 373)/ (double) 367);
				day		= (int)(fixedDate - GregorianToFixed( year,month,1) + 1);

			}

			
			/// <summary>
			/// returns a Gregorian year from a fixed representation
			/// </summary>
			/// <param name="fixedDate">The value of the ethiopian calendar in fixed date</param>
			/// <returns>An integer that represents the year</returns>
			private static int GregorianYearFromFixed (int fixedDate )
			{
				int		   d0 = fixedDate - 1;           /* 1 is the Gregorian EPOCH */
				int		 n400 = (int)Math.Floor( (double) d0 / (double) 146097);
				int        d1 = d0 % 146097;
				int      n100 = (int)Math.Floor( (double) d1 / (double) 36524);
				int        d2 = d1 % 36524;
				int        n4 = (int)Math.Floor( (double) d2 / (double) 1461);
				int        d3 = d2 % 1461;
				int        n1 = (int)Math.Floor( (double) d3 / (double) 365);
				/* int        d4 = d3 % 365 + 1; */
				int year = 400 * n400 + 100 * n100 + 4 * n4 + n1;

				return ( (n100 == 4 || n1 == 4) ? year : year + 1 );
			}

			
			#endregion Functions
		}

		
		#endregion private class ConversionHelper

		#region private class DateTimeFormatEtInfo
		
		private class DateTimeFormatEtInfo
		{		
			#region MemberVars.

			private static string []abbreviatedDayNames={
															"እሁድ",	/*Ihud	- Sun*/
															"ሰኞ",	/*Segno - Mon*/
															"ማክሰ",	/*Makse- Tue*/
															"ረቡዕ",	/*Erob	- Wed*/
															"ሐሙስ",	/*Hamus - Thu*/
															"ዓርብ",	/*Arb	- Fri*/
															"ቅዳሜ"	/*Kedame- Sat*/
														};	
			
			private static string []dayNames={
												 "እሁድ",	/*Ihud		- Sunday*/
												 "ሰኞ",		/*Segno		- Monday*/
												 "ማክሰኞ",	/*Maksegno	- Tuesday*/
												 "ረቡዕ",	/*Erob		- Wed*/
												 "ሐሙስ",	/*Hamus		- Thu*/
												 "ዓርብ",	/*Arb		- Fri*/
												 "ቅዳሜ"		/*Kedame	- Sat*/
											 }; 

			private static string []abbreviatedMonthNames={ 
															  "መስከ",	/*Meske */
															  "ጥቅም",	/*Tikim */
															  "ሕዳር",	/*Hedar*/
															  "ታህሳ",	/*Tahsa*/
															  "ጥር",	/*Tir*/
															  "የካቲ",	/*Yekati*/
															  "መጋቢ",	/*Megabi*/
															  "ሚያዝ",	/*Miaz*/
															  "ግንቦ",	/*Ginbo*/
															  "ሰኔ",	/*Sene*/
															  "ሐምሌ",	/*Hamle*/
															  "ነሐሴ",	/*Nehase*/
															  "ጳጉሜ"	/*Pagume*/
														  };

			private static string []monthNames={ 
												   "መስከረም",		/*Meskerem */
												   "ጥቅምት",		/*Tikimt */
												   "ሕዳር",		/*Hedar*/
												   "ታህሳስ",		/*Tahsas*/
												   "ጥር",		/*Tir*/
												   "የካቲት",		/*Yekatit*/
												   "መጋቢት",		/*Megabit*/
												   "ሚያዝያ",		/*Miazia*/
												   "ግንቦት",		/*Ginbot*/
												   "ሰኔ",		/*Sene*/
												   "ሐምሌ",		/*Hamle*/
												   "ነሐሴ",		/*Nehase*/
												   "ጳጉሜ"		/*Pagume*/
											   };
			
			private static string   abbreviatedEraName	="ዓ.ም";				/*Amete Mehret*/
			private static string	eraName				="ዓመተ ምህረት";		/*Amete Mehret*/
			

			private static string	amDesignator		="ቀን";		/*Ken   (Day Time)*/
			private static string	pmDesignator		="ማታ";		/*Mata  (Night time)*/
			
			private static string	morningDesignator	="ጠዋት";		/*Tewat	 (Before Lunch)*/
			private static string	afternoonDesignator	="ከሰዓት";	/*Keseat (After Lunch)*/
			private static string	nightDesignator		="ምሽት";		/*Meshet (Evening)*/
			private static string	midnightDesignator	="ሌሊት";		/*Lelit	 (Mid Night)*/

			private static string	dateSeparator		="/";
			private static string	timeSeparator		=":";
			
			private static string	fullDateTimePattern	="dddd, MMMM dd, yyyy h:mm:ss tt";
			private static string	longDatePattern		="dddd, MMMM dd, yyyy";
			private static string	shortDatePattern	="M/d/yyyy";
			
			private static string	longTimePattern		="h:mm:ss tt";
			private static string	shortTimePattern	="h:mm tt";
			
			private static string	monthDayPattern		="MMMM dd";
			private static string	yearMonthPattern	="MMMM, yyyy";
			
			#endregion

			#region Constructor

			/// <summary>
			/// 
			/// </summary>
			private DateTimeFormatEtInfo()
			{
			
			}
			
			
			#endregion

			#region Properties

			/// <summary>
			/// Gets the Month Names for the 13 months in the ethiopian calendar
			/// </summary>
			/// <returns>The array of strings which contain the month names</returns>
			public static string[] GetMonthNames
			{
				get
				{
					return monthNames;
				}
				
			}

			#endregion Properties

			#region Methods


			/// <summary>
			/// Returns the abbreviated name of the specified day of the week
			/// </summary>
			/// <param name="dayofweek">A System.DayOfWeek value</param>
			/// <returns>The abbreviated name of the day of the week represented by dayofweek</returns>
			public static string GetAbbreviatedDayName(DayOfWeek dayofweek)
			{
				switch(dayofweek)
				{
					case DayOfWeek.Sunday:
						return abbreviatedDayNames[0];
					case DayOfWeek.Monday:
						return abbreviatedDayNames[1]; 
					case DayOfWeek.Tuesday:
						return abbreviatedDayNames[2]; 
					case DayOfWeek.Wednesday:
						return abbreviatedDayNames[3]; 
					case DayOfWeek.Thursday:
						return abbreviatedDayNames[4]; 
					case DayOfWeek.Friday:
						return abbreviatedDayNames[5]; 
					case DayOfWeek.Saturday:
						return abbreviatedDayNames[6];
					default:
						return abbreviatedDayNames[0];
				}		
			}


			/// <summary>
			/// Returns the full name of the specified day of the week
			/// </summary>
			/// <param name="dayofweek">A System.DayOfWeek value</param>
			/// <returns>The full name of the day of the week represented by dayofweek</returns>
			public static string GetDayName(DayOfWeek dayofweek)
			{
				switch(dayofweek)
				{
					case DayOfWeek.Sunday:
						return dayNames[0];
					case DayOfWeek.Monday:
						return dayNames[1]; 
					case DayOfWeek.Tuesday:
						return dayNames[2]; 
					case DayOfWeek.Wednesday:
						return dayNames[3]; 
					case DayOfWeek.Thursday:
						return dayNames[4]; 
					case DayOfWeek.Friday:
						return dayNames[5]; 
					case DayOfWeek.Saturday:
						return dayNames[6];
					default:
						return dayNames[0];
				}
			}


			/// <summary>
			/// Returns the string containing the abbreviated name of the specified era,
			/// if an abbreviation exists.
			/// </summary>
			/// <param name="era"></param>
			/// <returns>A string containing the abbreviated name of the specified era, if an abbreviation exists.
			/// -or- A string containing the full name of the era, if an abbreviation does not exist
			/// </returns>
			public static string GetAbbreviatedEraName( int era  )
			{
				return abbreviatedEraName; 
			}

																												   
			/// <summary>
			/// Returns the string containing the name of the specified era
			/// </summary>
			/// <param name="era">The integer representing the era.</param>
			/// <returns>A string containing the name of the era</returns>
			public static string GetEraName(int era)
			{
				return eraName; 
			}

		
			/// <summary>
			/// Returns the abbreviated name of the specified month
			/// </summary>
			/// <param name="month">An integer from 1 through 13 representing the name of the month to retrieve</param>
			/// <returns>The abbreviated name of the month represented by month</returns>
			public static string GetAbbreviatedMonthName(int month	)
			{				 
				if(month<1 || month >13)
				{
					throw new ArgumentOutOfRangeException("month",month,"Month value should be between 1 & 13");
				}
			
				return abbreviatedMonthNames[month-1]; 
			}

			
			/// <summary>
			/// Returns the full name of the specified month
			/// </summary>
			/// <param name="month">An integer from 1 through 13 representing the name of the month to retrieve</param>
			/// <returns>The full name of the month represented by month</returns>
			public static string GetMonthName(int month)
			{
				if(month<1 || month >13)
				{
					throw new ArgumentOutOfRangeException("month",month,"Month value should be between 1 & 13");
				}
			
				return monthNames[month-1]; 
			}			
								

			/// <summary>
			/// returns the string representation of the year,month,day,hour,minute,second
			/// value passed by analysing the format string
			/// </summary>
			/// <param name="year">An integer that represents the year</param>
			/// <param name="month">A 1-based integer that represents the month</param>
			/// <param name="day">A 1-based integer that represents the day</param>			
			/// <param name="dayOfWeek">A System.DayOfWeek value</param>
			/// <param name="hour">An integer from 0 to 23 that represents the hour</param>
			/// <param name="minute">An integer from 0 to 59 that represents the minute</param>
			/// <param name="second">An integer from 0 to 59 that represents the second</param>
			/// <param name="format">A string having a format character or format pattern</param>
			/// <returns></returns>
			public static string ToString(	int year,int month, int day, DayOfWeek dayOfWeek,
											int hour, int minute, int second,string format)
			{
				try
				{
					//If the format string passed is null is length is 0
					//use the general DateFormat
					if(format==null || format.Length==0)
					{
						format="g";				
					}
				
					if(format.Length == 1)
					{
						switch(format)				
						{
							case "d":						//ShortDatePattern
							{
								format=shortDatePattern;
							}break;
						
							case "D":						//LongDatePattern
							{
								format =longDatePattern; 
							}break;

							case "f":						//Full date and Time (long date and short time)
							{
								format =longDatePattern + " " +shortTimePattern; 
							}break;

							case "F":						//FullDateTimePattern(long date and long time)
							{
								format =fullDateTimePattern; 
							}break;

							case "g":						//General (short date and short time)
							{
								format =shortDatePattern+ " " + shortTimePattern;	
							}break;
						
							case "G":						//General (short date and long time)
							{
								format =shortDatePattern+ " " +longTimePattern; 
							}break;
						
							case "m":						//MonthDayPattern
							case "M":
							{
								format =monthDayPattern; 
							}break;
						
							case "t":						//ShortTimePattern
							{
								format =shortTimePattern; 
							}break;
						
							case "T":						//LongTimePattern
							{
								format =longTimePattern; 
							}break;

							case "y":						//YearMonthPattern
							case "Y":
							{
								format =yearMonthPattern; 
							}break;

							default:						//Non Supported Format Character then use "g"
							{
								format =shortDatePattern+ " " + shortTimePattern;	
							}break;
						}
					}

					//The fullname of the day of the week as defined in DayNames
					if(format.IndexOf("dddd") !=-1)		
					{
						format=format.Replace("dddd",GetDayName(dayOfWeek));     
					}
				
					//The abbreviated name of the day of the week, as defined in AbbreviatedDayNames
					if(format.IndexOf("ddd") !=-1) 
					{
						format=format.Replace("ddd",GetAbbreviatedDayName(dayOfWeek));     
					}

					//The day of the month.single digit months will have leading zeros
					if(format.IndexOf("dd") !=-1) 
					{
						format=format.Replace("dd",(day < 10 ? "0":"")+day.ToString());     
					}
				
					//The day of the month. single digit months will not have leading zeros
					if(format.IndexOf("d") !=-1) 
					{
						format=format.Replace("d",day.ToString());     
					}
				
					//The full nme of the month, as defined in monthenames
					if(format.IndexOf("MMMM") !=-1) 
					{
						format=format.Replace("MMMM",GetMonthName(month));     
					}

					//The abbreviated name of month, aas defined in abbrevitedMonthNames
					if(format.IndexOf("MMM") !=-1) 
					{
						format=format.Replace("MMM",GetAbbreviatedMonthName(month));
					}
				
					//The numeric month. Single-digit months will have a leading zero.
					if(format.IndexOf("MM") !=-1) 
					{
						format=format.Replace("MM",(month < 10 ? "0":"")+month.ToString());     
					}

					//The numeric month. Single-digit months will not have a leading zero.
					if(format.IndexOf("M") !=-1)
					{
						format=format.Replace("M",month.ToString());     
					}

					//The year in four digits, including the century
					if(format.IndexOf("yyyy") !=-1) 
					{
						format=format.Replace("yyyy",year.ToString());     
					}

					//The year without the century. If the year without the century is lessthan 10, the year is displayed
					//with a leading zero.
					if(format.IndexOf("yy") !=-1) 
					{					
						int		century=int.Parse(year.ToString().Substring(0,2))*100;
						int		yearWithNoCentury=year-century;
						format=format.Replace("yy",(yearWithNoCentury < 10 ? "0":"")+yearWithNoCentury.ToString());
					}
				
					//The year without the century. If the year without the century is less thn 10, the year is
					//displayed with no leading zero.
					if(format.IndexOf("y") !=-1) 
					{
						int		century=int.Parse(year.ToString().Substring(0,2))*100;
						int		yearWithNoCentury=year-century;
						format=format.Replace("y",yearWithNoCentury.ToString());     
					}

					//The period or era.
					if(format.IndexOf("gg") !=-1) 
					{
						format=format.Replace("gg",abbreviatedEraName);     
					}

					//The hour in a 12-hour clock. Single-digit hours will have a leading zero.
					if(format.IndexOf("hh") !=-1) 
					{
						int hour12=(hour%12)==0 ? 12 : (hour % 12);
						format=format.Replace("hh",(hour12 < 10 ? "0":"")+ hour12.ToString());     
					}

					//The hour in a 12-hour clock. Single-digit hours will not hav a leading zero.
					if(format.IndexOf("h") !=-1) 
					{
						int hour12=(hour%12)==0 ? 12 : (hour % 12);
						format=format.Replace("h",hour12.ToString());     
					}

					//The hour in a 24-hour clock. Single-digit hours will have a leading zero.
					if(format.IndexOf("HH") !=-1) 
					{
						format=format.Replace("HH",(hour < 10 ? "0":"")+ hour.ToString());     
					}

					//The hour in a 24-hour clock. Single-digit hours will not have a leading zero.
					if(format.IndexOf("H") !=-1) 
					{
						format=format.Replace("H",hour.ToString());     
					}
				
					//The minute. Single-digit minutes will have a leading zero.
					if(format.IndexOf("mm") !=-1) 
					{
						format=format.Replace("mm",(minute < 10 ? "0":"")+ minute.ToString());     
					}

					//The minute. Single-digit minutes will not have a leading zero.
					if(format.IndexOf("m") !=-1) 
					{
						format=format.Replace("m",minute.ToString());     
					}

					//The second. Single-digit seconds will have a leading zero.
					if(format.IndexOf("ss") !=-1) 
					{
						format=format.Replace("ss",(second < 10 ? "0":"")+ second.ToString());     
					}

					//The second. Single-digit seconds will not have a leading zero.
					if(format.IndexOf("s") !=-1) 
					{
						format=format.Replace("s",second .ToString());
					}

					//The morning/afternoon/evening designator defined in morningDesignator, afternoonDesignator,nightDesignator,midNightDesignator if any.
					if(format.IndexOf("ttt") !=-1) 
					{
						if(hour>=0 && hour<6)
						{
							format=format.Replace("ttt",morningDesignator);     
						}
						else if(hour >=6 && hour<12)
						{
							format=format.Replace("ttt",afternoonDesignator);						
						}
						else if(hour>=12 && hour<18)
						{
							format=format.Replace("ttt",nightDesignator);     
						}
						else if(hour>=18 && hour<= 23)
						{
							format=format.Replace("ttt",midnightDesignator);     
						}				
					}
			
					//The AM/PM designator defined in AMDesignator or PMDesignator, if any.
					if(format.IndexOf("tt") !=-1) 
					{

						format=format.Replace("tt",(hour >=0 && hour<12)? amDesignator:pmDesignator);     
					}

					//The first character in the AM/PM designator defined in AMDesignator or PMDesignator, if any.
					if(format.IndexOf("t") !=-1) 
					{
						format=format.Replace("t",(hour >=0 && hour<12)? amDesignator.Substring(0,1):pmDesignator.Substring(0,1) );     
					}

					//The default time separator defined in TimeSeparator
					if(format.IndexOf(":") !=-1) 
					{
						format=format.Replace(":",timeSeparator);     
					}

					//The default date separator defined in DateSeparator
					if(format.IndexOf("/") !=-1) 
					{
						format=format.Replace("/",dateSeparator);     
					}

					// "%c" Where c is a format pattern if used alone. The "%" character can be omitted if the format pattern
					//is combined with the literal character can be omitted if the format pattern is combined with literal characters
					//or other format patterns
					if(format.IndexOf("%") !=-1) 
					{
						format=format.Replace("%","");     
					}
				}
				catch (Exception ex)
				{
					System.Diagnostics.Trace.WriteLine( ex.GetType()+" "+ex.Message);
				}

				return format;
			}

			
					#endregion		
		}
		#endregion private class DateTimeFormatEtInfo
	}
}
