namespace IgraProg3
{
    partial class Vpis
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
            this.vzdevek = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.igrajGumb = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.slikeButton = new System.Windows.Forms.Button();
            this.obstojSlikTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // vzdevek
            // 
            this.vzdevek.Location = new System.Drawing.Point(290, 163);
            this.vzdevek.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.vzdevek.Name = "vzdevek";
            this.vzdevek.Size = new System.Drawing.Size(320, 31);
            this.vzdevek.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(64, 158);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(211, 34);
            this.label1.TabIndex = 1;
            this.label1.Text = "Vpiši vzdevek:";
            // 
            // igrajGumb
            // 
            this.igrajGumb.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.igrajGumb.Location = new System.Drawing.Point(290, 258);
            this.igrajGumb.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.igrajGumb.Name = "igrajGumb";
            this.igrajGumb.Size = new System.Drawing.Size(320, 52);
            this.igrajGumb.TabIndex = 2;
            this.igrajGumb.Text = "Igraj";
            this.igrajGumb.UseVisualStyleBackColor = true;
            this.igrajGumb.Click += new System.EventHandler(this.igrajGumb_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS Reference Sans Serif", 16.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(360, 37);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(204, 53);
            this.label2.TabIndex = 3;
            this.label2.Text = "SPOMIN";
            // 
            // slikeButton
            // 
            this.slikeButton.Font = new System.Drawing.Font("MS Reference Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.slikeButton.Location = new System.Drawing.Point(290, 439);
            this.slikeButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.slikeButton.Name = "slikeButton";
            this.slikeButton.Size = new System.Drawing.Size(320, 52);
            this.slikeButton.TabIndex = 4;
            this.slikeButton.Text = "Zamenjaj imenik";
            this.slikeButton.UseVisualStyleBackColor = true;
            this.slikeButton.Click += new System.EventHandler(this.slikeButton_Click);
            // 
            // obstojSlikTextBox
            // 
            this.obstojSlikTextBox.Enabled = false;
            this.obstojSlikTextBox.Location = new System.Drawing.Point(290, 360);
            this.obstojSlikTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.obstojSlikTextBox.Name = "obstojSlikTextBox";
            this.obstojSlikTextBox.Size = new System.Drawing.Size(320, 31);
            this.obstojSlikTextBox.TabIndex = 5;
            this.obstojSlikTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Vpis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightBlue;
            this.ClientSize = new System.Drawing.Size(932, 542);
            this.Controls.Add(this.obstojSlikTextBox);
            this.Controls.Add(this.slikeButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.igrajGumb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.vzdevek);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Vpis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vpis";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox vzdevek;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button igrajGumb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button slikeButton;
        private System.Windows.Forms.TextBox obstojSlikTextBox;
    }
}