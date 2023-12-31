using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{

    public enum PropertyType
    {
        None,
        Speed,
        Handling,
        Acceleration,
        Health
    }
    [System.Serializable]
    public class PropertyModel
    {
        public PropertyType propertyType;
        public float propertyValue;

        public PropertyModel()
        {

        }

        public PropertyModel(PropertyModel primaryProperty)
        {
            this.propertyType = primaryProperty.propertyType;
            this.propertyValue = primaryProperty.propertyValue;
        }
    }

    [System.Serializable]
    public abstract class PropertyCard : Card
    {
        public PropertyModel PrimaryProperty => _primaryProperty;
        public PropertyModel SecondaryProperty => _secondaryProperty;

        private PropertyModel _primaryProperty;
        private PropertyModel _secondaryProperty;

        public PropertyCard(int id, CardTemplateModel cardTemplateModel) : base(id, cardTemplateModel)
        {
            //_primaryProperty = cardTemplateModel.PrimaryProperty;
            //_secondaryProperty = cardTemplateModel.SecondaryProperty;
        }

        public void SetPrimaryProperty(PropertyModel Property)
        {
            _primaryProperty = new PropertyModel(Property);
        }

        public void SetSecondaryProperty(PropertyModel Property)
        {
            _secondaryProperty = new PropertyModel(Property);
        }

    }
}
