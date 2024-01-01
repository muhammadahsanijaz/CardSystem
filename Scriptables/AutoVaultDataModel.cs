using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoonKart
{
    [Serializable]
    public class AutoVaultModel
    {
        public int slotIndex;
        public int cardId;
        public string time;
        

        public AutoVaultModel()
        {
            slotIndex = -1;
            cardId = -1;
            time = "";
        }

        public AutoVaultModel(int slotIndex, int cardId, string time)
        {
            this.slotIndex = slotIndex;
            this.cardId = cardId;
            this.time = time;
        }
    }

    [Serializable]
    public class AutoVaultData
    {
        public AutoVaultModel[] slotsData;
        public string _totalFuelTime = "";
        public string _currentFuelTime = "";
        public string _lastTimeFuelGenerated = "";
        public int totalFuel;
        public int FuelUnits;

        public DateTime LastTimeFuelGenerated
        {
            get
            {

                if (_lastTimeFuelGenerated == "")
                {
                    return default;
                }
                else
                {
                    try
                    {
                        return DateTime.Parse(_lastTimeFuelGenerated);
                    }
                    catch
                    {
                        Debug.LogError("Invalid Time" + _lastTimeFuelGenerated);
                        return DateTime.UtcNow;
                    }
                }
            }
            set => _lastTimeFuelGenerated = value.ToString("MM/dd/yyyy HH:mm:ss");
        }


        public string LastTimeFuelGeneratedInString
        {
            get { return _lastTimeFuelGenerated; }
            set
            {
                _lastTimeFuelGenerated = value;
            }
        }

        public TimeSpan TotalFuelTime
        {
            get
            {

                if (_totalFuelTime == "")
                {
                    return default;
                }
                else
                {
                    try
                    {
                        return TimeSpan.Parse(_totalFuelTime);
                    }
                    catch
                    {
                        Debug.LogError("Invalid Time" + _totalFuelTime);
                        return default;
                    }
                }
            }
            set => TotalFuelTimeInString = value.ToString("dd\\.hh\\:mm\\:ss");
        }


        public string TotalFuelTimeInString
        {
            get { return _totalFuelTime; }
            set
            {
                _totalFuelTime = value;
            }
        }

        public TimeSpan CurrentFuelTime
        {
            get
            {

                if (_currentFuelTime == "")
                {
                    return default;
                }
                else
                {
                    try
                    {
                        return TimeSpan.Parse(_currentFuelTime);
                    }
                    catch
                    {
                        Debug.LogError("Invalid Time" + _currentFuelTime);
                        return default;
                    }
                }
            }
            set => CurrentFuelTimeInString = value.ToString("dd\\.hh\\:mm\\:ss");
        }


        public string CurrentFuelTimeInString
        {
            get { return _currentFuelTime; }
            set
            {
                _currentFuelTime = value;
            }
        }
    }

    [Serializable]
    public class AutoVaultDataModel
    {
        public AutoVaultData autoVaultData;

        public AutoVaultDataModel()
        {
            // if (PersistentStorage.IsSave("AutoVaultDataModel"))
            // {
            //     autoVaultData = PersistentStorage.GetObjectWithJsonUtility<AutoVaultData>("AutoVaultDataModel");
            // }
            // else
            // {
            //     CreateData();
            // }
        }


        public void CreateData()
        {
            
            autoVaultData = PersistentStorage.GetObjectWithJsonUtility<AutoVaultData>("AutoVaultDataModel");
            if (autoVaultData == null)
            {
                autoVaultData = new AutoVaultData();
                autoVaultData.slotsData = new AutoVaultModel[6];
                SaveData();
            }

        }

        public void SaveData()
        {
            PersistentStorage.SetObjectWithJsonUtility("AutoVaultDataModel", autoVaultData, true);
        }

        public void AddAutoVaultStateInList(int slotNumber, int cardID, string utc)
        {
            autoVaultData.slotsData[slotNumber].slotIndex = slotNumber;
            autoVaultData.slotsData[slotNumber].cardId = cardID;
            autoVaultData.slotsData[slotNumber].time = utc;
            SaveData();
        }

        public void RemoveAutoVaultStateInList(int slotNumber)
        {
            foreach (var data in autoVaultData.slotsData)
            {
                if (data.slotIndex == slotNumber)
                {
                    autoVaultData.slotsData[slotNumber].slotIndex = -1;
                    autoVaultData.slotsData[slotNumber].cardId = -1;
                    autoVaultData.slotsData[slotNumber].time = "";
                }
            }

            SaveData();
        }
    }
}