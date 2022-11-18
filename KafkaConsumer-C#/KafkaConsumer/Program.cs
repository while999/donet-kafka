using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace KafkaConsumer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var consumer = new KafkaConsumer())
            {
                while (true)
                {
                    consumer.Consume(it =>
                    {
                        if (it != null)
                        {
                            Console.WriteLine($"Key:{it.Message.Key},Value:{it.Message.Value}");
                        }
                        else
                        {

                        }
                    });
                }
            }
        }

    }

    class KafkaConsumer : IDisposable
    {
        private IConsumer<string, string> _consumer;
        public KafkaConsumer(string server = null)
        {
            if (string.IsNullOrEmpty(server))
            {
                server = "127.0.0.1:9092";
            }
            var config = new ConsumerConfig
            {
                GroupId = "TestGroupone",
                BootstrapServers = server,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<string, string>(config).Build();
            //topic名称默认是test
            _consumer.Subscribe("test");

        }

        public void Consume(Action<ConsumeResult<string, string>> action = null)
        {
            var consumerResult = _consumer.Consume(TimeSpan.FromSeconds(2));
            action?.Invoke(consumerResult);
        }

        public void Dispose()
        {
            _consumer?.Dispose();
        }
    }
}
