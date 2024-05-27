using System;
using System.Collections.Generic;
using TickSystem.Core;
using UnityEngine;

namespace TickSystem
{
	[CreateAssetMenu(fileName = "NewTicker", menuName = "Tick System/Ticker")]
	public class TickerAsset : ScriptableObject
	{
		public IReadOnlyList<TickGroupAsset> TickGroupAssets => tickGroups;

		[Tooltip("Whether the ticker is active.")]
		[SerializeField]
		public bool active = true;

		[Tooltip("The tick groups managed by this ticker.")]
		[SerializeField]
		private List<TickGroupAsset> tickGroups;

		private Ticker _ticker;

		public void Add(TickGroupAsset tickGroup)
		{
			if (tickGroup == null) return;
			if (tickGroups.Contains(tickGroup)) return;
			tickGroups.Add(tickGroup);
			GetTicker().Add(tickGroups[^1].GetTickGroup());
		}

		public void Remove(TickGroupAsset tickGroup)
		{
			if (tickGroup == null) return;
			if (!tickGroups.Contains(tickGroup)) return;
			tickGroups.Remove(tickGroup);
			GetTicker().Remove(tickGroup.GetTickGroup());
		}

		public void ReloadTickGroups()
		{
			GetTicker().SetTickGroups(tickGroups.ConvertAll(tickGroup => tickGroup.GetTickGroup()));
		}

		public Ticker GetTicker()
		{
			if (_ticker != null) return _ticker;
			_ticker = new Ticker(active, tickGroups.ConvertAll(tickGroup => tickGroup.GetTickGroup()));
			return _ticker;
		}

		#region Unity Methods

		private void OnValidate()
		{
			for (int i = 0; i < tickGroups.Count; i++)
			{
				if (tickGroups[i] != null) continue;
				tickGroups.RemoveAt(i);
				i--;
			}
		}

		private void OnDisable()
		{
			_ticker?.Clear();
		}

		#endregion
	}
}