
namespace WinFormsTest
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.imageViewer1 = new UI.Utility.ImageViewer.ImageViewer();
            this.dateTimePickerEx1 = new CalendarLib.DateTimePickerEx();
            this.SuspendLayout();
            // 
            // imageViewer1
            // 
            this.imageViewer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.imageViewer1.Location = new System.Drawing.Point(0, 0);
            this.imageViewer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.imageViewer1.Name = "imageViewer1";
            this.imageViewer1.Size = new System.Drawing.Size(800, 155);
            this.imageViewer1.TabIndex = 0;
            // 
            // dateTimePickerEx1
            // 
            this.dateTimePickerEx1.Location = new System.Drawing.Point(53, 162);
            this.dateTimePickerEx1.Name = "dateTimePickerEx1";
            this.dateTimePickerEx1.Size = new System.Drawing.Size(150, 20);
            this.dateTimePickerEx1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 209);
            this.Controls.Add(this.dateTimePickerEx1);
            this.Controls.Add(this.imageViewer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private UI.Utility.ImageViewer.ImageViewer imageViewer1;
        private CalendarLib.DateTimePickerEx dateTimePickerEx1;
    }
}

