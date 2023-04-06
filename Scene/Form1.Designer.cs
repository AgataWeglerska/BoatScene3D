
namespace Scene
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
            this.components = new System.ComponentModel.Container();
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ReflectorPosition = new System.Windows.Forms.GroupBox();
            this.StraightReflector = new System.Windows.Forms.RadioButton();
            this.OnTheBuoy = new System.Windows.Forms.RadioButton();
            this.CameraView = new System.Windows.Forms.GroupBox();
            this.Camera3 = new System.Windows.Forms.RadioButton();
            this.Camera2 = new System.Windows.Forms.RadioButton();
            this.Camera1 = new System.Windows.Forms.RadioButton();
            this.ColouringMethod = new System.Windows.Forms.GroupBox();
            this.Gourand = new System.Windows.Forms.RadioButton();
            this.AddDeleteFog = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.ReflectorPosition.SuspendLayout();
            this.CameraView.SuspendLayout();
            this.ColouringMethod.SuspendLayout();
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.Canvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Canvas.Location = new System.Drawing.Point(3, 3);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(620, 620);
            this.Canvas.TabIndex = 0;
            this.Canvas.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 626F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.Canvas, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(896, 626);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.ReflectorPosition, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.CameraView, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ColouringMethod, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(629, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 193F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(264, 620);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // ReflectorPosition
            // 
            this.ReflectorPosition.Controls.Add(this.StraightReflector);
            this.ReflectorPosition.Controls.Add(this.OnTheBuoy);
            this.ReflectorPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ReflectorPosition.Location = new System.Drawing.Point(3, 216);
            this.ReflectorPosition.Name = "ReflectorPosition";
            this.ReflectorPosition.Size = new System.Drawing.Size(258, 207);
            this.ReflectorPosition.TabIndex = 2;
            this.ReflectorPosition.TabStop = false;
            this.ReflectorPosition.Text = "Reflector Direction";
            // 
            // StraightReflector
            // 
            this.StraightReflector.AutoSize = true;
            this.StraightReflector.Location = new System.Drawing.Point(19, 35);
            this.StraightReflector.Name = "StraightReflector";
            this.StraightReflector.Size = new System.Drawing.Size(82, 24);
            this.StraightReflector.TabIndex = 3;
            this.StraightReflector.TabStop = true;
            this.StraightReflector.Text = "Straight";
            this.StraightReflector.UseVisualStyleBackColor = true;
            // 
            // OnTheBuoy
            // 
            this.OnTheBuoy.AutoSize = true;
            this.OnTheBuoy.Location = new System.Drawing.Point(19, 65);
            this.OnTheBuoy.Name = "OnTheBuoy";
            this.OnTheBuoy.Size = new System.Drawing.Size(222, 24);
            this.OnTheBuoy.TabIndex = 4;
            this.OnTheBuoy.TabStop = true;
            this.OnTheBuoy.Text = "Directed On The Yellow Buoy";
            this.OnTheBuoy.UseVisualStyleBackColor = true;
            // 
            // CameraView
            // 
            this.CameraView.Controls.Add(this.Camera3);
            this.CameraView.Controls.Add(this.Camera2);
            this.CameraView.Controls.Add(this.Camera1);
            this.CameraView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CameraView.Location = new System.Drawing.Point(3, 3);
            this.CameraView.Name = "CameraView";
            this.CameraView.Size = new System.Drawing.Size(258, 207);
            this.CameraView.TabIndex = 0;
            this.CameraView.TabStop = false;
            this.CameraView.Text = "View";
            // 
            // Camera3
            // 
            this.Camera3.AutoSize = true;
            this.Camera3.Location = new System.Drawing.Point(19, 86);
            this.Camera3.Name = "Camera3";
            this.Camera3.Size = new System.Drawing.Size(163, 24);
            this.Camera3.TabIndex = 2;
            this.Camera3.TabStop = true;
            this.Camera3.Text = "View From The Boat";
            this.Camera3.UseVisualStyleBackColor = true;
            this.Camera3.CheckedChanged += new System.EventHandler(this.Camera3_CheckedChanged);
            // 
            // Camera2
            // 
            this.Camera2.AutoSize = true;
            this.Camera2.Location = new System.Drawing.Point(19, 56);
            this.Camera2.Name = "Camera2";
            this.Camera2.Size = new System.Drawing.Size(109, 24);
            this.Camera2.TabIndex = 1;
            this.Camera2.TabStop = true;
            this.Camera2.Text = "Follow Boat";
            this.Camera2.UseVisualStyleBackColor = true;
            this.Camera2.CheckedChanged += new System.EventHandler(this.Camera2_CheckedChanged);
            // 
            // Camera1
            // 
            this.Camera1.AutoSize = true;
            this.Camera1.Location = new System.Drawing.Point(19, 26);
            this.Camera1.Name = "Camera1";
            this.Camera1.Size = new System.Drawing.Size(103, 24);
            this.Camera1.TabIndex = 0;
            this.Camera1.TabStop = true;
            this.Camera1.Text = "Static View";
            this.Camera1.UseVisualStyleBackColor = true;
            this.Camera1.CheckedChanged += new System.EventHandler(this.Camera1_CheckedChanged);
            // 
            // ColouringMethod
            // 
            this.ColouringMethod.Controls.Add(this.AddDeleteFog);
            this.ColouringMethod.Controls.Add(this.Gourand);
            this.ColouringMethod.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ColouringMethod.Location = new System.Drawing.Point(3, 429);
            this.ColouringMethod.Name = "ColouringMethod";
            this.ColouringMethod.Size = new System.Drawing.Size(258, 188);
            this.ColouringMethod.TabIndex = 3;
            this.ColouringMethod.TabStop = false;
            this.ColouringMethod.Text = "Shading Method";
            // 
            // Gourand
            // 
            this.Gourand.AutoSize = true;
            this.Gourand.Checked = true;
            this.Gourand.Location = new System.Drawing.Point(19, 39);
            this.Gourand.Name = "Gourand";
            this.Gourand.Size = new System.Drawing.Size(145, 24);
            this.Gourand.TabIndex = 5;
            this.Gourand.TabStop = true;
            this.Gourand.Text = "Gourand Shading";
            this.Gourand.UseVisualStyleBackColor = true;
            // 
            // AddDeleteFog
            // 
            this.AddDeleteFog.Location = new System.Drawing.Point(19, 69);
            this.AddDeleteFog.Name = "AddDeleteFog";
            this.AddDeleteFog.Size = new System.Drawing.Size(163, 29);
            this.AddDeleteFog.TabIndex = 3;
            this.AddDeleteFog.Text = "Add/Delete Fog";
            this.AddDeleteFog.UseVisualStyleBackColor = true;
            this.AddDeleteFog.Click += new System.EventHandler(this.AddDeleteFog_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 626);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximumSize = new System.Drawing.Size(914, 673);
            this.MinimumSize = new System.Drawing.Size(914, 673);
            this.Name = "Form1";
            this.Text = "3D SCENE";
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ReflectorPosition.ResumeLayout(false);
            this.ReflectorPosition.PerformLayout();
            this.CameraView.ResumeLayout(false);
            this.CameraView.PerformLayout();
            this.ColouringMethod.ResumeLayout(false);
            this.ColouringMethod.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Canvas;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.GroupBox CameraView;
        private System.Windows.Forms.RadioButton Camera3;
        private System.Windows.Forms.RadioButton Camera2;
        private System.Windows.Forms.RadioButton Camera1;
        private System.Windows.Forms.GroupBox ReflectorPosition;
        private System.Windows.Forms.RadioButton StraightReflector;
        private System.Windows.Forms.RadioButton OnTheBuoy;
        private System.Windows.Forms.GroupBox ColouringMethod;
        private System.Windows.Forms.RadioButton Gourand;
        private System.Windows.Forms.Button AddDeleteFog;
    }
}

