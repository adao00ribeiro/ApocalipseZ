using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform Target;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Target)
        {
            ChasePlayer();
        }

    }

    private void ChasePlayer()
    {
        agent.stoppingDistance = 1.5f;
        agent.speed = 3;
        agent.SetDestination(Target.position);
    }
}
