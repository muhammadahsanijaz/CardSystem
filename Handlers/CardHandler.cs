using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonKart.UI
{
    public class CardHandler : UIWidget
    {
        public Card Card
        {
            get => _card;
        }

        private Card _card;

        public PerformFunctionality performFunctionality
        {
            set { PerformFunctionality = value; }
            get { return PerformFunctionality; }
        }

        [SerializeField] AudioSetup cardClickAS;
        [SerializeField] private PerformFunctionality PerformFunctionality;
        [SerializeField] private PerformFunctionality canRotateCard;
        [SerializeField] private PerformFunctionality canFloatCard;
        [SerializeField] private TextMeshProUGUI notificationText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI speedText;
        [SerializeField] private TextMeshProUGUI handlingText;
        [SerializeField] private TextMeshProUGUI accelerationText;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Image mainIcon;
        [SerializeField] private UIBehaviour[] equipUIBehaviour;
        [SerializeField] private UIBehaviour AppliedTextUI;
        [SerializeField] private UIBehaviour stackTextUI;
        [SerializeField] private UIBehaviour stacktimebarUI;
        [SerializeField] private UIBehaviour[] MergeUIBehaviour;
        [SerializeField] private TextMeshProUGUI dateTimeText;
        [SerializeField] private Button button;
        [SerializeField] private GameObject overlayContainer;
       public UIBehaviour cardFrontUIBehaviour;
        public UIBehaviour cardBackUIBehaviour;

        public Button Button => button;

        public string defaultEquipButtonText { get; set; }
        public string defaultUnEquipButtonText { get; set; }
        public CardCallFrom cardCallFrom { get; set; }

        public Action<int, CardUseState> OnCloseCallBack;

        private bool isInitialized;

        public void Initialize(Card card)
        {
            cardCallFrom = CardCallFrom.MyCard;
            base.OnInitialize();
            _card = card;
            isInitialized = true;
            if (PerformFunctionality == PerformFunctionality.Yes)
                gameObject.name = card.CardName;
            RefreshCardHandler();
        }

        public void RefreshCardHandler()
        {
            if (PerformFunctionality == PerformFunctionality.Yes)
                button.onClick.AddListener(CardButtonPressed);
            
            if(canFloatCard == PerformFunctionality.Yes)
                

            nameText.SetTextSafe(_card.CardName);
            notificationText.SetTextSafe(_card.CardStateModel.cardLevel.ToString());


            speedText.text = "-";
            handlingText.text = "-";
            accelerationText.text = "-";
            healthText.text = "-";
            if (_card is VehicleCard)
            {
                var vehicleCard = _card as VehicleCard;
                speedText.text = vehicleCard.VehicleStats.Speed.ToString("0.#");
                handlingText.text = vehicleCard.VehicleStats.Handling.ToString("0.#");
                accelerationText.text = vehicleCard.VehicleStats.Acceleration.ToString("0.#");
                healthText.text = vehicleCard.VehicleStats.Health.ToString("0.#");
            }
            else if (_card is DriverCard)
            {
                var driverCard = _card as DriverCard;
                speedText.text = ((driverCard.VehicleStats.Speed > 0) ? "+" : "") + driverCard.VehicleStats.Speed.ToString("0.#");
                handlingText.text = ((driverCard.VehicleStats.Handling > 0) ? "+" : "") + driverCard.VehicleStats.Handling.ToString("0.#");
                accelerationText.text = ((driverCard.VehicleStats.Acceleration > 0) ? "+" : "") + driverCard.VehicleStats.Acceleration.ToString("0.#");
                healthText.text = ((driverCard.VehicleStats.Health > 0) ? "+" : "") + driverCard.VehicleStats.Health.ToString("0.#");
            }
            else if (_card is PropertyCard)
            {
                var propertyCard = _card as PropertyCard;
                var ecard = _card as PropertyCard;


                SetSliderValueAccordingToProperty(ecard.PrimaryProperty);
                SetSliderValueAccordingToProperty(ecard.SecondaryProperty);
            }
            else if (_card is FuelCard)
            {
                mainIcon.SetActive(false);
                (this as FuelCardHandler).EnableFuelIcon((_card as FuelCard).CardStateModel.cardLevel);
            }

            mainIcon.sprite = _card.Icon;
        }


        void SetSliderValueAccordingToProperty(PropertyModel propertyModel)
        {
            switch (propertyModel.propertyType)
            {
                default:
                case PropertyType.None:
                    break;
                case PropertyType.Speed:
                    speedText.text = ((propertyModel.propertyValue > 0) ? "+" : "") + propertyModel.propertyValue.ToString("0.#");
                    break;
                case PropertyType.Handling:
                    handlingText.text = ((propertyModel.propertyValue > 0) ? "+" : "") + propertyModel.propertyValue.ToString("0.#");
                    break;
                case PropertyType.Acceleration:
                    accelerationText.text = ((propertyModel.propertyValue > 0) ? "+" : "") + propertyModel.propertyValue.ToString("0.#");
                    break;
                case PropertyType.Health:
                    healthText.text = ((propertyModel.propertyValue > 0) ? "+" : "") + propertyModel.propertyValue.ToString("0.#");
                    break;
            }
        }

        public void CheckProperties()
        {
            ResetAppearance();
            if (PerformFunctionality == PerformFunctionality.Yes)
                UpdateAppearance();
            var HoverOver = GetComponent<HoverOver>();
            if (HoverOver)
            {
                HoverOver.Initialize(this);
            }
        }

        protected override void OnVisible()
        {
            ResetAppearance();
            if (PerformFunctionality == PerformFunctionality.Yes)
                UpdateAppearance();
            var HoverOver = GetComponent<HoverOver>();
            if (HoverOver)
            {
                HoverOver.Initialize(this);
            }

            DoFloat();
            // if( Global.PlayerService.PlayerData.AutoVaultState.Exists(_card.CardName))
            // Global.PlayerService.PlayerData.AutoVaultState.Add(new AutoVaultState(card.CardName,utc));
            // if(_card.CardState == CardState.Stack)
            // {
            //     startTime = Convert.ToDateTime("08/31/2015 00:00:01");
            //     _timeUTC = DateTime.Parse(_card.TimeUTC);
            // }
        }

        protected override void OnDeinitialize()
        {
            base.OnDeinitialize();
        }

        protected override void OnHidden()
        {
            OnCloseCallBack = null;
        }

        private void Update()
        {
            if (!isInitialized || _card == null)
            {
                return;
            }
            
            if (_card.CardStateModel.cardState == CardState.Staking || _card.CardStateModel.cardState == CardState.CoolDown)
            {
                if (_card.StackedTimeString != "")
                {
                    TimeSpan timeRemaining = _card.StackedTime.Subtract(DateTime.UtcNow);
                    if (timeRemaining.TotalSeconds <= 0)
                    {
                        _card.StackedTime = default;
                        if (_card.CardStateModel.cardState == CardState.Staking)
                        {
                            _card.StakeCard();
                            Context.CardsManager.OnStatusChangeToStake();
                        }
                        else if (_card.CardStateModel.cardState == CardState.CoolDown)
                        {
                            _card.UnStakeCard();
                        }
                    }
                    else
                    {
                        int hours = (int)timeRemaining.TotalHours; // truncate partial hours
                        int minutes = timeRemaining.Minutes;
                        int seconds = timeRemaining.Seconds;

                        string niceTime = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

                        dateTimeText.text = niceTime;
                    }
                }
            }

            if (IsVisible)
            {
                EnableStakeUI();
            }
        }


        private void CardButtonPressed()
        {
            PlaySound(cardClickAS);
            if (performFunctionality == PerformFunctionality.No)
                return;
            switch (cardCallFrom)
            {
                case CardCallFrom.Presets:
                    if (_card.CardStateModel.cardState == CardState.None || _card.CardStateModel.cardState == CardState.Equip)
                    {
                        var dialog1 = (Context.UI as MenuUI).Open<UICardInfoDialogView>(_card, CardInfoDialogType.PropertiesView, cardCallFrom, defaultEquipButtonText, defaultUnEquipButtonText);
                        dialog1.HasClosed += OnDialogClosedCallBack;
                    }

                    break;
                case CardCallFrom.Vault:
                    if (_card.IsInitialized && (_card.CardStateModel.cardState == CardState.None || _card.CardStateModel.cardState == CardState.Staked || _card.CardStateModel.cardState == CardState.Applied))
                    {
                        var dialog2 = (Context.UI as MenuUI).Open<UICardInfoDialogView>(_card, CardInfoDialogType.PropertiesView, cardCallFrom, defaultEquipButtonText, defaultUnEquipButtonText);
                        dialog2.HasClosed += OnDialogClosedCallBack;
                    }

                    break;
                case CardCallFrom.MyCard:
                    var dialog = (Context.UI as MenuUI).Open<UICardInfoDialogView>(_card, CardInfoDialogType.PropertiesView, cardCallFrom, defaultEquipButtonText, defaultUnEquipButtonText);
                    dialog.HasClosed += OnDialogClosedCallBack;
                    break;
                case CardCallFrom.CardMerge:
                    if (_card.IsInitialized)
                    {
                        if (_card.CardStateModel.cardState == CardState.None)
                        {
                            OnDialogClosedCallBack(CardUseState.Equip);
                        }
                        else
                        {
                            OnDialogClosedCallBack(CardUseState.UnEquip);
                        }
                    }

                    break;
            }
        }

        void OnDialogClosedCallBack(CardUseState cardUseState)
        {
            switch (cardUseState)
            {
                case CardUseState.Equip:
                    switch (cardCallFrom)
                    {
                        case CardCallFrom.Presets:
                            break;
                        case CardCallFrom.Vault:
                            _card.ApplyCard();
                            break;
                        case CardCallFrom.MyCard:
                            break;
                        case CardCallFrom.CardMerge:
                            break;
                    }

                    break;
                case CardUseState.UnEquip:
                    switch (cardCallFrom)
                    {
                        case CardCallFrom.Presets:
                            break;
                        case CardCallFrom.Vault:
                            _card.UnapplyCard();
                            _card.StackedTime = DateTime.UtcNow.AddMinutes(Global.Settings.CardsSetting.UnstakingCoolDownTimeInMinutes); 
                            _card.CoolDownCard();
                            Context.CardsManager.OnStatusChangeToStake();
                            break;
                        case CardCallFrom.MyCard:
                            break;
                        case CardCallFrom.CardMerge:
                            break;
                    }

                    break;
                case CardUseState.Nothing:
                    break;
            }

            if (OnCloseCallBack != null)
            {
                OnCloseCallBack.Invoke(_card.Id, cardUseState);
            }

            if (PerformFunctionality == PerformFunctionality.Yes)
                UpdateAppearance();
        }

        private void ResetAppearance()
        {
            overlayContainer.SetActive(false);
            foreach (var eq in equipUIBehaviour)
            {
                eq.SetActive(false);
            }

            stacktimebarUI.SetActive(false);
            stackTextUI.SetActive(false);

            foreach (var merge in MergeUIBehaviour)
            {
                merge.SetActive(false);
            }
        }

        public void UpdateAppearance()
        {
            switch (cardCallFrom)
            {
                case CardCallFrom.CardMerge:
                case CardCallFrom.Vault:
                case CardCallFrom.Presets:
                    overlayContainer.SetActive(_card.CardStateModel.cardState != CardState.None);
                    foreach (var eq in equipUIBehaviour)
                    {
                        eq.SetActive(_card.CardStateModel.cardState == CardState.Equip);
                    }

                    foreach (var merge in MergeUIBehaviour)
                    {
                        merge.SetActive(_card.CardStateModel.cardState == CardState.Merge);
                    }

                    break;
                case CardCallFrom.MyCard:
                    overlayContainer.SetActive(false);
                    foreach (var eq in equipUIBehaviour)
                    {
                        eq.SetActive(false);
                    }

                    stacktimebarUI.SetActive(false);
                    stackTextUI.SetActive(false);

                    foreach (var merge in MergeUIBehaviour)
                    {
                        merge.SetActive(false);
                    }

                    break;
            }
        }

        private void EnableStakeUI()
        {
            switch (Card.CardStateModel.cardState)
            {
                case CardState.None:
                    AppliedTextUI.SetActive(false);
                    stackTextUI.SetActive(false);
                    stacktimebarUI.SetActive(false);
                    break;
                case CardState.Staking:
                    AppliedTextUI.SetActive(false);
                    stackTextUI.SetActive(false);
                    stacktimebarUI.SetActive(true);
                    break;
                case CardState.Staked:
                    AppliedTextUI.SetActive(false);
                    stackTextUI.SetActive(true);
                    stacktimebarUI.SetActive(false);
                    break;
                case CardState.Applied:
                    AppliedTextUI.SetActive(true);
                    stackTextUI.SetActive(false);
                    stacktimebarUI.SetActive(false);
                    break;
                case CardState.CoolDown:
                    AppliedTextUI.SetActive(false);
                    stackTextUI.SetActive(false);
                    stacktimebarUI.SetActive(true);
                    break;
                default:
                    break;
            }
        }
        public void DoFloat()
        {
            if(canFloatCard == PerformFunctionality.No)
                return;
            RectTransform.DOAnchorPosY(136, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }

        public void ShowFront()
        {
            if(canRotateCard == PerformFunctionality.No)
                return;
            RectTransform.DOScaleX(0, .3f).SetEase(Ease.Linear).OnComplete((() =>
            {
                cardFrontUIBehaviour.SetActive(true);
                cardBackUIBehaviour.SetActive(false);
                RectTransform.DOScaleX(2, .3f);
            }));
        }

        public void ShowBack()
        {
            if(canRotateCard == PerformFunctionality.No)
                return;
            
            RectTransform.DOScaleX(0, .3f).SetEase(Ease.Linear).OnComplete((() =>
            {
                cardFrontUIBehaviour.SetActive(false);
                cardBackUIBehaviour.SetActive(true);
                RectTransform.DOScaleX(2, .3f);
            }));
        }
        
    }
}