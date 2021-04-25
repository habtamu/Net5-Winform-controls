#define DESIGN

using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;  
using System.ComponentModel;
using System.Globalization;
using System.Collections;

#region Description
/*
  Managed version of the DateTimePicker control. 
  
  DateTimePickerFormat enumeration
  --
  Short - displays the date value in the short date format.
  Long -  displays the date value in the long date format.

  DateTimePicker class
  --
  Label-like control that displays the current date value and draws a
  downarrow on the far right. Hides and shows the popup date picker 
  control. Fires DropDown, CloseUp and ValueChanged events.
  
  DayPickerPopup class
  --
  Displays the calendar that allows users to select a date. Highlights
  current selection and 'today'. Draws box around date cell when user
  hovers over different dates. Can navigate days using device jog buttons.

  Left and right arrows in caption area that navigate months. Clicking on the
  month in the caption displays context menu to quickly jump to any month. 
  Clicking on the year in the caption displays a numeric updown control to 
  quickly jump to different years. Displays today's date at the bottom, jumps
  to today's date when clicked.
    
  The popup is intelligently positioned; it is located under the label control
  is possible, if not, the popup is positioned above the label control. The same
  logic when calculating the horizontal position. The popup is added to the topmost
  control so it appears on top of tab controls, etc.
  --
  CulturInfo, FirstDayofWeek, Font, MinMax-Date and Resizing support by Mark Johnson, Berlin Germany - mj10777@mj10777.de at coments :
  // mj10777 CultureInfo Support
  --
*/

#endregion Description

namespace CalendarLib
{		
	#region DateTimePickerEx class
		
	/// <summary>
	///  <list type="table">
	///   <item><description>Managed DateTimePicker control.</description></item>
	///  </list>
	/// </summary>
	/// <remarks>
	///  <para>User can select a day from popup calendar.</para>
	/// </remarks>
	[System.ComponentModel.DefaultEvent("ValueChanged")]
	[ToolboxItem(true),ToolboxBitmap(typeof(DateTimePickerEx))]
	public class DateTimePickerEx : Control
	{
		#region Const

		class Const
		{
			/// <summary>
			/// The width of the arrow that will be used to show
			/// a combo like drop down button
			/// </summary>
			public const int  arrowWidth  =20;
		}
		
		
		#endregion Const

		#region Apperance\Style	

		/// <summary>
		///  <list type="table">
		///   <item><description>Date Format</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>DateTimePickerFormat.Custom</para>
		///   </item>
		///  </list>
		/// </remarks>
		/// <seealso cref="DateTimePicker.Format"/>
		/// <seealso cref="DateTimePicker.Text"/>
		private DateTimePickerFormat dtpf_Format = DateTimePickerFormat.Custom;
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Lost Focus</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Attempt to solve LostFocus Problem outside the DateTimePicker</para>
		///  <para>Has the Event Method been sent to Parent?</para>
		///  <para>- if not : send it</para>
		/// </remarks>
		private bool b_LostFocusEvent	= false;

		/// <summary>
		/// Notifies wheter the mouse is down or not
		/// </summary>
		private bool b_MouseDown		= false;

		/// <summary>
		/// Is time picker enabled to be viewed or is it hidden and non editable
		/// </summary>
		private bool b_TimePicker ;

		/// <summary>
		///  <list type="table">
		///   <item><description>Used s_DaysOfWeek</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Stores set s_DayOfWeek used by the DateTimePicker</para>
		/// </remarks>
		public  string[] s_DayOfWeek;

		#endregion Apperance\Style

		#region Region\Coordinate\Size Vars.

		/// <summary>
		///  <list type="table">
		///   <item><description>Size of the drop arrow on far right</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para> Size(7,4);</para>
		///   </item>
		///  </list>
		///  <para>no logic build in to resize</para>
		/// </remarks>
		/// <seealso cref="DateTimePickerEx.CreateMemoryBitmap"/>
		public static Size DropArrowSize	= new Size(7,4);
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Array for Position of down Arrow coordinates</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>offscreen bitmap</para>
		///  <para>[0] ; Point Top/Left</para>
		///  <para>[1] ; Point Top/Right</para>
		///  <para>[2] ; Point Bottom/Middle</para>
		/// </remarks>
		/// <seealso cref="DateTimePickerEx.CreateMemoryBitmap"/>
		private Point[] pta_ArrowPoints		= new Point[3];
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Position of down Arrow coordinates</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>offscreen bitmap</para>
		/// </remarks>
		/// <seealso cref="DateTimePickerEx.CreateMemoryBitmap"/>
		private Rectangle rt_DropArrow		=Rectangle.Empty;

		#endregion Region\Coordinate\Size Vars.

		#region Graphics\Disposable Vars.

		/// <summary>
		///  <list type="table">
		///   <item><description>offscreen bitmap</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <seealso cref="DateTimePickerEx.CreateMemoryBitmap"/>
		/// <seealso cref="DateTimePickerEx.OnPaint"/>
		Bitmap bmp_Bmp;
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Graphics for Painting</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <seealso cref="DateTimePickerEx.CreateMemoryBitmap"/>
		/// <seealso cref="DateTimePickerEx.OnPaint"/>
		Graphics gr_Graphics;
			
		/// <summary>
		///  <list type="table">
		///   <item><description>SolidBrush Foreground</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>this.ForeColor;</para>
		///   </item>
		///  </list>
		///  <para>GDI Objects</para>
		/// </remarks>
		/// <seealso cref="DateTimePickerEx.CreateGdiObjects"/>
		/// <seealso cref="DateTimePickerEx.OnPaint"/>
		SolidBrush sb_Foreground;
			
		/// <summary>
		///  <list type="table">
		///   <item><description>SolidBrush Disabled</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>SystemColors.GrayText</para>
		///   </item>
		///  </list>
		///  <para>GDI Objects</para>
		/// </remarks>
		/// <seealso cref="DateTimePickerEx.CreateGdiObjects"/>
		/// <seealso cref="DateTimePickerEx.OnPaint"/>
		SolidBrush sb_Disabled;
			
		/// <summary>
		///  <list type="table">
		///   <item><description>SolidBrush Frame</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>SystemColors.WindowFrame</para>
		///   </item>
		///  </list>
		///  <para>GDI Objects</para>
		/// </remarks>
		/// <seealso cref="DateTimePickerEx.CreateGdiObjects"/>
		/// <seealso cref="DateTimePickerEx.OnPaint"/>
		SolidBrush sb_Frame;
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Pen Frame</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>SystemColors.WindowFrame</para>
		///   </item>
		///  </list>
		///  <para>GDI Objects</para>
		/// </remarks>
		/// <seealso cref="DateTimePickerEx.CreateGdiObjects"/>
		/// <seealso cref="DateTimePickerEx.OnPaint"/>
		Pen pen_Frame;



		#endregion Graphics\Disposable Vars.

		#region Controls\Components Vars.

		/// <summary>
		///  <list type="table">
		///   <item><description>The Day Picker</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>SystemColors.WindowFrame</para>
		///   </item>
		///  </list>
		///  <para>Displays Popup Month Calendar</para>
		/// </remarks>
		/// <seealso cref="DateTimePickerEx()"/>		
		/// <seealso cref="DateTimePickerEx.CustomFormat"/>
		/// <seealso cref="DateTimePickerEx.DayOfWeekCharacters"/>
		/// <seealso cref="DateTimePickerEx.FirstDayOfWeek"/>
		/// <seealso cref="DateTimePickerEx.Font"/>
		/// <seealso cref="DateTimePickerEx.FontCaption"/>
		/// <seealso cref="DateTimePickerEx.FontDay"/>
		/// <seealso cref="DateTimePickerEx.FontToday"/>
		/// <seealso cref="DateTimePickerEx.MinDateTime"/>
		/// <seealso cref="DateTimePickerEx.MaxDateTime"/>
		/// <seealso cref="DateTimePickerEx.OnMouseDown"/>
		/// <seealso cref="DateTimePicker.Value"/>
		// day picker, displays popup month calendar
		DayPickerPopup	dpp_DayPicker = null;

		/// <summary>
		/// The time picker that is used to show and edit time
		/// </summary>
		TimePicker		tp_TimePicker = null;	

		#endregion Controls\Components Vars.		

		#region DateTimeField

		private DateTimeFieldCollection			m_fieldsCollection;	
		private DateTime						m_editedDateTime;
		private bool							m_isEditing;

		#endregion DateTimeField

		#region Event

		/// <summary>
		///  <list type="table">
		///   <item><description>Occurs when the Value property changes.</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Exposed Events</para>
		/// </remarks>
		/// <seealso cref="DateTimePicker"/>
		/// <seealso cref="DateTimePicker.Value"/>
		/// <seealso cref="DateTimePickerEx.OnDayPickerValueChanged"/>
		public event EventHandler ValueChanged;
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Occurs when the drop-down calendar is dismissed and disappears</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Exposed Events</para>
		/// </remarks>
		/// <seealso cref="DateTimePicker"/>
		/// <seealso cref="DateTimePickerEx.OnDayPickerCloseUp"/>
		/// <seealso cref="DateTimePickerEx.OnMouseDown"/>
		public event EventHandler CloseUp;
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Occurs when the drop-down calendar is shown.</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Exposed Events</para>
		/// </remarks>
		/// <event cref="DropDown">
		///    Raised when something occurs.
		/// </event>
		/// <seealso cref="DateTimePickerEx.OnDayPickerCloseUp"/>
		/// <seealso cref="DateTimePickerEx.OnMouseDown"/>
		public event EventHandler DropDown;

		#endregion Event

		#region Constructor
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Constructor. Initializes a new instance of the DateTimePickerEx class.</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Create DayPickerPopup</para>
		///  <para>Set active CultureInfo to DayPickerPopup</para>
		///  <para>Set Events to DayPickerPopup</para>
		/// </remarks>		
		/// <seealso cref="DateTimePickerEx.dpp_DayPicker"/>
		/// <seealso cref="DateTimePickerEx.OnDayPickerCloseUp"/>
		/// <seealso cref="DateTimePickerEx.OnDayPickerValueChanged"/>
		public DateTimePickerEx()
		{
			dpp_DayPicker	=	new DayPickerPopup();
			tp_TimePicker	=	new TimePicker();			

			m_fieldsCollection	= new DateTimeFieldCollection("MMMM dd, yyyy");	//Default construc using the custom format and format string
			m_isEditing			= false;		//Are we currently editing ?
			m_editedDateTime	= Value;		//The new edited DateTime
			
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.Selectable,true);			

			// Set Font after DayPickerPopup !
			this.CalendarFont   = new Font("Ethiopia Jiret",11F,FontStyle.Regular);
			// Set Size to fit ShortDate with this Font - Designer support
			this.Size   = new Size(150, 20);
			
			tp_TimePicker.Location	=new System.Drawing.Point(this.Width,0);
			tp_TimePicker.Visible	=b_TimePicker;
			
			this.Controls.Add(tp_TimePicker);
			
			// hookup day picker events
			dpp_DayPicker.CloseUp		+= new EventHandler		(OnDayPickerCloseUp);
			dpp_DayPicker.ValueChanged	+= new EventHandler	(OnDayPickerValueChanged);

			//hook up time picker events
			tp_TimePicker.ValueChanged				+=new EventHandler(OnTimePickerValueChanged);			

			//hook up DateTime Field Events
			m_fieldsCollection.FieldChanged			+=new EventHandler(m_fieldsCollection_FieldChanged);
			m_fieldsCollection.DateAssigned			+=new DateAssignedEventHandler(m_fieldsCollection_DateAssigned);
			m_fieldsCollection.DateSpinned			+=new DateSpinEventHandler(m_fieldsCollection_DateSpinned);
			m_fieldsCollection.EditingOnProgress	+=new EventHandler(m_fieldsCollection_EditingOnProgress);
			m_fieldsCollection.EditingEnded			+=new EventHandler(m_fieldsCollection_EditingEnded); 
			
		} // public DateTimePicker()
			
			
		#endregion Constructor		

		#region BMP
				
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets created DayPickerPopup Bitmap</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to retrieve results of DayPickerPopup</para>
		/// </remarks>
		/// <value>Returns Bitmap created by DayPickerPopup</value>
		/// <seealso cref="DayPickerPopup.BMP"/>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public System.Drawing.Bitmap BMP
		{
			get
			{
				return this.dpp_DayPicker.BMP;
			}
		} // public System.Drawing.Bitmap BMP
				
			
		#endregion

		#region Calendar Support for Control

		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets wheter the gregorian calendar is to be used as the current calendar or the ethiopian calendarr</description>
		///   </item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to specify wheter gregorian or ethiopian caelndar to be seen first</para>
		/// </remarks>
		/// <value>Returns wheter gregorian is the default calendar or not</value>
		/// <seealso cref="DateTimePickerEx.OnMouseDown"/>		
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("is Defalut Calendar to be used firstly Gregorian or Ethiopian ?")]
		[DefaultValue(false)]		
		public bool IsGregorianCurrentCalendar
		{	
			get
			{
				return dpp_DayPicker.IsGregorian;
			}
			
			set
			{
				dpp_DayPicker.IsGregorian = value;
			}
		} // public Color CalendarForeColor
			

		
		/// <summary>
		/// Gets the current calendar wheter the calendar is gregorian or ethiopian
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
		public Calendar CurrentCalendar
		{
			get
			{
				return dpp_DayPicker.CurrentCalendar; 
			}		
		}
		
		
		/// <summary>
		/// Gets or sets wheter to allow the time picker to be displayed or not !
		/// </summary>
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Gets or sets wheter to allow the time picker to be displayed or not !")]
		[System.ComponentModel.DefaultValue(false)]
		public bool AllowTimePicker
		{
			get { return b_TimePicker;  }						
			set
			{
				if(b_TimePicker!=value)
				{
					b_TimePicker = value;
					tp_TimePicker.Visible=value;
 										
					base.OnSizeChanged(EventArgs.Empty);				
				}
			}	
		}		
		
		
		#endregion Calendar Support for Control
			
		#region Color-Support for Control
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the Calendar Foreground Color</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This really not needed, built in due to support in Desktop Version</para>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override Standard Foreground Colour</para>
		/// </remarks>
		/// <value>Returns set Standard Foreground Colour</value>
		/// <seealso cref="DateTimePickerEx.OnMouseDown"/>
		[System.ComponentModel.Category("Appearance")]
		[DefaultValue(typeof(Color), "ControlText")]
		[System.ComponentModel.Description("Calendar Foreground Color")]			
		public System.Drawing.Color CalendarForeColor
		{			
			get
			{
				return this.ForeColor;
			}
			
			
			set
			{
				this.ForeColor = value;
			}
		} // public Color CalendarForeColor	

		
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the Calendar Background Color</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This really not needed, built in due to support in Desktop Version</para>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override Standard Background Colour</para>
		/// </remarks>
		/// <value>Returns set Standard Foreground Colour</value>
		/// <seealso cref="DateTimePickerEx.OnMouseDown"/>		
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Calendar Background Color")]
		[DefaultValue(typeof(Color), "Window")]
		public System.Drawing.Color CalendarMonthBackground
		{
			get
			{
				return dpp_DayPicker.CalendarMonthBackground;
			}
			
			
			set
			{
				dpp_DayPicker.CalendarMonthBackground = value;
			}
		} // public Color CalendarMonthBackround
			
				
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the Day-Color (inactiv Month) SolidBrush Portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override SystemColors.GrayText</para>
		/// </remarks>
		/// <value>Returns set System.Drawing.Color used by the Day-Color (inactiv Month)</value>
		/// <seealso cref="DayPickerPopup.sb_ForeColorInactiveDays"/>
		/// <seealso cref="DayPickerPopup.ForeColorInactiveDays"/>
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Day-Color (Inactive Month)")]
		[DefaultValue(typeof(Color), "GrayText")]
		public System.Drawing.Color ForeColorInactiveDays
		{
			get
			{
				return this.dpp_DayPicker.ForeColorInactiveDays;
			}
			
			set
			{
				this.dpp_DayPicker.ForeColorInactiveDays = value;
			}
		} // public SolidBrush ForeColorInactiveDays
			
				
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the Invalid Day-Color (activ Month) SolidBrush Portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override System.Drawing.Color.Red</para>
		/// </remarks>
		/// <value>Returns set System.Drawing.Color used by the Invalid Day-Color (activ Month)</value>
		/// <seealso cref="DayPickerPopup.sb_RedActive"/>
		/// <seealso cref="DayPickerPopup.RedActive"/>
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Invalid Day-Color (activ Month)")]
		[DefaultValue(typeof(Color), "Red")]
		public System.Drawing.Color RedActive
		{
			get
			{
				return this.dpp_DayPicker.RedActive;
			}
			
			set
			{
				this.dpp_DayPicker.RedActive = value;
			}
		} // public SolidBrush RedActive

			
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the Invalid Day-Color (inactiv Month) SolidBrush Portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override System.Drawing.Color.LightSalmon</para>
		/// </remarks>
		/// <value>Returns set System.Drawing.Color used by the Invalid Day-Color (inactiv Month)</value>
		/// <seealso cref="DayPickerPopup.sb_RedInactive"/>
		/// <seealso cref="DayPickerPopup.RedInactive"/>
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Invalid Day-Color (inactiv Month)")]
		[DefaultValue(typeof(Color), "LightSalmon")]
		public System.Drawing.Color RedInactive
		{
			get
			{
				return this.dpp_DayPicker.RedInactive;
			}
			
			set
			{
				this.dpp_DayPicker.RedInactive = value;
			}
		} // public SolidBrush RedInactive


		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the Selected Day-Background SolidBrush Portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override SystemColors.Highlight</para>
		/// </remarks>
		/// <value>Returns set System.Drawing.Color used by the Selected Day-Background</value>
		/// <seealso cref="DayPickerPopup.sb_BackSelect"/>
		/// <seealso cref="DayPickerPopup.BackSelect"/>		
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Selected Day-Background")]
		[DefaultValue(typeof(Color), "Highlight")]
		public System.Drawing.Color BackSelect
		{
			get
			{
				return this.dpp_DayPicker.BackSelect;
			}
			
			set
			{
				this.dpp_DayPicker.BackSelect = value;
			}
		} // public SolidBrush BackSelect


		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the Selected Day-Text SolidBrush Portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override SystemColors.HighlightText</para>
		/// </remarks>
		/// <value>Returns set System.Drawing.Color used by the Selected Day-Text</value>
		/// <seealso cref="DayPickerPopup.sb_TextSelect"/>
		/// <seealso cref="DayPickerPopup.TextSelect"/>
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Selected Day-Text")]
		[DefaultValue(typeof(Color), "HighlightText")]
		public System.Drawing.Color TextSelect
		{
			get
			{
				return this.dpp_DayPicker.TextSelect;
			}
			
			set
			{
				this.dpp_DayPicker.TextSelect = value;
			}
		} // public SolidBrush CalendarTitelForeColor
			
				
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the Caption-Background SolidBrush Portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override SystemColors.ActiveCaption</para>
		/// </remarks>
		/// <value>Returns set System.Drawing.Color used by the Caption-Background</value>
		/// <seealso cref="DayPickerPopup.sb_CaptionBack"/>
		/// <seealso cref="DayPickerPopup.CalendarTitelBackColor"/>
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Caption-Background")]
		[DefaultValue(typeof(Color), "ActiveCaption")]
		public System.Drawing.Color CalendarTitelBackColor
		{
			get
			{
				return this.dpp_DayPicker.CalendarTitelBackColor;
			}
			
			set
			{
				this.dpp_DayPicker.CalendarTitelBackColor = value;
			}
		} // public SolidBrush CalendarTitelBackColor
			
			
				
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the Caption-Text SolidBrush Portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override SystemColors.ActiveCaptionText</para>
		/// </remarks>
		/// <value>Returns set System.Drawing.Color used by the Caption-Text</value>
		/// <seealso cref="DayPickerPopup.sb_CaptionBack"/>
		/// <seealso cref="DayPickerPopup.CalendarTitelForeColor"/>
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Caption-Text")]
		[DefaultValue(typeof(Color), "ActiveCaptionText")]
		public System.Drawing.Color CalendarTitelForeColor
		{
			get
			{
				return this.dpp_DayPicker.CalendarTitelForeColor;
			}
			
			set
			{
				this.dpp_DayPicker.CalendarTitelForeColor = value;
			}
		} // public SolidBrush CalendarTitelForeColor
		//-- Color-Support for Control
			
		#endregion
						
		#region Font-Support for Control
			
		#region Font
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the font used for the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		/// Does the following :
		///  <list type="bullet">
		///   <item>
		///    <description>All Fonts of Control will be set to this Value</description>
		///   </item>
		///  </list>
		///  <list type="bullet">
		///   <item>
		///    <description>Caption</description>
		///    <para>Top Portion of Control with Arrows Month / Year Control</para>
		///   </item>
		///  </list>
		///  <list type="bullet">
		///   <item>
		///    <description>Days</description>
		///    <para>Middle Portion of Control with Days of Week and Days</para>
		///   </item>
		///  </list>
		///  <list type="bullet">
		///   <item>
		///    <description>Today</description>
		///    <para>Bottom Portion of Control with "Today" Test and Date</para>
		///   </item>
		///  </list>
		///  <para>UseFontCaption, FontDay or FontToday to set different Font for each Portion</para>
		/// </remarks>
		/// <value>Standard Font used for all Control Portions</value>
		/// <seealso cref="DayPickerPopup.Font"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
		[System.ComponentModel.DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			
			
			set
			{
				if (value != null) // Needed for Designer support
				{ // update Font
					base.Font             = OnValidFontSize(value);
					this.Invalidate();
				} // if (value != null)
			}
		}  // public Font Font
			
			
		#endregion
			
		#region CalendarFont
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the font used for the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This really not needed, built in due to support in Desktop Version</para>
		/// Does the following :
		///  <list type="bullet">
		///   <item>
		///    <description>All Fonts of Control will be set to this Value</description>
		///   </item>
		///  </list>
		///  <list type="bullet">
		///   <item>
		///    <description>Caption</description>
		///    <para>Top Portion of Control with Arrows Month / Year Control</para>
		///   </item>
		///  </list>
		///  <list type="bullet">
		///   <item>
		///    <description>Days</description>
		///    <para>Middle Portion of Control with Days of Week and Days</para>
		///   </item>
		///  </list>
		///  <list type="bullet">
		///   <item>
		///    <description>Today</description>
		///    <para>Bottom Portion of Control with "Today" Test and Date</para>
		///   </item>
		///  </list>
		///  <para>UseFontCaption, FontDay or FontToday to set different Font for each Portion</para>
		/// </remarks>
		/// <value>Standard Font used for all Control Portions</value>
		/// <seealso cref="DayPickerPopup.Font"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("All Fonts of Control")]
		public Font CalendarFont
		{
			get
			{
				return base.Font;
			}
			
			set
			{
				if (value != null) // Needed for Designer support
				{ // update Font
					base.Font				= OnValidFontSize(value);
					this.dpp_DayPicker.Font = base.Font;
					this.Invalidate();
				} // if (value != null)
			}
		}  // public Font CalendarFont		
		
		
		#endregion		
	
		#region PopUpFontSize
        
		/// <summary>
        /// Gets/Sets the Font Size of pop up window for the currently selected Calendar Font
        /// </summary>
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Gets/Sets the Font Size of pop up window for the currently selected Calendar Font")]
		public float PopUpFontSize
		{
			get
			{
				return this.dpp_DayPicker.Font.Size;
			}

			
			set
			{
				this.dpp_DayPicker.Font = new Font(CalendarFont.Name,value);
				//this.Invalidate();
			}
		}


		#endregion PopUpFontSize

		#region FontCaption
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the font used for the Caption portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Caption</description>
		///    <para>Top Portion of Control with Arrows Month / Year Control</para>
		///   </item>
		///  </list>
		///  <para>Use "dtp_Something.Font=ft_Something;" to set Font's for all Portions of Control</para>
		/// </remarks>
		/// <value>Standard Font used for all Caption Control Portions</value>
		/// <seealso cref="DayPickerPopup.FontCaption"/>
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Font for the Caption portion of the Control")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Font FontCaption
		{
			get
			{
				return this.dpp_DayPicker.FontCaption;
			}
			
			set
			{
				if (value != null) // Needed for Designer support
				{ // update Font
					this.dpp_DayPicker.FontCaption = OnValidFontSize(value);
					this.Invalidate();
				} // if (value != null)
			}
		}  // public Font FontCaption
		
		
		#endregion
			
		#region FontDay
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the font used for the Day portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Days</description>
		///    <para>Middle Portion of Control with Days of Week and Days</para>
		///   </item>
		///  </list>
		///  <para>Use "dtp_Something.Font=ft_Something;" to set Font's for all Portions of Control</para>
		/// </remarks>
		/// <value>Standard Font used for all Day Control Portions</value>
		/// <seealso cref="DayPickerPopup.FontDay"/>
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Font for the Day portion of the Control")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Font FontDay
		{
			get
			{
				return this.dpp_DayPicker.FontDay;
			}
			
			set
			{
				if (value != null) // Needed for Designer support
				{ // update Font
					this.dpp_DayPicker.FontDay = OnValidFontSize(value);
					this.Invalidate();
				} // if (value != null)
			}
		}  // public Font FontDay
			
			
		#endregion
			
		#region FontToday
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the font used for the Today portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Today</description>
		///    <para>Bottom Portion of Control with "Today" Test and Date</para>
		///   </item>
		///  </list>
		///  <para>- Invalidate() is called to repaint the Control</para>
		///  <para>Use "dtp_Something.Font=ft_Something;" to set Font's for all Portions of Control</para>
		/// </remarks>
		/// <value>Standard Font used for all Today Control Portions</value>
		/// <seealso cref="DayPickerPopup.FontToday"/>
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Font for the Today portion of the Control")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Font FontToday
		{
			get
			{
				return this.dpp_DayPicker.FontToday;
			}
			
			set
			{
				if (value != null) // Needed for Designer support
				{ // update Font
					this.dpp_DayPicker.FontToday = OnValidFontSize(value);
					this.Invalidate();
				} // if (value != null)
			}
		}  // public Font FontToday
			
		
		#endregion
			
		// -- Font-Support for Control
		#endregion
		
		#region Minimum/Maximum Date Support
		
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets Minimum DateTime that the DateTimePicker will support.</description><para>mj10777 CultureInfo support</para></item>
		///   <para>Idea from : Éric Carmichael - eric@westgen.com</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>DateTime.MinValue</para>
		///   </item>
		///  </list>
		///  <para>This Property is used to :</para>
		///  <para>- To prevent the User on entering a non-disirable Date in the DateTimePicker</para>
		///  <para>- the DateTimePicker will show Date before MinDate and after MaxDate in shades of Red to show that they cannot be picked</para>
		///  <para>- the Month / Year Controls will not react if the Month/Year to be shown is not a valid selection</para>
		/// </remarks>
		/// <value>return Maximal Date that is Valid</value>
		/// <seealso cref="DateTimePicker.MaxDateTime"/>
		/// <seealso cref="DayPickerPopup.MinDate"/>
		/// <seealso cref="DayPickerPopup.MaxDate"/>
		/// <example>
		///  <code>
		/// 
		/// // Create 3 DateTimePickers
		/// // - 1 to work with
		/// // - 1 to set the Minimun Date
		/// // - 1 to set the Maximum Date
		/// // Set Event to react to Change (assuming of cource that the LinkLabel's has been created)
		/// 
		///public virtual void OnBuildPanelPickers()
		///{
		/// // Create the DateTimePicker to work with
		/// dtp_MainFrame = new mj10777.WinControls.DateTimePicker();
		/// // Create the DateTimePicker to set the Minimal allowed Date
		/// dtp_MinDate = new mj10777.WinControls.DateTimePicker();
		/// // Create the DateTimePicker to set the Maximal allowed Date
		/// dtp_MaxDate = new mj10777.WinControls.DateTimePicker();
		/// // Set the Maximum Date
		/// dtp_MainFrame.MinDateTime = new DateTime(2004,3,02);
		/// // Set the Maximum Date
		/// dtp_MainFrame.MaxDateTime = new DateTime(2004,3,29);
		/// // dtp_MainFrame.MaxDateTime = dt_DT.AddDays(49);
		/// // Set the Minimum Date
		/// dtp_MinDate.Value  = dtp_MainFrame.MinDateTime;
		/// // Set the Maximum Date
		/// dtp_MaxDate.Value  = dtp_MainFrame.MaxDateTime;
		/// // Set the Event to react to Change
		/// linkLabelMinDate.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabelMaxMinDate_LinkClicked);
		/// linkLabelMaxDate.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabelMaxMinDate_LinkClicked);
		///} // public virtual void OnBuildPanelPickers()
		///
		/// // Event to react to a Minimum / Maximum DateTime Change
		/// // - 1 to work with
		///
		///private void linkLabelMaxMinDate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		///{
		/// LinkLabel ll_DTP = (LinkLabel) sender;
		/// if (ll_DTP == linkLabelMaxDate)
		/// {
		///  dtp_MainFrame.MaxDateTime = DateTime.MaxValue;
		///  listEvents.Items.Add("MaxDateTime : "+dtp_MainFrame.MaxDateTime.ToShortDateString());
		///  listEvents.SelectedIndex = listEvents.Items.Count-1;
		/// }
		/// if (ll_DTP == linkLabelMinDate)
		/// {
		///  dtp_MainFrame.MinDateTime = DateTime.MinValue;
		///  listEvents.Items.Add("MinDateTime : "+dtp_MainFrame.MinDateTime.ToShortDateString());
		///  listEvents.SelectedIndex = listEvents.Items.Count-1;
		/// }
		///} // private void linkLabelMaxDate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		///  </code>
		/// </example>
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Minimum DateTime that the Control will support")]
		[DefaultValue(typeof(DateTime), "Aug. 07, 1753")]
		public DateTime MinDateTime
		{
			get
			{
				if(b_TimePicker)
				{
					return this.tp_TimePicker.MinDateTime; 
				}
				else
				{
					return this.dpp_DayPicker.MinDate;
				}
				
			}
			
			
			set
			{
				if ((value >= DateTime.MinValue) && (value < MaxDateTime))
				{
					this.dpp_DayPicker.MinDate		= value;
					this.tp_TimePicker.MinDateTime	= value; 
					
					
					//If current value is less than the minimum datetime that is set
					DateTime dateOnlyValue		=Value.Date;
					DateTime dateOnlyMinDateTime=value.Date;

					if(!(dateOnlyValue < dateOnlyMinDateTime))
					{
						// TODO: Change line below to DateTime.Date
						if(	!b_TimePicker  && 
							Value.Year	== value.Year &&
							Value.Month	== value.Month  &&
							Value.Day	== value.Day )
						{
							EthiopianCalendar calendar=new EthiopianCalendar();
							int curHour;
							int minHour;
							curHour=calendar.GetHour(Value);
							minHour=calendar.GetHour(value);
							if(curHour < minHour)
							{
								Value=value;
							}
							else if(curHour == minHour && Value.Minute < value.Minute)
							{
								Value=value;
							}				
						}
					}
					else
					{
						Value=value;
					}
				}
				else
				{
					this.dpp_DayPicker.MinDate = DateTime.MinValue;
					this.tp_TimePicker.MinDateTime	= value;					
				}
			}
		}  // public DateTime MinDateTime
			
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets Maximum DateTime that the DateTimePicker will support.</description><para>mj10777 CultureInfo support</para></item>
		///   <para>Idea from : Éric Carmichael - eric@westgen.com</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>DateTime.MaxValue</para>
		///   </item>
		///  </list>
		///  <para>This Property is used to :</para>
		///  <para>- To prevent the User on entering a non-disirable Date in the DateTimePicker</para>
		///  <para>- the DateTimePicker will show Date before MinDate and after MaxDate in shades of Red to show that they cannot be picked</para>
		///  <para>- the Month / Year Controls will not reacte if the Month/Year to be shown is not a valid selection</para>
		/// </remarks>
		/// <value>return Maximum Date that is Valid</value>
		/// <seealso cref="DateTimePicker.MinDateTime"/>
		/// <seealso cref="DayPickerPopup.MinDate"/>
		/// <seealso cref="DayPickerPopup.MaxDate"/>
		/// <example>
		///  <code>
		/// 
		/// // Create 3 DateTimePickers
		/// // - 1 to work with
		/// // - 1 to set the Minimun Date
		/// // - 1 to set the Maximum Date
		/// // Set Event to react to Change (assuming of cource that the LinkLabel's has been created)
		/// 
		///public virtual void OnBuildPanelPickers()
		///{
		/// // Create the DateTimePicker to work with
		/// dtp_MainFrame = new mj10777.WinControls.DateTimePicker();
		/// // Create the DateTimePicker to set the Minimal allowed Date
		/// dtp_MinDate = new mj10777.WinControls.DateTimePicker();
		/// // Create the DateTimePicker to set the Maximal allowed Date
		/// dtp_MaxDate = new mj10777.WinControls.DateTimePicker();
		/// // Set the Maximum Date
		/// dtp_MainFrame.MinDateTime = new DateTime(2004,3,02);
		/// // Set the Maximum Date
		/// dtp_MainFrame.MaxDateTime = new DateTime(2004,3,29);
		/// // dtp_MainFrame.MaxDateTime = dt_DT.AddDays(49);
		/// // Set the Minimum Date
		/// dtp_MinDate.Value  = dtp_MainFrame.MinDateTime;
		/// // Set the Maximum Date
		/// dtp_MaxDate.Value  = dtp_MainFrame.MaxDateTime;
		/// // Set the Event to react to Change
		/// linkLabelMinDate.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabelMaxMinDate_LinkClicked);
		/// linkLabelMaxDate.LinkClicked += new LinkLabelLinkClickedEventHandler(linkLabelMaxMinDate_LinkClicked);
		///} // public virtual void OnBuildPanelPickers()
		///
		/// // Event to reacte to a Minimum / Maximum DateTime Change
		/// // - 1 to work with
		///
		///private void linkLabelMaxMinDate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		///{
		/// LinkLabel ll_DTP = (LinkLabel) sender;
		/// if (ll_DTP == linkLabelMaxDate)
		/// {
		///  dtp_MainFrame.MaxDateTime = DateTime.MaxValue;
		///  listEvents.Items.Add("MaxDateTime : "+dtp_MainFrame.MaxDateTime.ToShortDateString());
		///  listEvents.SelectedIndex = listEvents.Items.Count-1;
		/// }
		/// if (ll_DTP == linkLabelMinDate)
		/// {
		///  dtp_MainFrame.MinDateTime = DateTime.MinValue;
		///  listEvents.Items.Add("MinDateTime : "+dtp_MainFrame.MinDateTime.ToShortDateString());
		///  listEvents.SelectedIndex = listEvents.Items.Count-1;
		/// }
		///} // private void linkLabelMaxDate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		///  </code>
		/// </example>				
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Maximum DateTime that the Control will support")]
		[DefaultValue(typeof(DateTime), "Aug. 07, 9997")]
		public DateTime MaxDateTime
		{
			get
			{
				if(b_TimePicker)
				{
					return this.tp_TimePicker.MaxDateTime ; 
				}
				else
				{
					return this.dpp_DayPicker.MaxDate;
				}

				
			}

			
			set
			{
				if ((value <= DateTime.MaxValue) && (value > MinDateTime))
				{
					this.dpp_DayPicker.MaxDate		= value;
					this.tp_TimePicker.MaxDateTime	= value;
					
					//If current vale is less than the minimum datetime that is set
					DateTime dateOnlyValue		= Value.Date;
					DateTime dateOnlyMaxDateTime= value.Date;

					if(!(dateOnlyValue > dateOnlyMaxDateTime))
					{
						if(	!b_TimePicker  && 
							Value.Year	== value.Year &&
							Value.Month	== value.Month  &&
							Value.Day	== value.Day )
						{
							EthiopianCalendar calendar=new EthiopianCalendar();
							int curHour;
							int maxHour;
							curHour=calendar.GetHour(Value);
							maxHour=calendar.GetHour(value);
							if(curHour > maxHour)
							{
								Value=value;
							}
							else if(curHour == maxHour && Value.Minute > value.Minute)
							{
								Value=value;
							}				
						}
					}
					else
					{
						Value=value;
					}				

					//if (this.Value > value)
					//	this.Value = value;
				}
				else
				{
					this.dpp_DayPicker.MaxDate		= DateTime.MaxValue;
					this.tp_TimePicker.MaxDateTime	= value;
				}
			}
		} // public DateTime MaxDateTime

		
		#endregion Minimun/Maximum Date Support
		
		#region Today String Representation

		/// <summary>
		///  <list type="table">
		///   <item><description>Get or Set "Today" Text used by Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override Value set in Culture</para>
		/// </remarks>
		/// <value>Returns set Today Text used by the DateTimePicker</value>
		/// <seealso cref="DayPickerPopup.s_Today"/>
		[System.ComponentModel.Browsable(false)]
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("'Today' Text used by Control")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string TodayText
		{
			get
			{
				return this.dpp_DayPicker.s_Today;
			}
			
			
			set
			{
				this.dpp_DayPicker.s_Today = value;
			}
		}  // public string Today
		
		
		#endregion Today String Representation
		
		#region FirstDayofWeek
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the FirstDayOfWeek (allows overriding Standard Value)</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override FirstDayOfWeek set by CultureInfo</para>
		/// </remarks>
		/// <value>Returns set FirstDayOfWeek used by the DateTimePicker</value>
		/// <seealso cref="DayPickerPopup.FirstDayOfWeek"/>
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("First Day of Week (0-6 ; Sunday to Saturday)")]
		[DefaultValue(typeof(System.DayOfWeek), "Sunday")]
		public System.DayOfWeek FirstDayOfWeek
		{
			get
			{
				return dpp_DayPicker.FirstDayOfWeek;
			}
			
			
			set
			{
				// update FirstDayOfWeek
				dpp_DayPicker.FirstDayOfWeek = value;
				this.Invalidate();
			}
		}  // public int FirstDayOfWeek
		
		
		#endregion FirstDayofWeek
		
		#region DayOfWeekCharacters
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the amount of Characters shown for the Weekdays (allows overriding Standard Value)</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override the Default amount of 3 characters, such as Mon, Tue</para>
		/// </remarks>
		/// <value>amount of Characters shown for the Weekdays</value>
		/// <seealso cref="DayPickerPopup.DayOfWeekCharacters"/>
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Characters shown for the Weekdays")]
		[DefaultValue(3)]
		public int DayOfWeekCharacters
		{
			get
			{
				return dpp_DayPicker.DayOfWeekCharacters;
			}
			
			
			set
			{
				// update DayOfWeekCharacters
				dpp_DayPicker.DayOfWeekCharacters = value;
				s_DayOfWeek = this.dpp_DayPicker.s_DayOfWeek;
				this.Invalidate();
			}
		}  // public int DayOfWeekCharacters
		
		
		#endregion DayOfWeekCharacters
		
		#region CustomFormat
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the Custom Format of the date displayed in the Control.</description><para>mj10777 CultureInfo support</para></item>
		///   <para>Idea from : Éric Carmichael - eric@westgen.com</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>yyyy-MM-dd</para>
		///    <para>yyyy-MM-ddTHH:mm:sszzzzzz (2004-03-29T00:00:00++02:00)</para>
		///    <para> - so that my strong style dataset - validated by an XML schema will recognioze the date and time.</para>7s
		///   </item>
		///  </list>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override the string used when DateTimePickerFormat.Custom is active</para>
		/// </remarks>
		/// <value>CustomFormat used in the DateTimePicker</value>
		/// <seealso cref="DayPickerPopup.CustomFormat"/>
		///  dtp_MainFrame.CustomFormat = textBoxCustom.Text;
		/// <example>
		///  <code>
		/// 
		/// // Changing the Default CustomFormat (yyyy-MM-dd) to something else (like yyyy-MMM-dd)
		/// 
		///private void linkLabelCustom_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		///{
		/// dtp_MainFrame.CustomFormat = textBoxCustom.Text; // Something like "yyyy-MMM-dd"
		/// dtp_MainFrame.Format       = mj10777.WinControls.DateTimePickerFormat.Custom;
		/// listEvents.Items.Add("Custom : "+dtp_MainFrame.CustomFormat);
		/// listEvents.SelectedIndex = listEvents.Items.Count-1;
		///} // private void linkLabelCustom_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		///  </code>
		/// </example>
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Custom Date Format (non-Standard display of Dates)")]
		[DefaultValue("MMMM dd, yyyy")]
		public string CustomFormat
		{
		
			get
			{
				return this.dpp_DayPicker.CustomFormat;
			}
			

			set
			{
				this.dpp_DayPicker.CustomFormat = value;
				m_fieldsCollection.BuildDateTimeFields(value);				
				Invalidate();				
			}
		}			
			
		
		#endregion CustomFormat

		#region Squibble

		/// <summary>
		///  <list type="table">
		///   <item><description>Specifies the Type to use to show the "Today" Date in the Control</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override the Default SquibbleType.Squibble for the Today Date selection</para>
		/// </remarks>
		/// <value>SquibbleType use for the Today Date selection</value>
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Format of the 'Today' Date selection in the Control")]
		[DefaultValue(typeof(SquibbleType), "Rectangle")]
		public SquibbleType Squibble
		{
			get
			{
				return this.dpp_DayPicker.sqt_Squibble;
			}
			
			
			set
			{
				if ((value == SquibbleType.Squibble) ||
					(value == SquibbleType.Rectangle) ||
					(value == SquibbleType.Ellipse))
				{
					this.dpp_DayPicker.sqt_Squibble = value;
				}
				else
				{
					this.dpp_DayPicker.sqt_Squibble = SquibbleType.Ellipse;
				}
			}
		}

		
		#endregion Squibble

		#region Format

		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the format of the date displayed in the Control.</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override the Default DateTimePickerFormat for the Label/TextBox Control  containing the DateTimePicker</para>
		/// </remarks>
		/// <value>DateTimePickerFormat use for Lable/TextBox containing the DateTimePicker</value>
		/// <seealso cref="DateTimePickerEx.dtpf_Format"/>
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("Format of the date displayed in the Control")]
		[DefaultValue(typeof(DateTimePickerFormat), "Custom")]
		public DateTimePickerFormat Format
		{
			get
			{
				return dtpf_Format;
			}
		
			
			set
			{
				// update format and repaint
				dtpf_Format = value;
				this.dpp_DayPicker.dtpf_Format = value;
				switch(dtpf_Format)
				{
					case DateTimePickerFormat.Short:
						m_fieldsCollection.BuildDateTimeFields("d");
						break;
					case DateTimePickerFormat.Long:
						m_fieldsCollection.BuildDateTimeFields("D");
						break;
					case DateTimePickerFormat.Time:
						m_fieldsCollection.BuildDateTimeFields("t");
						break;
					case DateTimePickerFormat.Custom:
						m_fieldsCollection.BuildDateTimeFields(CustomFormat);  
						break;
				}				
				Invalidate();				
			}
		} // public DateTimePickerFormat Format

		
		#endregion Format

		#region Value
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the date value assigned to the Control.</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override the Default DateTime Value</para>
		/// </remarks>
		/// <value>DateTimePickerFormat use for Lable/TextBox containing the DateTimePicker</value>
		/// <seealso cref="DayPickerPopup.Value"/>
		[System.ComponentModel.Category("Appearance")]
		[System.ComponentModel.Description("DateTime Value to use")]		
		public DateTime Value
		{			
			// setting the picker value raises the ValueChanged
			// event which causes the control to repaint
			get
			{ 
				if(b_TimePicker)
				{
					return tp_TimePicker.Value; 
				}
				else
				{
					return dpp_DayPicker.Value.Date;
				}
			}
			
			
			set
			{
				dpp_DayPicker.Value = value.Date;
				tp_TimePicker.Value	= value; 
			}			
		} // public DateTime Value

		
		#endregion Value

		#region ShouldSerializeText, ShouldSerializeValue, ResetValue, ShouldSerializeCalendarFont, ResetCalendarFont
		
		/// <summary>
		/// Gets a value indicating whether the value of the "Text" property should be 
		/// serialized
		/// </summary>
		/// <returns></returns>
		public bool ShouldSerializeText()
		{
			return ShouldSerializeValue();
		}

		
		/// <summary>
		/// Gets a value indicating whether the value of the "Value" property should be 
		/// serialized
		/// </summary>
		/// <returns></returns>
		public bool ShouldSerializeValue()
		{
			if (b_TimePicker) 
				return ((Value.Year != DateTime.Now.Year) ||  
						(Value.Month != DateTime.Now.Month) || 
						(Value.Day != DateTime.Now.Day) || 
						(Value.Hour != DateTime.Now.Hour) || 
						(Value.Minute != DateTime.Now.Minute));

			return (Value != DateTime.Today) ;
		}

		
		/// <summary>
		/// Resets the value of the "Value" property to its default value
		/// </summary>
		public void ResetValue()
		{
			if(b_TimePicker)
			{
				Value = new DateTime(DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day,
									 DateTime.Now.Hour,DateTime.Now.Minute,0);        
			}
			else
			{
				Value = DateTime.Today ;
			}
		}

		
		/// <summary>
		/// Gets a value indicating whether the value of the "CalendarFont" property should be 
		/// serialized
		/// </summary>
		/// <returns></returns>
		public bool ShouldSerializeCalendarFont()
		{
			return CalendarFont.Name.CompareTo("Ethiopia Jiret")!=0;
		}


		/// <summary>
		/// Resets the value of the "CalendarFont" property to its default value
		/// </summary>
		public void ResetCalendarFont()
		{
			this.CalendarFont=OnValidFontSize(new Font("Ethiopia Jiret",10F));		
		}
		

		/// <summary>
		/// Gets a value indicating whether the value of the "PopUpFontSize" property should be 
		/// serialized
		/// </summary>
		/// <returns></returns>
		public bool ShouldSerializePopUpFontSize()
		{
			return this.PopUpFontSize!=base.Font.Size;		
		}

		
		/// <summary>
		/// Resets the value of the "PopUpFontSize" property to its default value
		/// </summary>
		public void ResetPopUpFontSize()
		{		
			this.PopUpFontSize=base.Font.Size;		
		}		
		
		
		#endregion ShouldSerializeText, ShouldSerializeValue, ResetValue, ShouldSerializeCalendarFont, ResetCalendarFont

		#region Text

		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the date value assigned to the Control.</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override the Default DateTime Value using a Test-String</para>
		///  <para>---> An invalid String will return result in the DateTimePickerFormat.Short Format</para>
		///  <para>- Allow User to recieve a Text String of DateTime Value</para>
		/// </remarks>
		/// <value>Returns formatted DateTime value according to set DateTimePickerFormat</value>
		/// <seealso cref="DayPickerPopup.Value"/>
		[System.ComponentModel.Browsable(false)]
		[DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
		public override String Text
		{
			// Insure Date is Formatted to selected CultureInfo
			get
			{
				return GetText(Value);		//Get the Text for the Value of the DateTime Picker
			}

			
			set
			{
				//Calendar Change has no effect we always think in DateTime Values
				try // Needed for Designer support
				{
					// update the datetime value
					this.Value = DateTime.Parse(value);
				}
				catch
				{

				}
			}
		} // public override String Text
			
			

		/// <summary>
		/// Gets the correct Text for the DateTime Passed
		/// Using the current Format and custom format string
		/// </summary>
		/// <param name="dateTime">The DateTime for which the text is going to be retrived</param>
		/// <returns>The string showing the DateTime</returns>
		protected string GetText(DateTime dateTime)
		{
			// return date as string in the correct format
			string m_Text = "";

			switch (dtpf_Format)
			{
				case DateTimePickerFormat.Short:
				{
					m_Text = IsGregorianCurrentCalendar ? dateTime.ToString("d"): EthiopianCalendar.ToString(dateTime,"d");
				}break;
				
				case DateTimePickerFormat.Long:
				{
					m_Text = IsGregorianCurrentCalendar ? dateTime.ToString("D"): EthiopianCalendar.ToString(dateTime,"D");
				}break;
				
				case DateTimePickerFormat.Time:
				{
					m_Text	=IsGregorianCurrentCalendar ? dateTime.ToString("t"): EthiopianCalendar.ToString(dateTime,"t");  
					
				}break;

				case DateTimePickerFormat.Custom :
					
					try
					{
						m_Text = IsGregorianCurrentCalendar ? dateTime.ToString(CustomFormat): EthiopianCalendar.ToString(dateTime,CustomFormat);
					}
					catch(System.Exception)
					{
						//never happens
						m_Text = dateTime.ToShortDateString();
					}break;
			}  // switch (dtpf_Format)
			return m_Text;
		}


		#endregion Text	

		#region Overrides

		#region OnGotFocus

		/// <summary>
		/// When the control got focus
		/// </summary>
		/// <param name="e"></param>
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus (e);
			if(m_fieldsCollection != null)
			{
				m_fieldsCollection.OnGotFocus();
			}
		}

		
		#endregion OnGotFocus
		
		#region OnLostFocus

		/// <summary>
		/// When the control loses focus
		/// </summary>
		/// <param name="e">Event Args</param>
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus (e);

			if(m_fieldsCollection != null)
			{
				m_fieldsCollection.OnLostFocus();
			}

			if(b_MouseDown)
			{
				b_MouseDown=false;
				Invalidate(rt_DropArrow); 
			}
		}

		
		#endregion OnLostFocus
			
		#region OnPaint
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Paint formated DateTime to Control</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Additions made by leyu sisay on the way the combo box is drawn and giving similar look like the default datetimepicker</para>
		/// </remarks>
		/// <seealso cref="DateTimePickerEx.CreateMemoryBitmap"/>
		/// <seealso cref="DateTimePickerEx.CreateGdiObjects"/>
		/// <seealso cref="DateTimePickerEx.Text"/>
		/// <seealso cref="DateTimePickerEx.sb_Foreground"/>
		/// <seealso cref="DateTimePickerEx.sb_Disabled"/>
		/// <seealso cref="DateTimePickerEx.sb_Frame"/>
		/// <seealso cref="DateTimePickerEx.pta_ArrowPoints"/>
		/// <param name="e">PaintEventArgs sent (used for Graphics.DrawImage)</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			try
			{
				// Draw to memory bitmap
				CreateMemoryBitmap(e.Graphics);
				CreateGdiObjects();
			
				// Init background
				gr_Graphics.Clear(this.Enabled ? SystemColors.Window : SystemColors.Control);			
			
				//Text Area to be written 
				Rectangle rt_Text = new Rectangle(2, 2 , bmp_Bmp.Width - 4,bmp_Bmp.Height-4);

				//Resize the area text to be written (Excluding the area for the drop down button)
				rt_Text.Width -=Const.arrowWidth; 

				//The Displayed DateTime depends wheter the DateTime Picker is being edited or not
				DateTime displayDT = m_isEditing ? m_editedDateTime : this.Value;
						
				// The area that will be consumed by the string
				Size size = gr_Graphics.MeasureString(GetText(displayDT),this.Font).ToSize();
			
				// Check if Text fit int Text-Control-Area
				Font tmp_Font = this.Font;
				while (size.Height < rt_Text.Height)
				{
					tmp_Font = new System.Drawing.Font(this.Font.Name,tmp_Font.Size+1,this.Font.Style);
					size = gr_Graphics.MeasureString(GetText(displayDT),tmp_Font).ToSize();
				}
				while (size.Height > rt_Text.Height)
				{
					tmp_Font = new System.Drawing.Font(this.Font.Name,tmp_Font.Size-1,this.Font.Style);
					size = gr_Graphics.MeasureString(GetText(displayDT),tmp_Font).ToSize();
				}
				while (size.Width > rt_Text.Width)
				{
					tmp_Font = new System.Drawing.Font(this.Font.Name,tmp_Font.Size-1,this.Font.Style);
					size = gr_Graphics.MeasureString(GetText(displayDT),tmp_Font).ToSize();
				}
				if (base.Font.Size != tmp_Font.Size)
				{
					base.Font = tmp_Font;
				}				

				size.Width +=3;
				size.Height+=3; 
				
				RectangleF layOutRect	=new RectangleF(new PointF(4F,((bmp_Bmp.Height-size.Height)/2)+1),size);
								
				m_fieldsCollection.UpDateLocations(	displayDT,this.IsGregorianCurrentCalendar,
						this.Font,gr_Graphics,layOutRect);

				gr_Graphics.DrawString(	GetText(displayDT),
					this.Font,
					this.Enabled ? sb_Foreground : sb_Disabled,
					layOutRect);
				
				if(this.Focused && m_fieldsCollection.SelectedField !=null && !dpp_DayPicker.Visible )
				{
					gr_Graphics.FillRectangle(SystemBrushes.ActiveCaption , Rectangle.Round(m_fieldsCollection.SelectedField.ClientRect));
					
					System.Drawing.Region clipRegion = new System.Drawing.Region(m_fieldsCollection.SelectedField.ClientRect);
					gr_Graphics.Clip = clipRegion; 
					gr_Graphics.DrawString(	GetText(displayDT),
						this.Font,
						SystemBrushes.ActiveCaptionText,
						layOutRect);
					clipRegion.MakeInfinite();
					gr_Graphics.Clip =  clipRegion; 
				}

				//Now get the area the combo drop down is drawn
				rt_DropArrow=new Rectangle  (bmp_Bmp.Width -Const.arrowWidth,
					1,
					Const.arrowWidth-1,
					bmp_Bmp.Height-2);			
			
				RectangleF arrowRect=rt_DropArrow;
			
				//We need some white border around it so decrease the size of button
				arrowRect.Inflate(-1,-2.5F);
				arrowRect.Width =arrowRect.Width -1.5F;				

				//Now draw the combo button but inorder not to lose the edges inflate the rect			
				PointF left   =new PointF(arrowRect.X + arrowRect.Width/4  ,arrowRect.Height/2 + 1.5F);
				PointF right  =new PointF(arrowRect.X + arrowRect.Width*3/4  ,left.Y);
				PointF bottom =new PointF(left.X + (right.X - left.X)/2,left.Y + (right.X - left.X)/2);				
				
				if(this.Enabled)
				{		
					ThemedDrawing.DrawButton(gr_Graphics,arrowRect,b_MouseDown,new PointF[]{left,bottom,right});
				}
				else
				{
					gr_Graphics.FillRectangle(SystemBrushes.Window,rt_DropArrow);    
					gr_Graphics.FillRectangle(SystemBrushes.Control,arrowRect);
					gr_Graphics.DrawLines	 (SystemPens.GrayText,new PointF[]{left,bottom,right});					
				}

				ThemedDrawing.DrawBorder(gr_Graphics,new RectangleF(0,0,bmp_Bmp.Width-1 ,bmp_Bmp.Height-1));
								
				// Blit memory bitmap to screen
				e.Graphics.DrawImage(bmp_Bmp, 0, 0);
			}
			catch(Exception ex)
			{
				System.Diagnostics.Trace.WriteLine("DateTimePickerEx::OnPaint() "+ex.GetType()+" "+ex.Message);   
			}			
		}
			
		
		#endregion OnPaint
			
		#region OnPaintBackground
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Paint Background</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Avoid flickering</para>
		/// </remarks>
		/// <param name="e">PaintEventArgs sent (not used)</param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			// don't pass to base since we paint everything, avoid flashing
		} // protected override void OnPaintBackground(PaintEventArgs e)
			
		
		#endregion OnPaintBackground

		#region OnKeyDown

		/// <summary>
		/// Processs the key down event
		/// </summary>
		/// <param name="e">The Key Event Args</param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown (e);
			
			if(e.KeyData == Keys.Escape || e.KeyData == Keys.Enter )
			{
				if (!dpp_DayPicker.nud_YearUpDown.Visible)
					this.ClosePopUp();
			}
			else if((e.KeyData == Keys.F4 || (e.KeyCode == Keys.Down && e.Alt) )&& !dpp_DayPicker.Visible )
			{
				MouseEventArgs mouseEvent=new MouseEventArgs(MouseButtons.Left,1,rt_DropArrow.X+2,rt_DropArrow.Y+2,0);    
				OnMouseDown(mouseEvent); 
			}			
			else if(this.Focused && !dpp_DayPicker.Visible)
			{
				if(m_fieldsCollection != null)
				{
					m_fieldsCollection.OnKeyDown(e); 
				}
			}
		}

		
		#endregion OnKeyDown
			
		#region OnMouseDown
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Show or hide the day picker popup control.</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Determine the best location to display the day picker.</para>
		/// </remarks>
		/// <seealso cref="DateTimePickerEx.dpp_DayPicker"/>
		/// <seealso cref="DayPickerPopup.Display"/>
		/// <param name="e">EventArgs sent (not used)</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if(e.Button == MouseButtons.Left)
			{
				if(this.CanFocus  && !this.Focused)
				{
					this.Focus();
				}
				
				// add day picker control to the toplevel window
				// this allows the control to display on top of
				// tabs and other controls
				if (dpp_DayPicker.Parent == null)
				{
					this.TopLevelControl.Controls.Add(dpp_DayPicker);
				}// if (dpp_DayPicker.Parent == null)
			
				if (!b_LostFocusEvent)
				{
					SubscribeEventToChild(this.TopLevelControl,this);				
					b_LostFocusEvent = true;
				}   // if (!b_LostFocusEvent)
			
				// intelligently calculate where the day picker should be displayed,
				// try to display below the label control, display above the control
				// if there is not enough room
				Point pos = new Point(this.Left, this.Bottom+1);
				// map points to top level window
				Point parentPos		= this.Parent.PointToScreen(this.Parent.Location);
				Point topParentPos	= this.TopLevelControl.PointToScreen(this.Parent.Location);
				pos.Offset(parentPos.X - topParentPos.X, parentPos.Y - topParentPos.Y);			
							
				if ((pos.Y + dpp_DayPicker.Size.Height) > this.TopLevelControl.ClientRectangle.Height)
				{
					// there is not enough room, try displaying above the label
					pos.Y -= (this.Height + dpp_DayPicker.Size.Height+2);
					if (pos.Y < 0)
					{
						// there was not enough room, display at bottom of screen
						pos.Y = (this.TopLevelControl.ClientRectangle.Height-dpp_DayPicker.Size.Height);
					}
				}

				// try displaying aligned with the label control
				if ((pos.X + dpp_DayPicker.Size.Width) > this.TopLevelControl.ClientRectangle.Width)
					pos.X = (this.TopLevelControl.ClientRectangle.Width-dpp_DayPicker.Size.Width);
			
	
				//If we clicked on the Drop Down Arrow Only
				if(rt_DropArrow.Contains(e.X,e.Y))
				{
					b_MouseDown=true; 
					//Show/Hide the pop-up control
					// display or hide the day picker control
					dpp_DayPicker.Display(!dpp_DayPicker.Visible,pos.X,pos.Y,this.BackColor,this.ForeColor);
					
					if(m_fieldsCollection.SelectedField !=null)
					{
						if(m_isEditing)
						{
							m_fieldsCollection.EndEditing();
						}
						Invalidate();
						//RectangleF invalRect=  m_fieldsCollection.SelectedField.ClientRect;
						//invalRect .Inflate(2,2);
						//Invalidate(Rectangle.Round(invalRect));					
					}					
				}
				else
				{
					//If we clicked on the other part of the control
					//If the dpp_DayPicker was visible hide the pop-up and do nothing else
					if(dpp_DayPicker.Visible)
					{
						dpp_DayPicker.Display(false,pos.X,pos.Y,this.BackColor,this.ForeColor);
					}
					else	//The pop-up was not visible then call the fields collectios OnMouseDown method
					{
						if(m_fieldsCollection !=null)
						{
							m_fieldsCollection.OnMouseDown(e); 
						}
					}
				}

				// raise the DropDown or CloseUp event
				if (dpp_DayPicker.Visible && this.DropDown != null)
					this.DropDown(this,EventArgs.Empty);
				if (!dpp_DayPicker.Visible && this.CloseUp != null)
					this.CloseUp(this,EventArgs.Empty);
				
			}
		} // protected override void OnMouseDown(MouseEventArgs e)
			
		
		#endregion OnMouseDown

		#region OnMouseUp

		/// <summary>
		/// Show a different Drawing for the Combo Button when the mouse button is up
		/// </summary>
		/// <param name="e">MouseEventArgs</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp (e);
			if(b_MouseDown)
			{
				b_MouseDown		= false;	//We dont want the button to show up as being pressed
				Invalidate(rt_DropArrow);	//Only invalidate on the rect area
			}
		}


		#endregion OnMouseUp
			
		#region OnResize
		
		/// <summary>
		///  <list type="table">
		///   <item><description>OnResize</description></item>
		///   <para>Idea from : Jim Wilson, JW Hedgehog Inc - 10.2003 - Adding Designer Support</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>
		///   When the control is resized doesn't automatically repaint the whole control. 
		///   The problem is easily corrected by invalidating the control when it is resized.
		///  </para>
		/// </remarks>
		protected override void OnResize(EventArgs e)
		{	
			base.OnResize(e);
						
			if(b_TimePicker)
			{
				//location of time picker should be centered
				int tpHeight=tp_TimePicker.Height  < this.Height ? (this.Height - tp_TimePicker.Height)/2 : 0;  
				tp_TimePicker.Location=new System.Drawing.Point(this.Width - tp_TimePicker.Width ,tpHeight);
			}
			
			this.Invalidate();
		}
			
			
		#endregion OnResize

		#region SetBoundsCore

		/// <summary>
		/// Performs the work of setting the specified bounds of this control
		/// </summary>
		/// <param name="x">The new Left property value of the control.</param>
		/// <param name="y">The new Right property value of the control</param>
		/// <param name="width">The new Width property value of the control</param>
		/// <param name="height">The new Height property value of the control</param>
		/// <param name="specified">A bitwise combination of the BoundsSpecified values</param>
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			//Used to avoid the bug in the designer when the datetimepicker width
			//is minimized
			if(b_TimePicker)
			{
				base.SetBoundsCore(	x,y,width	> tp_TimePicker.Width + tp_TimePicker.Width / 2 ? 
										width  :  tp_TimePicker.Width + tp_TimePicker.Width / 2,
					height > tp_TimePicker.Height  ? height : tp_TimePicker.Height, specified);
			}
			else
			{
				base.SetBoundsCore(	x,y,width	> tp_TimePicker.Width/2  ? width  :	 tp_TimePicker.Width/2,
					height > 15 ? height : 15, specified);
			}
		}

	
		#endregion SetBoundsCore

		#region ProcessCmdKey

		/// <summary>
		/// Processes the keys entered like the arrow keys
		/// </summary>
		/// <param name="msg">The message with the key event</param>
		/// <param name="keyData">The Key Data that which key caused the event</param>
		/// <returns></returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			//If the message is key down and either the left,right,up or down button is pressed
			if(msg.Msg == Win32.WM_KEYDOWN && (keyData == Keys.Left || keyData == Keys.Right || keyData ==Keys.Up || keyData == Keys.Down))
			{
				if(dpp_DayPicker.Visible)
				{
					dpp_DayPicker.RaiseKeyDown( new KeyEventArgs(keyData));
				}
				else
				{
					OnKeyDown(new KeyEventArgs(keyData));
				}
				return true;
			}
  
			return base.ProcessCmdKey (ref msg, keyData);
		}

		
		#endregion ProcessCmdKey
		
		#endregion Overrides
	
		#region Event Handler

		#region OnDayPickerCloseUp
			
		/// <summary>
		///  <list type="table">
		///   <item><description>CloseUp event from the day picker control</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <param name="sender">Control where we are running</param>
		/// <param name="e">EventArgs sent (not used)</param>
		private void OnDayPickerCloseUp(object sender, System.EventArgs e)
		{
			// pass to our container
			if (this.CloseUp != null)
			{
				this.CloseUp(this, EventArgs.Empty);
				if (b_LostFocusEvent)
				{
					// this.Parent.MouseDown -= new MouseEventHandler(OnDayPicker_LostFocus);
					for (int i=0;i<this.Parent.Controls.Count;i++)
					{ // Remove Event from the Chidren
						// this.Parent.Controls[i].MouseDown -= new MouseEventHandler(this.OnDayPicker_LostFocus);
						// With the Desktop.DateTimePicker this did not work, therefore ...
						// for (int j=0;j<this.Parent.Controls[i].Controls.Count;j++)
						// { // Remove Event from the Grand-Chidren - this should take care of everything!
						//  this.Parent.Controls[i].Controls[j].MouseDown -= new MouseEventHandler(this.OnDayPicker_LostFocus);
						// } // for (int j=0;j<this.Parent.Controls[i].Controls.Count;j++)
					}  // for (int i=0;i<this.Parent.Controls.Count;i++)
					b_LostFocusEvent = false;
				}
			}
		} // private void OnDayPickerCloseUp(object sender, System.EventArgs e)
			
			
		#endregion OnDayPickerCloseUp
			
		#region OnDayPickerValueChanged
			
		/// <summary>
		///  <list type="table">
		///   <item><description>ValueChanged event from the day picker control</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <param name="sender">Control where we are running</param>
		/// <param name="e">EventArgs sent (not used)</param>
		private void OnDayPickerValueChanged(object sender, System.EventArgs e)
		{
//			Trace.WriteLine("Recieved Event from "+sender.ToString() + (b_TimePicker ? " not trasmitting....":" transmitting..." ) );
			if(b_TimePicker)	//We are in the time picker mode
			{
				//We assign the new value the datepicker has to the time picker
				DateTime dateOnly=dpp_DayPicker.Value;
				DateTime timeOnly=tp_TimePicker.Value;

				//Assign the new DateTime value to the time picker
				tp_TimePicker.Value=new DateTime(dateOnly.Year,
					dateOnly.Month,
					dateOnly.Day,
					timeOnly.Hour,
					timeOnly.Minute,
					timeOnly.Second,
					timeOnly.Millisecond);
			
				// Repaint to display the new value
				Invalidate();					//See how this.Value works
				
				//Event not broad casted from here....
			}
			else			//We are in the date picker mode
			{
				// Repaint to display the new value
				Invalidate();
				// pass along to our container
				if (this.ValueChanged != null)
					this.ValueChanged(this, e);
			}
		}
			
			
		#endregion OnDayPickerValueChanged
		
		#region OnTimePicker_ValueChanged

		/// <summary>
		/// When the value in the time picker is changed
		/// </summary>
		private void OnTimePickerValueChanged(object sender, EventArgs e)
		{
	//		Trace.WriteLine("Recieved Event from "+sender.ToString() + (b_TimePicker ? " trasmitting...":" not transmitting..." ) );
			
			//Only if we are in the time picker mode
			if(b_TimePicker)
			{
				//Redraw the scene
				Invalidate();
			
				//Broadcast the event to subscribers
				if(this.ValueChanged != null)
				{
					this.ValueChanged(this,e); 
				}
			}
		}

		
		#endregion OnTimePicker_ValueChanged

		#region OnDayPicker_LostFocus

		/// <summary>
		/// Used to lose focus of the date picker when any control got focus
		/// </summary>
		private void parentControl_GotFocus(object sender, EventArgs e)
		{
			if (!dpp_DayPicker.nud_YearUpDown.Visible)
				this.ClosePopUp();
		}

			
		/// <summary>
		///  <list type="table">
		///   <item><description>Lost Focus Event outside DateTimePicker</description></item>
		///   <para>Trying to solve LostFocus problem</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Create Rectangle with Position of DateTimePicker</para>
		///  <para>Check if Mouse was clicked outside of this Position</para>
		///  <para>- if it is : Close PopUpPicker if visable</para>
		///  <para>Problem : Controls on this Panel do not reacte to this event</para>
		/// </remarks>
		/// <param name="sender">Control on which we are running</param>
		/// <param name="e">MouseEventArgs sent</param>
		/// <seealso cref="DateTimePickerEx.OnMouseDown"/>

		private void OnDayPicker_LostFocus(object sender,MouseEventArgs e) 
		{
			Rectangle rt_Position = new Rectangle(this.Location.X,this.Location.Y,this.Size.Width,this.Size.Height);
			if (!rt_Position.Contains(e.X,e.Y)) // Outside Control
			{
				if (!dpp_DayPicker.nud_YearUpDown.Visible)
					this.ClosePopUp();
			}
		}		
		
		
		#endregion OnDayPicker_LostFocus

		#region OnFieldCollection_FieldChaged

		/// <summary>
		/// When the fields Changed event is fired we need to repaint the scene man
		/// </summary>
		private void m_fieldsCollection_FieldChanged(object sender, EventArgs e)
		{
			Invalidate();
		}


		#endregion OnFieldCollection_FieldChaged

		#region OnFieldCollection_DateAssigned

		/// <summary>
		/// When a new Date is Assigned through the key board input
		/// </summary>
		private void m_fieldsCollection_DateAssigned(object sender, DateTimeFieldAssignedArgs e)
		{
			if(m_isEditing && e.NewValue!=0 )
			{
				int newValue =e.NewValue;
				
				//Gets the year month and day of the edited DateTime
				int year	 =CurrentCalendar.GetYear (m_editedDateTime);
				int month	 =CurrentCalendar.GetMonth(m_editedDateTime);
				int day		 =CurrentCalendar.GetDayOfMonth(m_editedDateTime);

				switch(e.FieldType)
				{
					case DateTimeFieldType.Day:
						{

							//Process the entered number
							//Then check that if the new value is with in limits of a day in a month
							if(newValue > CurrentCalendar.GetDaysInMonth(year,month))
							{
								try
								{
									//Then take the first digit from prev stored value and replace prev digit
									if(newValue.ToString().Length > 1)
									{
										newValue=int.Parse(newValue.ToString()[1].ToString());			
									}
									//We don't need a date of zero and in pagume even 1 digit days should be validated
									if(newValue == 0 || newValue > CurrentCalendar.GetDaysInMonth(year,month))
									{
										newValue =day;
									}
								}
								catch(Exception ex)	//never happens
								{
									//Incase parsing failed set the stored value to prev value
									newValue	= day;
									System.Diagnostics.Trace.WriteLine(string.Format("{0}::OnFieldsCollectionDateAssigned() {1} {2}",this.GetType().Name,ex.GetType(),ex.Message));   
									break;
								}
							}

							m_fieldsCollection.StoredValue	= newValue;	//The stored value should have the new value set
							day								= newValue;	

							//Get the new Date in gregorian calendar
							DateTime editedDateTime	= new DateTime(year,month,day,CurrentCalendar);
  
							//The new edited Date time will have the new date as a value
							m_editedDateTime=new DateTime(editedDateTime.Year,editedDateTime.Month,editedDateTime.Day,
														  Value.Hour,Value.Minute,Value.Second);

							//Check if we entered the correct number of digits if so
							//No more editing possible like entering 18 for a month having 30 days
							//or 3 for a month having 5 days
							if(CurrentCalendar.GetDayOfMonth(m_editedDateTime).ToString().Length ==  CurrentCalendar.GetDaysInMonth(year,month).ToString().Length)
							{
								m_fieldsCollection.EndEditing();  
							}
						}
						break;

					case DateTimeFieldType.Month:
					{
						//Process the entered number
						//Then check that if the new value is with in limits of a month in a year for gregorian 12 ethiopian 13
						if(newValue > CurrentCalendar.GetMonthsInYear(year))
						{
							try
							{
								//Then take the first digit from prev stored value and replace prev digit
								if(newValue.ToString().Length > 1)
								{
									newValue=int.Parse(newValue.ToString()[1].ToString());			
								}
								//We don't need a date of zero month
								if(newValue == 0)
								{
									newValue =month;
								}
							}
							catch(Exception ex)	//never happens
							{
								//Incase parsing failed set the stored value to prev value
								newValue	= month;
								System.Diagnostics.Trace.WriteLine(string.Format("{0}::OnFieldsCollectionDateAssigned() {1} {2}",this.GetType().Name,ex.GetType(),ex.Message));
								break;
							}
						}

						m_fieldsCollection.StoredValue	=	newValue;	//The stored value should have the new value set
						month							=	newValue;	//Set the month to a new value set

						//Make sure the date lies with in the limits of the new month
						if( day > CurrentCalendar.GetDaysInMonth(year,month))
						{
							//If out of bounds then set the date to the max limit
							day = CurrentCalendar.GetDaysInMonth(year,month);
						}//Other wise keep the old date value

						//Get the new Date in gregorian calendar
						DateTime editedDateTime	= new DateTime(year,month,day,CurrentCalendar);
  
						//The new edited Date time will have the new date as a value
						m_editedDateTime=new DateTime(  editedDateTime.Year,editedDateTime.Month,editedDateTime.Day,
														Value.Hour,Value.Minute,Value.Second);

						//Check if we entered the correct number of digits if so
						//No more editing possible like entering 11,12,13
						if(CurrentCalendar.GetMonth(m_editedDateTime).ToString().Length ==  CurrentCalendar.GetMonthsInYear(year).ToString().Length)
						{
							m_fieldsCollection.EndEditing();  
						}
					}break;

					case DateTimeFieldType.Year:
					{
						try
						{
							//if the year is out of bounds set it to last value
							if(newValue > CurrentCalendar.GetYear(DateTime.MaxValue))
							{
								newValue = CurrentCalendar.GetYear(DateTime.MaxValue);
							}
							//if year is below bounds set it to ower limit
							if(newValue < CurrentCalendar.GetYear(DateTime.MinValue))
							{
								newValue = CurrentCalendar.GetYear(DateTime.MinValue);  
							}
							
							//Set the year set
							m_fieldsCollection.StoredValue	=	newValue;	//The stored value should have the new value set
							year							=	newValue;	//Set the year to a new value set

							//We dont want to assign dates that don't exist
							//Make sure the date lies with in the limits of the new Date
							if( day > CurrentCalendar.GetDaysInMonth(year,month))
							{
								//If out of bounds then set the date to the max limit
								day = CurrentCalendar.GetDaysInMonth(year,month);
							}//Other wise keep the old date value

							//Get the new Date in gregorian calendar (this may fail if date out of bounds)
							DateTime editedDateTime	= new DateTime(year,month,day,CurrentCalendar);
  
							//The new edited Date time will have the new date as a value
							m_editedDateTime=new DateTime(  editedDateTime.Year,editedDateTime.Month,editedDateTime.Day,
															Value.Hour,Value.Minute,Value.Second);

							//We can enter a maximum of 4 digits
							if(CurrentCalendar.GetYear(m_editedDateTime).ToString().Length == 4)
							{
								m_fieldsCollection.EndEditing();  
							}
						}
						catch(Exception ex)	//Never Happens
						{
							System.Diagnostics.Trace.WriteLine(string.Format("{0}::OnFieldCollection_DateAssigned() Failed to convert year {1} {2}",this.GetType().Name,ex.GetType(),ex.Message));
						}
					
					}break;
				}
				//System.Diagnostics.Trace.WriteLine("Stored Value is "+m_fieldsCollection.StoredValue.ToString()+"\r\n");
				Invalidate();
			}
		}

		
		#endregion OnFieldCollection_DateAssigned

		#region OnFieldCollection_DateSpinned

		/// <summary>
		/// When a new Date is Set through the up/down button
		/// </summary>
		private void m_fieldsCollection_DateSpinned(object sender, DateTimeFieldSpinArgs e)
		{
			//By what value should we increment the values
			int	increment	= e.Up ? 1 : -1;
 
			//Based on which type of value was modified add/subtract the offset with the
			//current DateTime value
			switch(e.FieldType)
			{
				case DateTimeFieldType.Year:
				{
					this.Value = CurrentCalendar.AddYears(this.Value,increment);  
				}break;

				case DateTimeFieldType.Month:
				{
					this.Value = CurrentCalendar.AddMonths(this.Value,increment); 
				}break;

				case DateTimeFieldType.Day:
				{
					this.Value = CurrentCalendar.AddDays(this.Value,increment);  
				}break;
			}
		}

		
		#endregion OnFieldCollection_DateSpinned

		#region OnFieldCollection_EditingOnProgress

		/// <summary>
		/// Notifies user that editing started
		/// </summary>
		private void m_fieldsCollection_EditingOnProgress(object sender, EventArgs e)
		{
			if(!m_isEditing)
			{
				m_isEditing			=true; 
				m_editedDateTime	=this.Value; 
			}
			//System.Diagnostics.Trace.WriteLine("Editing On Progress");   
		}

		
		#endregion OnFieldCollection_EditingOnProgress

		#region OnFieldCollection_EditingEnded

		/// <summary>
		/// Notifies the user that editing ended
		/// </summary>
		private void m_fieldsCollection_EditingEnded(object sender, EventArgs e)
		{
			if(m_isEditing)		//Always true
			{
				m_isEditing = false;
				this.Value  = m_editedDateTime;								
			}
			//System.Diagnostics.Trace.WriteLine("Editing Ended");
		}

		
		#endregion OnFieldCollection_EditingEnded
		
		#endregion Event Handler
					
		#region Helper Methods

		#region ClosePopUp
			
		/// <summary>
		///  <list type="table">
		///   <item><description>ClosePopUp</description></item>
		///   <para>Idea from : Dan Ardelean - dan.ardelean@swissinfo.org</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>
		///   Also I don’t know if you saw that my implementation of the control in
		///   case the drop down window is loosing focus than the window will close.
		///   There is still some work to do (if you are changing the year and the control losts focus).
		///  </para>
		/// </remarks>
		public void ClosePopUp()
		{ 
			if (dpp_DayPicker.Visible)
			{
				dpp_DayPicker.Visible=false;
				if (this.CloseUp != null)
				{
					this.CloseUp(this, EventArgs.Empty);          
				}
			}
			if(b_MouseDown)
			{
				b_MouseDown=false;
				Invalidate(rt_DropArrow); 
			}
		} // public void ClosePopUp()
			
			
		#endregion ClosePopUp
			
		#region CreateMemoryBitmap
			
		/// <summary>
		///  <list type="table">
		///   <item><description>Create offsceeen bitmap.</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This bitmap is used for double-buffering to prevent flashing.</para>
		/// </remarks>
		/// <seealso cref="DateTimePickerEx.OnPaint"/>
		/// <seealso cref="DateTimePickerEx.pta_ArrowPoints"/>
		/// <seealso cref="DateTimePickerEx.bmp_Bmp"/>
		/// <seealso cref="DateTimePickerEx.DropArrowSize"/>
		private void CreateMemoryBitmap(Graphics g)
		{
			if(b_TimePicker)
			{
				if (bmp_Bmp == null || bmp_Bmp.Width != this.Width - tp_TimePicker.Width || bmp_Bmp.Height != this.Height)
				{
					//Clear the back ground area of the datetime picker and the time picker control
					bmp_Bmp = new Bitmap(this.Width,
										 this.Height > tp_TimePicker.Height ? 
										this.Height : tp_TimePicker.Height);
					
					gr_Graphics = Graphics.FromImage(bmp_Bmp);
					gr_Graphics.Clear(this.Parent.BackColor);
					g.DrawImage(bmp_Bmp, 0, 0);					  

					// memory bitmap
					bmp_Bmp = new Bitmap(this.Width - tp_TimePicker.Width -1 ,this.Height );
					gr_Graphics = Graphics.FromImage(bmp_Bmp);
				}		
			}
			else
			{
				if (bmp_Bmp == null || bmp_Bmp.Width != this.Width  || bmp_Bmp.Height != this.Height)
				{
					// memory bitmap
					bmp_Bmp = new Bitmap(this.Width,this.Height);
					gr_Graphics = Graphics.FromImage(bmp_Bmp);
				}	
			}
            
		} // private void CreateMemoryBitmap()
			
		
		#endregion CreateMemoryBitmap

		#region CreateGdiObjects
				
		/// <summary>
		///  <list type="table">
		///   <item><description>Create GDI objects required to paint the control.</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Create if not allready done</para>
		/// </remarks>
		/// <seealso cref="DateTimePickerEx.OnPaint"/>
		/// <seealso cref="DateTimePickerEx.sb_Frame"/>
		/// <seealso cref="DateTimePickerEx.pen_Frame"/>
		/// <seealso cref="DateTimePickerEx.sb_Foreground"/>
		/// <seealso cref="DateTimePickerEx.sb_Disabled"/>
		private void CreateGdiObjects()
		{
			// window frame brush
			if (sb_Frame == null)
				sb_Frame = new SolidBrush(SystemColors.WindowFrame);
			
			// window frame pen
			if (pen_Frame == null)
				pen_Frame = new Pen(SystemColors.WindowFrame);
			
			// fore color brush, the .Net CF does not support OnForeColorChanged,
			// so we detect if the forecolor changed here
			if (sb_Foreground == null || sb_Foreground.Color != this.ForeColor)
				sb_Foreground = new SolidBrush(this.ForeColor);
			
			// disabled brush
			if (sb_Disabled == null)
				sb_Disabled = new SolidBrush(SystemColors.GrayText);
			
		}
			
		
		#endregion CreateGdiObjects

		#region OnValidFontSize
			
		/// <summary>
		///  <list type="table">
		///   <item><description>OnValidFont</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Check if set Font will fill Screen correctly</para>
		/// </remarks>
		/// <param name="ft_Font">Font to check</param>
		/// <returns>Return valid Font the will fit the Screen</returns>
		protected Font OnValidFontSize(Font ft_Font)
		{
			bool                rc = false;
			FontStyle fs_FontStyle = ft_Font.Style;
			string     s_FontName  = ft_Font.Name;
			float      f_FontSize  = ft_Font.Size;
				
			// create the memory bitmap
			Bitmap		bmp_bmp = new Bitmap(240,50);
			Graphics	gr_graphics = Graphics.FromImage(bmp_bmp);
			string		s_Text   = "DDDDDDDDDDDDDDDDDDDDDDDDD"; // 25=
			Rectangle rt_Client = dpp_DayPicker.ControlSize;
			if (this.TopLevelControl != null)
				rt_Client = this.TopLevelControl.ClientRectangle;
			Size sz_Text  = gr_graphics.MeasureString(s_Text,ft_Font).ToSize();
			int i_Height  = (sz_Text.Height*3)/2;							// Caption
			i_Height     += (sz_Text.Height*(dpp_DayPicker.i_NumRows+1));   // DaysOfWeek and Days
			i_Height     += (sz_Text.Height+(sz_Text.Height/3))+2;			// Bottom

			if ((sz_Text.Width > rt_Client.Width) && (i_Height > rt_Client.Height))
			{ // this would fit the Screen
				while (!rc)
				{
					f_FontSize -= 1;
					Font ft_FontTest = new Font(s_FontName,f_FontSize,fs_FontStyle);
					sz_Text   = gr_graphics.MeasureString(s_Text,ft_FontTest).ToSize();
					i_Height  = (sz_Text.Height*3)/2;                         // Caption
					i_Height += (sz_Text.Height*(dpp_DayPicker.i_NumRows+1));   // DaysOfWeek and Days
					i_Height += (sz_Text.Height+(sz_Text.Height/3))+2;        // Bottom
					if ((sz_Text.Width < rt_Client.Width) && (i_Height < rt_Client.Height))
					{ // this would fit the Screen
						ft_Font = ft_FontTest;
						rc = true;
					}
					else
					{ // this will not fit the Screen
					}
				}  // while (!rc)
			}   // if (sz_Text.Width < this.TopLevelControl.ClientRectangle.Width)
			gr_graphics.Dispose();
			//-------------------------------
			return ft_Font;
		} // protected Font OnValidFontSize(Font ft_Font)
			
		
		#endregion OnValidFontSize

		#region SubscribeEventToChild

		/// <summary>
		/// Suscribes the events to the child controls of the form containing
		/// the excluded control
		/// </summary>
		/// <param name="parentControl">The control to be excluded</param>
		/// <param name="excludedControl">The control to be excluded</param>
		protected void SubscribeEventToChild(Control parentControl,Control excludedControl)
		{
			if(parentControl != excludedControl)
			{
				parentControl.MouseDown +=	new MouseEventHandler(this.OnDayPicker_LostFocus);
				parentControl.GotFocus	+=	new EventHandler(this.parentControl_GotFocus);
			}
			foreach(Control control in parentControl.Controls)
			{
				SubscribeEventToChild(control,excludedControl);			
			}
		}

		
		#endregion SubscribeEventToChild
		
		//-- Helper Methods
		#endregion Helper Methods
	}
		

	#endregion DateTimePickerEx class
		
	#region DayPickerPopup class
		
	/// <summary>
	///  <list type="table">
	///   <item><description>Displays a calendar that allows user to select a new date.</description></item>
	///  </list>
	/// </summary>
	/// <remarks>
	///  <para>Displays box around today and user can hover over dates</para>
	///  <para>Allows quick access to month with month context menu and year with numeric updown control</para>
	/// </remarks>
	class DayPickerPopup : Control
	{		
		#region Constant

		class Const
		{
			/// <summary>
			/// The Caption Height
			/// </summary>
			public const int CaptionHeight = 28;
		}

		#endregion Constant

		#region String\Format\Apperance\Option Vars.

		/// <summary>
		///  <list type="table">
		///   <item><description>Prevent unneeded Control.Invalidate();</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		/// Set to true in Property Font
		///  <list type="bullet">
		///   <item>
		///    <description>Prevent Inavlidate in :</description>
		///    <para>FontCaption</para>
		///    <para>FontDay</para>
		///    <para>FontToday</para>
		///   </item>
		///  </list>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.Font"/>
		/// <seealso cref="DayPickerPopup.FontCaption"/>
		/// <seealso cref="DayPickerPopup.FontDay"/>
		/// <seealso cref="DayPickerPopup.FontToday"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		private bool				b_Font		 =false;

		// exposed events
		/// <summary>
		///  <list type="table">
		///   <item><description>Specifies the Type to use to show the "Today" Date in the Control</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>SquibbleType.Squibble</para>
		///   </item>
		///  </list>
		///  <para>Squibble = Desktop Look and feel</para>
		///  <para>Rectangle = WinCe-API Look and feel</para>
		///  <para>Ellipse = Other Look and feel</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.OnPaintSquibble"/>
		/// <seealso cref="DayPickerPopup.DrawHoverSelection"/>
		/// <seealso cref="DayPickerPopup.DrawTodaySelection"/>
		/// <seealso cref="DayPickerPopup.DrawBottomLabels"/>
		/// <seealso cref="DayPickerPopup.DrawCurSelection"/>
		/// <seealso cref="DateTimePickerEx.Squibble"/>
		public SquibbleType			sqt_Squibble = SquibbleType.Rectangle;	

		/// <summary>
		///  <list type="table">
		///   <item><description>Date Format</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>DateTimePickerFormat.Short</para>
		///   </item>
		///  </list>
		/// </remarks>
		/// <seealso cref="DateTimePicker.Format"/>
		/// <seealso cref="DateTimePicker.Text"/>
		public DateTimePickerFormat dtpf_Format  = DateTimePickerFormat.Custom;

		/// <summary>
		/// What should be the first day of the week
		/// </summary>
		private DayOfWeek			ci_FirstDayOfWeek	 =DayOfWeek.Sunday;

		/// <summary>
		///  <list type="table">
		///   <item><description>Field with value of First Day of Week</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This DayOfWeek is used to :</para>
		///  <para>- CalculateFirstDate :  dt_FirstDate</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.FirstDayOfWeek"/>
		/// <seealso cref="DayPickerPopup.CalculateFirstDate"/>
		private DayOfWeek			dow_FirstDayOfWeek;

		/// <summary>
		///  <list type="table">
		///   <item><description>Character's per Weekday to be shown</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>- Default = 2</para>
		///  <para>This Field is used :</para>
		///  <para>- DayOfWeekCharacters : Set as result of DateTimePicker.DayOfWeekCharacters=value</para>
		///  <para>- FirstDayofWeek : Set as result of sz_NumCols*sz_Size.Width;</para>
		/// </remarks>
		/// <seealso cref="DateTimePickerEx.DayOfWeekCharacters"/>
		/// <seealso cref="DayPickerPopup.DayOfWeekCharacters"/>
		/// <seealso cref="DayPickerPopup.FirstDayOfWeek"/>
		private int					i_DayOfWeekCharacters        = 3;

		/// <summary>
		///  <list type="table">
		///   <item><description>String to hold Text of "Today" and Date in supported Language</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : DrawBottomLabels : string text = string.Format("Today: {0}",dt_Today.ToShortDateString());</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>empty</para>
		///   </item>
		///  </list>
		///  <para>This Field is used :</para>
		///  <para>- OnPaint : string.Format("{0}: {1}",s_Today,dt_Today.ToString(s_CustomFormat));</para>
		///  <para>- OnPaint : Checks if result is greater thant the Width of the Control/BottomLabel/Caption</para>
		///  <para>--> if this is the case the Control will be resized</para>
		///  <para>--> this is the reson why this string creation has been moved from DrawBottomLabels to OnPaint</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.DrawBottomLabels"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		public  string				s_BottomLabels = "";

		/// <summary>
		///  <list type="table">
		///   <item><description>String Array to use to fill DaysOfWeek in Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : DrawDaysOfWeek : const string dow = "SMTWTFS";</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>S,M,T,W,T,F,S</para>
		///   </item>
		///  </list>
		///  <para>This Field is used to :</para>
		///  <para>- DrawDaysOfWeek : Paint Days of Week in Control</para>
		///  <para>- DateTimePicker_CultureInfo.OnDaysOfWeek : Reads CultureInfo.DateTimeFormat.DayNames[i_DOW];</para>
		///  <para>- DayOfWeekCharacters : Strings created accourding to Character Width set in i_DayOfWeek</para>
		///  <para>--> DateTimePicker_CultureInfo.s_DayOfWeek[i].Substring(0,i_DayOfWeekCharacters);</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.i_DayOfWeekCharacters"/>
		/// <seealso cref="DayPickerPopup.DayOfWeekCharacters"/>
		/// <seealso cref="DayPickerPopup.DrawDaysOfWeek"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		public  string[]			s_DayOfWeek    = new string[]{"S","M","T","W","T","F","S"};

		/// <summary>
		///  <list type="table">
		///   <item><description>Initialize CultureInfo used in Current Mashine</description><para>mj10777 CultureInfo support</para></item>
		///   <para>Idea from : Éric Carmichael - eric@westgen.com</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>yyyy-MM-ddTHH:mm:sszzzzzz</description>
		///    <para>2004-03-29T00:00:00++02:00</para>
		///    <para> - so that my strong style dataset - validated by an XML schema will recognioze the date and time.</para>7s
		///   </item>
		///  </list>
		///  <para>Stores set CultureInfo used by the DateTimePicker</para>
		/// </remarks>
		public string				s_CustomFormat = "MMMM dd, yyyy";

		/// <summary>
		///  <list type="table">
		///   <item><description>Text for "Today" in Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>Original Version used : DrawBottomLabels() : string text=string.Format("Today: {0}",dt_Today.ToShortDateString());</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Today</para>
		///   </item>
		///  </list>
		///  <para>This Field is used to :</para>
		///  <para>- OnPaint : used to Fill s_BottomLabels</para>
		///  <para>- DrawBottomLabels : Paint s_BottomLabels (with s_Today)</para>
		///  <para>- Culture : retrieves Value set in DateTimePicker_CultureInfo.s_Today accourding to set CultureInfo</para>
		///  <para>- DateTimePicker.Today : Property to allow User to override Value set in Culture</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		/// <seealso cref="DayPickerPopup.s_BottomLabels"/>
		/// <seealso cref="DayPickerPopup.DrawBottomLabels"/>		
		public  string				s_Today        = "ዛሬ";

		/// <summary>
		///  <list type="table">
		///   <item><description>Font Name for the Caption portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : Const.FontName="Arial";</para>
		///  </list>
		/// </summary>
		/// <remarks>
		/// Set in Propertiy FontCaption
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Frutiger Linotype</para>
		///   </item>
		///  </list>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.FontCaption"/>
		private string				s_FontName_Caption  = "Ethiopia Jiret";
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Font Size for the Caption portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : Const.FontSize=9;</para>
		///  </list>
		/// </summary>
		/// <remarks>
		/// Set in Propertiy FontCaption
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>10</para>
		///   </item>
		///  </list>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.FontCaption"/>
		private float				f_FontSize_Caption  = 9F;
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Font Style for the Caption portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>Original Version used : FontStyle.Bold</para>
		///  </list>
		/// </summary>
		/// <remarks>
		/// Set in Propertiy FontCaption
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>FontStyle.Bold</para>
		///   </item>
		///  </list>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.FontCaption"/>
		private FontStyle			fs_FontStyle_Caption = FontStyle.Regular;

		/// <summary>
		///  <list type="table">
		///   <item><description>Font Name for the Day portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : Const.FontName="Arial";</para>
		///  </list>
		/// </summary>
		/// <remarks>
		/// Set in Propertiy FontDay
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Frutiger Linotype</para>
		///   </item>
		///  </list>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.FontDay"/>
		private string				s_FontName_Day  = "Ethiopia Jiret";
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Font Size for the Day portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : Const.FontSize=9;</para>
		///  </list>
		/// </summary>
		/// <remarks>
		/// Set in Propertiy FontDay
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>10</para>
		///   </item>
		///  </list>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.FontDay"/>
		private float				f_FontSize_Day  = 9F;
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Font Style for the Day portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>Original Version used : FontStyle.Regular</para>
		///  </list>
		/// </summary>
		/// <remarks>
		/// Set in Propertiy FontDay
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>FontStyle.Bold</para>
		///   </item>
		///  </list>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.FontDay"/>
		private FontStyle			fs_FontStyle_Day = FontStyle.Regular;

		/// <summary>
		///  <list type="table">
		///   <item><description>Font Name for the Today portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : Const.FontName="Arial";</para>
		///  </list>
		/// </summary>
		/// <remarks>
		/// Set in Propertiy FontToday
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Frutiger Linotype</para>
		///   </item>
		///  </list>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.FontToday"/>
		private string				s_FontName_Today  = "Ethiopia Jiret";
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Font Size for the Today portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : Const.FontSize=9;</para>
		///  </list>
		/// </summary>
		/// <remarks>
		/// Set in Propertiy FontToday
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>10</para>
		///   </item>
		///  </list>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.FontToday"/>
		private float				f_FontSize_Today  = 9F;
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Font Style for the Today portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>Original Version used : FontStyle.Bold</para>
		///  </list>
		/// </summary>
		/// <remarks>
		/// Set in Propertiy FontToday
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>FontStyle.Bold</para>
		///   </item>
		///  </list>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.FontToday"/>
		private FontStyle			fs_FontStyle_Today = FontStyle.Regular;

		#endregion String\Format\Apperance\Option Vars.

		#region Graphics\Disposable Vars.
		
		/// <summary>
		///  <list type="table">
		///   <item><description>offscreen bitmap</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>memory bitmap to prevent flashing</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateMemoryBitmap"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		private Bitmap bmp_Bmp;

		/// <summary>
		///  <list type="table">
		///   <item><description>Graphics for Painting</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateMemoryBitmap"/>
		/// <seealso cref="DayPickerPopup.DrawCurSelection"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		private Graphics gr_Graphics;

		/// <summary>
		///  <list type="table">
		///   <item><description>Pen Day : HoverBox</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>SystemColors.GrayText</para>
		///   </item>
		///  </list>
		///  <para>GDI Objects</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateGdiObjects"/>
		/// <seealso cref="DayPickerPopup.DrawHoverSelection"/>
		private Pen pen_HoverBox;

		/// <summary>
		///  <list type="table">
		///   <item><description>SolidBrush Day : Cursor, Foreground</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>this.ForeColor</para>
		///   </item>
		///  </list>
		///  <para>GDI Objects</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateGdiObjects"/>
		/// <seealso cref="DayPickerPopup.DrawBottomLabels"/>
		/// <seealso cref="DayPickerPopup.DrawDay"/>
		/// <seealso cref="DayPickerPopup.DrawDays"/>
		private SolidBrush br_Foreground;

		/// <summary>
		///  <list type="table">
		///   <item><description>SolidBrush General : Background</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>this.BackColor</para>
		///   </item>
		///  </list>
		///  <para>GDI Objects</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateGdiObjects"/>
		/// <seealso cref="DayPickerPopup.DrawCaption"/>
		/// <seealso cref="DayPickerPopup.DrawDay"/>
		private SolidBrush br_Backround;

		/// <summary>
		///  <list type="table">
		///   <item><description>Pen General : Background</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>this.BackColor</para>
		///   </item>
		///  </list>
		///  <para>GDI Objects</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateGdiObjects"/>
		/// <seealso cref="DayPickerPopup.DrawHoverSelection"/>
		private Pen pn_Background;

		/// <summary>
		///  <list type="table">
		///   <item><description>SolidBrush General : Frame</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>SystemColors.WindowFrame</para>
		///   </item>
		///  </list>
		///  <para>GDI Objects</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateGdiObjects"/>
		/// <seealso cref="DayPickerPopup.DrawCaption"/>
		private SolidBrush sb_Frame;

		/// <summary>
		///  <list type="table">
		///   <item><description>Pen General : Frame</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>SystemColors.WindowFrame</para>
		///   </item>
		///  </list>
		///  <para>GDI Objects</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateGdiObjects"/>
		/// <seealso cref="DayPickerPopup.DrawCaption"/>
		/// <seealso cref="DayPickerPopup.DrawDaysOfWeek"/>
		/// <seealso cref="DayPickerPopup.DrawTodaySelection"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		private Pen pen_Frame;

		/// <summary>
		///  <list type="table">
		///   <item><description>Pen Today selection</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>System.Drawing.Color.Red</para>
		///   </item>
		///  </list>
		///  <para>GDI Objects</para>
		/// </remarks>
		/// <seealso cref="DateTimePickerEx.CreateGdiObjects"/>
		/// <seealso cref="DateTimePickerEx.OnPaint"/>
		private Pen pen_Today;

		/// <summary>
		/// Used in the Initialization of The Menu Extender
		/// Component
		/// </summary>
		private IContainer components			= new System.ComponentModel.Container();		
		
		/// <summary>
		///  <list type="table">
		///   <item><description>SolidBrush Day : Other Month</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>SystemColors.GrayText</para>
		///   </item>
		///  </list>
		///  <para>GDI Objects</para>
		///  <para>Designer-Control Support: Initalise here</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateGdiObjects"/>
		/// <seealso cref="DayPickerPopup.DrawDays"/>
		private SolidBrush sb_ForeColorInactiveDays = new SolidBrush(SystemColors.GrayText);

		/// <summary>
		///  <list type="table">
		///   <item><description>SolidBrush Day : Invalid Day - active Month</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>System.Drawing.Color.Red</para>
		///   </item>
		///  </list>
		///  <para>GDI Objects</para>
		///  <para>Designer-Control Support: Initalise here</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateGdiObjects"/>
		/// <seealso cref="DayPickerPopup.DrawDays"/>
		private SolidBrush sb_RedActive				= new SolidBrush(System.Drawing.Color.Red);

		/// <summary>
		///  <list type="table">
		///   <item><description>SolidBrush Day : Invalid Day - inactive Month</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>System.Drawing.Color.LightSalmon</para>
		///   </item>
		///  </list>
		///  <para>GDI Objects</para>
		///  <para>Designer-Control Support: Initalise here</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateGdiObjects"/>
		/// <seealso cref="DayPickerPopup.DrawDays"/>
		private SolidBrush sb_RedInactive			= new SolidBrush(System.Drawing.Color.LightSalmon);

		/// <summary>
		///  <list type="table">
		///   <item><description>SolidBrush Day : Background</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>SystemColors.Highlight</para>
		///   </item>
		///  </list>
		///  <para>GDI Objects</para>
		///  <para>Designer-Control Support: Initalise here</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateGdiObjects"/>
		/// <seealso cref="DayPickerPopup.DrawDay"/>
		/// <seealso cref="DayPickerPopup.DrawCurSelection"/>
		private SolidBrush sb_BackSelect			= new SolidBrush(SystemColors.Highlight);

		/// <summary>
		///  <list type="table">
		///   <item><description>SolidBrush Day : Text</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>SystemColors.HighlightText</para>
		///   </item>
		///  </list>
		///  <para>GDI Objects</para>
		///  <para>Designer-Control Support: Initalise here</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateGdiObjects"/>
		/// <seealso cref="DayPickerPopup.DrawDay"/>
		/// <seealso cref="DayPickerPopup.DrawCurSelection"/>
		private SolidBrush sb_TextSelect			= new SolidBrush(SystemColors.HighlightText);

		/// <summary>
		///  <list type="table">
		///   <item><description>SolidBrush Caption : Background</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>SystemColors.ActiveCaption</para>
		///   </item>
		///  </list>
		///  <para>GDI Objects</para>
		///  <para>Designer-Control Support: Initalise here</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateGdiObjects"/>
		/// <seealso cref="DayPickerPopup.DrawCaption"/>
		/// <seealso cref="DayPickerPopup.DrawDaysOfWeek"/>
		private SolidBrush sb_CaptionBack			= new SolidBrush(SystemColors.ActiveCaption);

		/// <summary>
		/// The Brush that will be used to paint the background
		/// of the months area in the pop up control
		/// </summary>
		private SolidBrush sb_MonthBack				= new SolidBrush(SystemColors.Window); 
		
		/// <summary>
		///  <list type="table">
		///   <item><description>SolidBrush Caption : Text</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>SystemColors.ActiveCaptionText</para>
		///   </item>
		///  </list>
		///  <para>GDI Objects</para>
		///  <para>Designer-Control Support: Initalise here</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateGdiObjects"/>
		/// <seealso cref="DayPickerPopup.DrawCaption"/>
		private SolidBrush sb_CalendarTitelForeColor = new SolidBrush(SystemColors.ActiveCaptionText);

		/// <summary>
		///  <list type="table">
		///   <item><description>Font used for the Caption portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : m_fontCaption;</para>
		///  </list>
		/// </summary>
		/// <remarks>
		/// Set in Propertiy FontCaption
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Frutiger Linotype</para>
		///    <para>10</para>
		///    <para>FontStyle.Bold</para>
		///   </item>
		///  </list>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.FontCaption"/>
		private Font        ft_FontCaption			= new Font("Ethiopia Jiret",9F,FontStyle.Regular);

		/// <summary>
		///  <list type="table">
		///   <item><description>Font used for the Day portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : m_fontCaption;</para>
		///  </list>
		/// </summary>
		/// <remarks>
		/// Set in Propertiy FontDay
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Frutiger Linotype</para>
		///    <para>10</para>
		///    <para>FontStyle.Bold</para>
		///   </item>
		///  </list>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.FontDay"/>
		private Font        ft_FontDay				= new Font("Ethiopia Jiret",9F,FontStyle.Regular);

		/// <summary>
		///  <list type="table">
		///   <item><description>Font used for the Today portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : m_fontCaption;</para>
		///  </list>
		/// </summary>
		/// <remarks>
		/// Set in Propertiy FontToday
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Frutiger Linotype</para>
		///    <para>10</para>
		///    <para>FontStyle.Bold</para>
		///   </item>
		///  </list>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.FontToday"/>
		private Font        ft_FontToday			= new Font("Ethiopia Jiret",9F,FontStyle.Regular);

		#endregion Graphics\Disposable Vars.

		#region Region\Coordinate\Size Vars. (Rects\Points)

		/// <summary>
		///  <list type="table">
		///   <item><description>Width and Height Partent Program</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Rectangle(0,0,240,268)</para>
		///    <para>- Compact Screen</para>
		///   </item>
		///  </list>
		///  <para>This Field is used :</para>
		///  <para>- to make sure the Control will fit the Scrren</para>
		/// </remarks>
		private Rectangle rt_ClientSize = new Rectangle(0,0,244,268);

		/// <summary>
		///  <list type="table">
		///   <item><description>Rectangle for Left Arrow Button Caption portion of Control</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Rectangle.Empty</para>
		///   </item>
		///  </list>
		///  <para>Hit testing</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.DrawCaption"/>
		/// <seealso cref="DayPickerPopup.OnMouseDown"/>
		/// <seealso cref="DayPickerPopup.CreateMemoryBitmap"/>
		private Rectangle rt_LeftButton  = Rectangle.Empty;

		/// <summary>
		///  <list type="table">
		///   <item><description>Rectangle for Right Arrow Button Caption portion of Control</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Rectangle.Empty</para>
		///   </item>
		///  </list>
		///  <para>Hit testing</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.DrawCaption"/>
		/// <seealso cref="DayPickerPopup.OnMouseDown"/>
		/// <seealso cref="DayPickerPopup.CreateMemoryBitmap"/>
		private Rectangle rt_RightButton = Rectangle.Empty;

		/// <summary>
		///  <list type="table">
		///   <item><description>Rectangle for Month ContextMenu Caption portion of Control</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Rectangle.Empty</para>
		///   </item>
		///  </list>
		///  <para>Hit testing</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.DrawCaption"/>
		/// <seealso cref="DayPickerPopup.OnMouseDown"/>
		private Rectangle rt_Month       = Rectangle.Empty;

		/// <summary>
		///  <list type="table">
		///   <item><description>Rectangle for Year NumericUpDown Caption portion of Control</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Rectangle.Empty</para>
		///   </item>
		///  </list>
		///  <para>Hit testing</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.DisplayYearUpDown"/>
		/// <seealso cref="DayPickerPopup.DrawCaption"/>
		/// <seealso cref="DayPickerPopup.OnMouseDown"/>
		private Rectangle rt_Year        = Rectangle.Empty;

		/// <summary>
		/// Used to show the toggle button for Gregorian or Ethiopian calendar
		/// </summary>
		private Rectangle rt_Calendar	 = Rectangle.Empty;  

		/// <summary>
		///  <list type="table">
		///   <item><description>Array[3] Left Arrow Button Point (Coordinates)</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>nothing</para>
		///   </item>
		///  </list>
		///  <para>Calculate Points to Draw Arrow</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.DrawCaption"/>
		/// <seealso cref="DayPickerPopup.CreateMemoryBitmap"/>
		private PointF[] pta_LeftArrowPoints = new PointF[3];

		/// <summary>
		///  <list type="table">
		///   <item><description>Array[3] Right Arrow Button Point (Coordinates)</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>nothing</para>
		///   </item>
		///  </list>
		///  <para>Calculate Points to Draw Arrow</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.DrawCaption"/>
		/// <seealso cref="DayPickerPopup.CreateMemoryBitmap"/>
		private PointF[] pta_RightArrowPoints = new PointF[3];

		/// <summary>
		///  <list type="table">
		///   <item><description>Width and Height of Arrows Buttons</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : Const.ArrowButtonOffset = new Size(6,6);</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Size(6,6);</para>
		///   </item>
		///  </list>
		///  <para>This Field is used :</para>
		///  <para>- OnPaint : sz_ArrowButtonOffset.Width  = is 3.33% of the Control Width</para>
		///  <para>- OnPaint : sz_ArrowButtonOffset.Height = is 25% of the Caption Height (stored in sz_SizeControl.Height)</para>
		///  <para>- CreateMemoryBitmap : used to set rt_LeftButton / rt_RightButton</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateMemoryBitmap"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		public  Size        sz_ArrowButtonOffset = new Size(6,6);
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Width and Height of Arrows Buttons</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : Const.ArrowButtonSize = new Size(20,15);</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Size(20,15);</para>
		///   </item>
		///  </list>
		///  <para>This Field is used :</para>
		///  <para>- OnPaint : sz_ArrowButtonSize.Width  = is 2% of the Control Width</para>
		///  <para>- OnPaint : sz_ArrowButtonSize.Height = is 25% of the Caption Height (stored in sz_SizeControl.Height)</para>
		///  <para>- CreateMemoryBitmap : used to set rt_LeftButton / rt_RightButton</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateMemoryBitmap"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		public  Size        sz_ArrowButtonSize   = new Size(20,15);
		
		/// <summary>
		///  <list type="table">
		///   <item><description>left/right arrow in button</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : Const.ArrowPointsOffset = new Size(13,9);</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Size(13,9);</para>
		///   </item>
		///  </list>
		///  <para>This Field is used :</para>
		///  <para>- CreateMemoryBitmap : used to set pta_LeftArrowPoints / pta_RightArrowPoints</para>
		///  <para>--> no longer used, but the results are not very good - thus retained;</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateMemoryBitmap"/>
		public  Size        sz_ArrowPointsOffset = new Size(13,9);

		/// <summary>
		///  <list type="table">
		///   <item><description>left/right arrow in button</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : Const.ArrowPointsSize = new Size(5,10);</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Size(5,10);</para>
		///   </item>
		///  </list>
		///  <para>This Field is used :</para>
		///  <para>- CreateMemoryBitmap : used to set pta_LeftArrowPoints / pta_RightArrowPoints</para>
		///  <para>--> no longer used, but the results are not very good - thus retained;</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CreateMemoryBitmap"/>
		public  Size        sz_ArrowPointsSize   = new Size(5,10);		
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Bottom Label Position of Today and Date</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : Const.BottomLabelsPos = new Point(6,135);</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Point(6,135);</para>
		///   </item>
		///  </list>
		///  <para>This Field is used :</para>
		///  <para>- OnPaint : Default pt_BottomLabelsPos.X is used a start Position of s_BottomLabels</para>
		///  <para>- OnPaint : pt_BottomLabelsPos.Y = The BottomLabel starts after the Caption, DaysOfWeek and Days</para>
		///  <para>- OnPaint : 2 Pixels are added to Y Position for cosmitic reasons</para>
		///  <para>- DrawBottomLabels : use Position to paint s_BottomLabels</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.DrawBottomLabels"/>
		/// <seealso cref="DayPickerPopup.GetDayIndex"/>
		/// <seealso cref="DayPickerPopup.OnMouseDown"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		public  Point       pt_BottomLabelsPos   = new Point(6,135);
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Height of Bottom Label Today and Date</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : Const.BottomLabelHeight=12;</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>12</para>
		///   </item>
		///  </list>
		///  <para>This Field is used :</para>
		///  <para>- OnPaint : Set as result of Height of "ZZ" String</para>
		///  <para>- OnPaint : used to help caculate i_ControlHeight</para>
		///  <para>- could be local in OnPaint - if no longer needed</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		private int         i_BottomLabelHeight = 12;
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Height of Caption with Arrows and Month Year Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : Const.CaptionHeight=28;</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>28</para>
		///   </item>
		///  </list>
		///  <para>This Field is used :</para>
		///  <para>- OnPaint : Set as result of ((Height of "ZZ" String)*3)/2</para>
		///  <para>- DrawCaption : used to Paint Background of Caption</para>
		///  <para>- DrawCaption : used to ... </para>
		///  <para>- DrawDaysOfWeek : used for Height of DaysOfWeek Text</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		/// <seealso cref="DayPickerPopup.DrawCaption"/>
		/// <seealso cref="DayPickerPopup.DrawDaysOfWeek"/>
		private int         i_CaptionHeight     = 28;
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Height of Control with Caption, Days and Today Label</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : DayPickerPopup(): this.Size=new Size(Const.ControlWidth,Const.BottomLabelsPos.Y+Const.BottomLabelHeight+5);</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>152</para>
		///   </item>
		///  </list>
		///  <para>This Field is used :</para>
		///  <para>- OnPaint : Set as result of i_CaptionHeight+(sz_Size.Height*(i_NumRows+1))+i_BottomLabelHeight</para>
		///  <para>- could be local in OnPaint - if no longer needed</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		private int         i_ControlHeight     = 152;
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Width of Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : Const.ControlWidth=164;</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>164</para>
		///   </item>
		///  </list>
		///  <para>This Field is used :</para>
		///  <para>- OnPaint : Set as result of i_NumCols*sz_Size.Width;</para>
		///  <para>- could be local in OnPaint - if no longer needed</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		private int         i_ControlWidth      = 164;
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Point of Day cells in Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : DaysGrid = new Point(6,43);</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Point(0,43);</para>
		///   </item>
		///  </list>
		///  <para>This Field is used to :</para>
		///  <para>- OnPaint : pt_DayGrids.Y : Calulated accourding to result of gr_Graphics.MeasureString("ZZ",ft_FontDay).ToSize();</para>
		///  <para>- DayPickerPopup : pt_DayGrids.Y += sz_DaysCell.Height</para>
		///  <para>- DrawDays : starting point of grid</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.DrawDays"/>
		/// <seealso cref="DayPickerPopup.DrawDaysOfWeek"/>
		/// <seealso cref="DayPickerPopup.GetDayCellPosition"/>
		/// <seealso cref="DayPickerPopup.GetDayIndex"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		public  Point       pt_DayGrids          = new Point(0,43); 
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Size of Day cells in Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : DaysCell = new Size(23,14);</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>Size(23,14);</para>
		///   </item>
		///  </list>
		///  <para>This Field is used to :</para>
		///  <para>- OnPaint : Calulated accourding to result of gr_Graphics.MeasureString("ZZ",ft_FontDay).ToSize();</para>
		///  <para>- OnPaint : final size depends of widest Width/Height of</para>
		///  <para>--> i_NumCols*sz_SizeControl.Width OR Caption Width OR BottomLable Width</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.DrawCurSelection"/>
		/// <seealso cref="DayPickerPopup.DrawDay"/>
		/// <seealso cref="DayPickerPopup.DrawDays"/>
		/// <seealso cref="DayPickerPopup.DrawDaysOfWeek"/>
		/// <seealso cref="DayPickerPopup.DrawHoverSelection"/>
		/// <seealso cref="DayPickerPopup.DrawTodaySelection"/>
		/// <seealso cref="DayPickerPopup.GetDayCellPosition"/>
		/// <seealso cref="DayPickerPopup.GetDayIndex"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		public  Size        sz_DaysCell          = new Size(23,14);		
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Number of Columns in Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : NumCols=7;</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>7</para>
		///   </item>
		///  </list>
		///  <para>This Field is used to :</para>
		///  <para>- OnPaint : Calculate Position of i_ControlWidth</para>
		///  <para>- DrawDays : Loop to Paint Days</para>
		///  <para>- GetDayCellPosition : Calculate pt_DayGrids</para>
		///  <para>- GetDayIndex : Calculate Index if Position found in day grid bounding rectangle</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		/// <seealso cref="DayPickerPopup.DrawDays"/>
		/// <seealso cref="DayPickerPopup.GetDayCellPosition"/>
		/// <seealso cref="DayPickerPopup.GetDayIndex(DateTime)"/>
		public  int          i_NumCols           = 7;
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Number of Rows in Control</description><para>mj10777 CultureInfo support</para></item>
		///   <para>replaces Original Version : NumRows=6;</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>6</para>
		///   </item>
		///  </list>
		///  <para>This Field is used to :</para>
		///  <para>- OnPaint : Calculate Position of pt_BottomLabelsPos.Y</para>
		///  <para>- DrawDays : Loop to Paint Days</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		/// <seealso cref="DayPickerPopup.DrawDays"/>
		public  int          i_NumRows           = 6;		

		#endregion Region\Coordinate\Size Vars. (Rects\Points)

		#region Controls\Components Vars.(ContextMenus,NumericUpDown)

		/// <summary>
		///  <list type="table">
		///   <item><description>ContextMenu for Months in Caption portion of Control</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>User can click on month in caption to quickly change values</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.DisplayMonthMenu"/>
		/// <seealso cref="DayPickerPopup.DrawCaption"/>
		/// <seealso cref="DayPickerPopup.InitMonthContextMenu"/>
		/// <seealso cref="DayPickerPopup.OnMonthMenuClick"/>
		/// <seealso cref="DayPickerPopup.OnMonthMenuPopup"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		private ContextMenuStrip cm_MonthMenu;

		/// <summary>
		/// Used to render the context menu to display amharic fonts
		/// </summary>
		//private MenuExtender  menuExtender;		
	
		/// <summary>
		///  <list type="table">
		///   <item><description>NumericUpDown for Years in Caption portion of Control</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>User can click on year in caption to quickly change values</para>
		/// </remarks>
		/// <seealso cref="DateTimePickerEx.OnDayPicker_LostFocus"/>
		/// <seealso cref="DayPickerPopup.Close"/>
		/// <seealso cref="DayPickerPopup.Display"/>
		/// <seealso cref="DayPickerPopup.DisplayYearUpDown"/>
		/// <seealso cref="DayPickerPopup.InitYearUpDown"/>
		/// <seealso cref="DayPickerPopup.MinDate"/>
		/// <seealso cref="DayPickerPopup.MaxDate"/>
		/// <seealso cref="DayPickerPopup.OnMouseDown"/>
		/// <seealso cref="DayPickerPopup.OnYearUpDownValueChanged"/>
		public NumericUpDown nud_YearUpDown;

		#endregion Controls\Components Vars.(ContextMenus,NumericUpDown)
	
		#region DateTime Calculation Vars.

		/// <summary>
		///  <list type="table">
		///   <item><description>Minimum DateTime</description><para>mj10777 CultureInfo support</para></item>
		///   <para>Idea from : Éric Carmichael - eric@westgen.com</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>DateTime.MinValue</para>
		///   </item>
		///  </list>
		///  <para>Supported Value is stored here</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.MinDate"/>
		private DateTime dt_MinDate = new System.DateTime(1753,8,7);

		/// <summary>
		///  <list type="table">
		///   <item><description>Maximum DateTime</description><para>mj10777 CultureInfo support</para></item>
		///   <para>Idea from : Éric Carmichael - eric@westgen.com</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>DateTime.MaxValue</para>
		///   </item>
		///  </list>
		///  <para>Maximum Value is stored here</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.MaxDate"/>
		private DateTime dt_MaxDate = new System.DateTime(9997,8,7);
		
		/// <summary>
		/// In What mode are we viewing the calendar
		/// never assign this member variable directly use
		/// the property instead
		/// don't change this value here in the line below
		/// </summary>
		private bool isGregorian				 = false;

		/// <summary>
		/// The Current Calendar to be used in day calculations !
		/// don't change this value here in the line below
		/// </summary>
		private Calendar curCalendar			 = new EthiopianCalendar();
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Today DateTime</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>DateTime.Today</para>
		///   </item>
		///  </list>
		///  <para>Today Value is stored here</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.DrawTodaySelection"/>
		/// <seealso cref="DayPickerPopup.OnMouseDown"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		/// <seealso cref="DayPickerPopup.UpdateCurSel"/>
		private DateTime dt_Today = DateTime.Today;

		/// <summary>
		///  <list type="table">
		///   <item><description>Hover Selection DateTime</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>DateTime.Today</para>
		///   </item>
		///  </list>
		///  <para>Supported Value is stored here</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.Display"/>
		/// <seealso cref="DayPickerPopup.DrawHoverSelection"/>
		/// <seealso cref="DayPickerPopup.GetDayIndex(DateTime)"/>
		/// <seealso cref="DayPickerPopup.OnMouseUp"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		/// <seealso cref="DayPickerPopup.UpdateCurSel"/>
		/// <seealso cref="DayPickerPopup.UpdateHoverCell"/>
		/// <seealso cref="DayPickerPopup.Value"/>
		private DateTime dt_HoverSel = DateTime.Today;
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Selected DateTime</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>DateTime.Today</para>
		///   </item>
		///  </list>
		///  <para>Selected Value is stored here</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CalculateFirstDate"/>
		/// <seealso cref="DayPickerPopup.DisplayYearUpDown"/>
		/// <seealso cref="DayPickerPopup.Display"/>
		/// <seealso cref="DayPickerPopup.DrawCaption"/>
		/// <seealso cref="DayPickerPopup.DrawDays"/>
		/// <seealso cref="DayPickerPopup.DrawCurSelection"/>
		/// <seealso cref="DayPickerPopup.DrawTodaySelection"/>
		/// <seealso cref="DayPickerPopup.OnMouseDown"/>
		/// <seealso cref="DayPickerPopup.OnMouseUp"/>
		/// <seealso cref="DayPickerPopup.OnKeyDown"/>
		/// <seealso cref="DayPickerPopup.OnMonthMenuClick"/>
		/// <seealso cref="DayPickerPopup.OnYearUpDownValueChanged"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		/// <seealso cref="DayPickerPopup.UpdateCurSel"/>
		/// <seealso cref="DayPickerPopup.Value"/>
		private DateTime dt_CurSel = DateTime.Today;

		/// <summary>
		///  <list type="table">
		///   <item><description>First date in the calendar DateTime</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>nothing</para>
		///   </item>
		///  </list>
		///  <para>Today Value is stored here</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CalculateFirstDate"/>
		/// <seealso cref="DayPickerPopup.CalculateDays"/>
		/// <seealso cref="DayPickerPopup.GetDayIndex"/>
		private DateTime dt_FirstDate;

		/// <summary>
		///  <list type="table">
		///   <item><description>AllowClose</description><para>mj10777 CultureInfo support</para></item>
		///   <para>Idea from : Éric Carmichael - eric@westgen.com</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>true</para>
		///   </item>
		///  </list>
		///  <para>used by Maximum/Minimum DateTime support</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.UpdateCurSel"/>
		/// <seealso cref="DayPickerPopup.Close"/>
		/// <seealso cref="DayPickerPopup.OnLostFocus"/>
		private bool b_AllowClose = true;

		/// <summary>
		///  <list type="table">
		///   <item><description>Capture Mouse</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>false</para>
		///   </item>
		///  </list>
		///  <para>Capturing mouse events (hovering over days)</para>
		///  <para>set to true in OnMouseDown when not over</para>
		///  <para>- Year, Month, Caption and Today</para>
		///  <para>- Left/Right Arrow Button</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.Display"/>
		/// <seealso cref="DayPickerPopup.OnMouseDown"/>
		/// <seealso cref="DayPickerPopup.OnMouseMove"/>
		/// <seealso cref="DayPickerPopup.OnMouseUp"/>
		private bool b_CaptureMouse=false;

		/// <summary>
		/// Flag wheter the mouse button is down on the left button
		/// </summary>
		private bool b_MouseDownOnLeft		=false;

		/// <summary>
		/// Flag wheter the mouse button is down on the right button
		/// </summary>
		private bool b_MouseDownOnRight		=false;

		/// <summary>
		/// Flag wheter the mouse is down on the calendar
		/// toggle button or not
		/// </summary>
		private bool b_MouseDownOnCalendar	=false;

		/// <summary>
		///  <list type="table">
		///   <item><description>Current Month</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>-1</para>
		///   </item>
		///  </list>
		///  <para>Helper Field to remember select Month/Year</para>
		///  <para>- calls CalculateDays in DrawDays if needed</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CalculateDays"/>
		/// <seealso cref="DayPickerPopup.DrawDays"/>
		/// <seealso cref="DayPickerPopup.OnMonthMenuPopup"/>
		private int i_CurMonth = -1;

		/// <summary>
		///  <list type="table">
		///   <item><description>Current Year</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>-1</para>
		///   </item>
		///  </list>
		///  <para>Helper Field to remember select Month/Year</para>
		///  <para>- calls CalculateDays in DrawDays if needed</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CalculateDays"/>
		/// <seealso cref="DayPickerPopup.DrawDays"/>
		private int i_CurYear  = -1;

		/// <summary>
		///  <list type="table">
		///   <item><description>Array[42] Calendar DateTime</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>nothing</para>
		///   </item>
		///  </list>
		///  <para>Cache calendar for better performance</para>
		///  <para>each DateTime structure is only 8 bytes</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.CalculateDays"/>
		/// <seealso cref="DayPickerPopup.DrawDays"/>
		/// <seealso cref="DayPickerPopup.DrawHoverSelection"/>
		/// <seealso cref="DayPickerPopup.DrawTodaySelection"/>
		/// <seealso cref="DayPickerPopup.OnMouseUp"/>
		/// <seealso cref="DayPickerPopup.UpdateHoverCell"/>
		private DateTime[] dta_Days = new DateTime[42];		

		#endregion DateTime Calculation Vars.
	
		#region Events

		// exposed events
		/// <summary>
		///  <list type="table">
		///   <item><description>CloseUp</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Exposed Events</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.Close"/>
		public event EventHandler CloseUp;
	
		/// <summary>
		///  <list type="table">
		///   <item><description>Value Changed</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Exposed Events</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.OnYearUpDownValueChanged"/>
		/// <seealso cref="DayPickerPopup.InitYearUpDown"/>
		/// <seealso cref="DayPickerPopup.OnKeyDown"/>
		/// <seealso cref="DayPickerPopup.UpdateCurSel"/>
		public event EventHandler ValueChanged;

		#endregion Events
		
		#region Properties
	
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets created DayPickerPopup Bitmap</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to retrieve results of DayPickerPopup</para>
		/// </remarks>
		/// <value>Returns Bitmap created by DayPickerPopup</value>
		/// <seealso cref="DayPickerPopup.bmp_Bmp"/>
		[Browsable(false)]
		public System.Drawing.Bitmap BMP
		{
			get
			{
				return this.bmp_Bmp;
			}
		}	

		
		/// <summary>
		/// Flag to keep track if teh current calendar is gregorian.
		/// </summary>
		public bool IsGregorian
		{
			get
			{
				return isGregorian;  
			}
			
			
			set
			{
				if(IsGregorian !=value)
				{
					isGregorian=value;
					Invalidate();
					Update(); 
					
					if(isGregorian)
					{						
						curCalendar=new GregorianCalendar(); 
						s_Today="Today";
					}
					else
					{
						curCalendar=new EthiopianCalendar();
						s_Today="ዛሬ";
					}
					if(this.Parent !=null)
					{
						foreach(Control c in this.Parent.Controls)
						{
							if( c is DateTimePickerEx)
							{
								((DateTimePickerEx)(c)).Invalidate(); 
							}
						}
					}
					
					DayOfWeekCharacters=DayOfWeekCharacters;
				}
			}		
		}

		
		/// <summary>
		/// gets the current calendar used by the pop up calendar
		/// </summary>
		public Calendar CurrentCalendar
		{
			get
			{
				return curCalendar;
			}
		}		
				
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the Day-Color (inactiv Month) SolidBrush Portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override SystemColors.GrayText</para>
		/// </remarks>
		/// <value>Returns set System.Drawing.Color used by the Day-Color (inactiv Month)</value>
		/// <seealso cref="DayPickerPopup.sb_ForeColorInactiveDays"/>
		/// <seealso cref="DateTimePickerEx.ForeColorInactiveDays"/>
		public System.Drawing.Color ForeColorInactiveDays
		{
			get
			{
				return sb_ForeColorInactiveDays.Color;
			}
			
			
			set
			{
				sb_ForeColorInactiveDays = new SolidBrush(value);
			}
		} // public System.Drawing.Color ForeColorInactiveDays
		

		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the Invalid Day-Color (activ Month) SolidBrush Portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override System.Drawing.Color.Red</para>
		/// </remarks>
		/// <value>Returns set System.Drawing.Color used by the Invalid Day-Color (activ Month)</value>
		/// <seealso cref="DayPickerPopup.sb_RedActive"/>
		/// <seealso cref="DateTimePickerEx.RedActive"/>
		public System.Drawing.Color RedActive
		{
			get
			{
				return sb_RedActive.Color;
			}
			
			
			set
			{
				sb_RedActive = new SolidBrush(value);
			}
		} // public System.Drawing.Color RedActive
		
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the Invalid Day-Color (inactiv Month) SolidBrush Portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override System.Drawing.Color.LightSalmon</para>
		/// </remarks>
		/// <value>Returns set System.Drawing.Color used by the Invalid Day-Color (inactiv Month)</value>
		/// <seealso cref="DayPickerPopup.sb_RedInactive"/>
		/// <seealso cref="DateTimePickerEx.RedInactive"/>
		public System.Drawing.Color RedInactive
		{
			get
			{
				return sb_RedInactive.Color;
			}
			
			
			set
			{
				sb_RedInactive = new SolidBrush(value);
			}
		} // public System.Drawing.Color RedInactive
				
				
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the Day-Background SolidBrush Portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override SystemColors.Highlight</para>
		/// </remarks>
		/// <value>Returns set System.Drawing.Color used by the Day-Background</value>
		/// <seealso cref="DayPickerPopup.sb_BackSelect"/>
		/// <seealso cref="DateTimePickerEx.BackSelect"/>
		public System.Drawing.Color BackSelect
		{
			get
			{
				return sb_BackSelect.Color;
			}
			
			
			set
			{
				sb_BackSelect = new SolidBrush(value);
			}
		} // public System.Drawing.Color BackSelect

			
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the Day-Text SolidBrush Portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override SystemColors.HighlightText</para>
		/// </remarks>
		/// <value>Returns set System.Drawing.Color used by the Day-Background</value>
		/// <seealso cref="DayPickerPopup.sb_TextSelect"/>
		/// <seealso cref="DateTimePickerEx.TextSelect"/>
		public System.Drawing.Color TextSelect
		{
			get
			{
				return sb_TextSelect.Color;
			}
			
			
			set
			{
				sb_TextSelect = new SolidBrush(value);
			}
		} // public System.Drawing.Color TextSelect
		

		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the Caption-Background SolidBrush Portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override SystemColors.ActiveCaption</para>
		/// </remarks>
		/// <value>Returns set System.Drawing.Color used by the Caption-Background</value>
		/// <seealso cref="DayPickerPopup.sb_CaptionBack"/>
		/// <seealso cref="DateTimePickerEx.CalendarTitelBackColor"/>
		public System.Drawing.Color CalendarTitelBackColor
		{
			get
			{
				return sb_CaptionBack.Color;
			}
			
			
			set
			{
				sb_CaptionBack = new SolidBrush(value);
			}
		} // public System.Drawing.Color CalendarTitelBackColor
	

		/// <summary>
		/// Gets/Sets the color for the back ground of month
		/// </summary>
		public	System.Drawing.Color CalendarMonthBackground
		{
			get
			{
				return sb_MonthBack.Color; 			
			}
			
			
			set
			{
				sb_MonthBack.Color=value; 
			}
		}
		

		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the Caption-Text SolidBrush Portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override SystemColors.ActiveCaptionText</para>
		/// </remarks>
		/// <value>Returns set System.Drawing.Color used by the Caption-Text</value>
		/// <seealso cref="DayPickerPopup.sb_CaptionBack"/>
		public System.Drawing.Color CalendarTitelForeColor
		{
			get
			{
				return sb_CalendarTitelForeColor.Color;
			}
			
			
			set
			{
				sb_CalendarTitelForeColor = new SolidBrush(value);
			}
		} // public System.Drawing.Color CalendarTitelForeColor
	

		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the CultureInfo for the calendar</description><para>mj10777 CultureInfo support</para></item>
		///   <para>Idea from : Éric Carmichael - eric@westgen.com</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>- Allow User to override Mashine CultureInfo</para>
		/// </remarks>
		/// <value>Returns set CultureInfo used by the DateTimePicker</value>
		/// <seealso cref="DayPickerPopup.s_CustomFormat"/>
		/// <seealso cref="DateTimePicker.CustomFormat"/>
		public string CustomFormat
		{
			get
			{
				return s_CustomFormat;
			}
			
			
			set
			{
				s_CustomFormat = value;
			}
		} // public string DateFormat
				
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets Minimum DateTime that the DateTimePicker will support.</description><para>mj10777 CultureInfo support</para></item>
		///   <para>Idea from : Éric Carmichael - eric@westgen.com</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>DateTime.MinValue</para>
		///   </item>
		///  </list>
		///  <para>This Property is used to :</para>
		///  <para>- To prevent the User on entering a non-disirable Date in the DateTimePicker</para>
		///  <para>- the DateTimePicker will show Date before MinDate and after MaxDate in shades of Red to show that they cannot be picked</para>
		///  <para>- the Month / Year Controls will not reacte if the Month/Year to be shown is not a valid selection</para>
		/// </remarks>
		/// <value>return Minimum Date that is Valid</value>
		/// <seealso cref="DateTimePicker.MinDateTime"/>
		/// <seealso cref="DayPickerPopup.dt_MinDate"/>
		/// <seealso cref="DayPickerPopup.nud_YearUpDown"/>
		/// <seealso cref="DayPickerPopup.DrawDays"/>
		/// <seealso cref="DayPickerPopup.InitYearUpDown"/>
		/// <seealso cref="DayPickerPopup.UpdateCurSel"/>
		internal DateTime MinDate
		{
			get
			{
				return this.dt_MinDate;
			}
			
			
			set
			{
				this.dt_MinDate = new DateTime(value.Year,value.Month,value.Day) ;
				this.nud_YearUpDown.Minimum = value.Year;
			}
		}  // internal DateTime MinDate
		
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets Maximum DateTime that the DateTimePicker will support.</description><para>mj10777 CultureInfo support</para></item>
		///   <para>Idea from : Éric Carmichael - eric@westgen.com</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <list type="bullet">
		///   <item>
		///    <description>Default</description>
		///    <para>DateTime.MaxValue</para>
		///   </item>
		///  </list>
		///  <para>This Property is used to :</para>
		///  <para>- To prevent the User on entering a non-disirable Date in the DateTimePicker</para>
		///  <para>- the DateTimePicker will show Date before MinDate and after MaxDate in shades of Red to show that they cannot be picked</para>
		///  <para>- the Month / Year Controls will not reacte if the Month/Year to be shown is not a valid selection</para>
		/// </remarks>
		/// <value>return Maximal Date that is Valid</value>
		/// <seealso cref="DateTimePicker.MaxDateTime"/>
		/// <seealso cref="DayPickerPopup.dt_MaxDate"/>
		/// <seealso cref="DayPickerPopup.nud_YearUpDown"/>
		/// <seealso cref="DayPickerPopup.DrawDays"/>
		/// <seealso cref="DayPickerPopup.InitYearUpDown"/>
		/// <seealso cref="DayPickerPopup.UpdateCurSel"/>
		internal DateTime MaxDate
		{
			get
			{
				return this.dt_MaxDate;
			}
			
			
			set
			{
				this.dt_MaxDate = new DateTime(value.Year,value.Month,value.Day) ;
				this.nud_YearUpDown.Maximum = value.Year;
			}
		} // internal DateTime MaxDate
				

		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the Selected date</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This Property is used to :</para>
		///  <para>UpdateCurSel: does the validation checking: </para>
		///  <para>- if Value LT MinDate : MinDate will be set</para>
		///  <para>- if Value GT MaxDate : MaxDate will be set</para>
		///  <para>- dt_HoverSel recieves set Value</para>
		/// </remarks>
		/// <value>return Selected Date</value>
		/// <seealso cref="DayPickerPopup.dt_HoverSel"/>
		/// <seealso cref="DayPickerPopup.dt_CurSel"/>
		/// <seealso cref="DayPickerPopup.UpdateCurSel"/>
		public DateTime Value
		{
			get
			{
				return dt_CurSel;
			}
			
			
			
			set
			{
				if (value != dt_CurSel)
					UpdateCurSel(value);
			}
		}
		
						
		/// <summary>
		/// Size of Control
		/// </summary>
		public Rectangle ControlSize
		{
			get
			{
				return rt_ClientSize;
			}
			
			
			set
			{
				rt_ClientSize = value;
			}
		}  // public ControlSize
				
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the font used for the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		/// Does the following :
		///  <list type="bullet">
		///   <item>
		///    <description>All Fonts of Control will be set to this Value</description>
		///    <para>b_Font is set to true to prevent Invalidate when setting other Fonts</para>
		///   </item>
		///  </list>
		///  <list type="bullet">
		///   <item>
		///    <description>Caption</description>
		///    <para>Top Portion of Control with Arrows Month / Year Control</para>
		///   </item>
		///  </list>
		///  <list type="bullet">
		///   <item>
		///    <description>Days</description>
		///    <para>Middle Portion of Control with Days of Week and Days</para>
		///   </item>
		///  </list>
		///  <list type="bullet">
		///   <item>
		///    <description>Today</description>
		///    <para>Bottom Portion of Control with "Today" Test and Date</para>
		///   </item>
		///  </list>
		///  <para>UseFontCaption, FontDay or FontToday to set different Font for each Portion</para>
		/// </remarks>
		/// <value>Standard Font used for all Control Portions</value>
		/// <seealso cref="DayPickerPopup.b_Font"/>
		/// <seealso cref="DayPickerPopup.FontCaption"/>
		/// <seealso cref="DayPickerPopup.FontDay"/>
		/// <seealso cref="DayPickerPopup.FontToday"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			
			
			set
			{
				base.Font   = value;
				// update Font
				b_Font				= true;  // Prevent Invalidate in the following Properties
				FontCaption			= base.Font;
				FontDay				= base.Font;
				FontToday			= base.Font;
				//menuExtender.Font	= base.Font;	//Set the font for the menu extender
				b_Font				= false; // Allow   Invalidate in the Font Properties
				this.Invalidate();
			}
		}  // public Font Font
				
						
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the font used for the Caption portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		/// the following Fields are set :
		///  <list type="bullet">
		///   <item>
		///    <description>ft_FontCaption</description>
		///    <para>Font set by User</para>
		///   </item>
		///  </list>
		///  <list type="bullet">
		///   <item>
		///    <description>fs_FontCaption</description>
		///    <para>Font Style found in ft_FontCaption.Style</para>
		///   </item>
		///  </list>
		///  <list type="bullet">
		///   <item>
		///    <description>s_FontCaption</description>
		///    <para>Font Name found in ft_FontCaption.Name</para>
		///   </item>
		///  </list>
		///  <list type="bullet">
		///   <item>
		///    <description>f_FontSize</description>
		///    <para>Font Size found in ft_FontCaption.Size</para>
		///   </item>
		///  </list>
		///  <para>- Invalidate() is called to repaint the Control (if not called from Property Font)</para>
		///  <para>Use "dtp_Something.Font=ft_Something;" to set Font's for all Portions of Control</para>
		/// </remarks>
		/// <value>Standard Font used for all Caption Control Portions</value>
		/// <seealso cref="DayPickerPopup.b_Font"/>
		/// <seealso cref="DayPickerPopup.Font"/>
		/// <seealso cref="DayPickerPopup.f_FontSize_Caption"/>
		/// <seealso cref="DayPickerPopup.fs_FontStyle_Caption"/>
		/// <seealso cref="DayPickerPopup.ft_FontCaption"/>
		/// <seealso cref="DayPickerPopup.s_FontName_Caption"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		public Font FontCaption
		{
			get
			{
				return ft_FontCaption;
			}
			
			
			set
			{
				// update Font
				ft_FontCaption			= value;
				fs_FontStyle_Caption	= ft_FontCaption.Style;
				s_FontName_Caption		= ft_FontCaption.Name;
				f_FontSize_Caption		= ft_FontCaption.Size;
				
				if (!b_Font) // Do this only when Property Font has not been set
					this.Invalidate();
			}
		}  // public Font FontCaption
		
				
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the font used for the Day portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		/// the following Fields are set :
		///  <list type="bullet">
		///   <item>
		///    <description>ft_FontDay</description>
		///    <para>Font set by User</para>
		///   </item>
		///  </list>
		///  <list type="bullet">
		///   <item>
		///    <description>fs_FontDay</description>
		///    <para>Font Style found in ft_FontDay.Style</para>
		///   </item>
		///  </list>
		///  <list type="bullet">
		///   <item>
		///    <description>s_FontDay</description>
		///    <para>Font Name found in ft_FontDay.Name</para>
		///   </item>
		///  </list>
		///  <list type="bullet">
		///   <item>
		///    <description>f_FontSize</description>
		///    <para>Font Size found in ft_FontDay.Size</para>
		///   </item>
		///  </list>
		///  <para>- Invalidate() is called to repaint the Control</para>
		///  <para>- Invalidate() is called to repaint the Control (if not called from Property Font)</para>
		///  <para>Use "dtp_Something.Font=ft_Something;" to set Font's for all Portions of Control</para>
		/// </remarks>
		/// <value>Standard Font used for all Today Control Portions</value>
		/// <seealso cref="DayPickerPopup.b_Font"/>
		/// <seealso cref="DayPickerPopup.Font"/>
		/// <seealso cref="DayPickerPopup.f_FontSize_Day"/>
		/// <seealso cref="DayPickerPopup.fs_FontStyle_Day"/>
		/// <seealso cref="DayPickerPopup.ft_FontDay"/>
		/// <seealso cref="DayPickerPopup.s_FontName_Day"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		public Font FontDay
		{
			get
			{
				return ft_FontDay;
			}
			
			set
			{
				// update Font
				ft_FontDay       = value;
				fs_FontStyle_Day = ft_FontDay.Style;
				s_FontName_Day  = ft_FontDay.Name;
				f_FontSize_Day  = ft_FontDay.Size;
				if (!b_Font) // Do this only when Property Font has not been set
					this.Invalidate();
			}
		}		
		
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the font used for the Today portion of the Control</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		/// the following Fields are set :
		///  <list type="bullet">
		///   <item>
		///    <description>ft_FontToday</description>
		///    <para>Font set by User</para>
		///   </item>
		///  </list>
		///  <list type="bullet">
		///   <item>
		///    <description>fs_FontToday</description>
		///    <para>Font Style found in ft_FontToday.Style</para>
		///   </item>
		///  </list>
		///  <list type="bullet">
		///   <item>
		///    <description>s_FontToday</description>
		///    <para>Font Name found in ft_FontToday.Name</para>
		///   </item>
		///  </list>
		///  <list type="bullet">
		///   <item>
		///    <description>f_FontSize</description>
		///    <para>Font Size found in ft_FontToday.Size</para>
		///   </item>
		///  </list>
		///  <para>- Invalidate() is called to repaint the Control (if not called from Property Font)</para>
		///  <para>Use "dtp_Something.Font=ft_Something;" to set Font's for all Portions of Control</para>
		/// </remarks>
		/// <value>Standard Font used for all Today Control Portions</value>
		/// <seealso cref="DayPickerPopup.b_Font"/>
		/// <seealso cref="DayPickerPopup.Font"/>
		/// <seealso cref="DayPickerPopup.f_FontSize_Today"/>
		/// <seealso cref="DayPickerPopup.fs_FontStyle_Today"/>
		/// <seealso cref="DayPickerPopup.ft_FontToday"/>
		/// <seealso cref="DayPickerPopup.s_FontName_Today"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		public Font FontToday
		{
			get
			{
				return ft_FontToday;
			}
			
			
			set
			{
				// update Font
				ft_FontToday       = value;
				fs_FontStyle_Today = ft_FontToday.Style;
				s_FontName_Today  = ft_FontToday.Name;
				f_FontSize_Today  = ft_FontToday.Size;
				if (!b_Font) // Do this only when Property Font has not been set
					this.Invalidate();
			}
		}
						
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Character's per Weekday to be shown</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>- Default = 2</para>
		///  <para>This Field is used :</para>
		///  <para>- DayOfWeekCharacters : Set as result of DateTimePicker.DayOfWeekCharacters=value</para>
		///  <para>- FirstDayofWeek : Set as result of sz_NumCols*sz_Size.Width;</para>
		/// </remarks>
		/// <value>Amount of Character's selected for per Weekday to be shown</value>
		/// <seealso cref="DayPickerPopup.DayOfWeekCharacters"/>
		/// <seealso cref="DayPickerPopup.FirstDayOfWeek"/>
		public int DayOfWeekCharacters
		{
			get
			{
				return i_DayOfWeekCharacters;
			}
			
			
			set
			{
				i_DayOfWeekCharacters = OnValidDayOfWeekCharacters(value);
				this.Invalidate();
			}
		}	
		

		/// <summary>
		///  <list type="table">
		///   <item><description>Gets or Sets the FirstDayOfWeek (allows overriding Standard Value)</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>This DayOfWeek is used to :</para>
		///  <para>- CalculateFirstDate :  dt_FirstDate</para>
		/// </remarks>
		/// <value>FirstDayOfWeek being used</value>
		/// <seealso cref="DayPickerPopup.FirstDayOfWeek"/>
		/// <seealso cref="DayPickerPopup.CalculateFirstDate"/>
		public DayOfWeek FirstDayOfWeek
		{
			get
			{
				return dow_FirstDayOfWeek;
			}
			
			
			set
			{
				dow_FirstDayOfWeek     = value;
				// rebuild DaysOfWeek Strings
				DayOfWeekCharacters = DayOfWeekCharacters;
			}
		}  // public DayOfWeek FirstDayOfWeek		
				
		
		#endregion Properties
	
		#region Constructor
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Constructor</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <seealso cref="DateTimePicker"/>
		public DayPickerPopup()
		{
			if (this.TopLevelControl != null)
				ControlSize = this.TopLevelControl.ClientRectangle;
			
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.Selectable,true);  

			// init display properties
			this.Visible	= false;
			this.Location	= new Point(0, 0);
			this.Size		= new Size(i_ControlWidth,pt_BottomLabelsPos.Y+i_BottomLabelHeight+5);
			this.dt_Today	=DateTime.Now;  
			this.dt_CurSel	=dt_Today;
			this.Value		=dt_Today;
			
			DayOfWeekCharacters=DayOfWeekCharacters;  
			
			// Initialize controls that popup when click on the
			// month or year in the caption
			
			InitYearUpDown();
		}
		
		
		#endregion Constructor

		#region Display
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Show or hide the calendar.</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <seealso cref="DateTimePickerEx.OnMouseDown"/>
		/// <param name="visible">true=DayPickerPopup is being displayed</param>
		/// <param name="x">Position Left</param>
		/// <param name="y">Position Top</param>
		/// <param name="backColor">Background Color to be used</param>
		/// <param name="foreColor">Foreground Color to be used</param>
		public void Display(bool visible, int x, int y, Color backColor, Color foreColor)
		{
			if (visible)
			{
				// initialize properties if being displayed
				b_CaptureMouse = false;
				nud_YearUpDown.Hide();

				this.BackColor = backColor;
				this.ForeColor = foreColor;
				this.Left = x;
				this.Top = y;
				this.BringToFront();								
				// default to hovering over the current selection
				dt_HoverSel = dt_CurSel;
			}
			// hide or show the calendar
			this.Visible = visible;		
		}
		
		
		#endregion Display

		#region Drawing Methods
	
		#region OnPaint
	
		/// <summary>
		///  <list type="table">
		///   <item><description>Paint the DateTimePicker</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>mj10777 - resizing logic</para>
		/// </remarks>
		/// <param name="e">PaintEventArgs sent (used for Graphics.DrawImage)</param>
		/// <seealso cref="DayPickerPopup.CalculateFirstDate"/>
		/// <seealso cref="DayPickerPopup.CreateMemoryBitmap"/>
		/// <seealso cref="DayPickerPopup.CreateGdiObjects"/>
		/// <seealso cref="DayPickerPopup.DrawCaption"/>
		/// <seealso cref="DayPickerPopup.DrawDaysOfWeek"/>
		/// <seealso cref="DayPickerPopup.DrawDays"/>
		/// <seealso cref="DayPickerPopup.DrawCurSelection"/>
		/// <seealso cref="DayPickerPopup.DrawTodaySelection"/>
		/// <seealso cref="DayPickerPopup.i_BottomLabelHeight"/>
		/// <seealso cref="DayPickerPopup.i_CaptionHeight"/>
		/// <seealso cref="DayPickerPopup.i_ControlWidth"/>
		/// <seealso cref="DayPickerPopup.pt_BottomLabelsPos"/>
		/// <seealso cref="DayPickerPopup.pt_DayGrids"/>
		/// <seealso cref="DayPickerPopup.s_DayOfWeek"/>
		/// <seealso cref="DayPickerPopup.sz_DaysCell"/>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (this.TopLevelControl != null)
				ControlSize = this.TopLevelControl.ClientRectangle;

			// We must do this twice, the first to Calculate
			// - the first  to Calculate the Width/Height of the Strings 
			// - the second to rebuild the Bitmap if needed to to change size
			CreateMemoryBitmap();

			// - Create Font if needed
			CreateGdiObjects();

			// mj10777 - Resizing Logic
			
			//Get the month and Year text
			s_BottomLabels       = isGregorian ? dt_CurSel.ToString("MMMM,")+ dt_CurSel.ToString(" yyyy"): EthiopianCalendar.ToString(dt_CurSel,"MMMM,")+EthiopianCalendar.ToString(dt_CurSel," yyyy");
			 
			
			//Get the size of the month and year caption
			Size sz_SizeCaption  = gr_Graphics.MeasureString(s_BottomLabels,ft_FontCaption).ToSize();
			//  68 % of Width must be reserved for Month/Year, Calculate if Width is OK
			int i_Caption_Width = (int)(((float)sz_SizeCaption.Width/68)*100);  // Very important to be done this way !
			
			//Show the today string and the date in short date format
			s_BottomLabels  =	s_Today + ": ";
			s_BottomLabels +=	isGregorian ? dt_Today.ToString("d"):EthiopianCalendar.ToString(dt_Today,"d");
						
			//Get the size of the today portion of the date to be shown
			Size sz_SizeToday    = gr_Graphics.MeasureString(s_BottomLabels ,ft_FontToday).ToSize();
			sz_SizeToday.Width  += pt_BottomLabelsPos.X; // StartPosition for String
			sz_SizeToday.Width  +=sz_DaysCell.Width;
			//sz_SizeToday.Width  +=gr_Graphics.MeasureString("ZZ" ,ft_FontToday).ToSize().Width;
			
			//
			//	TODO: size of bottom label must consider the size for toggle button
			//

			
			//Get the size for the Sun,Mon,Tue, will take ! (make a constant size for all dates)
			Size sz_SizeControl  = gr_Graphics.MeasureString("ZZ",ft_FontDay).ToSize();
			for (int i=0;i<s_DayOfWeek.Length;i++)
			{
				Size totalSize = gr_Graphics.MeasureString(s_DayOfWeek[i],ft_FontDay).ToSize();
				if (totalSize.Height > sz_SizeControl.Height)
					sz_SizeControl.Height = totalSize.Height;	//Standardize the size
				if (totalSize.Width > sz_SizeControl.Width)
					sz_SizeControl.Width = totalSize.Width;		//Standardize the size
			}


			//------------------------------------------------------------------------------------------
			// Calculate which Height is the Heigher and store in sz_SizeControl.Height
			if (sz_SizeCaption.Height > sz_SizeControl.Height)
				sz_SizeControl.Height = sz_SizeCaption.Height;
			if (sz_SizeToday.Height > sz_SizeControl.Height)
				sz_SizeControl.Height = sz_SizeToday.Height;
			
			// Calculate which is the Wider   and store in sz_SizeControl.Width  
			sz_SizeCaption.Width = i_Caption_Width/s_DayOfWeek.Length;
			
			
			if (sz_SizeCaption.Width > sz_SizeControl.Width)
			{ //-- The final Width is the Widest of whatever was wider
				sz_SizeControl.Width = sz_SizeCaption.Width;
			}
			sz_SizeToday.Width = sz_SizeToday.Width/s_DayOfWeek.Length;
			
			if (sz_SizeToday.Width > sz_SizeControl.Width)
			{ //-- The final Width is the Widest of whatever was wider
				sz_SizeControl.Width = sz_SizeToday.Width;
			}
			
			// Give an extra Pixel due to rounding errors !
			sz_SizeControl.Width++;
			
			//-- we now have the final Height and Width of the Control (I hope)
			//-- DaysOfWeek, Days and BottomLable have the same Height and Width
			i_BottomLabelHeight  = sz_SizeControl.Height;
			sz_DaysCell          = sz_SizeControl;
			
			//-- The Caption is Heigher that the others //Adjusted by leyu sisay
			i_CaptionHeight      = (sz_SizeControl.Height*3)/2 > Const.CaptionHeight ? (sz_SizeControl.Height*3)/2:Const.CaptionHeight;
			
			//-- The final Width ist the Widest of whatever was wider
			i_ControlWidth       = i_NumCols*sz_SizeControl.Width;
			
			// We need some space left of the DaysOfWeek and Days (same as sz_ArrowButtonOffset.Width)
			pt_DayGrids.X        = (i_ControlWidth/30);
			i_ControlWidth      += pt_DayGrids.X;
			
			// TODO : Check size for arrows
			pt_DayGrids.Y        = i_CaptionHeight;
			//-- The BottomLabel starts after the Caption, DaysOfWeek and Days 
			pt_BottomLabelsPos.Y = i_CaptionHeight+(sz_SizeControl.Height*(i_NumRows+1));
			//-- Add a bit to make it look nice
			pt_BottomLabelsPos.Y += 7;
			//-- Final Height of Control, plus some space to make the Today Ellipse fit
			i_ControlHeight      = pt_BottomLabelsPos.Y+i_BottomLabelHeight+(i_BottomLabelHeight/3);
			//--------------------------------------------------------------------
			//-- Set Control Width and Height
			this.Size            = new Size(i_ControlWidth,i_ControlHeight);
			
			//This Will do the repositioning
			if ((this.Top+this.Size.Height) > this.TopLevelControl.ClientRectangle.Height)
			{  // The Control must be repositioned so that the Bottom will fit the Screen
				if ((this.TopLevelControl.ClientRectangle.Height-this.Size.Height) > 0)
				{ // The Control Bottom will be set to the Bottom of the Screen
					this.Top = this.TopLevelControl.ClientRectangle.Height-this.Size.Height;
				}
				else
				{ // The Control is to big, set to Top and show as mush as possible
					this.Top = 0;
				}
			}  // if ((this.Top+this.Size.Height) > this.TopLevelControl.ClientRectangle.Height)
			if ((this.Left+this.Size.Width) > this.TopLevelControl.ClientRectangle.Width)
			{  // The Control must be repositioned so that the Right side will fit the Screen
				if ((this.TopLevelControl.ClientRectangle.Width-this.Size.Width) > 0)
				{ // The Control Left will be set to the Left of the Screen
					this.Left = this.TopLevelControl.ClientRectangle.Width-this.Size.Width;
				}
				else
				{ // The Control is to wide, set to Left and show as mush as possible
					this.Left = 0;
				}
			}  // if ((this.Left+this.Size.Width) > this.TopLevelControl.ClientRectangle.Width)
			
			
			//--------------------------------------------------------------------
			// Calculate the Arrow Button size and Positions
			//-- Arrow Offset.Width  is 2% of the Control Width
			sz_ArrowButtonOffset.Width  = pt_DayGrids.X; 
			//-- Arrow Offset.Height is 25% of the Caption Height (stored in sz_SizeControl.Height) 
			sz_ArrowButtonOffset.Height = i_CaptionHeight/4;
			//-- Arrow Size.Height   is 4/6 of the Caption Height (stored in sz_SizeControl.Height) 
			sz_ArrowButtonSize.Height   = (i_CaptionHeight/2);
			//-- Arrow Size.Width    is one third Wider than the Height (better Arrow Drawing)
			sz_ArrowButtonSize.Width    = (int)((float)((sz_ArrowButtonSize.Height/3)*4))+5; 
			// end - mj10777 - Resizing Logic
			//--------------------------------------------------------------------
		
			// draw to memory bitmap with correct Sizes and Positions
			CreateMemoryBitmap();

			// calculate the first date in the days grid, this is used
			// to draw the previous month days, the current month days,
			// and any days in the next month
			CalculateFirstDate();

			// init the background
			gr_Graphics.Clear(CalendarMonthBackground);

			// draw elements of the calendar
			// the caption and days of week
			DrawCaption(gr_Graphics);
			DrawDaysOfWeek(gr_Graphics);
			
			// the days grid and different selections
			DrawDays(gr_Graphics);
			// Current Selection is Painted
			DrawCurSelection(gr_Graphics);
			DrawHoverSelection(gr_Graphics, dt_HoverSel, true);
			DrawTodaySelection(gr_Graphics);

			// the today label at the bottom
			DrawBottomLabels(gr_Graphics);
			
			// frame around the control
			gr_Graphics.DrawRectangle(pen_Frame,0,0,this.Width-1,this.Height-1);

			// blit memory bitmap to screen
			e.Graphics.DrawImage(bmp_Bmp, 0, 0);
		} 
	
		
		#endregion OnPaint
	
		#region OnPaintBackground
	
		/// <summary>
		///  <list type="table">
		///   <item><description>Paint Background</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Avoid flickering</para>
		/// </remarks>
		/// <param name="e">PaintEventArgs sent (not used)</param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			// don't pass to base since we paint everything, avoid flashing
		} // protected override void OnPaintBackground(PaintEventArgs e)
			
		#endregion OnPaintBackground
	
		#region DrawCaption
	
		/// <summary>
		///  <list type="table">
		///   <item><description>Draw caption</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>current month, current year, left and right arrow buttons.</para>
		/// </remarks>
		/// <param name="g">Graphics to be used (created in CreateMemoryBitmap)</param>
		/// <seealso cref="DayPickerPopup.i_CaptionHeight"/>
		/// <seealso cref="DayPickerPopup.dt_CurSel"/>
		/// <seealso cref="DayPickerPopup.br_Backround"/>
		/// <seealso cref="DayPickerPopup.sb_CaptionBack"/>
		/// <seealso cref="DayPickerPopup.sb_CalendarTitelForeColor"/>
		/// <seealso cref="DayPickerPopup.sb_Frame"/>
		/// <seealso cref="DayPickerPopup.ft_FontCaption"/>
		/// <seealso cref="DayPickerPopup.cm_MonthMenu"/>
		/// <seealso cref="DayPickerPopup.pen_Frame"/>
		/// <seealso cref="DayPickerPopup.rt_LeftButton"/>
		/// <seealso cref="DayPickerPopup.rt_RightButton"/>
		/// <seealso cref="DayPickerPopup.pta_RightArrowPoints"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		private void DrawCaption(Graphics g)
		{
			// back area
			g.FillRectangle(sb_CaptionBack,0,0,this.Width,i_CaptionHeight);

			// draw the caption centered in the area
			// mj10777 : Use Monthname as used in CultureInfo
			string text = isGregorian ? dt_CurSel.ToString("MMMM,")+ dt_CurSel.ToString(" yyyy"): EthiopianCalendar.ToString(dt_CurSel,"MMMM,")+EthiopianCalendar.ToString(dt_CurSel," yyyy");

			Size totalSize = g.MeasureString(text,ft_FontCaption).ToSize();
		
			int x = (this.Width - totalSize.Width) / 2;
			int y = (i_CaptionHeight - totalSize.Height) / 2;
			
			g.DrawString(text,ft_FontCaption,sb_CalendarTitelForeColor,x,y);

			// calculate the bounding rectangle for each element (the
			// month and year) so we can detect if the user clicked on
			// either element later
			// calculate the month bounding rectangle
			text = isGregorian ? dt_CurSel.ToString("MMMM,") : EthiopianCalendar.ToString(dt_CurSel,"MMMM,");
			Size size = g.MeasureString(text,ft_FontCaption).ToSize();
			rt_Month.X = x;
			rt_Month.Y = y;
			rt_Month.Width = size.Width;
			rt_Month.Height = size.Height;

			// calculate the year bounding rectangle
			text = isGregorian ? dt_CurSel.ToString(" yyyy") : EthiopianCalendar.ToString(dt_CurSel," yyyy");
			size = g.MeasureString(text,ft_FontCaption).ToSize();
			rt_Year.X = x + totalSize.Width - size.Width;
			rt_Year.Y = y;
			rt_Year.Width = size.Width;
			rt_Year.Height = size.Height;

			ThemedDrawing.DrawButton(g,rt_LeftButton ,b_MouseDownOnLeft  ,pta_LeftArrowPoints);
			ThemedDrawing.DrawButton(g,rt_RightButton,b_MouseDownOnRight ,pta_RightArrowPoints);
		}
	
		
		#endregion DrawCaption
	
		#region DrawDaysOfWeek
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Draw days of week header</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Position Day in the middle of the reserved space</para>
		/// </remarks>
		/// <param name="g">Graphics to be used (created in CreateMemoryBitmap)</param>
		/// <seealso cref="DayPickerPopup.sb_CaptionBack"/>
		/// <seealso cref="DayPickerPopup.i_CaptionHeight"/>
		/// <seealso cref="DayPickerPopup.pt_DayGrids"/>
		/// <seealso cref="DayPickerPopup.s_DayOfWeek"/>
		/// <seealso cref="DayPickerPopup.sz_DaysCell"/>
		/// <seealso cref="DayPickerPopup.ft_FontDay"/>
		/// <seealso cref="DayPickerPopup.pen_Frame"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		private void DrawDaysOfWeek(Graphics g)
		{		
			// calculate where to draw days of week
			// Position Day in the middle of the reserved space
			Point pos = new Point(pt_DayGrids.X,i_CaptionHeight);
			// go through and draw each character
			// Set Day to the middle of the reserved space
			Size sz_SizeDay;
			int i_Position = 0;
			for (int i=0;i<s_DayOfWeek.Length;i++)//Sun Mon Tue Wed Thu Fri Sat
			{
				sz_SizeDay = gr_Graphics.MeasureString(s_DayOfWeek[i],ft_FontDay).ToSize();
				i_Position = (sz_DaysCell.Width-sz_SizeDay.Width)/2;
				g.DrawString(s_DayOfWeek[i],ft_FontDay,br_Foreground,pos.X+i_Position,pos.Y);
				pos.X += sz_DaysCell.Width;
			}
			// separator line
			pt_DayGrids.Y += sz_DaysCell.Height;
			g.DrawLine(pen_Frame,pt_DayGrids.X,pt_DayGrids.Y-1,this.Width - pt_DayGrids.X,pt_DayGrids.Y-1);
		} // private void DrawDaysOfWeek(Graphics g)
		
		
		#endregion DrawDaysOfWeek
	
		#region DrawDays
	
		/// <summary>
		///  <list type="table">
		///   <item><description>Draw days in the grid</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Recalculate and cache days if the month or year changed.</para>
		///  <para>Position Day in the middle of the reserved space</para>
		/// </remarks>
		/// <param name="g">Graphics to be used (created in CreateMemoryBitmap)</param>
		/// <seealso cref="DayPickerPopup.br_Foreground"/>
		/// <seealso cref="DayPickerPopup.sb_RedActive"/>
		/// <seealso cref="DayPickerPopup.sb_ForeColorInactiveDays"/>
		/// <seealso cref="DayPickerPopup.sb_RedInactive"/>
		/// <seealso cref="DayPickerPopup.CalculateDays"/>
		/// <seealso cref="DayPickerPopup.dt_CurSel"/>
		/// <seealso cref="DayPickerPopup.i_CurMonth"/>
		/// <seealso cref="DayPickerPopup.i_CurYear"/>
		/// <seealso cref="DayPickerPopup.pt_DayGrids"/>
		/// <seealso cref="DayPickerPopup.sz_DaysCell"/>
		/// <seealso cref="DayPickerPopup.dta_Days"/>
		/// <seealso cref="DayPickerPopup.ft_FontDay"/>
		/// <seealso cref="DayPickerPopup.dt_MaxDate"/>
		/// <seealso cref="DayPickerPopup.dt_MinDate"/>
		/// <seealso cref="DayPickerPopup.i_NumRows"/>
		/// <seealso cref="DayPickerPopup.i_NumCols"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		private void DrawDays(Graphics g)
		{
			// see if need to calculate new set of days (Any Calendar works here)
			if (curCalendar.GetMonth(dt_CurSel) != i_CurMonth || curCalendar.GetYear(dt_CurSel) != i_CurYear)
			{
				// the month or year changed, calculate and cache new set of days
				CalculateDays();
				i_CurMonth	=	curCalendar.GetMonth(dt_CurSel);
				i_CurYear	=	curCalendar.GetYear	(dt_CurSel);
			}
		
			// starting point of grid
		
			// Point pos = pt_DayGrids;
			Point pos = new Point(pt_DayGrids.X,pt_DayGrids.Y);
			
			// any extra pixels (used for single digit numbers)
			// int extra;
			// Set Date to the middle of the reserved space
			Size sz_SizeDay;
			int i_Position = 0;
			// loop through and draw each day in the grid
			for (int y=0;y<i_NumRows;y++)
			{
				for (int x=0;x<i_NumCols;x++)
				{
					// get the date from the cache
					DateTime dt_Display = dta_Days[(y*7)+x];			//This is the datetime to display
					
					int day= curCalendar.GetDayOfMonth(dt_Display);
										
					sz_SizeDay = gr_Graphics.MeasureString(day.ToString(),ft_FontDay).ToSize();
					i_Position = (sz_DaysCell.Width-sz_SizeDay.Width)/2;
					if ((dt_Display > this.MaxDate) || (dt_Display < this.MinDate))		//Check date time to display is with in bounds
					{
						g.DrawString(day.ToString(),
							ft_FontDay,
							(curCalendar.GetMonth(dt_Display) == i_CurMonth)
							? sb_RedActive : sb_RedInactive,pos.X+i_Position,pos.Y);
					}
					else	//Wow selecting this date is not in the valid range man !!!!!
					{
						g.DrawString(day.ToString(),
							ft_FontDay,
							(curCalendar.GetMonth(dt_Display) == i_CurMonth)
							? br_Foreground : sb_ForeColorInactiveDays,pos.X+i_Position,pos.Y);
					}
					// update position within the grid
					pos.X += sz_DaysCell.Width;
				}
				// update position within the grid
				pos.X = pt_DayGrids.X;
				pos.Y += sz_DaysCell.Height + 1;
			}
		}
		
		
		#endregion DrawDays
	
		#region DrawDay
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Draw the specified day</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <param name="g">Graphics to be used (created in OnKeyUp)</param>
		/// <param name="day">Day to be Painted</param>
		/// <param name="selected">true=new selected day ; false=former selected day</param>
		/// <seealso cref="DayPickerPopup.br_Backround"/>
		/// <seealso cref="DayPickerPopup.br_Foreground"/>
		/// <seealso cref="DayPickerPopup.sb_BackSelect"/>
		/// <seealso cref="DayPickerPopup.sb_TextSelect"/>
		/// <seealso cref="DayPickerPopup.sz_DaysCell"/>
		/// <seealso cref="DayPickerPopup.ft_FontDay"/>
		/// <seealso cref="DayPickerPopup.GetDayCellPosition"/>
		/// <seealso cref="DayPickerPopup.GetDayIndex(DateTime)"/>
		/// <seealso cref="DayPickerPopup.OnKeyDown"/>
		private void DrawDay(Graphics g, DateTime day, bool selected)
		{		
			// get the position of this cell in the grid
			int index = GetDayIndex(day);
			Point pos = GetDayCellPosition(index);

			//Get the date in the required calendar.
			int date=curCalendar.GetDayOfMonth(day);

			// cell background
			g.FillRectangle(selected ? sb_BackSelect : br_Backround ,
				pos.X-5,
				pos.Y,
				sz_DaysCell.Width,
				sz_DaysCell.Height);

			// extra space if single digit
			Size sz_SizeDay  = gr_Graphics.MeasureString(date.ToString(),ft_FontDay).ToSize();
			int i_Position = (sz_DaysCell.Width-sz_SizeDay .Width)/2;
			pos.X += i_Position;
			// the day
			g.DrawString(date.ToString(),
				ft_FontDay,selected ? sb_TextSelect : br_Foreground,
				pos.X,
				pos.Y);
		
		}		
		
		
		#endregion DrawDay
	
		#region DrawCurSelection

		/// <summary>
		///  <list type="table">
		///   <item><description>Draw the currently selected day</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Position Day in the middle of the reserved space</para>
		///  <para>replaced FillRectangle with FillEllipse</para>
		/// </remarks>
		/// <param name="g">Graphics to be used (created in CreateMemoryBitmap)</param>
		/// <seealso cref="DayPickerPopup.sb_BackSelect"/>
		/// <seealso cref="DayPickerPopup.sb_TextSelect"/>
		/// <seealso cref="DayPickerPopup.dt_CurSel"/>
		/// <seealso cref="DayPickerPopup.pt_DayGrids"/>
		/// <seealso cref="DayPickerPopup.sz_DaysCell"/>
		/// <seealso cref="DayPickerPopup.ft_FontDay"/>
		/// <seealso cref="DayPickerPopup.GetDayCellPosition"/>
		/// <seealso cref="DayPickerPopup.GetDayIndex(DateTime)"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		private void DrawCurSelection(Graphics g)
		{
			// calculate the coordinates of the current cell
			int index = GetDayIndex(dt_CurSel);
			Point pos = GetDayCellPosition(index);
			
			//Get the day on the correct calendar
			int day=curCalendar.GetDayOfMonth(dt_CurSel);  
			
			// Set Day to the middle of the reserved space
			Size sz_SizeDay = gr_Graphics.MeasureString(day.ToString(),ft_FontDay).ToSize();

			int i_Position = (sz_DaysCell.Width-sz_SizeDay.Width)/2;
			// background
			Rectangle rt_Position = new Rectangle(pos.X-(pt_DayGrids.X/4),pos.Y,sz_DaysCell.Width,sz_DaysCell.Height);
			if ((sqt_Squibble == SquibbleType.Squibble) || (sqt_Squibble == SquibbleType.Ellipse))
			{
				if (sqt_Squibble == SquibbleType.Squibble)
				{  // Polygon
					// gr_Graphics.FillEllipse(sb_BackSelect,pos.X-(pt_DayGrids.X/4),pos.Y,sz_DaysCell.Width,sz_DaysCell.Height);
					gr_Graphics.FillEllipse(sb_BackSelect,rt_Position);
				} // if (sqt_Squibble == SquibbleType.Squibble)
				if (sqt_Squibble == SquibbleType.Ellipse)
				{
					// gr_Graphics.FillEllipse(sb_BackSelect,pos.X-(pt_DayGrids.X/4),pos.Y,sz_DaysCell.Width,sz_DaysCell.Height);
					gr_Graphics.FillEllipse(sb_BackSelect,rt_Position);
				} // if (sqt_Squibble == SquibbleType.Ellipse)
			}  // if ((sqt_Squibble == SquibbleType.Squibble) || (sqt_Squibble == SquibbleType.Ellipse))
			if (sqt_Squibble == SquibbleType.Rectangle)
			{
				// gr_Graphics.FillRectangle(sb_BackSelect,pos.X-5,pos.Y,sz_DaysCell.Width,sz_DaysCell.Height);
				gr_Graphics.FillRectangle(sb_BackSelect,rt_Position);
			} // if (sqt_Squibble == SquibbleType.Rectangle)
			// the day
			gr_Graphics.DrawString(day.ToString(),ft_FontDay,sb_TextSelect,pos.X+i_Position,pos.Y);
		}
		
		
		#endregion DrawCurSelection
	
		#region DrawHoverSelection
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Draws of erases the hover selection box</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <param name="g">Graphics to be used (created in CreateMemoryBitmap, UpdateHoverCell)</param>
		/// <param name="date">Hovering over this Date</param>
		/// <param name="draw">draw or erase the hover selection</param>
		/// <seealso cref="DayPickerPopup.pt_DayGrids"/>
		/// <seealso cref="DayPickerPopup.sz_DaysCell"/>
		/// <seealso cref="DayPickerPopup.dta_Days"/>
		/// <seealso cref="DayPickerPopup.GetDayCellPosition"/>
		/// <seealso cref="DayPickerPopup.GetDayIndex(DateTime)"/>
		/// <seealso cref="DayPickerPopup.pn_Background"/>
		/// <seealso cref="DayPickerPopup.pen_HoverBox"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		/// <seealso cref="DayPickerPopup.UpdateHoverCell(Int32)"/>
		private void DrawHoverSelection(Graphics g, DateTime date, bool draw)
		{
			// see if hovering over a cell, return right away
			// if outside of the grid area
			int index = GetDayIndex(date);
			if (index < 0 || index >= dta_Days.Length)
				return;
			// get the coordinates of cell
			Point pos = GetDayCellPosition(index);
			pos.X += pt_DayGrids.X/4;
			// draw or erase the hover selection
			Rectangle rt_Position = new Rectangle(pos.X-(pt_DayGrids.X/2),pos.Y,sz_DaysCell.Width,sz_DaysCell.Height);
			if ((sqt_Squibble == SquibbleType.Squibble) || (sqt_Squibble == SquibbleType.Ellipse))
			{
				if (sqt_Squibble == SquibbleType.Squibble)
				{
					// g.DrawEllipse(draw ? pen_HoverBox : pn_Background,pos.X-(pt_DayGrids.X/2),pos.Y,sz_DaysCell.Width,sz_DaysCell.Height);
					g.DrawEllipse(draw ? pen_HoverBox : pn_Background,rt_Position);
				} // if (sqt_Squibble == SquibbleType.Squibble)
				if (sqt_Squibble == SquibbleType.Ellipse)
				{
					// g.DrawEllipse(draw ? pen_HoverBox : pn_Background,pos.X-(pt_DayGrids.X/2),pos.Y,sz_DaysCell.Width,sz_DaysCell.Height);
					g.DrawEllipse(draw ? pen_HoverBox : pn_Background,rt_Position);
				} // if (sqt_Squibble == SquibbleType.Ellipse)
			}  // if ((sqt_Squibble == SquibbleType.Squibble) || (sqt_Squibble == SquibbleType.Ellipse))
			if (sqt_Squibble == SquibbleType.Rectangle)
			{
				// g.DrawRectangle(draw ? pen_HoverBox : pn_Background,pos.X-(pt_DayGrids.X/2),pos.Y,sz_DaysCell.Width,sz_DaysCell.Height);
				g.DrawRectangle(draw ? pen_HoverBox : pn_Background,rt_Position);
			} // if (sqt_Squibble == SquibbleType.Rectangle)
		}
		
		#endregion DrawHoverSelection
	
		#region DrawTodaySelection
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Draw Ellipse around today Date on grid</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>The black Box has been replaced with a red Ellipse</para>
		/// </remarks>
		/// <param name="g">Graphics to be used (created in CreateMemoryBitmap, UpdateHoverCell)</param>
		/// <seealso cref="DayPickerPopup.dt_CurSel"/>
		/// <seealso cref="DayPickerPopup.pt_DayGrids"/>
		/// <seealso cref="DayPickerPopup.sz_DaysCell"/>
		/// <seealso cref="DayPickerPopup.GetDayCellPosition"/>
		/// <seealso cref="DayPickerPopup.GetDayIndex(DateTime)"/>
		/// <seealso cref="DayPickerPopup.dta_Days"/>
		/// <seealso cref="DayPickerPopup.pen_Frame"/>
		/// <seealso cref="DayPickerPopup.dt_Today"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		/// <seealso cref="DayPickerPopup.UpdateHoverCell(Int32)"/>
		private void DrawTodaySelection(Graphics g)
		{
			// see if today is visible in the current grid
			int index = GetDayIndex(dt_Today);
			if (index < 0 || index >= dta_Days.Length)
				return;
			// only draw on current month
			if (curCalendar.GetMonth(dt_Today) != curCalendar.GetMonth(dt_CurSel))
				return;
			// today is visible, draw box around cell
			Point pos = GetDayCellPosition(index);
			pos.X += pt_DayGrids.X/4;
			Rectangle rt_Position = new Rectangle(pos.X,pos.Y,(sz_DaysCell.Width-(pt_DayGrids.X/2)),sz_DaysCell.Height);
			if ((sqt_Squibble == SquibbleType.Squibble) || (sqt_Squibble == SquibbleType.Ellipse))
			{
				if (sqt_Squibble == SquibbleType.Squibble)
				{
					// g.DrawEllipse(pen_Today,rt_Position);
					// rt_Position.X -= 1; rt_Position.Y += 1;
					// g.DrawEllipse(pen_Today,rt_Position);
					OnPaintSquibble(pen_Today,g,ref rt_Position);
				} // if (sqt_Squibble == SquibbleType.Squibble)
				if (sqt_Squibble == SquibbleType.Ellipse)
				{
					// g.DrawEllipse(pen_Today,pos.X,pos.Y,(sz_DaysCell.Width-(pt_DayGrids.X/2)),sz_DaysCell.Height);
					// g.DrawEllipse(pen_Today,pos.X-1,pos.Y+1,(sz_DaysCell.Width-(pt_DayGrids.X/2)),sz_DaysCell.Height);
					g.DrawEllipse(pen_Today,rt_Position);
					rt_Position.X -= 1; rt_Position.Y += 1;
					g.DrawEllipse(pen_Today,rt_Position);
				} // if (sqt_Squibble == SquibbleType.Ellipse)
			}  // if ((sqt_Squibble == SquibbleType.Squibble) || (sqt_Squibble == SquibbleType.Ellipse))
			if (sqt_Squibble == SquibbleType.Rectangle)
			{
				// g.DrawRectangle(pen_Frame,pos.X,pos.Y,sz_DaysCell.Width,sz_DaysCell.Height);
				// g.DrawRectangle(pen_Frame,pos.X-1,pos.Y+1,sz_DaysCell.Width,sz_DaysCell.Height);
				g.DrawRectangle(pen_Frame,rt_Position);
				rt_Position.X -= 1; rt_Position.Y += 1;
				g.DrawRectangle(pen_Frame,rt_Position);
			} // if (sqt_Squibble == SquibbleType.Rectangle)
		}
		
		
		#endregion DrawTodaySelection
	
		#region DrawBottomLabel
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Draw the today label at bottom of calendar</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Position Day in the middle of the reserved space</para>
		///  <para>Draw an Ellipse around the Today Text</para>
		/// </remarks>
		/// <param name="g">Graphics to be used (created in CreateMemoryBitmap)</param>
		/// <seealso cref="DayPickerPopup.s_BottomLabels"/>
		/// <seealso cref="DayPickerPopup.pt_BottomLabelsPos"/>
		/// <seealso cref="DayPickerPopup.br_Foreground"/>
		/// <seealso cref="DayPickerPopup.ft_FontToday"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		/// <example>
		///  <code>
		/// 
		/// // Original DateTimePicker.cs (Version 08.04.2003)
		/// 
		///private void DrawBottomLabels(Graphics g)
		///{
		/// // draw today string, don't store bounding rectangle since
		/// // hit testing is the entire width of the calendar
		/// string text = string.Format("Today: {0}",dt_Today.ToShortDateString());
		/// g.DrawString(text, this.m_fontCaption,this.br_Foreground,
		///   Const.BottomLabelsPos.X, Const.BottomLabelsPos.Y);
		///}
		///  </code>
		/// </example>
		private void DrawBottomLabels(Graphics g)
		{
			// mj10777 CultureInfo Support
			// draw today string, don't store bounding rectangle since
			// hit testing is the entire width of the calendar
			// Insure Date is Formatted to select CultureInfo
			// Position Day in the middle of the reserved space
			Size sz_SizeDay  = gr_Graphics.MeasureString(s_BottomLabels,ft_FontToday).ToSize();
			int i_Position = pt_BottomLabelsPos.X +pt_DayGrids.X;//   (this.Width-sz_SizeDay.Width)/2;
			
			// Draw an Ellipse or Squibble around the Today Text, nothing by Rectangle
			if ((sqt_Squibble == SquibbleType.Squibble) || (sqt_Squibble == SquibbleType.Ellipse))
			{
				Rectangle rt_Position = new Rectangle(	i_Position,
					pt_BottomLabelsPos.Y,
					sz_DaysCell.Width,
					sz_DaysCell.Height);
				

				if (sqt_Squibble == SquibbleType.Squibble)
				{
					OnPaintSquibble(pen_Today,g,ref rt_Position);
					g.DrawString(s_BottomLabels,this.ft_FontToday,this.br_Foreground,i_Position + rt_Position.Width,pt_BottomLabelsPos.Y);
				}
				if (sqt_Squibble == SquibbleType.Ellipse)
				{
					g.DrawEllipse(pen_Today,rt_Position);
					rt_Position.X -= 1; rt_Position.Y += 1;
					g.DrawEllipse(pen_Today,rt_Position);
					g.DrawString(s_BottomLabels,this.ft_FontToday,this.br_Foreground,i_Position + rt_Position.Width,pt_BottomLabelsPos.Y);
				}
			}  // if ((sqt_Squibble == SquibbleType.Squibble) || (sqt_Squibble == SquibbleType.Ellipse))
			if (sqt_Squibble == SquibbleType.Rectangle)
			{
				g.DrawString(s_BottomLabels,this.ft_FontToday,this.br_Foreground,i_Position,pt_BottomLabelsPos.Y);
			} // if (sqt_Squibble == SquibbleType.Rectangle)

			//Get the area to enable user to select bet ween ethiopian
			//and gregorian calendar

			rt_Calendar=new Rectangle(this.Width - sz_DaysCell.Width -pt_DayGrids.X,
				pt_BottomLabelsPos.Y,
				sz_DaysCell.Width,
				sz_DaysCell.Height);

			StringFormat  strFormat	= new StringFormat();
			strFormat.LineAlignment = StringAlignment.Center;
			strFormat.Alignment		= StringAlignment.Center;

			string text = IsGregorian ? "E" : "G";	//Toggle the text that would be seen

			ThemedDrawing.DrawButton(g,rt_Calendar,b_MouseDownOnCalendar,text,FontToday,strFormat);
		}

		
		#endregion DrawBottomLabels
	
		#endregion Drawing Methods

		#region Events
	
		#region OnMouseDown
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Determine what area was taped (clicked)</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>take the appropriate action</para>
		///  <para>If no items were taped, see if should start tracking mouse</para>
		/// </remarks>
		/// <param name="e">MouseEventArgs sent (used for Default MouseEvent)</param>
		/// <seealso cref="DayPickerPopup.b_CaptureMouse"/>
		/// <seealso cref="DayPickerPopup.pt_BottomLabelsPos"/>
		/// <seealso cref="DayPickerPopup.DisplayMonthMenu"/>
		/// <seealso cref="DayPickerPopup.DisplayYearUpDown"/>
		/// <seealso cref="DayPickerPopup.rt_LeftButton"/>
		/// <seealso cref="DayPickerPopup.rt_Month"/>
		/// <seealso cref="DayPickerPopup.rt_Year"/>
		/// <seealso cref="DayPickerPopup.nud_YearUpDown"/>
		/// <seealso cref="DayPickerPopup.OnYearUpDownValueChanged"/>
		/// <seealso cref="DayPickerPopup.UpdateCurSel"/>
		/// <seealso cref="DayPickerPopup.UpdateHoverCell(Int32,Int32)"/>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			//base.OnMouseDown(e);

			// see if should hide the year updown control
			if (nud_YearUpDown.Visible)
			{
				if (!nud_YearUpDown.Bounds.Contains(e.X, e.Y))
				{
					// user clicked outside of updown control,
					// update grid with the new year specified
					// in the updown control
					OnYearUpDownValueChanged(null, EventArgs.Empty);
					nud_YearUpDown.Hide();
					this.Focus();
				}
			}
			// left arrow button
			if (rt_LeftButton.Contains(e.X, e.Y))
			{
				b_MouseDownOnLeft =true; 
				// display previous month
				UpdateCurSel(curCalendar.AddMonths(dt_CurSel,-1));
				return;
			}
			// right arrow button
			if (rt_RightButton.Contains(e.X, e.Y))
			{
				b_MouseDownOnRight =true; 
				// display the next month
				UpdateCurSel(curCalendar.AddMonths(dt_CurSel,1));
				return;
			}
			// month part of caption
			if (rt_Month.Contains(e.X, e.Y))
			{
				// display the context menu, the days grid is updated
				// if the user selects a new month
				//DisplayMonthMenu(e.X, e.Y);
				return;
			}
			// year part of caption
			if (rt_Year.Contains(e.X, e.Y))
			{
				// display the number updown year control, the days
				// grid is updated if the user selects a new year
				DisplayYearUpDown(e.X, e.Y);
				return;
			}
			//Calendar Part User Wants to toggle the calendar to be used
			if (rt_Calendar.Contains(e.X,e.Y))
			{
				b_MouseDownOnCalendar = true; 
				IsGregorian=!IsGregorian;  //Assigns the new calendar
				UpdateCurSel(dt_CurSel);
				
				if(nud_YearUpDown.Visible)
				{
					// init year to currently selected year
					nud_YearUpDown.Minimum = curCalendar.GetYear(dt_MinDate);
					nud_YearUpDown.Maximum = curCalendar.GetYear(dt_MaxDate); 
					nud_YearUpDown.Value   = curCalendar.GetYear(dt_CurSel);
				}
				return;
			}
			
			// today label
			if (e.Y >= pt_BottomLabelsPos.Y )
			{
				// select today in grid
				UpdateCurSel(dt_Today);
				this.Close();
				return;
			}
			// otherwise, start tracking mouse movements
			b_CaptureMouse = true;
			UpdateHoverCell(e.X, e.Y);
		} // protected override void OnMouseDown(MouseEventArgs e)
		
		
		#endregion 
	
		#region OnMouseUp
		
		/// <summary>
		///  <list type="table">
		///   <item><description>User is done hovering over days</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Set the current day if they stopped on a day</para>
		///  <para>otherwise they let up outside of the day grid</para>
		/// </remarks>
		/// <param name="e">MouseEventArgs sent (used for Default MouseEvent)</param>
		/// <seealso cref="DayPickerPopup.b_CaptureMouse"/>
		/// <seealso cref="DayPickerPopup.dta_Days"/>
		/// <seealso cref="DayPickerPopup.GetDayIndex(DateTime)"/>
		/// <seealso cref="DayPickerPopup.dt_HoverSel"/>
		/// <seealso cref="DayPickerPopup.UpdateCurSel"/>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (b_CaptureMouse)
			{
				// done capturing mouse movements
				b_CaptureMouse = false;
				// update the current selection to the day
				// last hovered over
				int index = GetDayIndex(dt_HoverSel);
				if ((index >= 0) && (index < dta_Days.Length))
				{
					UpdateCurSel(dt_HoverSel);
					this.Close();
				}
				else
				{
					// canceled hovering by moving outside of grid
					UpdateCurSel(dt_CurSel);
				}
			}

			if(b_MouseDownOnLeft)			//If mouse was on the left button
			{
				b_MouseDownOnLeft =false;	//No more on control
				Invalidate(rt_LeftButton);	//Invalidate the Area of left button only
			}
			if(b_MouseDownOnRight)			//Mouse was on the right button
			{
				b_MouseDownOnRight =false;	//No more on the right button
				Invalidate(rt_RightButton);	//Invalidate the Area of right button only
			}
			if(b_MouseDownOnCalendar)			//If mouse was on the calendar toggle button
			{
				b_MouseDownOnCalendar =false;	//Mouse no more on the button
				Invalidate(rt_Calendar);		//Invalidate the Calendar Area
			}

		} // protected override void OnMouseUp(MouseEventArgs e)
		
		
		#endregion 
	
		#region OnMouseMove
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Update the hover cell (mouse-over) if necessary</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <param name="e">MouseEventArgs sent (used for Default MouseEvent)</param>
		/// <seealso cref="DayPickerPopup.b_CaptureMouse"/>
		/// <seealso cref="DayPickerPopup.UpdateHoverCell(Int32,Int32)"/>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			// update the hover cell
			if (b_CaptureMouse)
				UpdateHoverCell(e.X, e.Y);
		} // protected override void OnMouseMove(MouseEventArgs e)
		
		#endregion 
	
		#region OnKeyDown

		public void RaiseKeyDown(KeyEventArgs e)
		{
			this.OnKeyDown(e); 
		}
		
		/// <summary>
		///  <list type="table">
		///   <item><description>User can navigate days with the hardware device jog buttons</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <param name="e">KeyEventArgs sent (used for e.KeyCode)</param>
		/// <seealso cref="DayPickerPopup.dt_CurSel"/>
		/// <seealso cref="DayPickerPopup.UpdateCurSel"/>
		/// <seealso cref="DayPickerPopup.UpdateHoverCell(Int32)"/>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			// holds the number of days to change
			int days = 0;
			switch (e.KeyCode)
			{
				case Keys.Left:
					days = -1;
					break;
				case Keys.Right:
					days = 1;
					break;
				case Keys.Up:
					days = -7;
					break;
				case Keys.Down:
					days = 7;
					break;			
			}
			// see if pressed any of the jog buttons
			if (days != 0)
			{
				// calculate the new day that should be selected
				DateTime newDay = curCalendar.AddDays(dt_CurSel,days);
				if (curCalendar.GetMonth(dt_CurSel) != curCalendar.GetMonth(newDay))
				{
					// user navigated to previous or next month
					UpdateCurSel(newDay);
				}
				else
				{
					// TODO: Performance Consideration required
					// the month did not change so update the current
					// selection by calling CreateGraphics (instead of
					// invalidating and repainting) for better performance
//					Graphics g = this.CreateGraphics();
//					DrawDay(g,dt_CurSel,false);
//					DrawDay(g,newDay,true);
//					g.Dispose();
//					dt_CurSel = newDay;
//					// update hover selection
//					UpdateHoverCell(GetDayIndex(dt_CurSel));
//					// raise the ValueChanged event
//					if (this.ValueChanged != null)
//						ValueChanged(this, EventArgs.Empty);
					UpdateCurSel(newDay);
				}
			}
		} // protected override void OnKeyUp(KeyEventArgs e)
		
		
		#endregion 
	
		#region OnMonthMenuPopup
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Event from the month context menu</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Put a checkmark next to the currently selected month</para>
		/// </remarks>
		/// <param name="sender">Control where we are running</param>
		/// <param name="e">EventArgs sent (not used)</param>
		/// <seealso cref="DayPickerPopup.InitMonthContextMenu"/>
		private void OnMonthMenuPopup(System.Object sender, System.EventArgs e)
		{
			// clear all checks
			foreach (MenuStrip item in cm_MonthMenu.Items)
				item.Enabled = false;
				//todo item.Checked = false;
			// check the current month
			if (i_CurMonth > 0 && i_CurMonth <= 13)
				//todo cm_MonthMenu.Items[i_CurMonth-1].Checked = true;		
				cm_MonthMenu.Items[i_CurMonth - 1].Enabled = true;

			//The 13th month will be enabled only if the current calender is not gregorian
			cm_MonthMenu.Items[12].Enabled = !IsGregorian; 
			for(int i=0;i<13;i++)
			{
				try
				{
					cm_MonthMenu.Items[i].Text = IsGregorian ? CultureInfo.InvariantCulture.DateTimeFormat.MonthNames[i]:EthiopianCalendar.MonthNames[i];
				}
				catch
				{  // DateTimeFormat some Culture Infos return an invalid DateTimeFormat
					int j = i+1;
					cm_MonthMenu.Items[i].Text = "("+j+")-Invalid.DateTimeFormat";
				}
			}
			
		} // private void OnMonthMenuPopup(System.Object sender, System.EventArgs e)
		
		
		#endregion 
	
		#region OnMonthMenuClick
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Event from the month context menu</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Update the current selection to the month that was clicked</para>
		///  <para>Check for correct last day of month, including leap year</para>
		///  <para>MenuItem item = sender as MenuItem;</para>
		///  <list type="button">
		///   <item><description>Changes due to US-English Formatting</description></item>
		///   <para>DateTime newDate = DateTime.Parse(string.Format("{0}, {1} {2}",item.Text,dt_CurSel.Day,dt_CurSel.Year));</para>
		///   <para>- allthough this works in German, it does not work in French</para>
		///   <para>- thus changed to following : DateTime newDate = new DateTime(dt_CurSel.Year,item.Index+1,dt_CurSel.Day);</para>
		///   <para>- MenuItem.Index does not work on Compact</para>
		///  </list>
		/// </remarks>
		/// <param name="sender">Control where we are running</param>
		/// <param name="e">EventArgs sent (not used)</param>
		/// <seealso cref="DayPickerPopup.InitMonthContextMenu"/>
		/// <seealso cref="DayPickerPopup.cm_MonthMenu"/>
		/// <seealso cref="DayPickerPopup.UpdateCurSel"/>
		private void OnMonthMenuClick(System.Object sender, System.EventArgs e)
		{			
			try
			{
				int month = cm_MonthMenu.Items.IndexOf(sender as ToolStripMenuItem) +1;
				if(month>0)
				{
					// update the current date selection
					//check if the selected day is supported in the selected month
					//if not set the date to the last month supported
					int year		=this.CurrentCalendar.GetYear		(dt_CurSel);
					int numOfDays	=this.CurrentCalendar.GetDaysInMonth(year,month);
					int day			=this.CurrentCalendar.GetDayOfMonth	(dt_CurSel) > numOfDays ? numOfDays : this.CurrentCalendar.GetDayOfMonth(dt_CurSel) ;
  
					DateTime newDate = new DateTime(year,month,day ,this.CurrentCalendar);
					UpdateCurSel(newDate);
				}
			}
			catch
			{
			}
		} // private void OnMonthMenuClick(System.Object sender, System.EventArgs e)
		
		
		#endregion 
	
		#region OnYearUpDownValueChanged
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Event from year updown control</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Update current selection with the year in the updown control</para>
		/// </remarks>
		/// <param name="sender">Control where we are running</param>
		/// <param name="e">EventArgs sent (not used)</param>
		/// <seealso cref="DayPickerPopup.dt_CurSel"/>
		/// <seealso cref="DayPickerPopup.InitYearUpDown"/>
		/// <seealso cref="DayPickerPopup.nud_YearUpDown"/>
		/// <seealso cref="DayPickerPopup.OnMouseDown"/>
		/// <seealso cref="DayPickerPopup.UpdateCurSel"/>
		private void OnYearUpDownValueChanged(System.Object sender, System.EventArgs e)
		{
			try
			{
				// only want to update the current selection
				// when the user is interacting with the
				// control (when it's visible)
				if (nud_YearUpDown.Visible)
				{
					// update the current selection to the year
					int month	=curCalendar.GetMonth(dt_CurSel);
					int day		=curCalendar.GetDayOfMonth(dt_CurSel);
 
					DateTime newDate = new DateTime((int)nud_YearUpDown.Value,
						month,
						day,
						curCalendar);
					UpdateCurSel(newDate.Date);
				}     
			}
			catch
			{
				// catch if the user entered an invalid year
				// in the control
				nud_YearUpDown.Value =curCalendar.GetYear(dt_CurSel);
			}
		} // private void OnYearUpDownValueChanged(System.Object sender, System.EventArgs e)
		
		
		#endregion
	
		#endregion Events

		#region Dispose
		
		protected override void Dispose(bool disposing)
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
				
				#region Dispose Graphic Objects

				//Bitmap
				if(bmp_Bmp != null)
					bmp_Bmp.Dispose();
			
				//Graphics
				if(gr_Graphics != null)
					gr_Graphics.Dispose();

				//Brushes				

				if (br_Foreground != null)
					br_Foreground.Dispose();
			
				if (sb_RedActive != null)
					sb_RedActive.Dispose();

				if (sb_RedInactive != null)
					sb_RedInactive.Dispose();

				if (sb_ForeColorInactiveDays != null)
					sb_ForeColorInactiveDays.Dispose();

				if (sb_BackSelect != null)
					sb_BackSelect.Dispose();

				if (sb_TextSelect != null)
					sb_TextSelect.Dispose();

				if (sb_CaptionBack != null)
					sb_CaptionBack.Dispose();

				if (sb_CalendarTitelForeColor != null)
					sb_CalendarTitelForeColor.Dispose();

				if (br_Backround != null)
					br_Backround.Dispose();

				if (sb_Frame != null)
					sb_Frame.Dispose();
			
				//Pens

				if (pen_HoverBox != null)
					pen_HoverBox.Dispose();

				if (pn_Background != null)
					pn_Background.Dispose();

				if (pen_Frame != null)
					pen_Frame.Dispose();
			
				if (pen_Today != null)
					pen_Today.Dispose();
			
				//Fonts
				if (ft_FontDay != null)
					ft_FontDay.Dispose();
			
				if (ft_FontToday != null)
					ft_FontToday.Dispose();			
			
				if (ft_FontCaption != null)
					ft_FontCaption.Dispose();			
			
				#endregion Dispose Graphic Objects
			}
			
			base.Dispose( disposing );		   
		}
		
		
		#endregion Dispose

		#region Helper Methods
	
		#region InitYearUpDown
	
		/// <summary>
		///  <list type="table">
		///   <item><description>Initialize the numeric updown control that is displayed</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>The year part of the caption is clicked</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.nud_YearUpDown"/>
		/// <seealso cref="DayPickerPopup.OnYearUpDownValueChanged"/>
		/// <seealso cref="DayPickerPopup.DayOfWeekCharacters"/>
		private void InitYearUpDown()
		{
			// create the numeric updown control
			nud_YearUpDown = new NumericUpDown();
			
			this.Controls.Add(nud_YearUpDown);

			// init other properties
			nud_YearUpDown.Visible = false;			
		}
		
		
		#endregion InitYearUpDown
	
		#region DisplayYearUpDown
	
		/// <summary>
		///  <list type="table">
		///   <item><description>Display the numeric updown year control</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <param name="x">Position Left (Mouse Position)</param>
		/// <param name="y">Position Top  (Mouse Position)</param>
		/// <seealso cref="DayPickerPopup.dt_CurSel"/>
		/// <seealso cref="DayPickerPopup.OnMouseDown"/>
		/// <seealso cref="DayPickerPopup.rt_Year"/>
		/// <seealso cref="DayPickerPopup.nud_YearUpDown"/>
		private void DisplayYearUpDown(int x, int y)
		{
			// init year to currently selected year
			nud_YearUpDown.Minimum = curCalendar.GetYear(dt_MinDate);
			nud_YearUpDown.Maximum = curCalendar.GetYear(dt_MaxDate);
			nud_YearUpDown.Value   = curCalendar.GetYear(dt_CurSel);

			// init the position and size of the control
			nud_YearUpDown.Left   = rt_Year.Left	+ 5;
			nud_YearUpDown.Top    = rt_Year.Top		- 3;
			nud_YearUpDown.Width  = rt_Year.Width	+ 10;
			nud_YearUpDown.Height = rt_Year.Height	+ 6;
			nud_YearUpDown.Show();
		}
	
		
		#endregion DisplayYearUpDown
	
		#region InitMonthContextMenu
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Initialize the context menu that is displayed when the user clicks the month part of the caption</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Checks for invalid CultureInfo.DateTimeFormat</para>
		/// </remarks>		
		/// <seealso cref="DayPickerPopup.DayOfWeekCharacters"/>		
		/// <seealso cref="DayPickerPopup.OnMonthMenuClick"/>
		/// <seealso cref="DayPickerPopup.OnMonthMenuPopup"/>
		
		#endregion InitMonthContextMenu
	
		#region DisplayMonthMenu
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Show the month context menu</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>The current month is checked in the popup event</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.cm_MonthMenu"/>
		/// <seealso cref="DayPickerPopup.OnMouseDown"/>
		/// <param name="x">Position Left (Mouse Position)</param>
		/// <param name="y">Position Top  (Mouse Position)</param>
		private void DisplayMonthMenu(int x, int y)
		{
			if(this.Visible)
			{
				cm_MonthMenu.Show(this, new Point(x, y));
			}
		}
		
	
		#endregion DisplayMonthMenu
	
		#region CalculateFirstDate
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Calculates the date for the first cell in the days grid</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Always show at least one day of previous month.</para>
		///  <para>- First Day of Week support, used only here</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.dt_CurSel"/>
		/// <seealso cref="DayPickerPopup.dt_FirstDate"/>
		/// <seealso cref="DayPickerPopup.dow_FirstDayOfWeek"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		private void CalculateFirstDate()
		{
			// mj10777 Language Support for dow_FirstDayofWeek used only here, Checked : So,Mo,Tu,We,Th,Fr,Sa
			int year  =curCalendar.GetYear(dt_CurSel);
			int month =curCalendar.GetMonth(dt_CurSel);
			
			//This changes the hour representation in the ethiopian calendar but we dont want the hour value changed
			DateTime tempFirstDate= new DateTime(year,month,1,curCalendar);
			//So create  a new datetime which have the correct values with omitting the hour value
			dt_FirstDate = new DateTime(tempFirstDate.Year,tempFirstDate.Month,tempFirstDate.Day);   
						
			int i_CountDays = 0;
			if (curCalendar.GetDayOfWeek(dt_FirstDate) == dow_FirstDayOfWeek)
				i_CountDays = -7;
			else
			{  
				int i_FirstDayOfWeek = (int)dow_FirstDayOfWeek;
				if ((int)curCalendar.GetDayOfWeek(dt_FirstDate) != 0) // Not Sunday
				{
					i_CountDays = -(int)curCalendar.GetDayOfWeek(dt_FirstDate)+i_FirstDayOfWeek;
					if (i_FirstDayOfWeek == 6)
						i_CountDays = i_CountDays;
					if (i_CountDays > 0)
						i_CountDays = -7+i_CountDays;
					if (i_CountDays <= -7)
						i_CountDays += 7;
				}
				else                                 // Sunday
					i_CountDays = -7+i_FirstDayOfWeek;
			}
			dt_FirstDate =curCalendar.AddDays(dt_FirstDate,i_CountDays); // Set the FirstDate
		}
		
		
		#endregion CalculateFirstDate
	
		#region CalculateDays
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Calculate and cache the days that are displayed in the calendar</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>The days are cached for better performance, each day is only 8 bytes</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.dta_Days"/>
		/// <seealso cref="DayPickerPopup.DrawDays"/>
		/// <seealso cref="DayPickerPopup.dt_FirstDate"/>
		private void CalculateDays()
		{
			for (int i=0;i<dta_Days.Length;i++)
				dta_Days[i] =curCalendar.AddDays(dt_FirstDate,i);
		} // private void CalculateDays()
		
		
		#endregion CalculateDays
	
		#region GetDayCellPosition
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Return the upper left x / y coordinates for the specified index</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <param name="index">Position of Day Cell</param>
		/// <returns>Point Position of Day Cell</returns>
		/// <seealso cref="DayPickerPopup.pt_DayGrids"/>
		/// <seealso cref="DayPickerPopup.sz_DaysCell"/>
		/// <seealso cref="DayPickerPopup.DrawDay"/>
		/// <seealso cref="DayPickerPopup.DrawCurSelection"/>
		/// <seealso cref="DayPickerPopup.DrawHoverSelection"/>
		/// <seealso cref="DayPickerPopup.DrawTodaySelection"/>
		/// <seealso cref="DayPickerPopup.i_NumCols"/>
		private Point GetDayCellPosition(int index)
		{
			// calculate the x and y coordinates for the specified index
			return new Point(
				pt_DayGrids.X + (((int)index % i_NumCols) * sz_DaysCell.Width),
				pt_DayGrids.Y + (((int)index / i_NumCols) * (sz_DaysCell.Height+1)));
		}
	
		
		#endregion GetDayCellPosition
	
		#region CreateMemoryBitmap
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Create memory bitmap for double-buffering</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Very unsatisfied on result of painting of Arrows!</para>
		///  <para>none</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.sz_ArrowButtonOffset"/>
		/// <seealso cref="DayPickerPopup.sz_ArrowButtonSize"/>
		/// <seealso cref="DayPickerPopup.bmp_Bmp"/>
		/// <seealso cref="DayPickerPopup.gr_Graphics"/>
		/// <seealso cref="DayPickerPopup.rt_LeftButton"/>
		/// <seealso cref="DayPickerPopup.rt_RightButton"/>
		/// <seealso cref="DayPickerPopup.pta_LeftArrowPoints"/>
		/// <seealso cref="DayPickerPopup.pta_RightArrowPoints"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		private void CreateMemoryBitmap()
		{
			// see if need to create memory bitmap
			if ((bmp_Bmp == null) || (bmp_Bmp.Width != this.Width) || (bmp_Bmp.Height != this.Height))
			{
				// create the memory bitmap
				bmp_Bmp = new Bitmap(this.Width,this.Height);
				gr_Graphics = Graphics.FromImage(bmp_Bmp);
				// calculate the coordinates of the left and right
				// arrow buttons now instead of each time paint
				// left button
				rt_LeftButton = new Rectangle(
					sz_ArrowButtonOffset.Width -1 ,sz_ArrowButtonOffset.Height - 1,
					sz_ArrowButtonSize.Width,sz_ArrowButtonSize.Height+2);
				
				// right button
				rt_RightButton = new Rectangle(
					this.Width-sz_ArrowButtonOffset.Width-sz_ArrowButtonSize.Width-1,
					sz_ArrowButtonOffset.Height-1,sz_ArrowButtonSize.Width ,sz_ArrowButtonSize.Height+2);				

				PointF top	 =	new PointF(rt_LeftButton.X + (rt_LeftButton.Width /2) + 2 ,rt_LeftButton.Y + rt_LeftButton.Height /4);
				PointF bottom =	new PointF(top.X, rt_LeftButton.Y + (rt_LeftButton.Height *3) /4 );
				PointF left	 =  new PointF(top.X - (bottom.Y - top.Y)/2,top.Y + (bottom.Y - top.Y)/2 ); 
								
				pta_LeftArrowPoints[0] = top;		// Top
				pta_LeftArrowPoints[1] = left;		// Left
				pta_LeftArrowPoints[2] = bottom;    // Bottom
				
				top			  =	new PointF(rt_RightButton.X + (rt_RightButton.Width /2) -2 ,rt_RightButton.Y + rt_RightButton.Height /4); 
				bottom		  =	new PointF(top.X, rt_RightButton.Y + (rt_RightButton.Height *3) /4 );
				PointF right  = new PointF(top.X + (bottom.Y - top.Y)/2,top.Y + (bottom.Y - top.Y)/2 );				
				
				pta_RightArrowPoints[0] = top;		//Top
				pta_RightArrowPoints[1] = right;	//Right
				pta_RightArrowPoints[2] = bottom;	//Bottom			
			}
		}
	
		
		#endregion CreateMemoryBitmap
	
		#region CreateGdiObjects
	
		/// <summary>
		///  <list type="table">
		///   <item><description>Create any gdi objects required for drawing</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Only when not allready done</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		/// <seealso cref="DayPickerPopup.ft_FontDay"/>
		/// <seealso cref="DayPickerPopup.s_FontName_Day"/>
		/// <seealso cref="DayPickerPopup.f_FontSize_Day"/>
		/// <seealso cref="DayPickerPopup.fs_FontStyle_Day"/>
		/// <seealso cref="DayPickerPopup.ft_FontToday"/>
		/// <seealso cref="DayPickerPopup.s_FontName_Today"/>
		/// <seealso cref="DayPickerPopup.f_FontSize_Today"/>
		/// <seealso cref="DayPickerPopup.fs_FontStyle_Today"/>
		/// <seealso cref="DayPickerPopup.ft_FontCaption"/>
		/// <seealso cref="DayPickerPopup.s_FontName_Caption"/>
		/// <seealso cref="DayPickerPopup.f_FontSize_Caption"/>
		/// <seealso cref="DayPickerPopup.fs_FontStyle_Caption"/>
		/// <seealso cref="DayPickerPopup.br_Backround"/>
		/// <seealso cref="DayPickerPopup.sb_CaptionBack"/>
		/// <seealso cref="DayPickerPopup.sb_CalendarTitelForeColor"/>
		/// <seealso cref="DayPickerPopup.br_Foreground"/>
		/// <seealso cref="DayPickerPopup.sb_RedActive"/>
		/// <seealso cref="DayPickerPopup.sb_Frame"/>
		/// <seealso cref="DayPickerPopup.sb_RedInactive"/>
		/// <seealso cref="DayPickerPopup.sb_ForeColorInactiveDays"/>
		/// <seealso cref="DayPickerPopup.sb_BackSelect"/>
		/// <seealso cref="DayPickerPopup.sb_TextSelect"/>
		/// <seealso cref="DayPickerPopup.pen_Frame"/>
		/// <seealso cref="DayPickerPopup.pen_HoverBox"/>
		/// <seealso cref="DayPickerPopup.OnPaint"/>
		private void CreateGdiObjects()
		{
			if (ft_FontDay == null)
				ft_FontDay = new Font(s_FontName_Day,f_FontSize_Day,fs_FontStyle_Day);
			// days grid
			if (br_Foreground == null || br_Foreground.Color != this.ForeColor)
				br_Foreground = new SolidBrush(this.ForeColor);
			if (sb_RedActive == null)
				sb_RedActive = new SolidBrush(System.Drawing.Color.Red);
			if (sb_RedInactive == null)
				sb_RedInactive = new SolidBrush(System.Drawing.Color.LightSalmon);
			if (sb_ForeColorInactiveDays == null)
				sb_ForeColorInactiveDays = new SolidBrush(SystemColors.GrayText);
			if (sb_BackSelect == null)
				sb_BackSelect = new SolidBrush(SystemColors.Highlight);
			if (sb_TextSelect == null)
				sb_TextSelect = new SolidBrush(SystemColors.HighlightText);
			if (pen_HoverBox == null)
				pen_HoverBox = new Pen(SystemColors.GrayText);
			// Today    
			if (ft_FontToday == null)
				ft_FontToday = new Font(s_FontName_Today,f_FontSize_Today,fs_FontStyle_Today);
			// Caption
			if (ft_FontCaption == null)
				ft_FontCaption = new Font(s_FontName_Caption,f_FontSize_Caption,fs_FontStyle_Caption);
			if (sb_CaptionBack == null)
				sb_CaptionBack = new SolidBrush(SystemColors.ActiveCaption);
			if (sb_CalendarTitelForeColor == null)
				sb_CalendarTitelForeColor = new SolidBrush(SystemColors.ActiveCaptionText);
			// General
			if (br_Backround == null || br_Backround.Color != this.BackColor)
				br_Backround = new SolidBrush(this.BackColor);
			if (pn_Background == null || pn_Background.Color != this.BackColor)
				pn_Background = new Pen(this.BackColor);
			if (sb_Frame == null)
				sb_Frame = new SolidBrush(SystemColors.WindowFrame);
			if (pen_Frame == null)
				pen_Frame = new Pen(SystemColors.WindowFrame);
			if (pen_Today == null)
				pen_Today = new Pen(System.Drawing.Color.Red);
		}
	
		
		#endregion CreateGdiObjects
	
		#region UpdateCurSel
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Update the current selection with the specified date</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <param name="newDate">Date selected</param>
		/// <seealso cref="DayPickerPopup.b_AllowClose"/>
		/// <seealso cref="DayPickerPopup.dt_CurSel"/>
		/// <seealso cref="DayPickerPopup.dt_HoverSel"/>
		/// <seealso cref="DayPickerPopup.dt_MaxDate"/>
		/// <seealso cref="DayPickerPopup.dt_MinDate"/>
		/// <seealso cref="DayPickerPopup.OnKeyDown"/>
		/// <seealso cref="DayPickerPopup.OnMonthMenuClick"/>
		/// <seealso cref="DayPickerPopup.OnMouseDown"/>
		/// <seealso cref="DayPickerPopup.OnMouseUp"/>
		/// <seealso cref="DayPickerPopup.OnYearUpDownValueChanged"/>
		/// <seealso cref="DayPickerPopup.Value"/>
		/// <seealso cref="DayPickerPopup.ValueChanged"/>
		private void UpdateCurSel(DateTime newDate)
		{
			if (newDate < this.MinDate)
			{
				newDate = this.MinDate;
				b_AllowClose = false;
			}
			else
			{
				if (newDate > this.MaxDate)
				{
					newDate = this.MaxDate;
					b_AllowClose = false;
				}
				else
				{
					b_AllowClose = true;
				}
			}  // else if (newDate < this.MinDate)
			// see if should raise ValueChanged event
			bool raiseEvent = (dt_CurSel != newDate) ? true : false;
			// store new date selection
			dt_CurSel = newDate;
			dt_HoverSel = dt_CurSel;
			// repaint
			Invalidate();
			Update();
			// raise ValueChanged event
			if (this.ValueChanged != null && raiseEvent)
				ValueChanged(this, EventArgs.Empty);
		}
	
		
		#endregion UpdateCurSel
	
		#region GetDayIndex
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Return index into days array for the specified date</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <param name="date">Date selected</param>
		/// <returns>Amount of Days from first day of month to recieved date</returns>
		/// <seealso cref="DayPickerPopup.dt_FirstDate"/>
		/// <seealso cref="DayPickerPopup.DrawCurSelection"/>
		/// <seealso cref="DayPickerPopup.DrawDay"/>
		/// <seealso cref="DayPickerPopup.DrawHoverSelection"/>
		/// <seealso cref="DayPickerPopup.DrawTodaySelection"/>
		/// <seealso cref="DayPickerPopup.OnKeyDown"/>
		/// <seealso cref="DayPickerPopup.OnMouseUp"/>
		private int GetDayIndex(DateTime date)
		{
			TimeSpan span = date.Subtract(dt_FirstDate);
			int i_rc = (int)span.TotalDays;
			//Trace.WriteLine("\n"+span); 
			return i_rc;
		}
	
		
		#endregion GetDayIndex
	
		#region GetDayIndex
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Return index into the days array for the specified coordinates</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <param name="x">X Position</param>
		/// <param name="y">Y Position</param>
		/// <returns>Index of Day found ton refieved x/Y Position</returns>
		/// <seealso cref="DayPickerPopup.pt_BottomLabelsPos"/>
		/// <seealso cref="DayPickerPopup.sz_DaysCell"/>
		/// <seealso cref="DayPickerPopup.pt_DayGrids"/>
		/// <seealso cref="DayPickerPopup.i_NumCols"/>
		/// <seealso cref="DayPickerPopup.UpdateHoverCell"/>
		private int GetDayIndex(int x, int y)
		{
			// see if in the day grid bounding rectangle
			Rectangle rc = new Rectangle(
				0, pt_DayGrids.Y,
				i_NumCols * sz_DaysCell.Width,
				pt_BottomLabelsPos.Y);
			if (!rc.Contains(x, y))
				return -1;
			// calculate the index
			return (x / sz_DaysCell.Width) +
				(((y-pt_DayGrids.Y) / (sz_DaysCell.Height+1)) * i_NumCols);
		}
	
		
		#endregion GetDayIndex
	
		#region UpdateHoverCell
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Update the cell that has the hover mark</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <param name="x">X Position</param>
		/// <param name="y">Y Position</param>
		/// <seealso cref="DayPickerPopup.GetDayIndex(Int32,Int32)"/>
		/// <seealso cref="DayPickerPopup.UpdateHoverCell(Int32)"/>
		private void UpdateHoverCell(int x, int y)
		{
			// calculate index into grid and then update the cell
			int index = GetDayIndex(x, y);
			UpdateHoverCell(index);
		} 
	
		
		#endregion UpdateHoverCell
	
		#region UpdateHoverCell
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Update the cell that has the hover mark</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Call CreateGraphics instead of invalidating for better performance</para>
		/// </remarks>
		/// <param name="newIndex">Index of Day Cell</param>
		/// <seealso cref="DayPickerPopup.dta_Days"/>
		/// <seealso cref="DayPickerPopup.DrawHoverSelection"/>
		/// <seealso cref="DayPickerPopup.DrawTodaySelection"/>
		/// <seealso cref="DayPickerPopup.dt_HoverSel"/>
		/// <seealso cref="DayPickerPopup.OnKeyDown"/>
		private void UpdateHoverCell(int newIndex)
		{
			// see if over the days grid
			if (newIndex < 0 || newIndex >= dta_Days.Length)
			{
				// outside of grid, erase current hover mark
				using (Graphics g = this.CreateGraphics())
				{
					DrawHoverSelection(g, dt_HoverSel, false);
					DrawTodaySelection(g);
				}				
				dt_HoverSel = DateTime.MinValue;
				return;
			}
			// see if hover date has changed
			if (dt_HoverSel != dta_Days[newIndex])
			{
				// erase old hover mark and draw new mark
				using ( Graphics g = this.CreateGraphics())
				{
					DrawHoverSelection(g,dt_HoverSel, false);
					DrawHoverSelection(g,dta_Days[newIndex], true);
					DrawTodaySelection(g);
				}
				
				// store current hover date
				dt_HoverSel = dta_Days[newIndex];
			}
		}
		
		
		#endregion UpdateHoverCell
	
		#region Close
		
		/// <summary>
		///  <list type="table">
		///   <item><description>Close the control. Raise the CloseUp event</description></item>
		///   <para>Changed from : Éric Carmichael - eric@westgen.com</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <seealso cref="DayPickerPopup.b_AllowClose"/>
		/// <seealso cref="DayPickerPopup.OnKeyDown"/>
		/// <seealso cref="DayPickerPopup.OnLostFocus"/>
		/// <seealso cref="DayPickerPopup.OnMouseDown"/>
		/// <seealso cref="DayPickerPopup.OnMouseUp"/>
		/// <seealso cref="DayPickerPopup.nud_YearUpDown"/>
		private void Close()
		{
			if (this.b_AllowClose == true)
			{
				if (this.nud_YearUpDown.Visible == false)
				{
					this.Hide();
					// raise the CloseUp event
					if (this.CloseUp != null)
						CloseUp(this, EventArgs.Empty);
				} // if (this.nud_YearUpDown.Visible == false)
			}  // if (this.b_AllowClose == true)
		}
	
		
		#endregion Close
	
		#region OnLostFocus
		
		/// <summary>
		///  <list type="table">
		///   <item><description>OnLostFocus</description></item>
		///   <para>Idea from : Éric Carmichael - eric@westgen.com</para>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>none</para>
		/// </remarks>
		/// <param name="e">EventArgs sent (not used)</param>
		protected override void OnLostFocus(EventArgs e)
		{
			base.OnLostFocus(e);
			this.b_AllowClose = true;
			this.Close();
		}
		
		
		#endregion OnLostFocus
	
		#region OnValidDayOfWeekCharacters
		
		/// <summary>
		///  <list type="table">
		///   <item><description>OnValidDayOfWeekCharacters</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Check if set DayOfWeekCharacters will fit the Screen</para>
		/// </remarks>
		/// <param name="i_DayOfWeekCharacters">Amount of Charaters to check</param>
		/// <returns>Return valid Amount of Charaters that would fit the Screen</returns>
		protected int OnValidDayOfWeekCharacters(int i_DayOfWeekCharacters)
		{
			bool rc=false;
			
			int i_Size = i_DayOfWeekCharacters;
			
			ci_FirstDayOfWeek = FirstDayOfWeek;
			OnDaysOfWeek();

			int i_Longest = 0;
			for (int i=0; i< s_DayOfWeek.Length;i++)
			{ 
				// Check for longest DayofWeek Text
				if (s_DayOfWeek[i].Length > i_Longest)
					i_Longest = s_DayOfWeek[i].Length;
			}
			if (i_Size > i_Longest)
				i_Size = i_Longest;
			//----
			string[] sa_DayOfWeek   = new string[s_DayOfWeek.Length];
			
			if (this.TopLevelControl != null)
				ControlSize = this.TopLevelControl.ClientRectangle;
			if (ft_FontDay == null)
				ft_FontDay = new Font(s_FontName_Day,f_FontSize_Day,fs_FontStyle_Day);
			Size sz_TotalSize, sz_DaySize;
			// create the memory bitmap
			Bitmap bmp_bmp = new Bitmap(this.Width,this.Height);
			Graphics gr_graphics = Graphics.FromImage(bmp_bmp);
			while (!rc)
			{
				sz_TotalSize = new Size(0,0);
				for (int i=0;i<s_DayOfWeek.Length;i++)
				{
					if (s_DayOfWeek[i].Length < i_Size)
						sa_DayOfWeek[i] = s_DayOfWeek[i];
					else
						sa_DayOfWeek[i] = s_DayOfWeek[i].Substring(0,i_Size);
					sz_DaySize = gr_graphics.MeasureString(sa_DayOfWeek[i],ft_FontDay).ToSize();
					if (sz_DaySize.Height > sz_TotalSize.Height)
						sz_TotalSize.Height  = sz_DaySize.Height;
					if (sz_DaySize.Width  > sz_TotalSize.Width)
						sz_TotalSize.Width   = sz_DaySize.Width;
				} // for (int i=0;i<s_DayOfWeek.Length;i++)
				// Give an extra Pixel due to rounding errors !
				sz_TotalSize.Width++;
				sz_TotalSize.Width = i_NumCols*sz_TotalSize.Width;
				// We need some space left of the DaysOfWeek and Days (same as sz_ArrowButtonOffset.Width)
				sz_TotalSize.Width  += (sz_TotalSize.Width/30);
				if (sz_TotalSize.Width < rt_ClientSize.Width)
				{ // this would fit the Screen
					i_DayOfWeekCharacters = i_Size;
					s_DayOfWeek = sa_DayOfWeek;
					rc = true;
				}
				else
				{ // this will not fit the Screen
					i_Size--;
				}
			}  // while (!rc)
			gr_graphics.Dispose();
			//-------------------------------
			return i_DayOfWeekCharacters; 
		}
	
		
		#endregion  OnValidDayOfWeekCharacters
	
		#region OnPaintSquibble
		
		/// <summary>
		///  <list type="table">
		///   <item><description>OnPaintSquibble</description><para>mj10777 CultureInfo support</para></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Create Array of Point for Squibble</para>
		/// </remarks>
		/// <param name="p">Pen to Draw with</param>
		/// <param name="g">Graphics to Draw with</param>
		/// <param name="rt_Position">Position with size to creation Points</param>
		/// <returns>Return valid Amount of Charaters that would fit the Screen</returns>
		protected void OnPaintSquibble(Pen p,Graphics g,ref Rectangle rt_Position)
		{
			//------------------------------------------------------------------------
			// Based on size 27x18, this is what the Squibble should look like -------
			//------------------------------------------------------------------------
			// Top Line      [0] - XXXXXXXXXXXXXXXX          - Top Line           [0]
			// Point       [5.2] -XXXXXXXXXXXXXXXXXXXX       - Curve Top Right    [6]
			// Connect Top [5.1] -             X  XXXXXX     - Curve Top Right    [6]
			// Top Line 2    [4] -       XXXXXXX     XXXXX   - Curve Top Right    [6]
			// Top Line 2    [4] -     XXXXXXXXX       XXXX  - Curve Top Right    [6]
			// Curve Top Left[6] -   XXXXX               XXX - Curve Top Right    [6]
			// Curve Top Left[6] -  XXXX                  XX - Curve Top Right    [6]
			// Curve Top Left[6] -  XX                     XX- Right Line         [2]
			// Curve Top Left[6] - XX                      XX- Right Line         [2]
			// Curve Top Left{6] - XX                      XX- Right Line         [2]
			// Left Line     [3] -XX                       XX- Right Line         [2]
			// Curve Bottom Left - XX                      XX- Right Line         [2]
			// Curve Bottom. [7] - XX                      XX- Right Line         [2]
			// Curve Bottom. [7] -  XXX                  XXX - Curve Bottom Right [8]
			// Curve Bottom. [7] -   XXX                XXXX - Curve Bottom Right [8]
			// Curve Bottom. [7] -    XXXX            XXXX   - Curve Bottom Right [8]
			// Curve Bottom. [7] -     XXXXXXXXXXXXXXXXXX    - Bottom Line 1      [1]
			// Bottom Line 2 [1] -       XXXXXXXXXXXXXX      - Bottom Line 2      [1]
			//------------------------------------------------------------------------
			//- Test Stndard Size ----------------------------------------------------
			//rt_Position.Width = 27; rt_Position.Height = 18;  
			//- Test Portion to see results ------------------------------------------
			SolidBrush  bb = new SolidBrush(Color.Black);     
			SolidBrush  bl = new SolidBrush(Color.Blue);     
			SolidBrush  gr = new SolidBrush(Color.Green);     
			SolidBrush  gd = new SolidBrush(Color.Gold);     
			//------------------------------------------------------------------------
			//- Take the Pen color used for Brush that we use here -------------------
			SolidBrush  b = new SolidBrush(p.Color);
			//- Resurve space for our Lines an Curves --------------------------------
			Rectangle[] rt_Squibble = new Rectangle[10];
			//------------------------------------------------------------------------
			//- Basis Pixel Height/Width based on rt_Rectangle size ------------------
			float f_1PH = (float)rt_Position.Height/18;
			float f_1PW = (float)rt_Position.Width/27;
			//------------------------------------------------------------------------
			int i_BaseSize = 0;  // Standard Width  (like 'Today', 'Heute')
			if ((((f_1PW/f_1PH) > 1) && ((f_1PW/f_1PH) < 1.5)) || ((f_1PW/f_1PH) > 2))
				i_BaseSize = 1;     // Very Wide Width (like 'Aujourd'hui') of very thin
			//------------------------------------------------------------------------
			if (f_1PH < 1)
				f_1PH = 1;  // Must be at least 1 Pixel 
			if (f_1PW < 1)
				f_1PW = 1;  // Must be at least 1 Pixel 
			float f_2PH = f_1PH*2;
			float f_2PW = f_1PW*2;
			//------------------------------------------------------------------------
			//------------------------------------------------------------------------
			//-- Top Line [0] --------------------------------------------------------
			//------------------------------------------------------------------------
			rt_Squibble[0].X      = rt_Position.X+(int)(f_1PW);
			rt_Squibble[0].Y      = rt_Position.Y;
			rt_Squibble[0].Height = (int)(f_2PH);
			rt_Squibble[0].Width  = (int)(f_1PW*15);
			//------------------------------------------------------------------------
			//-- Bottom Line [1] -----------------------------------------------------
			//------------------------------------------------------------------------
			rt_Squibble[1].X      = (rt_Position.X+(rt_Position.Width/4))+(int)f_1PW;
			rt_Squibble[1].Y      = (int)(rt_Position.Y+(rt_Position.Height-f_2PH));
			rt_Squibble[1].Height = rt_Squibble[0].Height;
			rt_Squibble[1].Width  = rt_Position.Width/2;
			//------------------------------------------------------------------------
			if (i_BaseSize == 0)
			{
				rt_Squibble[1].X      = (rt_Position.X+(rt_Position.Width/6));
				rt_Squibble[1].Width  = (rt_Squibble[0].X+rt_Squibble[0].Width)-rt_Position.X;
			}
			else
			{
				rt_Squibble[1].X      = (rt_Position.X+(rt_Position.Width/12));
				rt_Squibble[1].Width  = ((rt_Squibble[0].X+rt_Squibble[0].Width)-rt_Position.X)+(rt_Position.Width/6);
			}
			rt_Squibble[1].Y      = (int)(rt_Position.Y+(rt_Position.Height-f_2PH));
			rt_Squibble[1].Height = rt_Squibble[0].Height;
			//------------------------------------------------------------------------
			//-- Right Line [2] ------------------------------------------------------
			//------------------------------------------------------------------------
			rt_Squibble[2].X      = rt_Position.X+(rt_Position.Width-(int)(f_2PW));
			rt_Squibble[2].Y      = rt_Position.Y+(rt_Position.Height/3)+(int)f_1PH;
			rt_Squibble[2].Height = rt_Position.Height/3;
			rt_Squibble[2].Width  = (int)(f_2PW);
			if (rt_Squibble[2].Width > 2)
				rt_Squibble[2].Width = 2;
			//------------------------------------------------------------------------
			//-- Left  Line [3] ------------------------------------------------------
			//------------------------------------------------------------------------
			rt_Squibble[3].X      = rt_Position.X;
			rt_Squibble[3].Y      = rt_Position.Y+((rt_Position.Height/2)+(int)f_1PH);
			rt_Squibble[3].Height = (int)(f_1PH);
			rt_Squibble[3].Width  = 1; // (int)(f_1PW); // rt_Squibble[2].Width;
			//------------------------------------------------------------------------
			//-- Top 2 Line [4] ------------------------------------------------------
			//------------------------------------------------------------------------
			rt_Squibble[4].X      = rt_Squibble[1].X;
			rt_Squibble[4].Y      = rt_Squibble[0].Y+(int)(f_1PH*3);
			rt_Squibble[4].Height = rt_Squibble[0].Height;
			if (i_BaseSize == 0)
			{
				rt_Squibble[4].Width  = (rt_Squibble[1].Width/2)+(int)f_1PW;
			}
			else
			{
				rt_Squibble[4].Width  = ((rt_Squibble[1].Width/3)*2)+(int)f_1PW;
			}
			//------------------------------------------------------------------------
			//-- Top Connect 1 to 2 [5.1] --------------------------------------------
			//------------------------------------------------------------------------
			rt_Squibble[5].X      = rt_Squibble[4].X+(rt_Squibble[4].Width-(int)f_1PW);
			rt_Squibble[5].Y      = rt_Squibble[0].Y+rt_Squibble[0].Height;
			rt_Squibble[5].Height = (rt_Squibble[4].Y-rt_Squibble[0].Y)-rt_Squibble[4].Height;
			rt_Squibble[5].Width  = (int)(f_1PW);
			//------------------------------------------------------------------------
			//-- Paint Lines ---------------------------------------------------------
			//------------------------------------------------------------------------
			for (int i=0;i<6;i++)
			{
				if (i == 5)
				{
					if (i_BaseSize == 0)
						g.FillRectangle(b,rt_Squibble[i]);
				}
				else
					g.FillRectangle(b,rt_Squibble[i]);
			}
			//------------------------------------------------------------------------
			//-- Top Point Left of Top 1 [5.2] ---------------------------------------
			//------------------------------------------------------------------------
			rt_Squibble[5].X      = rt_Squibble[0].X-(int)f_1PW;
			rt_Squibble[5].Y      = rt_Squibble[0].Y+(int)f_1PH;
			rt_Squibble[5].Height = (int)f_1PH;
			rt_Squibble[5].Width  = (int)f_1PW;
			//-- Paint Top Point Left of Top 1
			g.FillRectangle(b,rt_Squibble[5]);
			//------------------------------------------------------------------------
			// Preperation to Paint Curve Top/Left[6];Bottom/Left[7];Bottom/Right[8] -
			//------------------------------------------------------------------------
			/*
			int i_LeftPX  = (rt_Squibble[4].X-rt_Squibble[3].Width)-rt_Position.X;
			int i_LeftPY  = (rt_Squibble[3].Y-(int)(f_1PH))-rt_Squibble[4].Y;
			rt_Squibble[6].X      = rt_Squibble[3].X;
			rt_Squibble[6].Height = (int)(f_1PH*2);
			rt_Squibble[6].Y      = rt_Squibble[3].Y;
			rt_Squibble[6].Width  = (int)(f_1PW*2);
			rt_Squibble[7]        = rt_Squibble[6];
			rt_Squibble[7].Y     -= (int)f_1PH;
			rt_Squibble[8]        = rt_Squibble[7];
			rt_Squibble[8].X      = rt_Squibble[2].X+(int)f_1PW;
			//------------------------------------------------------------------------
			// Preperation to Paint Curve Top/Right [9]-------------------------------
			//------------------------------------------------------------------------
			rt_Squibble[9].Width  = (int)(f_1PW*2);
			rt_Squibble[9].Height = rt_Squibble[6].Height+(int)f_1PH;
			rt_Squibble[9].X      = rt_Squibble[2].X;
			rt_Squibble[9].Y      = rt_Squibble[2].Y-(int)f_1PH;
			rt_Squibble[8]        = rt_Squibble[9];
			rt_Squibble[8].Y      = (rt_Squibble[2].Y+rt_Squibble[2].Height)-((int)f_1PH*1);
			//rt_Squibble[8].Y      = (rt_Squibble[2].Y+rt_Squibble[2].Height);
			// rt_Squibble[9].Y      = rt_Squibble[2].Y;
			int i_RightTX = rt_Squibble[9].X-(rt_Squibble[0].X-rt_Squibble[0].Width);
			int i_RightTY = rt_Squibble[9].Y-rt_Squibble[0].Y;
			bool b_once=false;
			//------------------------------------------------------------------------
			// Painting Curve Top/Right [9]-------------------------------------------
			//------------------------------------------------------------------------
			for (;rt_Squibble[9].X>rt_Squibble[0].X;)
			{
				for (int y=1;rt_Squibble[9].Y>rt_Squibble[0].Y;y++)
				{
				if (((i_RightTY/3)*2) <= y)
				{
				rt_Squibble[9].Width  = (int)(f_1PW*3);
				rt_Squibble[9].Y -= (int)(f_1PH*1);  
				rt_Squibble[8].Width  = (int)(f_1PW*3);
				rt_Squibble[8].Y += (int)(f_1PH*1);
				if (!b_once)
				{
				b_once = true;
				rt_Squibble[9].X -= (int)(f_1PW*2);
				rt_Squibble[8].X -= (int)(f_1PW*2);
				}
				else
				{
				rt_Squibble[9].X -= (int)(f_1PW*2);
				rt_Squibble[8].X -= (int)(f_1PW*2);
				}
				} // if ((i_RightTY/2) <= y)
				else
				{
				rt_Squibble[9].X -= (int)(f_1PW*1);
				rt_Squibble[9].Y -= (int)(f_1PH*1);
				rt_Squibble[8].X -= (int)(f_1PW*1);
				rt_Squibble[8].Y += (int)(f_1PH*1);
				}
				g.FillRectangle(b,rt_Squibble[9]);
				// g.FillRectangle(b,rt_Squibble[8]);
				rt_Squibble[9].Height = rt_Squibble[6].Height;
				rt_Squibble[8].Height = rt_Squibble[6].Height;
				} // for (int y=1;rt_Squibble[9].Y>rt_Squibble[2].Y;y++)
				if ((rt_Squibble[0].X+rt_Squibble[0].Width) < rt_Squibble[9].X)
				{ // Close the circle if not done allready
				rt_Squibble[9].Width = rt_Squibble[9].X-(rt_Squibble[0].X+rt_Squibble[0].Width);
				rt_Squibble[9].X     = rt_Squibble[0].X+rt_Squibble[0].Width;
			// g.FillRectangle(b,rt_Squibble[9]);
				}
				break;
			}  // for (;rt_Squibble[9].X<rt_Squibble[2].X;)
			*/
			// bool b_once=false;
			//------------------------------------------------------------------------
			// Painting Curve Top/Right [9] ------------------------------------------
			// Version 2 -------------------------------------------------------------
			// - [9] Go from Line Top    [0] to Right Line [2] -----------------------
			//------------------------------------------------------------------------
			rt_Squibble[9].X      = (rt_Squibble[0].X+rt_Squibble[0].Width)-(int)f_1PW;
			rt_Squibble[9].X      = rt_Squibble[0].X+rt_Squibble[0].Width;
			rt_Squibble[9].Y      = rt_Squibble[0].Y;
			int i_RightTY  = rt_Squibble[2].Y-rt_Squibble[9].Y;
			int i_RightTX  = rt_Squibble[2].X-rt_Squibble[9].X; 
			// How wide must the Width(X) be to fill the space durring the Y Loop
			// Amount of Pixels needed accourding to Rectangle for Left side
			int i_PixelRightTX = i_RightTX/i_RightTY;
			if (i_PixelRightTX < 1) // Must be at least one (otherwise BoomBoom) !
				i_PixelRightTX = 1;
			i_PixelRightTX++;  // Assume Rounding error
			i_RightTX = i_RightTY*i_PixelRightTX;
			int i_CountTX=0;
			rt_Squibble[9].X     -= i_PixelRightTX*3;
			rt_Squibble[9].Height = (int)(f_1PH*2);
			rt_Squibble[9].Width  = i_PixelRightTX*3;
			//------------------------------------------------------------------------
			// bool b_Right=false;
			for (;rt_Squibble[9].X<rt_Squibble[2].X;)
			{
				for (int y=1;rt_Squibble[9].Y<rt_Squibble[2].Y;y++)
				{ // some 'Today' text words : BlomboIstHier-Endlich / RumpelstilchenTag
					if (i_RightTX > i_CountTX)
					{ // the present position is inside the bounds
						int i_TestWidth   = rt_Squibble[9].Width-1;
						rt_Squibble[9].X += i_TestWidth;  // Allways change X-Position despite possible Width change
						if ((rt_Squibble[9].X+i_TestWidth) > (rt_Squibble[2].X+(rt_Squibble[2].Width/2)))
						{ // Width must not wider that reserved space
							i_TestWidth = (rt_Squibble[2].X+(rt_Squibble[2].Width/2))-rt_Squibble[9].X;
							rt_Squibble[9].Width  = i_TestWidth;
						}
						i_CountTX            += rt_Squibble[9].Width;
					}   // if (i_RightTX > i_CountBX)
					else
					{ // the present position is outside the bounds
						rt_Squibble[9].Width  = rt_Squibble[9].Width/2;
						rt_Squibble[9].X      = (rt_Squibble[2].X+(rt_Squibble[2].Width/2))-rt_Squibble[9].Width;
						i_CountTX += rt_Squibble[9].Width;
					}
					rt_Squibble[9].Y     += (int)f_1PH;
					g.FillRectangle(b,rt_Squibble[9]);
				} // for (int y=1;rt_Squibble[2].Y>rt_Squibble[9].Y;y++)
				break;
			}  // for (;rt_Squibble[9].X>rt_Squibble[2].X;)
			//------------------------------------------------------------------------
			// Painting Curve Top/Left [6] ; Bottom/Left [7] ; Bottom/Right [8] ------
			// Version 2 -------------------------------------------------------------
			// - [8] Go from Line Bottom [1] to Right Line [2] ------------------------
			//------------------------------------------------------------------------
			rt_Squibble[8].X      = (rt_Squibble[1].X+rt_Squibble[1].Width)-(int)(f_1PW*2);
			rt_Squibble[8].X      = (rt_Squibble[1].X+rt_Squibble[1].Width);
			rt_Squibble[8].Y      = rt_Squibble[1].Y+(int)f_1PH;
			int i_RightBY  = rt_Squibble[8].Y-(rt_Squibble[2].Y+rt_Squibble[2].Height);
			int i_RightBX  = rt_Squibble[2].X-rt_Squibble[8].X;   // = 9;
			// How wide must the Width(X) be to fill the space durring the Y Loop
			// Amount of Pixels needed accourding to Rectangle for Left side
			int i_PixelRightBX = i_RightBX/i_RightBY;
			if (i_PixelRightBX < 1) // Must be at least one (otherwise BoomBoom) !
				i_PixelRightBX = 1;
			if (i_BaseSize == 1)
			{ // Very Wide Width (like 'Aujourd'hui') or very Slim like 'Hoy' or even 'I Dag'
				i_PixelRightBX++;  // Make it wider
			}
			i_RightBX = i_RightBY*i_PixelRightBX;
			int i_CountBX=0;
			rt_Squibble[8].X     -= i_PixelRightBX*2;
			rt_Squibble[8].Height = (int)(f_1PH*2);
			rt_Squibble[8].Width  = i_PixelRightBX*2;
			//------------------------------------------------------------------------
			// b_Right=false;
			for (;rt_Squibble[8].X<rt_Squibble[2].X;)
			{
				for (int y=1;(rt_Squibble[2].Y+rt_Squibble[2].Height)<rt_Squibble[8].Y;y++)
				{ // some 'Today' text words : BlomboIstHier-Endlich / RumpelstilchenTag
					if (i_RightBX > i_CountBX)
					{ // the present position is inside the bounds
						int i_TestWidth   = rt_Squibble[8].Width-1;
						rt_Squibble[8].X += i_TestWidth;  // Allways change X-Position despite possible Width change
						if ((rt_Squibble[8].X+i_TestWidth) > (rt_Squibble[2].X+(rt_Squibble[2].Width/2)))
						{ // Width must not wider that reserved space
							i_TestWidth = (rt_Squibble[2].X+(rt_Squibble[2].Width/2))-rt_Squibble[8].X;
							rt_Squibble[8].Width  = i_TestWidth;
						}
						i_CountBX            += rt_Squibble[8].Width;
					}   // if (i_RightBX > i_CountBX)
					else
					{ // the present position is outside the bounds
						rt_Squibble[8].Width  = rt_Squibble[8].Width/2;
						rt_Squibble[8].X      = (rt_Squibble[2].X+(rt_Squibble[2].Width/2))-rt_Squibble[8].Width;
						i_CountBX += rt_Squibble[8].Width;
					}
					rt_Squibble[8].Y     -= (int)f_1PH;
					g.FillRectangle(b,rt_Squibble[8]);
				} // for (int y=1;rt_Squibble[2].Y>rt_Squibble[8].Y;y++)
				break;
			}  // for (;rt_Squibble[8].X>rt_Squibble[2].X;)
			//------------------------------------------------------------------------
			// Painting Curve Top/Left [6] ; Bottom/Left [7] ; Bottom/Right [8] ------
			// Version 2 -------------------------------------------------------------
			// - [6] Go from Line Top 2  [4] to Left  Line [3] ------------------------
			// - [7] Go from Line Bottom [1] to Left  Line [3] ------------------------
			//------------------------------------------------------------------------
			int i_LeftPY  = (rt_Squibble[3].Y-(int)(f_1PH))-rt_Squibble[4].Y;
			int i_LeftPX  = rt_Squibble[4].X-rt_Position.X;
			// How wide must the Width(X) be to fill the space durring the Y Loop
			// Amount of Pixels needed accourding to Rectangle for Left side
			int i_PixelLeftX  = i_LeftPX/i_LeftPY;
			i_PixelLeftX++;  // Avoid rounding errors, will be reduced when needed later to fit.
			if (i_PixelLeftX < 1) // Must be at least one (otherwise BoomBoom) !
				i_PixelLeftX = 1;
			i_LeftPX = i_LeftPY*i_PixelLeftX;
			int i_CountLX=0;
			rt_Squibble[6].X      = rt_Squibble[4].X;
			rt_Squibble[6].Height = (int)(f_1PH*2);
			rt_Squibble[6].Y      = rt_Squibble[4].Y;
			rt_Squibble[6].Width  = i_PixelLeftX;
			rt_Squibble[7]        = rt_Squibble[6];
			rt_Squibble[7].Y      = rt_Squibble[1].Y;
			//------------------------------------------------------------------------
			bool b_Left=false;
			for (;rt_Squibble[6].X>rt_Squibble[3].X;)
			{
				for (int y=1;rt_Squibble[3].Y>(rt_Squibble[6].Y);y++)
				{  // BlomboIstHier-Endlich / RumpelstilchenTag
					if (i_LeftPX > i_CountLX)
					{
						if (((rt_Squibble[6].X-rt_Squibble[6].Width) <= rt_Squibble[3].X) || (b_Left))
						{ // the present position is outside the bounds
							int i_TestWidth   = rt_Squibble[6].Width/2;
							if (i_TestWidth == 0) // Must be at least one (otherwise NoPleasure) !
								i_TestWidth = 1;
							if ((rt_Squibble[6].X-i_TestWidth) < rt_Squibble[3].X)
							{  
								i_TestWidth   = i_TestWidth/2;
								if (i_TestWidth == 0) // Must be at least one (otherwise NoPleasure) !
									i_TestWidth = 1;
								rt_Squibble[6].X       = rt_Squibble[3].X;
								rt_Squibble[7].X       = rt_Squibble[3].X;
								rt_Squibble[6].Width   = i_TestWidth;
								rt_Squibble[7].Width   = i_TestWidth;
							}
							else
							{ // "Today", "Heute"
								b_Left=true;
								rt_Squibble[6].X       = rt_Squibble[3].X;
								rt_Squibble[7].X       = rt_Squibble[3].X;
							} // else ((rt_Squibble[6].X-i_TestWidth) < rt_Squibble[3].X)
						}
						else
						{ // the present position is inside the bounds
							if (!b_Left)
							{
								rt_Squibble[6].X     -= i_PixelLeftX;
								rt_Squibble[7].X     -= i_PixelLeftX;
							}
						}  // else (((rt_Squibble[6].X-rt_Squibble[6].Width) < rt_Squibble[3].X) || (b_Left))
						i_CountLX += i_PixelLeftX;;
					}   // if (i_LeftPX > i_CountLX)
					rt_Squibble[6].Y     += (int)f_1PH;
					rt_Squibble[7].Y     -= (int)f_1PH;
					g.FillRectangle(b,rt_Squibble[6]);
					g.FillRectangle(b,rt_Squibble[7]);
				} // for (int y=1;rt_Squibble[3].Y>(rt_Squibble[6].Y);y++
				break;
			}  // (;rt_Squibble[6].X>rt_Squibble[3].X;)
			//------------------------------------------------------------------------
			//--Allow User to reposition Text to fit insided Squibble ----------------
			//------------------------------------------------------------------------
			rt_Position.X += (int)f_1PW;
			rt_Position.Y += (int)f_1PH;
			//------------------------------------------------------------------------
			return;
		} // protected void OnPaintSquibble(Rectangle rt_Position)
	
		
		#endregion
	
		#endregion Helper Methods

		#region String Localization
		
		/// <summary>
		///  <list type="table">
		///   <item><description>OnToday</description></item>
		///  </list>
		/// </summary>
		/// <remarks>
		///  <para>Sets s_Today text accourding to supported Culture</para>
		///  <para>- add your support here</para>
		/// </remarks>
		public void OnDaysOfWeek()
		{ 
			// Set the Month to look at in the Year 2004.
			int i_Month = 2;
			if (ci_FirstDayOfWeek == DayOfWeek.Thursday)
			{
				i_Month = 1;          // 01.01.2004
			}
			if (ci_FirstDayOfWeek == DayOfWeek.Friday)
			{
				i_Month = 10;         // 01.10.2004
			}
			if (ci_FirstDayOfWeek == DayOfWeek.Saturday)
			{
				i_Month = 5;          // 01.05.2004
			}
			if (ci_FirstDayOfWeek == DayOfWeek.Sunday)
			{
				i_Month = 2;          // 01.02.2004
			}
			if (ci_FirstDayOfWeek == DayOfWeek.Monday)
			{
				i_Month = 3;          // 01.03.2004
			}
			if (ci_FirstDayOfWeek == DayOfWeek.Tuesday)
			{
				i_Month = 6;          // 01.06.2004
			}
			if (ci_FirstDayOfWeek == DayOfWeek.Wednesday)
			{
				i_Month = 9;          // 01.09.2004
			}
			// Get the letters of the Days of the Week, accourding the FirstDayofWeek Value
			s_DayOfWeek = new string[7];
			for (int i=1;i<=s_DayOfWeek.Length;i++)
			{
				DateTime dt = new DateTime(2004,i_Month,i);
				int i_DOW = (int)dt.DayOfWeek;
				
				s_DayOfWeek[i-1] = isGregorian ? CultureInfo.InvariantCulture.DateTimeFormat.DayNames[i_DOW]: EthiopianCalendar.ToString(dt.DayOfWeek);
			}
		} // public void OnDaysOfWeek()

		
		#endregion String Localization
	}
		
	
	#endregion DayPickerPopup class	

	#region Squibble enumeration
		
	/// <summary>
	///  <list type="table">
	///   <item><description>Specifies the Type to use to show the "Today" Date in the Control</description></item>
	///  </list>
	/// </summary>
	/// <remarks>
	///  <para>Support to let user deside which type he would like to use</para>
	/// </remarks>
	public enum SquibbleType
	{
		
		/// <summary>
		/// The Desktop Squibble using DrawPolygon
		/// </summary>
		Squibble,
		
		/// <summary>
		/// The WinCe-API using DrawRectangle
		/// </summary>
		Rectangle,
		
		/// <summary>
		/// The first attempt using Draw Ellipse
		/// </summary>
		Ellipse
		
	}

		
	#endregion Squibble enumeration		
}
