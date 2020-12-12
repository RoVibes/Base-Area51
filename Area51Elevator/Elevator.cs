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
        bool isElevatorEmpty = true;
        Random random = new Random();
        Floors floor = new Floors();

        public void enterElevator(Agent agentThread)
        {
            string threadID = Thread.CurrentThread.Name;
            Console.WriteLine(threadID + " entered elevator. Current floor: " + Enum.GetName(typeof(FloorsEnum), currentfloor - 1));   
            if (isAgentGoingHome)   //I check if the agent decided to leave.
            {
                selectedFloor = 1;  //If he decided to leave I assign the selected floor to 1(G Floor) because thats the only floor that the Agent is able to leave.
                isAgentGoingHome = false;   //Setting the bool isAgentGoingHome to false because the next agent might not want to leave.
            }
            else
            {
                selectFloor();  //If the agent doesn't want to leave he selects a floor.
            }
            isElevatorEmpty = false;
            controlElevator(agentThread); //After that I call Control elevator which handles if the elevator will descend or ascend.
            
        }
        public int selectFloor()
        {
            selectedFloor = random.Next(1, 5);
            switch (selectedFloor)          //Set the floor security level according to the randomly selected floor.
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

        public void controlElevator(Agent agentThread)
        {
            string threadID = Thread.CurrentThread.Name;
            if (selectedFloor == currentfloor)      //If the selected floor is the current floor of the elevator I call leave elevator.
            { 
                leaveElevator(agentThread);
            }
            else if (selectedFloor < currentfloor)      //If the selected floor is lower than the current floor of the elevator we descend it.
            {
                Console.WriteLine(threadID + " selected floor: " + Enum.GetName(typeof(FloorsEnum), selectedFloor - 1) + ". Agent security level: " + Enum.GetName(typeof(SecurityLevels), agentThread.securityLevel - 1));
                descendElevator(agentThread);
            }
            else if (selectedFloor > currentfloor) //If the selected floor is higher than the current floor of the elevator we ascend it.
            {
                Console.WriteLine(threadID + " selected floor: " + Enum.GetName(typeof(FloorsEnum), selectedFloor - 1) + ". Agent security level: " + Enum.GetName(typeof(SecurityLevels), agentThread.securityLevel - 1));
                ascendElevator(agentThread);
            }
            
        }
        public void descendElevator(Agent agentThread)  
        {
            if (isElevatorEmpty)        //Check if the elevator has an Agent in it. If it doesn't descend the elevator to the floor that the agent currently is so the agent can get in.
            {
                for (int i = currentfloor; i >= 1; i--)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Elevator is descending... Floor: " + Enum.GetName(typeof(FloorsEnum), i - 1));
                    if (i == agentThread.leftAtFloor)       //When the elevator arrives at the floor that the current agent is I call EnterElevator method and the Agent gets in the elevator.
                    {
                        currentfloor = i;
                        enterElevator(agentThread);     
                        break;
                    }
                }
            }
            else                       //If theres an agent in the elevator descend the elevator to the floor that the agent selected..
            {
                for (int i = currentfloor; i >= 1; i--)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Elevator is descending... Floor: " + Enum.GetName(typeof(FloorsEnum), i - 1));
                    if (i == selectedFloor) //When the elevator reaches the selected floor I call leaveElevator so the agent could go in the floor.
                    {
                        currentfloor = i;
                        leaveElevator(agentThread);
                        break;
                    }
                }
            }
            
        }
        public void ascendElevator(Agent agentThread) 
        {
            if (isElevatorEmpty)    //Check if the elevator has an Agent in it. If it doesn't descend the elevator so the agent can get in.
            {
                for (int i = currentfloor; i <= 4; i++)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Elevator is ascending... Floor: " + Enum.GetName(typeof(FloorsEnum), i - 1));
                    if (i == agentThread.leftAtFloor) //When the elevator arrives at the floor that the current agent is I call EnterElevator method and the Agent gets in the elevator.
                    {
                        currentfloor = i;
                        enterElevator(agentThread);
                        break;
                    }
                }
            }
            else                //If theres an agent in the elevator ascend the elevator to the selected floor.
            {
                for (int i = currentfloor; i <= topFloor; i++)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Elevator is ascending... Floor: " + Enum.GetName(typeof(FloorsEnum), i - 1));
                    if (i == selectedFloor) //When the elevator reaches the selected floor I call leaveElevator so the agent could go in the floor.
                    {
                        currentfloor = i;
                        leaveElevator(agentThread);
                        break;
                    }
                }
            }
            
        }
        public void leaveElevator(Agent agentThread)
        {

            if (agentThread.securityLevel >= floorSecurityLevel)         //I check if the security level of the agent is higher than the security level of the floor.
            {
                accessFloor(agentThread);   //If its higher I call access floor so the agent can get into the selected floor.
                isElevatorEmpty = true;
            }
            else    //If the security level of the agent is lower then the security level of the floor, he doesn't have permissions to access the floor he selected.
            {   
                string threadID = Thread.CurrentThread.Name;
                Console.WriteLine(threadID + " doesn't have permission to access this floor.. Agent security level: " + Enum.GetName(typeof(SecurityLevels), agentThread.securityLevel - 1));
                selectFloor();  //The agent selects another floor because he doesn't have permissions to access the currently selected one.
                Console.WriteLine("Agent will select another floor. Selected floor: " + Enum.GetName(typeof(FloorsEnum), selectedFloor - 1));
                controlElevator(agentThread);   //After he selects another floor I call control elevator so the elevator goes to the desired floor and I check again if the agent has permissions to enter the selected floor.
            }
        }
        public void accessFloor(Agent agentThread)
        {
            switch (selectedFloor)  //I switch the selected floor and call the according floor method from the class Floors.
            {
                case 1: floor.FloorG(agentThread); currentfloor = 1; break;
                case 2: floor.FloorS(agentThread); currentfloor = 2; break;
                case 3: floor.FloorT1(agentThread); currentfloor = 3; break;
                case 4: floor.FloorT2(agentThread); currentfloor = 4; break;
                default:
                    break;
            }
        }
       

    }
}
