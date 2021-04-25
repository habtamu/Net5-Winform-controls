namespace UI.Utility.ImageViewer
{
    partial class ImageViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageViewer));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnPrev = new System.Windows.Forms.ToolStripButton();
            this.btnNext = new System.Windows.Forms.ToolStripButton();
            this.lblPage = new System.Windows.Forms.ToolStripLabel();
            this.lblTitle = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnZoomBox = new System.Windows.Forms.ToolStripButton();
            this.btnPan = new System.Windows.Forms.ToolStripButton();
            this.btnZoomInteractive = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
            this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
            this.cmbZoom = new System.Windows.Forms.ToolStripComboBox();
            this.btnFullScreen = new System.Windows.Forms.ToolStripButton();
            this.btnCloseFullScreen = new System.Windows.Forms.ToolStripButton();
            this.imgPanel = new ImagePanel();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPrev,
            this.btnNext,
            this.lblPage,
            this.lblTitle,
            this.toolStripSeparator1,
            this.btnZoomBox,
            this.btnPan,
            this.btnZoomInteractive,
            this.toolStripSeparator2,
            this.btnZoomIn,
            this.btnZoomOut,
            this.cmbZoom,
            this.btnFullScreen,
            this.btnCloseFullScreen});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(580, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnPrev
            // 
            this.btnPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPrev.Image = ((System.Drawing.Image)(resources.GetObject("btnPrev.Image")));
            this.btnPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(23, 22);
            this.btnPrev.Text = "toolStripButton1";
            this.btnPrev.Visible = false;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext
            // 
            this.btnNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNext.Image = ((System.Drawing.Image)(resources.GetObject("btnNext.Image")));
            this.btnNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(23, 22);
            this.btnNext.Text = "toolStripButton2";
            this.btnNext.Visible = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblPage
            // 
            this.lblPage.Name = "lblPage";
            this.lblPage.Size = new System.Drawing.Size(0, 22);
            this.lblPage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblPage.Visible = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(0, 22);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator1.Visible = false;
            // 
            // btnZoomBox
            // 
            this.btnZoomBox.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomBox.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomBox.Image")));
            this.btnZoomBox.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomBox.Name = "btnZoomBox";
            this.btnZoomBox.Size = new System.Drawing.Size(23, 22);
            this.btnZoomBox.ToolTipText = "Zoom to Box";
            this.btnZoomBox.Click += new System.EventHandler(this.ZoomButtonClick);
            // 
            // btnPan
            // 
            this.btnPan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPan.Image = ((System.Drawing.Image)(resources.GetObject("btnPan.Image")));
            this.btnPan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPan.Name = "btnPan";
            this.btnPan.Size = new System.Drawing.Size(23, 22);
            this.btnPan.Text = "toolStripButton1";
            this.btnPan.ToolTipText = "Pan";
            this.btnPan.Click += new System.EventHandler(this.ZoomButtonClick);
            // 
            // btnZoomInteractive
            // 
            this.btnZoomInteractive.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomInteractive.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomInteractive.Image")));
            this.btnZoomInteractive.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomInteractive.Name = "btnZoomInteractive";
            this.btnZoomInteractive.Size = new System.Drawing.Size(23, 22);
            this.btnZoomInteractive.Text = "toolStripButton1";
            this.btnZoomInteractive.ToolTipText = "Zoom";
            this.btnZoomInteractive.Click += new System.EventHandler(this.ZoomButtonClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomIn.Image")));
            this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(23, 22);
            this.btnZoomIn.Text = "toolStripButton1";
            this.btnZoomIn.ToolTipText = "Zoom In";
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomOut.Image")));
            this.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(23, 22);
            this.btnZoomOut.Text = "toolStripButton1";
            this.btnZoomOut.ToolTipText = "Zoom Out";
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // cmbZoom
            // 
            this.cmbZoom.AutoCompleteCustomSource.AddRange(new string[] {
            "500%",
            "200%",
            "150%",
            "100%",
            "75%",
            "50%",
            "25%",
            "10%",
            "Fit Width",
            "Fit Height",
            "Fit Page"});
            this.cmbZoom.Items.AddRange(new object[] {
            "500%",
            "200%",
            "150%",
            "100%",
            "50%",
            "Fit Width",
            "Fit Height",
            "Fit Page"});
            this.cmbZoom.Name = "cmbZoom";
            this.cmbZoom.Size = new System.Drawing.Size(121, 25);
            this.cmbZoom.SelectedIndexChanged += new System.EventHandler(this.cmbZoom_SelectedIndexChanged);
            this.cmbZoom.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmbZoom_KeyDown);
            this.cmbZoom.Validated += new System.EventHandler(this.cmbZoom_Validated);
            // 
            // btnFullScreen
            // 
            this.btnFullScreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnFullScreen.Image = ((System.Drawing.Image)(resources.GetObject("btnFullScreen.Image")));
            this.btnFullScreen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFullScreen.Name = "btnFullScreen";
            this.btnFullScreen.Size = new System.Drawing.Size(23, 22);
            this.btnFullScreen.Text = "toolStripButton1";
            this.btnFullScreen.ToolTipText = "Full Screen";
            this.btnFullScreen.Click += new System.EventHandler(this.btnFullScreen_Click);
            // 
            // btnCloseFullScreen
            // 
            this.btnCloseFullScreen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnCloseFullScreen.Image = ((System.Drawing.Image)(resources.GetObject("btnCloseFullScreen.Image")));
            this.btnCloseFullScreen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCloseFullScreen.Name = "btnCloseFullScreen";
            this.btnCloseFullScreen.Size = new System.Drawing.Size(23, 22);
            this.btnCloseFullScreen.Text = "toolStripButton1";
            this.btnCloseFullScreen.ToolTipText = "Close Full Sreen";
            this.btnCloseFullScreen.Click += new System.EventHandler(this.btnCloseFullScreen_Click);
            // 
            // imgPanel
            // 
            this.imgPanel.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.imgPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imgPanel.Host = null;
            this.imgPanel.Location = new System.Drawing.Point(0, 25);
            this.imgPanel.Name = "imgPanel";
            this.imgPanel.Size = new System.Drawing.Size(580, 403);
            this.imgPanel.TabIndex = 1;
            this.imgPanel.tool = DragTool.Zoom;
            this.imgPanel.ZoomLevel = 1F;
            // 
            // ImageViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.imgPanel);
            this.Controls.Add(this.toolStrip1);
            this.Name = "ImageViewer";
            this.Size = new System.Drawing.Size(580, 428);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnPrev;
        private System.Windows.Forms.ToolStripButton btnNext;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnZoomBox;
        private System.Windows.Forms.ToolStripLabel lblPage;
        private System.Windows.Forms.ToolStripLabel lblTitle;
        private System.Windows.Forms.ToolStripButton btnPan;
        private System.Windows.Forms.ToolStripButton btnZoomInteractive;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnZoomIn;
        private System.Windows.Forms.ToolStripButton btnZoomOut;
        private System.Windows.Forms.ToolStripComboBox cmbZoom;
        private System.Windows.Forms.ToolStripButton btnFullScreen;
        private System.Windows.Forms.ToolStripButton btnCloseFullScreen;
        internal ImagePanel imgPanel;
    }
}
