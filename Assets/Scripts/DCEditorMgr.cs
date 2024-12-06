using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DCEditor.Data;
using DCEditor.Drawer;
using UnityEngine;
using SimpleJSON;
using UnityEditor;

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
        [Drawer.ReadOnly]
        [SerializeField] 
        [Header("棋盘数据")]
        private List<LayerData> broadDetails;

        public List<LayerData> BroadDetails => broadDetails;
        
        public Transform Root => transform;
        
        [DisplayName("导出地址(附名称.dclayout)")]
        public string exportPath;
        [DisplayName("导入地址(附名称.dclayout)")]
        public string importPath;

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

                    DCLayer lr = newObj.AddComponent<DCLayer>();
                    lr.Init(i);
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
                DCLayer dclayer = trans.GetComponent<DCLayer>();
                for (int j = 0; j < trans.childCount; j++)
                {
                    Transform tr = trans.GetChild(j);
                    DCDominoCard card = tr.GetComponent<DCDominoCard>();
                    card.Init(i * 10000 + j, dclayer.Layer);
                    broadDetails[i].dominos.Add(card.Data);
                }
            }
        }

        /// <summary>
        /// 获取所有遮挡关系
        /// </summary>
        public void GetAllColliderRelation()
        {
            for (int i = 0; i < Root.childCount; i++)
            {
                Transform trans = Root.GetChild(i);
                for (int j = 0; j < trans.childCount; j++)
                {
                    Transform tr = trans.GetChild(j);
                    DCDominoCard card = tr.GetComponent<DCDominoCard>();
                    card.UpdateColliderData();
                }
            }
            EditorUtility.DisplayDialog("", "生成遮挡关系成功", "ok");
        }

        /// <summary>
        /// 导出配置
        /// </summary>
        public void ExportCfg()
        {
            List<DominoData> datas = new List<DominoData>();
            for (int i = 0; i < Root.childCount; i++)
            {
                Transform trans = Root.GetChild(i);
                for (int j = 0; j < trans.childCount; j++)
                {
                    Transform tr = trans.GetChild(j);
                    DCDominoCard card = tr.GetComponent<DCDominoCard>();
                    datas.Add(card.Data);
                }
            }

            DominoDataList lst = new DominoDataList(datas);
            string json = JsonUtility.ToJson(lst);

            bool res = true;
            if (File.Exists(exportPath))
            {
                res = EditorUtility.DisplayDialog("", "文件已存在，是否覆盖？", "是", "否");
            }
            if (res)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(exportPath))
                    {
                        writer.Write(json);
                        EditorUtility.DisplayDialog("", "导出成功", "ok");
                    }
                }
                catch (Exception ex)
                {
                    EditorUtility.DisplayDialog("", "导出操作取消：" + ex, "ok");
                }
            }
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 导入配置
        /// </summary>
        public void ImportCfg()
        {
            ResetBoard();
            
            DominoDataList lst;
            try
            {
                using (StreamReader reader = new StreamReader(importPath))
                {
                    string data = reader.ReadToEnd();
                    lst = JsonUtility.FromJson<DominoDataList>(data);
                    if (lst == null)
                    {
                        EditorUtility.DisplayDialog("", "这不是一个正确格式的配置", "ok");
                    }
                    else
                    {
                        EditorUtility.DisplayDialog("", "导入成功", "ok");
                        DoImport(lst);
                    }
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        private void DoImport(DominoDataList cfg)
        {
            int max = cfg.lst.Max(x => x.layer);

            UpdateLayerDetails(max + 1);
            foreach (var item in cfg.lst)
            {
                Transform trans = Root.GetChild(item.layer);
                DCDominoCard card = Instantiate(dominoPrefab, trans);
                card.Init(item.id, item.layer);
                broadDetails[item.layer].dominos.Add(card.Data);

                card.transform.position = item.position;
                card.transform.rotation = Quaternion.Euler(item.rotation);
                card.m_type = item.type;
            }
            
            RefreshInspector();
        }
    }
}
