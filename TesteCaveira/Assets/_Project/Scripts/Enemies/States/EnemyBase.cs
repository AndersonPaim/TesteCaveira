using UnityEngine;
using UnityEngine.AI;
using Interfaces;
using System;
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

        protected WaypointController Waypoints;
        protected NavMeshAgent Agent;
        protected Animator Anim;
        protected StateMachine CurrentState;
        protected NavMeshPath Path;
        protected GameObject Player;
        protected float Health;
        protected bool IsDead = false;

        public virtual void SetupEnemy(WaypointController waypoints, GameObject player)
        {
            Waypoints = waypoints;
            Player = player;
            AudioController.SetupManager();
            Health = EnemyBalancer.health;
            IsDead = false;
        }

        public virtual void TakeDamage(float damage)
        {
            if(IsDead)
            {
                return;
            }

            if(Health <= 0)
            {
                IsDead = true;
                OnEnemyDie?.Invoke(gameObject, EnemyBalancer.killScore);
            }
        }

        public void KillEnemy()
        {
            IsDead = true;
            OnEnemyDie?.Invoke(gameObject, EnemyBalancer.killScore);
            EnemyDying dyingState = new EnemyDying(gameObject, Player, Agent, Mesh, Anim, EnemyBalancer, Waypoints);
            CurrentState.StateMachineNextState = dyingState;
            CurrentState.Stage = Events.EXIT;
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

