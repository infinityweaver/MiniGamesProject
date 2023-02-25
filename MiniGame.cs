using System;
using System.Collections.Generic;
using System.Text;

namespace MiniGamesProject
{
    class MiniGame
    {
        public static int MINIGAME_ID_COUNTER = 0;

        public string Name { get; set; }
        public int MiniGameID { get; private set; }
        public DateTime DateCreated { get; private set; }

        /// <summary>
        /// Default MiniGame Constructor
        /// </summary>
        /// <param name="name">The name of the minigame object.</param>
        public MiniGame(string name)
        {
            this.Name = name;
            this.MiniGameID = MiniGame.MINIGAME_ID_COUNTER++;
            this.DateCreated = DateTime.Today;
        }
    }
}
