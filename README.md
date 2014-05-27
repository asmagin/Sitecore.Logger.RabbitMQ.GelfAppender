RabbitMQ.GEFL.Appender for Sitecore
==============================

The RabbitMQ GELF appender is a customer appender for [Sitecore] (http://sitecore.net/) CMS which publishes log messages onto [RabbitMQ](http://www.rabbitmq.com/) message bus using the GELF (json) format.

## USAGE

### download src and compile :)

### change your app/web config file

sample config
```  
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <configSections>
    <section name="log4net" type="log4net.Config.Log4NetConfigurationSectionHandler, Sitecore.Logging" />
  </configSections>
  
  <log4net>
  	<appender name="rabbitmq" type="log4net.Appender.GelfRabbitMqAppender, Sitecore.Logger.RabbitMQ.GelfAppender">
      <HostName value="localhost" />
      <VirtualHost value="/" />
      <Port value="5672" />
      <Exchange value="log4net.gelf.appender" />
      <Username value="guest" />
      <Password value="guest" />
      <Facility value="SampleClient" />
    </appender>
   
    <root>
      <level value="ERROR" />
      <appender-ref ref="rabbitmq" />
    </root>
  </log4net>

</configuration>
```  

### Gelf Format 
https://github.com/Graylog2/graylog2-docs/wiki/GELF

## Use case

Sending your log messages onto a message bus (RabbitMQ) means they can be picked up easily and processed by a variety of consumers.

In particular it's handy with a log aggregator like [LogStash](http://logstash.net/). Just configure the [RabbitMQ input](http://logstash.net/docs/1.2.2/inputs/rabbitmq) and use [Kibana](http://www.elasticsearch.org/overview/kibana/) to search the [ElasticSearch](http://www.elasticsearch.org/overview/) database of your logs.

==============================
Initial version was created for log4net and could be found [here](https://github.com/hancengiz/rabbitmq.log4net.gelf.appender)