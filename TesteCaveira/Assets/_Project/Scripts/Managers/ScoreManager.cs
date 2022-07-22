using System;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public Action<int> OnUpdateScore;

        private int _score = 0;
        private int _kills = 0;

        public void KillEnemyScore(int score)
        {
            _kills++;
            _score += score;
            OnUpdateScore?.Invoke(_score);
        }

        public int GetScore()
        {
            return _score;
        }

        public int GetKills()
        {
            return _kills;
        }
    }
}