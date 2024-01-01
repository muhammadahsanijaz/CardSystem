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

        public VehicleStats(float speed, float handling, float health, float acceleration)
        {
            this.Speed = speed;
            this.Handling = handling;
            this.Health = health;
            this.Acceleration = acceleration;
        }

        public VehicleStats(VehicleStats vehicleStats)
        {
            this.Speed = vehicleStats.Speed;
            this.Handling = vehicleStats.Handling;
            this.Health = vehicleStats.Health;
            this.Acceleration = vehicleStats.Acceleration;
        }

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

        public AccessoriesCard RimCard
        {
            get => _rimCard;
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
        private AccessoriesCard _rimCard, _antennaCard, _exhaustCard;
        [SerializeField]
        private TechCard[] _techCards;
        [SerializeField]
        private PowerupCard[] _powerupCards;


        public VehicleData(VehicleCard vehicleCard, DriverCard driverCard, AccessoriesCard Antenna, AccessoriesCard Exhaust, AccessoriesCard Rim)
        {
            _vehicleCard = vehicleCard;
            _vehicleCard.EquipCard();
            _driverCard = driverCard;
            _driverCard.EquipCard();
            _antennaCard = Antenna;
            _antennaCard.EquipCard();
            _exhaustCard = Exhaust;
            _exhaustCard.EquipCard();
            _rimCard = Rim;
            _rimCard.EquipCard();

            _techCards = new TechCard[4];
            _powerupCards = new PowerupCard[_vehicleCard.VehicleSlots.PowerupSlots];
        }

        public void CalculateProperities()
        {
            if (_vehicleCard == null)
            {
                Debug.LogError("Vehicle card is null. Check Card Library");
                return;
            }
            _vehicleStats = new VehicleStats(_vehicleCard.VehicleStats);
            _vehicleStats.CopyVehicleStats(_vehicleCard.VehicleStats);
            _vehicleStats.CalculateValues(_driverCard.VehicleStats);
            if (_techCards == null || _techCards.Length == 0)
            {
                _techCards = new TechCard[4];
            }
            if (_powerupCards == null)
            {
                _powerupCards = new PowerupCard[_vehicleCard.VehicleSlots.PowerupSlots];
            }

            foreach (var tech in _techCards)
            {
                if (tech != null)
                {
                    Debug.LogError("Called 2");
                    VehicleStats.CalculateValues(tech.PrimaryProperty);
                    VehicleStats.CalculateValues(tech.SecondaryProperty);
                }
            }

            if (_rimCard != null)
            {
                VehicleStats.CalculateValues(_rimCard.PrimaryProperty);
                VehicleStats.CalculateValues(_rimCard.SecondaryProperty);
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

        internal void SetVehicleCard(VehicleCard card)
        {
            _vehicleCard = card;
            if (_powerupCards != null)
            {
                foreach (var cards in _powerupCards)
                {
                    if (cards != null && cards.CardName != "")
                        cards.UnEquipCard();
                }
            }
            _powerupCards = new PowerupCard[_vehicleCard.VehicleSlots.PowerupSlots];
        }

        internal void UnloadAssets()
        {
            _techCards = null;
            _powerupCards = null;
            _vehicleCard = null;
            _driverCard = null;
            _exhaustCard = null;
            _antennaCard = null;
            _rimCard = null;
        }

        internal void SetDriverCard(DriverCard card)
        {
            _driverCard = card;
        }

        internal void SetPowerupCard(PowerupCard card,int slot, bool isEquip = true)
        {
            if (_powerupCards == null) _powerupCards = new PowerupCard[_vehicleCard.VehicleSlots.PowerupSlots];

            if (isEquip)
            {
                    _powerupCards[slot] = card;
            }
            else
            {
                _powerupCards[slot] = null;
            }
        }

        internal void SetTechCard(TechCard card,int subType, bool isEquip = true)
        {
            if (_techCards == null || _techCards.Length == 0) 
                _techCards = new TechCard[4];
            if (isEquip)
            {
                _techCards[subType] = card;
            }
            else
            {
                _techCards[subType] = null;
            }
        }

        internal void SetAccessoriesCard( AccessoriesCard card, AccessoriesCard.AccessoriesType accessory)
        {
            switch (accessory)
            {
                case AccessoriesCard.AccessoriesType.Antennas:
                    _antennaCard = card;
                    break;
                case AccessoriesCard.AccessoriesType.Rim:
                    _rimCard = card;
                    break;
                case AccessoriesCard.AccessoriesType.Exhaust:
                    _exhaustCard = card;
                    break;
            }
        }
    }
}