namespace Sitecore.Logger.RabbitMQ.GelfAppender
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public class GelfMessage : Dictionary<string, object>
    {
        public static GelfMessage EmptyGelfMessage
        {
            get
            {
                return new GelfMessage
                       {
                           Version = "1.0",
                           Host = Environment.MachineName,
                           File = "",
                           Line = ""
                       };
            }
        }

        public string Facility
        {
            get
            {
                return this.ValueAs<string>("facility");
            }
            set
            {
                this.SetValueAs(value, "facility");
            }
        }

        public string File
        {
            get
            {
                return this.ValueAs<string>("file");
            }
            set
            {
                this.SetValueAs(value, "file");
            }
        }

        public string FullMessage
        {
            get
            {
                return this.ValueAs<string>("full_message");
            }
            set
            {
                this.SetValueAs(value, "full_message");
            }
        }

        public string Host
        {
            get
            {
                return this.ValueAs<string>("host");
            }
            set
            {
                this.SetValueAs(value, "host");
            }
        }

        public long Level
        {
            get
            {
                return this.ValueAs<long>("level");
            }
            set
            {
                this.SetValueAs(value, "level");
            }
        }

        public string Line
        {
            get
            {
                return this.ValueAs<string>("line");
            }
            set
            {
                this.SetValueAs(value, "line");
            }
        }

        public string ShortMessage
        {
            get
            {
                return this.ValueAs<string>("short_message");
            }
            set
            {
                this.SetValueAs(value, "short_message");
            }
        }

        public DateTime Timestamp
        {
            get
            {
                if (!this.ContainsKey("timestamp"))
                {
                    return DateTime.MinValue;
                }

                object val = this["timestamp"];
                double value;
                bool parsed = Double.TryParse(val as string, NumberStyles.Any, CultureInfo.InvariantCulture, out value);
                return parsed ? value.FromUnixTimestamp() : DateTime.MinValue;
            }
            set
            {
                if (!this.ContainsKey("timestamp"))
                {
                    this.Add("timestamp", value.ToUnixTimestamp().ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    this["timestamp"] = value.ToUnixTimestamp().ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        public string Version
        {
            get
            {
                return this.ValueAs<string>("version");
            }
            set
            {
                this.SetValueAs(value, "version");
            }
        }

        private void SetValueAs(object value, string key)
        {
            if (!this.ContainsKey(key))
            {
                this.Add(key, value);
            }
            else
            {
                this[key] = value;
            }
        }

        private T ValueAs<T>(string key)
        {
            if (!this.ContainsKey(key))
            {
                return default(T);
            }

            return (T)this[key];
        }
    }
}