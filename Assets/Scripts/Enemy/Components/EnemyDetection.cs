using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public float MinLaudiness = 5;
    public float MaxLaudiness = 20;
    public float Laudiness;
    private float CurrentLaudines;
    public LayerMask layer;
    public bool isProvoked;
    public bool IsDetection;
    public Transform target;
    private void Start ( )
    {
        isProvoked = true;
    }
   private void Update(){
    Detection();
   }
   void OnDisable()
    {
       IsDetection = false;
    }
    public void Detection ()
    {
        if ( target == null )
        {
            IsDetection = false;
        }
            if ( isProvoked )
            {
                CurrentLaudines += Time.deltaTime;
                if ( CurrentLaudines >= Laudiness )
                {
                    isProvoked = false;
                }
            }
            Collider[] collider  = Physics.OverlapSphere ( transform.position , CurrentLaudines , layer, QueryTriggerInteraction.UseGlobal );
            if ( collider.Length > 0 )
            {
                target = collider[0].gameObject.transform;
                IsDetection = true;
            }
        
       
    }
    public void ResetLaudiness ( )
    {
        Laudiness = MinLaudiness;
        CurrentLaudines = Laudiness;
    }
    public void SetIsProvoked (bool  _isProvoked )
    {
        isProvoked = _isProvoked;
        Laudiness += 5;
        if ( Laudiness > MaxLaudiness)
        {
            Laudiness = MaxLaudiness;
        }
    }
    private void OnDrawGizmos ( )
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere ( transform.position , CurrentLaudines );
    }
}
