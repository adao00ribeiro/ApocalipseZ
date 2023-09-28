using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ApocalipseZ
{
    public class DecalFxManager : MonoBehaviour
    {
        private GameObject[] decals;
        [Header("Decals")]
        [Range(1, 200)]
        [Tooltip("Pool size for each type of decals. For example, if pool size is 10, concrete, wood, dirt, metal decals will have 10 their instances after start")]
        public int decalsPoolSizeForEachType;

        private GameObject[] concretedecal_pool;

        private GameObject[] wooddecal_pool;

        private GameObject[] dirtdecal_pool;

        private GameObject[] metaldecal_pool;

        private int decalIndex_wood = 0;
        private int decalIndex_concrete = 0;
        private int decalIndex_dirt = 0;
        private int decalIndex_metal = 0;
        // Start is called before the first frame update

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

        private GameObject[] concreteDecal_pool
        {
            get
            {
                if (concretedecal_pool == null)
                {
                    concretedecal_pool = new GameObject[decalsPoolSizeForEachType];
                    var decalsParentObject_concrete = new GameObject("decalsPool_concrete");

                    for (int i = 0; i < decalsPoolSizeForEachType; i++)
                    {
                        concretedecal_pool[i] = Instantiate(GameController.Instance.DataManager.GetDecal("Concrete").Decal);
                        concretedecal_pool[i].SetActive(false);
                        concretedecal_pool[i].transform.parent = decalsParentObject_concrete.transform;
                    }
                }
                return concretedecal_pool;
            }
        }

        private GameObject[] woodDecal_pool
        {
            get
            {
                if (wooddecal_pool == null)
                {
                    wooddecal_pool = new GameObject[decalsPoolSizeForEachType];
                    var decalsParentObject_wood = new GameObject("decalsPool_wood");

                    for (int i = 0; i < decalsPoolSizeForEachType; i++)
                    {
                        wooddecal_pool[i] = Instantiate(GameController.Instance.DataManager.GetDecal("Wood").Decal);
                        wooddecal_pool[i].SetActive(false);
                        wooddecal_pool[i].transform.parent = decalsParentObject_wood.transform;
                    }
                }
                return wooddecal_pool;
            }
        }

        private GameObject[] dirtDecal_pool
        {
            get
            {
                if (dirtdecal_pool == null)
                {
                    dirtdecal_pool = new GameObject[decalsPoolSizeForEachType];
                    var decalsParentObject_dirt = new GameObject("decalsPool_dirt");

                    for (int i = 0; i < decalsPoolSizeForEachType; i++)
                    {
                        dirtdecal_pool[i] = Instantiate(GameController.Instance.DataManager.GetDecal("Dirt").Decal);
                        dirtdecal_pool[i].SetActive(false);
                        dirtdecal_pool[i].transform.parent = decalsParentObject_dirt.transform;
                    }
                }
                return dirtdecal_pool;
            }
        }

        private GameObject[] metalDecal_pool
        {
            get
            {
                if (metaldecal_pool == null)
                {
                    metaldecal_pool = new GameObject[decalsPoolSizeForEachType];
                    var decalsParentObject_metal = new GameObject("decalsPool_metal");

                    for (int i = 0; i < decalsPoolSizeForEachType; i++)
                    {
                        metaldecal_pool[i] = Instantiate(GameController.Instance.DataManager.GetDecal("Metal").Decal);
                        metaldecal_pool[i].SetActive(false);
                        metaldecal_pool[i].transform.parent = decalsParentObject_metal.transform;
                    }
                }
                return metaldecal_pool;
            }
        }

    }
}