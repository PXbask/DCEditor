using System;
using System.Collections.Generic;
using UnityEngine;

namespace DCEditor.Data
{
    public enum DominoCardType
    {
        Front = 1,
        Back = 2,
        Special = 3,
    }
    
    [Serializable]
    public class DominoData
    {
        public DominoCardType type;
        public int id;
        public int layer;
        public List<int> blocks;
        public Vector3 position;
        public Vector3 rotation;

        public DominoData()
        {
            id = 0;
            layer = 0;
            blocks = new List<int>();
        }
    }

    public class DominoDataList
    {
        public List<DominoData> lst;

        public DominoDataList(List<DominoData> lst)
        {
            this.lst = lst;
        }
    }
}
