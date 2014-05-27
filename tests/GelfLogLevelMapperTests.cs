namespace Tests
{
    using log4net.Appender;
    using log4net.spi;

    using NUnit.Framework;

    using Sitecore.Logger.RabbitMQ.GelfAppender;

    [TestFixture]
    public class GelfLogLevelMapperTests
    {
        [Test]
        public void ShouldMapLog4NetLevelToCorrectLogLevel()
        {
            var logLevelMapper = new GelfLogLevelMapper();

            Assert.That(logLevelMapper.Map(Level.ALERT), Is.EqualTo((long)LocalSyslogAppender.SyslogSeverity.Alert));

            Assert.That(logLevelMapper.Map(Level.FATAL ), Is.EqualTo((long)LocalSyslogAppender.SyslogSeverity.Critical));
            Assert.That(logLevelMapper.Map(Level.CRITICAL), Is.EqualTo((long)LocalSyslogAppender.SyslogSeverity.Critical));

            Assert.That(logLevelMapper.Map(Level.DEBUG), Is.EqualTo((long)LocalSyslogAppender.SyslogSeverity.Debug));
            Assert.That(logLevelMapper.Map(null), Is.EqualTo((long)LocalSyslogAppender.SyslogSeverity.Debug));

            Assert.That(logLevelMapper.Map(Level.EMERGENCY), Is.EqualTo((long)LocalSyslogAppender.SyslogSeverity.Emergency));
            Assert.That(logLevelMapper.Map(Level.SEVERE), Is.EqualTo((long)LocalSyslogAppender.SyslogSeverity.Emergency));

            Assert.That(logLevelMapper.Map(Level.ERROR), Is.EqualTo((long)LocalSyslogAppender.SyslogSeverity.Error));

            Assert.That(logLevelMapper.Map(Level.FINE), Is.EqualTo((long)LocalSyslogAppender.SyslogSeverity.Informational));
            Assert.That(logLevelMapper.Map(Level.FINER), Is.EqualTo((long)LocalSyslogAppender.SyslogSeverity.Informational));
            Assert.That(logLevelMapper.Map(Level.FINEST), Is.EqualTo((long)LocalSyslogAppender.SyslogSeverity.Informational));
            Assert.That(logLevelMapper.Map(Level.INFO), Is.EqualTo((long)LocalSyslogAppender.SyslogSeverity.Informational));
            Assert.That(logLevelMapper.Map(Level.OFF), Is.EqualTo((long)LocalSyslogAppender.SyslogSeverity.Informational));

            Assert.That(logLevelMapper.Map(Level.NOTICE), Is.EqualTo((long)LocalSyslogAppender.SyslogSeverity.Notice));
            Assert.That(logLevelMapper.Map(Level.VERBOSE), Is.EqualTo((long)LocalSyslogAppender.SyslogSeverity.Notice));
            Assert.That(logLevelMapper.Map(Level.TRACE), Is.EqualTo((long)LocalSyslogAppender.SyslogSeverity.Notice));

            Assert.That(logLevelMapper.Map(Level.WARN), Is.EqualTo((long)LocalSyslogAppender.SyslogSeverity.Warning));
        }
    }
}