// ReSharper disable once CheckNamespace
namespace log4net.Util
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    public class LogReceivedEventArgs : EventArgs
    {
        private readonly LogLog loglog;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loglog"></param>
        public LogReceivedEventArgs(LogLog loglog)
        {
            this.loglog = loglog;
        }

        /// <summary>
        /// 
        /// </summary>
        public LogLog LogLog
        {
            get { return this.loglog; }
        }
    }
}