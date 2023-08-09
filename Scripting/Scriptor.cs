using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Loader;

namespace Scripting
{
    public class Scriptor
    {
        // using Roslyn
        // https://learn.microsoft.com/ko-kr/dotnet/api/microsoft.codeanalysis.csharp.csharpcompilation?view=roslyn-dotnet-4.6.0

        private CSharpCompilation? Compiler = null;

        public string SrcPath { get; set; } = "";
        public string DllPath { get; set; } = "";

        public bool Prepare(string srcCode, string fileName = "")
        {
            // 1. create syntax tree
            // parsing
            Debug.WriteLine("Parsing the code into the SyntaxTree");
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(srcCode);

            // 2. create references
            // get .NET installed path
            string runtimeDir = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            Debug.WriteLine($"runtime by interop -> {runtimeDir}");

            // set referencing packages
            var refPaths = new[] {
                // add assembly path by TypeInfo(QualifiedClassName)
                typeof(System.Object).GetTypeInfo().Assembly.Location,
                typeof(Console).GetTypeInfo().Assembly.Location,
                //typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location,
                // add assembly path based .NETCore path
                Path.Combine(runtimeDir, "System.Runtime.dll"),
                // add my library file path
                Path.Combine(Environment.CurrentDirectory, "TestLib.dll")
            };
            // print referencing packages
            Debug.WriteLine("Adding the following references");
            foreach (var r in refPaths)
            {
                Debug.WriteLine(r);
            }

            // create metadata from referencing packages
            MetadataReference[] references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();

            // 3. create compile options
            CSharpCompilationOptions csOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

            // 4. create compiler with syntaxTreee, meta-refs, compile-options
            Debug.WriteLine("Compiling ...");
            string asmName = String.IsNullOrEmpty(fileName) ? Path.GetRandomFileName() : fileName;
            this.Compiler = CSharpCompilation.Create(
                asmName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: csOptions);

            return true;
        }

        public Assembly? Compile()
        {
            if (this.Compiler == null)
                return null;

            using var ms = new MemoryStream();
            EmitResult result = this.Compiler.Emit(ms);
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
                return null;
            }

            Debug.WriteLine("Compilation successful!");
            ms.Seek(0, SeekOrigin.Begin);

            return AssemblyLoadContext.Default.LoadFromStream(ms);
        }

        public bool Compile(string fileName)
        {
            if (this.Compiler == null)
                return false;

            string filePath = Path.Combine(this.DllPath, fileName + ".dll");
            Debug.WriteLine($"Compilation emit path = {filePath}");

            EmitResult result = this.Compiler.Emit(filePath);
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

            Debug.WriteLine("Compilation successful!");
            return true;
        }

        public Assembly? LoadFile(string fileName)
        {
            string asmFile = fileName + @".dll";
            string asmPath = Path.Combine(this.DllPath, asmFile);
            Debug.WriteLine($"Load assembly, path={asmPath}");
            try
            {
                return AssemblyLoadContext.Default.LoadFromAssemblyPath(asmPath);
            }
            catch (Exception e)
            {
                Debug.Write($"Error, ex={e.Message}");
            }
            return null;
        }

        public void RunScript(Assembly assembly, string methodName)
        {
            // check assembly
            if (assembly == null)
                return;

            // list up class in assembly
            Debug.WriteLine($"List up all classes in assembly {assembly.GetName()}");
            //foreach (Type t in assembly.GetTypes())
            foreach (Type t in assembly.GetExportedTypes())
            {
                Debug.WriteLine($"\t {t.FullName}");
            }

            // get class type in assembly
            //Debug.WriteLine($"Run assembly, className= {assembly.GetTypes().Last().FullName}");
            //string qfyName = "FcpScripts." + className;
            //Debug.WriteLine($"Run assembly, paramName= {qfyName}");
            //var type = assembly.GetType(qfyName);
            Type type = assembly.GetExportedTypes().First();
            if (type == null)
            {
                return;
            }

            // get method info for invoking
            //var instance = assembly.CreateInstance(qfyName)
            var instance = assembly.CreateInstance(type.FullName);
            var m = type.GetMember(methodName).First() as MethodInfo;
            if (m != null)
            {
                 m.Invoke(instance, null);
            }
        }

        public void RunTestCase(Assembly assembly, string className)
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
