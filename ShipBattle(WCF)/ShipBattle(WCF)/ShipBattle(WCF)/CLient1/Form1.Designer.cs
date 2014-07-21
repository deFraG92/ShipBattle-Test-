namespace ClientsPart
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ReadyBut = new System.Windows.Forms.Button();
            this.ShipTypeCMB = new System.Windows.Forms.ComboBox();
            this.ShipTurnCMB = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ReadyBut
            // 
            this.ReadyBut.Font = new System.Drawing.Font("Times New Roman", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ReadyBut.Location = new System.Drawing.Point(432, 76);
            this.ReadyBut.Name = "ReadyBut";
            this.ReadyBut.Size = new System.Drawing.Size(98, 27);
            this.ReadyBut.TabIndex = 1;
            this.ReadyBut.Text = "Ready";
            this.ReadyBut.UseVisualStyleBackColor = true;
            this.ReadyBut.Click += new System.EventHandler(this.ReadyBut_Click);
            // 
            // ShipTypeCMB
            // 
            this.ShipTypeCMB.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ShipTypeCMB.FormattingEnabled = true;
            this.ShipTypeCMB.Location = new System.Drawing.Point(318, 31);
            this.ShipTypeCMB.Name = "ShipTypeCMB";
            this.ShipTypeCMB.Size = new System.Drawing.Size(121, 27);
            this.ShipTypeCMB.TabIndex = 5;
            this.ShipTypeCMB.Text = "GreatShip";
            this.ShipTypeCMB.SelectedValueChanged += new System.EventHandler(this.ShipType_SelectedValueChanged);
            // 
            // ShipTurnCMB
            // 
            this.ShipTurnCMB.Font = new System.Drawing.Font("Times New Roman", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ShipTurnCMB.FormattingEnabled = true;
            this.ShipTurnCMB.Items.AddRange(new object[] {
            "StraightShip",
            "TurnShip"});
            this.ShipTurnCMB.Location = new System.Drawing.Point(486, 31);
            this.ShipTurnCMB.Name = "ShipTurnCMB";
            this.ShipTurnCMB.Size = new System.Drawing.Size(121, 27);
            this.ShipTurnCMB.TabIndex = 7;
            this.ShipTurnCMB.Text = "StraightShip";
            this.ShipTurnCMB.SelectedValueChanged += new System.EventHandler(this.ShipTurn_SelectedValueChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1, 438);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(838, 156);
            this.textBox1.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(842, 606);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.ShipTurnCMB);
            this.Controls.Add(this.ShipTypeCMB);
            this.Controls.Add(this.ReadyBut);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "Form1";
            this.Text = "ShipBattle";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Field_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Field_MouseClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ReadyBut;
        private System.Windows.Forms.ComboBox ShipTypeCMB;
        private System.Windows.Forms.ComboBox ShipTurnCMB;
        private System.Windows.Forms.TextBox textBox1;
    }
}

