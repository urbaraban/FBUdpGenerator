using FBUdpGenerator;
using FBUdpGenerator.Services;
using System.Net;

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

        [Test]
        public void TestSendToReciver()
        {
            UdpReciver udpReciver = new UdpReciver();
            udpReciver.StartRecive(5011);

            UdpSender udpSender = new UdpSender();
            udpSender.Send(
                ShuffleGenerator.GetByteArray(1000),
                new IPEndPoint(IPAddress.Loopback, 5011),
                TrafficLoadMBits.OneMbit);


            udpReciver.StopRecive();
            Thread.Sleep(1100);
            Assert.IsFalse(udpReciver.IsReciving);
        }
    }
}