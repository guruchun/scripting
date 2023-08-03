namespace TestLib
{
    public class MyAccess
    {
        static int stcValue;
        public int TestMethod(int value)
        {
            Console.WriteLine($"MyAccess.TestMethod() is executing... input={value}");
            stcValue = value * value;
            return stcValue;
        }

        public static void SetValue(int value)
        {
            stcValue = value;
        }
        public static int GetValue()
        {
            return stcValue;
        }
    }
}