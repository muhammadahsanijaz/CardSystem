using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{
    [System.Serializable]
    public class TechCard : PropertyCard
    {
        public enum TechType
        {
            Engine,
            Wheels,
            BodyKit,
            GearBox
        }

        public TechType MyTechType
        {
            private set => _myTechType = value;
            get => _myTechType;
        }

        private TechType _myTechType;

        public TechCard(int id, CardTemplateModel cardTemplateModel) : base(id, cardTemplateModel)
        {
            MyTechType = (TechType)cardTemplateModel.cardSubType;
        }

        public void UpdateLevel(TechTemplateModel level)
        {

        }
    }
}
