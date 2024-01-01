using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{
    [System.Serializable]
    public class AccessoriesCard : PropertyCard
    {
        public enum AccessoriesType
        {
            Antennas,
            Rim,
            Exhaust
        }

        public AccessoriesType MyAccessoriesType
        {
            private set => _myAccessoriesType = value;
            get => _myAccessoriesType;
        }

        public string ModelName
        {
            get => _modelName;
        }

        private string _modelName;
        private AccessoriesType _myAccessoriesType;

        public AccessoriesCard(int id, CardTemplateModel cardTemplateModel) : base(id, cardTemplateModel)
        {
            MyAccessoriesType = (AccessoriesType)cardTemplateModel.cardSubType;
            _modelName = cardTemplateModel.modelName;
        }

        internal void UpdateLevel(TechTemplateModel accessories)
        {
            SetPrimaryProperty(accessories.primaryPropertyModel);
            SetSecondaryProperty(accessories.secondaryPropertyModel);
        }
    }
}
