
namespace CybermazeMapper
{
    partial class RoomControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonNorth = new System.Windows.Forms.Button();
            this.buttonSouth = new System.Windows.Forms.Button();
            this.buttonWest = new System.Windows.Forms.Button();
            this.buttonEast = new System.Windows.Forms.Button();
            this.labelNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonNorth
            // 
            this.buttonNorth.BackColor = System.Drawing.Color.Transparent;
            this.buttonNorth.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonNorth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNorth.Location = new System.Drawing.Point(0, 0);
            this.buttonNorth.Name = "buttonNorth";
            this.buttonNorth.Size = new System.Drawing.Size(40, 5);
            this.buttonNorth.TabIndex = 0;
            this.buttonNorth.UseVisualStyleBackColor = false;
            this.buttonNorth.Click += new System.EventHandler(this.buttonNorth_Click);
            // 
            // buttonSouth
            // 
            this.buttonSouth.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonSouth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSouth.Location = new System.Drawing.Point(0, 35);
            this.buttonSouth.Name = "buttonSouth";
            this.buttonSouth.Size = new System.Drawing.Size(40, 5);
            this.buttonSouth.TabIndex = 1;
            this.buttonSouth.UseVisualStyleBackColor = true;
            this.buttonSouth.Click += new System.EventHandler(this.buttonSouth_Click);
            // 
            // buttonWest
            // 
            this.buttonWest.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonWest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonWest.Location = new System.Drawing.Point(0, 5);
            this.buttonWest.Name = "buttonWest";
            this.buttonWest.Size = new System.Drawing.Size(5, 30);
            this.buttonWest.TabIndex = 2;
            this.buttonWest.UseVisualStyleBackColor = true;
            this.buttonWest.Click += new System.EventHandler(this.buttonWest_Click);
            // 
            // buttonEast
            // 
            this.buttonEast.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonEast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEast.Location = new System.Drawing.Point(35, 5);
            this.buttonEast.Name = "buttonEast";
            this.buttonEast.Size = new System.Drawing.Size(5, 30);
            this.buttonEast.TabIndex = 3;
            this.buttonEast.UseVisualStyleBackColor = true;
            this.buttonEast.Click += new System.EventHandler(this.buttonEast_Click);
            // 
            // labelNumber
            // 
            this.labelNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNumber.Location = new System.Drawing.Point(5, 5);
            this.labelNumber.Name = "labelNumber";
            this.labelNumber.Size = new System.Drawing.Size(30, 30);
            this.labelNumber.TabIndex = 5;
            this.labelNumber.Text = "__";
            this.labelNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // RoomControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.labelNumber);
            this.Controls.Add(this.buttonEast);
            this.Controls.Add(this.buttonWest);
            this.Controls.Add(this.buttonSouth);
            this.Controls.Add(this.buttonNorth);
            this.Name = "RoomControl";
            this.Size = new System.Drawing.Size(40, 40);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonNorth;
        private System.Windows.Forms.Button buttonSouth;
        private System.Windows.Forms.Button buttonWest;
        private System.Windows.Forms.Button buttonEast;
        private System.Windows.Forms.Label labelNumber;
    }
}
