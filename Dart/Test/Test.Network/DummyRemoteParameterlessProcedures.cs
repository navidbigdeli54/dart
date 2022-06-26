using Core.Network;

namespace Test.Network
{
    public class DummyRemoteParameterlessProcedures : RemoteProcedures
    {
        public bool IsCalled { get; private set; }

        public void DummyProcedure()
        {
            IsCalled = true;
        }
    }
}