using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonKart;
using UnityEditor;
using System;
using System.Linq;

[CustomEditor(typeof(CharacterDataModel))]
public class CharacterDataModelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CharacterDataModel characterDataModel = (CharacterDataModel)target;
        if (characterDataModel.dataModel == null)
        {
            characterDataModel.dataModel = new StatsTemplateModel<CharacterTemplateModel>();
        }
        var listOfType = Enum.GetNames(typeof(CardRarity)).ToList();
        var index = (int)characterDataModel.characterType;
        characterDataModel.characterType = (CardRarity)EditorGUILayout.Popup(index, listOfType.ToArray());
        if(characterDataModel.characterType == CardRarity.Common)
        {
            EditorGUILayout.LabelField("BASE MODEL");
            for (int i = 0; i < characterDataModel.dataModel.Base.Count; i++)
            {
                EditorGUILayout.LabelField("Level " + (i + 1));
                characterDataModel.dataModel.Base[i] = Process(characterDataModel.dataModel.Base[i]);
                EditorGUILayout.Space();
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    characterDataModel.dataModel.Base.RemoveAt(i);
                }
            }
            EditorGUILayout.Space();

            if (GUILayout.Button("Add", GUILayout.Height(30)))
            {
                characterDataModel.dataModel.Base.Add(new CharacterTemplateModel());
            }
            EditorGUILayout.Space();
            
        }
        EditorGUILayout.Space();
        if (characterDataModel.characterType == CardRarity.Rare)
        {
            EditorGUILayout.LabelField("RARE MODEL");
            for (int i = 0; i < characterDataModel.dataModel.Rare.Count; i++)
            {

                EditorGUILayout.LabelField("Level " + (i + 1));
                characterDataModel.dataModel.Rare[i] = Process(characterDataModel.dataModel.Rare[i]);
                EditorGUILayout.Space();
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    characterDataModel.dataModel.Rare.RemoveAt(i);
                }
            }
            EditorGUILayout.Space();

            if (GUILayout.Button("Add", GUILayout.Height(30)))
            {
                characterDataModel.dataModel.Rare.Add(new CharacterTemplateModel());
            }
            EditorGUILayout.Space();
        }
        EditorGUILayout.Space();
        if (characterDataModel.characterType == CardRarity.Epic)
        {
            EditorGUILayout.LabelField("EPIC MODEL");
            for (int i = 0; i < characterDataModel.dataModel.Epic.Count; i++)
            {

                EditorGUILayout.LabelField("Level " + (i + 1));
                characterDataModel.dataModel.Epic[i] = Process(characterDataModel.dataModel.Epic[i]);
                EditorGUILayout.Space();
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    characterDataModel.dataModel.Epic.RemoveAt(i);
                }
            }
            EditorGUILayout.Space();

            if (GUILayout.Button("Add", GUILayout.Height(30)))
            {
                characterDataModel.dataModel.Epic.Add(new CharacterTemplateModel());
            }
            EditorGUILayout.Space();
        }
        EditorGUILayout.Space();
        if (characterDataModel.characterType == CardRarity.Legendary)
        {
            EditorGUILayout.LabelField("LEGENDARY MODEL");
            for (int i = 0; i < characterDataModel.dataModel.Legendary.Count; i++)
            {

                EditorGUILayout.LabelField("Level " + (i + 1));
                characterDataModel.dataModel.Legendary[i] = Process(characterDataModel.dataModel.Legendary[i]);
                EditorGUILayout.Space();
                if (GUILayout.Button("Remove", GUILayout.Width(100)))
                {
                    characterDataModel.dataModel.Legendary.RemoveAt(i);
                }
            }
            EditorGUILayout.Space();

            if (GUILayout.Button("Add", GUILayout.Height(30)))
            {
                characterDataModel.dataModel.Legendary.Add(new CharacterTemplateModel());
            }

            EditorGUILayout.Space();
            EditorUtility.SetDirty(characterDataModel);

        }
        EditorGUILayout.Space();
        
    }


    private CharacterTemplateModel Process(CharacterTemplateModel characterTemplateModel)
    {
        
        EditorGUILayout.LabelField("STATS", GUILayout.Width(120));
        if(characterTemplateModel.cardsStats == null)
        {
            characterTemplateModel.cardsStats = new CardsStats();
        }
        characterTemplateModel.cardsStats.cardsRequired = EditorGUILayout.IntField("Cards Required", characterTemplateModel.cardsStats.cardsRequired);
        characterTemplateModel.cardsStats.goldCardsRequied = EditorGUILayout.IntField("Gold Cards Requied", characterTemplateModel.cardsStats.goldCardsRequied);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("CHARACTER STATS", GUILayout.Width(120));
        if (characterTemplateModel.characterStats == null)
        {
            characterTemplateModel.characterStats = new VehicleStats(0,0,0,0);
        }
        characterTemplateModel.characterStats.Speed = EditorGUILayout.FloatField("Speed", characterTemplateModel.characterStats.Speed);
        characterTemplateModel.characterStats.Handling = EditorGUILayout.FloatField("Handling", characterTemplateModel.characterStats.Handling);
        characterTemplateModel.characterStats.Acceleration = EditorGUILayout.FloatField("Acceleration", characterTemplateModel.characterStats.Acceleration);
        characterTemplateModel.characterStats.Health = EditorGUILayout.FloatField("Health", characterTemplateModel.characterStats.Health);
        return characterTemplateModel;
       
    }
}
