using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoonKart
{

    
    [System.Serializable]
    public class CardsStateModel
    {
        [FormerlySerializedAs("id")] public int cardId = -1;
        public CardState cardState;
        public int cardLevel = 1;
        public CardTemplateModel assignedModel;
    }

    [System.Serializable]
    [CreateAssetMenu(fileName = "CardsLibrary", menuName = "Settings/Cards Library")]
    public class CardsLibrary : ScriptableObject
    {

        [HideInInspector] public bool toggleNativeEditor;
        [HideInInspector] public bool toggleCardLibrary;


       
        public CardsDataModel CardsModel {
            get => _cardsModel;
            set
            {
                _cardsModel = value;
            }
        }

        public List<CardsStateModel> CardStates
        {
            get => _cardsStates;
            set
            {
                _cardsStates = value;
            }
        }

        [SerializeField]
        private CardsDataModel _cardsModel;

        [ListDrawerSettings(Expanded = true)]
        public List<CardTemplateModel> cardModels;

        [SerializeField][ListDrawerSettings(Expanded = true)]
        private List<CardsStateModel> _cardsStates;



        public void RefreshLibrary()
        {
            LoadDataModel();
            LoadCardsLibrary();
        }

        public void SaveLibrary()
        {
            SaveDataModel();
            SaveCardsLibrary();
        }

        [ContextMenu("ResetAllCardStates")]
        public  void ResetAllCardStatestAll()
        {
            PlayerPrefs.DeleteAll();
            foreach (var state in _cardsStates)
            {
                state.cardState = CardState.None;
                state.cardLevel = 1;
            }
            Resources.Load<CarSettings>("Settings/CarSettings").UnloadAssets();
        }

        public CardTemplateModel GetFirstCardModel(CardType cardType)
        {
            return cardModels.Find(x => x.CardType == cardType);
        }

        public CardTemplateModel GetRandomCardModel(CardRarity cardRarity)
        {
            List<CardTemplateModel> CardTemplateModel = cardModels.FindAll(x => x.cardRarity == cardRarity);
            return CardTemplateModel[Random.Range(0, CardTemplateModel.Count)];
        }
        
        // public CardTemplateModel GetFuelCardModel()
        // {
        //     List<CardTemplateModel> CardTemplateModel = cardModels.FindAll(x => x.CardType == CardType.Fuel 7);
        //     return CardTemplateModel[Random.Range(0, CardTemplateModel.Count)];
        // }

        internal CardTemplateModel GetCardModel(string assignedModel)
        {
            return cardModels.Find(x => x.templateName == assignedModel);
        }

        public CardTemplateModel GetCardModel(int templateId)
        {
            return cardModels.Find(x => x.templateId == templateId);
        }


        void SaveDataModel()
        {
            PersistentStorage.SetObjectWithNewtonsoftJson("CardDataModel", _cardsModel.cardModels, true);
        }

        void SaveCardsLibrary()
        {
            PersistentStorage.SetObjectWithNewtonsoftJson("CardsLibrary", _cardsStates, true);
        }

        void LoadDataModel()
        {
            _cardsModel.cardModels = PersistentStorage.GetObjectWithNewtonsoftJson("CardDataModel", _cardsModel.cardModels);
        }

        void LoadCardsLibrary()
        {
            CardStates = PersistentStorage.GetObjectWithNewtonsoftJson("CardsLibrary", CardStates);
        }

        internal CardsStateModel GetStateModel(int cardId)
        {
           return _cardsStates.Find(x => x.cardId == cardId);
        }
    }
}
