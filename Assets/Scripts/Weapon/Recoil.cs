using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ApocalipseZ
{
    public class Recoil : MonoBehaviour
    {
        public float recoilReleaseSpeed = 2f;

        private void Update()
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(Vector3.zero), Time.deltaTime * recoilReleaseSpeed);
        }

        public void AddRecoil(Vector2 recoil)
        {
            transform.localRotation *= Quaternion.Euler(new Vector3(Random.Range(0, recoil.x), Random.Range(0, recoil.y), 0));
        }
    }
}