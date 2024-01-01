using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonKart;
using UnityEditor;
using System;
using System.Linq;

[CustomEditor(typeof(KartsDataModel))]
public class KartsDataModelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        KartsDataModel kartsDataModel = (KartsDataModel)target;
        if (kartsDataModel.dataModel == null)
        {
            kartsDataModel.dataModel = new StatsTemplateModel<KartTempelateModel>();
        }
        var listOfType = Enum.GetNames(typeof(CardRarity)).ToList();
        var index = (int)kartsDataModel.kartType;
        kartsDataModel.kartType = (CardRarity)EditorGUILayout.Popup(index, listOfType.ToArray());
        if (kartsDataModel.kartType== CardRarity.Common)
        {
            EditorGUILayout.LabelField("BASE MODEL");
            for (int i = 0; i < kartsDataModel.dataModel.Base.Count; i++)
            {
                EditorGUILayout.LabelField("Level " + (i + 1));
                kartsDataModel.dataModel.Base[i] = Process(kartsDataModel.dataModel.Base[i]);
                EditorGUILayout.Space();
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    kartsDataModel.dataModel.Base.RemoveAt(i);
                }
            }
            EditorGUILayout.Space();

            if (GUILayout.Button("Add", GUILayout.Height(30)))
            {
                kartsDataModel.dataModel.Base.Add(new KartTempelateModel());
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
        
        
        if (kartsDataModel.kartType == CardRarity.Rare)
        {
            EditorGUILayout.LabelField("RARE MODEL");
            for (int i = 0; i < kartsDataModel.dataModel.Rare.Count; i++)
            {

                EditorGUILayout.LabelField("Level " + (i + 1));
                kartsDataModel.dataModel.Rare[i] = Process(kartsDataModel.dataModel.Rare[i]);
                EditorGUILayout.Space();
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    kartsDataModel.dataModel.Rare.RemoveAt(i);
                }
            }
            EditorGUILayout.Space();

            if (GUILayout.Button("Add", GUILayout.Height(30)))
            {
                kartsDataModel.dataModel.Rare.Add(new KartTempelateModel());
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
        
        
        if (kartsDataModel.kartType == CardRarity.Epic)
        {
            EditorGUILayout.LabelField("EPIC MODEL");
            for (int i = 0; i < kartsDataModel.dataModel.Epic.Count; i++)
            {

                EditorGUILayout.LabelField("Level " + (i + 1));
                kartsDataModel.dataModel.Epic[i] = Process(kartsDataModel.dataModel.Epic[i]);
                EditorGUILayout.Space();
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    kartsDataModel.dataModel.Epic.RemoveAt(i);
                }
            }
            EditorGUILayout.Space();

            if (GUILayout.Button("Add", GUILayout.Height(30)))
            {
                kartsDataModel.dataModel.Epic.Add(new KartTempelateModel());
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
        
        
        if (kartsDataModel.kartType == CardRarity.Legendary)
        {
            EditorGUILayout.LabelField("LEGENDARY MODEL");
            for (int i = 0; i < kartsDataModel.dataModel.Legendary.Count; i++)
            {

                EditorGUILayout.LabelField("Level " + (i + 1));
                kartsDataModel.dataModel.Legendary[i] = Process(kartsDataModel.dataModel.Legendary[i]);
                EditorGUILayout.Space();
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    kartsDataModel.dataModel.Legendary.RemoveAt(i);
                }
            }
            EditorGUILayout.Space();

            if (GUILayout.Button("Add", GUILayout.Height(30)))
            {
                kartsDataModel.dataModel.Legendary.Add(new KartTempelateModel());
            }

            EditorGUILayout.Space();
            EditorUtility.SetDirty(kartsDataModel);
        }
       

    }


    private KartTempelateModel Process(KartTempelateModel KartTempelateModel)
    {

        EditorGUILayout.LabelField("STATS", GUILayout.Width(100));
        if (KartTempelateModel.cardsStats == null)
        {
            KartTempelateModel.cardsStats = new CardsStats();
        }
        KartTempelateModel.cardsStats.cardsRequired = EditorGUILayout.IntField("Cards Required", KartTempelateModel.cardsStats.cardsRequired);
        KartTempelateModel.cardsStats.goldCardsRequied = EditorGUILayout.IntField("Gold Cards Requied", KartTempelateModel.cardsStats.goldCardsRequied);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("VEHICLES STATS", GUILayout.Width(120));
        if (KartTempelateModel.vehicleStats == null)
        {
            KartTempelateModel.vehicleStats = new VehicleStats(0,0,0,0);
        }
        KartTempelateModel.vehicleStats.Speed = EditorGUILayout.FloatField("Speed", KartTempelateModel.vehicleStats.Speed);
        KartTempelateModel.vehicleStats.Handling = EditorGUILayout.FloatField("Handling", KartTempelateModel.vehicleStats.Handling);
        KartTempelateModel.vehicleStats.Acceleration = EditorGUILayout.FloatField("Acceleration", KartTempelateModel.vehicleStats.Acceleration);
        KartTempelateModel.vehicleStats.Health = EditorGUILayout.FloatField("Health", KartTempelateModel.vehicleStats.Health);
        EditorGUILayout.LabelField("VEHICLES SLOTS", GUILayout.Width(120));
        if (KartTempelateModel.vehicleSlots == null)
        {
            KartTempelateModel.vehicleSlots = new VehicleSlots();
        }
        KartTempelateModel.vehicleSlots.TrunkSpace = EditorGUILayout.IntField("Trunk Space", KartTempelateModel.vehicleSlots.TrunkSpace);
        KartTempelateModel.vehicleSlots.TechSlots = EditorGUILayout.IntField("Tech Slot", KartTempelateModel.vehicleSlots.TechSlots);
        KartTempelateModel.vehicleSlots.PowerupSlots = EditorGUILayout.IntField("Power Up Slot", KartTempelateModel.vehicleSlots.PowerupSlots);
        return KartTempelateModel;

    }
}

