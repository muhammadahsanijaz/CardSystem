using MoonKart.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{
    public class CardsManager : GameService
    {

        public Action<Card, int, bool> CarSetupUpdated;
        public List<Card> Cards => _cards;
        public PacksLibrary PacksLibrary => _packsLibrary;
        public AutoVaultDataModel AutoVaultDataModel => _autoVaultDataModel;

        private List<Card> _cards;
        private List<CardHandler> _cardHandler;
        private List<CardHandler> _cardHandlerForMerging;

        private CardsFactory _cardsfactory;
        private CardFilters _cardFilters;

        private PacksLibrary _packsLibrary;
        private AutoVaultDataModel _autoVaultDataModel;
        public Action<int,int, TimeSpan> FuelCountUpdate;
        
        #region CardCreation

        protected override void OnInitialize()
        {
            base.OnInitialize();

            InitializeCardList();

        }

        protected override void OnDeinitialize()
        {
            foreach (var cardHandler in _cardHandler)
            {
                cardHandler.Deinitialize();
            }

            _cards = null;
            _cardFilters = null;

            base.OnDeinitialize();
        }

        private void InitializeCardList()
        {
            _cardsfactory = new CardsFactory();
            _cards = new List<Card>();
            _cardHandler = new List<CardHandler>();
            _packsLibrary = new PacksLibrary();
            _packsLibrary.CreateData();
            InitializeVaultData();
            PopulateCards();
            _cardFilters = new CardFilters(this, Context.UI as MenuUI, transform);
            OnStatusChangeToStake();
            InvokeRepeating(nameof(PerSecondTick), 0, 1);
            
        }

   


        public CardHandler CreateCardHandler(Transform cardHolder, Card card)
        {
            var info = Global.Settings.CardsSetting.GetCardCell(card);
            var carHandler = Instantiate(info, cardHolder);
            carHandler.Initialize(card);
            _cardHandler.Add(carHandler);

            return carHandler;
        }

        public CardHandler CreateCardHandlerForMerging(Transform cardHolder, Card card)
        {
            var info = Global.Settings.CardsSetting.GetCardCell(card);
            var carHandler = Instantiate(info, cardHolder);
            carHandler.Initialize(card);
            var handler = carHandler.GetComponent<CardHandler>();
            handler.performFunctionality = PerformFunctionality.No;
            handler.CheckProperties();
            carHandler.name = card.CardName;

            return carHandler;
        }

        public void CreateNewCardHandler(Card card)
        {
            _cardFilters.CreateNewCardHandler(this, card, Context.UI as MenuUI, transform);
        }


        public void PopulateCards()
        {
            foreach (var cardsStates in Global.Settings.CardsLibrary.CardStates)
            {
                AddCardInCardManager(cardsStates);
            }
        }

        public Card AddCardInCardManager(CardsStateModel cardsStates)
        {
            CardTemplateModel cardModel = cardsStates.assignedModel; //Global.Settings.CardsModels.GetCardModel(cardsStates.assignedModel);

            Card card;
            switch (cardModel.CardType)
            {
                default:
                case CardType.Vehicle:

                    card = _cardsfactory.CreateVehicleCard(cardsStates.cardId, cardModel);
                    card.SetCardLevel(cardsStates);
                    var vehicleProperities = Context.Settings.VehicleDataModel.GetData(card.CardRarity, card.CardStateModel.cardLevel);
                    (card as VehicleCard).UpdateLevel(vehicleProperities);
                    break;
                case CardType.Driver:
                    card = _cardsfactory.CreateDriverCard(cardsStates.cardId, cardModel);
                    card.SetCardLevel(cardsStates);
                    var driverProperities = Context.Settings.DriverDataModel.GetData(card.CardRarity, card.CardStateModel.cardLevel);
                    (card as DriverCard).UpdateLevel(driverProperities);
                    break;
                case CardType.Tech:
                    card = _cardsfactory.CreateTechCard(cardsStates.cardId, cardModel);
                    card.SetCardLevel(cardsStates);
                    var techCard = card as TechCard;
                    var techProperities = Context.Settings.EngineDataModel.GetData(card.CardRarity, card.CardStateModel.cardLevel);
                    switch (techCard.MyTechType)
                    {
                        case TechCard.TechType.Engine:
                            techProperities = Context.Settings.EngineDataModel.GetData(card.CardRarity, card.CardStateModel.cardLevel);
                            break;
                        case TechCard.TechType.Wheels:
                            techProperities = Context.Settings.WheelsDataModel.GetData(card.CardRarity, card.CardStateModel.cardLevel);
                            break;
                        case TechCard.TechType.BodyKit:
                            techProperities = Context.Settings.BodyTechDataModel.GetData(card.CardRarity, card.CardStateModel.cardLevel);
                            break;
                        case TechCard.TechType.GearBox:
                            techProperities = Context.Settings.GearBoxDataModal.GetData(card.CardRarity, card.CardStateModel.cardLevel);
                            break;
                    }

                    techCard.UpdateLevel(techProperities);
                    break;
                case CardType.Powerup:
                    card = _cardsfactory.CreatePowerupCard(cardsStates.cardId, cardModel);
                    card.SetCardLevel(cardsStates);
                    var powerupCard = card as PowerupCard;
                    var powerupProperities = Context.Settings.PowerUpDataModel.GetData(powerupCard.MyPowerupType, powerupCard.CardRarity, powerupCard.CardStateModel.cardLevel);
                    powerupCard.UpdateLevel(powerupProperities);
                    break;
                case CardType.Accessories:
                    card = _cardsfactory.CreateAccessoriesCard(cardsStates.cardId, cardModel);
                    card.SetCardLevel(cardsStates);
                    var accessoriesCard = card as AccessoriesCard;
                    var accessoriesProperities = Context.Settings.GearBoxDataModal.GetData(card.CardRarity, card.CardStateModel.cardLevel);
                    accessoriesCard.UpdateLevel(accessoriesProperities);
                    break;
                case CardType.Fuel:
                    card = _cardsfactory.CreateFuelCard(cardsStates.cardId, cardModel);
                     (card as FuelCard).SetFuelLevel(cardsStates);
                    break;
            }

            card.SetIcon(Global.Settings.CardsSetting.GetIcon(card));

            _cards.Add(card);
            return card;
        }

        

        internal void DestoryCards(List<Card> destoryingCardList)
        {
            foreach (var card in destoryingCardList)
            {
                _cardFilters.RemoveCardHandler(card);
                if (Global.Settings.CardsLibrary.CardStates.Contains(card.CardStateModel))
                {
                    Global.Settings.CardsLibrary.CardStates.Remove(card.CardStateModel);
                }

                if (Cards.Contains(card))
                {
                    Cards.Remove(card);
                }
            }
        }

        internal void MergeCard(Card upgradeCard)
        {
            upgradeCard.CardStateModel.cardLevel++;

            if (upgradeCard is VehicleCard)
            {
                var vehicleProperities = Context.Settings.VehicleDataModel.GetData(upgradeCard.CardRarity, upgradeCard.CardStateModel.cardLevel);
                (upgradeCard as VehicleCard).UpdateLevel(vehicleProperities);
            }
            else if (upgradeCard is DriverCard)
            {
                var driverProperities = Context.Settings.DriverDataModel.GetData(upgradeCard.CardRarity, upgradeCard.CardStateModel.cardLevel);
                (upgradeCard as DriverCard).UpdateLevel(driverProperities);
            }
            else if (upgradeCard is TechCard)
            {
                var techCard = upgradeCard as TechCard;
                var techProperities = Context.Settings.EngineDataModel.GetData(upgradeCard.CardRarity, upgradeCard.CardStateModel.cardLevel);
                switch (techCard.MyTechType)
                {
                    case TechCard.TechType.Engine:
                        techProperities = Context.Settings.EngineDataModel.GetData(upgradeCard.CardRarity, upgradeCard.CardStateModel.cardLevel);
                        break;
                    case TechCard.TechType.Wheels:
                        techProperities = Context.Settings.WheelsDataModel.GetData(upgradeCard.CardRarity, upgradeCard.CardStateModel.cardLevel);
                        break;
                    case TechCard.TechType.BodyKit:
                        techProperities = Context.Settings.BodyTechDataModel.GetData(upgradeCard.CardRarity, upgradeCard.CardStateModel.cardLevel);
                        break;
                    case TechCard.TechType.GearBox:
                        techProperities = Context.Settings.GearBoxDataModal.GetData(upgradeCard.CardRarity, upgradeCard.CardStateModel.cardLevel);
                        break;
                }

                techCard.UpdateLevel(techProperities);
            }
        }

        #endregion

        #region Fuel
        void InitializeVaultData()
        {
            _autoVaultDataModel = new AutoVaultDataModel();
            _autoVaultDataModel.CreateData();

            if (_autoVaultDataModel.autoVaultData.TotalFuelTimeInString == "")
            {
                _autoVaultDataModel.autoVaultData.TotalFuelTime = new TimeSpan(0, Global.Settings.CardsSetting.MaxFuelProduceTimeInMinutes, 0);
                _autoVaultDataModel.autoVaultData.CurrentFuelTime = new TimeSpan(0, 0, 0);
                _autoVaultDataModel.autoVaultData.LastTimeFuelGenerated = DateTime.UtcNow;
                _autoVaultDataModel.SaveData();
            }

            var timeDifference = DateTime.UtcNow.Subtract(_autoVaultDataModel.autoVaultData.LastTimeFuelGenerated);
            while (_autoVaultDataModel.autoVaultData.totalFuel < Global.Settings.CardsSetting.TotalFuel && timeDifference > _autoVaultDataModel.autoVaultData.TotalFuelTime)
            {
                _autoVaultDataModel.autoVaultData.totalFuel++;
                timeDifference = timeDifference - _autoVaultDataModel.autoVaultData.TotalFuelTime;
            }
        }


        private void PerSecondTick()
        {
            if (_autoVaultDataModel.autoVaultData.CurrentFuelTimeInString != "")
            {

                TimeSpan timeRemaining = _autoVaultDataModel.autoVaultData.TotalFuelTime.Subtract(_autoVaultDataModel.autoVaultData.CurrentFuelTime);
                //Debug.LogError("currentFuelTime " + timeRemaining.ToString());
                if (timeRemaining.TotalSeconds <= 0)
                {
                    if (_autoVaultDataModel.autoVaultData.totalFuel < Global.Settings.CardsSetting.TotalFuel)
                    {
                        _autoVaultDataModel.autoVaultData.totalFuel++;
                        FuelCountUpdate?.Invoke(_autoVaultDataModel.autoVaultData.totalFuel, _autoVaultDataModel.autoVaultData.FuelUnits, _autoVaultDataModel.autoVaultData.TotalFuelTime);
                    }
                    _autoVaultDataModel.autoVaultData.LastTimeFuelGenerated = DateTime.UtcNow;
                    _autoVaultDataModel.SaveData();
                    _autoVaultDataModel.autoVaultData.CurrentFuelTime = new TimeSpan(0, 0, 0);
                }
                else
                {
                    _autoVaultDataModel.autoVaultData.CurrentFuelTime = new TimeSpan(0, 0, (int)(_autoVaultDataModel.autoVaultData.CurrentFuelTime.TotalSeconds + 1));
                }
            }
            else
            {
                _autoVaultDataModel.autoVaultData.CurrentFuelTimeInString = _autoVaultDataModel.autoVaultData.TotalFuelTimeInString;
            }
        }

        public void OnStatusChangeToStake()
        {
            int fuelUnits = 0;
            foreach (var slotData in _autoVaultDataModel.autoVaultData.slotsData)
            {
                if (slotData.slotIndex != -1)
                {
                    Card card = GetCardById(slotData.cardId);
                    if (card.CardStateModel.cardState == CardState.Staked)
                    {
                        fuelUnits += ((int)card.CardRarity + 1 * (int)card.CardCategory + 1);
                    }
                    _autoVaultDataModel.autoVaultData.FuelUnits = fuelUnits;
                    FuelCountUpdate?.Invoke(_autoVaultDataModel.autoVaultData.totalFuel, _autoVaultDataModel.autoVaultData.FuelUnits, _autoVaultDataModel.autoVaultData.TotalFuelTime);
                }
            }
            var timeDiffInSec = (Global.Settings.CardsSetting.MaxFuelProduceTimeInMinutes - Global.Settings.CardsSetting.MinFuelProduceTimeInMinutes) * 60;
            int timeInSecond = timeDiffInSec * fuelUnits / Global.Settings.CardsSetting.MaxFuelUnit;
            _autoVaultDataModel.autoVaultData.TotalFuelTime = new TimeSpan(0, 0, Global.Settings.CardsSetting.MaxFuelProduceTimeInMinutes * 60 - timeInSecond);
            _autoVaultDataModel.SaveData();
        }

        public bool CheckForTheFuelOnRaceStart()
        {
            if (_autoVaultDataModel.autoVaultData.totalFuel >= Global.Settings.CardsSetting.FuelRequiredToPlayGame)
            {
                _autoVaultDataModel.autoVaultData.totalFuel -= Global.Settings.CardsSetting.FuelRequiredToPlayGame;
                _autoVaultDataModel.SaveData();
                return true;
            }
            else
            {
                return false;
            }
        }
        internal void ConsumeFuelCard(Card card)
        {
            var FuelCard = card as FuelCard;
            if (_autoVaultDataModel.autoVaultData.totalFuel < Global.Settings.CardsSetting.TotalFuel)
            {
                _autoVaultDataModel.autoVaultData.totalFuel += FuelCard.CardStateModel.cardLevel;
                FuelCountUpdate?.Invoke(_autoVaultDataModel.autoVaultData.totalFuel, _autoVaultDataModel.autoVaultData.FuelUnits, _autoVaultDataModel.autoVaultData.TotalFuelTime);
            }

            _cardFilters.RemoveCardHandler(card);

            if (Cards.Contains(card))
            {
                Cards.Remove(card);
            }
        }
        #endregion

        #region CardSorting

        public Card GetCardById(int cardId)
        {
            return _cardsfactory.GetCardById(_cards, cardId);
        }

        public Card GetCardByName(string cardname)
        {
            return _cardsfactory.GetCardByName(_cards, cardname);
        }

        public Card GetCardByState(List<Card> cards, CardState state)
        {
            return _cardsfactory.GetCardByState(cards, state);
        }

        public List<Card> GetAllCardByTemplateId(List<Card> cards, int templateId)
        {
            return _cardsfactory.GetCardListByTemplateId(cards, templateId);
        }

        public List<Card> GetAllCardByLevel(List<Card> cards, int level)
        {
            return _cardsfactory.GetCardListByLevel(cards, level);
        }

        public List<Card> GetAllCardByState(List<Card> cards, CardState state)
        {
            return _cardsfactory.GetCardListByState(cards, state);
        }

        public List<T> GetCardsByTypeOf<T>(List<T> cards, Type cardType)
        {
            return _cardsfactory.GetCardsOfType(cards, cardType);
        }

        public T GetFirstCardsByTypeOf<T>(List<T> cards, Type cardType)
        {
            return _cardsfactory.GetFirstCardOfType(cards, cardType);
        }

        public AccessoriesCard GetFirstAccessoriesCardsByTypeOf(List<Card> cards, AccessoriesCard.AccessoriesType cardType)
        {
            return _cardsfactory.GetFirstAccessoriesCardsByTypeOf(cards, cardType);
        }


        #endregion

        #region CardFilters

        public List<CardHandler> GetAllSortedCards(List<CardHandler> cards)
        {
            return _cardFilters.GetAllSortedCards(cards);
        }

        internal void EnableCardHandlersWithRarity(CardSearchData cardSearchData, Action<int, CardUseState> OnCloseCallBack, CardCallFrom cardCallFrom, string equipButtonName = "Equip",
            string unEquipButtonName = "UnEquip", float cardSize = 0.9f)
        {
            var cards = Context.CardsManager._cardFilters.ProcessFilters(cardSearchData);
            var cardHandlers = _cardFilters.GetCardHandlers(cards);
            cardHandlers = _cardFilters.GetSortedCardsByRarity(cardHandlers);
            _cardFilters.EnableCardHandlers(cardHandlers, OnCloseCallBack, cardCallFrom, equipButtonName, unEquipButtonName, cardSize);
        }

        //public List<Card> GetSortedCardsByLevel(List<Card> cards)
        //{
        //    return _cardFilters.GetSortedCardsByLevel(cards);
        //}

        public void InitializeCardHandlers(SlotsActionData slotsActionData, Transform containerTransform)
        {
            _cardFilters.InitializeCurrentCards(Context.CardsManager, slotsActionData, containerTransform); // apply type filters here
        }       
        public int GetTotalCard(SlotsActionData slotsActionData)
        {
            return _cardFilters.GetTotalCard(Context.CardsManager, slotsActionData); // apply type filters here
        }


        public void DeinitializeCardHandlers()
        {
            _cardFilters.ChangeAllCardHandlerParentsTo(transform);
            _cardFilters.DisableAllCardHandlers();
        }

        internal void DisableAllCardHandlers()
        {
            _cardFilters.DisableAllCardHandlers();
        }

        internal void ClearCardHandlerList()
        {
            _cardFilters.ClearCardHandlerList();
        }

        internal string GetCardNameAccordingTheType(Type cardType)
        {
            
            return _cardFilters.GetCardNameAccordingTheType(cardType);
        }

        internal CardHandler GetCardHandler(Card card)
        {
            return _cardFilters.GetCardHandler(card);
        }

        #endregion
    }
}