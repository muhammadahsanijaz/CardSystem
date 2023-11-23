using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{
    [System.Serializable]
    public abstract class EntityCard : Card
    {

        public string ModelName
        {
            get => modelName;
        }

        public VehicleStats VehicleStats
        {
            get => vehicleStats;
        }


        private string modelName;
        private VehicleStats vehicleStats;

        public EntityCard(int id, CardTemplateModel cardTemplateModel) : base(id, cardTemplateModel)
        {
            modelName = cardTemplateModel.modelName;
            vehicleStats = new VehicleStats();
            vehicleStats.CopyVehicleStats(cardTemplateModel.VehicleStats);
        }



    }
}
