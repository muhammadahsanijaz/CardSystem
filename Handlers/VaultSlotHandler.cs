using UnityEngine;
namespace MoonKart
{
    public class VaultSlotHandler : SlotHandler
    {
        public GameObject lockIcon;
        protected override void ChangeSlotApperance(CardCallFrom callFrom)
        {
            base.ChangeSlotApperance(callFrom);
            if(callFrom == CardCallFrom.Vault)
            {
                if(Card != null && Card.CardName == "")
                lockIcon.SetActive(Card.CardStateModel.cardState == CardState.Staking);
            }
        }
    }
}
