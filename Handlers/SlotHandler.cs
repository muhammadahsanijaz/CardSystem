using System;
using System.Collections;
using System.Collections.Generic;
using MoonKart.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

namespace MoonKart
{
    public class SlotHandler : UIWidget
    {
        //public
        public Card Card
        {
            get
            {
                if (_card == null || _card.CardName == "")
                {
                    return null;
                }

                return _card;
            }
            private set => _card = value;
        }

        //Private
        private Card _card;

        //serialized
        [SerializeField] internal TextMeshProUGUI NameText;
        [SerializeField] internal Image cardIcon;
        [SerializeField] internal Image slotBoard;

        public virtual void Initialize()
        {
            _card = null;
            NameText.text = "";
        }

        public virtual void UnEquipCard(CardCallFrom callFrom)
        {
            if (Card != null)
            {
                Card.UnEquipCard();
            }

            Card = null;
            ChangeSlotApperance(callFrom);
        }

        public virtual void UnStackCard()
        {
            Card.UnStackCard();
            Card = null;
            NameText.text = "";
        }

        public virtual void EquipCard(Card card, CardCallFrom callFrom)
        {
            if (Card != null)
            {
                switch (callFrom)
                {
                    case CardCallFrom.Presets:
                        Card.UnEquipCard();
                        break;
                    case CardCallFrom.Vault:
                        Card.UnStackCard();
                        break;
                }
            }

            Card = card;

            switch (callFrom)
            {
                case CardCallFrom.Presets:
                    Card.EquipCard();
                    break;
                case CardCallFrom.Vault:
                    Card.StackCard();
                    break;
            }

            ChangeSlotApperance(callFrom);
        }


        public virtual void AssignSlot(Card card, CardCallFrom callFrom)
        {
            Card = card;
            ChangeSlotApperance(callFrom);
        }

        internal virtual void UpdateSlot(Card card, CardUseState cardUseState, CardCallFrom cardCallFrom = CardCallFrom.Presets)
        {
            Debug.LogError("Slot " + gameObject.name);
            switch (cardUseState)
            {
                case CardUseState.Equip:
                    switch (cardCallFrom)
                    {
                        case CardCallFrom.Presets:
                            Global.PlayerService.CarSetupUpdated?.Invoke(card, transform.GetSiblingIndex(), true);
                            EquipCard(card, cardCallFrom);
                            break;
                        case CardCallFrom.Vault:
                            EquipCard(card, cardCallFrom);
                            break;
                    }

                    break;
                case CardUseState.UnEquip:

                    switch (cardCallFrom)
                    {
                        case CardCallFrom.Presets:
                            Global.PlayerService.CarSetupUpdated?.Invoke(card, transform.GetSiblingIndex(), false);
                            UnEquipCard(cardCallFrom);

                            break;
                        case CardCallFrom.Vault:
                            UnEquipCard(cardCallFrom);

                            break;
                    }

                    break;
            }
        }

        protected virtual void ChangeSlotApperance(CardCallFrom callFrom)
        {
            switch (callFrom)
            {
                case CardCallFrom.Presets:
                    NameText.SetTextSafe((_card != null) ? _card.CardName : "");
                    cardIcon.sprite = (Card != null && Card.Icon != null) ? Card.Icon : Global.Settings.CardsSetting.addIconSlot;
                    break;
                case CardCallFrom.Vault:
                    cardIcon.sprite = (Card != null && Card.Icon != null) ? Card.Icon : Global.Settings.CardsSetting.defaultIconSlot;
                    break;
            }

            ChangeSlotBorder();
        }

        public virtual void ChangeSlotBorder()
        {
            if (_card != null)
            {
                slotBoard.sprite = Global.Settings.CardsSetting.slotBorderByRarity[(int)_card.CardRarity];
            }
            else
            {
                slotBoard.sprite = Global.Settings.CardsSetting.slotBorderDefault;
            }
        }
    }
}