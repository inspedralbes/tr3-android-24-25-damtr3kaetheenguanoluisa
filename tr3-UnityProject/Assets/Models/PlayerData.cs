using System.Collections.Generic;
namespace MyGame.Models
{
    [System.Serializable]
    public class PlayerData
    {
        public bool success;
        public string message;
        public PlayerInfo player;
    }

    [System.Serializable]
    public class PlayerInfo
    {
        public int id;
        public string username;
        public int bombs;
        public int bombsUsed;
        public int speed;
        public int victories;
        public int enemiesDefeated;

        public int bombAmount 
        { 
            get { return bombs; }
            set { bombs = value; }
        }
    }

    [System.Serializable]
    public class PlayersData 
    {
        public List<PlayerUpdateInfo> players;
    }

    [System.Serializable]
    public class PlayerUpdateInfo
    {
        public int id;
        public string username;
        public int bombAmount;
        public int bombsUsed;
        public int speed;
        public int victories;
        public int enemiesDefeated;
    }
}
