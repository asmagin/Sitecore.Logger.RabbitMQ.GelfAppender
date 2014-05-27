namespace Sitecore.Logger.RabbitMQ.GelfAppender
{
    using System;

    internal class UnknownFacility : IKnowAboutConfiguredFacility
    {
        public void UseToCall(Action<string> facilitySettingAction)
        {
        }
    }
}