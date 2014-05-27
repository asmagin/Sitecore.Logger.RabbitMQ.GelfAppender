namespace Sample.Console.Client
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using log4net;
    using log4net.Config;

    class Program
    {
        static void Main(string[] args)
        {
            DOMConfigurator.Configure();
            using (new ForeverLoggingClass())
            {
                Console.WriteLine("Will keep logging a message per second.");
                Console.ReadLine();
            }
            LogManager.Shutdown();
        }
    }

    public class ForeverLoggingClass : IDisposable
    {
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ForeverLoggingClass));
        private readonly CancellationTokenSource cancellationTokenSource;

        public ForeverLoggingClass()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(this.LogSomeStuff, this.cancellationTokenSource.Token);
        }

        private void LogSomeStuff()
        {
            int count = 0;
            while (!this.cancellationTokenSource.IsCancellationRequested)
            {
                Log.Info(string.Format("info message : {0}", ++count));
                Log.Error(new Exception(string.Format("some random exception {0}", ++count)));
                Thread.Sleep(new Random().Next(1, 10) * 1000);
            }
        }

        public void Dispose()
        {
            this.cancellationTokenSource.Cancel();
        }
    }
}
