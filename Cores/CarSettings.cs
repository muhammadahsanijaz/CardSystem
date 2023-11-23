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
		public CarSetup[] Cars => _cars;

		// PRIVATE MEMBERS

		
		[SerializeField]
		private string _defaultCar;

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

		// PUBLIC MEMBERS

		public string ID => _id;
		public string DisplayName => _vehicleData.VehicleCard?.CardName;
		public string Description => _vehicleData.VehicleCard?.Description;

		public VehicleData VehicleData => _vehicleData;

		// PUBLIC METHODS
		[JsonIgnore]
		public AssetRefEntityPrototype CarPrototype
		{
			get
			{
				return Global.Settings.CardsSetting.GetVehicleAssetRef(_vehicleData.VehicleCard);
			}
		}
		[JsonIgnore]
		public GameObject CarPrefab
		{
			get
			{
                if (_cachedCarPrefab == null)
                {
					Log.Error(_vehicleData.VehicleCard.ModelName);
					_cachedCarPrefab = Global.Settings.CardsSetting.GetCardModel(_vehicleData.VehicleCard);
                    //var prototypeAsset = UnityDB.FindAsset<EntityPrototypeAsset>(_carPrototype.Id);
                    //_cachedCarPrefab = prototypeAsset != null && prototypeAsset.Parent != null ? prototypeAsset.Parent.gameObject : null;
                }

                return _cachedCarPrefab;
			}
		}

        internal void SetNewCarSetup(VehicleCard vehicleCard, DriverCard driverCard)
        {
			_vehicleData = new VehicleData(vehicleCard, driverCard);
			_vehicleData.CalculateProperities();
			SaveCarSetup();
		}

		public void InitializeCarSetup()
		{
			_vehicleData.CalculateProperities();
		}


		public void UpdateCarSetup(Card card,int slot, bool isEquip)
        {
			if(card is VehicleCard)
            {
				_vehicleData.SetVehicleCard(card as VehicleCard, isEquip);
			}
			else if (card is DriverCard)
			{
				_vehicleData.SetDriverCard(card as DriverCard, isEquip);
			}
			else if (card is PowerupCard)
			{
				_vehicleData.SetPowerupCard(card as PowerupCard,slot, isEquip);
			}
			else if (card is TechCard)
			{
				_vehicleData.SetTechCard(card as TechCard,slot, isEquip);
			}
			else if (card is AccessoriesCard)
			{
				_vehicleData.SetAccessoriesCard(card as AccessoriesCard, isEquip);
			}
			_vehicleData.CalculateProperities();
        }


		internal void SaveCarSetup()
        {
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

	[Serializable]
	public enum ECarDifficulty
	{
		None,
		Easy,
		Normal,
		Hard,
	}
}
