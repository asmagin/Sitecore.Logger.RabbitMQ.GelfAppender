namespace Sitecore.Logger.RabbitMQ.GelfAppender
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public class AmqpClientProperties
    {
        public static IDictionary<string, object> WithFacility(IKnowAboutConfiguredFacility facilityInformation)
        {
            IDictionary<string, object> properties = new Dictionary<string, object>();
            AssemblyName executingAssemblyName = Assembly.GetExecutingAssembly().GetName();
            properties.Add(Properties.Product, executingAssemblyName.Name);
            properties.Add(Properties.Version, executingAssemblyName.Version.ToString());
            properties.Add(Properties.Platform, Environment.OSVersion.Platform.ToString());
            facilityInformation.UseToCall(facility => properties.Add(Properties.Facility, facility));
            properties.Add(Properties.Host, Environment.MachineName);
            return properties;
        }

        public static class Properties
        {
            public static string Facility = "facility";

            public static string Host = "host";

            public static string Platform = "platform";

            public static string Product = "product";

            public static string Version = "version";
        }
    }
}