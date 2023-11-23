using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{

    [Serializable]
    public class VehicleStats
    {
        public float Speed;
        public float Handling;
        public float Health;
        public float Acceleration;

        public void CopyVehicleStats(VehicleStats vehicleStats)
        {
            Speed = vehicleStats.Speed;
            Handling = vehicleStats.Handling;
            Health = vehicleStats.Health;
            Acceleration = vehicleStats.Acceleration;
        }

        public void CalculateValues(VehicleStats stats)
        {
            Speed += Speed * stats.Speed / 100;
            Handling += Handling * stats.Handling / 100;
            Health += Health * stats.Health / 100;
            Acceleration += Acceleration * stats.Acceleration / 100;
        }

        public void CalculateValues(PropertyModel property)
        {
            if (property == null) return;
            
            switch (property.propertyType)
            {
                case PropertyType.Speed:
                    Speed += Speed * property.propertyValue / 100;
                    break;
                case PropertyType.Handling:
                    Handling += Handling * property.propertyValue / 100;
                    break;
                case PropertyType.Acceleration:
                    Acceleration += Acceleration * property.propertyValue / 100;
                    break;
                case PropertyType.Health:
                    Health += Health * property.propertyValue / 100;
                    break;
            }
        }
    }
    [Serializable]
    public class VehicleSlots
    {
        public int TrunkSpace;
        public int TechSlots;
        public int PowerupSlots;
    }



    [Serializable]
    public class VehicleData
    {
        //public 
        public VehicleCard VehicleCard
        {
            get => _vehicleCard;
        }

        public DriverCard DriverCard
        {
            get => _driverCard;
        }

        public TechCard[] TechCards
        {
            get => _techCards;
            set => _techCards = value;
        }

        public PowerupCard[] PowerupCards
        {
            get => _powerupCards;
            set => _powerupCards = value;
        }

        public AccessoriesCard WheelCard
        {
            get => _wheelCard;
        }

        public AccessoriesCard AntennaCard
        {
            get => _antennaCard;
        }

        public AccessoriesCard ExhaustCard
        {
            get => _exhaustCard;
        }

        public VehicleStats VehicleStats
        {
            get => _vehicleStats;
        }

        private VehicleStats _vehicleStats;


        //Private
        [SerializeField]
        private VehicleCard _vehicleCard;
        [SerializeField]
        private DriverCard _driverCard;
        [SerializeField]
        private AccessoriesCard _wheelCard, _antennaCard, _exhaustCard;
        [SerializeField]
        private TechCard[] _techCards;
        [SerializeField]
        private PowerupCard[] _powerupCards;


        public VehicleData(VehicleCard vehicleCard, DriverCard driverCard)
        {
            _vehicleCard = vehicleCard;
            _driverCard = driverCard;
            _techCards = new TechCard[4];
            _powerupCards = new PowerupCard[VehicleCard.VehicleSlots.PowerupSlots];
        }

        public void CalculateProperities()
        {
            if (_vehicleCard == null)
            {
                Debug.LogError("Vehicle card is null. Check Card Library");
                return;
            }
            _vehicleStats = new VehicleStats();
            _vehicleStats.CopyVehicleStats(_vehicleCard.VehicleStats);
            _vehicleStats.CalculateValues(_driverCard.VehicleStats);
            if (_techCards == null)
            {
                _techCards = new TechCard[4];
            }
            if (_powerupCards == null)
            {
                _powerupCards = new PowerupCard[VehicleCard.VehicleSlots.PowerupSlots];
            }

            foreach (var tech in _techCards)
            {
                if (tech != null)
                {
                    VehicleStats.CalculateValues(tech.PrimaryProperty);
                    VehicleStats.CalculateValues(tech.SecondaryProperty);
                }
            }

            if (_wheelCard != null)
            {
                VehicleStats.CalculateValues(_wheelCard.PrimaryProperty);
                VehicleStats.CalculateValues(_wheelCard.SecondaryProperty);
            }
            if (_antennaCard != null)
            {
                VehicleStats.CalculateValues(_antennaCard.PrimaryProperty);
                VehicleStats.CalculateValues(_antennaCard.SecondaryProperty);
            }
            if (_exhaustCard != null)
            {
                VehicleStats.CalculateValues(_exhaustCard.PrimaryProperty);
                VehicleStats.CalculateValues(_exhaustCard.SecondaryProperty);
            }
        }

        internal void SetVehicleCard(VehicleCard card, bool isEquip = true)
        {
            _vehicleCard = (isEquip) ? card : null;
            UpdateEquipedSlots();
        }

        private void UpdateEquipedSlots()
        {
            _techCards = new TechCard[4];
            _powerupCards = new PowerupCard[VehicleCard.VehicleSlots.PowerupSlots];
            _exhaustCard = null;
            _antennaCard = null;
            _wheelCard = null;
        }

        internal void UnloadAssets()
        {
            _techCards = null;
            _powerupCards = null;
            _vehicleCard = null;
            _driverCard = null;
            _exhaustCard = null;
            _antennaCard = null;
            _wheelCard = null;
        }

        internal void SetDriverCard(DriverCard card, bool isEquip = true)
        {
            _driverCard = (isEquip) ? card : null;
        }

        internal void SetPowerupCard(PowerupCard card,int slot, bool isEquip = true)
        {
            if (_powerupCards == null) _powerupCards = new PowerupCard[VehicleCard.VehicleSlots.PowerupSlots];

            if (isEquip)
            {
                    _powerupCards[slot] = card;
            }
            else
            {
                _powerupCards[slot] = null;
            }
        }

        internal void SetTechCard(TechCard card,int slot, bool isEquip = true)
        {
            if (_techCards == null) _techCards = new TechCard[4];
            if (isEquip)
            {
                _techCards[slot] = card;
            }
            else
            {
                _techCards[slot] = null;
            }
        }

        internal void SetAccessoriesCard(AccessoriesCard card, bool isEquip = true)
        {
            switch (card.MyAccessoriesType)
            {
                case AccessoriesCard.AccessoriesType.Antennas:
                    _antennaCard = (isEquip) ? card : null;
                    break;
                case AccessoriesCard.AccessoriesType.Wheels:
                    _wheelCard = (isEquip) ? card : null;
                    break;
                case AccessoriesCard.AccessoriesType.Exhaust:
                    _exhaustCard = (isEquip) ? card : null;
                    break;
            }
        }
    }
}