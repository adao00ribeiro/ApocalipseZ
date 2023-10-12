using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace ApocalipseZ
{
    public class Character : MonoBehaviour
    {
        public Transform spine;
        public Flag flag;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
           
        }

        public void SetFlag(Flag _flag)
        {
            _flag.transform.SetParent(spine);
            _flag.transform.localPosition = Vector3.zero;
            _flag.transform.localRotation = Quaternion.identity;
            flag = _flag;
        }
        public void DropFlag()
        {
            if(flag==null){
                return;
            }
            flag.transform.SetParent(null);
            flag.IsLocalPoint = false;
            flag = null;
           
        }
       
      
    }
}