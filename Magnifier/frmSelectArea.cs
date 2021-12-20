using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Magnifier
{
    public partial class frmSelectArea : Form
    {
        private Rectangle rect1 = Rectangle.Empty;
        private Rectangle rect2 = Rectangle.Empty;
        private Rectangle rect3 = Rectangle.Empty;
        private Rectangle rect4 = Rectangle.Empty;

        private Rectangle rect5 = Rectangle.Empty;
        private Rectangle rect6 = Rectangle.Empty;
        private Rectangle rect7 = Rectangle.Empty;
        private Rectangle rect8 = Rectangle.Empty;

        //

        private Rectangle rect9 = Rectangle.Empty;
        private Rectangle rect10 = Rectangle.Empty;
        private Rectangle rect11 = Rectangle.Empty;
        private Rectangle rect12 = Rectangle.Empty;

        private int PreviousX = -1;
        private int PreviousY = -1;

        private int PickStyle=-1;

        private bool IsMouseDown = false;
        private bool InMouseMove = false;

        private int MouseDownX = -1;
        private int MouseDownY = -1;

        private int HandleSize = 25;        

        private int fWidth = -1;
        private int fHeight = -1;
        private int fLeft = -1;
        private int fTop = -1;

        private bool ChangeValues = false;

        private bool First = false;

        public frmSelectArea(bool first)
        {
            InitializeComponent();

            First = first;

            this.Icon = Properties.Resources.magnifier_48;

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            if (First)
            {
                fLeft = Properties.Settings.Default.fLeft;
                fTop = Properties.Settings.Default.fTop;
                fWidth = Math.Max(90, Properties.Settings.Default.fWidth);
                fHeight = Math.Max(90, Properties.Settings.Default.fHeight);
            }
            else
            {
                fLeft = Properties.Settings.Default.f2Left;
                fTop = Properties.Settings.Default.f2Top;
                fWidth = Math.Max(90, Properties.Settings.Default.f2Width);
                fHeight = Math.Max(90, Properties.Settings.Default.f2Height);
            }

            fLeft = Math.Max(0, fLeft);
            fTop = Math.Max(0, fTop);
            fWidth = Math.Max(0, fWidth);
            fHeight = Math.Max(0, fHeight);

            //HandleSize = Properties.Settings.Default.HandleSize;

            HandleSize = 7;

            int w = 0;
            int h = 0;

            for (int k=0;k<Screen.AllScreens.Length;k++)
            {
                w += Screen.AllScreens[k].WorkingArea.Width;

                h += Screen.AllScreens[k].WorkingArea.Height;
            }

            this.Width = w;
            this.Height = h;

            int bw = btnOK.Width + btnCancel.Width + 30;

            btnOK.Top = Screen.PrimaryScreen.WorkingArea.Height - btnOK.Height - 12;
            btnCancel.Top = Screen.PrimaryScreen.WorkingArea.Height - btnCancel.Height - 12;

            btnOK.Left = Screen.PrimaryScreen.WorkingArea.Width - bw;
            btnCancel.Left = Screen.PrimaryScreen.WorkingArea.Width - btnCancel.Width - 12;

            ChangeValues = true;

            nudX.Value = fLeft;
            nudY.Value = fTop;
            nudWidth.Value = fWidth;
            nudHeight.Value = fHeight;

            ChangeValues = false;
        }

        private void frmSelectArea_Resize(object sender, EventArgs e)
        {
            
        }

        private void FixRects()
        {
            rect1 = new Rectangle(fLeft, fTop, HandleSize, HandleSize);

            rect2 = new Rectangle(fLeft+fWidth - HandleSize, fTop, HandleSize, HandleSize);

            rect3 = new Rectangle(fLeft+ fWidth - HandleSize, fTop+fHeight - HandleSize, HandleSize, HandleSize);

            rect4 = new Rectangle(fLeft, fTop+fHeight - HandleSize, HandleSize, HandleSize);

            //---------------

            rect5 = new Rectangle(fLeft+HandleSize, fTop, fWidth - 2 * HandleSize, HandleSize);

            rect6 = new Rectangle(fLeft+fWidth - HandleSize, fTop+HandleSize, HandleSize, fHeight - 2 * HandleSize);

            rect7 = new Rectangle(fLeft+HandleSize, fTop+fHeight - HandleSize, fWidth - 2 * HandleSize, HandleSize);

            rect8 = new Rectangle(fLeft, fTop+HandleSize, HandleSize, fHeight - 2 * HandleSize);

            //-------------------

            rect9 = new Rectangle(fLeft+fWidth/2-HandleSize/2, fTop, HandleSize, HandleSize);

            rect10 = new Rectangle(fLeft + fWidth - HandleSize, fTop+fHeight/2-HandleSize/2, HandleSize, HandleSize);

            rect11 = new Rectangle(fLeft + fWidth/2 - HandleSize/2, fTop + fHeight - HandleSize, HandleSize, HandleSize);

            rect12 = new Rectangle(fLeft, fTop + fHeight/2 - HandleSize/2, HandleSize, HandleSize);
        }

        private void frmSelectArea_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {

                if (InMouseMove) return;

                InMouseMove = true;

                if (Control.MouseButtons != MouseButtons.Left)
                {
                    Point p = new Point(e.X, e.Y);

                    if (rect1.Contains(p))
                    {
                        this.Cursor = Cursors.SizeNWSE;
                    }
                    else if (rect2.Contains(p))
                    {
                        this.Cursor = Cursors.SizeNESW;
                    }
                    else if (rect3.Contains(p))
                    {
                        this.Cursor = Cursors.SizeNWSE;
                    }
                    else if (rect4.Contains(p))
                    {
                        this.Cursor = Cursors.SizeNESW;
                    }
                    //--------
                    else if (rect5.Contains(p))
                    {
                        this.Cursor = Cursors.SizeNS;
                    }
                    else if (rect6.Contains(p))
                    {
                        this.Cursor = Cursors.SizeWE;
                    }
                    else if (rect7.Contains(p))
                    {
                        this.Cursor = Cursors.SizeNS;
                    }
                    else if (rect8.Contains(p))
                    {
                        this.Cursor = Cursors.SizeWE;
                    }
                    else
                    {
                        this.Cursor = Cursors.Hand;
                    }

                    PreviousX = p.X;
                    PreviousY = p.Y;
                }
                else
                {
                    int difx = e.X - PreviousX;
                    int dify = e.Y - PreviousY;

                    Point p = new Point(e.X, e.Y);

                    if (PickStyle == 1)
                    {
                        fWidth -= difx;
                        fLeft += difx;

                        fHeight -= dify;
                        fTop += dify;

                    }
                    if (PickStyle == 2)
                    {

                        fWidth += difx;
                        fHeight -= dify;
                        //fLeft -= difx;
                        fTop += dify;

                    }
                    if (PickStyle == 3)
                    {

                        fWidth += difx;
                        fHeight += dify;

                        //fLeft -= difx;
                        //fTop -= dify;
                    }
                    if (PickStyle == 4)
                    {
                        fWidth -= difx;
                        fLeft += difx;
                        fHeight += dify;

                        //fTop -= dify;
                    }
                    //--------
                    if (PickStyle == 5)
                    {
                        //fWidth += difx;
                        fHeight -= dify;
                        //fLeft -= difx;
                        fTop += dify;
                    }
                    if (PickStyle == 6)
                    {                        
                        fWidth += difx;
                        //fHeight += dify;
                        //fLeft -= difx;
                        //fTop -= dify;
                    }
                    if (PickStyle == 7)
                    {                        
                        //fWidth += difx;
                        fHeight += dify;
                        //fLeft -= difx;
                        //fTop -= dify;
                    }
                    if (PickStyle == 8)
                    {                        
                        fWidth -= difx;
                        //fHeight += dify;
                        fLeft += difx;
                        //fTop -= dify;
                    }
                    if (PickStyle == 9)
                    {

                        //fWidth += difx;
                        //fHeight += dify;
                        fLeft += difx;
                        fTop += dify;

                    }

                    PreviousX = p.X;
                    PreviousY = p.Y;
                }
            }
            finally
            {
                ChangeValues = true;

                fLeft = Math.Max(0, fLeft);
                fTop = Math.Max(0, fTop);
                fWidth = Math.Max(0, fWidth);
                fHeight = Math.Max(0, fHeight);

                nudX.Value = fLeft;
                nudY.Value = fTop;
                nudWidth.Value = fWidth;
                nudHeight.Value = fHeight;

                ChangeValues = false;

                FixRects();

                InMouseMove = false;

                this.Invalidate();
            }
        }

        private void frmSelectArea_Paint(object sender, PaintEventArgs e)
        {
            Font fs = new Font(this.Font.FontFamily, 15, GraphicsUnit.Pixel);

            string txt = TranslateHelper.Translate("Move and Resize to specify Magnification Source Area then press OK button");

            if (!First)
            {
                txt = TranslateHelper.Translate("Move and Resize to specify Magnification Destination Area then press OK button");
            }


            string lbl1 = "X : ";
            string lbl2 = " Y : ";
            string lbl3 = TranslateHelper.Translate("Width") + " : ";
            string lbl4 = TranslateHelper.Translate("Height") + " : ";

            string lbl = "X : " + fLeft.ToString() + " Y : " + fTop.ToString() + " " + TranslateHelper.Translate("Width")
                + " : " + fWidth.ToString() + " " + TranslateHelper.Translate("Height") + " : " + fHeight.ToString();

            SizeF sz = e.Graphics.MeasureString(txt, fs);
            SizeF szlbl = e.Graphics.MeasureString(lbl, fs);

            SizeF szl1 = e.Graphics.MeasureString(lbl1, fs);
            SizeF szl2 = e.Graphics.MeasureString(lbl2, fs);
            SizeF szl3 = e.Graphics.MeasureString(lbl3, fs);
            SizeF szl4 = e.Graphics.MeasureString(lbl4, fs);

            e.Graphics.Clear(Color.Olive);

            e.Graphics.FillRectangle(Brushes.DarkGray,new Rectangle(fLeft,fTop ,fWidth ,fHeight));

            e.Graphics.FillRectangle(Brushes.Olive,new Rectangle(fLeft+HandleSize,fTop+HandleSize,fWidth-2*HandleSize,fHeight-2*HandleSize));

            e.Graphics.FillRectangle(Brushes.Red, rect1);
            e.Graphics.FillRectangle(Brushes.Red, rect2);
            e.Graphics.FillRectangle(Brushes.Red, rect3);
            e.Graphics.FillRectangle(Brushes.Red, rect4);

            //

            e.Graphics.FillRectangle(Brushes.Red, rect9);
            e.Graphics.FillRectangle(Brushes.Red, rect10);
            e.Graphics.FillRectangle(Brushes.Red, rect11);
            e.Graphics.FillRectangle(Brushes.Red, rect12);

            //e.Graphics.DrawString(txt, fs, Brushes.Black, fLeft+(float)(fWidth / 2 - sz.Width / 2),

            /*
            e.Graphics.DrawString(txt, fs, Brushes.DarkBlue, fLeft + (float)(fWidth / 2 - sz.Width / 2),
                fTop +(float)(fHeight / 2 - sz.Height / 2));

            */

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;            

            StringFormat strformat = new StringFormat();

            string szbuf = txt;

            GraphicsPath path = new GraphicsPath();
            path.AddString(szbuf, fs.FontFamily,(int)FontStyle.Bold,
            15f,new RectangleF
            (fLeft + (float)(fWidth / 2 - (sz.Width+50) / 2),
                fTop + (float)(fHeight / 2 - sz.Height / 2),sz.Width+50,sz.Height)
            , strformat);
            //Pen pen = new Pen(Color.Black, 6);
            Pen pen = new Pen(Color.Yellow, 6);
            pen.LineJoin = LineJoin.Round;
            e.Graphics.DrawPath(pen, path);
            //SolidBrush brush = new SolidBrush(Color.White);
            SolidBrush brush = new SolidBrush(Color.Black);
            e.Graphics.FillPath(brush, path);

            path.Dispose();
            pen.Dispose();
            brush.Dispose();

            //e.Graphics.DrawRectangle(Pens.Black,fLeft + (float)(fWidth / 2 - sz.Width / 2),
            //fTop + (float)(fHeight / 2 - sz.Height / 2),sz.Width,sz.Height);

            // ----

            /*
            GraphicsPath pathlbl = new GraphicsPath();
            pathlbl.AddString(lbl, fs.FontFamily, (int)FontStyle.Bold,
            15f, new RectangleF
            (25, this.Height- szlbl.Height-25, szlbl.Width+50, szlbl.Height)
            , strformat);
            //Pen pen = new Pen(Color.Black, 6);
            Pen penlbl = new Pen(Color.Yellow, 6);
            penlbl.LineJoin = LineJoin.Round;
            e.Graphics.DrawPath(penlbl, pathlbl);
            //SolidBrush brush = new SolidBrush(Color.White);
            SolidBrush brushlbl = new SolidBrush(Color.Black);
            e.Graphics.FillPath(brushlbl, pathlbl);
            */

            int x50 = 20;
            int y25 = 25;

            GraphicsPath pathlbl1 = new GraphicsPath();
            pathlbl1.AddString(lbl1, fs.FontFamily, (int)FontStyle.Bold,
            15f, new RectangleF
            (25, this.Height - szl1.Height - 25, szl1.Width + x50, szl1.Height)
            , strformat);
            //Pen pen = new Pen(Color.Black, 6);
            Pen penlbl = new Pen(Color.Yellow, 6);
            penlbl.LineJoin = LineJoin.Round;
            e.Graphics.DrawPath(penlbl, pathlbl1);
            //SolidBrush brush = new SolidBrush(Color.White);
            SolidBrush brushlbl = new SolidBrush(Color.Black);
            e.Graphics.FillPath(brushlbl, pathlbl1);

            nudX.Left = (int)(25 + szl1.Width + x50);
            nudX.Top = (int)(this.Height - szl1.Height - y25);

            GraphicsPath pathlbl2 = new GraphicsPath();
            pathlbl2.AddString(lbl2, fs.FontFamily, (int)FontStyle.Bold,
            15f, new RectangleF
            (nudX.Right, this.Height - szl2.Height - 25, szl2.Width + x50, szl2.Height)
            , strformat);
            e.Graphics.DrawPath(penlbl, pathlbl2);
            e.Graphics.FillPath(brushlbl, pathlbl2);

            nudY.Left = (int)(nudX.Right + szl2.Width + x50);
            nudY.Top = (int)(this.Height - szl2.Height - y25);

            GraphicsPath pathlbl3 = new GraphicsPath();
            pathlbl3.AddString(lbl3, fs.FontFamily, (int)FontStyle.Bold,
            15f, new RectangleF
            (nudY.Right, this.Height - szl3.Height - 25, szl3.Width + x50, szl3.Height)
            , strformat);
            e.Graphics.DrawPath(penlbl, pathlbl3);
            e.Graphics.FillPath(brushlbl, pathlbl3);

            nudWidth.Left = (int)(nudY.Right + szl3.Width + x50);
            nudWidth.Top = (int)(this.Height - szl3.Height - y25);

            GraphicsPath pathlbl4 = new GraphicsPath();
            pathlbl4.AddString(lbl4, fs.FontFamily, (int)FontStyle.Bold,
            15f, new RectangleF
            (nudWidth.Right, this.Height - szl4.Height - 25, szl4.Width + x50, szl4.Height)
            , strformat);
            e.Graphics.DrawPath(penlbl, pathlbl4);
            e.Graphics.FillPath(brushlbl, pathlbl4);

            nudHeight.Left = (int)(nudWidth.Right + szl4.Width + x50);
            nudHeight.Top = (int)(this.Height - szl4.Height - y25);

            pathlbl1.Dispose();
            pathlbl2.Dispose();
            pathlbl3.Dispose();
            pathlbl4.Dispose();

            penlbl.Dispose();
            brushlbl.Dispose();

            fs.Dispose();            
        }

        private void frmSelectArea_MouseDown(object sender, MouseEventArgs e)
        {
            if (!IsMouseDown)
            {
                IsMouseDown = true;

                Point p = new Point(e.X, e.Y);

                if (rect1.Contains(p))
                {
                    PickStyle = 1;
                }
                else if (rect2.Contains(p))
                {
                    PickStyle = 2;
                }
                else if (rect3.Contains(p))
                {
                    PickStyle = 3;
                }
                else if (rect4.Contains(p))
                {
                    PickStyle = 4;
                }
                //--------
                else if (rect5.Contains(p))
                {
                    PickStyle = 5;
                }
                else if (rect6.Contains(p))
                {
                    PickStyle = 6;
                }
                else if (rect7.Contains(p))
                {
                    PickStyle = 7;
                }
                else if (rect8.Contains(p))
                {
                    PickStyle = 8;
                }
                else
                {
                    PickStyle = 9;
                }
            }
        }

        private void frmSelectArea_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;
        }

        private void frmSelectArea_Load(object sender, EventArgs e)
        {
            FixRects();

            /*
            fWidth = this.Width;
            fHeight = this.Height;
            fLeft = this.Left;
            fTop = this.Top;
            */
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (First)
            {
                Properties.Settings.Default.fLeft = fLeft;
                Properties.Settings.Default.fTop = fTop;
                Properties.Settings.Default.fWidth = Math.Max(90, fWidth);
                Properties.Settings.Default.fHeight = Math.Max(90, fHeight);

                frmMain.Instance.bmp1 = new Bitmap(Properties.Settings.Default.fWidth - 2 * HandleSize, Properties.Settings.Default.fHeight - 2 * HandleSize);

                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.f2Left = fLeft;
                Properties.Settings.Default.f2Top = fTop;
                Properties.Settings.Default.f2Width = Math.Max(90, fWidth);
                Properties.Settings.Default.f2Height = Math.Max(90, fHeight);

                Properties.Settings.Default.Save();
            }

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void nudX_ValueChanged(object sender, EventArgs e)
        {
            if (ChangeValues) return;

            fLeft = (int)nudX.Value;
            fTop = (int)nudY.Value;
            fWidth = (int)nudWidth.Value;
            fHeight = (int)nudHeight.Value;

            FixRects();

            this.Invalidate();
        }
    }
}
