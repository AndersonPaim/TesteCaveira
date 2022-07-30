using System.Collections.Generic;
using _Project.Scripts.Enemies;

namespace _Project.Scripts
{
    [System.Serializable]
    public class Wave
    {
        public List<EnemySpawner> Enemies;
        public int SpawnDelay;
    }
}