using MoonKart.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{
    public class CardsManager : GameService
    {
        public List<Card> Cards => _cards;
        public CardFilters CardFilters => _cardFilters;

        private List<Card> _cards;
        private List<CardHandler> _cardHandler;
        private List<CardHandler> _cardHandlerForMerging;

        private CardsFactory _cardsfactory;
        private CardFilters _cardFilters;

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

            PopulateCards();
            _cardFilters = new CardFilters(this, Context.UI as MenuUI, transform);
        }

        public CardHandler CreateCardHandler(Transform cardHolder, Card card)
        {
            var info = Global.Settings.CardsSetting.GetCardCell(card.CardCategory, card.CardRarity);
            var carHandler = Instantiate(info, cardHolder);
            carHandler.Initialize(card);
            carHandler.name = card.CardName;
            _cardHandler.Add(carHandler);

            return carHandler;
        }
        
        public CardHandler CreateCardHandlerForMerging(Transform cardHolder, Card card)
        {
            var info = Global.Settings.CardsSetting.GetCardCell(card.CardCategory, card.CardRarity);
            var carHandler = Instantiate(info, cardHolder);
            carHandler.Initialize(card);
            carHandler.name = card.CardName;
           // _cardHandlerForMerging.Add(carHandler);

            return carHandler;
        }


        public void PopulateCards()
        {
            int counter = 0;
            foreach (var cardsStates in Global.Settings.CardsLibrary.CardStates)
            {

                CardTemplateModel cardModel = cardsStates.assignedModel;//Global.Settings.CardsModels.GetCardModel(cardsStates.assignedModel);
                //for (int i = 0; i < cardsStates.cardCount; i++)
                {
                    Card card;
                    switch (cardModel.CardType)
                    {
                        default:
                        case CardType.Vehicle:

                            card = _cardsfactory.CreateVehicleCard(cardsStates.cardId,cardModel);
                            card.SetCardLevel(cardsStates);
                            var vehicleProperities = Context.Settings.VehicleDataModel.GetData(card.CardRarity,card.CardStateModel.cardLevel);
                            (card as VehicleCard).UpdateLevel(vehicleProperities);
                            break;
                        case CardType.Driver:
                            card = _cardsfactory.CreateDriverCard(cardsStates.cardId,cardModel);
                            card.SetCardLevel(cardsStates);
                            var driverProperities = Context.Settings.DriverDataModel.GetData(card.CardRarity, card.CardStateModel.cardLevel);
                            (card as DriverCard).UpdateLevel(driverProperities);
                            break;
                        case CardType.Tech:
                            card = _cardsfactory.CreateTechCard(cardsStates.cardId,cardModel);
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
                            card = _cardsfactory.CreatePowerupCard(cardsStates.cardId,cardModel);
                            card.SetCardLevel(cardsStates);
                            break;
                        case CardType.Accessories:
                            card = _cardsfactory.CreateAccessoriesCard(cardsStates.cardId,cardModel);
                            card.SetCardLevel(cardsStates);
                            break;
                    }

                    card.SetIcon(Global.Settings.CardsSetting.GetIcon(card));

                    _cards.Add(card);
                }
            }
        }


        internal void DestoryCards(List<Card> destoryingCardList)
        {
            Debug.LogError("before " + Cards.Count + "-" + Global.Settings.CardsLibrary.CardStates.Count);
            bool done1 = false;
            bool done2 = false;
            foreach (var card in destoryingCardList)
            {

                _cardFilters.RemoveCardHandler(card);
                if (Global.Settings.CardsLibrary.CardStates.Contains(card.CardStateModel))
                {
                    done2 = Global.Settings.CardsLibrary.CardStates.Remove(card.CardStateModel);
                }
                else
                {
                    Debug.LogError("not found 1");
                }
                if (Cards.Contains(card))
                {
                    done1 = Cards.Remove(card);
                }
                else
                {
                    Debug.LogError("not found 2");
                }
            }
            Debug.LogError("after " + Cards.Count + "-" +Global.Settings.CardsLibrary.CardStates.Count + "-" + done1 + "-" + done2);
        }

        internal void MergeCard(Card upgradeCard)
        {
            upgradeCard.CardStateModel.cardLevel++;

            if(upgradeCard is VehicleCard)
            {
                var vehicleProperities = Context.Settings.VehicleDataModel.GetData(upgradeCard.CardRarity, upgradeCard.CardStateModel.cardLevel);
                (upgradeCard as VehicleCard).UpdateLevel(vehicleProperities);
            }
            else if(upgradeCard is DriverCard)
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

        public List<Card> GetSortedCardsByTypeOf(List<Card> cards)
        {
            return _cardsfactory.GetSortedCardsByTypeOf(cards);
        }

        public List<Card> GetSortedCardsByRarity(List<Card> cards)
        {
            return _cardsfactory.GetSortedCardsByRarity(cards);
        }

        public List<Card> GetSortedCardsByLevel(List<Card> cards)
        {
            return _cardsfactory.GetSortedCardsByLevel(cards);
        }

        #endregion

        #region CardFilters

        public void InitializeCardHandlers(SlotsActionData slotsActionData, Transform containerTransform)
        {
            _cardFilters.InitializeCurrentCards(Context.CardsManager, slotsActionData, containerTransform); // apply type filters here
        }


        public void DeinitializeCardHandlers()
        {
            _cardFilters.ChangeAllCardHandlerParentsTo(transform);
            _cardFilters.DisableAllCardHandlers();
        }

        #endregion
    }
}