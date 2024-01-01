using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MoonKart.UI
{
    public class FuelCardHandler : CardHandler
    {
        public Image[] fuelsIconImage;

        public void EnableFuelIcon(int currentLvl)
        {
            for (int j = 0; j < fuelsIconImage.Length; j++)
            {
                if (j == currentLvl - 1)
                {
                    fuelsIconImage[j].SetActive(true);
                }
                else
                {
                    fuelsIconImage[j].SetActive(false);
                }
            }
        }
    }
}