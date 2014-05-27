namespace Tests
{
    using System.Threading;

    using log4net;
    using log4net.Appender;
    using log4net.Repository.Hierarchy;
    using log4net.spi;

    using Newtonsoft.Json;

    using NUnit.Framework;

    using Sitecore.Logger.RabbitMQ.GelfAppender;

    [TestFixture]
    public class GelfRabbitMqAdapterIntegrationTests
    {
        private GelfRabbitMqAppender appender;
        private TestingRabbitListener testingRabbitListener;
        private ILog logger;

        [SetUp]
        public void SetUp()
        {
            this.appender = new GelfRabbitMqAppender
                           {
                               Threshold = Level.ERROR,
                               HostName = "localhost",
                               VirtualHost = "/",
                               Name = "GelfRabbitMQAdapter",
                               Port = 5672,
                               Exchange = "tests.log4net.gelf.appender",
                               Username = "guest",
                               Password = "guest",
                               Facility = "test-system"
                           };
            this.appender.ActivateOptions();

            var root = ((Hierarchy)LogManager.GetLoggerRepository()).Root;
            root.AddAppender(this.appender);
            root.Repository.Configured = true;
            this.logger = LogManager.GetLogger(this.GetType());

            this.testingRabbitListener = new TestingRabbitListener();
        }

        [TearDown]
        public void TearDown()
        {
            this.testingRabbitListener.Dispose();
            LogManager.Shutdown();
        }

        [Test]
        public void ShouldPublishGelfMessage_WhenLog4NetLogsEvent()
        {
            const string message = "should be published to rabbit";

            this.logger.Error(message);
            Thread.Sleep(200);

            Assert.That(this.testingRabbitListener.ReceivedMessages.Count, Is.EqualTo(1));
            var receivedMessage = this.testingRabbitListener.ReceivedMessages[0];
            var gelfMessage = JsonConvert.DeserializeObject<GelfMessage>(receivedMessage);
            Assert.That(gelfMessage.ShortMessage, Is.EqualTo(message));
            Assert.That(gelfMessage.Facility, Is.EqualTo("test-system"));
        } 
        
        [Test]
        public void ShouldNotPublishInfoLevelsWhenAppenderLogLevelIsError()
        {
            const string message = "should not be published to rabbit";

            this.logger.Info(message);
            Thread.Sleep(200);

            Assert.That(this.testingRabbitListener.ReceivedMessages.Count, Is.EqualTo(0));
        }
    }
}