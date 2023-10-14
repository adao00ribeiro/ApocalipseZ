using System.Collections;
using System.Collections.Generic;
using ApocalipseZ;
using UnityEngine;

public class PointFlagTeam : MonoBehaviour
{
    public PVPFLAGManager pVPFLAGManager;

    void Start(){
        pVPFLAGManager = GameObject.FindAnyObjectByType<PVPFLAGManager>();
    }
     private void OnTriggerEnter(Collider other)
    {
      FpsPlayer player =   other.GetComponent<FpsPlayer>();
       if(!player){
        return;
        }
        if(player.gameObject.tag == gameObject.tag){
           Flag flag = player.GetFlag();

           if(flag){
            if(gameObject.tag== "TeamA"){
              pVPFLAGManager.IncrementFlagTeamA();
            }else{
            pVPFLAGManager.IncrementFlagTeamB();
            }
            player.DropFlag();
            flag.ReturnLocalPosition();
          
           }
        }
    }
}
