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
        public int speed;
        public int victories;
        public int enemiesDefeated;
    }
}
