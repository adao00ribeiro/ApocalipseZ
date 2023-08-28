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
        private Vector2 offset;
        bool IsLocked = false;
        public Transform HUD;
        public static UISlotItem SlotSelecionado;
        public static UISlotItem SlotEnter;

        Inventory inventory;
        WeaponManager weaponManager;
        public bool isEmpty;


        private void Awake()
        {
            TextQuantidade.text = "";

            SetImage(null);
        }

        public void SetInventory(Inventory _inventory)
        {
            inventory = _inventory;
        }
        public void SetWeaponManager(WeaponManager _weaponmanager)
        {
            weaponManager = _weaponmanager;
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

                        if (SlotEnter.SlotIndex != SlotSelecionado.SlotIndex)
                        {
                            inventory.CmdInsertItem(SlotEnter.SlotIndex, SlotSelecionado.SlotIndex);
                        }

                    }
                    if (SlotEnter.AcceptedType == TypeContainer.FASTITEMS)
                    {
                        print("fastitens");
                    }
                    if (SlotEnter.AcceptedType == TypeContainer.WEAPONS)
                    {
                        if (SlotSelecionado.AcceptedType == TypeContainer.INVENTORY)
                        {
                            weaponManager.CmdAddWeaponRemoveInventory(SlotEnter.SlotIndex, SlotSelecionado.SlotIndex);
                        }
                        else if (SlotSelecionado.AcceptedType == TypeContainer.WEAPONS)
                        {
                            //weaponManager.CmdMove(SlotEnter.SlotIndex, SlotSelecionado.SlotIndex);
                        }

                    }
                }
                Destroy(SlotSelecionado.gameObject);
            }

        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (isEmpty)
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
            if (isEmpty)
            {
                Image.color = Color.clear;
            }
            SlotEnter = null;

            //   tooltip.Deactivate();
        }
        public void SetIsEmpty(bool empty)
        {
            isEmpty = empty;
        }
        public void SetTextQuantidade(string text)
        {
            TextQuantidade.text = text;
        }
        public void SetImage(Sprite image)
        {
            Image.sprite = image;
            if (image == null)
            {
                Image.color = Color.clear;
            }
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