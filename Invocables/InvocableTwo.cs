using Coravel.Invocable;
using System;
using System.Threading.Tasks;

namespace coravelMultiQueueSample.Invocables
{
    public class InvocableTwo : IInvocable
    {
        public InvocableTwo()
        {
        }

        public Task Invoke()
        {
            Console.WriteLine("Invocable two");
            return Task.CompletedTask;
        }
    }
}
