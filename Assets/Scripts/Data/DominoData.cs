using System;
using System.Collections.Generic;
using UnityEngine;

namespace DCEditor.Data
{
    [Serializable]
    public class DominoData
    {
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
