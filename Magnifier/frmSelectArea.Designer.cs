namespace Magnifier
{
    partial class frmSelectArea
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectArea));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.nudX = new System.Windows.Forms.NumericUpDown();
            this.nudY = new System.Windows.Forms.NumericUpDown();
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nudX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCancel.Image = global::Magnifier.Properties.Resources.delete;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // nudX
            // 
            resources.ApplyResources(this.nudX, "nudX");
            this.nudX.Maximum = new decimal(new int[] {
            -559939584,
            902409669,
            54,
            0});
            this.nudX.Name = "nudX";
            this.nudX.ValueChanged += new System.EventHandler(this.nudX_ValueChanged);
            // 
            // nudY
            // 
            resources.ApplyResources(this.nudY, "nudY");
            this.nudY.Maximum = new decimal(new int[] {
            -559939584,
            902409669,
            54,
            0});
            this.nudY.Name = "nudY";
            this.nudY.ValueChanged += new System.EventHandler(this.nudX_ValueChanged);
            // 
            // nudWidth
            // 
            resources.ApplyResources(this.nudWidth, "nudWidth");
            this.nudWidth.Maximum = new decimal(new int[] {
            -559939584,
            902409669,
            54,
            0});
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.ValueChanged += new System.EventHandler(this.nudX_ValueChanged);
            // 
            // nudHeight
            // 
            resources.ApplyResources(this.nudHeight, "nudHeight");
            this.nudHeight.Maximum = new decimal(new int[] {
            -559939584,
            902409669,
            54,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.ValueChanged += new System.EventHandler(this.nudX_ValueChanged);
            // 
            // frmSelectArea
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Olive;
            this.Controls.Add(this.nudHeight);
            this.Controls.Add(this.nudWidth);
            this.Controls.Add(this.nudY);
            this.Controls.Add(this.nudX);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmSelectArea";
            this.TransparencyKey = System.Drawing.Color.Olive;
            this.Load += new System.EventHandler(this.frmSelectArea_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmSelectArea_Paint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmSelectArea_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmSelectArea_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmSelectArea_MouseUp);
            this.Resize += new System.EventHandler(this.frmSelectArea_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.nudX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown nudX;
        private System.Windows.Forms.NumericUpDown nudY;
        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.NumericUpDown nudHeight;
    }
}