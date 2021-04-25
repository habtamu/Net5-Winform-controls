
namespace UI.Utility.RichTextBox
{
    partial class RTFEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RTFEditor));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsNew = new System.Windows.Forms.ToolStripButton();
            this.tsOpen = new System.Windows.Forms.ToolStripButton();
            this.tsSave = new System.Windows.Forms.ToolStripButton();
            this.tsFileSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.tsCut = new System.Windows.Forms.ToolStripButton();
            this.tsCopy = new System.Windows.Forms.ToolStripButton();
            this.tsPaste = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsUndo = new System.Windows.Forms.ToolStripButton();
            this.tsRedo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBold = new System.Windows.Forms.ToolStripButton();
            this.tsItalic = new System.Windows.Forms.ToolStripButton();
            this.tsUnderline = new System.Windows.Forms.ToolStripButton();
            this.tsStrikeout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsLeft = new System.Windows.Forms.ToolStripButton();
            this.tsCenter = new System.Windows.Forms.ToolStripButton();
            this.tsRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsBullet = new System.Windows.Forms.ToolStripButton();
            this.tsIdentPlus = new System.Windows.Forms.ToolStripButton();
            this.tsIdentMinus = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsFontSize = new System.Windows.Forms.ToolStripComboBox();
            this.tsFontType = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsFontColor = new System.Windows.Forms.ToolStripButton();
            this.tsBackgroundColor = new System.Windows.Forms.ToolStripButton();
            this.rtb = new System.Windows.Forms.RichTextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsNew,
            this.tsOpen,
            this.tsSave,
            this.tsFileSeparator,
            this.tsCut,
            this.tsCopy,
            this.tsPaste,
            this.toolStripSeparator1,
            this.tsUndo,
            this.tsRedo,
            this.toolStripSeparator2,
            this.tsBold,
            this.tsItalic,
            this.tsUnderline,
            this.tsStrikeout,
            this.toolStripSeparator3,
            this.tsLeft,
            this.tsCenter,
            this.tsRight,
            this.toolStripSeparator4,
            this.tsBullet,
            this.tsIdentPlus,
            this.tsIdentMinus,
            this.toolStripSeparator6,
            this.tsFontSize,
            this.tsFontType,
            this.toolStripSeparator5,
            this.tsFontColor,
            this.tsBackgroundColor});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsNew
            // 
            this.tsNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsNew.Image = ((System.Drawing.Image)(resources.GetObject("tsNew.Image")));
            this.tsNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsNew.Name = "tsNew";
            this.tsNew.Size = new System.Drawing.Size(23, 22);
            this.tsNew.Text = "&New";
            this.tsNew.Click += new System.EventHandler(this.tsNew_Click);
            // 
            // tsOpen
            // 
            this.tsOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsOpen.Image = ((System.Drawing.Image)(resources.GetObject("tsOpen.Image")));
            this.tsOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsOpen.Name = "tsOpen";
            this.tsOpen.Size = new System.Drawing.Size(23, 22);
            this.tsOpen.Text = "&Open";
            this.tsOpen.Click += new System.EventHandler(this.tsOpen_Click);
            // 
            // tsSave
            // 
            this.tsSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsSave.Image = global::UI.Utility.RichTextBox.Properties.Resources.save;
            this.tsSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsSave.Name = "tsSave";
            this.tsSave.Size = new System.Drawing.Size(23, 22);
            this.tsSave.Text = "&Save";
            this.tsSave.Click += new System.EventHandler(this.tsSave_Click);
            // 
            // tsFileSeparator
            // 
            this.tsFileSeparator.Name = "tsFileSeparator";
            this.tsFileSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // tsCut
            // 
            this.tsCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsCut.Image = ((System.Drawing.Image)(resources.GetObject("tsCut.Image")));
            this.tsCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsCut.Name = "tsCut";
            this.tsCut.Size = new System.Drawing.Size(23, 22);
            this.tsCut.Text = "C&ut";
            this.tsCut.Click += new System.EventHandler(this.tsCut_Click);
            // 
            // tsCopy
            // 
            this.tsCopy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsCopy.Image = ((System.Drawing.Image)(resources.GetObject("tsCopy.Image")));
            this.tsCopy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsCopy.Name = "tsCopy";
            this.tsCopy.Size = new System.Drawing.Size(23, 22);
            this.tsCopy.Text = "&Copy";
            this.tsCopy.Click += new System.EventHandler(this.tsCopy_Click);
            // 
            // tsPaste
            // 
            this.tsPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsPaste.Image = ((System.Drawing.Image)(resources.GetObject("tsPaste.Image")));
            this.tsPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsPaste.Name = "tsPaste";
            this.tsPaste.Size = new System.Drawing.Size(23, 22);
            this.tsPaste.Text = "&Paste";
            this.tsPaste.Click += new System.EventHandler(this.tsPaste_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsUndo
            // 
            this.tsUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsUndo.Image = global::UI.Utility.RichTextBox.Properties.Resources.undo;
            this.tsUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsUndo.Name = "tsUndo";
            this.tsUndo.Size = new System.Drawing.Size(23, 22);
            this.tsUndo.Text = "Undo";
            this.tsUndo.Click += new System.EventHandler(this.tsUndo_Click);
            // 
            // tsRedo
            // 
            this.tsRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRedo.Image = global::UI.Utility.RichTextBox.Properties.Resources.redo;
            this.tsRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsRedo.Name = "tsRedo";
            this.tsRedo.Size = new System.Drawing.Size(23, 22);
            this.tsRedo.Text = "Redo";
            this.tsRedo.Click += new System.EventHandler(this.tsRedo_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsBold
            // 
            this.tsBold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBold.Image = global::UI.Utility.RichTextBox.Properties.Resources.bold;
            this.tsBold.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBold.Name = "tsBold";
            this.tsBold.Size = new System.Drawing.Size(23, 22);
            this.tsBold.Text = "Bold";
            this.tsBold.Click += new System.EventHandler(this.tsBold_Click);
            // 
            // tsItalic
            // 
            this.tsItalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsItalic.Image = global::UI.Utility.RichTextBox.Properties.Resources.italic;
            this.tsItalic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsItalic.Name = "tsItalic";
            this.tsItalic.Size = new System.Drawing.Size(23, 22);
            this.tsItalic.Text = "Italic";
            this.tsItalic.Click += new System.EventHandler(this.tsItalic_Click);
            // 
            // tsUnderline
            // 
            this.tsUnderline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsUnderline.Image = global::UI.Utility.RichTextBox.Properties.Resources.underline;
            this.tsUnderline.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsUnderline.Name = "tsUnderline";
            this.tsUnderline.Size = new System.Drawing.Size(23, 22);
            this.tsUnderline.Text = "Underline";
            this.tsUnderline.Click += new System.EventHandler(this.tsUnderline_Click);
            // 
            // tsStrikeout
            // 
            this.tsStrikeout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsStrikeout.Image = global::UI.Utility.RichTextBox.Properties.Resources.strikethrough;
            this.tsStrikeout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsStrikeout.Name = "tsStrikeout";
            this.tsStrikeout.Size = new System.Drawing.Size(23, 22);
            this.tsStrikeout.Text = "Strikeout";
            this.tsStrikeout.Click += new System.EventHandler(this.tsStrikeout_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsLeft
            // 
            this.tsLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsLeft.Image = global::UI.Utility.RichTextBox.Properties.Resources.justifyleft;
            this.tsLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsLeft.Name = "tsLeft";
            this.tsLeft.Size = new System.Drawing.Size(23, 22);
            this.tsLeft.Text = "Left";
            this.tsLeft.Click += new System.EventHandler(this.tsLeft_Click);
            // 
            // tsCenter
            // 
            this.tsCenter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsCenter.Image = global::UI.Utility.RichTextBox.Properties.Resources.justifycenter;
            this.tsCenter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsCenter.Name = "tsCenter";
            this.tsCenter.Size = new System.Drawing.Size(23, 22);
            this.tsCenter.Text = "Center";
            this.tsCenter.Click += new System.EventHandler(this.tsCenter_Click);
            // 
            // tsRight
            // 
            this.tsRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRight.Image = global::UI.Utility.RichTextBox.Properties.Resources.justifyright;
            this.tsRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsRight.Name = "tsRight";
            this.tsRight.Size = new System.Drawing.Size(23, 22);
            this.tsRight.Text = "Right";
            this.tsRight.Click += new System.EventHandler(this.tsRight_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsBullet
            // 
            this.tsBullet.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBullet.Image = global::UI.Utility.RichTextBox.Properties.Resources.bullist;
            this.tsBullet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBullet.Name = "tsBullet";
            this.tsBullet.Size = new System.Drawing.Size(23, 22);
            this.tsBullet.Text = "Bullet";
            this.tsBullet.Click += new System.EventHandler(this.tsBullet_Click);
            // 
            // tsIdentPlus
            // 
            this.tsIdentPlus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsIdentPlus.Image = global::UI.Utility.RichTextBox.Properties.Resources.indent;
            this.tsIdentPlus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsIdentPlus.Name = "tsIdentPlus";
            this.tsIdentPlus.Size = new System.Drawing.Size(23, 22);
            this.tsIdentPlus.Text = "Ident Plus";
            this.tsIdentPlus.Click += new System.EventHandler(this.tsIdentPlus_Click);
            // 
            // tsIdentMinus
            // 
            this.tsIdentMinus.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsIdentMinus.Image = global::UI.Utility.RichTextBox.Properties.Resources.outdent;
            this.tsIdentMinus.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsIdentMinus.Name = "tsIdentMinus";
            this.tsIdentMinus.Size = new System.Drawing.Size(23, 22);
            this.tsIdentMinus.Text = "IdentMinus";
            this.tsIdentMinus.Click += new System.EventHandler(this.tsIdentMinus_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // tsFontSize
            // 
            this.tsFontSize.DropDownWidth = 10;
            this.tsFontSize.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "11",
            "12",
            "14",
            "16",
            "18",
            "20",
            "22",
            "24",
            "26",
            "28",
            "36",
            "48",
            "72"});
            this.tsFontSize.Name = "tsFontSize";
            this.tsFontSize.Size = new System.Drawing.Size(75, 25);
            this.tsFontSize.SelectedIndexChanged += new System.EventHandler(this.tsFontSize_SelectedIndexChanged);
            this.tsFontSize.Leave += new System.EventHandler(this.tsFontSize_Leave);
            // 
            // tsFontType
            // 
            this.tsFontType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsFontType.Image = ((System.Drawing.Image)(resources.GetObject("tsFontType.Image")));
            this.tsFontType.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsFontType.Name = "tsFontType";
            this.tsFontType.Size = new System.Drawing.Size(64, 22);
            this.tsFontType.Text = "Font Type";
            this.tsFontType.Click += new System.EventHandler(this.tsFontType_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator5.Visible = false;
            // 
            // tsFontColor
            // 
            this.tsFontColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsFontColor.Image = global::UI.Utility.RichTextBox.Properties.Resources.forecolor;
            this.tsFontColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsFontColor.Name = "tsFontColor";
            this.tsFontColor.Size = new System.Drawing.Size(23, 22);
            this.tsFontColor.Text = "Font color";
            this.tsFontColor.Visible = false;
            this.tsFontColor.Click += new System.EventHandler(this.tsFontColor_Click);
            // 
            // tsBackgroundColor
            // 
            this.tsBackgroundColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsBackgroundColor.Image = global::UI.Utility.RichTextBox.Properties.Resources.backcolor;
            this.tsBackgroundColor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsBackgroundColor.Name = "tsBackgroundColor";
            this.tsBackgroundColor.Size = new System.Drawing.Size(23, 22);
            this.tsBackgroundColor.Text = "Background Color";
            this.tsBackgroundColor.Visible = false;
            this.tsBackgroundColor.Click += new System.EventHandler(this.tsBackgroundColor_Click);
            // 
            // rtb
            // 
            this.rtb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rtb.Location = new System.Drawing.Point(0, 25);
            this.rtb.Name = "rtb";
            this.rtb.Size = new System.Drawing.Size(800, 111);
            this.rtb.TabIndex = 1;
            this.rtb.Text = "";
            // 
            // RTFEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rtb);
            this.Controls.Add(this.toolStrip1);
            this.Name = "RTFEditor";
            this.Size = new System.Drawing.Size(800, 136);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsNew;
        private System.Windows.Forms.ToolStripSeparator tsFileSeparator;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsUndo;
        private System.Windows.Forms.ToolStripButton tsRedo;
        private System.Windows.Forms.ToolStripButton tsBold;
        private System.Windows.Forms.RichTextBox rtb;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsItalic;
        private System.Windows.Forms.ToolStripButton tsUnderline;
        private System.Windows.Forms.ToolStripButton tsStrikeout;
        private System.Windows.Forms.ToolStripButton tsOpen;
        private System.Windows.Forms.ToolStripButton tsSave;
        private System.Windows.Forms.ToolStripButton tsCut;
        private System.Windows.Forms.ToolStripButton tsCopy;
        private System.Windows.Forms.ToolStripButton tsPaste;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsLeft;
        private System.Windows.Forms.ToolStripButton tsCenter;
        private System.Windows.Forms.ToolStripButton tsRight;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripComboBox tsFontSize;
        private System.Windows.Forms.ToolStripButton tsFontType;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tsFontColor;
        private System.Windows.Forms.ToolStripButton tsBackgroundColor;
        private System.Windows.Forms.ToolStripButton tsBullet;
        private System.Windows.Forms.ToolStripButton tsIdentPlus;
        private System.Windows.Forms.ToolStripButton tsIdentMinus;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    }
}
