using UnityEngine;
using System;
using Photon.Deterministic;
using Quantum;
using ExitGames.Client.Photon.StructWrapping;
using Newtonsoft.Json;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace MoonKart
{
	[Serializable]
	[CreateAssetMenu(fileName = "CarSettings", menuName = "Settings/Car Settings")]
	public class CarSettings : ScriptableObject // 
	{
		// PUBLIC MEMBERS

		public string DefaultCar => _defaultCar;
		public int FuelTimeInHours => FuelTimeInHours;
		public int TotalFuel => TotalFuel;
		public CarSetup[] Cars => _cars;
		// PRIVATE MEMBERS

		
		[SerializeField]
		private string _defaultCar;
		
		[SerializeField]private Sprite[] defaultCarIcon;

	#if ODIN_INSPECTOR
		[SerializeField, ListDrawerSettings(Expanded = true)]
		private CarSetup[] _cars;
	#else
		[SerializeField]
		private CarSetup[] _cars;
	#endif

		// PUBLIC METHODS

		public CarSetup GetCarSetup(string carID)
		{
			if (carID.HasValue() == false)
				return null;

			return _cars.Find(t => t.ID == carID);
		}

		public void SetCarSetup(CarSetup carSetup)
		{
			int index = _cars.FindIndex(t => t.ID == carSetup.ID);
			_cars[index] = carSetup;
		}

		public CarSetup GetCarSetup(AssetRefEntityPrototype carRef)
		{
			if (carRef.Id.IsValid == false)
				return null;

			return _cars.Find(t => t.CarPrototype == carRef);
		}

		public CarSetup GetRandomCarSetup()
		{
			return _cars[UnityEngine.Random.Range(0, _cars.Length)];
		}

		public void UnloadAssets()
		{
			for (int i = 0; i < _cars.Length; i++)
			{
				_cars[i].UnloadAssets();
			}
		}

    }

	[Serializable]
	public class CarSetup
	{

		// PRIVATE MEMBERS
		[SerializeField]
		private string _id;
		[SerializeField]
		private VehicleData _vehicleData;
		[NonSerialized] private GameObject _cachedCarPrefab;
		[NonSerialized] private GameObject _cachedDriverPrefab;
		[SerializeField] private bool _isSaved = false;

		// PUBLIC MEMBERS

		public string ID => _id;
		public bool IsSaved => _isSaved;
		public string DisplayName => _vehicleData.VehicleCard?.CardName;
		public string Description => _vehicleData.VehicleCard?.Description;

		public VehicleData VehicleData => _vehicleData;

		// PUBLIC METHODS

		//////////////// Car Prototype

		public AssetRefEntityPrototype CarPrototype
		{
			get
			{
				return Global.Settings.CardsSetting.GetVehicleAssetRef(_vehicleData.VehicleCard);
			}
		}

		public GameObject CarPrefab
		{
			get
			{
                if (_cachedCarPrefab == null)
                {
					_cachedCarPrefab = Global.Settings.CardsSetting.GetCardModel(_vehicleData.VehicleCard);
                    //var prototypeAsset = UnityDB.FindAsset<EntityPrototypeAsset>(_carPrototype.Id);
                    //_cachedCarPrefab = prototypeAsset != null && prototypeAsset.Parent != null ? prototypeAsset.Parent.gameObject : null;
                }

                return _cachedCarPrefab;
			}
		}


		public GameObject DriverPrefab
		{
			get
			{
				if (_cachedDriverPrefab == null)
				{
					_cachedDriverPrefab = Global.Settings.CardsSetting.GetCardModel(_vehicleData.DriverCard);
				}

				return _cachedDriverPrefab;
			}
		}

		internal void SetNewCarSetup(VehicleCard vehicleCard, DriverCard driverCard, 
			AccessoriesCard Antenna, AccessoriesCard Exhaust, AccessoriesCard Rim , int currentCar)
        {
			_vehicleData = new VehicleData(vehicleCard, driverCard, Antenna, Exhaust, Rim);
			_vehicleData.CalculateProperities();
			SaveCarSetup(false);
		}

		public void InitializeCarSetup()
		{
			_vehicleData.CalculateProperities();
		}


		public void UpdateCarSetup( Card card, int subType,bool isEquip)
        {
			if(card is VehicleCard)
            {
				_vehicleData.SetVehicleCard(card as VehicleCard);
			}
			else if (card is DriverCard)
			{
				_vehicleData.SetDriverCard(card as DriverCard);
			}
			else if (card is PowerupCard)
			{
				_vehicleData.SetPowerupCard(card as PowerupCard, subType, isEquip);
			}
			else if (card is TechCard)
			{
				_vehicleData.SetTechCard( card as TechCard, subType, isEquip);
			}
			else if (card is AccessoriesCard)
			{
				_vehicleData.SetAccessoriesCard(card as AccessoriesCard, (AccessoriesCard.AccessoriesType)subType);
			}
			_vehicleData.CalculateProperities();
        }


		internal void SaveCarSetup(bool isSaved)
        {
			_isSaved = isSaved;
			PersistentStorage.SetObjectWithJsonUtility(ID,this,true);
		}
        

        public void UnloadAssets()
		{
			_cachedCarPrefab = null;
			_vehicleData.UnloadAssets();
			

		}

        internal void ResetSetup(CarSetup carSetup)
        {
	       
			VehicleData.TechCards = carSetup.VehicleData.TechCards;
			VehicleData.PowerupCards = carSetup.VehicleData.PowerupCards;
		}
    }

}
