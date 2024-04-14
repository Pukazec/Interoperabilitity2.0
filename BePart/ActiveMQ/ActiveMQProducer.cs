using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace BePart.ActiveMQ
{
    public class ActiveMQProducer
    {
        private readonly string queueName;

        public ActiveMQProducer(string queue)
        {
            queueName = queue;
        }

        public void SendMessage(string messageText)
        {
            Uri connecturi = new Uri("activemq:tcp://localhost:61616");

            IConnectionFactory factory = new ConnectionFactory(connecturi);
            using (IConnection connection = factory.CreateConnection())
            using (Apache.NMS.ISession session = connection.CreateSession())
            {
                IDestination destination = session.GetQueue(queueName);
                using (IMessageProducer producer = session.CreateProducer(destination))
                {
                    connection.Start();
                    ITextMessage message = session.CreateTextMessage(messageText);
                    producer.Send(message);
                }
            }
        }
    }
}
