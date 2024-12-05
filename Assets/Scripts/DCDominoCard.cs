using System;
using DCEditor.Data;
using UnityEngine;

namespace DCEditor
{
    public class DCDominoCard : MonoBehaviour
    {
        public DominoData Data { get; set; }

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

        public void Init(int id)
        {
            Data = new DominoData();
            Data.id = id;
            Data.layer = -1;
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
    }
}

