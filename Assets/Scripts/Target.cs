using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ {
    public class Target : MonoBehaviour
    {
        private PlayerStats playerstats;
        // Start is called before the first frame update
        void Start ( )
        {
            playerstats = GetComponent<PlayerStats> ( );
        }

        // Update is called once per frame
        void Update ( )
        {
            if ( playerstats ==null)
            {
                return;
            }
            if ( playerstats .health <= 0 )
            {
                playerstats.health = 100;
            }
        }
    }

}