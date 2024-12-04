using System;
using DCEditor.Utility;
using UnityEngine;
using QFramework;

namespace DCEditor.ViewCtrl
{
	public partial class InspectorSection : ViewController
	{
		[SerializeField] private layerDetail layerDetailPrefab;
		
		void Start()
		{
			Init();
		}

		private void Init()
		{
			RgstEvt();
		}

		private void RgstEvt()
		{
			layNumInput.onSubmit.RemoveAllListeners();
			layNumInput.onSubmit.AddListener(OnInputFieldSubmit);
		}

		private void OnInputFieldSubmit(string str)
		{
			if (int.TryParse(str, out int v))
			{
				if (v >= 0)
				{
					layerContent.DestroyChildren();
					MyUtility.InstantiateMultiple(layerDetailPrefab, v,
						(index, t) => { t.transform.SetParent(layerContent); });
					return;
				}
			}
			Debug.LogError("请输入正整数");
		}
	}
}
