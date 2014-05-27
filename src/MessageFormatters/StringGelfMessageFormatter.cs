namespace Sitecore.Logger.RabbitMQ.GelfAppender.MessageFormatters
{
    using log4net.Util;

    public class StringGelfMessageFormatter : IGelfMessageFormatter
    {
        private const int MaximumShortMessageLength = 250;

        public bool CanApply(object messageObject)
        {
            return (messageObject is string || messageObject is SystemStringFormat);
        }

        public void Format(GelfMessage gelfMessage, object messageObject)
        {
            string message = messageObject.ToString();
            if (message.Length > MaximumShortMessageLength)
            {
                gelfMessage.FullMessage = message;
                gelfMessage.ShortMessage = message.TruncateString(MaximumShortMessageLength);
            }
            else
            {
                gelfMessage.ShortMessage = message;
            }
        }
    }
}