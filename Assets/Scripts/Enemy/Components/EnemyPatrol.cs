using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyPatrol : MonoBehaviour
{
    
    public float Radius;
    public Vector3 positionSpaw;
    public float DistanceSpaw;
    public bool walkPointSet;
    public Vector3 walkPoint;
    public float TimerReset;
    public float CurrentTimerReset;

    public bool IsWalk;
    private void Start ( )
    {
        Invoke ( "Init" , 1 );
    }
    public void Init ( )
    {
        positionSpaw = transform.position;
        walkPoint = positionSpaw;
        walkPointSet = false;
    }
    public void Patrol ( NavMeshAgent agent)
    {
        if ( !walkPointSet )
        {
            SearchWalkPoint ( );
        }

        if ( walkPointSet )
        {
            IsWalk = true;
            agent.stoppingDistance = 0.2f;
            agent.speed = 0.5f;
            agent.SetDestination ( walkPoint );
            
        }
        float distanceToWalkPoint = Vector3.Distance (transform.position , walkPoint);
       
        if ( distanceToWalkPoint <= agent.stoppingDistance )
        {
            CurrentTimerReset += Time.deltaTime;

            if (CurrentTimerReset > TimerReset)
            {
                walkPointSet = false;
                CurrentTimerReset = 0;
            }
            IsWalk = false;
        }
    }
    public void SearchWalkPoint ( )
    {
        walkPoint = RandomNavmeshLocation ( Radius );
        walkPoint.y = transform.position.y;

        TimerReset = Random.Range (0,10);
        Debug.DrawLine ( walkPoint , walkPoint + Vector3.up * 2 , Color.blue , 10 );
        walkPointSet = true;
    }
    public Vector3 RandomNavmeshLocation ( float radius )
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += positionSpaw;
        NavMeshHit hit;
        Vector3 finalPosition = walkPoint;
        if ( UnityEngine.AI.NavMesh.SamplePosition ( randomDirection , out hit , radius , 1 ) )
        {
            finalPosition = hit.position;
        }
       
        return finalPosition;
    }
    public bool ItsFarFrinSpawPoint ( )
    {
        return Vector3.Distance ( transform.position , positionSpaw ) > DistanceSpaw; 
    }
}
