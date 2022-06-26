using Core.Network;

namespace Test.Network
{
    public class DummyRemoteProcedurePrivateMethod : RemoteProcedures
    {
        public bool IsCalled { get; private set; }

        private void DummyProcedure()
        {
            IsCalled = true;
        }
    }
}