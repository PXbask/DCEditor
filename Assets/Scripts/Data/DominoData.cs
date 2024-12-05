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
        public List<uint> blocks;
    }
}
