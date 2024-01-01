using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{
    [System.Serializable]
    public class FuelCard : Card
    {

        public FuelCard(int id, CardTemplateModel cardTemplateModel) : base(id, cardTemplateModel)
        {
            
            IsInitialized = false;
        }

        public void SetFuelLevel(CardsStateModel model)
        {
            SetCardLevel(model);
            CardStateModel.cardLevel = CardTemplateModel.cardSubType;
        }

    }
}
