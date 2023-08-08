using System.Reflection;
using Microsoft.CodeAnalysis;
using System.Diagnostics;

namespace Scripting
{
    public partial class Form2 : Form
    {
        private Scriptor Runner = new Scriptor();

        public Form2()
        {
            InitializeComponent();

            Runner.SrcPath = Path.Combine(Application.StartupPath, "asmSrc");
            Runner.DllPath = Path.Combine(Application.StartupPath, "asmDll");
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // check directory exists
            if (!Directory.Exists(Runner.SrcPath))
                Directory.CreateDirectory(Runner.SrcPath);
            if (!Directory.Exists(Runner.DllPath))
                Directory.CreateDirectory(Runner.DllPath);

            // list up assembly source files
            // "$AppRoot/asmSrc/*.cs"
            DirectoryInfo d = new(Runner.SrcPath);
            string[] sourceFiles = d.EnumerateFiles(@"*.cs", SearchOption.AllDirectories)
                .Select(a => a.Name).ToArray();
            ScriptList.Items.AddRange(sourceFiles);
            if (sourceFiles.Length > 0)
            {
                ScriptList.SelectedIndex = 0;
            }
        }

        private void Open_Click(object sender, EventArgs e)
        {
            // load source file
            if (ScriptList.SelectedIndex < 0)
            {
                CodeName.Text = "";
                CodeView.Text = "";
                return;
            }

            // select source file from the source list
            //string fileName = @"TestScript1.cs";
            string? fileName = ScriptList.SelectedItem.ToString();
            if (fileName == null)
                return;

            CodeName.Text = Path.GetFileNameWithoutExtension(fileName);
            string filePath = Path.Combine(Runner.SrcPath, fileName);

            // open the file and display into textview
            string srcCode = File.ReadAllText(filePath);
            CodeView.Text = srcCode;
        }

        private void Build_Click(object sender, EventArgs e)
        {
            // "$AppRoot/asmSrc/*.cs" --> "$AppRoot/asmDll/*.dll"
            Runner.Prepare(CodeView.Text, CodeName.Text);
            Runner.Compile(CodeName.Text);
        }

        private void Run_Click(object sender, EventArgs e)
        {
            // load assembly from file
            Assembly? assembly = Runner.LoadFile(CodeName.Text);
            if (assembly != null)
            {
                // run method of class in assembly
                Runner.RunScript(assembly, CodeName.Text);
                //Scriptor.RunTestCase(assembly, className);
            }
        }

        private void BuildRun_Click(object sender, EventArgs e)
        {
            Runner.Prepare(CodeView.Text);
            Assembly? asm = Runner.Compile();
            if (asm != null)
            {
                Runner.RunScript(asm, CodeName.Text);
            }
        }

        // call directly for testing
        private void Test_Click(object sender, EventArgs e)
        {
            TestLib.MyAccess myAccess = new TestLib.MyAccess();
            int vGet = myAccess.GetValue();
            Debug.WriteLine($"direct get = {vGet}");
            myAccess.SetValue(vGet + 5);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            // overwriting source file
        }
    }
}

//namespace SimpleScripts
//{
//    public class MyScript : ScriptType.IScriptType1
//    {
//        public int LibFunc1(int value)
//        {
//            MyAccess obj = new();
//            return obj.TestMethod(value);
//        }
//    }
//}

