using System.Reflection;
using System.Runtime.Loader;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System.Diagnostics;
//using TestLib;
//ÇÑ±ÛÀÌ¶û english¶û

namespace Scripting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Build_Click(object sender, EventArgs e)
        {
            ScriptingEx.StartScript();
        }

        // call directly for testing
        private void Run_Click(object sender, EventArgs e)
        {
#if false
            MyAccess myAccess = new MyAccess();
            int retValue = myAccess.TestMethod(15);
            Debug.WriteLine($"result={retValue}");
#else
            int vGet = TestLib.MyAccess.GetValue();
            Debug.WriteLine($"direct get = {vGet}");
            TestLib.MyAccess.SetValue(vGet + 5);
#endif
        }
    }

    public class ScriptingEx
    {
        public static void StartScript()
        {
            // TODO load source file
            // select source file from the source list
            //string path = @"TestScript1.cs";

            // open the file and display into textview
            //string readText = File.ReadAllText(path);
            string readText = @"
                using TestLib;
                namespace SimpleScripts
                {
                    public class MyScript : ScriptType.IScriptType1
                    {
                        public int LibFunc1(int value)
                        {
                            MyAccess obj = new MyAccess();
                            return obj.TestMethod(value);
                        }
                    }
                }
            ";

            Assembly? compiledScript = CompileCode(readText);
            if (compiledScript != null)
            {
                RunScript(compiledScript);
            }
        }

#if (false)
// using CodeDOM
        static Assembly CompileCode(string code)
        {
            Microsoft.CSharp.CSharpCodeProvider csProvider = new Microsoft.CSharp.CSharpCodeProvider();

            CompilerParameters options = new CompilerParameters();
            options.GenerateExecutable = false;
            options.GenerateInMemory = true;


            options.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

            //Add references to ScriptingInterface.dll
            String pathTestLib = Path.Combine(Environment.CurrentDirectory, "TestLib.dll");
            options.ReferencedAssemblies.Add(pathTestLib);
            String pathIntfLib = Path.Combine(Environment.CurrentDirectory, "ScriptType.dll");
            options.ReferencedAssemblies.Add(pathIntfLib);

            // Compile our code
            CompilerResults result;
            result = csProvider.CompileAssemblyFromSource(options, code);

            if (result.Errors.HasErrors)
            {
                // Report back to the user that the script has errored
                Debug.WriteLine("Script has errored");

                for (int i = 0; i < result.Errors.Count; i++)
                {
                    Debug.WriteLine("Error {0}: {1}", i + 1, result.Errors[i]);
                }
                return null;
            }

            if (result.Errors.HasWarnings)
            {
                Debug.WriteLine("Script has warnings");
            }

            return result.CompiledAssembly;
        }
#else
        // using Roslyn
        static Assembly? CompileCode(string code)
        {
            Debug.WriteLine("Parsing the code into the SyntaxTree");
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

            // TODO 
            string assemblyName = Path.GetRandomFileName();
            var refPaths = new[] {
                typeof(System.Object).GetTypeInfo().Assembly.Location,
                typeof(Console).GetTypeInfo().Assembly.Location,
                Path.Combine(Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location), "System.Runtime.dll"),
                Path.Combine(Environment.CurrentDirectory, "ScriptType.dll"),
                Path.Combine(Environment.CurrentDirectory, "TestLib.dll")
        };
            MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

            Debug.WriteLine("Adding the following references");
            foreach (var r in refPaths)
                Debug.WriteLine(r);

            Debug.WriteLine("Compiling ...");
            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
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
                }
                else
                {
                    Debug.WriteLine("Compilation successful! Now instantiating and executing the code ...");
                    ms.Seek(0, SeekOrigin.Begin);

                    Assembly assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
                    return assembly;
                }
            }

            return null;
        }
#endif

        static void RunScript(Assembly assembly)
        {
            foreach (Type type in assembly.GetExportedTypes())
            {
                foreach (Type iface in type.GetInterfaces())
                {
                    if (iface == typeof(ScriptType.IScriptType1))
                    {
                        ConstructorInfo constructor = type.GetConstructor(System.Type.EmptyTypes);
                        if (constructor != null && constructor.IsPublic)
                        {
                            ScriptType.IScriptType1 scriptObject = constructor.Invoke(null) as ScriptType.IScriptType1;
                            if (scriptObject != null)
                            {
                                int retVal = scriptObject.LibFunc1(50);
                                Debug.WriteLine($"LibFunc1 result={retVal}");
                            }
                        }
                    }
                }
            }

            //var type = assembly.GetType("RoslynCompileSample.Writer");
            //var instance = assembly.CreateInstance("RoslynCompileSample.Writer");
            //var meth = type.GetMember("Write").First() as MethodInfo;
            //meth.Invoke(instance, new[] { "joel" });
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

