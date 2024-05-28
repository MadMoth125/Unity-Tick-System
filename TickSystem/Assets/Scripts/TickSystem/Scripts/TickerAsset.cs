using System;
using System.Collections.Generic;
using TickSystem.Core;
using UnityEditor;
using UnityEngine;

namespace TickSystem
{
	[CreateAssetMenu(fileName = "NewTicker", menuName = "Tick System/Ticker")]
	public class TickerAsset : ScriptableObject
	{
		/// <summary>
		/// Whether the ticker is active.
		/// </summary>
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
		
		/// <summary>
		/// The tick group assets managed by this ticker asset.
		/// </summary>
		public IReadOnlyList<TickGroupAsset> TickGroupAssets => tickGroups;

		/// <summary>
		/// The instances of any ticker assets.
		/// </summary>
		private static List<TickerAsset> TickerAssets { get; } = new List<TickerAsset>();

		[Tooltip("Whether the ticker is active.")]
		[SerializeField]
		private bool active = true;

		[Tooltip("The tick groups managed by this ticker.")]
		[SerializeField]
		private List<TickGroupAsset> tickGroups;

		private Ticker _ticker;

		// Initialize the tick updater before any scenes load.
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void InitializeSystem()
		{
			TickUpdater.InitializeInstance();
			TickUpdater.Instance.SetTickerAsset(TickerAssets[0]);
		}
		
		/// <summary>
		/// Adds a tick group asset to the ticker.
		/// </summary>
		/// <param name="tickGroup">The tick group asset to add.</param>
		public void Add(TickGroupAsset tickGroup)
		{
			if (tickGroup == null) return;
			if (tickGroups.Contains(tickGroup)) return;
			tickGroups.Add(tickGroup);
			GetTicker().Add(tickGroups[^1].GetTickGroup());
		}

		/// <summary>
		/// Removes a tick group asset from the ticker.
		/// </summary>
		/// <param name="tickGroup">The tick group asset to remove.</param>
		public void Remove(TickGroupAsset tickGroup)
		{
			if (tickGroup == null) return;
			if (!tickGroups.Contains(tickGroup)) return;
			tickGroups.Remove(tickGroup);
			GetTicker().Remove(tickGroup.GetTickGroup());
		}

		/// <summary>
		/// Reloads the tick groups for the ticker.
		/// </summary>
		public void ReloadTickGroups()
		{
			GetTicker().SetTickGroups(tickGroups.ConvertAll(tickGroup => tickGroup.GetTickGroup()));
		}

		/// <summary>
		/// Gets the ticker instance.
		/// </summary>
		/// <returns>The ticker instance.</returns>
		public Ticker GetTicker()
		{
			if (_ticker != null) return _ticker;
			_ticker = new Ticker(active, tickGroups.ConvertAll(tickGroup => tickGroup.GetTickGroup()));
			return _ticker;
		}

		/// <summary>
		/// Updates the parameters of the ticker with the most recent values.
		/// </summary>
		private void UpdateParameters()
		{
			if (Application.isPlaying && _ticker != null)
			{
				_ticker.active = active;
			}
		}
		
		#region Unity Methods

		private void OnValidate()
		{
			#if UNITY_EDITOR
			
			UpdateParameters();
			
			for (int i = 0; i < tickGroups.Count; i++)
			{
				if (tickGroups[i] != null) continue;
				tickGroups.RemoveAt(i);
				i--;
			}
			
			#endif
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

		#if UNITY_EDITOR
		
		[ContextMenu("Load all " + nameof(TickGroupAsset) + " instances.")]
		private void FindAllTickGroupAssets()
		{
			if (Application.isPlaying)
			{
				Debug.LogWarning("Cannot load TickGroupAssets during play mode.");
				return;
			}
			
			string[] guids = AssetDatabase.FindAssets($"t:{nameof(TickGroupAsset)}", null);
			if (guids.Length == 0)
			{
				Debug.Log($"No {nameof(TickGroupAsset)} assets found in project.");
				return;
			}

			foreach (string guid in guids)
			{
				Add(AssetDatabase.LoadAssetAtPath<TickGroupAsset>(AssetDatabase.GUIDToAssetPath(guid)));
			}
			
		}
		
		#endif
	}
}