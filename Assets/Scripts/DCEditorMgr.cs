using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DCEditor.Data;
using DCEditor.Drawer;
using UnityEngine;
using UnityEditor;

namespace DCEditor
{
    [ExecuteAlways]
    public class DCEditorMgr : MonoBehaviour
    {
        private static DCEditorMgr m_instance;
        public static DCEditorMgr Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = FindFirstObjectByType<DCEditorMgr>();
                return m_instance;
            }
        }

        /// <summary>
        /// 需要生成的预制体
        /// </summary>
        [SerializeField] 
        [DisplayName("要摆放的预制体")]
        private DCDominoCard dominoPrefab;
        
        /// <summary>
        /// 棋盘数据
        /// </summary>
        [Drawer.ReadOnly]
        [SerializeField] 
        [Header("棋盘数据")]
        private List<LayerData> m_broadDetails;
        public List<LayerData> BroadDetails
        {
            get => m_broadDetails;
        }
        
        /// <summary>
        /// Mgr物体下的根节点
        /// </summary>
        public Transform Root => transform;
        
        [DisplayName("导出地址(附名称.dclayout)")]
        public string exportPath;
        [DisplayName("导入地址(附名称.dclayout)")]
        public string importPath;
        
        /// <summary>
        /// 刷新棋盘下子物体的所有信息, 并添加到数据中
        /// </summary>
        public void RefreshAllData()
        {
            for (int i = 0; i < Root.childCount; i++)
            {
                m_broadDetails[i].dominos.Clear();
                
                Transform trans = Root.GetChild(i);
                DCLayer dclayer = trans.GetComponent<DCLayer>();
                for (int j = 0; j < trans.childCount; j++)
                {
                    Transform tr = trans.GetChild(j);
                    DCDominoCard card = tr.GetComponent<DCDominoCard>();
                    card.Init(i * 10000 + j, dclayer.Layer);
                    m_broadDetails[i].dominos.Add(card.Data);
                }
            }
        }

        /// <summary>
        /// 获取层级之间所有遮挡关系
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

        #region 修改层级
        
        /// <summary>
        /// 修改层级
        /// </summary>
        /// <param name="count">层级数</param>
        public void UpdateLayerDetails(int count)
        {
            UpdateLayerRefreshInspector(count);
            UpdateLayerRefreshHierarchy(count);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void UpdateLayerRefreshInspector(int count)
        {
            //当棋盘信息没有被赋值时
            if (m_broadDetails == null)
            {
                m_broadDetails = new List<LayerData>(count);
                for (int i = 0; i < count; i++)
                {
                    m_broadDetails.Add(new LayerData());
                }
            }
            //当棋盘信息已经有其他值时，会复用之前的部分数据
            else
            {
                List<LayerData> tmplst = new List<LayerData>(m_broadDetails);
                m_broadDetails = new List<LayerData>(count);
                for (int i = 0; i < count; i++)
                {
                    if (i < tmplst.Count)
                    {
                        m_broadDetails.Add(tmplst[i]);
                    }
                    else
                    {
                        m_broadDetails.Add(new LayerData());
                    }
                }
            }
        }
        
        /// <summary>
        /// 刷新Hierarchy结构
        /// </summary>
        private void UpdateLayerRefreshHierarchy(int count)
        {
            int childCount = Root.childCount;
            //多余的层级删掉
            if (count < childCount)
            {
                for (int i = count; i < childCount; i++)
                {
                    DestroyImmediate(Root.GetChild(count));
                }
            }
            //缺少的层级会添加
            if (count > childCount)
            {
                for (int i = childCount; i < count; i++)
                {
                    GameObject newObj = new GameObject($"layer_{i}");
                    newObj.transform.SetParent(Root);
                    Vector3 pos = newObj.transform.position;
                    Vector3 extents = dominoPrefab.GetComponent<Renderer>().bounds.extents;
                    newObj.transform.position = new Vector3(pos.x, (2 * i + 1) * extents.y, pos.z);

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

        /// <summary>
        /// 重置数据信息
        /// </summary>
        private void ResetBoardRefreshInspector()
        {
            m_broadDetails = null;
        }

        /// <summary>
        /// 重置Hierarchy信息
        /// </summary>
        private void ResetBoardRefreshHierarchy()
        {
            for (int i = Root.childCount - 1; i >= 0 ; i--)
            {
                DestroyImmediate(Root.GetChild(i).gameObject);
            }
        }

        #endregion

        #region 导入配置

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

        /// <summary>
        /// 导入操作
        /// </summary>
        private void DoImport(DominoDataList cfg)
        {
            int max = cfg.lst.Max(x => x.layer);

            UpdateLayerDetails(max + 1);
            foreach (var item in cfg.lst)
            {
                Transform trans = Root.GetChild(item.layer);
                DCDominoCard card = Instantiate(dominoPrefab, trans);
                card.Init(item.id, item.layer);
                m_broadDetails[item.layer].dominos.Add(card.Data);

                card.transform.position = item.position;
                card.transform.rotation = Quaternion.Euler(item.rotation);
                card.m_type = item.type;
            }
            
            RefreshAllData();
        }

        #endregion
    }
}
