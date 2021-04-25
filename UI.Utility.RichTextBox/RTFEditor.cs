using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace UI.Utility.RichTextBox
{

    #region event delegates

    public delegate void RTFEditorOnOpen(object sender, RTFEditorEventArgs e);
    public delegate void RTFEditorOnSave(object sender, RTFEditorEventArgs e);

    #endregion EventArgs extension and event delegates

    public partial class RTFEditor : UserControl
    {
        #region  Control constructor and load event
        public RTFEditor()
        {
            InitializeComponent();

            //Initialize control values
            tsFontSize.SelectedText = rtb.Font.Size.ToString();
            tsFontType.Text = rtb.Font.FontFamily.Name;

            tsFontSize.Enabled = false;
            tsFontType.Enabled = false;
        }

        /// <summary>
        /// Handles the Load event of the RTFEditor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RTFEditor_Load(object sender, EventArgs e)
        {
            // Set up default values
            this.rtb.AllowDrop = true;

            // Set up default values defined as user-changable
            this.tsFontSize.SelectedText = this.defaultFontSize.ToString();
            this.tsFontType.Text = this.defaultFontFamily;
            this.rtb.WordWrap = this.defaultWordWrap;

            if (String.IsNullOrEmpty(this.rtb.Text))
            {
                this.rtb.Font = new Font(this.defaultFontFamily, this.defaultFontSize, FontStyle.Regular);
            }

            this.tsFontColor.BackColor = this.defaultBackgroundColor;
            this.tsFontColor.ForeColor = this.tsFontColor.BackColor;
            this.tsBackgroundColor.BackColor = this.defaultFontColor;
            this.tsBackgroundColor.ForeColor = this.tsBackgroundColor.BackColor;

            // Default language

            rtb.Focus();
        }

        #endregion

        #region Event handlers

        [Description("Occurs when the Open command is issued.")]
        public event RTFEditorOnOpen OnDocumentOpen;

        [Description("Occurs when the Save command is issued.")]
        public event RTFEditorOnSave OnDocumentSave;

        #endregion Event handlers

        #region Input/output variables and properties

        /// <summary>
        /// Gets or sets the document text.
        /// </summary>
        /// <value>The document text.</value>
        [Description("Default document text in plain text format.")]
        public String DocumentText
        {
            get { return this.rtb.Text; }
            set { this.rtb.Text = value; }
        }

        /// <summary>
        /// Gets or sets the document RTF.
        /// </summary>
        /// <value>The document RTF.</value>
        [Description("Default document text in RTF format.")]
        public String DocumentRtf
        {
            get { return this.rtb.Rtf; }
            set { this.rtb.Rtf = value; }
        }

        /// <summary>
        /// Gets or sets the default font family.
        /// </summary>
        /// <value>The default font family.</value>
        private String defaultFontFamily = "Arial";
        [Description("Default font style.")]
        public String DefaultFontFamily
        {
            get { return this.defaultFontFamily; }
            set { this.defaultFontFamily = value; }
        }

        /// <summary>
        /// Gets or sets the size of the default font.
        /// </summary>
        /// <value>The size of the default font.</value>
        private int defaultFontSize = 8;
        [Description("Default font size.")]
        public int DefaultFontSize
        {
            get { return this.defaultFontSize; }
            set { this.defaultFontSize = value; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [file panel visible].
        /// </summary>
        /// <value><c>true</c> if [file panel visible]; otherwise, <c>false</c>.</value>
        [Description("The file operation (new, open, save) panel is visible (true) or hidden (false).")]
        public bool FilePanelVisible
        {
            get
            {
                if (this.tsNew.Visible)
                { return true; }
                else { return false; }
            }
            set
            {
                this.tsNew.Visible = value;
                this.tsOpen.Visible = value;
                this.tsSave.Visible = value;
                this.tsFileSeparator.Visible = value;

            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable new document].
        /// </summary>
        /// <value><c>true</c> if [enable new document]; otherwise, <c>false</c>.</value>
        [Description("New document operation is enabled (true) or not (false).")]
        public bool EnableNewDocument
        {
            get
            {
                if (this.tsNew.Visible)
                { return true; }
                else { return false; }
            }
            set
            {
                this.tsNew.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable open document].
        /// </summary>
        /// <value><c>true</c> if [enable open document]; otherwise, <c>false</c>.</value>
        [Description("Open document operation is enabled (true) or not (false).")]
        public bool EnableOpenDocument
        {
            get
            {
                if (this.tsOpen.Visible)
                { return true; }
                else { return false; }
            }
            set
            {
                this.tsOpen.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable save document].
        /// </summary>
        /// <value><c>true</c> if [enable save document]; otherwise, <c>false</c>.</value>
        [Description("Save document operation is enabled (true) or not (false).")]
        public bool EnableSaveDocument
        {
            get
            {
                if (this.tsSave.Visible)
                { return true; }
                else { return false; }
            }
            set
            {
                this.tsSave.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [default word wrap].
        /// </summary>
        /// <value><c>true</c> if [default word wrap]; otherwise, <c>false</c>.</value>
        private bool defaultWordWrap = false;
        [Description("Editor should use word wrap (true) or not (false).")]
        public bool DefaultWordWrap
        {
            get { return this.defaultWordWrap; }
            set { this.defaultWordWrap = value; }
        }

        /// <summary>
        /// Gets or sets the color of the default background.
        /// </summary>
        /// <value>The color of the default background.</value>
        private Color defaultBackgroundColor = Color.White;
        [Description("Default background color.")]
        public Color DefaultBackgroundColor
        {
            get { return this.defaultBackgroundColor; }
            set { this.defaultBackgroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the color of the default font.
        /// </summary>
        /// <value>The color of the default font.</value>
        private Color defaultFontColor = Color.Black;
        [Description("Default foreground (font) color.")]
        public Color DefaultFontColor
        {
            get { return this.defaultFontColor; }
            set { this.defaultFontColor = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [read only].
        /// </summary>
        /// <value><c>true</c> if [read only]; otherwise, <c>false</c>.</value>
        [Description("The text in the editor is read only (true) or writeable (false).")]
        public Boolean ReadOnly
        {
            get { return this.rtb.ReadOnly; }
            set { this.rtb.ReadOnly = value; }
        }

        #endregion Input/output variables and properties
        
        #region Selection changed event

        /// <summary>
        /// Handles the SelectionChanged event of the rtb control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void rtb_SelectionChanged(object sender, EventArgs e)
        {
            // Font style, family and size
            if (this.rtb.SelectionFont != null)
            {
                this.tsBold.Checked = this.rtb.SelectionFont.Bold;
                this.tsItalic.Checked = this.rtb.SelectionFont.Italic;
                this.tsUnderline.Checked = this.rtb.SelectionFont.Underline;
                this.tsStrikeout.Checked = this.rtb.SelectionFont.Strikeout;
                
                this.tsFontType.Text = this.rtb.SelectionFont.Name;
                this.tsFontType.ToolTipText = this.rtb.SelectionFont.Name;
                double fontSize = Math.Round(this.rtb.SelectionFont.Size);
                this.tsFontSize.Text = fontSize.ToString();
            }

            // Alignment
            this.tsRight.Checked = (this.rtb.SelectionAlignment == HorizontalAlignment.Right);
            this.tsLeft.Checked = (this.rtb.SelectionAlignment == HorizontalAlignment.Left);
            this.tsCenter.Checked = (this.rtb.SelectionAlignment == HorizontalAlignment.Center);
            
            // Font color and background color
            this.tsFontColor.BackColor = this.rtb.SelectionColor;
            this.tsFontColor.ForeColor = this.rtb.SelectionColor;
            this.tsBackgroundColor.BackColor = this.rtb.SelectionBackColor;
            this.tsBackgroundColor.ForeColor = this.rtb.SelectionBackColor;

            // Bulleting
            this.tsBullet.Checked = this.rtb.SelectionBullet;
        }

        #endregion Selection changed event

        #region File management

        /// <summary>
        /// Handles the Click event of the tsNew control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsNew_Click(object sender, EventArgs e)
        {
            if (!this.EnableNewDocument)
            {
                return;
            }

            // biriwork - isDirty vizsgalat
            this.rtb.Clear();
        }

        /// <summary>
        /// Handles the Click event of the tsOpen control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsOpen_Click(object sender, EventArgs e)
        {
            if (!this.EnableOpenDocument)
            {
                return;
            }

            // biriwork - isDirty vizsgalat

            // Fire open event if we have a subscription
            RTFEditorEventArgs ea = new RTFEditorEventArgs();
            ea.Cancel = false;
            if (OnDocumentOpen != null)
            {
                OnDocumentOpen(this, ea);
            }

            if (!ea.Cancel)
            {
                // Open file and send it to the RTF editor
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (!String.IsNullOrEmpty(ofd.FileName))
                    {
                        try
                        {
                            this.rtb.LoadFile(ofd.FileName);
                        }
                        catch (Exception ex)
                        {
                            try
                            {
                                this.rtb.LoadFile(ofd.FileName, RichTextBoxStreamType.PlainText);
                            }
                            catch (Exception exc)
                            {
                                throw new Exception(exc.Message);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the tsSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsSave_Click(object sender, EventArgs e)
        {
            if (!this.EnableSaveDocument)
            {
                return;
            }

            // Fire save event if we have a subscription
            RTFEditorEventArgs ea = new RTFEditorEventArgs();
            ea.Cancel = false;
            if (OnDocumentSave != null)
            {
                OnDocumentSave(this, ea);
            }

            if (!ea.Cancel)
            {
                // Save file from RTF editor
                SaveFileDialog sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (!String.IsNullOrEmpty(sfd.FileName))
                    {
                        this.rtb.SaveFile(sfd.FileName);
                    }
                }
            }
        }



        #endregion File management

        #region Clipboard management

        /// <summary>
        /// Handles the Click event of the tsCut control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsCut_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            this.rtb.Cut();
        }

        /// <summary>
        /// Handles the Click event of the tsCopy control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsCopy_Click(object sender, EventArgs e)
        {
            this.rtb.Copy();
        }

        /// <summary>
        /// Handles the Click event of the tsPaste control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsPaste_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            this.rtb.Paste();
        }

        /// <summary>
        /// Handles the Click event of the pasteToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tsPaste_Click(sender, e);
        }

        #endregion Clipboard management

        #region Font style management

        /// <summary>
        /// Handles the Click event of the tsBold control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsBold_Click(object sender, EventArgs e)
        {
            ChangeFontStyle(!this.rtb.SelectionFont.Bold, this.rtb.SelectionFont.Italic, this.rtb.SelectionFont.Underline, this.rtb.SelectionFont.Strikeout);
        }

        /// <summary>
        /// Handles the Click event of the tsItalic control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsItalic_Click(object sender, EventArgs e)
        {
            ChangeFontStyle(this.rtb.SelectionFont.Bold, !this.rtb.SelectionFont.Italic, this.rtb.SelectionFont.Underline, this.rtb.SelectionFont.Strikeout);
        }

        /// <summary>
        /// Handles the Click event of the tsUnderline control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsUnderline_Click(object sender, EventArgs e)
        {
            ChangeFontStyle(this.rtb.SelectionFont.Bold, this.rtb.SelectionFont.Italic, !this.rtb.SelectionFont.Underline, this.rtb.SelectionFont.Strikeout);
        }

        /// <summary>
        /// Handles the Click event of the tsStrikeout control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsStrikeout_Click(object sender, EventArgs e)
        {
            ChangeFontStyle(this.rtb.SelectionFont.Bold, this.rtb.SelectionFont.Italic, this.rtb.SelectionFont.Underline, !this.rtb.SelectionFont.Strikeout);
        }

        /// <summary>
        /// Changes the font style.
        /// </summary>
        /// <param name="fontBold">if set to <c>true</c> [font bold].</param>
        /// <param name="fontItalic">if set to <c>true</c> [font italic].</param>
        /// <param name="fontUnderline">if set to <c>true</c> [font underline].</param>
        /// <param name="fontStrikeout">if set to <c>true</c> [font strikeout].</param>
        private void ChangeFontStyle(bool fontBold, bool fontItalic, bool fontUnderline, bool fontStrikeout)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            int fontStyle = 0;

            if (fontBold) { fontStyle += (int)FontStyle.Bold; }
            if (fontItalic) { fontStyle += (int)FontStyle.Italic; }
            if (fontUnderline) { fontStyle += (int)FontStyle.Underline; }
            if (fontStrikeout) { fontStyle += (int)FontStyle.Strikeout; }

            this.rtb.SelectionFont = new Font(this.rtb.SelectionFont, (FontStyle)fontStyle);
            rtb_SelectionChanged(rtb, new EventArgs());
        }

        #endregion Font style management

        #region Alignment management

        /// <summary>
        /// Handles the Click event of the tsLeft control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsLeft_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            this.rtb.SelectionAlignment = HorizontalAlignment.Left;
            rtb_SelectionChanged(rtb, new EventArgs());
        }

        /// <summary>
        /// Handles the Click event of the tsCenter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsCenter_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            this.rtb.SelectionAlignment = HorizontalAlignment.Center;
            rtb_SelectionChanged(rtb, new EventArgs());
        }

        /// <summary>
        /// Handles the Click event of the tsRight control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsRight_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            this.rtb.SelectionAlignment = HorizontalAlignment.Right;
            rtb_SelectionChanged(rtb, new EventArgs());
        }

        #endregion Alignment management

        #region Font type and size change

        /// <summary>
        /// Handles the SelectedIndexChanged event of the tsFontSize control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsFontSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeFontTypeAndSize(this.rtb.SelectionFont.FontFamily, Int32.Parse(this.tsFontSize.SelectedItem.ToString()));
        }

        /// <summary>
        /// Handles the Leave event of the tsFontSize control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsFontSize_Leave(object sender, EventArgs e)
        {
            try
            {
                ChangeFontTypeAndSize(this.rtb.SelectionFont.FontFamily, Int32.Parse(this.tsFontSize.Text));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Handles the KeyUp event of the tsFontSize control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void tsFontSize_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ChangeFontTypeAndSize(this.rtb.SelectionFont.FontFamily, Int32.Parse(this.tsFontSize.Text));
            }
        }

        /// <summary>
        /// Handles the Click event of the tsFontType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsFontType_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            FontDialog fontDlg = new FontDialog();
            fontDlg.Font = this.rtb.SelectionFont;
            fontDlg.ShowColor = true;
            if (fontDlg.ShowDialog() == DialogResult.OK)
            {
                this.rtb.SelectionFont = fontDlg.Font;
                this.rtb.SelectionColor = fontDlg.Color;

                rtb_SelectionChanged(rtb, new EventArgs());
            }
        }

        /// <summary>
        /// Changes the size of the font type and.
        /// </summary>
        /// <param name="fontFamily">The font family.</param>
        /// <param name="fontSize">Size of the font.</param>
        private void ChangeFontTypeAndSize(FontFamily fontFamily, int fontSize)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            // New font
            try
            {
                Font myFont = new Font(fontFamily, fontSize, this.rtb.SelectionFont.Style);
                this.rtb.SelectionFont = myFont;
                rtb_SelectionChanged(rtb, new EventArgs());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion Font type and size change

        #region Font and background color

        /// <summary>
        /// Handles the Click event of the tsFontColor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsFontColor_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            this.rtb.SelectionColor = this.tsFontColor.BackColor;
        }

        /// <summary>
        /// Handles the Click event of the tsBackgroundColor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsBackgroundColor_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            this.rtb.SelectionBackColor = this.tsBackgroundColor.BackColor;
        }

        /// <summary>
        /// Handles the Click event of the tsSelectFontColor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsSelectFontColor_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            ColorDialog colorDlg = new ColorDialog();
            colorDlg.Color = this.rtb.SelectionColor;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                this.rtb.SelectionColor = colorDlg.Color;
                rtb_SelectionChanged(rtb, new EventArgs());
            }
        }

        /// <summary>
        /// Handles the Click event of the tsSelectBackgroundColor control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsSelectBackgroundColor_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            ColorDialog colorDlg = new ColorDialog();
            colorDlg.Color = this.rtb.SelectionBackColor;
            if (colorDlg.ShowDialog() == DialogResult.OK)
            {
                this.rtb.SelectionBackColor = colorDlg.Color;
                rtb_SelectionChanged(rtb, new EventArgs());
            }
        }

        #endregion Font and background color

        #region Bulleting

        /// <summary>
        /// Handles the Click event of the tsBullet control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsBullet_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            this.rtb.SelectionBullet = !this.rtb.SelectionBullet;
            rtb_SelectionChanged(rtb, new EventArgs());
        }

        #endregion Bulleting

        #region Text identing

        /// <summary>
        /// Handles the Click event of the tsIdentPlus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsIdentPlus_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            if (this.rtb.SelectionAlignment == HorizontalAlignment.Left)
            {
                this.rtb.SelectionIndent += 36;
            }
        }

        /// <summary>
        /// Handles the Click event of the tsIdentMinus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsIdentMinus_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            if (this.rtb.SelectionAlignment == HorizontalAlignment.Left && this.rtb.SelectionIndent >= 36)
            {
                this.rtb.SelectionIndent -= 36;
            }
        }

        #endregion Text identing

        #region Undo/redo

        /// <summary>
        /// Handles the Click event of the tsUndo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsUndo_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            this.rtb.Undo();
        }

        /// <summary>
        /// Handles the Click event of the tsRedo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void tsRedo_Click(object sender, EventArgs e)
        {
            if (this.rtb.ReadOnly)
            {
                return;
            }

            this.rtb.Redo();
        }



        #endregion Undo/redo

        
    }
}
