// ReSharper disable once CheckNamespace

namespace log4net.Appender
{
    using log4net.spi;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Framing.v0_9_1;

    using Sitecore.Logger.RabbitMQ.GelfAppender;

    public class GelfRabbitMqAppender : AppenderSkeleton
    {
        private readonly GelfAdapter gelfAdapter;

        private IConnection connection;

        private IKnowAboutConfiguredFacility facilityInformation = new UnknownFacility();

        private IModel model;

        public GelfRabbitMqAppender()
            : this(new GelfAdapter())
        {
        }

        public GelfRabbitMqAppender(GelfAdapter gelfAdapter)
        {
            this.gelfAdapter = gelfAdapter;
            this.SetDefaultConfig();
        }

        public string Exchange { get; set; }

        public string Facility { get; set; }

        public string HostName { get; set; }

        public string Password { get; set; }

        public int Port { get; set; }

        public string Username { get; set; }

        public string VirtualHost { get; set; }

        public override void ActivateOptions()
        {
            base.ActivateOptions();

            if (!string.IsNullOrWhiteSpace(this.Facility))
            {
                this.facilityInformation = new KnownFacility(this.Facility);
                this.gelfAdapter.Facility = this.Facility;
            }

            this.OpenConnection();
        }

        public override void OnClose()
        {
            base.OnClose();
            this.SafeShutdownForConnection();
            this.SafeShutDownForModel();
        }

        public void EnsureConnectionIsOpen()
        {
            if (this.model != null)
            {
                return;
            }
            this.OpenConnection();
        }

        protected virtual ConnectionFactory CreateConnectionFactory()
        {
            return new ConnectionFactory
                   {
                       Protocol = Protocols.FromEnvironment(),
                       HostName = this.HostName,
                       Port = this.Port,
                       VirtualHost = this.VirtualHost,
                       UserName = this.Username,
                       Password = this.Password,
                       ClientProperties = AmqpClientProperties.WithFacility(this.facilityInformation)
                   };
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            this.EnsureConnectionIsOpen();
            this.model.ExchangeDeclare(this.Exchange, ExchangeType.Topic);
            var messageBody = this.gelfAdapter.Adapt(loggingEvent).AsJson();
            this.model.BasicPublish(this.Exchange, "log4net.gelf.appender", true, null, messageBody.AsByteArray());
        }

        private void ConnectionShutdown(IConnection shutingDownConnection, ShutdownEventArgs reason)
        {
            this.SafeShutdownForConnection();
            this.SafeShutDownForModel();
        }

        private void OpenConnection()
        {
            this.connection = this.CreateConnectionFactory().CreateConnection();
            this.connection.ConnectionShutdown += this.ConnectionShutdown;
            this.model = this.connection.CreateModel();
            this.model.ExchangeDeclare(this.Exchange, ExchangeType.Topic);
        }

        private void SafeShutDownForModel()
        {
            if (this.model == null)
            {
                return;
            }
            this.model.Close(Constants.ReplySuccess, "gelf rabbit appender shutting down!");
            this.model.Dispose();
            this.model = null;
        }

        private void SafeShutdownForConnection()
        {
            if (this.connection == null)
            {
                return;
            }
            this.connection.ConnectionShutdown -= this.ConnectionShutdown;
            this.connection.AutoClose = true;
            this.connection = null;
        }

        private void SetDefaultConfig()
        {
            this.HostName = "localhost";
            this.Port = 5672;
            this.VirtualHost = "/";
            this.Exchange = "log4net.gelf.appender";
            this.Username = "guest";
            this.Password = "guest";
        }
    }
}