using Core.Network;

namespace Test.Network
{
    public class DummyRemoteSingleParameterProcedures : RemoteProcedures
    {
        public bool IsCalled { get; private set; }

        public string Parameter { get; private set; }

        public void DummyProcedure(string parameter)
        {
            IsCalled = true;
            Parameter = parameter;
        }
    }
}