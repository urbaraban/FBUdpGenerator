using FBUdpGenerator;

namespace FPUdpGeneratorTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestReciver()
        {
            UdpReciver udpReciver = new UdpReciver();
            udpReciver.StartRecive(5011);
            Assert.IsTrue(udpReciver.IsReciving);
        }

        [Test]
        public void TestStopReciver()
        {
            UdpReciver udpReciver = new UdpReciver();
            udpReciver.StartRecive(5011);
            udpReciver.StopRecive();
            Thread.Sleep(1000);
            Assert.IsFalse(udpReciver.IsReciving);
        }
    }
}