﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Random = UnityEngine.Random;
using FishNet.Object;

namespace ApocalipseZ
{
    [RequireComponent(typeof(NavMeshAgent))]

    public class Zombie : NetworkBehaviour
    {
        public event Action OnZombieIsDead;

        public EnemyMovimentType Type;

        private Animator animator;
        private NavMeshAgent agent;
        NavMeshPath path;

        IStats stats;
        //components
        EnemyPatrol Patrol;
        EnemyDetection Detection;
        EnemyAttack Attack;
        EnemyAnimation Animation;

        //target
        public Transform Target;
        public Vector3 TargetPosition;
        public AudioClip zombieRoar;

        private void Start()
        {
            if (base.IsServer)
            {
                stats = GetComponent<IStats>();
                Patrol = GetComponent<EnemyPatrol>();
                Detection = GetComponent<EnemyDetection>();
                Attack = GetComponent<EnemyAttack>();
                Animation = GetComponent<EnemyAnimation>();
                agent = GetComponent<NavMeshAgent>();
                path = new NavMeshPath();
                animator = GetComponent<Animator>();
                agent.angularSpeed = 999;
            }
            else
            {
                Destroy(GetComponent<EnemyPatrol>());
                Destroy(GetComponent<EnemyDetection>());

            }


        }

        private void FixedUpdate()
        {
            if (!base.IsServer)
            {
                return;
            }
            if (stats.IsDead())
            {
                OnZombieIsDead?.Invoke();
                GameController.Instance.TimerManager.Add(() =>
                {
                    base.Despawn();
                }, 10);

                Animation.SetType(Type = EnemyMovimentType.DIE);
                agent.speed = 0;
                Patrol.enabled = false;
                Detection.enabled = false;
                Attack.enabled = false;
                enabled = false;
                return;
            }
            if(agent.velocity.x ==0 && agent.velocity.z == 0 && !Attack.IsAttacking){
            Type = EnemyMovimentType.IDLE;
            }
           

            if (Target)
            {
                float distance = Vector3.Distance(transform.position, Target.position);
                if (distance > 30)
                {
                    Target = null;
                }

            }
            Detection.Detection(ref Target);
            if (!Detection.IsDetection && !Attack.IsAttacking)
            {
                Patrol.Patrol(agent);
                if (Patrol.IsWalk)
                {
                    Type = EnemyMovimentType.WALK;
                }
            }
            if (Detection.IsDetection && !Attack.IsAttacking)
            {
                ChasePlayer();
                Type = EnemyMovimentType.RUN;
            }
            Attack.Attack(ref Target);
            if (Attack.IsAttacking && Detection.IsDetection)
            {
                Type = EnemyMovimentType.ATACK;
            }
            Animation.SetType(Type);
        }
        private void ChasePlayer()
        {
            agent.stoppingDistance = 1.5f;
            agent.speed = 3;
            agent.SetDestination(Target.position);
        }

        private void FaceTarget()
        {
            // Vector3 direction = (target.position - transform.position).normalized;
            // Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            // transform.rotation = Quaternion.Slerp ( transform.rotation , lookRotation , Time.deltaTime * turnSpeed );
        }

    }
}