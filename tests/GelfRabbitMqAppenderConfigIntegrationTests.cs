using System.Linq;
using System.Xml;
using log4net;
using NUnit.Framework;

namespace tests
{
    using System.Collections;
    using System.Reflection;

    using log4net.Appender;
    using log4net.Config;
    using log4net.Repository;
    using log4net.Repository.Hierarchy;
    using log4net.spi;

    using Sitecore.Logger.RabbitMQ.GelfAppender;

    [TestFixture]
    public class GelfRabbitMqAppenderConfigIntegrationTests
    {

        [Test]
        public void SetsAppenderPropertiesFromConfig()
        {
            var doc = CreateLog4NetXmlConfigurationDocument(facility: "test-system");

            DOMConfigurator.Configure(doc.DocumentElement);

            var appender = LogManager.GetLoggerRepository(Assembly.GetCallingAssembly()).GetAppenders().First();
            var gelfAppender = (GelfRabbitMqAppender) appender;

            Assert.That(gelfAppender, Is.Not.Null);
            Assert.That(gelfAppender.VirtualHost, Is.EqualTo("/"));
            Assert.That(gelfAppender.Facility, Is.EqualTo("test-system"));

            LogManager.Shutdown();
        }

        private static XmlDocument CreateLog4NetXmlConfigurationDocument(string facility)
        {
            const string appenderName = "rabbitmq.gelf.appender";

            var doc = new XmlDocument();
            var log4netElement = doc.AppendChild(doc.CreateElement("log4net"));
  
            var appenderElement = AddAppender(log4netElement, doc, appenderName);

            AddAppenderProprty(appenderElement, doc, "VirtualHost", "/");
            AddAppenderProprty(appenderElement, doc, "Facility", facility);

            AddRootLoggingElement(doc, appenderName, log4netElement);
            return doc;
        }

        private static XmlElement AddAppender(XmlNode log4netElement, XmlDocument doc, string appenderName)
        {
            var appenderElement = (XmlElement) log4netElement.AppendChild(doc.CreateElement("appender"));
            appenderElement.SetAttribute("name", appenderName);
            appenderElement.SetAttribute("type", typeof(GelfRabbitMqAppender).AssemblyQualifiedName);
            return appenderElement;
        }

        private static void AddRootLoggingElement(XmlDocument doc, string appenderName, XmlNode log4netElement)
        {
            var appenderRefElement = doc.CreateElement("appender-ref");
            appenderRefElement.SetAttribute("ref", appenderName);

            log4netElement.AppendChild(doc.CreateElement("root")).AppendChild(appenderRefElement);
        }

        private static void AddAppenderProprty(XmlElement appenderElement, XmlDocument doc, string propertyName, string propertyValue)
        {
            var vhostElement = (XmlElement) appenderElement.AppendChild(doc.CreateElement(propertyName));
            vhostElement.SetAttribute("value", propertyValue);
        }
    }

    public static class Extentions
    {
        public static IAppender[] GetAppenders(this ILoggerRepository repository)
        {
            var hierarchy = repository as Hierarchy;

            ArrayList appenderList = new ArrayList();
            CollectAppenders(appenderList, (IAppenderAttachable)hierarchy.Root);
            foreach (Logger logger in hierarchy.GetCurrentLoggers())
                CollectAppenders(appenderList, (IAppenderAttachable)logger);
            return (IAppender[])appenderList.ToArray(typeof(IAppender));
        }
        
        public static void CollectAppenders(ArrayList appenderList, IAppenderAttachable container)
        {
            foreach (IAppender appender in container.Appenders)
                CollectAppender(appenderList, appender);
        }

        public static void CollectAppender(ArrayList appenderList, IAppender appender)
        {
            if (appenderList.Contains((object)appender))
                return;
            appenderList.Add((object)appender);
            IAppenderAttachable container = appender as IAppenderAttachable;
            if (container != null)
                CollectAppenders(appenderList, container);
        }

    }
}