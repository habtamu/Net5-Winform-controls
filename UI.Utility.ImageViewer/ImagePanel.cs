using System;
using System.Drawing;
using System.Windows.Forms;

namespace UI.Utility.ImageViewer
{
    public interface IImageBrowser
    {
        int NPages { get;}
        void MoveNext();
        void MovePrev();
        bool IsCurrnetImageBack { get;}
        int CurrentPageNumber { get;}
        string CurrentTitle { get;}
        Image CurrentImage { get;}
    }
    public interface IImagePanelHost
    {
        void ZoomLevelChanged(float Level);
        void DragToolChanged(DragTool tool);
        void PanChanged(Point pan);
    }
    public enum DragTool
    {
        Zoom,
        Box,
        Pan
    }
    public class ImagePanel : Panel
    {
        enum DragState
        {
            Panning,
            Boxing,
            Zooming,
            None
        }
        Point m_CurrentOffset;
        float m_CurrentZoomLevel;
        Image m_CurrentImage;
        DragState m_DragState;
        int m_LastX = -1, m_LastY = -1;
        DragTool m_Tool;
        IImagePanelHost m_Host=null;
        public IImagePanelHost Host
        {
            get
            {
                return m_Host;
            }
            set
            {
                m_Host = value;
            }
        }
        public DragTool tool
        {
            get
            {
                return m_Tool;
            }
            set
            {
                switch (value)
                {
                    case DragTool.Box:
                        this.Cursor = Cursors.Cross;
                        break;
                    case DragTool.Pan:
                        this.Cursor = Cursors.Hand;
                        break;
                    case DragTool.Zoom:
                        this.Cursor = Cursors.SizeNS;
                        break;
                }
                m_Tool = value;
                if (m_Host != null)
                    m_Host.DragToolChanged(value);
            }
        }
        public float ZoomLevel
        {
            get
            {
                return m_CurrentZoomLevel;
            }
            set
            {
                if (m_CurrentZoomLevel < 0.0f)
                    throw new Exception("Invalid zoom level");
                m_CurrentZoomLevel = value;
                if (m_Host != null)
                    m_Host.ZoomLevelChanged(m_CurrentZoomLevel);
                Center();
                this.Invalidate();
            }
        }
        public ImagePanel()
        {


            m_CurrentImage = null;
            m_CurrentZoomLevel = 1;
            m_CurrentOffset = new Point(0, 0);
            m_DragState = DragState.None;
            this.tool = DragTool.Zoom;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rec;

            if (m_CurrentImage == null)
            {
                rec = new Rectangle(0, 0, 0, 0);
            }
            else
            {
                rec = new Rectangle(
                    m_CurrentOffset.X,
                    m_CurrentOffset.Y,
                    (int)(m_CurrentZoomLevel * m_CurrentImage.Width)
                    , (int)(m_CurrentZoomLevel * m_CurrentImage.Height));
            }
            Brush fillbrush = Brushes.Black;
            if (rec.Top > 0)
                e.Graphics.FillRectangle(fillbrush, 0, 0, this.Width, rec.Top);
            if (rec.Top + rec.Height < this.Height)
                e.Graphics.FillRectangle(fillbrush, 0, rec.Top + rec.Height, this.Width, this.Height - rec.Top - rec.Height);
            if (rec.Left > 0)
                e.Graphics.FillRectangle(fillbrush, 0, rec.Top > 0 ? rec.Top : 0, rec.Left
                    , ((rec.Top + rec.Height < this.Height) ? rec.Top + rec.Height : this.Height) - (rec.Top > 0 ? rec.Top : 0));
            if (rec.Left + rec.Width < this.Width)
                e.Graphics.FillRectangle(fillbrush, rec.Left + rec.Width, rec.Top > 0 ? rec.Top : 0, this.Width - rec.Left - rec.Width
            , ((rec.Top + rec.Height < this.Height) ? rec.Top + rec.Height : this.Height) - (rec.Top > 0 ? rec.Top : 0));
            if(m_CurrentImage!=null)
                e.Graphics.DrawImage(m_CurrentImage, rec);
            if (m_DragState == DragState.Boxing)
                e.Graphics.DrawRectangle(Pens.Red, m_ZoomBox);
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            
        }
        public void SetImage(Image img)
        {
            m_CurrentImage = img;
            BestFit();
            this.Invalidate();
        }
       
        protected override void OnMouseDown(MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Middle:
                    break;
                case MouseButtons.Right:
                    switch (m_Tool)
                    {
                        case DragTool.Box:
                            this.tool = DragTool.Zoom;
                            break;
                        case DragTool.Zoom:
                            this.tool = DragTool.Pan;
                            break;
                        case DragTool.Pan:
                            this.tool = DragTool.Box;
                            break;
                    }
                    break;
                case MouseButtons.Left:
                    this.Capture = true;
                    switch (m_Tool)
                    {
                        case DragTool.Box:
                            m_DragState = DragState.Boxing;
                            break;
                        case DragTool.Zoom:
                            m_DragState = DragState.Zooming;
                            break;
                        case DragTool.Pan:
                            m_DragState = DragState.Panning;
                            break;
                    }
                    m_LastX = e.X;
                    m_LastY = e.Y;
                    break;
            }
            base.OnMouseDown(e);
        }
        internal void Zoom(Rectangle rec)
        {
            float newaspect = (float)rec.Width / (float)rec.Height;
            float aspect = (float)this.Width / (float)this.Height;
            if (newaspect > aspect)
            {
                rec.Y = rec.Y - (int)((rec.Width / aspect - rec.Height) / 2);
                rec.Height = (int)(rec.Width / aspect);
            }
            else if (newaspect < aspect)
            {
                rec.X = rec.X - (int)((rec.Height * aspect - rec.Width) / 2);
                rec.Width = (int)(rec.Height * aspect);
            }
            float NewZoomLevel = m_CurrentZoomLevel * (float)this.Width / (float)rec.Width;
            m_CurrentOffset.X = (int)(
                this.Width / 2f - NewZoomLevel / m_CurrentZoomLevel * (-m_CurrentOffset.X + rec.X + rec.Width / 2f)
                );

            m_CurrentOffset.Y = (int)(
                    this.Height / 2f - NewZoomLevel / m_CurrentZoomLevel * (-m_CurrentOffset.Y + rec.Y + rec.Height / 2f)
                );
            m_CurrentZoomLevel = NewZoomLevel;

            if (m_Host != null)
                m_Host.ZoomLevelChanged(m_CurrentZoomLevel);
        }
        internal void Zoom(float factor)
        {
            m_CurrentOffset=new Point(
                (int)(m_CurrentOffset.X*factor-(factor-1)*(Width/2))
            ,(int)(m_CurrentOffset.Y*factor-(factor-1)*(Height/2))
            );

            m_CurrentZoomLevel *= factor;
            if (m_Host != null)
                m_Host.ZoomLevelChanged(m_CurrentZoomLevel);

        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            m_CurrentZoomLevel *= 1 + e.Delta * 0.1f;
            if (m_Host != null)
                m_Host.ZoomLevelChanged(m_CurrentZoomLevel);
            this.Invalidate();
            base.OnMouseWheel(e);
        }
        public void BestFit()
        {
            if(m_CurrentImage==null)
                return;
            float xf=(float)this.Width/(float)m_CurrentImage.Width;
            float yf=(float)this.Height/(float)m_CurrentImage.Height;
            m_CurrentZoomLevel = xf < yf ? xf : yf;
            if (m_Host != null)
                m_Host.ZoomLevelChanged(m_CurrentZoomLevel);
            Center();

        }
        public void FitWidth()
        {
            if (m_CurrentImage == null)
                return;
            m_CurrentZoomLevel = (float)this.Width / (float)m_CurrentImage.Width;
            if (m_Host != null)
                m_Host.ZoomLevelChanged(m_CurrentZoomLevel);
            Center();

        }
        public void FitHeight()
        {
            if (m_CurrentImage == null)
                return;
            m_CurrentZoomLevel = (float)this.Height / (float)m_CurrentImage.Height;
            if (m_Host != null)
                m_Host.ZoomLevelChanged(m_CurrentZoomLevel);
            Center();

        }
        internal void Center()
        {
            if (m_CurrentImage == null)
                return;
            bool Changed = false;
            if (m_CurrentImage.Width * m_CurrentZoomLevel < (float)this.Width)
            {
                m_CurrentOffset.X = (int)(this.Width / 2 - m_CurrentImage.Width * m_CurrentZoomLevel / 2);
                Changed = true;
            }
            else if (m_CurrentOffset.X > 0)
            {
                m_CurrentOffset.X = 0;
                Changed = true;
            }
            else if (m_CurrentOffset.X + m_CurrentImage.Width * m_CurrentZoomLevel < this.Width - 1)
            {
                m_CurrentOffset.X = (int)(this.Width - m_CurrentImage.Width * m_CurrentZoomLevel);
                Changed = true;
            }

            if (m_CurrentImage.Height * m_CurrentZoomLevel < (float)this.Height)
            {
                m_CurrentOffset.Y = (int)(this.Height / 2 - m_CurrentImage.Height * m_CurrentZoomLevel / 2);
                Changed = true;
            }
            else if (m_CurrentOffset.Y > 0)
            {
                m_CurrentOffset.Y = 0;
                Changed = true;
            }
            else if (m_CurrentOffset.Y + m_CurrentImage.Height * m_CurrentZoomLevel < this.Height - 1)
            {
                m_CurrentOffset.Y = (int)(this.Height - m_CurrentImage.Height * m_CurrentZoomLevel);
                Changed = true;
            }
            if (m_Host != null)
                m_Host.PanChanged(m_CurrentOffset);
        }
        Rectangle m_ZoomBox;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.Capture)
            {
                int NDX = m_LastX - e.X;
                int NDY = m_LastY - e.Y;

                switch (m_DragState)
                {
                    case DragState.Panning:
                        if (NDX * NDX + NDY * NDY == 0)
                            break;
                        m_CurrentOffset.Offset(-(int)(NDX), -(int)(NDY));
                        m_LastX = e.X;
                        m_LastY = e.Y;
                        if (m_Host != null)
                            m_Host.PanChanged(m_CurrentOffset);
                        Center();
                        this.Invalidate();
                        break;
                    case DragState.Zooming:
                        double dsq = (double)(NDX * NDX + NDY * NDY) / (double)(
                        this.Width * this.Width + this.Height * this.Height);
                        if (NDY > 0)
                            Zoom((float)(1+Math.Sqrt(dsq)));
                        else
                            Zoom((float)(1/(1+Math.Sqrt(dsq))));
                        Center();
                        m_LastX = e.X;
                        m_LastY = e.Y;
                        this.Invalidate();
                        break;
                    case DragState.Boxing:
                        m_DragState = DragState.Boxing;
                        m_ZoomBox = new Rectangle(m_LastX, m_LastY, e.X - m_LastX, e.Y - m_LastY);
                        this.Invalidate();
                        break;

                }

            }
            base.OnMouseMove(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.Capture)
            {
                if (m_DragState == DragState.Boxing)
                {
                    if(m_ZoomBox.Width>5 && m_ZoomBox.Height>5)
                        Zoom(m_ZoomBox);
                    m_DragState = DragState.None;
                    this.Invalidate();
                }
                else
                    m_DragState = DragState.None;
                this.Capture = false;
                
            }
            base.OnMouseUp(e);
        }
        protected override void OnResize(EventArgs eventargs)
        {
            Center();
            this.Invalidate();
        }
    }
}
