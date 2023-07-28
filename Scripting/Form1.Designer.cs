namespace Scripting
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
            textBox1 = new TextBox();
            Build = new Button();
            Run = new Button();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(45, 49);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(413, 358);
            textBox1.TabIndex = 0;
            textBox1.Text = "abc";
            // 
            // Build
            // 
            Build.Location = new Point(640, 51);
            Build.Name = "Build";
            Build.Size = new Size(75, 23);
            Build.TabIndex = 1;
            Build.Text = "Build";
            Build.UseVisualStyleBackColor = true;
            Build.Click += Build_Click;
            // 
            // Run
            // 
            Run.Location = new Point(640, 93);
            Run.Name = "Run";
            Run.Size = new Size(75, 23);
            Run.TabIndex = 1;
            Run.Text = "Run";
            Run.UseVisualStyleBackColor = true;
            Run.Click += Run_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(Run);
            Controls.Add(Build);
            Controls.Add(textBox1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private Button Build;
        private Button Run;
    }
}