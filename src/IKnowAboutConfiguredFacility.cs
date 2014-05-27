namespace Sitecore.Logger.RabbitMQ.GelfAppender
{
    using System;

    public interface IKnowAboutConfiguredFacility
    {
        void UseToCall(Action<string> facilitySettingAction);
    }
}