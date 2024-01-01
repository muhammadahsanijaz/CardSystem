using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MoonKart.UI
{
    [Serializable]
    public class PackData
    {
        public int packId;
        public int count;
    }

    [Serializable]
    public class PacksData
    {
        public List<PackData> packsData;

    }

    [Serializable]
    public class PacksLibrary
    {
        public PacksData model;

        public void CreateData()
        {
            model = PersistentStorage.GetObjectWithJsonUtility<PacksData>("packsInventory");
            if (model == null)
            {
                model = new PacksData();
                model.packsData = new List<PackData>();
                SaveData();
            }
        }

        public void SaveData()
        {
            PersistentStorage.SetObjectWithJsonUtility("packsInventory", model, true);
        }

        public void AddPacksInList(int packId, int count)
        {
            if (model == null)
            {
                model = new PacksData();
                model.packsData = new List<PackData>();
            }

            if(model.packsData == null)
            {
                model.packsData = new List<PackData>();
            }
            var packdataIndex = model.packsData.FindIndex(x => x.packId == packId);
            if (packdataIndex != -1)
            {
                model.packsData[packdataIndex].count += count;
            }
            else
            {
                model.packsData.Add( new PackData { packId = packId, count = count });
            }
            
            SaveData();
        }

        public void RemovePacksInList(int packId, int count)
        {
            var packdataIndex = model.packsData.FindIndex(x => x.packId == packId);
            model.packsData[packdataIndex].count -= count;
            if(model.packsData[packdataIndex].count == 0)
            {
                model.packsData.RemoveAt(packdataIndex);
            }
            SaveData();
        }

    }
}
