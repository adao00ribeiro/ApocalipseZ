using System.Collections;
using System.Collections.Generic;
using FishNet.Managing;
using UnityEngine;

namespace ApocalipseZ
{
    public class RegistrarPrefabSpaws : MonoBehaviour
    {
        public NetworkManager manager;
        // Start is called before the first frame update
        void Start()
        {
            /*
            ScriptableItem[] weapons =  ScriptableManager.Instance.GetItemsWeapons ( );
            for ( int i = 0 ; i < weapons.Length ; i++ )
            {
                manager.spawnPrefabs.Add ( weapons[i].sitem.Prefab );
                print ( "REGISTRADO weapons" );
            }
            ScriptableItem[] Consumable =  ScriptableManager.Instance.GetItemsConsumable ( );
            for ( int i = 0 ; i < Consumable.Length ; i++ )
            {
                manager.spawnPrefabs.Add ( Consumable[i].sitem.Prefab );
                print ( "REGISTRADO CONSUMABLE" );
            }
            ScriptableEnemys Zombies =  ScriptableManager.Instance.GetDataEnemys ( );
            for ( int i = 0 ; i < Zombies.Enemys.Length ; i++ )
            {
                manager.spawnPrefabs.Add ( Zombies.Enemys[i].Prefab);
                print ( "REGISTRADO Zombies" );
            }
            */
            Destroy(gameObject);
        }

    }
}
