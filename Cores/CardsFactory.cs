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

        public FuelCard CreateFuelCard(int id, CardTemplateModel cardTemplateModel)
        {
            return new FuelCard(id, cardTemplateModel);
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

        public AccessoriesCard GetFirstAccessoriesCardsByTypeOf(List<Card> cards, AccessoriesCard.AccessoriesType subtype)
        {
            foreach (var card in cards)
            {
                if (card.GetType() == typeof(AccessoriesCard))
                {
                    var AC = (AccessoriesCard)card;
                    if (AC.MyAccessoriesType == subtype)
                    {
                        return AC;
                    }
                }
            }
            return default;
        }

        #endregion
    }
}
