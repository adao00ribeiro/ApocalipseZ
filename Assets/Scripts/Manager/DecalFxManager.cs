using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ApocalipseZ
{
    public class DecalFxManager : MonoBehaviour
    {
        private DataDecals DataDecals;


        private GameObject[] decals;

        [Header("Decals")]

        [Range(1, 200)]
        [Tooltip("Pool size for each type of decals. For example, if pool size is 10, concrete, wood, dirt, metal decals will have 10 their instances after start")]
        public int decalsPoolSizeForEachType;

        [HideInInspector]
        public GameObject[] concreteDecal_pool;
        [HideInInspector]
        public GameObject[] woodDecal_pool;
        [HideInInspector]
        public GameObject[] dirtDecal_pool;
        [HideInInspector]
        public GameObject[] metalDecal_pool;

        private int decalIndex_wood = 0;
        private int decalIndex_concrete = 0;
        private int decalIndex_dirt = 0;
        private int decalIndex_metal = 0;
        // Start is called before the first frame update

        /*
        void Start()
        {
            DataDecals = GameController.Instance.DataDecals;
            DecalsPool();
        }
        public void DecalsPool()
        {
            concreteDecal_pool = new GameObject[decalsPoolSizeForEachType];
            var decalsParentObject_concrete = new GameObject("decalsPool_concrete");

            for (int i = 0; i < decalsPoolSizeForEachType; i++)
            {
                concreteDecal_pool[i] = Instantiate(DataDecals.GetDecal("Concrete"));
                concreteDecal_pool[i].SetActive(false);
                concreteDecal_pool[i].transform.parent = decalsParentObject_concrete.transform;
            }

            woodDecal_pool = new GameObject[decalsPoolSizeForEachType];
            var decalsParentObject_wood = new GameObject("decalsPool_wood");

            for (int i = 0; i < decalsPoolSizeForEachType; i++)
            {
                woodDecal_pool[i] = Instantiate(DataDecals.GetDecal("Wood"));
                woodDecal_pool[i].SetActive(false);
                woodDecal_pool[i].transform.parent = decalsParentObject_wood.transform;
            }

            dirtDecal_pool = new GameObject[decalsPoolSizeForEachType];
            var decalsParentObject_dirt = new GameObject("decalsPool_dirt");

            for (int i = 0; i < decalsPoolSizeForEachType; i++)
            {
                dirtDecal_pool[i] = Instantiate(DataDecals.GetDecal("Dirt"));
                dirtDecal_pool[i].SetActive(false);
                dirtDecal_pool[i].transform.parent = decalsParentObject_dirt.transform;
            }

            metalDecal_pool = new GameObject[decalsPoolSizeForEachType];
            var decalsParentObject_metal = new GameObject("decalsPool_metal");

            for (int i = 0; i < decalsPoolSizeForEachType; i++)
            {
                metalDecal_pool[i] = Instantiate(DataDecals.GetDecal("Metal"));
                metalDecal_pool[i].SetActive(false);
                metalDecal_pool[i].transform.parent = decalsParentObject_metal.transform;
            }

        }
        internal void ApplyFX(RaycastHit hit, bool applyParent)
        {
            if (hit.collider.CompareTag("Concrete"))
            {
                concreteDecal_pool[decalIndex_concrete].SetActive(true);
                var decalPostion = hit.point + hit.normal * 0.025f;
                concreteDecal_pool[decalIndex_concrete].transform.position = decalPostion;
                concreteDecal_pool[decalIndex_concrete].transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);
                if (applyParent)
                    decals[decalIndex_concrete].transform.parent = hit.transform;

                decalIndex_concrete++;

                if (decalIndex_concrete == decalsPoolSizeForEachType)
                {
                    decalIndex_concrete = 0;
                }
            }
            else if (hit.collider.CompareTag("Wood"))
            {
                woodDecal_pool[decalIndex_wood].SetActive(true);
                var decalPostion = hit.point + hit.normal * 0.025f;
                woodDecal_pool[decalIndex_wood].transform.position = decalPostion;
                woodDecal_pool[decalIndex_wood].transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);
                if (applyParent)
                    decals[decalIndex_wood].transform.parent = hit.transform;

                decalIndex_wood++;

                if (decalIndex_wood == decalsPoolSizeForEachType)
                {
                    decalIndex_wood = 0;
                }
            }
            else if (hit.collider.CompareTag("Dirt"))
            {
                dirtDecal_pool[decalIndex_dirt].SetActive(true); var decalPostion = hit.point + hit.normal * 0.025f;
                dirtDecal_pool[decalIndex_dirt].transform.position = decalPostion;
                dirtDecal_pool[decalIndex_dirt].transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);
                if (applyParent)
                    decals[decalIndex_dirt].transform.parent = hit.transform;

                decalIndex_dirt++;

                if (decalIndex_dirt == decalsPoolSizeForEachType)
                {
                    decalIndex_dirt = 0;
                }
            }
            else if (hit.collider.CompareTag("Metal"))
            {
                metalDecal_pool[decalIndex_metal].SetActive(true);
                var decalPostion = hit.point + hit.normal * 0.025f;
                metalDecal_pool[decalIndex_metal].transform.position = decalPostion;
                metalDecal_pool[decalIndex_metal].transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);
                if (applyParent)
                    decals[decalIndex_metal].transform.parent = hit.transform;

                decalIndex_metal++;

                if (decalIndex_metal == decalsPoolSizeForEachType)
                {
                    decalIndex_metal = 0;
                }
            }
            else
            {
                concreteDecal_pool[decalIndex_concrete].SetActive(true);
                var decalPostion = hit.point + hit.normal * 0.025f;
                concreteDecal_pool[decalIndex_concrete].transform.position = decalPostion;
                concreteDecal_pool[decalIndex_concrete].transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);
                if (applyParent)
                    decals[decalIndex_concrete].transform.parent = hit.transform;

                decalIndex_concrete++;

                if (decalIndex_concrete == decalsPoolSizeForEachType)
                {
                    decalIndex_concrete = 0;
                }
            }

        }
        */
    }
}