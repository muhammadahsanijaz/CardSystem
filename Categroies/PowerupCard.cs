using Quantum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{
    [System.Serializable]
    public class PowerupCard : PropertyCard
    {
        public PowerupType MyPowerupType => _myPowerupType;

        private PowerupType _myPowerupType;
        public PowerupCard(int id, CardTemplateModel cardTemplateModel) : base(id, cardTemplateModel)
        {
            _myPowerupType = (PowerupType)cardTemplateModel.cardSubType;
        }

        internal void UpdateLevel(PowerUpTemplateModel powerup)
        {
            SetPrimaryProperty(powerup.primaryProperty);
            SetSecondaryProperty(powerup.secondaryProperty);
        }
    }
}
