using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;

namespace DCEditor
{
    [ExecuteAlways]
    public class DCEditorMgr : MonoBehaviour
    {
        [SerializeField] private GameObject dominoPrefab;
        
        public int LayerCount { get; set; }

        [SerializeField] private List<LayerDetail> layerDetails;

        public void CreateNew()
        {
            Instantiate(dominoPrefab);
        }
        
        public void UpdateLayerDetails(int count)
        {
            layerDetails = new List<LayerDetail>(count);
            for (int i = 0; i < count; i++)
            {
                layerDetails.Add(null);
            }
        }

        [Serializable]
        public class LayerDetail
        {
            public List<DominoDetail> dominos;
        }
        
        [Serializable]
        public class DominoDetail
        {
            public uint id;
            public uint layer;
            public List<uint> blocks;
        }
    }
}
