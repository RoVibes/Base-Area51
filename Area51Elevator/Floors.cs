using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Area51Elevator
{
    
    class Floors
    {
        public void FloorG(Agent agentThread)
        {
            string threadID = Thread.CurrentThread.Name;
            Console.WriteLine(threadID + " entered floor G");
            agentThread.leftAtFloor = 1;
        }
        public void FloorS(Agent agentThread)
        {
            string threadID = Thread.CurrentThread.Name;
            Console.WriteLine(threadID + " entered floor S");
            agentThread.leftAtFloor = 2;
        }
        public void FloorT1(Agent agentThread)
        {
            string threadID = Thread.CurrentThread.Name;
            Console.WriteLine(threadID + " entered floor T1");
            agentThread.leftAtFloor = 3;
        }
        public void FloorT2(Agent agentThread)
        {
            string threadID = Thread.CurrentThread.Name;
            Console.WriteLine(threadID + " entered floor T2");
            agentThread.leftAtFloor = 4;
        }

    }
}
