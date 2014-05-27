namespace Sitecore.Logger.RabbitMQ.GelfAppender
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using log4net.spi;

    using Sitecore.Logger.RabbitMQ.GelfAppender.MessageFormatters;
    using Sitecore.Logging;

    public class GelfAdapter
    {
        private static readonly ExceptionMessageFormatter ExceptionMessageFormatter = new ExceptionMessageFormatter();

        private readonly GelfLogLevelMapper gelfLogLevelMapper;

        private readonly IList<IGelfMessageFormatter> messageObjectFormatters;

        public GelfAdapter()
            : this(new GelfLogLevelMapper())
        {
        }

        public GelfAdapter(GelfLogLevelMapper gelfLogLevelMapper)
            : this(gelfLogLevelMapper,
                new List<IGelfMessageFormatter>
                {
                    new StringGelfMessageFormatter(),
                    ExceptionMessageFormatter,
                    new DictionaryGelfMessageFormatter(),
                    new GenericObjectGelfMessageFormatter(),
                })
        {
        }

        public GelfAdapter(GelfLogLevelMapper gelfLogLevelMapper, IList<IGelfMessageFormatter> messageObjectFormatters)
        {
            this.gelfLogLevelMapper = gelfLogLevelMapper;
            this.messageObjectFormatters = messageObjectFormatters;
        }

        public string Facility { private get; set; }

        public GelfMessage Adapt(LoggingEvent loggingEvent)
        {
            GelfMessage gelfMessage = GelfMessage.EmptyGelfMessage;
            gelfMessage.Level = this.gelfLogLevelMapper.Map(loggingEvent.Level);
            gelfMessage.Timestamp = loggingEvent.TimeStamp;
            if (!string.IsNullOrWhiteSpace(this.Facility))
            {
                gelfMessage.Facility = this.Facility;
            }
            gelfMessage["_LoggerName"] = loggingEvent.LoggerName;
            gelfMessage["_LoggerLevel"] = loggingEvent.Level.ToString();
            gelfMessage["_ProcessName"] = Process.GetCurrentProcess().ProcessName;
            gelfMessage["_ThreadName"] = loggingEvent.ThreadName;
            gelfMessage["_Domain"] = loggingEvent.Domain;

            this.AddLocationInfo(loggingEvent, gelfMessage);
            this.FormatGelfMessage(gelfMessage, loggingEvent);
            return gelfMessage;
        }

        private void AddLocationInfo(LoggingEvent loggingEvent, GelfMessage gelfMessage)
        {
            if (loggingEvent.LocationInformation == null)
            {
                return;
            }
            gelfMessage.File = loggingEvent.LocationInformation.FileName;
            gelfMessage.Line = loggingEvent.LocationInformation.LineNumber;
        }

        private void AppendExceptionInformationIfExists(GelfMessage gelfMessage, Exception exceptionObject)
        {
            if (exceptionObject != null)
            {
                ExceptionMessageFormatter.Format(gelfMessage, exceptionObject);
            }
        }

        private void FormatGelfMessage(GelfMessage gelfMessage, LoggingEvent loggingEvent)
        {
            IGelfMessageFormatter messageFormatter = this.messageObjectFormatters.First(x => x.CanApply(loggingEvent.MessageObject));
            messageFormatter.Format(gelfMessage, loggingEvent.MessageObject);
            this.AppendExceptionInformationIfExists(gelfMessage, loggingEvent.ExceptionObject());

            if (string.IsNullOrWhiteSpace(gelfMessage.ShortMessage))
            {
                gelfMessage.ShortMessage = "Logged object of type: " + loggingEvent.MessageObject.GetType().FullName;
            }
        }
    }
}