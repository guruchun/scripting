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
            CodeName = new TextBox();
            Test = new Button();
            BuildRun = new Button();
            ScriptList = new ComboBox();
            Save = new Button();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // CodeView
            // 
            CodeView.Location = new Point(45, 76);
            CodeView.Multiline = true;
            CodeView.Name = "CodeView";
            CodeView.Size = new Size(516, 331);
            CodeView.TabIndex = 0;
            CodeView.Text = "abc";
            // 
            // Build
            // 
            Build.Location = new Point(592, 107);
            Build.Name = "Build";
            Build.Size = new Size(140, 23);
            Build.TabIndex = 1;
            Build.Text = "Build && Save";
            Build.UseVisualStyleBackColor = true;
            Build.Click += Build_Click;
            // 
            // Run
            // 
            Run.Location = new Point(592, 136);
            Run.Name = "Run";
            Run.Size = new Size(140, 23);
            Run.TabIndex = 1;
            Run.Text = "Load && Run";
            Run.UseVisualStyleBackColor = true;
            Run.Click += Run_Click;
            // 
            // Open
            // 
            Open.Location = new Point(592, 49);
            Open.Name = "Open";
            Open.Size = new Size(67, 23);
            Open.TabIndex = 2;
            Open.Text = "Open";
            Open.UseVisualStyleBackColor = true;
            Open.Click += Open_Click;
            // 
            // CodePath
            // 
            CodeName.Location = new Point(392, 47);
            CodeName.Name = "CodePath";
            CodeName.Size = new Size(169, 23);
            CodeName.TabIndex = 3;
            // 
            // Test
            // 
            Test.Location = new Point(592, 384);
            Test.Name = "Test";
            Test.Size = new Size(140, 23);
            Test.TabIndex = 4;
            Test.Text = "Test";
            Test.UseVisualStyleBackColor = true;
            Test.Click += Test_Click;
            // 
            // BuildRun
            // 
            BuildRun.Location = new Point(592, 339);
            BuildRun.Name = "BuildRun";
            BuildRun.Size = new Size(140, 23);
            BuildRun.TabIndex = 5;
            BuildRun.Text = "Build && Run";
            BuildRun.UseVisualStyleBackColor = true;
            BuildRun.Click += BuildRun_Click;
            // 
            // ScriptList
            // 
            ScriptList.DropDownStyle = ComboBoxStyle.DropDownList;
            ScriptList.FormattingEnabled = true;
            ScriptList.Location = new Point(45, 47);
            ScriptList.Name = "ScriptList";
            ScriptList.Size = new Size(341, 23);
            ScriptList.TabIndex = 6;
            // 
            // Save
            // 
            Save.Location = new Point(665, 49);
            Save.Name = "Save";
            Save.Size = new Size(67, 23);
            Save.TabIndex = 7;
            Save.Text = "Save";
            Save.UseVisualStyleBackColor = true;
            Save.Click += Save_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(592, 25);
            label1.Name = "label1";
            label1.Size = new Size(79, 15);
            label1.TabIndex = 8;
            label1.Text = "Script Source";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(592, 89);
            label2.Name = "label2";
            label2.Size = new Size(94, 15);
            label2.TabIndex = 9;
            label2.Text = "Compiled Script";
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(Save);
            Controls.Add(ScriptList);
            Controls.Add(BuildRun);
            Controls.Add(Test);
            Controls.Add(CodeName);
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
        private TextBox CodeName;
        private Button Test;
        private Button BuildRun;
        private ComboBox ScriptList;
        private Button Save;
        private Label label1;
        private Label label2;
    }
}