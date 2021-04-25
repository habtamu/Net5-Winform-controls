using System;
using System.Windows.Forms;  
using System.Collections;
using System.Drawing;
using System.Text; 
using System.Text.RegularExpressions;
using System.Globalization; 
 
namespace CalendarLib
{
	#region DateTimeFieldType Enumeration

	/// <summary>
	/// What type of field is it Day,Month, and Year are supported
	/// </summary>
	internal enum DateTimeFieldType
	{
		Day,
		Month,
		Year
	}
	
	#endregion DateTimeFieldType Enumeration

	#region DateTimeFieldNameFormat Enumeration
	
	/// <summary>
	/// In What way is the field\ formatted
	/// With no leadingZeros
	/// With leading zeros
	/// With Abbreviated Names
	/// With the Full Name
	/// </summary>
	internal enum DateTimeFieldNameFormat
	{
		NoLeadingZero,
		LeadingZero,
		Abbreviated,
		Full	
	}

	
	#endregion DateTimeFieldNameFormat Enumeration

	#region internal class DateTimeField

	/// <summary>
	/// The Date Time Fields That Are Displayed/Edited in the control
	/// </summary>
	internal class DateTimeField
	{
		#region Member Vars.

		private RectangleF					m_ClientRect;
		private DateTimeFieldType			m_FieldType;
		private DateTimeFieldNameFormat		m_NameFormat;
		
		private bool						m_LeftRightNavigable;
		private bool						m_UpDownModifable;
		private bool						m_Selectable;
		private bool						m_AcceptsNumericals;		

		private CharacterRange				m_CharacterRange;		

		#endregion Member Vars.

		#region Properties.

		/// <summary>
		/// Gets/Sets the Client Rect for DateTimeField
		/// </summary>
		public RectangleF ClientRect
		{		
			get
			{
				return m_ClientRect;
			}
			set
			{
				m_ClientRect=value;
			}		
		}


		/// <summary>
		/// Gets\Sets the Character Range for this DateTimeField object
		/// </summary>
		public CharacterRange CharacterRange
		{
			get
			{
				return m_CharacterRange; 
			}
			set
			{
				m_CharacterRange=value;
			}		
		}
		
		
		/// <summary>
		/// Gets Which Type is this field
		/// </summary>
		public DateTimeFieldType FieldType
		{
			get
			{
				return m_FieldType;			
			}	
		}

		
		/// <summary>
		/// Gets In Which Format the Type is renderd
		/// </summary>
		public DateTimeFieldNameFormat NameFormat
		{
			get
			{			
				return m_NameFormat; 
			}	
		}

		
		/// <summary>
		/// Gets Wheter this can be Right or Left Navigated
		/// </summary>
		public bool LeftRightNavigable
		{
			get
			{
				return m_LeftRightNavigable;
			}		
		}
		
		
		/// <summary>
		/// Gets Wheter this Field Can be Modified By the Up/Down Arrow Keys
		/// </summary>
		public bool UpDownModifable
		{
			get
			{
				return m_UpDownModifable;
			}
		}

		
		/// <summary>
		/// Gets wheter the Field Can be Selected or Not
		/// </summary>
		public bool Selectable
		{
			get
			{
				return m_Selectable;
			}		
		}


		/// <summary>
		/// Gets Wheter the Field can accept Numeral Keys
		/// </summary>
		public bool AcceptsNumericals
		{
			get
			{
				return m_AcceptsNumericals;			
			}	
		}
		
		
		#endregion Properties.

		#region Constructor

		public DateTimeField(DateTimeFieldType fieldType,DateTimeFieldNameFormat nameFormat)
		{
			//Set the Rectangle to empty
			m_ClientRect=Rectangle.Empty;
	
			//Sets the Character Range
			m_CharacterRange =new CharacterRange(0,0); 

			//Set the Field Type
			m_FieldType		=fieldType;
			
			//Set the Name Format;
			//The Year DateTimeFieldType can be assigned a format of abbreviated because
			//Year Part supports 3 formats NoLeadingZeros,WithZeros and Full if Abbreviated
			//is set change it to Full format
			m_NameFormat	= (fieldType == DateTimeFieldType.Year && 
							   nameFormat == DateTimeFieldNameFormat.Abbreviated) ?
							  DateTimeFieldNameFormat.Full : nameFormat;

			//Only the Day parts with abbreviated or full names are non selectable
			//editable and non navigable
			if( fieldType == DateTimeFieldType.Day && 
				(nameFormat == DateTimeFieldNameFormat.Abbreviated || nameFormat == DateTimeFieldNameFormat.Full ))
			{
				m_LeftRightNavigable	=false;
				m_UpDownModifable		=false;
				m_Selectable			=false;
				m_AcceptsNumericals		=false;
			}
			else
			{
				m_LeftRightNavigable	=true;
				m_UpDownModifable		=true;
				m_Selectable			=true;
				m_AcceptsNumericals		=true;		
			}	
		}

		
		#endregion Constructor
	}

	#endregion internal class DateTimeField

	#region internal class DateTimeFieldCollection

	/// <summary>
	/// Collections of the DateTime Fields
	/// </summary>
	internal class DateTimeFieldCollection : CollectionBase
	{		
		#region Static Member Vars

		private static string	fullDateTimePattern	="dddd, MMMM dd, yyyy h:mm:ss tt";
		private static string	longDatePattern		="dddd, MMMM dd, yyyy";
		private static string	shortDatePattern	="M/d/yyyy";
			
		private static string	longTimePattern		="h:mm:ss tt";
		private static string	shortTimePattern	="h:mm tt";
			
		private static string	monthDayPattern		="MMMM dd";
		private static string	yearMonthPattern	="MMMM, yyyy";
		private static object[] supportedFormats	
			={
				 new object[]{"d"    ,DateTimeFieldType.Day ,DateTimeFieldNameFormat.NoLeadingZero},
				 new object[]{"dd"   ,DateTimeFieldType.Day ,DateTimeFieldNameFormat.LeadingZero},
				 new object[]{"ddd"  ,DateTimeFieldType.Day ,DateTimeFieldNameFormat.Abbreviated},
				 new object[]{"dddd" ,DateTimeFieldType.Day ,DateTimeFieldNameFormat.Full},
				 new object[]{"M"	 ,DateTimeFieldType.Month ,DateTimeFieldNameFormat.NoLeadingZero},
				 new object[]{"MM"   ,DateTimeFieldType.Month ,DateTimeFieldNameFormat.LeadingZero},
				 new object[]{"MMM"  ,DateTimeFieldType.Month ,DateTimeFieldNameFormat.Abbreviated},
				 new object[]{"MMMM" ,DateTimeFieldType.Month ,DateTimeFieldNameFormat.Full},
				 new object[]{"y"	 ,DateTimeFieldType.Year  ,DateTimeFieldNameFormat.NoLeadingZero},
				 new object[]{"yy"   ,DateTimeFieldType.Year  ,DateTimeFieldNameFormat.LeadingZero},
				 new object[]{"yyyy" ,DateTimeFieldType.Year  ,DateTimeFieldNameFormat.Full}
			 };

		   
			
	
		#endregion Static Member Vars

		#region Member Vars.
	
		private string				formatStr;
		private DateTimeField		m_selectedField;
		private int					m_storedValue;
		private bool				m_editing;

		#endregion Member Vars.	

		#region Constructors

		/// <summary>
		/// Constructs the Collection of DateTimeFields from the formatString
		/// </summary>
		/// <param name="format">string Having the format if the format string is
		/// null or empty then fields based on the "g" format will be created</param>
		public DateTimeFieldCollection(string format):base()
		{			
			BuildDateTimeFields(format);
		}
	
		
		#endregion Constructors		

		#region Events
		
		public event	EventHandler				FieldChanged		= null;
	
		public event	DateAssignedEventHandler	DateAssigned		= null;

		public event	DateSpinEventHandler		DateSpinned			= null;

		public event	EventHandler				EditingOnProgress	= null;

		public event	EventHandler				EditingEnded		= null;

		#endregion Events
		
		#region Event Raisers

		/// <summary>
		/// Raises the Field Changed Event
		/// </summary>
		protected void RaiseFieldChangedEvent()
		{
			if(FieldChanged !=null)
			{
				FieldChanged(this,EventArgs.Empty); 
			}		
		}


		/// <summary>
		/// Raises the DateTime Assigned Event
		/// </summary>
		/// <param name="e">The EventArgs that holds the DateTimeAssigned info</param>
		protected void RaiseDateAssignedEvent(DateTimeFieldAssignedArgs e)
		{	
			if(DateAssigned != null)
			{
				DateAssigned(this,e);		
			}
		}
		
		
		/// <summary>
		/// Raises the Date Spinned Event
		/// </summary>
		/// <param name="e">The DateTimeFieldSpin event Args</param>
		protected void RaiseDateSpinnedEvent(DateTimeFieldSpinArgs e)
		{	
			if(DateSpinned != null)
			{
				DateSpinned(this,e);		
			}	
		}


		/// <summary>
		/// Raises the Editing Started Event
		/// </summary>
		protected void RaiseEditingOnProgress()
		{
			if(!m_editing && EditingOnProgress  !=null)
			{
				m_editing = true;
				EditingOnProgress (this,EventArgs.Empty);
			}
		}

		
		/// <summary>
		/// Raises the Editing Ended Event
		/// </summary>
		protected void RaiseEditingEnded()
		{
			if(m_editing && EditingEnded != null)
			{
				m_editing		= false;
				m_storedValue	= 0;
				EditingEnded(this,EventArgs.Empty);   
			}		
		}

		
		#endregion Event Raisers

		//Properties

		#region CharacterRanges

		/// <summary>
		/// Gets the Character Range
		/// </summary>
		protected CharacterRange[] CharacterRanges
		{
			get
			{
				CharacterRange[] ranges=new CharacterRange[this.Count];

				int i=0;
				foreach(DateTimeField dateTimeField in this)
				{
					ranges[i]=dateTimeField.CharacterRange;
					i++;	
				}			
				return ranges;
			}	
		}


		#endregion Character Ranges		

		#region SelectedField

		/// <summary>
		/// Gets\Sets the SelectedField
		/// </summary>
		public DateTimeField SelectedField
		{
			get
			{
				return m_selectedField;
			}		

			
			set
			{
				if(m_selectedField !=value)
				{
					RaiseEditingEnded(); 
					m_selectedField	= value;
					m_storedValue	= -1;
				}
			}
		}

		#endregion SelectedField

		#region StoredValue
		
		/// <summary>
		/// Gets or Sets the Stored value 
		/// </summary>
		public int StoredValue
		{
			get
			{
				return m_storedValue;
			}
		
			set
			{
				if(value!=m_storedValue)
				{
					m_storedValue=value;
				}
			
			}	
		}


		#endregion StoredValue		

		//Methods

		#region EndEditing

		/// <summary>
		/// Finishes the Current editing of a field
		/// if there is one! Call this only when the enterd
		/// value is validated and reaches the limit
		/// </summary>
		public void EndEditing()
		{
			RaiseEditingEnded();		
		}

		#endregion EndEditing

		#region BuildDateTimeFields
		
		/// <summary>
		/// Builds the DateTime Fields from the format string given
		/// </summary>
		/// <param name="format">The format string to be used</param>
		public void BuildDateTimeFields(string format)
		{
			this.Clear();
			m_selectedField  	=	null;
			formatStr			=   string.Copy(format);
			m_storedValue		=	-1;
			m_editing			=	false;
			 
			//Note that only day month and year part of strings will be processed
			//If the format string passed is null or its  length is 0
			//use the general DateFormat
			if(formatStr==null || formatStr.Length==0)
			{
				formatStr="g";				
			}
				
			if(formatStr.Length == 1)
			{
				switch(formatStr)				
				{
					case "d":						//ShortDatePattern
					{
						formatStr=shortDatePattern;
					}break;
						
					case "D":						//LongDatePattern
					{
						formatStr =longDatePattern; 
					}break;

					case "f":						//Full date and Time (long date and short time)
					{
						formatStr =longDatePattern + " " +shortTimePattern; 
					}break;

					case "F":						//FullDateTimePattern(long date and long time)
					{
						formatStr =fullDateTimePattern; 
					}break;

					case "g":						//General (short date and short time)
					{
						formatStr =shortDatePattern+ " " + shortTimePattern;	
					}break;
						
					case "G":						//General (short date and long time)
					{
						formatStr =shortDatePattern+ " " +longTimePattern; 
					}break;
						
					case "m":						//MonthDayPattern
					case "M":
					{
						formatStr =monthDayPattern; 
					}break;
						
					case "t":						//ShortTimePattern
					{
						formatStr =shortTimePattern; 
					}break;
						
					case "T":						//LongTimePattern
					{
						formatStr =longTimePattern; 
					}break;

					case "y":						//YearMonthPattern
					case "Y":
					{
						formatStr =yearMonthPattern; 
					}break;

					default:						//Non Supported Format Character then use "g"
					{
						formatStr =shortDatePattern+ " " + shortTimePattern;	
					}break;
				}
			}			

			//Now Search the format string and add objects as required;
			SortedList sortedList=new SortedList();
			Regex	r;
			
			
			foreach(object[] formatObj in supportedFormats)
			{
				r	=new Regex("\\b"+(string)formatObj[0]+"\\b");
				
				foreach(Match m in r.Matches(formatStr))
				{
					if(m.Success)
					{
						foreach(Capture c in m.Captures)
						{
							sortedList.Add( c.Index,new DateTimeField( (DateTimeFieldType) formatObj[1],
											(DateTimeFieldNameFormat)formatObj[2]));						
						}
					}
				}
			}
	
			foreach(object obj in sortedList.Values)
			{
				this.Add((DateTimeField)obj);
			}		 
		}
		

		#endregion BuildDateTimeFields

		#region Update Locations

		/// <summary>
		/// Updates the location for the DateTimeField objects
		/// </summary>
		/// <param name="dateTime">The DateTime Value that is currently set</param>
		/// <param name="gregorian">Flag wheter the current calendar is gregorian or ethiopian</param>
		/// <param name="font">The font that would be used to render the formatted DateTime</param>
		/// <param name="g">The graphics object that is used to draw the string</param>	
		/// <param name="rectF">The Layout rectangle</param>	
		public void UpDateLocations(DateTime dateTime,bool gregorian,Font font,Graphics g, RectangleF rectF)
		{
			try			
			{
				int		length		=0;
				string	formatVar	="";
			
				StringBuilder   strBuilder		= new StringBuilder(formatStr);
				string			tempFormatStr	= formatStr;
				string			tempDateTime	="";
				Regex			r;

			
				//First get all the key words we are interested in
				//i.e d,dd,ddd,dddd,M,MM,MMM,MMMM,y,yy,yyyy
				foreach(object[] formatObj in supportedFormats)
				{
					formatVar	=(string) formatObj[0];
				 
					r	=new Regex("\\b"+formatVar+"\\b");
				
					foreach(Match m in r.Matches(tempFormatStr))
					{
						if(m.Success)
						{						
							// if they exist then convert the DateTime object
							// to string and determine how many chars will be taken by that
							if(gregorian)
							{
								length = dateTime.ToString(" "+formatVar).Length - 1;
									 
							}
							else
							{
								length = EthiopianCalendar.ToString(dateTime," "+formatVar).Length -1;
									 
							}
						
							//Replace the instance of all the ocuurance of the Format chars with the "?" char
							foreach(Capture c in m.Captures)
							{
								strBuilder.Replace(formatVar,new string('!',length),c.Index,formatVar.Length);
							}						
						}
						tempFormatStr = strBuilder.ToString();
					}				
				}

				//Use the modified format string to format the DateTime object
				if(gregorian)
				{
					tempDateTime = dateTime.ToString(tempFormatStr); 
				}
				else
				{
					tempDateTime = EthiopianCalendar.ToString(dateTime,tempFormatStr);			
				}

//				System.Console.WriteLine(strBuilder.ToString());
//				System.Console.WriteLine(tempDateTime);
//				System.Console.WriteLine(); 

				//Now record the starting and ending indexes from the new converted DateTime string
				r	=new Regex(@"[!]+");
				int i=0;	
				foreach(Match m in r.Matches(tempDateTime))
				{
					if(m.Success)
					{						
						foreach(Capture c in m.Captures)
						{
							this[i].CharacterRange	= new CharacterRange(c.Index,c.Length);
							i++;
						}						
					}				
				}

				//Set the measurable Character ranges
				StringFormat  stringFormat= new  StringFormat();
				stringFormat.SetMeasurableCharacterRanges(this.CharacterRanges);   

				//Now format the DateTime with thecorrect format string			
				string correctDateTime;
				if(gregorian)
				{
					correctDateTime = dateTime.ToString(formatStr); 
				}
				else
				{
					correctDateTime = EthiopianCalendar.ToString(dateTime,formatStr);
				}
			
				Region[]  fieldRegion = g.MeasureCharacterRanges(correctDateTime,font,rectF,stringFormat);
				RectangleF fieldRectangle;
				
				for(int index=0; index < fieldRegion.Length ;index++)
				{
					fieldRectangle =fieldRegion[index].GetBounds(g);
					fieldRectangle.Inflate(0,-1);  
					this[index].ClientRect=fieldRectangle; 
				}				
			}
			catch(Exception ex)
			{
				System.Diagnostics.Trace.WriteLine(ex.Message);    
			}
		}		


		#endregion Update Locations

		//Helper Methods

		#region DateTimePicker Helpers	
		

		/// <summary>
		/// Processes the Key Down Events
		/// </summary>
		/// <param name="e">The Key Event Args</param>
		public void OnKeyDown(KeyEventArgs e)
		{
			if(e.KeyData == Keys.F4)
			{
				RaiseEditingEnded(); 
			}

			//This assures us that at least one field can be selected (Prevents infinite loop)
            if(SelectedField != null)
			{
				//Get the index of the currently selected field
				int index = IndexOf(this.SelectedField);
				if(index==-1)	//Never happens 
				{
					return;
				}

				//Now if the user wants to navigate to left or right !!
				if(e.KeyData == Keys.Left || e.KeyData == Keys.Right)
				{
					//By what value should we increment or decrement the field
					int seed = e.KeyData == Keys.Left ? -1 : 1;
					
					while(true)
					{
						index+=seed;
						if(index > (this.Count -1) )	//If greater than limit loop back
						{
							index = 0;
						}
						else if (index < 0 )			//If less than limit roll back
						{
							index = this.Count -1;
						}
						
						if(this[index].Selectable)			//Only if selectable 
						{
							this.SelectedField = this[index];	//Set this as the current field
							RaiseFieldChangedEvent();			//Raise the Event
							break;								//break out of the loop
						}					
					}
				}	//end if(e.KeyData == Keys.Left || e.KeyData == Keys.Right)

					//User want to increment values using the arrow up/down keys
				else if(e.KeyData == Keys.Up || e.KeyData == Keys.Down)
				{					
					//Raise the event with the correct parameters
					RaiseDateSpinnedEvent(new DateTimeFieldSpinArgs(this[index].FieldType,e.KeyData == Keys.Up));   
				}	//end if(e.KeyData == Keys.Up || e.KeyData == Keys.Down)
				//If the selected item can accept numericals and the hit key is a numerical
				else if( this.SelectedField.AcceptsNumericals &&
						((e.KeyData >= Keys.NumPad0 && e.KeyData <= Keys.NumPad9) || 
						(e.KeyData >= Keys.D0  && e.KeyData <= Keys.D9 )))
				{
					RaiseEditingOnProgress();
  
					if(m_storedValue == -1)
					{
						m_storedValue=Utilities.IntValue(e.KeyData);   
					}
					else
					{
						m_storedValue=Utilities.IntValue(m_storedValue,e.KeyData);   
					}

					RaiseDateAssignedEvent(new DateTimeFieldAssignedArgs(this[index].FieldType,m_storedValue));
				}
			}
		}


		/// <summary>
		/// Processes the OnMouseDown event in the DateTimeFields collection
		/// </summary>
		/// <param name="e">The Mouse Event Args</param>
		public void OnMouseDown(MouseEventArgs e)
		{		
			if(e.Button == MouseButtons.Left)
			{
				RaiseEditingEnded();
 
				bool valueSet=false;
				//First Check wheter the coordinate is in one of the DateTime Fields
				foreach(DateTimeField field in this)
				{
					//Check wheter the coordinate is in one of the fields
					if(field.ClientRect.Contains(new PointF(e.X,e.Y)))
					{
						valueSet = true;

						if(field.Selectable)	//If the field clicked on is selectable
						{
							SelectedField	= field;	//Set the selected field							
						}
						else if(SelectedField ==null)	//this means the field is not selectable
						{
							//If the prev selected field was null we need to assign a new value
							valueSet=false;
						}
						
						break;
					}
				}
				
				if(!valueSet)	//If the value was/is not set
				{
					//If the value was not set
					//If the user clicked on the left most part first selectatble
					//field will be selected if the user selected at the right most
					//psrt the first selectable field from the right will be selected
					for(int index=0;index < this.Count ; index ++)
					{
						if((e.X < this[index].ClientRect.X && this[index].Selectable))
						{
							SelectedField	= this[index];
							valueSet		= true;
							break;
						}
						else if (e.X > this[this.Count -1 - index].ClientRect.X  && this[this.Count -1 - index].Selectable)
						{
							SelectedField	= this[this.Count -1 - index];
							valueSet		= true;
							break;						
						}
					}
				}

				if(valueSet)
				{
					RaiseFieldChangedEvent();
				}
			}
		}


		/// <summary>
		/// When the controls gets focus
		/// </summary>
		public void OnGotFocus()
		{
			Point pt=this.SelectedField == null ? new Point(0,0) : new Point((int)SelectedField.ClientRect.X +2,(int)SelectedField.ClientRect.Y+2);   
			MouseEventArgs e=new MouseEventArgs(MouseButtons.Left,1,pt.X,pt.Y,0);
			OnMouseDown(e);		
		}

		
		/// <summary>
		/// When the control loses focus
		/// </summary>
		public void OnLostFocus()
		{
			RaiseEditingEnded();  
			RaiseFieldChangedEvent();
		}


		#endregion DateTimePicker Helpers

		//Collection Methods

		#region Collection Spec Methods

		#region Indexer
		
		public DateTimeField  this[ int index ]  
		{
			get  
			{
				return( (DateTimeField) List[index] );
			}
			set  
			{
				List[index] = value;
			}
		}

		
		#endregion Indexer

		#region Add
		
		public int Add( DateTimeField value )  
		{
			return( List.Add( value ) );
		}
		
		
		#endregion Add

		#region IndexOf
		
		public int IndexOf( DateTimeField value )  
		{
			return( List.IndexOf( value ) );
		}

		
		#endregion IndexOf

		#region Insert
		
		public void Insert( int index, DateTimeField value )  
		{
			List.Insert( index, value );
		}

		
		#endregion Insert

		#region Remove
		
		public void Remove( DateTimeField value )  
		{
			List.Remove( value );
		}

		
		#endregion Remove

		#region Contains
		
		public bool Contains( DateTimeField value )  
		{
			return( List.Contains( value ) );
		}

		
		#endregion Contains

		#region OnInsert
		
		/// <summary>
		/// Performs additional custom processes before inserting a new element into the CollectionBase instance
		/// </summary>
		/// <param name="index">The zero-based index at which to insert value</param>
		/// <param name="value">The new value of the element at index</param>
		protected override void OnInsert( int index, Object value )  
		{
			if ( value.GetType() != typeof(DateTimeField) )
				throw new ArgumentException( "value must be of type DateTimeField.", "value" );
		}


		#endregion OnInsert
		
		#region OnRemove

		/// <summary>
		/// Performs additional custom processes when removing an element from the CollectionBase instance.		
		/// </summary>
		/// <param name="index">The zero-based index at which value can be found</param>
		/// <param name="value">The value of the element to remove from index</param>
		protected override void OnRemove( int index, Object value )  
		{
			
			if ( value.GetType() != typeof(DateTimeField) )
				throw new ArgumentException( "value must be of type DateTimeField.", "value" );
		}


		#endregion OnRemove
		
		#region OnSet

		/// <summary>
		/// Performs additional custom processes before setting a value in the CollectionBase instance
		/// </summary>
		/// <param name="index">The zero-based index at which oldValue can be found</param>
		/// <param name="oldValue">The value to replace with newValue</param>
		/// <param name="newValue">The new value of the element at index</param>
		protected override void OnSet( int index, Object oldValue, Object newValue )  
		{
			if ( newValue.GetType() != typeof(DateTimeField) )
				throw new ArgumentException( "value must be of type DateTimeField.", "newValue" );
		}


		#endregion OnSet
		
		#region OnValidate

		/// <summary>
		/// Performs additional custom processes when validating a value
		/// </summary>
		/// <param name="value">The object to validate</param>
		protected override void OnValidate( Object value )
		{
			base.OnValidate(value); 
			if ( value.GetType() != typeof(DateTimeField) )
				throw new ArgumentException( "value must be of type DateTimeField.", "value" );
		}


		#endregion OnValidate		

		#endregion Collection Spec Methods
	}

	#endregion internal class DateTimeFieldCollection

	#region EventArgs


	/// <summary>
	/// Passed when a user enters a new DateTime value from
	/// Direct Key Board Input
	/// </summary>
	internal class DateTimeFieldAssignedArgs: EventArgs 
	{
		#region Member Vars.

		private DateTimeFieldType   m_FieldType;
		private int					m_NewValue;	

		#endregion Member Vars.

		#region Constructor

		/// <summary>
		/// Constrcts the DateTimeFieldAssigned Args (Use this when value is set from key
		/// board input)
		/// </summary>
		/// <param name="fieldType">The field Type (year,month,day)</param>
		/// <param name="newValue">The integer value of the new value set</param>
		public DateTimeFieldAssignedArgs(DateTimeFieldType fieldType,int newValue)
		{
			m_FieldType	= fieldType;
			m_NewValue	= newValue;		
		}


		#endregion Constructor	

		#region Properties

		/// <summary>
		/// Gets the Field Type
		/// </summary>
		public DateTimeFieldType FieldType
		{
			get
			{
				return m_FieldType; 
			}
		}

		
		/// <summary>
		/// Gets\Sets the new Value set
		/// </summary>
		public int	NewValue
		{
			get
			{
				return m_NewValue;
			}
		}

		
		#endregion Properties
	}

	
	/// <summary>
	/// Passed When the user changes DateTime value by using the up/down
	/// keys
	/// </summary>
	internal class DateTimeFieldSpinArgs: EventArgs 
	{
		#region Member Vars.

		/// <summary>
		/// Which type is it (Year,Month,Day)
		/// </summary>
		private DateTimeFieldType   m_FieldType;

		/// <summary>
		/// Which direction did user used Up/Down
		/// </summary>
		private bool				m_Up;

		#endregion Member Vars.

		#region Constructor

		/// <summary>
		/// Constructs the DateTimeFieldsSpinArgs which is used when the user changes
		/// values by sing the up/down arrow keys
		/// </summary>
		/// <param name="fieldType">Which Field Type(Day,Month,Year)</param>
		/// <param name="up">Which Direction Up/Down key used</param>
		public DateTimeFieldSpinArgs(DateTimeFieldType fieldType,bool up)
		{
			m_FieldType = fieldType;
			m_Up		= up;
		}


		#endregion Constructor	

		#region Properties

		/// <summary>
		/// Gets the Field Type
		/// </summary>
		public DateTimeFieldType FieldType
		{
			get
			{
				return m_FieldType; 
			}
		}

		/// <summary>
		/// Gets the Direction Up/Down (Incrementing/Decrementing)
		/// </summary>
		public bool	Up
		{
			get
			{
				return m_Up;
			}
		}
		

		#endregion Properties
	}
	

	#endregion EventArgs

	#region Delegates

	internal delegate void DateAssignedEventHandler	(object sender, DateTimeFieldAssignedArgs e);

	internal delegate void DateSpinEventHandler		(object sender, DateTimeFieldSpinArgs e);

	#endregion Delegates
}