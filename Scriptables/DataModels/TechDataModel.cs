using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{
    [CreateAssetMenu(fileName = "TechDataModel", menuName = "Settings/Tech Data Model")]
    public class TechDataModel : ScriptableObject
    {
        public CardRarity TechType;
        public StatsTemplateModel<TechTemplateModel> dataModel;
        
        public TechTemplateModel GetData( CardRarity cardRarity, int level)
        {
            return dataModel.GetData(dataModel, cardRarity, level);
        }
    }
}
