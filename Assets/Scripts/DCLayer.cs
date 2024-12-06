using DCEditor.Drawer;
using UnityEngine;

namespace DCEditor
{
    public class DCLayer : MonoBehaviour
    {
        [ReadOnly]
        [SerializeField]
        private int m_layer;

        public int Layer
        {
            get => m_layer;
            set => m_layer = value;
        }

        public void Init(int layer)
        {
            Layer = layer;
        }
    }
}
