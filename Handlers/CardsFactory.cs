using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoonKart {
    /// <summary>
    /// Contains All functions search card py type, handle card merging etc.
    /// Find Card by Id, CardType Parameter, TypeOf 
    /// </summary>
    public class CardsFactory
    {

        #region Create Cards

        public VehicleCard CreateVehicleCard(int id,CardTemplateModel cardTemplateModel)
        {
            return new VehicleCard(id, cardTemplateModel);
        }

        public DriverCard CreateDriverCard(int id,CardTemplateModel cardTemplateModel)
        {
            return new DriverCard(id, cardTemplateModel);
        }

        public TechCard CreateTechCard(int id,CardTemplateModel cardTemplateModel)
        {
            return new TechCard(id, cardTemplateModel);
        }

        public AccessoriesCard CreateAccessoriesCard(int id,CardTemplateModel cardTemplateModel)
        {
            return new AccessoriesCard(id, cardTemplateModel);
        }

        public PowerupCard CreatePowerupCard(int id,CardTemplateModel cardTemplateModel)
        {
            return new PowerupCard(id, cardTemplateModel);
        }
        #endregion

        #region FilterRegion
        // Get Individual Card
        public Card GetCardById(List<Card> cards, int cardId)
        {
            return cards.Find(x => x.Id == cardId);
        }

        public Card GetCardByName(List<Card> cards, string cardName)
        {
            foreach (var card in cards)
            {
                if(card.CardName == cardName)
                {
                    return card;
                }
            }
            return null;
        }
        public List<Card> GetCardListByTemplateId(List<Card> cards, int TemplateId)
        {
            List<Card> cardList = new List<Card>();
            foreach (var card in cards)
            {
                if(card.CardTemplateModel.templateId == TemplateId)
                {
                     cardList.Add(card);
                }
            }
            return cardList;
        }

        public List<Card> GetCardListByLevel(List<Card> cards, int level)
        {
            List<Card> cardList = new List<Card>();
            foreach (var card in cards)
            {
                if (card.CardStateModel.cardLevel == level)
                {
                    cardList.Add(card);
                }
            }
            return cardList;
        }

        public List<Card> GetCardListByState(List<Card> cards, CardState state)
        {
            List<Card> cardList = new List<Card>();
            foreach (var card in cards)
            {
                if (card.CardStateModel.cardState == state)
                {
                    cardList.Add(card);
                }
            }
            return cardList;
        }

        public Card GetCardByState(List<Card> cards, CardState state)
        {
            foreach (var card in cards)
            {
                if (card.CardStateModel.cardState == state)
                {
                    return card;
                }
            }
            return null;
        }

        // Get card of specific type
        public List<T> GetCardsOfType<T>(List<T> cards, Type cardType)
        {
            var newCards = new List<T>();
            foreach (var card in cards)
            {
                if ( card.GetType() == cardType)
                {
                    newCards.Add(card);
                }
            }

            return newCards;
        }

        // Get card of specific type
        public T GetFirstCardOfType<T>(List<T> cards, Type cardType)
        {
            foreach (var card in cards)
            {
                if (card.GetType() == cardType)
                {
                    return card;
                }
            }
            return default;
        }

        // All Sorted Functions
        public List<Card> GetSortedCardsByTypeOf(List<Card> cards)
        {
            List<Card> sortedCards = new List<Card>();
            sortedCards.AddRange(cards.FindAll(x => x.GetType() == typeof(VehicleCard)));
            sortedCards.AddRange(cards.FindAll(x => x.GetType() == typeof(DriverCard)));
            // Sorting Tech Cards
            List<Card> techCards = cards.FindAll(x => x.GetType() == typeof(TechCard));
            string[] techTypeNames = Enum.GetNames(typeof(TechCard.TechType));
            foreach (var techTypeName in techTypeNames)
            {
                sortedCards.AddRange(techCards.FindAll(x => (x as TechCard)?.MyTechType.ToString() == techTypeName));
            }
            // Sorting Powerup Cards
            List<Card> powerupCards = cards.FindAll(x => x.GetType() == typeof(PowerupCard));
            string[] powerupTypeNames = Enum.GetNames(typeof(TechCard.TechType));
            foreach (var powerupTypeName in powerupTypeNames)
            {
                sortedCards.AddRange(powerupCards.FindAll(x => (x as PowerupCard)?.MyPowerupType.ToString() == powerupTypeName));
            }
            // Sorting Accessories Cards
            List<Card> accessoriesCards = cards.FindAll(x => x.GetType() == typeof(AccessoriesCard));
            string[] accessoriesTypeNames = Enum.GetNames(typeof(TechCard.TechType));
            foreach (var accessoriesTypeName in accessoriesTypeNames)
            {
                sortedCards.AddRange(accessoriesCards.FindAll(x => (x as AccessoriesCard)?.MyAccessoriesType.ToString() == accessoriesTypeName));
            }
            return sortedCards;
        }

        public List<Card> GetSortedCardsByRarity(List<Card> cards)
        {
            List<Card> newCards = new List<Card>();
            newCards.AddRange(cards.FindAll(x => x.CardRarity == CardRarity.Legendary));
            newCards.AddRange(cards.FindAll(x => x.CardRarity == CardRarity.Epic));
            newCards.AddRange(cards.FindAll(x => x.CardRarity == CardRarity.Rare));
            newCards.AddRange(cards.FindAll(x => x.CardRarity == CardRarity.Common));
            return newCards;
        }

        public List<Card> GetSortedCardsByRarity(List<Card> cards, CardRarity cardRarity)
        {
            List<Card> newCards = new List<Card>();
            newCards.AddRange(cards.FindAll(x => x.CardRarity == cardRarity));
            return newCards;
        }

        public List<Card> GetSortedCardsByLevel(List<Card> cards)
        {
            List<Card> sortedCards = new List<Card>();

            for (int i = 1; i <= 10; i++)
            {
                sortedCards.AddRange(GetSortedCardsByRarity(cards.FindAll(x => x.CardStateModel.cardLevel == i)));
            }

            return sortedCards;
        }

        #endregion
    }
}
