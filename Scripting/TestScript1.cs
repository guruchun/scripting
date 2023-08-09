using System.Diagnostics;
using TestLib;
namespace FcpScripts
{
    public class TestScript1
    {

        public bool Run()
        {
            MyAccess myAccess = MyAccess.GetInstance();
            Debug.WriteLine($"called Run");

            // get
            int a = myAccess.GetTag<int>("aaa");
            double b = myAccess.GetTag<double>("bbb");
            string? bstr = myAccess.GetTag<string>("bbb");
            Debug.WriteLine($"direct get aaa= {a}, bbb={b}, bstr={bstr}");

            // set
            myAccess.SetTag("aaa", a + 1);
            myAccess.SetTag<double>("bbb", b + 0.1D);

            return true;
        }

        public bool SetUp()
        {
            Debug.WriteLine("called setup");
            return true;
        }
        public bool testCase1()
        {
            Debug.WriteLine("called testCase1");
            return true;
        }
        public bool testCase2()
        {
            Debug.WriteLine("called testCase2");
            return true;
        }

        public bool TearDown()
        {
            Debug.WriteLine("called teardown");
            return true;
        }
    }
}