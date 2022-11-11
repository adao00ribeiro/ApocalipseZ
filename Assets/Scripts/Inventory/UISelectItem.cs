using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ApocalipseZ
{
    public class UISelectItem : MonoBehaviour
    {
        public int id;
        public TypeContainer AcceptedType;
        [SerializeField] SSlotInventory slot;

        private Image image;

        Vector3 positionInitial;
        // Start is called before the first frame update
        void Awake()
        {

            positionInitial = transform.position;
            image = GetComponent<Image>();
            enabled = false;
            SetSlot(new SSlotInventory());
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = InputManager.GetMousePosition();

        }

        private void OnDisable()
        {
            SetSlot(new SSlotInventory());
            transform.position = positionInitial;

        }
        public void SetSlot(SSlotInventory _slot)
        {
            if (_slot.GetDataItem() == null)
            {
                slot = _slot;
                image.sprite = null;
                image.color = Color.clear;
                return;
            }

            slot.SetSItem(_slot.GetDataItem());
            slot.SetQuantity(_slot.GetQuantity());

            image.sprite = slot.GetDataItem().Thumbnail;
            image.preserveAspect = true;
            image.color = Color.white;
        }

        public SSlotInventory GetSlot()
        {
            return slot;
        }
        private InputManager PInputManager;
        public InputManager InputManager
        {
            get
            {
                if (PInputManager == null)
                {
                    PInputManager = GameObject.Find("InputManager").GetComponent<InputManager>();
                }
                return PInputManager;
            }
        }

    }
}