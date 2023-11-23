using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{
    [System.Serializable]
    public class PowerupCard : PropertyCard
    {
        public enum PowerupType
        {
            OilSlick,
            SpeedBoost,
            Shield,
            Rocket,
            EMP,
            Lightning
        }

        public PowerupType MyPowerupType
        {
            private set => _myPowerupType = value;
            get => _myPowerupType;
        }

        private PowerupType _myPowerupType;

        public PowerupCard(int id, CardTemplateModel cardTemplateModel) : base(id, cardTemplateModel)
        {
            MyPowerupType = (PowerupType)cardTemplateModel.cardSubType;
        }

    }
}
