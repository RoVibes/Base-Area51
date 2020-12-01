using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Area51Elevator
{
    public enum Actions { Arrive, Call, Leave };
    class Base
    {
        Elevator elevator = new Elevator();
        Mutex mutexElevator = new Mutex();
        Mutex leaveBaseMutex = new Mutex();
        List<Agent> Agents = new List<Agent>();
        Random random = new Random();
        int agentcount = 0;
        public void getAgents(List<Agent> agents)
        {
            foreach (Agent a in agents)
            {
                Agents.Add(a);
                a.securityLevel = random.Next(1, 4);
            }
        }
        
        public void handleThread()
        {
            foreach (Agent a in Agents)
            {
                ThreadStart agentTS = delegate { getRandomAction(a); };
                Thread agent = new Thread(agentTS);
                agentcount++;
                agent.Name = "Agent " + agentcount;
                agent.Start();
            }
        }
        public void getRandomAction(Agent agentThread)
        {
            arriveAtBase(agentThread);
            while (true)
            {
                int randomAction = random.Next(1,3);
                
                switch (randomAction)
                {
                    case 1:
                        mutexElevator.WaitOne();
                        callElevator(agentThread);
                        mutexElevator.ReleaseMutex();
                        break;
                    case 2:
                        leaveBaseMutex.WaitOne();
                        leaveBase(agentThread);
                        leaveBaseMutex.ReleaseMutex();
                        return;
                    default:
                        break;
                }
            }

        }
        public void arriveAtBase(Agent agentThread)
        {
            string threadID = Thread.CurrentThread.Name;
            Console.WriteLine(threadID + " " + Actions.Arrive + "d at the base");
            agentThread.leftAtFloor = 1;
            
        }

        public void callElevator(Agent agentThread)
        {
            string threadID = Thread.CurrentThread.Name;
            Console.WriteLine(threadID + " " + Actions.Call + "ed the elevator at floor: "+ Enum.GetName(typeof(FloorsEnum), agentThread.leftAtFloor - 1) +" Agent security level: " + Enum.GetName(typeof(SecurityLevels), agentThread.securityLevel - 1));
            Console.WriteLine(threadID + " waiting for the elevator..." );
            if (elevator.currentfloor == agentThread.leftAtFloor)
            {
                elevator.enterElevator(agentThread);
            }
            else if (elevator.currentfloor > agentThread.leftAtFloor)
            {
                descendElevator(agentThread);
            }
            else if (elevator.currentfloor < agentThread.leftAtFloor)
            {
                ascendElevator(agentThread);
            }

        }
        public void leaveBase(Agent agentThread)
        {
            mutexElevator.WaitOne();
            elevator.isAgentGoingHome = true;
            string threadID = Thread.CurrentThread.Name;
            Console.WriteLine(threadID + " decided to leave");
            if (agentThread.leftAtFloor != 1)
            {
                
                callElevator(agentThread);
                elevator.isAgentGoingHome = false;
            }
            elevator.isAgentGoingHome = false;
            Console.WriteLine(threadID + " left");
            mutexElevator.ReleaseMutex();

        }

        public void descendElevator(Agent agentThread)
        {
            for (int i = elevator.currentfloor; i >= 1; i--)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Elevator is descending... Floor: " + Enum.GetName(typeof(FloorsEnum), i - 1));
                if (i == agentThread.leftAtFloor)
                {
                    elevator.currentfloor = i;
                    elevator.enterElevator(agentThread);
                    break;
                }
            }
        }
        public void ascendElevator(Agent agentThread)
        {
            for (int i = elevator.currentfloor; i <= 4; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Elevator is ascending... Floor: " + Enum.GetName(typeof(FloorsEnum), i - 1));
                if (i == agentThread.leftAtFloor)
                {
                    elevator.currentfloor = i;
                    elevator.enterElevator(agentThread);
                    break;
                }
            }
        }


    }
}
