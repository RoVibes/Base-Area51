using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Area51Elevator
{
    public enum FloorsEnum
    {
        G, S, T1, T2
    };
    public enum SecurityLevels
    {
        Confidential, Secret, TopSecret
    }
    class Elevator
    {
        public bool isAgentGoingHome = false;
        public int currentfloor = 1;
        int selectedFloor = 0;
        int topFloor = 4;
        int floorSecurityLevel = 0;
        Random random = new Random();
        Floors floor = new Floors();
        
        public void descendElevator(Agent agentThread)
        {
            for (int i = currentfloor; i >= 1; i--)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Elevator is descending... Floor: " + Enum.GetName(typeof(FloorsEnum), i - 1));
                if (i == selectedFloor)
                {
                    currentfloor = i;
                    leaveElevator(agentThread);
                    break;
                }
            }
        }
        public void ascendElevator(Agent agentThread)
        {
            for (int i = currentfloor; i <= topFloor; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Elevator is ascending... Floor: " + Enum.GetName(typeof(FloorsEnum), i - 1));
                if (i == selectedFloor)
                {
                    currentfloor = i;
                    leaveElevator(agentThread);
                    break;
                }
            }
        }
        public int selectFloor()
        {
            selectedFloor = random.Next(1, 5);
            switch (selectedFloor)
            {
                case 1: floorSecurityLevel = 1; break;
                case 2: floorSecurityLevel = 2; break;
                case 3: floorSecurityLevel = 3; break;
                case 4: floorSecurityLevel = 3; break;
                default:
                    break;
            }
            return selectedFloor;
        }
        public void enterElevator(Agent agentThread)
        {
            string threadID = Thread.CurrentThread.Name;
            Console.WriteLine(threadID + " entered elevator. Current floor: " + Enum.GetName(typeof(FloorsEnum), currentfloor - 1));
            if (isAgentGoingHome)
            {
                selectedFloor = 1;
                isAgentGoingHome = false;
            }
            else
            {
                selectFloor();
            }
            controlElevator(agentThread);
            
        }
        public void controlElevator(Agent agentThread)
        {
            string threadID = Thread.CurrentThread.Name;
            if (selectedFloor == currentfloor)
            { 
                leaveElevator(agentThread);
            }
            else if (selectedFloor < currentfloor)
            {
                Console.WriteLine(threadID + " selected floor: " + Enum.GetName(typeof(FloorsEnum), selectedFloor - 1) + ". Agent security level: " + Enum.GetName(typeof(SecurityLevels), agentThread.securityLevel - 1));
                descendElevator(agentThread);
            }
            else if (selectedFloor > currentfloor)
            {
                Console.WriteLine(threadID + " selected floor: " + Enum.GetName(typeof(FloorsEnum), selectedFloor - 1) + ". Agent security level: " + Enum.GetName(typeof(SecurityLevels), agentThread.securityLevel - 1));
                ascendElevator(agentThread);
            }
            
        }
        public void accessFloor(Agent agentThread)
        {
            switch (selectedFloor)
            {
                case 1: floor.FloorG(agentThread); currentfloor = 1; break;
                case 2: floor.FloorS(agentThread); currentfloor = 2; break;
                case 3: floor.FloorT1(agentThread); currentfloor = 3; break;
                case 4: floor.FloorT2(agentThread); currentfloor = 4; break;
                default:
                    break;
            }
        }
        public void leaveElevator(Agent agentThread)
        {
            
            if (agentThread.securityLevel >= floorSecurityLevel)
            {
                accessFloor(agentThread);
            }
            else
            {
                string threadID = Thread.CurrentThread.Name;
                Console.WriteLine(threadID + " doesn't have permission to access this floor.. Agent security level: " + Enum.GetName(typeof(SecurityLevels), agentThread.securityLevel - 1));
                selectFloor();
                Console.WriteLine("Agent will select another floor. Selected floor: " + Enum.GetName(typeof(FloorsEnum), selectedFloor - 1));
                controlElevator(agentThread);
            }
        }

    }
}
