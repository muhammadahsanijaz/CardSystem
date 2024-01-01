using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{
    [System.Serializable]
    public class CharacterTemplateModel : CardsStatsModel
    {
        public VehicleStats characterStats;
    }
    [CreateAssetMenu(fileName = "CharacterDataModel", menuName = "Settings/Character Data Model")]
    public class CharacterDataModel : ScriptableObject
    {
        public CardRarity characterType;
        public StatsTemplateModel<CharacterTemplateModel> dataModel;
        public CharacterTemplateModel GetData( CardRarity cardRarity, int level)
        {
            return dataModel.GetData(dataModel, cardRarity, level);
        }

    }
}

