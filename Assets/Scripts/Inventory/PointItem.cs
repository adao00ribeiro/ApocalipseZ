using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ramdom = UnityEngine.Random;
namespace ApocalipseZ
{
    public class PointItem : MonoBehaviour
    {
        public ItemType type;

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
            GameObject objeto = null;
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
            return objeto;
        }

    }
}