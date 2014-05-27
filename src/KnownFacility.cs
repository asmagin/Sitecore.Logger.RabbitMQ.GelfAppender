namespace Sitecore.Logger.RabbitMQ.GelfAppender
{
    using System;

    public class KnownFacility : IKnowAboutConfiguredFacility
    {
        private readonly string facility;

        public KnownFacility(string facility)
        {
            this.facility = facility;
        }

        public void UseToCall(Action<string> facilitySettingAction)
        {
            facilitySettingAction(this.facility);
        }
    }
}