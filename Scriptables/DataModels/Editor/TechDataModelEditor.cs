using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonKart;
using UnityEditor;
using System;
using System.Linq;

[CustomEditor(typeof(TechDataModel))]
public class TechDataModelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TechDataModel techDataModel = (TechDataModel)target;
        if (techDataModel.dataModel == null)
        {
            techDataModel.dataModel = new StatsTemplateModel<TechTemplateModel>();
        }
        var listOfType = Enum.GetNames(typeof(CardRarity)).ToList();
        var ind = (int)techDataModel.TechType;
        techDataModel.TechType = (CardRarity)EditorGUILayout.Popup(ind, listOfType.ToArray());
        if(techDataModel.TechType == CardRarity.Common)
        {
            EditorGUILayout.LabelField("BASE MODEL");
            for (int i = 0; i < techDataModel.dataModel.Base.Count; i++)
            {

                EditorGUILayout.LabelField("Level " + (i + 1));
                EditorGUILayout.LabelField("STATS", GUILayout.Width(100));
                if (techDataModel.dataModel.Base[i].cardsStats == null)
                {
                    techDataModel.dataModel.Base[i].cardsStats = new CardsStats();
                }
                techDataModel.dataModel.Base[i].cardsStats.cardsRequired = EditorGUILayout.IntField("Cards Required", techDataModel.dataModel.Base[i].cardsStats.cardsRequired);
                techDataModel.dataModel.Base[i].cardsStats.goldCardsRequied = EditorGUILayout.IntField("Gold Cards Requied", techDataModel.dataModel.Base[i].cardsStats.goldCardsRequied);

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Primary", GUILayout.Width(100));
                List<string> listOfName = Enum.GetNames(typeof(PropertyType)).ToList();
                if (techDataModel.dataModel.Base[i].primaryPropertyModel == null)
                {
                    techDataModel.dataModel.Base[i].primaryPropertyModel = new PropertyModel();
                }
                int index = (int)techDataModel.dataModel.Base[i].primaryPropertyModel.propertyType;
                techDataModel.dataModel.Base[i].primaryPropertyModel.propertyType = (PropertyType)EditorGUILayout.Popup(index, listOfName.ToArray());
                techDataModel.dataModel.Base[i].primaryPropertyModel.propertyValue = EditorGUILayout.FloatField(techDataModel.dataModel.Base[i].primaryPropertyModel.propertyValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Secondary", GUILayout.Width(100));
                if (techDataModel.dataModel.Base[i].secondaryPropertyModel == null)
                {
                    techDataModel.dataModel.Base[i].secondaryPropertyModel = new PropertyModel();
                }
                index = (int)techDataModel.dataModel.Base[i].secondaryPropertyModel.propertyType;
                techDataModel.dataModel.Base[i].secondaryPropertyModel.propertyType = (PropertyType)EditorGUILayout.Popup(index, listOfName.ToArray());
                techDataModel.dataModel.Base[i].secondaryPropertyModel.propertyValue = EditorGUILayout.FloatField(techDataModel.dataModel.Base[i].secondaryPropertyModel.propertyValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    techDataModel.dataModel.Base.RemoveAt(i);
                }
            }
            EditorGUILayout.Space();

            if (GUILayout.Button("Add", GUILayout.Height(30)))
            {
                techDataModel.dataModel.Base.Add(new TechTemplateModel());
            }
            EditorGUILayout.Space();
        }
        
        EditorGUILayout.Space();
        if (techDataModel.TechType == CardRarity.Rare)
        {
            EditorGUILayout.LabelField("RARE MODEL");
            for (int i = 0; i < techDataModel.dataModel.Rare.Count; i++)
            {

                EditorGUILayout.LabelField("Level " + (i + 1));
                EditorGUILayout.LabelField("STATS", GUILayout.Width(100));
                if (techDataModel.dataModel.Rare[i].cardsStats == null)
                {
                    techDataModel.dataModel.Rare[i].cardsStats = new CardsStats();
                }
                techDataModel.dataModel.Rare[i].cardsStats.cardsRequired = EditorGUILayout.IntField("Cards Required", techDataModel.dataModel.Rare[i].cardsStats.cardsRequired);
                techDataModel.dataModel.Rare[i].cardsStats.goldCardsRequied = EditorGUILayout.IntField("Gold Cards Requied", techDataModel.dataModel.Rare[i].cardsStats.goldCardsRequied);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Primary", GUILayout.Width(100));
                if (techDataModel.dataModel.Rare[i].primaryPropertyModel == null)
                {
                    techDataModel.dataModel.Rare[i].primaryPropertyModel = new PropertyModel();
                }
                List<string> listOfName = Enum.GetNames(typeof(PropertyType)).ToList();
                int index = (int)techDataModel.dataModel.Rare[i].primaryPropertyModel.propertyType;
                techDataModel.dataModel.Rare[i].primaryPropertyModel.propertyType = (PropertyType)EditorGUILayout.Popup(index, listOfName.ToArray());
                techDataModel.dataModel.Rare[i].primaryPropertyModel.propertyValue = EditorGUILayout.FloatField(techDataModel.dataModel.Rare[i].primaryPropertyModel.propertyValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Secondary", GUILayout.Width(100));
                if (techDataModel.dataModel.Rare[i].secondaryPropertyModel == null)
                {
                    techDataModel.dataModel.Rare[i].secondaryPropertyModel = new PropertyModel();
                }
                index = (int)techDataModel.dataModel.Rare[i].secondaryPropertyModel.propertyType;
                techDataModel.dataModel.Rare[i].secondaryPropertyModel.propertyType = (PropertyType)EditorGUILayout.Popup(index, listOfName.ToArray());
                techDataModel.dataModel.Rare[i].secondaryPropertyModel.propertyValue = EditorGUILayout.FloatField(techDataModel.dataModel.Rare[i].secondaryPropertyModel.propertyValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    techDataModel.dataModel.Rare.RemoveAt(i);
                }
            }
            EditorGUILayout.Space();

            if (GUILayout.Button("Add", GUILayout.Height(30)))
            {
                techDataModel.dataModel.Rare.Add(new TechTemplateModel());
            }
            EditorGUILayout.Space();
        }
        
        EditorGUILayout.Space();
        if (techDataModel.TechType == CardRarity.Epic)
        {
            EditorGUILayout.LabelField("EPIC MODEL");
            for (int i = 0; i < techDataModel.dataModel.Epic.Count; i++)
            {

                EditorGUILayout.LabelField("Level " + (i + 1));
                EditorGUILayout.LabelField("STATS", GUILayout.Width(100));
                if (techDataModel.dataModel.Epic[i].cardsStats == null)
                {
                    techDataModel.dataModel.Epic[i].cardsStats = new CardsStats();
                }
                techDataModel.dataModel.Epic[i].cardsStats.cardsRequired = EditorGUILayout.IntField("Cards Required", techDataModel.dataModel.Epic[i].cardsStats.cardsRequired);
                techDataModel.dataModel.Epic[i].cardsStats.goldCardsRequied = EditorGUILayout.IntField("Gold Cards Requied", techDataModel.dataModel.Epic[i].cardsStats.goldCardsRequied);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Primary", GUILayout.Width(100));
                if (techDataModel.dataModel.Epic[i].primaryPropertyModel == null)
                {
                    techDataModel.dataModel.Epic[i].primaryPropertyModel = new PropertyModel();
                }
                List<string> listOfName = Enum.GetNames(typeof(PropertyType)).ToList();
                int index = (int)techDataModel.dataModel.Epic[i].primaryPropertyModel.propertyType;
                techDataModel.dataModel.Epic[i].primaryPropertyModel.propertyType = (PropertyType)EditorGUILayout.Popup(index, listOfName.ToArray());
                techDataModel.dataModel.Epic[i].primaryPropertyModel.propertyValue = EditorGUILayout.FloatField(techDataModel.dataModel.Epic[i].primaryPropertyModel.propertyValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Secondary", GUILayout.Width(100));
                if (techDataModel.dataModel.Epic[i].secondaryPropertyModel == null)
                {
                    techDataModel.dataModel.Epic[i].secondaryPropertyModel = new PropertyModel();
                }
                index = (int)techDataModel.dataModel.Epic[i].secondaryPropertyModel.propertyType;
                techDataModel.dataModel.Epic[i].secondaryPropertyModel.propertyType = (PropertyType)EditorGUILayout.Popup(index, listOfName.ToArray());
                techDataModel.dataModel.Epic[i].secondaryPropertyModel.propertyValue = EditorGUILayout.FloatField(techDataModel.dataModel.Epic[i].secondaryPropertyModel.propertyValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    techDataModel.dataModel.Epic.RemoveAt(i);
                }
            }
            EditorGUILayout.Space();

            if (GUILayout.Button("Add", GUILayout.Height(30)))
            {
                techDataModel.dataModel.Epic.Add(new TechTemplateModel());
            }
            EditorGUILayout.Space();
        }
        
        EditorGUILayout.Space();
        if (techDataModel.TechType == CardRarity.Legendary)
        {
            EditorGUILayout.LabelField("LEGENDARY MODEL");
            for (int i = 0; i < techDataModel.dataModel.Legendary.Count; i++)
            {

                EditorGUILayout.LabelField("Level " + (i + 1));
                EditorGUILayout.LabelField("STATS", GUILayout.Width(100));
                if (techDataModel.dataModel.Legendary[i].cardsStats == null)
                {
                    techDataModel.dataModel.Legendary[i].cardsStats = new CardsStats();
                }
                techDataModel.dataModel.Legendary[i].cardsStats.cardsRequired = EditorGUILayout.IntField("Cards Required", techDataModel.dataModel.Legendary[i].cardsStats.cardsRequired);
                techDataModel.dataModel.Legendary[i].cardsStats.goldCardsRequied = EditorGUILayout.IntField("Gold Cards Requied", techDataModel.dataModel.Legendary[i].cardsStats.goldCardsRequied);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Primary", GUILayout.Width(100));
                if (techDataModel.dataModel.Legendary[i].primaryPropertyModel == null)
                {
                    techDataModel.dataModel.Legendary[i].primaryPropertyModel = new PropertyModel();
                }
                List<string> listOfName = Enum.GetNames(typeof(PropertyType)).ToList();
                int index = (int)techDataModel.dataModel.Legendary[i].primaryPropertyModel.propertyType;
                techDataModel.dataModel.Legendary[i].primaryPropertyModel.propertyType = (PropertyType)EditorGUILayout.Popup(index, listOfName.ToArray());
                techDataModel.dataModel.Legendary[i].primaryPropertyModel.propertyValue = EditorGUILayout.FloatField(techDataModel.dataModel.Legendary[i].primaryPropertyModel.propertyValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Secondary", GUILayout.Width(100));
                if (techDataModel.dataModel.Legendary[i].secondaryPropertyModel == null)
                {
                    techDataModel.dataModel.Legendary[i].secondaryPropertyModel = new PropertyModel();
                }
                index = (int)techDataModel.dataModel.Legendary[i].secondaryPropertyModel.propertyType;
                techDataModel.dataModel.Legendary[i].secondaryPropertyModel.propertyType = (PropertyType)EditorGUILayout.Popup(index, listOfName.ToArray());
                techDataModel.dataModel.Legendary[i].secondaryPropertyModel.propertyValue = EditorGUILayout.FloatField(techDataModel.dataModel.Legendary[i].secondaryPropertyModel.propertyValue);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    techDataModel.dataModel.Legendary.RemoveAt(i);
                }
            }
            if (GUILayout.Button("Add", GUILayout.Height(30)))
            {
                techDataModel.dataModel.Legendary.Add(new TechTemplateModel());
            }
        }
        EditorGUILayout.Space();

        

        EditorGUILayout.Space();
        EditorUtility.SetDirty(techDataModel);

    }
}
