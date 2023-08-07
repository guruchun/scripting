namespace Scripting
{
    partial class Form2
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
            CodeView = new TextBox();
            Build = new Button();
            Run = new Button();
            Open = new Button();
            CodePath = new TextBox();
            Test = new Button();
            SuspendLayout();
            // 
            // CodeView
            // 
            CodeView.Location = new Point(45, 76);
            CodeView.Multiline = true;
            CodeView.Name = "CodeView";
            CodeView.Size = new Size(413, 331);
            CodeView.TabIndex = 0;
            CodeView.Text = "abc";
            // 
            // Build
            // 
            Build.Location = new Point(640, 93);
            Build.Name = "Build";
            Build.Size = new Size(75, 23);
            Build.TabIndex = 1;
            Build.Text = "Build";
            Build.UseVisualStyleBackColor = true;
            Build.Click += Build_Click;
            // 
            // Run
            // 
            Run.Location = new Point(640, 177);
            Run.Name = "Run";
            Run.Size = new Size(75, 23);
            Run.TabIndex = 1;
            Run.Text = "Run";
            Run.UseVisualStyleBackColor = true;
            Run.Click += Run_Click;
            // 
            // Open
            // 
            Open.Location = new Point(640, 49);
            Open.Name = "Open";
            Open.Size = new Size(75, 23);
            Open.TabIndex = 2;
            Open.Text = "Open";
            Open.UseVisualStyleBackColor = true;
            Open.Click += Open_Click;
            // 
            // CodePath
            // 
            CodePath.Location = new Point(45, 47);
            CodePath.Name = "CodePath";
            CodePath.Size = new Size(413, 23);
            CodePath.TabIndex = 3;
            // 
            // Test
            // 
            Test.Location = new Point(640, 384);
            Test.Name = "Test";
            Test.Size = new Size(75, 23);
            Test.TabIndex = 4;
            Test.Text = "Test";
            Test.UseVisualStyleBackColor = true;
            Test.Click += Test_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(Test);
            Controls.Add(CodePath);
            Controls.Add(Open);
            Controls.Add(Run);
            Controls.Add(Build);
            Controls.Add(CodeView);
            Name = "Form2";
            Text = "Form1";
            Load += Form2_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox CodeView;
        private Button Build;
        private Button Run;
        private Button Open;
        private TextBox CodePath;
        private Button Test;
    }
}