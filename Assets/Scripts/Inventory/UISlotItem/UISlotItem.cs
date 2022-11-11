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
        [SerializeField] private TypeContainer AcceptedType;
        [SerializeField] private Image Image;
        [SerializeField] private Text TextQuantidade;
        [SerializeField] private SSlotInventory slot = null;
        public string nameItemSlot;
        [SerializeField] private UISlotItem PrefabUiSlotItem;

        public static UISlotItem SlotSelecionado;
        public static UISlotItem SlotEnter;
        Transform HUD;

        IFpsPlayer player;
        private Vector2 offset;
        IContainer Container;

        bool IsLocked = false;
        private void Awake()
        {

            Image = transform.Find("Image").GetComponent<Image>();
            TextQuantidade = transform.Find("Image/TextQuantidade").GetComponent<Text>();
            TextQuantidade.text = "";
        }
        // Start is called before the first frame update
        private void Start()
        {
            HUD = GameObject.Find("HUD").transform;

        }
        private void OnDisable()
        {
            if (SlotSelecionado)
            {
                Destroy(SlotSelecionado.gameObject);
            }
        }
        public void UpdateSlot()
        {

            SetSlot(Container.GetSlotContainer(SlotIndex));
        }

        internal void SetSlotIndex(int i)
        {
            SlotIndex = i;
        }

        public void RemoveSlot()
        {

            UISlotItemTemp slotui = new UISlotItemTemp(SlotIndex, AcceptedType, slot.GetSlotTemp());
            // Container.CmdRemoveSlotContainer(AcceptedType, slotui);

        }
        public void MoveSlot(int slotIndexSelecionado, int slotIndexEnter)
        {
            //   Container.CmdMoveSlotContainer(AcceptedType, slotIndexSelecionado, slotIndexEnter);
        }
        public void AddSlot(SSlotInventory _slot)
        {

            UISlotItemTemp slotui = new UISlotItemTemp(SlotIndex, AcceptedType, new SlotInventoryTemp(_slot.GetSlotIndex(), _slot.GetDataItem().GuidId, _slot.GetAmmo(), _slot.GetQuantity()));

            // Container.CmdAddSlotContainer(AcceptedType, slotui);
        }

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
            //this.transform.SetParent(ui.painel[slotId].transform);
            //this.transform.position = ui.painel[slotId].transform.position;
            //GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (SlotSelecionado != null)
            {
                if (SlotEnter != null && !SlotEnter.IsLocked && SlotSelecionado != SlotEnter)
                {
                    if (SlotEnter.AcceptedType == SlotSelecionado.AcceptedType)
                    {
                        MoveSlot(SlotSelecionado.SlotIndex, SlotEnter.SlotIndex);
                    }
                    else
                    {
                        SlotEnter.AddSlot(SlotSelecionado.slot);
                        RemoveSlot();
                        SetSlot(null);
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


                SlotSelecionado = Instantiate(PrefabUiSlotItem, HUD);
                SlotSelecionado.SetSlotIndex(SlotIndex);
                SlotSelecionado.AcceptedType = AcceptedType;
                SlotSelecionado.SetFpsPlayer(player);
                SlotSelecionado.SetContainer(Container);
                SlotSelecionado.UpdateSlot();
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

                if (SlotEnter.AcceptedType == TypeContainer.FASTITEMS)
                {
                    if (SlotSelecionado.slot.GetDataItem().Type == ItemType.weapon)
                    {
                        SlotEnter.IsLocked = true;
                        SlotEnter.SetImagemColor(Color.red);
                        return;
                    }
                }
                if (SlotEnter.AcceptedType == TypeContainer.WEAPONS)
                {
                    if (SlotSelecionado.slot.GetDataItem().Type != ItemType.weapon)
                    {
                        SlotEnter.IsLocked = true;
                        SlotEnter.SetImagemColor(Color.red);
                        return;
                    }
                }
                if (SlotEnter.slot != null && SlotEnter.AcceptedType != SlotSelecionado.AcceptedType)
                {
                    SlotEnter.IsLocked = true;
                    SlotEnter.SetImagemColor(Color.red);
                    return;
                }
                if (SlotSelecionado != null && SlotSelecionado != SlotEnter)
                {
                    SlotEnter.IsLocked = false;
                    SlotEnter.SetImagemColor(Color.green);
                }
            }
            //     tooltip.Activate(item);

        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (slot == null)
            {
                SetImagemColor(Color.clear);
            }
            SlotEnter = null;


            //   tooltip.Deactivate();
        }

        public void SetSlot(SSlotInventory _slot)
        {

            slot = _slot;

            if (slot == null)
            {
                Image.sprite = null;
                Image.color = Color.clear;
                TextQuantidade.text = "";
                nameItemSlot = "NONE";
                return;
            }
            Image.sprite = slot.GetDataItem().Thumbnail;
            Image.color = Color.white;
            TextQuantidade.text = slot.GetQuantity().ToString();
            nameItemSlot = slot.GetDataItem().name;
        }
        public void SetFpsPlayer(IFpsPlayer _player)
        {
            player = _player;
        }
        public void SetContainer(IContainer container)
        {
            Container = container;
        }

        public int GetId()
        {
            return SlotIndex;
        }

        public void SetImagemColor(Color color)
        {
            Image.color = color;
        }


        public UISlotItemTemp GetUISlotItemTemp()
        {
            return new UISlotItemTemp(SlotIndex, AcceptedType, new SlotInventoryTemp(slot.GetSlotIndex(), slot.GetDataItem().GuidId, slot.GetAmmo(), slot.GetQuantity()));
        }
    }
}