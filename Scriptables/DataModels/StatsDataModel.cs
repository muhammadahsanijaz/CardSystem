using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{

    [System.Serializable]
    public class CardsStats
    {
        public int cardsRequired;
        public int goldCardsRequied;
    }

    [System.Serializable]
    public abstract class CardsStatsModel
    {
        public CardsStats cardsStats;
    }

    [System.Serializable]
    public class StatsTemplateModel<T>
    {
        public List<T> Base;
        public List<T> Rare;
        public List<T> Epic;
        public List<T> Legendary;

        public StatsTemplateModel()
        {
            Base = new List<T>();
            Rare = new List<T>();
            Epic = new List<T>();
            Legendary = new List<T>();
        }

        public T GetData(StatsTemplateModel<T> dataModel, CardRarity cardRarity, int level)
        {
            switch (cardRarity)
            {
                case CardRarity.Common:
                    return dataModel.Base[level - 1];
                case CardRarity.Rare:
                    return dataModel.Rare[level - 1];
                case CardRarity.Epic:
                    return dataModel.Epic[level - 1];
                case CardRarity.Legendary:
                    return dataModel.Legendary[level - 1];
            }
            return default;
        }

    }

    [System.Serializable]
    public class TechTemplateModel : CardsStatsModel
    {
        public PropertyModel primaryPropertyModel;
        public PropertyModel secondaryPropertyModel;
    }


}
