using Network;
using System.Text.Json.Nodes;

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
        public void InvokingSingleParameterProcedureWithJsonTest()
        {
            DummyRemoteSingleParameterProcedures remoteProcedures = new DummyRemoteSingleParameterProcedures();

            Parameter parameter = new Parameter("parameter", Guid.NewGuid().ToString());
            ((IRemoteProcedures)remoteProcedures).Invoke(new Procedure("DummyProcedure", new Parameter[] { parameter }));

            Assert.IsTrue(remoteProcedures.IsCalled);
            Assert.AreEqual(parameter.Value, remoteProcedures.Parameter);
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
        public void InvokingTwoeParameterProcedureWithJsonTest()
        {
            DummyRemoteTwoDifferentParameterProcedures remoteProcedures = new DummyRemoteTwoDifferentParameterProcedures();

            Parameter firstParameter = new Parameter("first", Random.Shared.Next(0, int.MaxValue));
            Parameter secondParameter = new Parameter("second", Random.Shared.Next(0, 1) % 2 == 0);
            ((IRemoteProcedures)remoteProcedures).Invoke(new Procedure("DummyProcedure", new Parameter[] { firstParameter, secondParameter }));

            Assert.IsTrue(remoteProcedures.IsCalled);
            Assert.AreEqual(firstParameter.Value, remoteProcedures.FirstParameter);
            Assert.AreEqual(secondParameter.Value, remoteProcedures.SecondParameter);
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

        [Test]
        public void ProcedureTest()
        {
            Parameter firstParameter = new Parameter("first", Random.Shared.Next(0, int.MaxValue));
            Parameter secondParameter = new Parameter("second", Random.Shared.Next(0, 1) % 2 == 0);
            Procedure procedure = new Procedure("DummyProcedure", new Parameter[] { firstParameter, secondParameter });

            string strigifiedProcedure = procedure.ToString();

            Procedure parsedProcedure = new Procedure(JsonNode.Parse(strigifiedProcedure).AsObject());

            Assert.AreEqual(procedure.Name, parsedProcedure.Name);
            Assert.AreEqual(procedure.Parameters.Length, parsedProcedure.Parameters.Length);
            Assert.AreEqual(procedure.Parameters[0].Name, parsedProcedure.Parameters[0].Name);
            Assert.AreEqual(procedure.Parameters[0].Value, parsedProcedure.Parameters[0].Value);
            Assert.AreEqual(procedure.Parameters[1].Name, parsedProcedure.Parameters[1].Name);
            Assert.AreEqual(procedure.Parameters[1].Value, parsedProcedure.Parameters[1].Value);
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