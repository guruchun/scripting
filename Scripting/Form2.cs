using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Diagnostics;
using System.Linq;
//using TestLib;
//ÇÑ±ÛÀÌ¶û english¶û

namespace Scripting
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Open_Click(object sender, EventArgs e)
        {
            // TODO load source file
            // select source file from the source list
            //string path = @"TestScript1.cs";
            CodePath.Text = Path.Combine(Application.StartupPath, @"TestScript1.cs");

            // open the file and display into textview
            //string readText = File.ReadAllText(path);
            string readText = @"
                using TestLib;
                namespace FcpScripts
                {
                    public class TestScript1
                    {
                        public bool SetUp()
                        {
                        }

                        public bool RunTest(int value)
                        {
                            MyAccess obj = new MyAccess();
                            return obj.TestMethod(value);
                        }

                        public bool TearDown()
                        {
                        }
                    }
                }
            ";

            CodeView.Text = readText;
        }

        private void Build_Click(object sender, EventArgs e)
        {
            Scriptor.CompileCode(CodeView.Text, Path.GetFileNameWithoutExtension(CodePath.Text));
        }

        // call directly for testing
        private void Run_Click(object sender, EventArgs e)
        {
            // find file and load to assembly
            string className = Path.GetFileNameWithoutExtension(CodePath.Text);
            string asmFile =  className + @".dll";

            //Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
            Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(asmFile);

            // run method of class in assembly
            Scriptor.RunTestCase(assembly, className);
        }

        private void Test_Click(object sender, EventArgs e)
        {
            int vGet = TestLib.MyAccess.GetValue();
            Debug.WriteLine($"direct get = {vGet}");
            TestLib.MyAccess.SetValue(vGet + 5);
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO list up assembly source files
            // compile "./asmSrc/*.cs" to "./asmDll/*.dll"

            //DirectoryInfo d = new DirectoryInfo(Path.Combine(Application.StartupPath, "asmSrc"));
            //string[] sourceFiles = d.EnumerateFiles(@"*.cs", SearchOption.AllDirectories)
            //    .Select(a => a.FullName).ToArray();
        }
    }

    public class Scriptor
    {
        // using Roslyn(CodeAnalysis.CSharp)
        public static bool CompileCode(string code, string fileName)
        {
            // parsing
            Debug.WriteLine("Parsing the code into the SyntaxTree");
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

            // set referencing packages
            // get .net runtime path
            string runtimeDir = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            Debug.WriteLine($"runtime by interop -> {runtimeDir}");
            var refPaths = new[] {
                // add assembly path by TypeInfo(QualifiedClassName)
                typeof(System.Object).GetTypeInfo().Assembly.Location,
                typeof(Console).GetTypeInfo().Assembly.Location,
                // add assembly path based .NETCore path
                Path.Combine(runtimeDir, "System.Runtime.dll"),
                // add my library file path
                Path.Combine(Environment.CurrentDirectory, "TestLib.dll")
            };
            Debug.WriteLine("Adding the following references");
            foreach (var r in refPaths)
                Debug.WriteLine(r);

            // create metadata from referencing packages
            MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

            // complile the code with syntaxTreee, meta-refs, compile-options
            Debug.WriteLine("Compiling ...");
            string asmName = String.IsNullOrEmpty(fileName) ? Path.GetRandomFileName() : fileName;
            CSharpCompilation compilation = CSharpCompilation.Create(
                asmName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    Debug.WriteLine("Compilation failed!");
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Debug.WriteLine("\t{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }
                    return false;
                }

                // compile ok --> save to assembly file
                //
                // load to assembly
                Debug.WriteLine("Compilation successful! Now instantiating and executing the code ...");
                ms.Seek(0, SeekOrigin.Begin);

                Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
            }

            return true;
        }

        public static void RunTestCase(Assembly assembly, string className)
        {
            // check assembly
            if (assembly == null)
                return;

            // get class type in assembly
            string qfyName = "FcpScripts." + className;
            var type = assembly.GetType(qfyName);
            if (type == null)
            {
                return;
            }

            // get method info for invoking
            var instance = assembly.CreateInstance(qfyName);
            var setUp = type.GetMember("SetUp").First() as MethodInfo;
            if (setUp != null)
            {
                setUp.Invoke(instance, null);
            }
            // TODO run all test cases
            //MethodInfo[] tests = type.GetMembers().Where(p => p.Name.StartsWith("test")).ToArray<MethodInfo>();
            //for (var tc in tests)
            //{
            //    tc.Invoke(instance, null);
            //}
            var tearDown = type.GetMember("TearDown").First() as MethodInfo;
            if (tearDown != null)
            {
                tearDown.Invoke(instance, null);
            }
        }

        //public static void RunInterface(Assembly assembly)
        //{
        //    foreach (Type type in assembly.GetExportedTypes())
        //    {
        //        foreach (Type iface in type.GetInterfaces())
        //        {
        //            if (iface == typeof(ScriptType.IScriptType1))
        //            {
        //                ConstructorInfo constructor = type.GetConstructor(System.Type.EmptyTypes);
        //                if (constructor != null && constructor.IsPublic)
        //                {
        //                    ScriptType.IScriptType1 scriptObject = constructor.Invoke(null) as ScriptType.IScriptType1;
        //                    if (scriptObject != null)
        //                    {
        //                        int retVal = scriptObject.LibFunc1(50);
        //                        Debug.WriteLine($"LibFunc1 result={retVal}");
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
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

