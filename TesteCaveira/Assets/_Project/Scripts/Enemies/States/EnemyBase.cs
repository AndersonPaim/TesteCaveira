using UnityEngine;
using UnityEngine.AI;
using Interfaces;
using Managers;

namespace Enemy
{
    public class EnemyBase : MonoBehaviour, IDamageable
    {
        public delegate void EnemyHandler(GameObject enemy, int score);
        public EnemyHandler OnEnemyDie;

        [SerializeField] protected EnemyBalancer EnemyBalancer;
        [SerializeField] protected SkinnedMeshRenderer Mesh;
        [SerializeField] protected EnemyAudioController AudioController;

        protected GameManager Manager;
        protected NavMeshAgent Agent;
        protected Animator Anim;
        protected StateMachine CurrentState;
        protected NavMeshPath Path;
        protected GameObject Player;
        protected float Health;
        protected bool IsDead = false;

        public virtual void SetupEnemy(GameManager manager)
        {
            Manager = manager;
            Player = manager.PlayerController.gameObject;
            AudioController.SetupManager(manager.AudioManager);
            CurrentState = new EnemySpawn(gameObject, Player, Agent, Mesh, Anim, Path, EnemyBalancer, manager);
            Health = EnemyBalancer.health;
            IsDead = false;
        }

        public virtual void TakeDamage(float damage)
        {
            if(IsDead)
            {
                return;
            }

            Health -= damage;

            if(Health > 0)
            {
                CurrentState.TakeDamage(Enemies.ARCHER);
            }
            else
            {
                IsDead = true;
                OnEnemyDie?.Invoke(gameObject, EnemyBalancer.killScore);
                CurrentState.Death();
            }
        }

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            CurrentState = CurrentState.Process();
        }

        protected virtual void Initialize()
        {
            Anim = GetComponent<Animator>();
            Agent = GetComponent<NavMeshAgent>();
            Path = new NavMeshPath();
        }
    }
}
