using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Environments.Models
{
    public class GameState
    {
        public PartyState[] PartyStates { get; set; } = new PartyState[40];

        public class PartyState
        {
            public PlayerState[] PlayerStates { get; set; } = new PlayerState[5];
        }

        public class PlayerState
        {
            public Point[] Health { get; set; } = new Point[100];
            public Point[] Mana { get; set; } = new Point[100];
            public Point[] Cast { get; set; } = new Point[100];
        }
    }
}
