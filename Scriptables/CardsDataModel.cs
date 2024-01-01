using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MoonKart
{
    public enum CardType
    {
        Vehicle = 0,
        Driver = 1,
        Tech = 2,
        Powerup = 3,
        Accessories = 4,
        Fuel = 5,
    }

    public enum CardTypeFilter
    {
        All = -1,
        Vehicle = 0,
        Driver = 1,
        Tech = 2,
        Powerup = 3,
        Accessories = 4,
        Fuel = 5,
    }
    [System.Serializable]
    public class CardTemplateModel
    {
        public int templateId = -1;
        public string templateName;
        public string modelName;
        public string description;
        public int cardSubType;
        public float coolDownTime;
        public float duration;
        public bool powerupDefaultStateReady;
        public CardType CardType;
        public CardCategory cardCategory;
        public CardRarity cardRarity;
        public VehicleStats VehicleStats;
       // public PropertyModel PrimaryProperty;
       // public PropertyModel SecondaryProperty;

        public CardTemplateModel(CardTemplateModel cardTemplateModel)
        {
            this.templateName = cardTemplateModel.templateName;
            this.modelName = cardTemplateModel.modelName;
            this.description = cardTemplateModel.description;
            this.cardSubType = cardTemplateModel.cardSubType;
            this.coolDownTime = cardTemplateModel.coolDownTime;
            this.duration = cardTemplateModel.duration;
            this.CardType = cardTemplateModel.CardType;
            this.cardCategory = cardTemplateModel.cardCategory;
            this.cardRarity = cardTemplateModel.cardRarity;
            this.VehicleStats =  new VehicleStats(cardTemplateModel.VehicleStats);
           // this.PrimaryProperty = new PropertyModel(cardTemplateModel.PrimaryProperty);
          //  this.SecondaryProperty = new PropertyModel(cardTemplateModel.SecondaryProperty);
        }

        public CardTemplateModel(CardEditorModel cardEditorModel)
        {
            this.templateName = cardEditorModel.templateName;
            this.modelName = cardEditorModel.modelName;
            this.description = cardEditorModel.description;
            this.cardSubType = cardEditorModel.cardSubType;
            this.coolDownTime = cardEditorModel.coolDownTime;
            this.powerupDefaultStateReady = cardEditorModel.powerupDefaultStateReady;
            this.duration = cardEditorModel.duration;
            this.CardType = cardEditorModel.CardType;
            this.VehicleStats = new VehicleStats(cardEditorModel.VehicleStats);
          //  this.PrimaryProperty = new PropertyModel(cardEditorModel.PrimaryProperty);
          //  this.SecondaryProperty = new PropertyModel(cardEditorModel.SecondaryProperty);
        }
    }


    [System.Serializable]
    public class CardEditorModel
    {
        public int templateId = -1;
        public bool isNameDifferentWithRarity;
        public string templateName;
        public string[] templateNameWithRarity = new string[4];
        public string modelName;
        public string description;
        public int cardSubType;
        public float coolDownTime;
        public float duration;
        public bool powerupDefaultStateReady;
        public CardType CardType;
        public bool onlyRegularAndCommon;
        public VehicleStats VehicleStats;
       // public PropertyModel PrimaryProperty;
       // public PropertyModel SecondaryProperty;
        public CardEditorModel() { }
        public CardEditorModel(CardEditorModel cardEditorModel)
        {
            this.isNameDifferentWithRarity = cardEditorModel.isNameDifferentWithRarity;
            this.templateNameWithRarity = cardEditorModel.templateNameWithRarity;
            this.templateName = cardEditorModel.templateName;
            this.modelName = cardEditorModel.modelName;
            this.description = cardEditorModel.description;
            this.cardSubType = cardEditorModel.cardSubType;
            this.coolDownTime = cardEditorModel.coolDownTime;
            this.powerupDefaultStateReady = cardEditorModel.powerupDefaultStateReady;
            this.duration = cardEditorModel.duration;
            this.CardType = cardEditorModel.CardType;
            this.onlyRegularAndCommon = cardEditorModel.onlyRegularAndCommon;
            this.VehicleStats = new VehicleStats(cardEditorModel.VehicleStats);
           // this.PrimaryProperty = new PropertyModel(cardEditorModel.PrimaryProperty);
           // this.SecondaryProperty = new PropertyModel(cardEditorModel.SecondaryProperty);
        }
    }

    [CreateAssetMenu(fileName = "CardsDataModel", menuName = "Settings/Cards Data Model")]
    public class CardsDataModel : ScriptableObject
    {

        [HideInInspector] public bool toggleNativeEditor;
        [HideInInspector] public bool toggleCardModelEditor;
        [HideInInspector] public CardTypeFilter selectedCardType;

        public CardsSettings CardsSetting;

        [ListDrawerSettings(Expanded = true)]
        public List<CardEditorModel> cardModels;


        //public CardTemplateModel GetCardModel(int templateId)
        //{
        //    return cardModels.Find(x => x.templateId == templateId);
        //}
    }
}
