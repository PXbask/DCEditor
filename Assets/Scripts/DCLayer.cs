using UnityEngine;

namespace DCEditor
{
    public class DCLayer : MonoBehaviour
    {
        public int Layer { get; private set; }

        public void Init(int layer)
        {
            Layer = layer;
        }
    }
}
