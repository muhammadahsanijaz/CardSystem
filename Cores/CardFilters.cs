using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quantum;
using Sirenix.OdinInspector;
using MoonKart.UI;

namespace MoonKart
{
    public enum PerformFunctionality
    {
        Yes,
        No
    }
    
    public enum CardCategoryFilter
    {
        All = -1,
        Regular = 0,
        GoldFoil = 1
    }

    public enum CardRarityFilter
    {
        All = -1,
        Common = 0,
        Rare = 1,
        Epic = 2,
        Legendary = 3,
    }

    [Serializable]
    public class SlotsActionData
    {
        public Type cardType;
        public int category;
        public SlotHandler interactionSlot;
    }

    [Serializable]
    public class CardSearchData
    {
        public CardCategoryFilter cardCategory;
        public CardRarityFilter cardRarity;
        public string searchString;
    }

    [Serializable]
    public class CardFitlersDataModel
    {
        public List<CardHandler> allCardHandlers;
        public List<Card> currentCards;
    }


    [Serializable]
    public class CardFilters
    {
        [SerializeField] private CardFitlersDataModel cardFilterDataModel = new CardFitlersDataModel();


        public CardFilters(CardsManager cardsController, MenuUI menuUI, Transform cardsContainerTransform)
        {
            cardFilterDataModel = new CardFitlersDataModel();
            cardFilterDataModel.allCardHandlers = new List<CardHandler>();
            CreateCardHandlers(cardsController, menuUI, cardsContainerTransform);
        }


        public void InitializeCurrentCards(CardsManager cardsManager,SlotsActionData cardFilterData, Transform containerTransform)
        {
            ChangeAllCardHandlerParentsTo(containerTransform);
            cardFilterDataModel.currentCards = ProcessCardHandlersByType(cardsManager, cardFilterData);
        }
   

        public void ChangeAllCardHandlerParentsTo(Transform containerTransform)
        {
            foreach (var cardhandler in cardFilterDataModel.allCardHandlers)
            {
                cardhandler.transform.SetParent(containerTransform);
            }
        }

        public List<Card> ProcessFilters(CardSearchData cardSearchData)
        {
            var cards = cardFilterDataModel.currentCards;
            cards = ShowOnCardCategory(cards, cardSearchData.cardCategory);
            cards = ShowCardRarity(cards, cardSearchData.cardRarity);
            cards = ShowCardSearch(cards, cardSearchData.searchString);
            
            return cards;
        }

        public string GetCardNameAccordingTheType(Type cardType)
        {
            if (cardType == typeof(VehicleCard))
            {
                return "Vehicles";
            }
            else if (cardType == typeof(PowerupCard))
            {
                return "Power Ups";
            }
            else if (cardType == typeof(TechCard))
            {
                return "Tech Cards";
            }
            else if (cardType == typeof(AccessoriesCard))
            {
                return "Accessories";
            }
            else if (cardType == typeof(DriverCard))
            {
                return "Drivers";
            }

            return "My Cards";
        }

        public List<Card> ShowOnCardCategory(List<Card> cards, CardCategoryFilter selectedCardCategory)
        {
            if (selectedCardCategory != CardCategoryFilter.All)
            {
                cards = cards.FindAll(x => (int)x.CardCategory == (int)selectedCardCategory);
                //List<Card> newcards = new List<Card>();
                //foreach (var card in cards)
                //{
                //    if (card.CardCategory == selectedCardCategory)
                //    {
                //        newcards.Add(card);
                //    }
                //}
                //return newcards;
            }

            return cards;
        }

        public List<Card> ShowCardRarity(List<Card> cards, CardRarityFilter selectedCardRarity)
        {
            if (selectedCardRarity != CardRarityFilter.All)
            {
                cards = cards.FindAll(x => (int)x.CardRarity == (int)selectedCardRarity);
            }

            return cards;
        }

        public List<Card> ShowCardSearch(List<Card> cards, string compareString)
        {
            if (!string.IsNullOrWhiteSpace(compareString) && compareString != "")
            {
                //cards = cards.FindAll(x => x.CardName.Contains(compareString));
                List<Card> newcards = new List<Card>();
                foreach (var card in cards)
                {
                    if (card.CardName.StartsWith(compareString, StringComparison.OrdinalIgnoreCase))
                    {
                        newcards.Add(card);
                    }
                }

                return newcards;
            }


            return cards;
        }

        public List<CardHandler> EnableCardHandlers(List<CardHandler> cardHandlers, Action<int, CardUseState> OnCloseCallBack, CardCallFrom cardCallFrom, string equipButtonName = "Equip", string unEquipButtonName = "UnEquip", float cardSize = 0.9f)
        {
            int siblingIndex = 0;
            foreach (var cardHandler in cardHandlers)
            {
                cardHandler.transform.SetSiblingIndex(siblingIndex);
                cardHandler.OnCloseCallBack = OnCloseCallBack;
                cardHandler.defaultEquipButtonText = equipButtonName;
                cardHandler.defaultUnEquipButtonText = unEquipButtonName;
                cardHandler.cardCallFrom = cardCallFrom;
                cardHandler.transform.localScale = new Vector3(cardSize, cardSize, cardSize);
                cardHandler.SetActive(true);
                cardHandler.Visible();
                if (cardCallFrom == CardCallFrom.Vault && cardCallFrom == CardCallFrom.CardMerge && !cardHandler.Card.IsInitialized)
                {
                    cardHandler.Hidden();
                    cardHandler.SetActive(false);
                }
                siblingIndex++;
            }

            return cardHandlers;
        }

        public void RefreshAllCardHandlers()
        {
            foreach (var cardHandler in cardFilterDataModel.allCardHandlers)
            {
                cardHandler.Hidden();
                cardHandler.SetActive(false);
            }
        }

        public void DisableAllCardHandlers()
        {
            foreach (var cardHandler in cardFilterDataModel.allCardHandlers)
            {
                cardHandler.Hidden();
                cardHandler.SetActive(false);
            }
        }

        internal void UpdateCardHandler(Card upgradeCard)
        {
            var cardHandler = cardFilterDataModel.allCardHandlers.Find(x => x.Card == upgradeCard);
            cardHandler.Card.CardStateModel.cardLevel = upgradeCard.CardStateModel.cardLevel;
        }

        private List<Card> GetTypeOfTechCard(List<Card> cards, TechCard.TechType currentTechType)
        {
            List<Card> newCards = new List<Card>(); 
            foreach (var card in cards)
            {
                if (card is TechCard)
                {
                    var techCard = card as TechCard;
                    if (techCard.MyTechType == currentTechType)
                    {
                        newCards.Add(card);
                    }
                }
            }
            return newCards;
        }

        internal void RemoveCardHandler(Card card)
        {
            var cardHandler = cardFilterDataModel.allCardHandlers.Find(x => x.Card == card);
            cardFilterDataModel.allCardHandlers.Remove(cardHandler);
            cardHandler.Deinitialize();
            GameObject.Destroy(cardHandler.gameObject);
        }

        private List<Card> GetTypeOfAccessoriesCard(List<Card> cards, AccessoriesCard.AccessoriesType currentAccessoriesType)
        {
            List<Card> newCards = new List<Card>();
            foreach (var card in cards)
            {
                if (card is AccessoriesCard)
                {
                    var techCard = card as AccessoriesCard;
                    if (techCard.MyAccessoriesType == currentAccessoriesType)
                    {
                        newCards.Add(card);
                    }
                }
            }
            return newCards;
        }

        private List<Card> GetTypeOfPowerupCard(List<Card> cards, PowerupType currentPowerupType)
        {
            List<Card> newCards = new List<Card>();
            foreach (var card in cards)
            {
                if (card is PowerupCard)
                {
                    var techCard = card as PowerupCard;
                    if (techCard.MyPowerupType == currentPowerupType)
                    {
                        newCards.Add(card);
                    }
                }
            }
            return newCards;
        }

        private void CreateCardHandlers(CardsManager cardsController, MenuUI menuUI, Transform cardsContainerTransform)
        {
            var cards = cardsController.Cards;
            foreach (var card in cards)
            {
                CreateNewCardHandler(cardsController, card, menuUI, cardsContainerTransform);
            }
        }

        public void CreateNewCardHandler(CardsManager cardsController,Card card, MenuUI menuUI, Transform cardsContainerTransform)
        {
            var cardHandler = cardsController.CreateCardHandler(cardsContainerTransform, card);
            cardHandler.Initialize(menuUI, cardHandler);
            cardHandler.Hidden();
            cardHandler.SetActive(false);
            cardFilterDataModel.allCardHandlers.Add(cardHandler);
        }


        private List<Card> ProcessCardHandlersByType(CardsManager cardsManager, SlotsActionData filterData)
        {
            var cards = cardsManager.GetCardsByTypeOf(cardsManager.Cards, filterData.cardType);

            if (filterData.cardType == typeof(Card))
            {
                cards = cardsManager.Cards;
            }

            if (filterData.category != -1)
            {
                if (filterData.cardType == typeof(TechCard))
                {
                    var techType = (TechCard.TechType)filterData.category;
                    cards = GetTypeOfTechCard(cards, techType);
                }
                else if (filterData.cardType == typeof(AccessoriesCard))
                {
                    var accessoriesType = (AccessoriesCard.AccessoriesType)filterData.category;
                    cards = GetTypeOfAccessoriesCard(cards, accessoriesType);
                }
                else if (filterData.cardType == typeof(PowerupCard))
                {
                    var powerupType = (PowerupType)filterData.category;
                    cards = GetTypeOfPowerupCard(cards, powerupType);
                }
            }

            // Apply Sorting here

            return cards;
        }
        
        public int GetTotalCard(CardsManager cardsManager, SlotsActionData filterData)
        {
            var cards = cardsManager.GetCardsByTypeOf(cardsManager.Cards, filterData.cardType);

            if (filterData.cardType == typeof(Card))
            {
                cards = cardsManager.Cards;
            }

            if (filterData.category != -1)
            {
                if (filterData.cardType == typeof(TechCard))
                {
                    var techType = (TechCard.TechType)filterData.category;
                    cards = GetTypeOfTechCard(cards, techType);
                }
                else if (filterData.cardType == typeof(AccessoriesCard))
                {
                    var accessoriesType = (AccessoriesCard.AccessoriesType)filterData.category;
                    cards = GetTypeOfAccessoriesCard(cards, accessoriesType);
                }
                else if (filterData.cardType == typeof(PowerupCard))
                {
                    var powerupType = (PowerupType)filterData.category;
                    cards = GetTypeOfPowerupCard(cards, powerupType);
                }
            }

            // Apply Sorting here

            return cards.Count;
        }

        public void ClearCardHandlerList()
        {
            foreach (var cardHandler in cardFilterDataModel.allCardHandlers)
            {
                UnityEngine.Object.Destroy(cardHandler.gameObject);
            }

            cardFilterDataModel.allCardHandlers = new List<CardHandler>();
        }

        internal CardHandler GetCardHandler(Card card)
        {
            return cardFilterDataModel.allCardHandlers.Find(x => x.Card == card);
            
        }

        internal List<CardHandler> GetCardHandlers(List<Card> cards)
        {
            List<CardHandler> newCardHandlers = new List<CardHandler>();
            foreach (var cardHandler in cardFilterDataModel.allCardHandlers)
            {
                if (cards.Contains(cardHandler.Card))
                {
                    newCardHandlers.Add(cardHandler);
                }
            }
            return newCardHandlers;
        }

        // All Sorted Functions
        public List<CardHandler> GetAllSortedCards(List<CardHandler> cards)
        {
            List<CardHandler> sortedCards = new List<CardHandler>();
            sortedCards.AddRange(cards.FindAll(x => x.GetType() == typeof(VehicleCard)));
            sortedCards.AddRange(cards.FindAll(x => x.GetType() == typeof(DriverCard)));
            // Sorting Tech Cards
            List<CardHandler> techCards = cards.FindAll(x => x.GetType() == typeof(TechCard));
            string[] techTypeNames = Enum.GetNames(typeof(TechCard.TechType));
            foreach (var techTypeName in techTypeNames)
            {
                sortedCards.AddRange(techCards.FindAll(x => (x.Card as TechCard)?.MyTechType.ToString() == techTypeName));
            }
            // Sorting Powerup Cards
            List<CardHandler> powerupCards = cards.FindAll(x => x.GetType() == typeof(PowerupCard));
            string[] powerupTypeNames = Enum.GetNames(typeof(TechCard.TechType));
            foreach (var powerupTypeName in powerupTypeNames)
            {
                sortedCards.AddRange(powerupCards.FindAll(x => (x.Card as PowerupCard)?.MyPowerupType.ToString() == powerupTypeName));
            }
            // Sorting Accessories Cards
            List<CardHandler> accessoriesCards = cards.FindAll(x => x.GetType() == typeof(AccessoriesCard));
            string[] accessoriesTypeNames = Enum.GetNames(typeof(TechCard.TechType));
            foreach (var accessoriesTypeName in accessoriesTypeNames)
            {
                sortedCards.AddRange(accessoriesCards.FindAll(x => (x.Card as AccessoriesCard)?.MyAccessoriesType.ToString() == accessoriesTypeName));
            }
            return sortedCards;
        }

        public List<CardHandler> GetSortedCardsByRarity(List<CardHandler> cards)
        {
            List<CardHandler> newCards = new List<CardHandler>();
            newCards.AddRange(cards.FindAll(x => x.Card.CardRarity == CardRarity.Legendary));
            newCards.AddRange(cards.FindAll(x => x.Card.CardRarity == CardRarity.Epic));
            newCards.AddRange(cards.FindAll(x => x.Card.CardRarity == CardRarity.Rare));
            newCards.AddRange(cards.FindAll(x => x.Card.CardRarity == CardRarity.Common));
            return newCards;
        }

        public List<CardHandler> GetCardsByRarity(List<CardHandler> cards, CardRarity cardRarity)
        {
            List<CardHandler> newCards = new List<CardHandler>();
            newCards.AddRange(cards.FindAll(x => x.Card.CardRarity == cardRarity));
            return newCards;
        }

        public List<CardHandler> GetSortedCardsByLevel(List<CardHandler> cards)
        {
            List<CardHandler> sortedCards = new List<CardHandler>();

            for (int i = 1; i <= 10; i++)
            {
                sortedCards.AddRange(GetSortedCardsByRarity(cards.FindAll(x => x.Card.CardStateModel.cardLevel == i)));
            }

            return sortedCards;
        }

    }
}