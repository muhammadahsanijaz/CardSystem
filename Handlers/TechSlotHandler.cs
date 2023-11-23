using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{
    public class TechSlotHandler : SlotHandler
    {
        public TechCard.TechType techType;

        protected override void ChangeSlotApperance(CardCallFrom callFrom)
        {
            var sprite = Global.Settings.CardsSetting.addIconSlot;
            switch (techType)
            {
                case TechCard.TechType.Engine:
                    sprite = Global.Settings.CardsSetting.addEngineSlot;
                    break;
                case TechCard.TechType.Wheels:
                    sprite = Global.Settings.CardsSetting.addWheelSlot;
                    break;
                case TechCard.TechType.BodyKit:
                    sprite = Global.Settings.CardsSetting.addBodyKitSlot;
                    break;
                case TechCard.TechType.GearBox:
                    sprite = Global.Settings.CardsSetting.addGearBoxSlot;
                    break;
            }

            NameText.SetTextSafe((Card != null) ? Card.CardName : "");
            cardIcon.sprite = (Card != null && Card.Icon != null) ? Card.Icon : sprite;
            
            ChangeSlotBorder();
        }
    }
}
