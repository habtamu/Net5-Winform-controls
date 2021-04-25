using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI.Utility.ImageViewer
{
    public partial class ImageViewer : UserControl,IImagePanelHost
    {
        IImageBrowser m_Browser=null;
        internal bool m_FullScreenMode;
        internal int m_Page;
        ImageViewer m_Parent;
        public ImageViewer()
        {
            InitializeComponent();
            imgPanel.Host = this;
            SetFullSreenMode(false,null);
            SetState(true);
            ShowImage();
           
        }
        internal void ShowImageSetState(bool IsParent)
        {
            SetState(IsParent);
            ShowImage();
        }

        void SetState(bool IsParent)
        {
            if (m_Browser == null)
            {
                btnNext.Enabled = false;
                btnPrev.Enabled = false;
            }
            else
            {
                btnPrev.Enabled = m_Page > 0;
                btnNext.Enabled = m_Browser.NPages - 1 > m_Page;
            }
            if (m_Parent != null  && !IsParent)
            {
                m_Parent.m_Page = this.m_Page;
                m_Parent.SetState(false);
            }
        }
        internal void ShowImage()
        {
            if(m_Browser!=null)
                if (m_Browser.NPages > 0)
                {
                    imgPanel.SetImage(m_Browser.CurrentImage);
                    lblPage.Text = "Page "+m_Browser.CurrentPageNumber.ToString();
                    lblTitle.Text = "("+m_Browser.CurrentTitle+")";
                    goto l_Finish;
                }
            lblPage.Text = "";
            lblTitle.Text = "";
            imgPanel.SetImage(null);
l_Finish:
            if (m_Parent != null)
                m_Parent.ShowImage();

        }
        public void SetSource(IImageBrowser browser)
        {
            m_Browser = browser;
            m_Page = 0;
            ShowImageSetState(false);
        }
        
        private void btnPrev_Click(object sender, EventArgs e)
        {
            m_Browser.MovePrev();
            m_Page--;
            ShowImageSetState(false);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            m_Browser.MoveNext();
            m_Page++;
            ShowImageSetState(false);
        }

        private void ZoomButtonClick(object sender, EventArgs e)
        {
            if (sender == btnZoomBox)
            {
              imgPanel.tool = DragTool.Box;
            }
            else if (sender == btnPan)
            {
               imgPanel.tool = DragTool.Pan;
            }
            else if (sender == btnZoomInteractive)
            {
                imgPanel.tool = DragTool.Zoom;
            }

        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            imgPanel.Zoom(1.5f);
            imgPanel.Center();
            imgPanel.Invalidate();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            imgPanel.Zoom(0.5f);
            imgPanel.Center();
            imgPanel.Invalidate();
        }

        #region IImagePanelHost Members

        public void ZoomLevelChanged(float Level)
        {
            cmbZoom.Text = (Level * 100).ToString("0.0");
        }

        public void DragToolChanged(DragTool tool)
        {
            btnPan.Checked = tool == DragTool.Pan;
            btnZoomInteractive.Checked = tool == DragTool.Zoom;
            btnZoomBox.Checked = tool == DragTool.Box;
        }

        public void PanChanged(Point pan)
        {
        }

        #endregion

        private void cmbZoom_Click(object sender, EventArgs e)
        {

        }

        private void cmbZoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = cmbZoom.Items.Count - cmbZoom.SelectedIndex;
            switch (index)
            {
                case 1://best fit
                    imgPanel.BestFit();
                    imgPanel.Invalidate();
                    break;
                case 2:
                    imgPanel.FitHeight();
                    imgPanel.Invalidate();
                    break;
                case 3://fit width;
                    imgPanel.FitWidth();
                    imgPanel.Invalidate();
                    break;
                default:
                    SetComboZoom();
                    break;
            }
        }
        void SetComboZoom()
        {
            string txt = cmbZoom.Text;
            if (txt.Length == 0)
                goto l_Invalid;
            txt = txt.TrimEnd('%');
            try
            {
                imgPanel.ZoomLevel = float.Parse(txt) / 100f;
            }
            catch
            {
                goto l_Invalid;
            }

            return;
        l_Invalid:
            MessageBox.Show("Please enter valid zoom level");
            ZoomLevelChanged(imgPanel.ZoomLevel);
        }
        public void ImageAdded(bool MoveLast)
        {
            SetState(m_Parent==null);
        }
        public void ImageDeleted(int Page)
        {
            if (Page == m_Page)
            {
                m_Page--;
                ShowImageSetState(m_Parent == null);
            }
        }
        private void cmbZoom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SetComboZoom();
            }
        }

        private void cmbZoom_Validated(object sender, EventArgs e)
        {
            ZoomLevelChanged(imgPanel.ZoomLevel);
        }
        public void SetPage(Image img, int Page)
        {
            this.m_Page = Page;
            imgPanel.SetImage(img);
        }
        internal void  SetFullSreenMode(bool fsm,ImageViewer parent)
        {
            m_FullScreenMode=fsm;
            btnFullScreen.Enabled = !fsm;
            btnCloseFullScreen.Enabled = fsm;
            if (fsm)
            {
                m_Parent = parent;
            }
            else
                m_Parent = null;
        }

        private void btnFullScreen_Click(object sender, EventArgs e)
        {
            ViewerForm vf = new ViewerForm();
            vf.imageViewer1.SetFullSreenMode(true, this);
            vf.imageViewer1.m_Browser = this.m_Browser;
            vf.imageViewer1.m_Page = this.m_Page;
            vf.imageViewer1.ShowImageSetState(true);
            vf.ShowDialog();
        }

        private void btnCloseFullScreen_Click(object sender, EventArgs e)
        {
            ((Form)this.Parent).Close();
        }
    }
    public class ImageArray:IImageBrowser
    {
        Image[] m_Images;
        int Page;
        public ImageArray(Image[] images)
        {
            m_Images = images;
            Page = 0;
        }
        public ImageArray(string [] Files)
        {
            m_Images = new Image[Files.Length];
            for (int i = 0; i < Files.Length; i++)
            {
                m_Images[i] = Image.FromFile(Files[i]);
            }
            Page = 0;
        }
        #region IImageBrowser Members

        public int NPages
        {
            get {
                return m_Images.Length;

            }
        }

        public void MoveNext()
        {
            if (Page == m_Images.Length - 1)
                throw new Exception("No image after the last image.");
            Page++;
        }

        public void MovePrev()
        {
            if (Page == 0)
                throw new Exception("No image before the first image");
            Page--;
        }

        public bool IsCurrnetImageBack
        {
            get {
                return false;
            }
        }

        public int CurrentPageNumber
        {
            get {
                return (Page + 1);
            }
        }

        public string  CurrentTitle
        {
            get {
                return "Page " + CurrentPageNumber.ToString();
            }
        }

        public Image CurrentImage
        {
            get { 

            return m_Images[Page];
            }
        }

        #endregion
    }
}
