namespace TestLib
{
    public class MyAccess
    {
        static int stcValue;
        public int Power(int value)
        {
            Console.WriteLine($"MyAccess.TestMethod() is executing... input={value}");
            stcValue = value * value;
            return stcValue;
        }

        public void SetValue(int value)
        {
            stcValue = value;
        }
        public int GetValue()
        {
            return stcValue;
        }
    }
}