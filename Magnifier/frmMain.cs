using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Magnifier
{
    public partial class frmMain : Form
    {
        private Rectangle rect1 = Rectangle.Empty;
        private Rectangle rect2 = Rectangle.Empty;
        private Rectangle rect3 = Rectangle.Empty;
        private Rectangle rect4 = Rectangle.Empty;

        private Rectangle rect5 = Rectangle.Empty;
        private Rectangle rect6 = Rectangle.Empty;
        private Rectangle rect7 = Rectangle.Empty;
        private Rectangle rect8 = Rectangle.Empty;

        private int PreviousX = -1;
        private int PreviousY = -1;

        private int PickStyle = -1;

        private bool IsMouseDown = false;
        private bool InMouseMove = false;

        private int MouseDownX = -1;
        private int MouseDownY = -1;

        //private int HandleSize = 25;

        private int HandleSize = 7;

        private int f2Width = -1;
        private int f2Height = -1;
        private int f2Left = -1;
        private int f2Top = -1;

        public static frmMain Instance = null;

        public bool InitCursor = false;

        SolidBrush sbCircle = null;

        private System.Drawing.Color sbCircleColor = System.Drawing.Color.Empty;

        public frmMain()
        {
            InitializeComponent();

            /*
            Rectangle boundsScreen = Screen.GetBounds(Point.Empty);
            */

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);                        

            HandleSize = Properties.Settings.Default.HandleSize;            

            Instance = this;

            this.Visible = false;

            InterceptMouse.DisableMouse();

            frmStart fst = new frmStart();

            if (fst.ShowDialog() == DialogResult.OK)
            {
                frmSelectArea fs = new frmSelectArea(true);

                if (fs.ShowDialog() == DialogResult.OK)
                {
                    frmSelectArea fs2 = new frmSelectArea(false);

                    if (fs2.ShowDialog() == DialogResult.OK)
                    {
                        this.Visible = true;
                        this.TopMost = true;

                        f2Left = Properties.Settings.Default.f2Left;
                        f2Top = Properties.Settings.Default.f2Top;
                        f2Width = Properties.Settings.Default.f2Width;
                        f2Height = Properties.Settings.Default.f2Height;

                        this.Width = f2Width;
                        this.Height = f2Height;

                        this.Left = f2Left;
                        this.Top = f2Top;

                        InterceptKeys.DisableKeys();

                        if (!Properties.Settings.Default.FollowMouseCursor)
                        {
                            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                        }
                    }
                    else
                    {
                        Environment.Exit(0);
                    }
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            else
            {
                Environment.Exit(0);
            }
        }

        public void SpecifyAreas()
        {
            this.Visible = false;

            InterceptKeys.UnHookAll();

            frmSelectArea fs = new frmSelectArea(true);

            if (fs.ShowDialog() == DialogResult.OK)
            {
                frmSelectArea fs2 = new frmSelectArea(false);

                if (fs2.ShowDialog() == DialogResult.OK)
                {
                    this.Visible = true;

                    f2Left = Properties.Settings.Default.f2Left;
                    f2Top = Properties.Settings.Default.f2Top;
                    f2Width = Properties.Settings.Default.f2Width;
                    f2Height = Properties.Settings.Default.f2Height;

                    this.Width = f2Width;
                    this.Height = f2Height;

                    this.Left = f2Left;
                    this.Top = f2Top;                    
                    
                    PreviousX = this.Left;
                    PreviousY = this.Top;

                    InterceptKeys.DisableKeys();

                }
                else
                {
                    Environment.Exit(0);
                }
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private void FixRects()
        {
            return;

            rect1 = new Rectangle(f2Left, f2Top, HandleSize, HandleSize);

            rect2 = new Rectangle(f2Left + f2Width - HandleSize, f2Top, HandleSize, HandleSize);

            rect3 = new Rectangle(f2Left + f2Width - HandleSize, f2Top + f2Height - HandleSize, HandleSize, HandleSize);

            rect4 = new Rectangle(f2Left, f2Top + f2Height - HandleSize, HandleSize, HandleSize);

            //---------------

            rect5 = new Rectangle(f2Left + HandleSize, f2Top, f2Width - 2 * HandleSize, HandleSize);

            rect6 = new Rectangle(f2Left + f2Width - HandleSize, f2Top + HandleSize, HandleSize, f2Height - 2 * HandleSize);

            rect7 = new Rectangle(f2Left + HandleSize, f2Top + f2Height - HandleSize, f2Width - 2 * HandleSize, HandleSize);

            rect8 = new Rectangle(f2Left, f2Top + HandleSize, HandleSize, f2Height - 2 * HandleSize);
        }

        private void frmMain_MouseMove(object sender, MouseEventArgs e)
        {                        
            return;

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
                    f2Width -= difx;
                    f2Height -= dify;
                    f2Left += difx;
                    f2Top += dify;
                }
                if (PickStyle == 2)
                {
                    f2Width += difx;
                    f2Height -= dify;
                    //f2Left -= difx;
                    f2Top += dify;
                }
                if (PickStyle == 3)
                {
                    f2Width += difx;
                    f2Height += dify;
                    //f2Left -= difx;
                    //f2Top -= dify;
                }
                if (PickStyle == 4)
                {
                    f2Width -= difx;
                    f2Height += dify;
                    f2Left += difx;
                    //f2Top -= dify;
                }
                //--------
                if (PickStyle == 5)
                {
                    //f2Width += difx;
                    f2Height -= dify;
                    //f2Left -= difx;
                    f2Top += dify;
                }
                if (PickStyle == 6)
                {
                    f2Width += difx;
                    //f2Height += dify;
                    //f2Left -= difx;
                    //f2Top -= dify;
                }
                if (PickStyle == 7)
                {
                    //f2Width += difx;
                    f2Height += dify;
                    //f2Left -= difx;
                    //f2Top -= dify;
                }
                if (PickStyle == 8)
                {
                    f2Width -= difx;
                    //f2Height += dify;
                    f2Left += difx;
                    //f2Top -= dify;
                }
                if (PickStyle == 9)
                {
                    //f2Width += difx;
                    //f2Height += dify;
                    f2Left += difx;
                    f2Top += dify;
                }

                PreviousX = p.X;
                PreviousY = p.Y;
            }                        

            Properties.Settings.Default.f2Left = f2Left;
            Properties.Settings.Default.f2Top = f2Top;
            Properties.Settings.Default.f2Width = f2Width;
            Properties.Settings.Default.f2Height = f2Height;

            FixRects();

            FixRects();

            InMouseMove = false;

            this.Invalidate();
        }

        private void frmMain_MouseDown(object sender, MouseEventArgs e)
        {
            return;

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

        private void frmMain_Load(object sender, EventArgs e)
        {
            //3FixRects();            

            this.Icon = Properties.Resources.magnifier_48;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.f2Left = f2Left;
            Properties.Settings.Default.f2Top = f2Top;
            Properties.Settings.Default.f2Width = f2Width;
            Properties.Settings.Default.f2Height = f2Height;

            Properties.Settings.Default.Save();

            InterceptMouse.UnHookAll();
            InterceptKeys.UnHookAll();
        }

        public Bitmap bmp1 = null;

        private Bitmap cursorImage = null;

        private bool Test = true;

        private void frmMain_Paint(object sender, PaintEventArgs e)
        {
            if (bmp1 == null) return;

            //e.Graphics.Clear(Color.Black);

            Point p = new Point();            

            if (CaptureScreen.IsCursorDifferent())
            {
                cursorImage = CaptureScreen.CaptureImageCursor(ref p, 1);

                //cursorImage.Save(@"c:\1\cursorimg.bmp");
            }
            else
            {
                p.X = Control.MousePosition.X;
                p.Y = Control.MousePosition.Y;
            }

            p.X = p.X - Properties.Settings.Default.fLeft;
            p.Y = p.Y - Properties.Settings.Default.fTop;

            using (Graphics g = Graphics.FromImage(bmp1))
            {
                g.Clear(Color.Black);

                g.CopyFromScreen(Properties.Settings.Default.fLeft, Properties.Settings.Default.fTop, 0, 0,
                    new System.Drawing.Size(Properties.Settings.Default.fWidth,
                        Properties.Settings.Default.fHeight));
                /*
                g.CopyFromScreen(Properties.Settings.Default.fLeft + HandleSize, Properties.Settings.Default.fTop + HandleSize, 0, 0,
                    new System.Drawing.Size(Properties.Settings.Default.fWidth - 2 * HandleSize,
                        Properties.Settings.Default.fHeight - 2 * HandleSize));
                */

                if (Properties.Settings.Default.DrawMouseCircle)
                {
                    Color sbcc = Color.FromArgb(Properties.Settings.Default.MouseCircleOpacity, Properties.Settings.Default.MouseCircleColor);

                    bool color_different = (sbCircleColor.R != sbcc.R || sbCircleColor.G != sbcc.G || sbCircleColor.B != sbcc.B || sbCircleColor.A != sbcc.A);

                    if (sbCircle == null || color_different)
                    {
                        sbCircle = new System.Drawing.SolidBrush(Color.FromArgb(Properties.Settings.Default.MouseCircleOpacity, Properties.Settings.Default.MouseCircleColor));

                        sbCircleColor = sbcc;
                    }

                    g.FillEllipse(sbCircle, p.X - Properties.Settings.Default.MouseCircleRadius / 2, p.Y - Properties.Settings.Default.MouseCircleRadius / 2, Properties.Settings.Default.MouseCircleRadius, Properties.Settings.Default.MouseCircleRadius);
                }

                if (cursorImage != null)
                {
                    g.DrawImage((Image)cursorImage, p);
                }

            }

            /*
            decimal d1 = (decimal)Properties.Settings.Default.fWidth-2 * HandleSize;
            decimal d2 = (decimal)Properties.Settings.Default.fHeight -2 * HandleSize;
            */

            decimal d1 = (decimal)Properties.Settings.Default.fWidth;
            decimal d2 = (decimal)Properties.Settings.Default.fHeight;

            decimal d=d1/d2;
            decimal de=d2/d1;

            //3decimal dd1=(decimal)f2Width;
            //3decimal dd2=(decimal)f2Height;            

            decimal dd1 = (decimal)this.ClientRectangle.Width;
            decimal dd2 = (decimal)this.ClientRectangle.Height;

            // oldw oldh
            // x    newh

            // oldw oldh
            // neww x

            decimal dh=(dd1*d2)/d1;
            int ih=(int)dh;

            decimal dw=(dd2*d1)/d2;
            int iw=(int)dw;

            int Handle2Size = 5;

            //3e.Graphics.FillRectangle(Brushes.DimGray, new Rectangle(f2Left -Handle2Size, f2Top -Handle2Size, f2Width +2*Handle2Size, f2Height +2*Handle2Size));

            e.Graphics.FillRectangle(Brushes.DimGray, new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height));

            if (Test)
            {
                //bmp1.Save(@"c:\1\testmag.bmp");

                Test = false;
            }

            if (ih<=f2Height)
            {
                //3e.Graphics.DrawImage(bmp1,new Rectangle(f2Left,f2Top,f2Width,ih),new Rectangle(0,0,bmp1.Width,bmp1.Height),GraphicsUnit.Pixel);

                //e.Graphics.DrawImage(bmp1, new Rectangle(0, 0, this.Width, ih), new Rectangle(0, 0, bmp1.Width, bmp1.Height), GraphicsUnit.Pixel);

                e.Graphics.DrawImage(bmp1, new Rectangle(0, this.ClientRectangle.Height/2-ih/2, this.ClientRectangle.Width, ih), new Rectangle(0, 0, bmp1.Width, bmp1.Height), GraphicsUnit.Pixel);
            }
            else
            {
                //3e.Graphics.DrawImage(bmp1,new Rectangle(f2Left,f2Top,iw,f2Height),new Rectangle(0,0,bmp1.Width,bmp1.Height),GraphicsUnit.Pixel);

                //e.Graphics.DrawImage(bmp1, new Rectangle(0, 0, iw, this.Height), new Rectangle(0, 0, bmp1.Width, bmp1.Height), GraphicsUnit.Pixel);

                e.Graphics.DrawImage(bmp1, new Rectangle(this.ClientRectangle.Width/2-iw/2, 0, iw, this.ClientRectangle.Height), new Rectangle(0, 0, bmp1.Width, bmp1.Height), GraphicsUnit.Pixel);
            }
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            return;

            Properties.Settings.Default.f2Left = f2Left;
            Properties.Settings.Default.f2Top = f2Top;
            Properties.Settings.Default.f2Width = f2Width;
            Properties.Settings.Default.f2Height = f2Height;

            //FixRects();
            this.Invalidate();
        }

        private void timMagnify_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void frmMain_Move(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                /*
                int difx = this.Left - PreviousX;
                int dify = this.Top - PreviousY;

                Properties.Settings.Default.fLeft += difx;
                Properties.Settings.Default.fTop += dify;

                PreviousX = this.Left;
                PreviousY = this.Top;

                this.Invalidate();
                */
            }
        }
    }
}
