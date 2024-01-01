namespace MoonKart
{
    public class AccessoriesSlotHandler : SlotHandler
    {
        public AccessoriesCard.AccessoriesType accessoriesType;

        protected override void ChangeSlotApperance(CardCallFrom callFrom)
        {
            var sprite = Global.Settings.CardsSetting.addIconSlot;
            switch (accessoriesType)
            {

                case AccessoriesCard.AccessoriesType.Antennas:
                    sprite = Global.Settings.CardsSetting.addIconSlot;
                    break;
                case AccessoriesCard.AccessoriesType.Rim:
                    sprite = Global.Settings.CardsSetting.addIconSlot;
                    break;
                case AccessoriesCard.AccessoriesType.Exhaust:
                    sprite = Global.Settings.CardsSetting.addIconSlot;
                    break;
            }

            NameText.SetTextSafe((Card != null) ? Card.CardName : "");
            cardIcon.sprite = (Card != null && Card.Icon != null) ? Card.Icon : sprite;
            
            ChangeSlotBorder();
        }

        protected override void ChangeModelApperance()
        {
            switch (accessoriesType)
            {
                case AccessoriesCard.AccessoriesType.Antennas:
                    Context.Garage.ShowAntenna(Context.Settings.CardsSetting.GetCardModel(Card));
                    break;
                case AccessoriesCard.AccessoriesType.Rim:
                    Context.Garage.ShowRim(Context.Settings.CardsSetting.GetCardModel(Card));
                    break;
                case AccessoriesCard.AccessoriesType.Exhaust:
                    Context.Garage.ShowExhaust(Context.Settings.CardsSetting.GetCardModel(Card));
                    break;
                default:
                    break;
            }
        }
    }
}
