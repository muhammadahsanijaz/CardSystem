using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoonKart
{
    [System.Serializable]
    public class GameEndReward
    {
        public float common;
        public float rare;
        public float epic;
        public float legendary;
        public int numberOfCard;
    }
    
    [System.Serializable]
    public class PacksTemplateModel
    {
        public int packId;
        public string packCategory;
        public float common;
        public float rare;
        public float epic;
        public float legendary;
        public int numberOfCard;
        public int price;
        public Sprite card;
        public Sprite titleText;
        public Sprite flare;
        public Sprite headerBg;
        public PackOpeningSprites PackOpeningSprites;
    }

    [System.Serializable]
    public class PackOpeningSprites
    {
        public Sprite mainPacksSprite;
        public Sprite topLsSprite;
        public Sprite topM1Sprite;
        public Sprite topM2Sprite;
        public Sprite topRisSprite;
        public Sprite midLsSprite;
        public Sprite midM1Sprite;
        public Sprite midM2Sprite;
        public Sprite btmLSprite;
        public Sprite btmM1Sprite;
        public Sprite btmM2Sprite;
        public Sprite btmRsSprite;

        public Sprite cracksSprite;
        public Color crackColor = Color.white;
        
        public Sprite centerGlowSprite;
        public Color centerGlowColor = Color.white;
        
        public Sprite outerGlowSprite;
        public Color outerGlowColor = Color.white;
    }


    [CreateAssetMenu(fileName = "PacksDataModel", menuName = "Settings/Packs Data Model")]
    public class PacksDataModel : ScriptableObject
    {
        public List<PacksTemplateModel> packs;
        public GameEndReward gameEndReward;
        
        public List<CardTemplateModel> GetCard(PacksTemplateModel packModel)
        {
            List<CardTemplateModel> cardTemplateModels = new List<CardTemplateModel>();
            for (int i = 0; i < packModel.numberOfCard; i++)
            {
                float percentage = Random.Range(0, ((packModel.common )  + packModel.rare + packModel.epic + packModel.legendary));

                float commonPercentage = packModel.common;
                //float fuelPercentage = packModel.common*2;
                float rarePercentage = (packModel.common ) + packModel.rare;
                float epicPercentage = (packModel.common) + packModel.rare + packModel.epic;
                float legendaryPercentage = (packModel.common) + packModel.rare + packModel.epic + packModel.legendary;

                if (percentage <= commonPercentage)
                {
                    cardTemplateModels.Add(Global.Settings.CardsLibrary.GetRandomCardModel(CardRarity.Common));
                }
                // else if (percentage > commonPercentage && percentage <= fuelPercentage)
                // {
                //     cardTemplateModels.Add(Global.Settings.CardsLibrary.GetFuelCardModel());
                // }
                else if (percentage > commonPercentage && percentage <= rarePercentage)
                {
                    cardTemplateModels.Add(Global.Settings.CardsLibrary.GetRandomCardModel(CardRarity.Rare));
                }
                else if (percentage > rarePercentage && percentage <= epicPercentage)
                {
                    cardTemplateModels.Add(Global.Settings.CardsLibrary.GetRandomCardModel(CardRarity.Epic));
                }
                else if (percentage > epicPercentage && percentage <= legendaryPercentage)
                {
                    cardTemplateModels.Add(Global.Settings.CardsLibrary.GetRandomCardModel(CardRarity.Legendary));
                }
            }

            return cardTemplateModels;
        }
        
        public List<CardTemplateModel> GetGameEndRewardedCards()
        {
            List<CardTemplateModel> cardTemplateModels = new List<CardTemplateModel>();
            
            for (int i = 0; i < gameEndReward.numberOfCard; i++)
            {
                float percentage = Random.Range(0, (gameEndReward.common + gameEndReward.rare + gameEndReward.epic + gameEndReward.legendary));

                float commonPercentage = gameEndReward.common;
                float rarePercentage = gameEndReward.common + gameEndReward.rare;
                float epicPercentage = gameEndReward.common + gameEndReward.rare + gameEndReward.epic;
                float legendaryPercentage = gameEndReward.common + gameEndReward.rare + gameEndReward.epic + gameEndReward.legendary;

                if (percentage <= commonPercentage)
                {
                    cardTemplateModels.Add(Global.Settings.CardsLibrary.GetRandomCardModel(CardRarity.Common));
                }
                else if (percentage > commonPercentage && percentage <= rarePercentage)
                {
                    cardTemplateModels.Add(Global.Settings.CardsLibrary.GetRandomCardModel(CardRarity.Rare));
                }
                else if (percentage > rarePercentage && percentage <= epicPercentage)
                {
                    cardTemplateModels.Add(Global.Settings.CardsLibrary.GetRandomCardModel(CardRarity.Epic));
                }
                else if (percentage > epicPercentage && percentage <= legendaryPercentage)
                {
                    cardTemplateModels.Add(Global.Settings.CardsLibrary.GetRandomCardModel(CardRarity.Legendary));
                }
            }

            return cardTemplateModels;
        }
    }
}