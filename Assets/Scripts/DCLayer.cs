using DCEditor.Drawer;
using DCEditor.Utility;
using UnityEditor;
using UnityEngine;

namespace DCEditor
{
    public class DCLayer : MonoBehaviour
    {
        [ReadOnly]
        [SerializeField]
        private int m_layer;

        private Transform Root => transform;

        public int Layer
        {
            get => m_layer;
            set => m_layer = value;
        }

        public void Init(int layer)
        {
            Layer = layer;
        }
        
        /// <summary>
        /// 沿x轴对称
        /// </summary>
        public void SymmetricalAlongX()
        {
            int childCount = Root.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform cardTrans = Root.GetChild(i);
                if(!MyUtility.IsDCCard(cardTrans)) return;
                //在对称位置生成物体
                Vector3 originPos = cardTrans.position;
                Vector3 symmetricalPos = new Vector3(-originPos.x, originPos.y, originPos.z);
                Vector3 originRot = cardTrans.rotation.eulerAngles;
                Vector3 symmetricalRot = new Vector3(originRot.x, 180 - originRot.y, originRot.z);
                GameObject obj = Instantiate(DCEditorMgr.Instance.GetPrefab, Root);
                DCDominoCard card = obj.GetComponent<DCDominoCard>();
                Undo.RegisterCreatedObjectUndo(card.gameObject, "Symmetrical Created");
                card.transform.position = symmetricalPos;
                card.transform.rotation = Quaternion.Euler(symmetricalRot);
            }
        }
        
        /// <summary>
        /// 沿z轴对称
        /// </summary>
        public void SymmetricalAlongZ()
        {
            int childCount = Root.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Transform cardTrans = Root.GetChild(i);
                if(!MyUtility.IsDCCard(cardTrans)) return;
                //在对称位置生成物体
                Vector3 originPos = cardTrans.position;
                Vector3 symmetricalPos = new Vector3(originPos.x, originPos.y, -originPos.z);
                Vector3 originRot = cardTrans.rotation.eulerAngles;
                Vector3 symmetricalRot = new Vector3(originRot.x, -originRot.y, originRot.z);
                GameObject obj = Instantiate(DCEditorMgr.Instance.GetPrefab, Root);
                DCDominoCard card = obj.GetComponent<DCDominoCard>();
                Undo.RegisterCreatedObjectUndo(card.gameObject, "Symmetrical Created");
                card.transform.position = symmetricalPos;
                card.transform.rotation = Quaternion.Euler(symmetricalRot);
            }
        }
    }
}
