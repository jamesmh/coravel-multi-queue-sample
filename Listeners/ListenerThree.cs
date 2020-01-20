using System;
using System.Threading.Tasks;
using Coravel.Events.Interfaces;
using coravelMultiQueueSample.Events;

namespace coravelMultiQueueSample.Listeners
{
    public class ListenerThree : IListener<TestEvent>
    {
        public Task HandleAsync(TestEvent broadcasted)
        {
            Console.WriteLine("Listener three");
            return Task.CompletedTask;
        }
    }
}