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
        public void getAgents(List<Agent> agents)       //Gets the list of agents and assigns them a random security level.
        {
            foreach (Agent a in agents)
            {
                Agents.Add(a);
                a.securityLevel = random.Next(1, 4);
            }
        }
        
        public void handleThread()  //Starts the threads for the Agents.
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
            arriveAtBase(agentThread); //Agents arrive at the base.
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
            agentThread.leftAtFloor = 1;        //Setting the property of the current agent to 1 when he arrives.
            
        }

        public void callElevator(Agent agentThread)     
        {
            string threadID = Thread.CurrentThread.Name;
            Console.WriteLine(threadID + " " + Actions.Call + "ed the elevator at floor: "+ Enum.GetName(typeof(FloorsEnum), agentThread.leftAtFloor - 1) +" Agent security level: " + Enum.GetName(typeof(SecurityLevels), agentThread.securityLevel - 1));
            Console.WriteLine(threadID + " waiting for the elevator..." );
            if (elevator.currentfloor == agentThread.leftAtFloor)   //Checking if the currentfloor of the Elevator is equal to the floor that the agent last left.
            {
                elevator.enterElevator(agentThread);        //if its true the agent enters the elevator.
            }
            else if (elevator.currentfloor > agentThread.leftAtFloor)   //if the currentfloor is higher than the floor that the agent last left
            {
                elevator.descendElevator(agentThread);      //Call the elevator to come to the floor at which the agent is now so he could get in.
            }
            else if (elevator.currentfloor < agentThread.leftAtFloor) //if the currentfloor is lower than the floor that the agent last left
            {
                elevator.ascendElevator(agentThread);       //Call the elevator to come to the floor at which the agent is now so he could get in.
            }

        }
        public void leaveBase(Agent agentThread)
        {
            mutexElevator.WaitOne();
            elevator.isAgentGoingHome = true;       //Set the bool to true because the agent desided to leave(I check this bool at the class Elevator in the method EnterElevator.)
            string threadID = Thread.CurrentThread.Name;
            Console.WriteLine(threadID + " decided to leave");
            if (agentThread.leftAtFloor != 1)       //If the agent that decided to leave is at a floor different than floor 1(G Floor) I call the callElevator Method so he can get into the elevator, descend to floor G so he can leave the base.
            {
                
                callElevator(agentThread);
                elevator.isAgentGoingHome = false;
            }
            elevator.isAgentGoingHome = false;      
            Console.WriteLine(threadID + " left"); //If the agent currently is at a floor 1(G Floor) he leaves.
            mutexElevator.ReleaseMutex();

        }

    }
}
