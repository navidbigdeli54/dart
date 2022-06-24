using Network;
using System.Text;

namespace Test.Network
{
    public class PayloadTest
    {
        [Test]
        public void PayloadSizeTest()
        {
            int Length = Random.Shared.Next(0, StateObject.BUFFER_SIZE);

            Payload payload = new Payload(Length);

            Assert.That(Payload.PAYLOAD_SIZE, Is.EqualTo(Encoding.ASCII.GetByteCount(payload.ToJson().ToJsonString())));
        }
    }
}
