using Quantum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{
    [System.Serializable]
    public class PowerUpTemplateModel
    {
        public int level;
        public CardsStats _cardsStats;
        public float cooldownTime;
        public float duration;
        public PropertyModel primaryProperty;
        public PropertyModel secondaryProperty;
    }

    [CreateAssetMenu(fileName = "PowerUpDataModel", menuName = "Settings/PowerUp Data Model")]
    public class PowerUpDataModel : ScriptableObject
    {
        public List<PowerUpTemplateModel> oilSlick;
        public List<PowerUpTemplateModel> rocketLauncher;
        public List<PowerUpTemplateModel> shield;
        public List<PowerUpTemplateModel> EMMP;
        public List<PowerUpTemplateModel> turbo;
        public List<PowerUpTemplateModel> lightningBolt;

        public PowerUpTemplateModel GetData(PowerupType powerupType, CardRarity cardRarity, int level)
        {
            PowerUpTemplateModel powerUpTemplateModel = null;
            switch (powerupType)
            {
                case PowerupType.OilSlick:
                    foreach (var temp in oilSlick)
                    {
                        if (temp.level == level)
                        {
                            powerUpTemplateModel = temp;
                        }
                    }

                    break;
                case PowerupType.Booster:
                    foreach (var temp in turbo)
                    {
                        if (temp.level == level)
                        {
                            powerUpTemplateModel = temp;
                        }
                    }
                    break;
                case PowerupType.Shield:
                    foreach (var temp in shield)
                    {
                        if (temp.level == level)
                        {
                            powerUpTemplateModel = temp;
                        }
                    }
                    break;
                case PowerupType.Rocket:
                    foreach (var temp in rocketLauncher)
                    {
                        if (temp.level == level)
                        {
                            powerUpTemplateModel = temp;
                        }
                    }
                    break;
                case PowerupType.EMP:
                    foreach (var temp in EMMP)
                    {
                        if (temp.level == level)
                        {
                            powerUpTemplateModel = temp;
                        }
                    }
                    break;
                case PowerupType.Lightning:
                    foreach (var temp in lightningBolt)
                    {
                        if (temp.level == level)
                        {
                            powerUpTemplateModel = temp;
                        }
                    }
                    break;
            }

            return powerUpTemplateModel;
        }
    }
}