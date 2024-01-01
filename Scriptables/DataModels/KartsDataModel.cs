using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{
   
    
    [System.Serializable]
    public class KartTempelateModel : CardsStatsModel
    {
        public VehicleStats vehicleStats;
        public VehicleSlots vehicleSlots;
    }

    [CreateAssetMenu(fileName = "KartsDataModel", menuName = "Settings/Karts Data Model")]
    public class KartsDataModel : ScriptableObject
    {
        public CardRarity kartType;
        public StatsTemplateModel<KartTempelateModel> dataModel;
        public KartTempelateModel GetData( CardRarity cardRarity, int level)
        {
            return dataModel.GetData(dataModel, cardRarity, level);
         
        }
    }  
}

