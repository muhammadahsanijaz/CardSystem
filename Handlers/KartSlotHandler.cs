using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MoonKart
{
    public class KartSlotHandler : SlotHandler
    {
      

        protected override void ChangeSlotApperance(CardCallFrom callFrom)
        {
            var sprite = Global.Settings.CardsSetting.addIconSlot;

            NameText.SetTextSafe((Card != null) ? Card.CardName : "");
            cardIcon.sprite = (Card != null && Card.Icon != null) ? Card.Icon : sprite;
        }

        protected override void ChangeModelApperance()
        {
            var setup = Global.Settings.CarSetting.GetCarSetup(Context.Player.CarPresetIndex);
            Context.Garage.ShowCar(Context.Settings.CardsSetting.GetCardModel(Card),
                Context.Settings.CardsSetting.GetCardModel(setup.VehicleData.AntennaCard),
                Context.Settings.CardsSetting.GetCardModel(setup.VehicleData.ExhaustCard),
                Context.Settings.CardsSetting.GetCardModel(setup.VehicleData.RimCard), false);
        }
    }
}
