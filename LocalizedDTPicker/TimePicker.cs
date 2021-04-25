using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CalendarLib
{
	/// <summary>
	/// Used to pick time for the calendar
	/// </summary>
	[System.ComponentModel.DefaultEvent("ValueChanged")]
	[ToolboxItem(true),ToolboxBitmap(typeof(TimePicker))]
	public class TimePicker : System.Windows.Forms.UserControl
	{
		#region Design Vars
		
		private System.Windows.Forms.Panel		panelTimePicker;
		private TextBoxTime	txtHour;
		private System.Windows.Forms.Label		labelSeparator;
		private TextBoxTime	txtMinute;
		private TextBoxTime	txtAM_PM;

		#endregion Design Vars
		
		#region Required Design Var

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion Required Design Var

		#region Dispose

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		
		#endregion Dispose

		#region Component Designer generated code
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panelTimePicker = new System.Windows.Forms.Panel();
			this.labelSeparator = new System.Windows.Forms.Label();
			this.txtAM_PM = new CalendarLib.TextBoxTime();
			this.txtMinute = new CalendarLib.TextBoxTime();
			this.txtHour = new CalendarLib.TextBoxTime();
			this.panelTimePicker.SuspendLayout();
			this.SuspendLayout();
			// 
			// panelTimePicker
			// 
			this.panelTimePicker.Controls.Add(this.labelSeparator);
			this.panelTimePicker.Controls.Add(this.txtAM_PM);
			this.panelTimePicker.Controls.Add(this.txtMinute);
			this.panelTimePicker.Controls.Add(this.txtHour);
			this.panelTimePicker.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelTimePicker.Location = new System.Drawing.Point(0, 0);
			this.panelTimePicker.Name = "panelTimePicker";
			this.panelTimePicker.Size = new System.Drawing.Size(86, 22);
			this.panelTimePicker.TabIndex = 0;
			// 
			// labelSeparator
			// 
			this.labelSeparator.Font = new System.Drawing.Font("Ethiopia Jiret", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelSeparator.Location = new System.Drawing.Point(27, 3);
			this.labelSeparator.Name = "labelSeparator";
			this.labelSeparator.Size = new System.Drawing.Size(6, 13);
			this.labelSeparator.TabIndex = 3;
			this.labelSeparator.Text = ":";
			this.labelSeparator.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// txtAM_PM
			// 
			this.txtAM_PM.BackColor = System.Drawing.SystemColors.Window;
			this.txtAM_PM.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtAM_PM.Font = new System.Drawing.Font("Ethiopia Jiret", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtAM_PM.HideSelection = false;
			this.txtAM_PM.Location = new System.Drawing.Point(51, 3);
			this.txtAM_PM.MaxLength = 4;
			this.txtAM_PM.Name = "txtAM_PM";
			this.txtAM_PM.ReadOnly = true;
			this.txtAM_PM.Size = new System.Drawing.Size(30, 13);
			this.txtAM_PM.TabIndex = 2;
			this.txtAM_PM.TabStop = false;
			this.txtAM_PM.Text = "";
			this.txtAM_PM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtAM_PM.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtAM_PM_KeyUp);
			// 
			// txtMinute
			// 
			this.txtMinute.BackColor = System.Drawing.SystemColors.Window;
			this.txtMinute.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtMinute.Font = new System.Drawing.Font("Ethiopia Jiret", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtMinute.HideSelection = false;
			this.txtMinute.Location = new System.Drawing.Point(33, 3);
			this.txtMinute.MaxLength = 2;
			this.txtMinute.Name = "txtMinute";
			this.txtMinute.ReadOnly = true;
			this.txtMinute.Size = new System.Drawing.Size(20, 13);
			this.txtMinute.TabIndex = 1;
			this.txtMinute.TabStop = false;
			this.txtMinute.Text = "";
			this.txtMinute.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtMinute_KeyUp);
			// 
			// txtHour
			// 
			this.txtHour.BackColor = System.Drawing.SystemColors.Window;
			this.txtHour.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.txtHour.Font = new System.Drawing.Font("Ethiopia Jiret", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtHour.HideSelection = false;
			this.txtHour.Location = new System.Drawing.Point(6, 3);
			this.txtHour.MaxLength = 2;
			this.txtHour.Name = "txtHour";
			this.txtHour.ReadOnly = true;
			this.txtHour.Size = new System.Drawing.Size(20, 13);
			this.txtHour.TabIndex = 0;
			this.txtHour.Text = "";
			this.txtHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.txtHour.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtHour_KeyUp);
			// 
			// TimePicker
			// 
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.Add(this.panelTimePicker);
			this.Name = "TimePicker";
			this.Size = new System.Drawing.Size(86, 22);
			this.panelTimePicker.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		
		
		#endregion

		#region Member Vars
		
		/// <summary>
		/// The current time value that is being displayed
		/// </summary>
		private DateTime curDateTime;

		/// <summary>
		/// The minimum time that can be set
		/// </summary>
		private DateTime minDateTime;

		/// <summary>
		/// The Maximum Time that can be set
		/// </summary>
		private DateTime maxDateTime;

		/// <summary>
		/// The minimum Time values that can be set
		/// </summary>
		private Time	minTime;

		/// <summary>
		/// The maximum Time values that can be set
		/// </summary>
		private Time	maxTime;

		/// <summary>
		/// The current time values the user is editing on
		/// </summary>
		private Time	curTime;
		
		/// <summary>
		/// Gregorian calendar to use or Ethiopian calendar to use
		/// </summary>
		private bool	isGregorian;

		#endregion Member Vars.
		
		#region Properties

		/// <summary>
		/// gets/sets the value for the time picker control
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DateTime Value
		{		
			get
			{
				return curDateTime; 
			}
		
			
			set
			{
				if(value!=curDateTime) 
				{
					UpdateCurDateTime(value);
				}
			}		
		}

		
		/// <summary>
		/// gets/sets the minimum time that can be set
		/// throws ArgumentException if set value is greater than value in maxDateTime
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DateTime MinDateTime
		{
			get
			{
				return minDateTime;			
			}
		
			
			set
			{
				if(value>maxDateTime)
				{
					throw new ArgumentException("'"+value.ToString("g") +"' is not a valid value for 'MinDateTime'. 'MinDateTime' should be less than 'MaxDateTime'.");
				}

				minDateTime=value;
				
				//If current vale is less than the minimum datetime that is set
				DateTime dateOnlyValue		=new DateTime(Value.Year,Value.Month,Value.Day);
				DateTime dateOnlyMinDateTime=new DateTime(value.Year,value.Month,value.Day);

				if(!(dateOnlyValue < dateOnlyMinDateTime))
				{
					if(	!isGregorian && 
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
		}

		
		/// <summary>
		/// gets/sets the maximum value that can be set
		/// throws ArgumentException if set value is less than value in minDateTime
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public DateTime MaxDateTime
		{
			get
			{
				return maxDateTime;  
			}

			
			set
			{
				if(value < minDateTime)
				{
					throw new ArgumentException("'"+value.ToString("g") +"' is not a valid value for 'MaxDateTime'. 'MaxDateTime' should be greater than 'MinDateTime'.");
				}

				maxDateTime=value;
				
				//If current vale is less than the minimum datetime that is set
				DateTime dateOnlyValue		=new DateTime(Value.Year,Value.Month,Value.Day);
				DateTime dateOnlyMinDateTime=new DateTime(value.Year,value.Month,value.Day);

				if(!(dateOnlyValue > dateOnlyMinDateTime))
				{
					if(	!isGregorian && 
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
			}
		}

		
		/// <summary>
		/// gets/sets wheter the time picker is operating
		/// in the gregorian or the ethiopian calendar
		/// </summary>
		[System.ComponentModel.Browsable(true)]
		[DefaultValue(false)]
		public bool IsGregorian
		{
			get
			{
				return isGregorian; 
			}
			
			
			set
			{
				if(value != isGregorian)
				{
					isGregorian=value;
					UpdateCurDateTime(curDateTime);
				}
			}
		}


		#endregion Properties
		
		#region Constructor

		/// <summary>
		/// Must be called from the constructor of the DateTimePickerEx
		/// </summary>
		public TimePicker()
		{
			this.SetStyle(ControlStyles.Selectable,true);
			
			//Set the current time 
			
			curDateTime		= DateTime.Now;			//Get the current time
			isGregorian		= false;
			minTime			= new Time(0,0,0);
			maxTime			= new Time(23,59,59);
			curTime			= new Time();				
			
			MaxDateTime		= new DateTime(9997, 8, 7);
			MinDateTime		= new DateTime(1753, 8, 7);
									
			Value			= curDateTime;				//Used to set min max limit Value
			
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			this.panelTimePicker.Paint		+=new PaintEventHandler(panelTimePicker_Paint); 

			DisplayTime(); 
		}


		#endregion Constructor
 
		#region EventHander
		
		/// <summary>
		/// Raised when the value is changed
		/// </summary>
		public event EventHandler ValueChanged;

		#endregion EventHander

		#region Methods

		/// <summary>
		/// Updates the current time by the new time given
		/// Raises the ValueChanged event if the new value is different
		/// from the previous one.
		/// </summary>
		/// <param name="newDateTime">new DateTime object having the new time</param>
		protected void UpdateCurDateTime(DateTime newDateTime)
		{
			if (newDateTime   < MinDateTime)
			{
				if(	!isGregorian && 
					newDateTime.Year	== MinDateTime.Year &&
					newDateTime.Month	== MinDateTime.Month  &&
					newDateTime.Day		== MinDateTime.Day )
				{
					EthiopianCalendar calendar=new EthiopianCalendar();
					int newHour;
					int minHour;
					newHour=calendar.GetHour(newDateTime);
					minHour=calendar.GetHour(MinDateTime);
					if(newHour < minHour)
					{
						newDateTime		= MinDateTime ;
					}
					else if(newHour == minHour && newDateTime.Minute < MinDateTime.Minute)
					{
						newDateTime		= MinDateTime ;
					}				
				}
				else
				{
					newDateTime		= MinDateTime ;
				}
			}
			else if (newDateTime  > MaxDateTime)
			{
				if(	!isGregorian && 
					newDateTime.Year	== MaxDateTime.Year &&
					newDateTime.Month	== MaxDateTime.Month  &&
					newDateTime.Day		== MaxDateTime.Day )
				{
					EthiopianCalendar calendar=new EthiopianCalendar();
					int newHour;
					int maxHour;
					newHour=calendar.GetHour(newDateTime);
					maxHour=calendar.GetHour(MaxDateTime);
					if(newHour > maxHour)
					{
						newDateTime		= MaxDateTime ;
					}
					else if(newHour == maxHour  && newDateTime.Minute > MaxDateTime.Minute)
					{
						newDateTime		= MaxDateTime ;
					}				
				}
				else
				{
					newDateTime		= MaxDateTime;				
				}
			}		

			// see if should raise ValueChanged event
			bool raiseEvent = (curDateTime  != newDateTime ) ? true : false;

			// store new date selection
			curDateTime		  = newDateTime ;

			SetMinMaxLimits();
			DisplayTime(); 
			
			// Repaint
			//Invalidate();
			//Update();
			
			// raise ValueChanged event
			if (this.ValueChanged != null && raiseEvent)
				ValueChanged(this, EventArgs.Empty);
		}

		
		/// <summary>
		/// Sets the minimum and maximum limits
		/// on the hour and minute ranges if a limit really exists
		/// Wheter the clanedar is gregorian or ethiopian
		/// has an effec in the way the limits are caluclated 
		/// </summary>
		private void SetMinMaxLimits()
		{
			//Set the minimum and maximum limits on their default values
			minTime.Hour  		=0;
			minTime.Minute  	=0;
			maxTime.Hour  		=23;
			maxTime.Minute  	=59;
			
			//If we are on the same year, month and day to the minTime limit
			//Then set the minimum limits for the hour and minute
			if(	curDateTime.Year == minDateTime.Year &&
				curDateTime.Month == minDateTime.Month &&
				curDateTime .Day == minDateTime.Day)
			{
				if(isGregorian) 
				{
					minTime.Hour	=minDateTime.Hour;
					minTime.Minute	=minDateTime.Minute;
				}
				else
				{
					minTime.Hour	=int.Parse(EthiopianCalendar.ToString(minDateTime,"HH"));
					minTime.Minute	=int.Parse(EthiopianCalendar.ToString(minDateTime,"mm"));
				}
			}
			//If we are on the same year, month and day to the maxTime limit
			//Then set the maximum limits for the hour and minute
			if(	curDateTime.Year == maxDateTime.Year &&
				curDateTime.Month == maxDateTime.Month &&
				curDateTime.Day == maxDateTime.Day)
			{
				if(isGregorian)
				{
					maxTime.Hour		=maxDateTime.Hour;
					maxTime.Minute   	=maxDateTime.Minute;
				}
				else
				{
					maxTime.Hour		=int.Parse(EthiopianCalendar.ToString(maxDateTime,"HH"));
					maxTime.Minute   	=int.Parse(EthiopianCalendar.ToString(maxDateTime,"mm"));
				}
			}
			
			//Set the current  hour and minute values
			curTime.Minute   	=curDateTime.Minute;
			if(isGregorian) 
			{
				curTime.Hour		=curDateTime.Hour;
			}
			else
			{
				curTime.Hour		=int.Parse(EthiopianCalendar.ToString(curDateTime,"HH"));
			}
			
			if(curTime.Hour > maxTime.Hour)
			{
				if(curTime.Hour == 23)
				{
					curTime.Hour=maxTime.Hour;   
				}
				else
				{
					curTime.Hour=minTime.Hour;   
				}
			}
			if(curTime.Hour < minTime.Hour)
			{
				if(curTime.Hour == 0)
				{
					curTime.Hour=minTime.Hour;  
				}
				else
				{
					curTime.Hour=maxTime.Hour;  
				}
			}
		
			if( curTime.Hour == maxTime.Hour &&   curTime.Minute > maxTime.Minute )
			{
				curTime.Minute =maxTime.Minute ;   
			}
			if(curTime.Hour == minTime.Hour && curTime.Minute  < minTime.Minute )
			{
				curTime.Minute =minTime.Minute;
			}
		}


		/// <summary>
		/// Updates the DateTime Object we have with
		/// the new values from the user.
		/// </summary>
		private void SetTime()
		{
			if(curTime.Hour > maxTime.Hour)
			{
				if(curTime.Hour == 23)
				{
					curTime.Hour=maxTime.Hour;   
				}
				else
				{
					curTime.Hour=minTime.Hour;   
				}
			}
			if(curTime.Hour < minTime.Hour)
			{
				if(curTime.Hour == 0)
				{
					curTime.Hour=minTime.Hour;  
				}
				else
				{
					curTime.Hour=maxTime.Hour;  
				}
			}
		
			if( curTime.Hour == maxTime.Hour &&   curTime.Minute > maxTime.Minute )
			{
				curTime.Minute =maxTime.Minute ;   
			}
			if(curTime.Hour == minTime.Hour && curTime.Minute  < minTime.Minute )
			{
				curTime.Minute =minTime.Minute;
			}

					
			if(isGregorian)
			{
				this.Value=new DateTime(Value.Year,Value.Month,Value.Day,curTime.Hour,curTime.Minute,curTime.Second,Value.Millisecond);
			}
			else
			{
				this.Value=new DateTime(Value.Year,Value.Month,Value.Day,(( 24+ curTime.Hour + 6) % 24 ),curTime.Minute,curTime.Second,Value.Millisecond);
			}
		}
		
	
		/// <summary>
		/// Displays the value of the Time in the DateTime object
		/// </summary>
		protected void DisplayTime()
		{
			if(txtHour==null || txtMinute==null || txtAM_PM==null)
				return;
			if(isGregorian)
			{
			txtHour		.Text=Value.ToString("hh");
			txtMinute	.Text=Value.ToString("mm");
			txtAM_PM	.Text=Value.ToString("tt");
			}
			else
			{
			txtHour		.Text=EthiopianCalendar.ToString(Value,"hh");
			txtMinute	.Text=EthiopianCalendar.ToString(Value,"mm");
			txtAM_PM	.Text=EthiopianCalendar.ToString(Value,"ttt");
			}		
		}


		#endregion Methods

		#region Overriden

		/// <summary>
		/// Sets the Bounds for the control the limits sepcified
		/// </summary>
		/// <param name="x">The x location</param>
		/// <param name="y">The y location</param>
		/// <param name="width">The width to be set</param>
		/// <param name="height">The height to be set</param>
		/// <param name="specified">Which parameter is specified</param>
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			   base.SetBoundsCore (x, y, 86,22, specified);
		}



		#endregion Overriden

		#region Events

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtHour_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyData == Keys.Up || e.KeyData ==Keys.Down)
			{
				if(e.KeyData == Keys.Up)
				{
					curTime.Hour=curTime.Hour + 1; 
				}
				else if (e.KeyData ==Keys.Down)
				{
					curTime.Hour=curTime.Hour-1; 
				}
				SetTime();
			}
			else if(e.KeyData ==Keys.Right)
			{
				this.txtMinute.Select();
			}
			else if(e.KeyData ==Keys.Left)
			{
				this.txtAM_PM.Select();
			}
		}

		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtMinute_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyData == Keys.Up || e.KeyData ==Keys.Down)
			{
				if(e.KeyData == Keys.Up)
				{
					curTime.Minute	= curTime.Minute + 1; 
				}
				else if (e.KeyData ==Keys.Down)
				{
					curTime.Minute	= curTime.Minute - 1;
				}
				SetTime();
			}

			else if(e.KeyData ==Keys.Right)
			{
				this.txtAM_PM.Select();  
			}
			else if(e.KeyData ==Keys.Left)
			{
				this.txtHour.Select();  
			}	
		}

		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtAM_PM_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyData == Keys.Up || e.KeyData ==Keys.Down)
			{
				this.curTime.IsAm = !curTime.IsAm;
				SetTime();
			}
		
			else if(e.KeyData ==Keys.Right)
			{
				this.txtHour.Select();  
			}
			else if(e.KeyData ==Keys.Left)
			{
				this.txtMinute.Select();  
			}	
		}


		/// <summary>
		/// When the Panel Paints
		/// </summary>
		private void panelTimePicker_Paint(object sender, PaintEventArgs e)
		{
			Rectangle  rect=this.panelTimePicker.Bounds;
			rect.Inflate(-1,-1); 
			e.Graphics.DrawRectangle(ThemedPens.TextBoxBorder,rect);
		}

		
		#region Commented
		//		private void textBox1_Enter(object sender, System.EventArgs e)
		//		{
		//			//	textBox1.Select(0,textBox1.Text.Length);    
		//		}
		//
		//		
		//		private void textBox1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		//		{
		//			if(e.KeyData == Keys.Up || e.KeyData == Keys.Down  )
		//			{
		//				int hour=1;
		//				try
		//				{
		//					hour=int.Parse(textBox1.Text);
		//				}
		//				catch
		//				{
		//				}
		//
		//				if(e.KeyData == Keys.Up )
		//				{
		//					hour++;
		//				}
		//
		//				if(e.KeyData == Keys.Down)
		//				{
		//					hour--;
		//				}
		//				
		//				if(hour>12)
		//				{
		//					hour=12;
		//				}
		//
		//				if(hour<1)
		//				{
		//					hour=1;
		//				}
		//				
		//				textBox1.Text=hour.ToString();  
		//			}
		//		}
		//
		//		
		//		private void textBox1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		//		{			
		//			if(!Char.IsDigit(e.KeyChar) )
		//			{
		//				e.Handled =true;
		//			}
		//			else
		//			{
		//				e.Handled =false;
		//			}
		//		}
		//
		//		private void textBox1_TextChanged(object sender, System.EventArgs e)
		//		{
		//			if(textBox1.Text.Length<2)     
		//			{
		//				textBox1.Text="0"+textBox1.Text;  
		//			}
		//			//	textBox1.Select(0,textBox1.Text.Length); 
		//		}
		//
		//		private void textBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		//		{
		//			//textBox1.Select(0,textBox1.Text.Length); 
		//		}
		//
		//		
		//		private void textBox1_Leave(object sender, System.EventArgs e)
		//		{
		//			//textBox1.Select(0,0); 
		//		}

		#endregion Commented

		#endregion Events	

		#region internal class Time

		internal class Time
		{
			#region Member Vars.

			private int		hour;
			private int		minute;
			private int		second;
			private bool	isAM;

			#endregion Member Vars.

			#region Properties

			/// <summary>
			/// gets/sets the hour
			/// </summary>
			public int	Hour
			{
				get
				{
					return hour;
				}
				
				
				set
				{
					if(value<0)
					{
						value=23;
					}
					if(value>23)
					{
						value=0;
					}

					hour=value;

					//What is the hour is it AM or PM
					if(hour >=0 && hour<12)
					{
						isAM =true;
						
					}
					else
					{
						isAM =false;
					}
				}
			}

			
			/// <summary>
			/// gets/sets the minute
			/// </summary>
			public int	Minute
			{
				get
				{
					return minute;
				}

				
				set
				{
					if(value<0)
					{
						value=59;
					}
					if(value>59)
					{
						value=0;
					}

					minute=value;
				}
			}

			
			/// <summary>
			/// gets/sets the second
			/// </summary>
			public int	Second
			{
				get
				{
					return second;
				}
				set
				{
					if(value<0)
					{
						value=0;
					}
					if(value>59)
					{
						value=0;
					}
					second=value;
				}			
			}


			/// <summary>
			/// Sets wheter the time set is AM or PM
			/// </summary>
			public bool IsAm
			{
				get
				{
					return isAM; 
				}
				
				set
				{
					if(isAM !=value)
					{
						if(isAM)
						{
							Hour+=12;						
						}
						else
						{
							Hour-=12;
						}
					}			
				}
			}
			
			
			#endregion Properties

			#region Constructor

			/// <summary>
			/// Constructs the Time object
			/// </summary>
			public Time()
			{
				hour	=0;
				minute	=0;
				second	=0;
				isAM 	=false;
			}

			/// <summary>
			/// Constructs the Time object with the params
			/// </summary>
			/// <param name="hr">An int 0-23 representing hour</param>
			/// <param name="min">An int 0-59 representing minute</param>
			/// <param name="sec">An int 0-59 representing second</param>
			public Time(int hr,int min, int sec)
			{
				Hour	=hr;
				Minute	=min;
				Second	=sec; 
			}

			
			#endregion Constructor
		}

		
		#endregion internal class Time		
	}

	#region class TextBoxTime : TextBox
	
	/// <summary>
	/// A  Text box
	/// 1. Whose BkColor won't be affected by the ReadOnly property
	/// 2. Whose cursel isn't visible
	/// 3. Text selected on activation( focus or mouse click )
	/// 4. Text selection removed on deactivation (lost focus)
	/// </summary>
	[ToolboxItem(false)]
	internal class TextBoxTime : TextBox
	{
		#region Const

		private const int WM_SETFOCUS  = 0x0007;

		#endregion Const

		#region Interop Services

		[DllImport("user32.dll", SetLastError=true)]
		private static extern bool HideCaret(IntPtr hWnd);

		#endregion Interop Services

		#region Constructor

		public TextBoxTime():base()
		{
			this.Enter			+=new EventHandler(TextBoxTime_Enter);
			this.TextChanged	+=new EventHandler(TextBoxTime_TextChanged);
			this.Leave			+=new EventHandler(TextBoxTime_Leave);
			this.Click			+=new EventHandler(TextBoxTime_Click);
			
		}

		
		#endregion Constructor

		#region Properties

		/// <summary>
		/// gets/sets the read only property
		/// for the text  ox
		/// </summary>
		[DefaultValue(false)]
		public new bool ReadOnly
		{
			get { return base.ReadOnly;}
			
			
			set 
			{				 
				base.BackColor	= Color.FromKnownColor (KnownColor.Window);
				base.ReadOnly	= value;
			}
		}

		
		#endregion Properties

		#region Events
		
		/// <summary>
		/// When the text box got focus
		/// </summary>
		private void TextBoxTime_Enter(object sender, EventArgs e)
		{
			//Now Select the whole text
			this.Select(0,this.Text.Length);
		}

		
		/// <summary>
		/// When the text in the text box changes
		/// </summary>
		private void TextBoxTime_TextChanged(object sender, EventArgs e)
		{
			//Only if we have the focus
			if(this.ContainsFocus)
			{
				this.Select(0,this.Text.Length);	//Select the whole text
			}
			else
			{
				this.Select(0,0);					//We don't have focus then remove selection
			}
		}

		
		/// <summary>
		/// When the text box lost focus
		/// </summary>
		private void TextBoxTime_Leave(object sender, EventArgs e)
		{
			this.Select(0,0);   //Then select nothing
		}

		
		/// <summary>
		/// When the user cliked on the  control
		/// </summary>
		private void TextBoxTime_Click(object sender, EventArgs e)
		{
			this.Select(0,this.Text.Length);//Selects the text of cliked on text box
		}


		#endregion Events

		#region WndProc(ref Message m) 
		
		/// <summary>
		/// Overrides the Wnd Proc of the TextBox control
		/// </summary>
		/// <param name="m">Message to be precessed</param>
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name="FullTrust")]
		protected override void WndProc(ref Message m) 
		{
			base.WndProc(ref m);					//Default processing
			
			// Listen for operating system messages.
			switch (m.Msg)
			{
				case WM_SETFOCUS:					//When ever the control has focus
				{	
					if( !HideCaret(this.Handle))	//Hide the caret
					{
						//Error encountered
						System.Diagnostics.Trace.WriteLine(Marshal.GetLastWin32Error().ToString());  
					}	
				}break;
			}			
		}

		
		#endregion WndProc(ref Message m) 

	}

	
	#endregion class TextBoxTime : TextBox

}
