using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{
    [System.Serializable]
    public class VehicleCard : EntityCard
    {

        public VehicleSlots VehicleSlots
        {
            get => vehicleSlots;
        }
        private VehicleSlots vehicleSlots;


        public VehicleCard(int id, CardTemplateModel cardTemplateModel) : base(id,cardTemplateModel)
        {
           
        }

        public void SetCardSlots(int slots)
        {
            vehicleSlots.TechSlots = slots;
        }

        public void SetTrunkSlots(int slots)
        {
            vehicleSlots.TrunkSpace = slots;
        }

        public void SetPoweupSlots(int slots)
        {
            vehicleSlots.PowerupSlots = slots;
        }

        public void SetPoweupSlots(VehicleSlots slotData)
        {
            vehicleSlots = slotData;
        }

        public void UpdateLevel(KartTempelateModel model)
        {
            VehicleStats.CopyVehicleStats(CardTemplateModel.VehicleStats);
            VehicleStats.CalculateValues(model.vehicleStats);
            vehicleSlots = model.vehicleSlots;
        }
    }
}
