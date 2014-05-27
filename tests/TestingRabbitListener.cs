namespace Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class TestingRabbitListener : IDisposable
    {
        private IConnection connection;
        private IModel model;
        private const string queueName = "test.consumer";
        private const string exchangeName = "tests.log4net.gelf.appender";
        public List<string> ReceivedMessages { get; private set; }

        public TestingRabbitListener()
        {
            this.ReceivedMessages = new List<string>();
            this.SubscribeToExchange();
        }

        private void SubscribeToExchange()
        {
            var factory = new ConnectionFactory
                              {
                                  HostName = "localhost",
                                  UserName = "guest",
                                  Password = "guest",
                                  Protocol = Protocols.DefaultProtocol
                              };
            this.connection = factory.CreateConnection();
            this.model = this.connection.CreateModel();
            var consumerQueue = this.model.QueueDeclare(queueName, false, true, true, null);
            this.model.QueueBind(consumerQueue, exchangeName, "#");

            var consumer = new EventingBasicConsumer(this.model);
            consumer.Received += (o, e) =>
                                     {
                                         var data = Encoding.ASCII.GetString(e.Body);
                                         this.ReceivedMessages.Add(data);
                                         Console.WriteLine(data);
                                     };
            this.model.BasicConsume(consumerQueue, true, consumer);
        }

        public void Dispose()
        {
            this.model.QueueUnbind(queueName, exchangeName, "#", null);
            this.model.Dispose();
            this.connection.Dispose();
        }
    }
}