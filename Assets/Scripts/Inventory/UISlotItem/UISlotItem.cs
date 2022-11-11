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
        [SerializeField] private int SlotIndex;
        [SerializeField] private Image Image;
        [SerializeField] private Text TextQuantidade;
        [SerializeField] private SSlotInventory slot;
        private Vector2 offset;
        bool IsLocked = false;
        private void Awake()
        {

            Image = transform.Find("Image").GetComponent<Image>();
            TextQuantidade = transform.Find("Image/TextQuantidade").GetComponent<Text>();
            TextQuantidade.text = "";
        }
        // Start is called before the first frame update

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (CanvasFpsPlayer.SlotSelecionado)
            {
                //this.transform.SetParent(this.transform.parent.parent);
                //this.transform.position = eventData.position - offset;
                CanvasFpsPlayer.SlotSelecionado.GetComponent<Image>().raycastTarget = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (CanvasFpsPlayer.SlotSelecionado)
            {
                CanvasFpsPlayer.SlotSelecionado.transform.position = eventData.position - offset;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (CanvasFpsPlayer.SlotSelecionado != null)
            {

                Destroy(CanvasFpsPlayer.SlotSelecionado.gameObject);
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



            }
            offset = eventData.position - new Vector2(this.transform.position.x, this.transform.position.y);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {


        }

        public void OnPointerExit(PointerEventData eventData)
        {


            //   tooltip.Deactivate();
        }
        public void SetInventorySlot(SSlotInventory _slot)
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