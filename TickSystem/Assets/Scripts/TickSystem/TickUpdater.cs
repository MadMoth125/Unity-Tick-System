using UnityEngine;

namespace TickSystem
{
	public class TickUpdater : MonoBehaviour
	{
		public static TickUpdater Instance { get; private set; }

		private TickerAsset _tickerAsset;

		public static void InitializeInstance()
		{
			if (Instance != null) return;
			Instance = new GameObject("Tick System Updater").AddComponent<TickUpdater>();
			DontDestroyOnLoad(Instance.gameObject);
		}

		public void SetTickerAsset(TickerAsset tickerAsset)
		{
			if (tickerAsset == null)
			{
				Debug.LogWarning($"The '{nameof(TickerAsset)}' instance is null.");
			}
			_tickerAsset = tickerAsset;
		}

		#region Unity Methods

		private void Update()
		{
			if (_tickerAsset == null) return;
			_tickerAsset.GetTicker().Update(Time.deltaTime, Time.unscaledDeltaTime);
		}

		#endregion
	}
}