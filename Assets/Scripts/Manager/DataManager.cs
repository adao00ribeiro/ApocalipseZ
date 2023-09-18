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
        [SerializeField] private DataCharacter[] ListChareacter;
        [SerializeField] private DataBullet[] ListBullets;
        [SerializeField] private DataAudio[] ListAudios;
        [SerializeField] private DataParticles[] ListParticles;
        [SerializeField] private ScriptableTextureSounds ScriptableTextureSounds;
        void Start()
        {
            ListArmsWeapon = Resources.LoadAll<DataArmsWeapon>("Datas/DataArmsWeapon");
            ListItems = Resources.LoadAll<DataItem>("Datas/DataItems");
            ListChareacter = Resources.LoadAll<DataCharacter>("Datas/DataCharacters");
            ListBullets = Resources.LoadAll<DataBullet>("Datas/DataBullets");
            ListAudios = Resources.LoadAll<DataAudio>("Datas/DataAudios");
            ListParticles = Resources.LoadAll<DataParticles>("Datas/DataParticles");
        }
        internal DataArmsWeapon GetArmsWeaponById(string guidId)
        {
            DataArmsWeapon temp = null;
            foreach (DataArmsWeapon arms in ListArmsWeapon)
            {
                if (arms.GuidId == guidId)
                {
                    temp = arms;
                }
            }
            return temp;
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
        internal DataItem GetDataItemWeapon()
        {

            List<DataItem> listWeapons = new List<DataItem>();
            foreach (DataItem item in ListItems)
            {
                if (item.Type == ItemType.weapon)
                {
                    listWeapons.Add(item);
                }
            }


            return listWeapons[Random.Range(0, listWeapons.Count - 1)];
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
        internal DataCharacter GetDataCharacter(string Name)
        {
            DataCharacter temp = null;
            foreach (DataCharacter item in ListChareacter)
            {
                if (item.Name == Name)
                {
                    temp = item;
                }
            }
            return temp;
        }
        internal DataBullet GetDataBullet(string Name)
        {
            DataBullet temp = null;
            foreach (DataBullet item in ListBullets)
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