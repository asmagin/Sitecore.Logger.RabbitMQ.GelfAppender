// ReSharper disable once CheckNamespace
namespace Sitecore.Logging
{
    using System;
    using System.Reflection;

    using global::log4net.spi;

    internal static class LoggingEventExtention
    {
        public static Exception ExceptionObject(this LoggingEvent @event)
        {
            if (@event != null)
            {
                var field = typeof(LoggingEvent).GetField("m_thrownException", BindingFlags.NonPublic | BindingFlags.Instance);

                if (field != null)
                {
                    return field.GetValue(@event) as Exception;
                }
            }
            return null;
        }
    }
}