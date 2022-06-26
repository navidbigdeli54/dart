using Core.Network;

namespace Test.Network
{
    public class DummyRemoteTwoDifferentParameterProcedures : RemoteProcedures
    {
        public bool IsCalled { get; private set; }

        public int FirstParameter { get; private set; }

        public bool SecondParameter { get; private set; }

        public void DummyProcedure(int first, bool second)
        {
            IsCalled = true;
            FirstParameter = first;
            SecondParameter = second;
        }
    }
}