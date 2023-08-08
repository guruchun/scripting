using System.Diagnostics;
using TestLib;
namespace FcpScripts
{
    public class TestScript1
    {

        public bool Run()
        {
            MyAccess obj = new MyAccess();
            int ret = obj.Power(3);
            Debug.WriteLine($"called TestMethod(3)={ret}");
            int val = obj.GetValue();
            Debug.WriteLine($"called Get={val}");
            obj.SetValue(77);
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