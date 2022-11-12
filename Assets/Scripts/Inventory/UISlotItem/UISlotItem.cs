using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace ApocalipseZ
{


    public class UISlotItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TypeContainer AcceptedType;
        [SerializeField] private int SlotIndex;
        [SerializeField] private Image Image;
        [SerializeField] private Text TextQuantidade;
        [SerializeField] private SlotInventoryTemp slot;
        private Vector2 offset;
        bool IsLocked = false;
        public Transform HUD;
        public static UISlotItem SlotSelecionado;
        public static UISlotItem SlotEnter;


        private void Awake()
        {

            Image = transform.Find("Image").GetComponent<Image>();
            TextQuantidade = transform.Find("Image/TextQuantidade").GetComponent<Text>();
            TextQuantidade.text = "";
        }



        
        // Start is called before the first frame update

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (SlotSelecionado)
            {
                //this.transform.SetParent(this.transform.parent.parent);
                //this.transform.position = eventData.position - offset;
                SlotSelecionado.GetComponent<Image>().raycastTarget = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (SlotSelecionado)
            {
                SlotSelecionado.transform.position = eventData.position - offset;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (SlotSelecionado != null)
            {
                if (SlotEnter != null && SlotSelecionado != SlotEnter)
                {
                    if (SlotEnter.AcceptedType == TypeContainer.INVENTORY)
                    {
                  Inventory inventory =  GameController.Instance.FpsPlayer.GetInventory();
                  slotentervazio
                  inventory.CmdMoveItem( SlotSelecionado.slot, SlotEnter.slot);

                    }
                     if (SlotEnter.AcceptedType == TypeContainer.FASTITEMS)
                    {
                         print("fastitens");
                    }
                     if (SlotEnter.AcceptedType == TypeContainer.WEAPONS)
                    {
                        print("weapons");
                    }
                }
                Destroy(SlotSelecionado.gameObject);
            }

        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (slot == null)
            {
                return;
            }
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                // GameObject Slotoptions = PlayerController.CreateUi(controller.Prefab_PainelSlotOptions);
                // Slotoptions.transform.position = eventData.position;
            }
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                SlotSelecionado = Instantiate(this, HUD);
                SlotSelecionado.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
                SlotSelecionado.transform.position = eventData.position;

            }
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (SlotSelecionado != null)
            {
                SlotEnter = this;
                SlotEnter.Image.color = Color.green;
            }

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (slot == null)
            {
                Image.color = Color.clear;
            }
            SlotEnter = null;

            //   tooltip.Deactivate();
        }
        public void SetInventorySlot(SlotInventoryTemp _slot)
        {
            slot = _slot;
        }
        public void SetTextQuantidade(string text)
        {
            TextQuantidade.text = text;
        }
        public void SetImage(Sprite image)
        {
            Image.sprite = image;
        }
        public void SetSlotIndex(int index)
        {
            SlotIndex = index;
        }
        public int GetSlotIndex()
        {
            return SlotIndex;
        }
    }
}