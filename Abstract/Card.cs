using System;
using System.Collections;
using System.Collections.Generic;
using MoonKart.UI;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace MoonKart
{
    public enum EquipState
    {
        EquipToSlot,
        InInventory
    }

    public enum CardCategory
    {
        Regular = 0,
        GoldFoil = 1
    }

    public enum CardRarity
    {
        Common = 0,
        Rare = 1,
        Epic = 2,
        Legendary = 3,
    }

    public enum CardState
    {
        None = 0,
        Equip = 1,
        Staking = 2,
        Merge = 3,
        Staked = 4,
        Applied = 5,
        CoolDown = 6,
    }

    /// <summary>
    /// Abstract Class of card
    /// </summary>
    [System.Serializable]
    public abstract class Card
    {
        // Public parameters
        public int Id
        {
            private set => _id = value;
            get => _id;
        }

        public string CardName
        {
            get => _cardName;
        }

        public CardCategory CardCategory
        {
            get
            {
                if (_cardTemplateModel == null)
                    return CardCategory.Regular;
                return _cardTemplateModel.cardCategory;
            }
        }

        public CardRarity CardRarity
        {
            get
            {
                if (_cardTemplateModel == null)
                    return CardRarity.Common;
                return _cardTemplateModel.cardRarity;
            }
        }

        public Sprite Icon
        {
            get => _icon;
        }

        public string Description
        {
            get
            {
                if (_cardTemplateModel == null)
                    return "";
                return _cardTemplateModel.description;
            }
        }

        public DateTime StackedTime
        {
            get
            {

                if (_stackedTime == "")
                {
                    return default;
                }
                else
                {
                    try
                    {
                        return DateTime.Parse(_stackedTime);
                    }
                    catch
                    {
                        Debug.LogError("Invalid Time" + _stackedTime);
                        return DateTime.UtcNow;
                    }
                }
            }
            set => _stackedTime = value.ToString("MM/dd/yyyy HH:mm:ss");
        }

        public string StackedTimeString
        {
            get => _stackedTime;
            set => _stackedTime = value;
        }


        public CardsStateModel CardStateModel
        {
            get => _cardStateModel;
        }
        public CardTemplateModel CardTemplateModel
        {
            get => _cardTemplateModel;
        }


        // Private parameters
        private int _id = -1;
        private CardTemplateModel _cardTemplateModel;
        private CardsStateModel _cardStateModel;
        [SerializeField] protected string _cardName;
        private Sprite _icon;
        private string _stackedTime;

        internal bool IsInitialized;

        // Constructors
        protected Card(int id, CardTemplateModel cardTemplateModel)
        {
            _cardTemplateModel = cardTemplateModel;
            Id = id;
            _cardName = cardTemplateModel.templateName;
            IsInitialized = true;
        }

        protected Card()
        {

        }

        public void SetCardLevel(CardsStateModel model)
        {
            _cardStateModel = model;
        }

        public void EquipCard()
        {
            if (!IsInitialized || CardStateModel.cardState != CardState.None)
                return;
            CardStateModel.cardState = CardState.Equip;
        }

        public void UnEquipCard()
        {
            if (!IsInitialized || CardStateModel.cardState != CardState.Equip)
                return;
            CardStateModel.cardState = CardState.None;
        }

        public void ApplyCard()
        {
            if (!IsInitialized || CardStateModel.cardState != CardState.None)
                return;
            CardStateModel.cardState = CardState.Applied;
        }

        public void UnapplyCard()
        {
            if (!IsInitialized || CardStateModel.cardState != CardState.Applied)
                return;
            CardStateModel.cardState = CardState.None;
        }

        public void StakingCard()
        {
            if (!IsInitialized || CardStateModel.cardState != CardState.Applied)
                return;
            CardStateModel.cardState = CardState.Staking;
        }

        public void StakeCard()
        {
            if (!IsInitialized || CardStateModel.cardState != CardState.Staking)
                return;
            CardStateModel.cardState = CardState.Staked;
        }

        public void CoolDownCard()
        {
            if (!IsInitialized || CardStateModel.cardState != CardState.Staked)
                return;
            CardStateModel.cardState = CardState.CoolDown;
        }

        public void UnStakeCard()
        {
            if (!IsInitialized || CardStateModel.cardState != CardState.CoolDown)
                return;
            CardStateModel.cardState = CardState.None;
        }

        public void MergeCard()
        {
            if (!IsInitialized || CardStateModel.cardState != CardState.None)
                return;
            CardStateModel.cardState = CardState.Merge;
        }

        public void UnMergeCard()
        {
            if (!IsInitialized || CardStateModel.cardState != CardState.Merge)
                return;
            CardStateModel.cardState = CardState.None;
        }


        public void SetIcon(Sprite icon)
        {
            if (icon == null)
            {
                Debug.LogError("Icon Not Found");
                return;
            }
            _icon = icon;
        }
    }
}