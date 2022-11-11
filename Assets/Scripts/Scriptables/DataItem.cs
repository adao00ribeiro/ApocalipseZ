using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{

    public enum ItemType : byte { none, weapon, ammo, consumable }

    [CreateAssetMenu(fileName = "DataItem", menuName = "Data/Item", order = 1)]
    public class DataItem : ScriptableObject
    {
        [SerializeField] public string GuidId;
        [SerializeField] public ItemType Type;
        [SerializeField] public string Name;
        [SerializeField] public bool isStackable;
        [SerializeField] public Sprite Thumbnail;
        [SerializeField] public string Description = "NONE";
        [SerializeField] public int maxStacksize;
        [SerializeField] public int Ammo;
        [SerializeField] public int addHealth;
        [SerializeField] public int addSatiety;
        [SerializeField] public int addHydratation;
        [SerializeField] public float Durability;
        [SerializeField] public GameObject Prefab;


        public DataItem()
        {
            this.GuidId = System.Guid.NewGuid().ToString();
            this.Type = ItemType.none;
            this.Name = "NONE";
            this.isStackable = true;
            this.Thumbnail = null;
            this.Description = "NONE";
            this.maxStacksize = 4;
            this.Ammo = 0;
            this.Durability = 0.0f;
            this.Prefab = null;
        }

    }
}
