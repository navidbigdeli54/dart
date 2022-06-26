using Core.Network;

namespace Test.Network
{
    public class DummyTwoRemoteProcedureWithTheSameName : RemoteProcedures
    {
        public void DummyProcedure() { }

        public void DummyProcedure(string parameter) { }
    }
}