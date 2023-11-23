using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MoonKart.UI
{
    public class CardHandler : UIWidget
    {
        public Card Card
        {
            private set => _card = value;
            get => _card;
        }

        private Card _card;

        [SerializeField] private PerformFunctionality PerformFunctionality;
        [SerializeField] private TextMeshProUGUI notificationText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI speedText;
        [SerializeField] private TextMeshProUGUI handlingText;
        [SerializeField] private TextMeshProUGUI accelerationText;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private Image mainIcon;
        [SerializeField] private UIBehaviour[] equipUIBehaviour;
        [SerializeField] private UIBehaviour stackTextUI;
        [SerializeField] private UIBehaviour stacktimebarUI;
        [SerializeField] private UIBehaviour[] MergeUIBehaviour;
        [SerializeField] private TextMeshProUGUI dateTimeText;
        [SerializeField] private Button button;
        [SerializeField] private GameObject overlayContainer;

        public Button Button => button;

        public string defaultEquipButtonText { get; set; }
        public string defaultUnEquipButtonText { get; set; }
        public CardCallFrom callFromVault { get; set; }

        public Action<int, CardUseState> OnCloseCallBack;

        public void Initialize(Card card)
        {
            callFromVault = CardCallFrom.MyCard;
            base.OnInitialize();
            _card = card;
            RefreshCardHandler();
        }

        public void RefreshCardHandler() 
        {
            if (_card == null)
                return;

            if (PerformFunctionality == PerformFunctionality.Yes)
                button.onClick.AddListener(CardButtonPressed);

            nameText.SetTextSafe(_card.CardName);
            notificationText.SetTextSafe(_card.CardStateModel.cardLevel.ToString());


            speedText.text = "-";
            handlingText.text = "-";
            accelerationText.text = "-";
            healthText.text = "-";
            if (_card is VehicleCard)
            {
                var vehicleCard = _card as VehicleCard;
                speedText.text = vehicleCard.VehicleStats.Speed.ToString("00");
                handlingText.text = vehicleCard.VehicleStats.Handling.ToString("00");
                accelerationText.text = vehicleCard.VehicleStats.Acceleration.ToString("00");
                healthText.text = vehicleCard.VehicleStats.Health.ToString("00");
            }
            else if (_card is DriverCard)
            {
                var driverCard = _card as DriverCard;
                speedText.text = "+" + driverCard.VehicleStats.Speed.ToString();
                handlingText.text = "+" + driverCard.VehicleStats.Handling.ToString();
                accelerationText.text = "+" + driverCard.VehicleStats.Acceleration.ToString();
                healthText.text = "+" + driverCard.VehicleStats.Health.ToString();
            }
            else if (_card is PropertyCard)
            {
                var propertyCard = _card as PropertyCard;
                var ecard = _card as PropertyCard;


                SetSliderValueAccordingToProperty(ecard.PrimaryProperty);
                SetSliderValueAccordingToProperty(ecard.SecondaryProperty);
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
                    speedText.text = "+" + propertyModel.propertyValue.ToString();
                    break;
                case PropertyType.Handling:
                    handlingText.text = "+" + propertyModel.propertyValue.ToString();
                    break;
                case PropertyType.Acceleration:
                    accelerationText.text = "+" + propertyModel.propertyValue.ToString();
                    break;
                case PropertyType.Health:
                    healthText.text = "+" + propertyModel.propertyValue.ToString();
                    break;
            }
        }

        protected override void OnVisible()
        {
            ResetAppearance();
            if (PerformFunctionality == PerformFunctionality.Yes)
                UpdateAppearance();

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
            if (_card.CardStateModel.cardState == CardState.Staking)
            {
                if (Card.StackedTimeString != "")
                {
                    TimeSpan timeRemaining = Card.StackedTime.Subtract(DateTime.UtcNow);
                    if (timeRemaining.TotalSeconds <= 0)
                    {
                        _card.CardStateModel.cardState = CardState.Staked;
                        EnableStakeUI(true);
                    }
                    else
                    {
                        EnableStakeUI(false);
                        int hours = (int)timeRemaining.TotalHours; // truncate partial hours
                        int minutes = timeRemaining.Minutes;
                        int seconds = timeRemaining.Seconds;

                        string niceTime = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

                        dateTimeText.text = niceTime;
                    }
                    
                }
            }
            else if(_card.CardStateModel.cardState == CardState.Staked)
            {
                EnableStakeUI(true);
            }
        }


        private void CardButtonPressed()
        {
            switch (callFromVault)
            {
                case CardCallFrom.Presets:
                    if (Card.CardStateModel.cardState == CardState.None || Card.CardStateModel.cardState == CardState.Equip)
                    {
                        var dialog1 = (Context.UI as MenuUI).Open<UICardInfoDialogView>(_card, CardInfoDialogType.PropertiesView, callFromVault, defaultEquipButtonText, defaultUnEquipButtonText);
                        dialog1.HasClosed += OnDialogClosedCallBack;
                    }
                    break;
                case CardCallFrom.Vault:
                    if (Card.CardStateModel.cardState == CardState.None || Card.CardStateModel.cardState == CardState.Staked)
                    {
                        var dialog2 = (Context.UI as MenuUI).Open<UICardInfoDialogView>(_card, CardInfoDialogType.PropertiesView, callFromVault, defaultEquipButtonText, defaultUnEquipButtonText);
                        dialog2.HasClosed += OnDialogClosedCallBack;
                    }
                    break;
                case CardCallFrom.MyCard:
                    var dialog = (Context.UI as MenuUI).Open<UICardInfoDialogView>(_card, CardInfoDialogType.PropertiesView, callFromVault, defaultEquipButtonText, defaultUnEquipButtonText);
                    dialog.HasClosed += OnDialogClosedCallBack;
                    break;
                case CardCallFrom.CardMerge:
                    if (Card.CardStateModel.cardState == CardState.None)
                    {
                        OnDialogClosedCallBack(CardUseState.Equip);
                    }
                    else
                    {
                        OnDialogClosedCallBack(CardUseState.UnEquip);
                    }
                    break;
            
            }
        }

        void OnDialogClosedCallBack(CardUseState cardUseState)
        {
            if (OnCloseCallBack != null)
            {
                OnCloseCallBack.Invoke(_card.Id, cardUseState);
            }

            switch (cardUseState)
            {
                case CardUseState.Equip:
                    switch (callFromVault)
                    {
                        case CardCallFrom.Presets:
                            break;
                        case CardCallFrom.Vault:
                            _card.StackCard();
                            break;
                        case CardCallFrom.MyCard:
                            break;
                        case CardCallFrom.CardMerge:
                            break;
                    }

                    break;
                case CardUseState.UnEquip:
                    switch (callFromVault)
                    {
                        case CardCallFrom.Presets:
                            break;
                        case CardCallFrom.Vault:
                            _card.UnStackCard();

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
            switch (callFromVault)
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

        private void EnableStakeUI(bool flag)
        {
            if (flag)
            {
                stackTextUI.SetActive(true);
                stacktimebarUI.SetActive(false);
            }
            else
            {
                stackTextUI.SetActive(false);
                stacktimebarUI.SetActive(true);
            }
        }
    }
}