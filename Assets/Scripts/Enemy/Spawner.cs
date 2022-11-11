using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Mirror;
namespace ApocalipseZ
{
    [System.Serializable]
    public struct ZombieTypeList
    {
        public TypeEnemy type;
    }
    public class Spawner : NetworkBehaviour
    {
        public Vector3 Size;
        public ZombieTypeList[] listType;
        public float NumberOfZombie;
        float CurrentTimer;
        public float TimerSpaw;
        public List<Zombie> ListZombie = new List<Zombie>();
        // Start is called before the first frame update

        private void FixedUpdate()
        {
            if (isServer)
            {
                if (NumberOfZombie > ListZombie.Count)
                {
                    CurrentTimer += Time.fixedDeltaTime;

                    if (CurrentTimer >= TimerSpaw)
                    {
                        // Spawn ( ScriptableManager.Instance.GetPrefabEnemy ( listType[Random.Range ( 0 , listType.Length )].type ) );
                        CurrentTimer = 0;
                    }
                }
            }

        }


        public void Spawn(GameObject prefab)
        {
            GameObject temp = Instantiate(prefab, GetRandomPosition(), Quaternion.identity);
            Zombie zombieTemp = temp.GetComponent<Zombie>();
            zombieTemp.OnZombieIsDead += () =>
            {
                ListZombie.Remove(zombieTemp);
            };
            ListZombie.Add(zombieTemp);
            NetworkServer.Spawn(temp);
        }
        private Vector3 GetRandomPosition()
        {
            float x = Random.Range(transform.position.x - Size.x / 2, transform.position.x + Size.x / 2);
            float z = Random.Range(transform.position.z - Size.z / 2, transform.position.z + Size.z / 2);
            return new Vector3(x, transform.position.y, z);
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireCube(transform.position, Size);
        }




    }

}