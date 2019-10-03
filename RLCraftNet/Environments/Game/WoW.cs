using System;

namespace Environments.Game
{
    public class WoW : BaseEnvironment
    {
        private const string WINDOW_NAME = "World of Warcraft";

        public WoW() : base(WINDOW_NAME)
        {
            if (WindowResolution == Resolution.None)
            {
                //throw new Exception();
            }

            if (WindowResolution == Resolution._2560_x_1440)
            {

            }
        }

        public override void Observe()
        {

        }

        public override void GetReward()
        {

        }

        public override void Act()
        {

        }
    }
}
