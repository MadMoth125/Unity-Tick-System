using System;
using System.Collections.Generic;
using TickSystem.Core;
using UnityEngine;

namespace TickSystem
{
	[CreateAssetMenu(fileName = "NewTicker", menuName = "Tick System/Ticker")]
	public class TickerAsset : ScriptableObject
	{
		public bool Active
		{
			get => active;
			set
			{
				if (active == value) return;
				active = value;
				UpdateParameters();
			}
		}
		
		public IReadOnlyList<TickGroupAsset> TickGroupAssets => tickGroups;

		private static List<TickerAsset> TickerAssets { get; } = new List<TickerAsset>();

		[Tooltip("Whether the ticker is active.")]
		[SerializeField]
		private bool active = true;

		[Tooltip("The tick groups managed by this ticker.")]
		[SerializeField]
		private List<TickGroupAsset> tickGroups;

		private Ticker _ticker;

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitializeSystem()
		{
			TickUpdater.InitializeInstance();
			TickUpdater.Instance.SetTickerAsset(TickerAssets[0]);
		}
		
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

		private void UpdateParameters()
		{
			if (!Application.isPlaying) return;
			if (_ticker == null) return;
			_ticker.active = active;
		}
		
		#region Unity Methods

		private void OnValidate()
		{
			UpdateParameters();
			
			for (int i = 0; i < tickGroups.Count; i++)
			{
				if (tickGroups[i] != null) continue;
				tickGroups.RemoveAt(i);
				i--;
			}
		}

		private void OnEnable()
		{
			if (TickerAssets.Contains(this)) return;
			TickerAssets.Add(this);
		}

		private void OnDisable()
		{
			_ticker?.Clear();
			
			if (!TickerAssets.Contains(this)) return;
			TickerAssets.Remove(this);
		}

		#endregion
	}
}