using Network;

namespace Test.Network
{
    public class RemoteProcedureTest
    {
        #region Public Methods
        [Test]
        public void InvokingParameterlessProcedureTest()
        {
            DummyRemoteParameterlessProcedures remoteProcedures = new DummyRemoteParameterlessProcedures();

            ((IRemoteProcedures)remoteProcedures).Invoke("DummyProcedure");

            Assert.IsTrue(remoteProcedures.IsCalled);
        }

        [Test]
        public void InvokingSingleParameterProcedureTest()
        {
            DummyRemoteSingleParameterProcedures remoteProcedures = new DummyRemoteSingleParameterProcedures();

            string parameter = Guid.NewGuid().ToString();
            ((IRemoteProcedures)remoteProcedures).Invoke("DummyProcedure", new object[] { parameter });

            Assert.IsTrue(remoteProcedures.IsCalled);
            Assert.AreEqual(parameter, remoteProcedures.Parameter);
        }

        [Test]
        public void InvokingTwoeParameterProcedureTest()
        {
            DummyRemoteTwoDifferentParameterProcedures remoteProcedures = new DummyRemoteTwoDifferentParameterProcedures();

            int firstParameter = Random.Shared.Next(0, int.MaxValue);
            bool secondParameter = Random.Shared.Next(0, 1) % 2 == 0;
            ((IRemoteProcedures)remoteProcedures).Invoke("DummyProcedure", new object[] { firstParameter, secondParameter });

            Assert.IsTrue(remoteProcedures.IsCalled);
            Assert.AreEqual(firstParameter, remoteProcedures.FirstParameter);
            Assert.AreEqual(secondParameter, remoteProcedures.SecondParameter);
        }

        [Test]
        public void ConstructingTwoRemoteProcedureWithTheSameNameTest()
        {
            Assert.Throws<ArgumentException>(() => new DummyTwoRemoteProcedureWithTheSameName());
        }

        [Test]
        public void InvokingPrivateProcedureTest()
        {
            DummyRemoteProcedurePrivateMethod remoteProcedures = new DummyRemoteProcedurePrivateMethod();

            ((IRemoteProcedures)remoteProcedures).Invoke("DummyProcedure");

            Assert.IsFalse(remoteProcedures.IsCalled);
        }
        #endregion

        #region Nested Types
        private class DummyRemoteParameterlessProcedures : RemoteProcedures
        {
            public bool IsCalled { get; private set; }

            public void DummyProcedure()
            {
                IsCalled = true;
            }
        }

        private class DummyRemoteSingleParameterProcedures : RemoteProcedures
        {
            public bool IsCalled { get; private set; }

            public string Parameter { get; private set; }

            public void DummyProcedure(string parameter)
            {
                IsCalled = true;
                Parameter = parameter;
            }
        }

        private class DummyRemoteTwoDifferentParameterProcedures : RemoteProcedures
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

        private class DummyTwoRemoteProcedureWithTheSameName : RemoteProcedures
        {
            public void DummyProcedure() { }

            public void DummyProcedure(string parameter) { }
        }

        private class DummyRemoteProcedurePrivateMethod : RemoteProcedures
        {
            public bool IsCalled { get; private set; }

            private void DummyProcedure()
            {
                IsCalled = true;
            }
        }
        #endregion
    }
}