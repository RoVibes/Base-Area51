using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Area51Elevator
{
    class Program
    {
        /*Проект на Роберт Христов Манастирски*/
        static void Main(string[] args)
        {
            int agentsNum = 5;
            Random random = new Random();
            Base area51 = new Base();
            //Elevator elevator = new Elevator();
            List<Agent> agents = new List<Agent>();

            for (int i = 0; i < agentsNum; i++)
            {
                agents.Add(new Agent(random.Next(1,4)));
            }
            area51.getAgents(agents);
            area51.handleThread();


        
            Console.ReadKey();
        }
        /*Проект на Роберт Христов Манастирски*/
    }
}
