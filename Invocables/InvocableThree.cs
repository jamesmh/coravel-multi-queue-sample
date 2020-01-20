using Coravel.Invocable;
using System;
using System.Threading.Tasks;

namespace coravelMultiQueueSample.Invocables
{
    public class InvocableThree : IInvocable
    {
        public InvocableThree()
        {
        }

        public Task Invoke()
        {
            Console.WriteLine("Invocable three");
            return Task.CompletedTask;
        }
    }
}
