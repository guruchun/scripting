namespace TestLib
{
    public class MyAccess
    {
        public int TestMethod(int value)
        {
            Console.WriteLine($"MyAccess.TestMethod() is executing... input={value}");
            return value * value;
        }
    }
}