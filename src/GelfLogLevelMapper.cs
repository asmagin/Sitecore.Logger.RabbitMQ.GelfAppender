namespace Sitecore.Logger.RabbitMQ.GelfAppender
{
    using System.Collections.Generic;

    using log4net.Appender;
    using log4net.spi;

    public class GelfLogLevelMapper
    {
        private readonly Dictionary<string, long> levelMappings;

        public GelfLogLevelMapper()
        {
            this.levelMappings = new Dictionary<string, long>
                                 {
                                     { Level.ALERT.Name, (long)LocalSyslogAppender.SyslogSeverity.Alert },
                                     { Level.CRITICAL.Name, (long)LocalSyslogAppender.SyslogSeverity.Critical },
                                     { Level.FATAL.Name, (long)LocalSyslogAppender.SyslogSeverity.Critical },
                                     { Level.DEBUG.Name, (long)LocalSyslogAppender.SyslogSeverity.Debug },
                                     { Level.EMERGENCY.Name, (long)LocalSyslogAppender.SyslogSeverity.Emergency },
                                     { Level.ERROR.Name, (long)LocalSyslogAppender.SyslogSeverity.Error },
                                     { Level.FINE.Name, (long)LocalSyslogAppender.SyslogSeverity.Informational },
                                     { Level.FINER.Name, (long)LocalSyslogAppender.SyslogSeverity.Informational },
                                     { Level.FINEST.Name, (long)LocalSyslogAppender.SyslogSeverity.Informational },
                                     { Level.INFO.Name, (long)LocalSyslogAppender.SyslogSeverity.Informational },
                                     { Level.OFF.Name, (long)LocalSyslogAppender.SyslogSeverity.Informational },
                                     { Level.NOTICE.Name, (long)LocalSyslogAppender.SyslogSeverity.Notice },
                                     { Level.VERBOSE.Name, (long)LocalSyslogAppender.SyslogSeverity.Notice },
                                     { Level.TRACE.Name, (long)LocalSyslogAppender.SyslogSeverity.Notice },
                                     { Level.SEVERE.Name, (long)LocalSyslogAppender.SyslogSeverity.Emergency },
                                     { Level.WARN.Name, (long)LocalSyslogAppender.SyslogSeverity.Warning }
                                 };
        }

        public virtual long Map(Level log4NetLevel)
        {
            if (log4NetLevel == null)
            {
                return (long)LocalSyslogAppender.SyslogSeverity.Debug;
            }
            var mappedValue = (long)LocalSyslogAppender.SyslogSeverity.Debug;
            this.levelMappings.TryGetValue(log4NetLevel.Name, out mappedValue);
            return mappedValue;
        }
    }
}