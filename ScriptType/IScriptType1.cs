namespace ScriptType
{
    public interface IScriptType1
    {
        int LibFunc1(int value);
    }

    public interface IScriptTC
    {
        public bool SetUp();
        public bool RunTest();
        public bool TearDown();
    }
}