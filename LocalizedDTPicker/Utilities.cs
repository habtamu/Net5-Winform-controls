using System;
using System.Drawing;
using System.Drawing.Drawing2D;   
using System.Windows.Forms;

namespace CalendarLib
{
	internal class Utilities
	{
		#region Static Methods
	
		/// <summary>
		/// Returns an Int value by concatinating the new value from the key code
		/// </summary>
		/// <param name="prevValue">The previous integer value</param>
		/// <param name="keyData">The KeyData which holds the info which key was pressed</param>
		/// <returns>The new intger value</returns>
		public static int IntValue(int prevValue,Keys keyData)
		{
			try
			{
				return int.Parse(string.Format("{0}{1}",prevValue,IntValue(keyData)));  
			}
			catch(Exception ex)
			{
				System.Diagnostics.Trace.WriteLine(string.Format("Utilities::IntValue(int prevValue,Keys keyData) {0} {1}",ex.GetType(),ex.Message)); 
			}
		
			return prevValue;
		}

		
		/// <summary>
		/// Returns an Int value by parsing the new value from the key code
		/// </summary>
		/// <param name="keyData">The KeyData which holds the info which key was pressed</param>
		/// <returns>The new intger value</returns>
		public static int IntValue(Keys keyData)
		{
			switch (keyData)
			{
				case Keys.D0:
				case Keys.NumPad0:
				{
					return 0;
				}
				
				case Keys.D1:
				case Keys.NumPad1:
				{
					return 1;
				}
   
				case Keys.D2:
				case Keys.NumPad2:
				{
					return 2;
				}
				
				case Keys.D3:
				case Keys.NumPad3:
				{
					return 3;
				}

				case Keys.D4:
				case Keys.NumPad4:
				{
					return 4;
				}

				case Keys.D5:
				case Keys.NumPad5:
				{
					return 5;
				}
 
				case Keys.D6:
				case Keys.NumPad6:
				{
					return 6;
				}

				case Keys.D7:
				case Keys.NumPad7:
				{
					return 7;
				}
				
				case Keys.D8:
				case Keys.NumPad8:
				{
					return 8;
				}
  
				case Keys.D9:
				case Keys.NumPad9:
				{
					return 9;
				}
			}
			return 0;
		}
	

		#endregion Static Methods
		
		#region Private Constructor
		
		/// <summary>
		/// Private Constructor (No Instance Allowed)
		/// </summary>
		private Utilities()
		{
		
		}
		
		
		#endregion Private Constructor	
	}
	
	#region class ThemedBrushes

	internal class ThemedBrushes
	{
		public static readonly SolidBrush   ButtonCorner			=new SolidBrush(Color.FromArgb(217,227,246));
		public static readonly SolidBrush	ButtonLabel				=new SolidBrush(Color.FromArgb(77,97,133)); 
	
		#region Private Constructor

		private ThemedBrushes()
		{}

		
		#endregion Private Constructor	
	}

	#endregion class ThemedBrushes

	#region class ThemedColors

	internal class ThemedColors
	{
		public static readonly Color ThemeStartNormal		=Color.FromArgb(225,234,255);
		public static readonly Color ThemeEndNormal			=Color.FromArgb(149,183,242);
		public static readonly Color ThemeStartSelected		=Color.FromArgb(110,142,241);
		public static readonly Color ThemeEndSelected		=Color.FromArgb(210,222,235);		

		#region Private Constructor

		private ThemedColors()
		{}

		
		#endregion Private Constructor		
	}

	#endregion class ThemedColors

	#region class ThemedPens
		
	internal class ThemedPens
	{	
		public static readonly Pen ButtonBorder		= new Pen(Color.FromArgb(152,182,252),1);
		public static readonly Pen ButtonLabel		= new Pen(Color.FromArgb(77,97,133),2F);
		public static readonly Pen TextBoxBorder	= new Pen(Color.FromArgb(127,157,185));		

		#region Private Constructor

		private ThemedPens()
		{}

		
		#endregion Private Constructor	
	}

	#endregion class ThemedPens

	internal class ThemedDrawing
	{
		#region Drawing Methods
	
		/// <summary>
		/// Draws a Themed button with the given graphics object
		/// in speciified Rectangle and with the state specified, and the 
		/// the Points which are used to draw interconnected lines
		/// </summary>
		/// <param name="g">The Graphics object to draw on</param>
		/// <param name="rect">The client rect</param>
		/// <param name="selected">The Button is in selected or not</param>
		/// <param name="points">The array of points which are used to draw the lines</param>
		public static void DrawButton(Graphics g,RectangleF rect,bool selected,PointF[] points)
		{
			DrawButton(g,rect,selected);
			g.DrawLines(ThemedPens.ButtonLabel,points);		
		}
	

		/// <summary>
		/// Draws a Themed button with the given graphics object
		/// in speciified Rectangle and with the state specified, and the 
		/// the text on the button.
		/// </summary>
		/// <param name="g">The Graphics object to draw on</param>
		/// <param name="rect">The client rect</param>
		/// <param name="selected">The Button is in selected or not</param>		
		/// <param name="text">The Text to be Drawn on the button</param>		
		/// <param name="font">The Font used to draw the text</param>
		/// <param name="format">The format on which text shoul be drawn</param>
		public static void DrawButton(Graphics g,RectangleF rect,bool selected,string text,Font font,StringFormat format)
		{
			DrawButton(g,rect,selected);
			g.DrawString(text,font,ThemedBrushes.ButtonLabel,rect,format);   
		}

		
		/// <summary>
		/// Draws a Themed button with the given graphics object
		/// in speciified Rectangle and with the state specified
		/// </summary>
		/// <param name="g">The Graphics object to draw on</param>
		/// <param name="rect">The client rect</param>
		/// <param name="selected">The Button is in selected or not</param>		
		public static void DrawButton(Graphics g,RectangleF rect,bool selected)
		{
			using ( LinearGradientBrush linearBrush = selected ?
						new LinearGradientBrush(rect,ThemedColors.ThemeStartSelected ,ThemedColors.ThemeEndSelected ,LinearGradientMode.ForwardDiagonal) :
						new LinearGradientBrush(rect,ThemedColors.ThemeStartNormal ,ThemedColors.ThemeEndNormal,LinearGradientMode.ForwardDiagonal)
						)
			{
				
				g.FillRectangle(linearBrush,rect);
			}					
					
			g.DrawLine(ThemedPens.ButtonBorder ,rect.Left,rect.Top,rect.Left,rect.Bottom);
			g.DrawLine(ThemedPens.ButtonBorder ,rect.Left,rect.Top,rect.Right,rect.Top);
			g.DrawLine(ThemedPens.ButtonBorder ,rect.Right,rect.Top,rect.Right,rect.Bottom);
			g.DrawLine(ThemedPens.ButtonBorder  ,rect.Left,rect.Bottom,rect.Right,rect.Bottom);
			
			float offset=0.5F;
			
			g.FillRectangle( ThemedBrushes.ButtonCorner,RectangleF.FromLTRB(rect.Left,rect.Top,rect.Left + offset,rect.Top + offset));

			g.FillRectangle( ThemedBrushes.ButtonCorner,RectangleF.FromLTRB(rect.Left,rect.Bottom,rect.Left + offset,rect.Bottom + offset));

			g.FillRectangle( ThemedBrushes.ButtonCorner,RectangleF.FromLTRB(rect.Right,rect.Top,rect.Right + offset,rect.Top  + offset));

			g.FillRectangle( ThemedBrushes.ButtonCorner,RectangleF.FromLTRB(rect.Right,rect.Bottom,rect.Right + offset,rect.Bottom  + offset));			
			
		}
	

		/// <summary>
		/// Draws a Border around the Rectangle given
		/// </summary>
		/// <param name="g">The Graphics object used</param>
		/// <param name="rect">The Rectangle about which the border is going to be drawn</param>
		public static void DrawBorder(Graphics g,RectangleF rect)
		{
			g.DrawRectangle(ThemedPens.TextBoxBorder,rect.X,rect.Y,rect.Width,rect.Height);
		}	


		#endregion Drawing Methods

		#region Private Constructor
		
		private ThemedDrawing()
		{}
	

		#endregion Private Constructor
	}
}
