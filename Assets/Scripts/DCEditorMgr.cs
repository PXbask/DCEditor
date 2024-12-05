using System;
using System.Collections.Generic;
using System.ComponentModel;
using DCEditor.Data;
using DCEditor.Drawer;
using QFramework;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.Serialization;

namespace DCEditor
{
    [ExecuteAlways]
    public class DCEditorMgr : MonoBehaviour
    {
        public static DCEditorMgr instance;

        public static DCEditorMgr Instance
        {
            get
            {
                if (instance == null)
                    instance = FindFirstObjectByType<DCEditorMgr>();
                return instance;
            }
        }

        /// <summary>
        /// 需要生成的预制体
        /// </summary>
        [SerializeField] 
        [DCEditor.Drawer.DisplayName("预制体")]
        private DCDominoCard dominoPrefab;
        
        /// <summary>
        /// 层级数
        /// </summary>
        public int LayerCount { get; set; }
        
        /// <summary>
        /// 棋盘数据
        /// </summary>
        [SerializeField] 
        [Header("棋盘数据")]
        private List<LayerData> broadDetails;

        public List<LayerData> BroadDetails => broadDetails;
        
        public Transform Root => transform;

        private static int idNum = 0;

        #region 修改层级
        public void UpdateLayerDetails(int count)
        {
            UpdateLayerRefreshInspector(count);
            UpdateLayerRefreshHierarchy(count);
        }

        public void UpdateLayerRefreshInspector(int count)
        {
            if (broadDetails == null)
            {
                broadDetails = new List<LayerData>(count);
                for (int i = 0; i < count; i++)
                {
                    broadDetails.Add(new LayerData());
                }
            }
            else
            {
                List<LayerData> tmplst = new List<LayerData>(broadDetails);
                broadDetails = new List<LayerData>(count);
                for (int i = 0; i < count; i++)
                {
                    if (i < tmplst.Count)
                    {
                        broadDetails.Add(tmplst[i]);
                    }
                    else
                    {
                        broadDetails.Add(new LayerData());
                    }
                }
            }
        }
        
        private void UpdateLayerRefreshHierarchy(int count)
        {
            int childCount = Root.childCount;
            //多余的删掉
            if (count < childCount)
            {
                for (int i = count; i < childCount; i++)
                {
                    DestroyImmediate(Root.GetChild(count));
                }
            }
            //少的添加
            if (count > childCount)
            {
                for (int i = childCount; i < count; i++)
                {
                    GameObject newObj = new GameObject($"layer_{i}");
                    newObj.transform.SetParent(Root);
                    Vector3 pos = newObj.transform.position;
                    Vector3 bound = dominoPrefab.GetComponent<Renderer>().bounds.extents;
                    newObj.transform.position = new Vector3(pos.x, (2 * i + 1) * bound.y, pos.z);
                }
            }
        }
        #endregion

        #region 重置棋盘

        /// <summary>
        /// 重置棋盘
        /// </summary>
        public void ResetBoard()
        {
            ResetBoardRefreshInspector();
            ResetBoardRefreshHierarchy();
        }

        private void ResetBoardRefreshInspector()
        {
            broadDetails = null;
            idNum = 0;
        }

        private void ResetBoardRefreshHierarchy()
        {
            for (int i = Root.childCount - 1; i >= 0 ; i--)
            {
                DestroyImmediate(Root.GetChild(i).gameObject);
            }
        }

        #endregion

        /// <summary>
        /// 刷新棋盘下子物体们的所有信息
        /// </summary>
        public void RefreshInspector()
        {
            for (int i = 0; i < Root.childCount; i++)
            {
                broadDetails[i].dominos.Clear();
                
                Transform trans = Root.GetChild(i);
                for (int j = 0; j < trans.childCount; j++)
                {
                    Transform tr = trans.GetChild(j);
                    DominoData data = tr.GetComponent<DCDominoCard>().Data;
                    broadDetails[i].dominos.Add(data);
                }
            }
        }
    }
}
