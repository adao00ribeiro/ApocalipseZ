using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
namespace ApocalipseZ
{

    public class DataManager : MonoBehaviour
    {

        [SerializeField] private DataArmsWeapon[] ListArmsWeapon;
        [SerializeField] private DataItem[] ListItems;
        [SerializeField] private DataAudio[] ListAudios;
        [SerializeField] private DataParticles[] ListParticles;
        [SerializeField] private ScriptableTextureSounds ScriptableTextureSounds;
        void Start()
        {
            ListArmsWeapon = Resources.LoadAll<DataArmsWeapon>("Datas/DataArmsWeapon");
            ListItems = Resources.LoadAll<DataItem>("Datas/DataItems");
            ListAudios = Resources.LoadAll<DataAudio>("Datas/DataAudios");
            ListParticles = Resources.LoadAll<DataParticles>("Datas/DataParticles");
        }

        internal DataArmsWeapon GetArmsWeapon(string weaponName)
        {
            DataArmsWeapon temp = null;
            foreach (DataArmsWeapon arms in ListArmsWeapon)
            {
                if (arms.Name == weaponName)
                {
                    temp = arms;
                }
            }
            return temp;
        }

        internal DataItem GetDataItem(string Name)
        {
            DataItem temp = null;
            foreach (DataItem item in ListItems)
            {
                if (item.Name == Name)
                {
                    temp = item;
                }
            }
            return temp;
        }
        internal DataItem GetDataItemById(string guidId)
        {
            DataItem temp = null;
            foreach (DataItem item in ListItems)
            {
                if (item.GuidId == guidId)
                {
                    temp = item;
                }
            }
            return temp;
        }

        internal DataAudio GetDataAudio(string Name)
        {
            DataAudio temp = null;
            foreach (DataAudio item in ListAudios)
            {
                if (item.Name == Name)
                {
                    temp = item;
                }
            }
            return temp;
        }

        internal DataParticles GetDataParticles(string NameParticles)
        {
            DataParticles temp = null;
            foreach (DataParticles item in ListParticles)
            {
                if (item.Name == NameParticles)
                {
                    temp = item;
                }
            }
            return temp;
        }

        internal ScriptableTextureSounds GetScriptableTextureSounds()
        {
            return ScriptableTextureSounds;
        }
    }
}