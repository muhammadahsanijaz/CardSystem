using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MoonKart
{
    [System.Serializable]
    public class DriverCard : EntityCard
    {

        public DriverCard(int id, CardTemplateModel cardTemplateModel) : base(id, cardTemplateModel)
        {
            
        }

        public void UpdateLevel(CharacterTemplateModel model)
        {
            VehicleStats.CopyVehicleStats(model.characterStats);
        }
    }
}
