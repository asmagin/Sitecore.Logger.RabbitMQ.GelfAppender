namespace Sitecore.Logger.RabbitMQ.GelfAppender.MessageFormatters
{
    public interface IGelfMessageFormatter
    {
        bool CanApply(object messageObject);

        void Format(GelfMessage gelfMessage, object messageObject);
    }
}