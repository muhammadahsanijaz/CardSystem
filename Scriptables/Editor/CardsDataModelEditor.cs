using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MoonKart;
using System;
using System.Linq;
using Quantum;

[CustomEditor(typeof(CardsDataModel))]
public class CardsDataModelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CardsDataModel cardsDataModel = (CardsDataModel)target;
        if (GUILayout.Button("Toggle Native Editor"))
        {
            cardsDataModel.toggleNativeEditor = !cardsDataModel.toggleNativeEditor;
        }


        if (cardsDataModel.toggleNativeEditor)
        {
            base.OnInspectorGUI();
            return;
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Toggle Card Model Editor"))
        {
            cardsDataModel.toggleCardModelEditor = !cardsDataModel.toggleCardModelEditor;
        }


        EditorGUILayout.Space();

        if (cardsDataModel.toggleCardModelEditor)
        {
            EditorGUILayout.BeginHorizontal();
            var listOfCardTypeFilter = Enum.GetNames(typeof(CardTypeFilter));
            foreach (var item in listOfCardTypeFilter)
            {
                if (cardsDataModel.selectedCardType.ToString() == item)
                {
                    if (GUILayout.Button(item, GUILayout.Height(30)))
                    {
                        cardsDataModel.selectedCardType = (CardTypeFilter)Enum.Parse(typeof(CardTypeFilter), item);
                    }
                }
                else
                {
                    if (GUILayout.Button(item))
                    {
                        cardsDataModel.selectedCardType = (CardTypeFilter)Enum.Parse(typeof(CardTypeFilter), item);
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (cardsDataModel.cardModels == null)
            {
                cardsDataModel.cardModels = new List<CardEditorModel>();
            }

            for (int i = 0; i < cardsDataModel.cardModels.Count; i++)
            {

                cardsDataModel.cardModels[i].templateId = i;

                if (cardsDataModel.selectedCardType != CardTypeFilter.All &&
                    cardsDataModel.cardModels[i].CardType != (CardType)cardsDataModel.selectedCardType)
                {
                    continue;
                }


                EditorGUILayout.LabelField("Model Index : " + (cardsDataModel.cardModels[i].templateId + 1));

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("is Name Different With Rarity");
                cardsDataModel.cardModels[i].isNameDifferentWithRarity = EditorGUILayout.Toggle(cardsDataModel.cardModels[i].isNameDifferentWithRarity);
                EditorGUILayout.EndHorizontal();

                if (cardsDataModel.cardModels[i].isNameDifferentWithRarity)
                {
                    if(cardsDataModel.cardModels[i].templateNameWithRarity.Length == 0)
                    {
                        cardsDataModel.cardModels[i].templateNameWithRarity = new string[4];
                    }
                    for (int k = 0; k < cardsDataModel.cardModels[i].templateNameWithRarity.Length; k++)
                    {
                        cardsDataModel.cardModels[i].templateNameWithRarity[k] = EditorGUILayout.TextField("Card Template Name", cardsDataModel.cardModels[i].templateNameWithRarity[k]);
                    }
                }
                else
                {
                    cardsDataModel.cardModels[i].templateName = EditorGUILayout.TextField("Card Template Name", cardsDataModel.cardModels[i].templateName);
                }

                if (cardsDataModel.cardModels[i].CardType == CardType.Vehicle &&
                    cardsDataModel.CardsSetting.cardsData.modelContainer.vehicles != null &&
                    cardsDataModel.CardsSetting.cardsData.modelContainer.vehicles.Length > 0)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("3DModel");
                    List<string> listOfModels = GetListOfName(cardsDataModel.CardsSetting.cardsData.modelContainer.vehicles);
                    int modelIndex = 0;
                    if (listOfModels.Contains(cardsDataModel.cardModels[i].modelName))
                    {
                        modelIndex = listOfModels.IndexOf(cardsDataModel.cardModels[i].modelName);
                    }

                    cardsDataModel.cardModels[i].modelName = listOfModels[EditorGUILayout.Popup(modelIndex, listOfModels.ToArray())];
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    if (cardsDataModel.cardModels[i].VehicleStats == null)
                    {
                        cardsDataModel.cardModels[i].VehicleStats = new VehicleStats(0,0,0,0);
                    }

                    cardsDataModel.cardModels[i].VehicleStats.Speed = EditorGUILayout.FloatField("Speed", cardsDataModel.cardModels[i].VehicleStats.Speed);
                    cardsDataModel.cardModels[i].VehicleStats.Handling = EditorGUILayout.FloatField("Handling", cardsDataModel.cardModels[i].VehicleStats.Handling);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    cardsDataModel.cardModels[i].VehicleStats.Health = EditorGUILayout.FloatField("Health", cardsDataModel.cardModels[i].VehicleStats.Health);
                    cardsDataModel.cardModels[i].VehicleStats.Acceleration = EditorGUILayout.FloatField("Acceleration", cardsDataModel.cardModels[i].VehicleStats.Acceleration);
                    EditorGUILayout.EndHorizontal();
                }
                else if (cardsDataModel.cardModels[i].CardType == CardType.Driver &&
                         cardsDataModel.CardsSetting.cardsData.modelContainer.drivers != null &&
                         cardsDataModel.CardsSetting.cardsData.modelContainer.drivers.Length > 0)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("3DModel");
                    List<string> listOfModels = GetListOfName(cardsDataModel.CardsSetting.cardsData.modelContainer.drivers);
                    int modelIndex = 0;
                    if (listOfModels.Contains(cardsDataModel.cardModels[i].modelName))
                    {
                        modelIndex = listOfModels.IndexOf(cardsDataModel.cardModels[i].modelName);
                    }

                    cardsDataModel.cardModels[i].modelName = listOfModels[EditorGUILayout.Popup(modelIndex, listOfModels.ToArray())];
                    EditorGUILayout.EndHorizontal();
                    if (cardsDataModel.cardModels[i].VehicleStats == null)
                    {
                        cardsDataModel.cardModels[i].VehicleStats = new VehicleStats(0,0,0,0);
                    }

                    EditorGUILayout.BeginHorizontal();
                    cardsDataModel.cardModels[i].VehicleStats.Speed = EditorGUILayout.FloatField("Speed", cardsDataModel.cardModels[i].VehicleStats.Speed);
                    cardsDataModel.cardModels[i].VehicleStats.Handling = EditorGUILayout.FloatField("Handling", cardsDataModel.cardModels[i].VehicleStats.Handling);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    cardsDataModel.cardModels[i].VehicleStats.Health = EditorGUILayout.FloatField("Health", cardsDataModel.cardModels[i].VehicleStats.Health);
                    cardsDataModel.cardModels[i].VehicleStats.Acceleration = EditorGUILayout.FloatField("Acceleration", cardsDataModel.cardModels[i].VehicleStats.Acceleration);
                    EditorGUILayout.EndHorizontal();
                }
                else if (cardsDataModel.cardModels[i].CardType == CardType.Accessories)
                {
                    switch ((AccessoriesCard.AccessoriesType)cardsDataModel.cardModels[i].cardSubType)
                    {
                        case AccessoriesCard.AccessoriesType.Antennas:
                            EditorGUILayout.BeginHorizontal();
                            List<string> listOfAntennaModels = GetListOfName(cardsDataModel.CardsSetting.cardsData.modelContainer.antenna);
                            EditorGUILayout.LabelField("3DModel");
                            cardsDataModel.cardModels[i].modelName = GetModelName(cardsDataModel.cardModels[i], listOfAntennaModels);
                            EditorGUILayout.EndHorizontal();
                            break;

                        case AccessoriesCard.AccessoriesType.Exhaust:
                            EditorGUILayout.BeginHorizontal();
                            List<string> listOfExhaustModels = GetListOfName(cardsDataModel.CardsSetting.cardsData.modelContainer.exhaust);
                            EditorGUILayout.LabelField("3DModel");
                            cardsDataModel.cardModels[i].modelName = GetModelName(cardsDataModel.cardModels[i], listOfExhaustModels);
                            EditorGUILayout.EndHorizontal();
                            break;

                        case AccessoriesCard.AccessoriesType.Rim:
                            EditorGUILayout.BeginHorizontal();
                            List<string> listOfRimModels = GetListOfName(cardsDataModel.CardsSetting.cardsData.modelContainer.rim);
                            EditorGUILayout.LabelField("3DModel");
                            cardsDataModel.cardModels[i].modelName = GetModelName(cardsDataModel.cardModels[i], listOfRimModels);
                            EditorGUILayout.EndHorizontal();
                            break;
                    }
                    
                }

                cardsDataModel.cardModels[i].description = EditorGUILayout.TextField("Card Description", cardsDataModel.cardModels[i].description);
                EditorGUILayout.BeginHorizontal();
                List<string> listOfType = Enum.GetNames(typeof(CardType)).ToList();
                int index = (int)cardsDataModel.cardModels[i].CardType;
                cardsDataModel.cardModels[i].CardType = (CardType)EditorGUILayout.Popup(index, listOfType.ToArray());

                EditorGUILayout.LabelField("Only Regular And Common");
                cardsDataModel.cardModels[i].onlyRegularAndCommon = EditorGUILayout.Toggle(cardsDataModel.cardModels[i].onlyRegularAndCommon);

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();

                switch (cardsDataModel.cardModels[i].CardType)
                {
                    case CardType.Tech:
                        listOfType = Enum.GetNames(typeof(TechCard.TechType)).ToList();
                        index = cardsDataModel.cardModels[i].cardSubType;
                        cardsDataModel.cardModels[i].cardSubType = EditorGUILayout.Popup(index, listOfType.ToArray());
                        break;
                    case CardType.Powerup:
                        listOfType = Enum.GetNames(typeof(PowerupType)).ToList();
                        index = cardsDataModel.cardModels[i].cardSubType;
                        cardsDataModel.cardModels[i].cardSubType = EditorGUILayout.Popup(index, listOfType.ToArray());
                        break;
                    case CardType.Accessories:
                        listOfType = Enum.GetNames(typeof(AccessoriesCard.AccessoriesType)).ToList();
                        index = cardsDataModel.cardModels[i].cardSubType;
                        cardsDataModel.cardModels[i].cardSubType = EditorGUILayout.Popup(index, listOfType.ToArray());
                        break;
                    case CardType.Fuel:
                       // listOfType = Enum.GetNames(typeof(AccessoriesCard.AccessoriesType)).ToList();
                       // index = cardsDataModel.cardModels[i].cardSubType;
                        cardsDataModel.cardModels[i].cardSubType = EditorGUILayout.IntField( "Fuel Count ",cardsDataModel.cardModels[i].cardSubType);
                        break;
                }

                EditorGUILayout.EndHorizontal();

                switch (cardsDataModel.cardModels[i].CardType)
                {
                    case CardType.Powerup:

                        //EditorGUILayout.BeginHorizontal();
                        //EditorGUILayout.LabelField("Primary", GUILayout.Width(100));
                        //ProcessProperty(cardsDataModel.cardModels[i].PrimaryProperty);
                        //EditorGUILayout.LabelField("Secondary", GUILayout.Width(100));
                        //ProcessProperty(cardsDataModel.cardModels[i].SecondaryProperty);
                        //EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        cardsDataModel.cardModels[i].coolDownTime = EditorGUILayout.FloatField("CoolDown Time", cardsDataModel.cardModels[i].coolDownTime);
                        cardsDataModel.cardModels[i].duration = EditorGUILayout.FloatField("Duration", cardsDataModel.cardModels[i].duration);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("powerup Default State Ready");
                        cardsDataModel.cardModels[i].powerupDefaultStateReady = EditorGUILayout.Toggle(cardsDataModel.cardModels[i].powerupDefaultStateReady);
                        EditorGUILayout.EndHorizontal();
                        break;
                    case CardType.Accessories:
                    case CardType.Tech:
                        //EditorGUILayout.BeginHorizontal();
                        //EditorGUILayout.LabelField("Primary", GUILayout.Width(100));
                        //ProcessProperty(cardsDataModel.cardModels[i].PrimaryProperty);
                        //EditorGUILayout.LabelField("Secondary", GUILayout.Width(100));
                        //ProcessProperty(cardsDataModel.cardModels[i].SecondaryProperty);
                        //EditorGUILayout.EndHorizontal();
                        break;
                }


                EditorGUILayout.Space();
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    cardsDataModel.cardModels.RemoveAt(i);
                }
            }


            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Click to Add Model");
            if (GUILayout.Button("Add", GUILayout.Height(30)))
            {
                var cardmodelTemplate = new CardEditorModel();
                if (cardsDataModel.cardModels.Count > 0)
                    cardmodelTemplate = new CardEditorModel(cardsDataModel.cardModels[cardsDataModel.cardModels.Count - 1]);
                if (cardsDataModel.selectedCardType != CardTypeFilter.All)
                {
                    cardmodelTemplate.CardType = (CardType)cardsDataModel.selectedCardType;
                }

                cardsDataModel.cardModels.Add(cardmodelTemplate);
            }

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Select Cards Data Models"))
            {
                Selection.activeObject = Resources.Load<CardsDataModel>("Settings/CardsDataModel");
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("Close Model Editor"))
            {
                cardsDataModel.toggleCardModelEditor = false;
            }

            EditorUtility.SetDirty(cardsDataModel);
        }
    }

    private string GetModelName(CardEditorModel cardEditorModel, List<string> listOfModelName)
    {
        int modelIndex = 0;
        if (listOfModelName.Contains(cardEditorModel.modelName))
        {
            modelIndex = listOfModelName.IndexOf(cardEditorModel.modelName);
        }

        return listOfModelName[EditorGUILayout.Popup(modelIndex, listOfModelName.ToArray())];
    }

    private List<string> GetListOfName(GameObject[] list)
    {
        List<string> names = new List<string>();
        foreach (var item in list)
        {
            names.Add(item.name);
        }

        return names;
    }


    private void ProcessProperty(PropertyModel propertyModel)
    {
        if (propertyModel == null)
        {
            propertyModel = new PropertyModel();
        }

        List<string> listOfName = Enum.GetNames(typeof(PropertyType)).ToList();
        int index = (int)propertyModel.propertyType;
        propertyModel.propertyType = (PropertyType)EditorGUILayout.Popup(index, listOfName.ToArray());
        propertyModel.propertyValue = EditorGUILayout.FloatField(propertyModel.propertyValue);
    }
}
