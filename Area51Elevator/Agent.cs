using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Area51Elevator
{
    class Agent
    {
        public int securityLevel { get; set; }
        public int leftAtFloor { get; set; }

        public Agent(int securityLevel)
        {
            this.securityLevel = securityLevel;
        }
    }
}
