using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MoonKart;
using System;
using System.Linq;
using Quantum;

[CustomEditor(typeof(CardsLibrary))]
public class CardsLibraryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CardsLibrary cardsLibrary = (CardsLibrary)target;
        if (GUILayout.Button("Toggle Native Editor"))
        {
            cardsLibrary.toggleNativeEditor = !cardsLibrary.toggleNativeEditor;
        }


        if (cardsLibrary.toggleNativeEditor)
        {
            base.OnInspectorGUI();
            return;
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Select Cards Setting"))
        {
            Selection.activeObject = Resources.Load<CardsSettings>("Settings/CardsSettings");
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        //////////////////////////////////

        int templateId = 0;
        cardsLibrary.cardModels = new List<CardTemplateModel>();
        //for (int i = 0; i < cardsLibrary.CardsModel.cardModels.Count; i++)
        foreach (var cardEditorModel in cardsLibrary.CardsModel.cardModels)
        {
            var tempModel = new CardTemplateModel(cardEditorModel);
            tempModel.cardCategory = CardCategory.Regular;
            tempModel.cardRarity = CardRarity.Common;
            if (!cardEditorModel.onlyRegularAndCommon)
            {
                for (int i = 0; i < Enum.GetNames(typeof(CardCategory)).Length; i++)
                {
                    for (int j = 0; j < Enum.GetNames(typeof(CardRarity)).Length; j++)
                    {
                        var newCardTemplateModel = new CardTemplateModel(tempModel);
                        newCardTemplateModel.templateId = templateId;
                        newCardTemplateModel.cardCategory = (CardCategory)i;
                        newCardTemplateModel.cardRarity = (CardRarity)j;
                        if (cardEditorModel.isNameDifferentWithRarity)
                        {
                            newCardTemplateModel.modelName = cardEditorModel.templateNameWithRarity[j];
                        }
                        cardsLibrary.cardModels.Add(newCardTemplateModel);
                        templateId++;
                    }
                }
            }
            else
            {
                var newCardTemplateModel = new CardTemplateModel(tempModel);
                newCardTemplateModel.templateId = templateId;
                cardsLibrary.cardModels.Add(newCardTemplateModel);
                templateId++;
            }
        }

        /////////////////////////////////

        if (GUILayout.Button("Toggle Card Library"))
        {
            cardsLibrary.toggleCardLibrary = !cardsLibrary.toggleCardLibrary;
        }

        EditorGUILayout.Space();
        if (cardsLibrary.toggleCardLibrary)
        {
            SortedList<int,string> listOfTemplatesTitle = new SortedList<int, string>();
            //SortedList<int,string> listOfTemplatesNames = new SortedList<int, string>();
            if (cardsLibrary.CardStates == null)
                cardsLibrary.CardStates = new List<CardsStateModel>();

            if (cardsLibrary.cardModels != null)
            {
                for (int i = 0; i < cardsLibrary.cardModels.Count; i++)
                {
                    string subTypeName = "";
                    var item = cardsLibrary.cardModels[i];
                    switch (item.CardType)
                    {
                        default:
                        case CardType.Tech:
                            subTypeName = ((TechCard.TechType)item.cardSubType).ToString();
                            break;
                        case CardType.Powerup:
                            subTypeName = ((PowerupType)item.cardSubType).ToString();
                            break;
                        case CardType.Accessories:
                            subTypeName = ((AccessoriesCard.AccessoriesType)item.cardSubType).ToString();
                            break;
                        case CardType.Fuel:
                            subTypeName = item.cardSubType.ToString();
                            break;
                    }
                    listOfTemplatesTitle.Add(item.templateId, item.CardType.ToString() + "-" + item.templateName + "-" +  subTypeName + "-" + item.cardCategory.ToString() + "-" + item.cardRarity.ToString());
                }
            }

            for (int i = 0; i < cardsLibrary.CardStates.Count; i++)
            {
                if (cardsLibrary.CardStates[i].cardId == -1)
                {
                    cardsLibrary.CardStates[i].cardId = i;
                }

                EditorGUILayout.BeginHorizontal();
                int index = 0;
                EditorGUILayout.LabelField(cardsLibrary.CardStates[i].cardId.ToString(),GUILayout.Width(30));
                
                if (cardsLibrary.CardStates[i].assignedModel != null && cardsLibrary.CardStates[i].assignedModel.templateName != "")
                {
                    index = cardsLibrary.CardStates[i].assignedModel.templateId;// listOfTemplatesTitle.Keys.IndexOf(cardsLibrary.CardStates[i].assignedModel);
                }

                if (index < 0 || index > cardsLibrary.cardModels.Count)
                {
                    index = 0;
                }
                index = EditorGUILayout.Popup(index, listOfTemplatesTitle.Values.ToArray(), GUILayout.MinWidth(400));
                cardsLibrary.CardStates[i].assignedModel = cardsLibrary.GetCardModel(index);

                EditorGUILayout.TextField(cardsLibrary.CardStates[i].cardState.ToString());
                cardsLibrary.CardStates[i].cardLevel = EditorGUILayout.IntField(cardsLibrary.CardStates[i].cardLevel);
                if (GUILayout.Button("X", GUILayout.Width(30)))
                {
                    cardsLibrary.CardStates.RemoveAt(i);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Click to Add Quantity");
            
            if (GUILayout.Button("DUPLICATE LAST", GUILayout.Height(30)))
            {
                if (cardsLibrary.CardStates.Count != 0)
                {
                    cardsLibrary.CardStates.Add(new CardsStateModel
                    {
                        assignedModel = cardsLibrary.CardStates[cardsLibrary.CardStates.Count - 1].assignedModel,
                        cardLevel = 1,
                        cardState = 0,
                        cardId = cardsLibrary.CardStates[cardsLibrary.CardStates.Count - 1].cardId + 1,
                    });
                }
                else
                {
                    cardsLibrary.CardStates.Add(new CardsStateModel
                    {
                        assignedModel = cardsLibrary.cardModels[0],
                        cardLevel = 1,
                        cardState = 0,
                        cardId = 0,
                    });
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (cardsLibrary.cardModels != null)
            {
                for (int i = 0; i < cardsLibrary.cardModels.Count; i++)
                {
                    string subTypeName = "";
                    var item = cardsLibrary.cardModels[i];
                    switch (item.CardType)
                    {
                        default:
                        case CardType.Tech:
                            subTypeName = ((TechCard.TechType)item.cardSubType).ToString();
                            break;
                        case CardType.Powerup:
                            subTypeName = ((PowerupType)item.cardSubType).ToString();
                            break;
                        case CardType.Accessories:
                            subTypeName = ((AccessoriesCard.AccessoriesType)item.cardSubType).ToString();
                            break;
                        case CardType.Fuel:
                            subTypeName = item.cardSubType.ToString();
                            break;
                    }
                    var buttonName = item.CardType.ToString() + "-" + item.templateName + "-" + subTypeName + "-" + item.cardCategory.ToString() + "-" + item.cardRarity.ToString();
                    if (GUILayout.Button("ADD " + buttonName))
                    {
                        if (cardsLibrary.CardStates.Count != 0)
                        {
                            cardsLibrary.CardStates.Add(new CardsStateModel
                            {
                                assignedModel = item,
                                cardLevel = 1,
                                cardState = 0,
                                cardId = cardsLibrary.CardStates[cardsLibrary.CardStates.Count - 1].cardId + 1,
                            });
                        }
                        else
                        {
                            cardsLibrary.CardStates.Add(new CardsStateModel
                            {
                                assignedModel = cardsLibrary.cardModels[0],
                                cardLevel = 1,
                                cardState = 0,
                                cardId = 0,
                            });
                        }
                    }
                }
            }


            EditorGUILayout.Space();
            EditorGUILayout.Space();
            if (GUILayout.Button("Close Card Library"))
            {
                cardsLibrary.toggleCardLibrary = false;
            }


            EditorUtility.SetDirty(cardsLibrary);
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Save"))
        {
            cardsLibrary.SaveLibrary();
            EditorUtility.SetDirty(cardsLibrary);
        }

        if (GUILayout.Button("Load"))
        {
            cardsLibrary.RefreshLibrary();
            EditorUtility.SetDirty(cardsLibrary);
        }

        EditorGUILayout.EndHorizontal();
    }

    
}