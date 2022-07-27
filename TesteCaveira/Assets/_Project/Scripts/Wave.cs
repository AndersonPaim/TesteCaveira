using System.Collections.Generic;

namespace Managers.Spawner
{
    [System.Serializable]
    public class Wave
    {
        public List<EnemySpawner> Enemies;
        public int SpawnDelay;
    }
}