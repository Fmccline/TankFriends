using UnityEngine;

namespace Assets.Scripts.Factories
{
    public class TankManagerSpawner : MonoBehaviour
    {
        public int m_NumHumans;
        public int m_NumAI;
        public float m_SpawnRadius;
        public GameObject m_TankPrefab;

        static int MAX_TANKS = 8;
        static Color[] COLORS = { Color.blue, Color.red, Color.green, Color.magenta,
                                  Color.cyan, Color.black, new Color(0.5f, 0.5f, 0.5f), new Color(155f / 255f, 76f / 255f, 0f) };

        public TankManager[] SpawnTankManagers(IGameMode gameMode)
        {
            int totalPlayers = m_NumHumans + m_NumAI;
            int totalTanks = (totalPlayers > MAX_TANKS) ? MAX_TANKS : totalPlayers;
            TankManager[] tanks = new TankManager[totalTanks];
            for (int i = 0; i < tanks.Length; ++i)
            {
                float rotation = i * Mathf.PI * 2 / totalTanks;

                Vector2 direction = new Vector2(Mathf.Cos(rotation), Mathf.Sin(rotation)) * m_SpawnRadius;
                Vector3 spawnPosition = new Vector3(direction.y, 0f, direction.x);
                Quaternion spawnRotation = new Quaternion { eulerAngles = new Vector3(0f, Mathf.Rad2Deg * rotation, 0f) };
                tanks[i] = new TankManager()
                {
                    m_SpawnPosition = spawnPosition,
                    m_SpawnRotation = spawnRotation,
                    m_PlayerColor = COLORS[i],
                    m_Instance = Instantiate(m_TankPrefab, spawnPosition, spawnRotation) as GameObject,
                    m_PlayerNumber = i + 1,
                    m_InputType = GetTankInputType(m_NumHumans--)
                };
                tanks[i].Setup();
            }
            return tanks;
        }

        private TankInput.Type GetTankInputType(int numHumans)
        {
            if (numHumans > 0)
            {
                return TankInput.Type.Human;
            }
            else
            {
                return TankInput.Type.DumbAI;
            }
        }
    }
}
