using System;
using System.Collections.Generic;
using UnityEngine;

namespace DCEditor.Data
{
    [Serializable]
    public class LayerData
    {
        public List<DominoData> dominos;

        public LayerData()
        {
            dominos = new List<DominoData>();
        }
    }
}
