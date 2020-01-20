using Coravel.Invocable;
using System;
using System.Threading.Tasks;

namespace coravelMultiQueueSample.Invocables
{
    public class InvocableOne : IInvocable
    {
        public InvocableOne()
        {
        }

        public Task Invoke()
        {
            Console.WriteLine("Invocable one");
            return Task.CompletedTask;
        }
    }
}
