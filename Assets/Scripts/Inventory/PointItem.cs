using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using Ramdom = UnityEngine.Random;
using ApocalipseZ;

namespace ApocalipseZ
{

    public class PointItem : MonoBehaviour
    {
        [Header("Tipo de item")]
        [SerializeField] public ItemType type;

        [Header("Select Weapon Spaw")]
        [SerializeField] public EWeapon WeaponSpaw;

        private void OnValidate()
        {
            if (type == ItemType.none)
            {
                WeaponSpaw = EWeapon.DEFAULT;
            }
        }
     
        private void Start()
        {
            if (type == ItemType.none)
            {
                System.Type tipo = typeof(ItemType);
                System.Array values = System.Enum.GetValues(tipo);
                //Array values = Enum.GetValues(type);
                type = (ItemType)values.GetValue(Random.Range(1, values.Length));
            }
        }

        public GameObject GetPrefab()
        {
            if (WeaponSpaw != EWeapon.DEFAULT)
            {
                DataItem objeto2 = GameController.Instance.DataManager.GetDataItemWeaponByName(WeaponSpaw.ToString());
                return objeto2.Prefab;
            }
            DataItem objeto = GameController.Instance.DataManager.GetDataItemWeapon();

            /*
            ScriptableItem[]items;

            switch ( type )
            {
                case ItemType.consumable:
                    items = ScriptableManager.Instance.GetItemsConsumable ( );
                    objeto = items[Random.Range ( 0, items.Length )].sitem.Prefab;
                    break;
                case ItemType.weapon:
                    items = ScriptableManager.Instance.GetItemsWeapons ( );
                    objeto = items[Random.Range ( 0 , items.Length )].sitem.Prefab;
                    break;
            }
            */
            return objeto.Prefab;
        }

    }
}

