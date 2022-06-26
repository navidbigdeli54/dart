using Core.Network;
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

            Assert.That(remoteProcedures.IsCalled, Is.True);
            Assert.That(parameter, Is.EqualTo(remoteProcedures.Parameter));
        }

        [Test]
        public void InvokingSingleParameterProcedureWithJsonTest()
        {
            DummyRemoteSingleParameterProcedures remoteProcedures = new DummyRemoteSingleParameterProcedures();

            Parameter parameter = new Parameter("parameter", Guid.NewGuid().ToString());
            ((IRemoteProcedures)remoteProcedures).Invoke(new Procedure("DummyProcedure", new Parameter[] { parameter }));

            Assert.That(remoteProcedures.IsCalled, Is.True);
            Assert.That(parameter.Value, Is.EqualTo(remoteProcedures.Parameter));
        }

        [Test]
        public void InvokingTwoeParameterProcedureTest()
        {
            DummyRemoteTwoDifferentParameterProcedures remoteProcedures = new DummyRemoteTwoDifferentParameterProcedures();

            int firstParameter = Random.Shared.Next(0, int.MaxValue);
            bool secondParameter = Random.Shared.Next(0, 1) % 2 == 0;
            ((IRemoteProcedures)remoteProcedures).Invoke("DummyProcedure", new object[] { firstParameter, secondParameter });

            Assert.That(remoteProcedures.IsCalled, Is.True);
            Assert.That(firstParameter, Is.EqualTo(remoteProcedures.FirstParameter));
            Assert.That(secondParameter, Is.EqualTo(remoteProcedures.SecondParameter));
        }

        [Test]
        public void InvokingTwoeParameterProcedureWithJsonTest()
        {
            DummyRemoteTwoDifferentParameterProcedures remoteProcedures = new DummyRemoteTwoDifferentParameterProcedures();

            Parameter firstParameter = new Parameter("first", Random.Shared.Next(0, int.MaxValue));
            Parameter secondParameter = new Parameter("second", Random.Shared.Next(0, 1) % 2 == 0);
            ((IRemoteProcedures)remoteProcedures).Invoke(new Procedure("DummyProcedure", new Parameter[] { firstParameter, secondParameter }));

            Assert.That(remoteProcedures.IsCalled, Is.True);
            Assert.That(firstParameter.Value, Is.EqualTo(remoteProcedures.FirstParameter));
            Assert.That(secondParameter.Value, Is.EqualTo(remoteProcedures.SecondParameter));
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

            Assert.That(remoteProcedures.IsCalled, Is.False);
        }

        [Test]
        public void ProcedureTest()
        {
            Parameter firstParameter = new Parameter("first", Random.Shared.Next(0, int.MaxValue));
            Parameter secondParameter = new Parameter("second", Random.Shared.Next(0, 1) % 2 == 0);
            Procedure procedure = new Procedure("DummyProcedure", new Parameter[] { firstParameter, secondParameter });

            string strigifiedProcedure = procedure.ToString();

            Procedure parsedProcedure = new Procedure(JsonNode.Parse(strigifiedProcedure).AsObject());

            Assert.That(procedure.Name, Is.EqualTo(parsedProcedure.Name));
            Assert.That(procedure.Parameters.Length, Is.EqualTo(parsedProcedure.Parameters.Length));
            Assert.That(procedure.Parameters[0].Name, Is.EqualTo(parsedProcedure.Parameters[0].Name));
            Assert.That(procedure.Parameters[0].Value, Is.EqualTo(parsedProcedure.Parameters[0].Value));
            Assert.That(procedure.Parameters[1].Name, Is.EqualTo(parsedProcedure.Parameters[1].Name));
            Assert.That(procedure.Parameters[1].Value, Is.EqualTo(parsedProcedure.Parameters[1].Value));
        }
        #endregion
    }
}