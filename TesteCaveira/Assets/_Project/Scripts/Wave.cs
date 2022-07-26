using System.Collections.Generic;

namespace Managers.Spawner
{
    [System.Serializable]
    public class Wave
    {
        public List<EnemySpawn> Enemies;
        public int SpawnDelay;
    }
}