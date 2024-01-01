using MoonKart.UI;
using System;
using TMPro;
using UnityEngine;
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
            if (_card != null)
            {
                _card.UnEquipCard();
            }

            _card = null;
            ChangeSlotApperance(callFrom);
        }

        public virtual void UnStakeCard()
        {
            _card.StackedTime = DateTime.UtcNow.AddMinutes(Global.Settings.CardsSetting.UnstakingCoolDownTimeInMinutes);
            _card.CoolDownCard();
            Context.CardsManager.OnStatusChangeToStake();
            _card.UnapplyCard();
            _card = null;
            NameText.text = "";
        }

        public virtual void EquipCard(Card card, CardCallFrom callFrom)
        {
            if (_card != null)
            {
                switch (callFrom)
                {
                    case CardCallFrom.Presets:
                        _card.UnEquipCard();
                        break;
                    case CardCallFrom.Vault:
                        _card.StackedTime = DateTime.UtcNow.AddMinutes(Global.Settings.CardsSetting.UnstakingCoolDownTimeInMinutes);
                        _card.CoolDownCard();
                        Context.CardsManager.OnStatusChangeToStake();
                        _card.UnapplyCard();
                        break;
                }
            }

            _card = card;

            switch (callFrom)
            {
                case CardCallFrom.Presets:
                    _card.EquipCard();
                    break;
                case CardCallFrom.Vault:
                    _card.ApplyCard();
                    break;
            }

            ChangeSlotApperance(callFrom);
        }


        public virtual void AssignSlot(Card card, CardCallFrom callFrom)
        {
            _card = card;
            ChangeSlotApperance(callFrom);
        }

        internal virtual void UpdateSlot(Card card, CardUseState cardUseState, CardCallFrom cardCallFrom = CardCallFrom.Presets)
        {
            switch (cardUseState)
            {
                case CardUseState.Equip:
                    switch (cardCallFrom)
                    {
                        case CardCallFrom.Presets:
                            Context.CardsManager.CarSetupUpdated?.Invoke(card, transform.GetSiblingIndex(), true);
                            EquipCard(card, cardCallFrom);
                            ChangeModelApperance();
                            break;
                        case CardCallFrom.Vault:
                            EquipCard(card, cardCallFrom);
                            break;
                    }
                    ChangeSlotApperance(cardCallFrom);
                    break;
                case CardUseState.UnEquip:

                    switch (cardCallFrom)
                    {
                        case CardCallFrom.Presets:
                            Context.CardsManager.CarSetupUpdated?.Invoke(card, transform.GetSiblingIndex(), false);
                            UnEquipCard(cardCallFrom);
                            ChangeModelApperance();
                            break;
                        case CardCallFrom.Vault:
                            UnEquipCard(cardCallFrom);

                            break;
                    }
                    ChangeSlotApperance(cardCallFrom);
                    break;
            }
        }

        protected virtual void ChangeSlotApperance(CardCallFrom callFrom)
        {
            switch (callFrom)
            {
                case CardCallFrom.Presets:
                    NameText.SetTextSafe((_card != null) ? _card.CardName : "");
                    cardIcon.sprite = (_card != null && _card.Icon != null) ? _card.Icon : Global.Settings.CardsSetting.addIconSlot;
                    break;
                case CardCallFrom.Vault:
                    cardIcon.sprite = (_card != null && _card.Icon != null) ? _card.Icon : Global.Settings.CardsSetting.defaultIconSlot;
                    break;
            }

            ChangeSlotBorder();
        }

        protected virtual void ChangeModelApperance()
        {

        }

        public virtual void ChangeSlotBorder()
        {
            if (_card != null && _card.CardName != "")
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