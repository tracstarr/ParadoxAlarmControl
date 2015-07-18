using NUnit.Framework;
using ParadoxIp.Managers;

namespace ParadoxIpTest
{
    [TestFixture]
    public class NotificationTests
    {
        [Test]
        public void ListenTest()
        {
            ParadoxNotificationListener.Main();
            Assert.Inconclusive("Not completed yet");
        }
    }
}
