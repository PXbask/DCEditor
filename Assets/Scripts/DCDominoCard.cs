using System;
using DCEditor.Data;
using DCEditor.Drawer;
using UnityEngine;

namespace DCEditor
{
    [ExecuteAlways]
    public class DCDominoCard : MonoBehaviour
    {
        [SerializeField]
        private Material mat_front;
        
        [SerializeField]
        private Material mat_back;
        
        [SerializeField]
        private Material mat_special;
        
        [SerializeField]
        [Header("骨牌类型")] 
        public DominoCardType m_type;
        
        [ReadOnly]
        [SerializeField]
        [Header("骨牌数据")]
        private DominoData m_data;

        public DominoData Data
        {
            get => m_data;
            set => m_data = value;
        }

        public int Id
        {
            get => Data.id;
            set => Data.id = value;
        }
        
        public int Layer
        {
            get => Data.layer;
            set => Data.layer = value;
        }

        private bool operationCancelled;

        public void Init(int id, int layer)
        {
            Data = new DominoData();
            Data.id = id;
            Data.layer = layer;
            Data.type = DominoCardType.Front;

            UpdateObjData();
        }

        private void OnValidate()
        {
            UpdateObjData();
        }

        private void UpdateObjData()
        {
            UpdateDominoType();
        }

        private void UpdateDominoType()
        {
            Data.type = m_type;

            MeshRenderer renderer = GetComponent<MeshRenderer>();
            switch (Data.type)
            {
                case DominoCardType.Front:
                    renderer.material = mat_front;
                    break;
                case DominoCardType.Back:
                    renderer.material = mat_back;
                    break;
                case DominoCardType.Special:
                    renderer.material = mat_special;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Update()
        {
            Vector3 pos = transform.localPosition;
            pos = new Vector3(pos.x, 0, pos.z);
            transform.localPosition = pos;
            
            Data.position = transform.position;
            Data.rotation = transform.rotation.eulerAngles;
        }

        public void UpdateLayer(int v)
        {
            UpdateLayerRefreshData(v);
            UpdateLayerRefreshHierarchy(v);
        }

        private void UpdateLayerRefreshData(int v)
        {
            if(v == Layer) return;
            operationCancelled = false;
            
            var lst = DCEditorMgr.Instance.BroadDetails;
            //清除之前的
            if (lst[Layer].dominos.Contains(Data))
            {
                lst[Layer].dominos.Remove(Data);
            }
            //处理新数据
            if (v >= lst.Count)
            {
                operationCancelled = true;
                Debug.LogError("层级超出范围");
            }
            else
            {
                Layer = v;
                lst[v].dominos.Add(Data);
            }
        }
        
        private void UpdateLayerRefreshHierarchy(int v)
        {
            if(operationCancelled) return;
            
            var rt = DCEditorMgr.Instance.Root;
            var par = rt.GetChild(Layer);
            transform.SetParent(par);
            
            var pos = transform.position;
            pos = new Vector3(pos.x, 0, pos.z);
            transform.position = pos;
        }

        /// <summary>
        /// 当移除本物体时
        /// </summary>
        public void DestroyObj()
        {
            var lst = DCEditorMgr.Instance.BroadDetails;
            if (lst[Layer].dominos.Contains(Data))
            {
                lst[Layer].dominos.Remove(Data);
                DestroyImmediate(gameObject);
            }
        }

        /// <summary>
        /// 获取遮挡关系
        /// </summary>
        public void UpdateColliderData()
        {
            Vector3 extent = GetComponent<Renderer>().bounds.extents;
            Vector3 halfExtents = new Vector3(extent.x, extent.y + 0.01f, extent.z);
            var res = Physics.OverlapBox(transform.position, halfExtents);
            Data.blocks.Clear();
            
            foreach (var r in res)
            {
                GameObject obj = r.gameObject;
                if (obj.transform.position.y < gameObject.transform.position.y)
                {
                    DCDominoCard card = obj.GetComponent<DCDominoCard>();
                    if (card != null && card != this && card.Data.layer == Data.layer - 1)
                    {
                        Data.blocks.Add(card.Data.id);
                    }
                }
            }
        }
    }
}

