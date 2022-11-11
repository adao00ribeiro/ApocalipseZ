using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ApocalipseZ
{
    public class RequireItem : MonoBehaviour
    {
        [SerializeField] private string NameItem;

        public bool OwnsItem(IContainer Inventory)
        {
            return true;
        }
    }
}