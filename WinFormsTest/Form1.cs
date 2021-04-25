using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UI.Utility.ImageViewer;

namespace WinFormsTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Image image = Image.FromFile("flag.png");
            imageViewer1.SetSource(new ImageArray(new[] { image }));
        }

    }
}
