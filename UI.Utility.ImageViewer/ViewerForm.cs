using System;
using System.Windows.Forms;

namespace UI.Utility.ImageViewer
{
    public partial class ViewerForm : Form
    {
        public ViewerForm()
        {
            InitializeComponent();
        }

        private void ViewerForm_Load(object sender, EventArgs e)
        {
            imageViewer1.imgPanel.BestFit();
        }
        
       
    }
}