using UnityEngine;

namespace TickSystem
{
	public class TickUpdater : MonoBehaviour
	{
		public static TickUpdater Instance { get; private set; }
		public TickerAsset Ticker { get; private set; }
		
		public static void InitializeInstance()
		{
			if (Instance != null) return;
			Instance = new GameObject("Tick System Updater").AddComponent<TickUpdater>();
			DontDestroyOnLoad(Instance.gameObject);
		}

		public void SetTickerAsset(TickerAsset tickerAsset)
		{
			if (tickerAsset == null) return;
			Ticker = tickerAsset;
		}

		#region Unity Methods

		private void Update()
		{
			if (Ticker == null) return;
			Ticker.GetTicker().Update(Time.deltaTime, Time.unscaledDeltaTime);
		}

		#endregion
	}
}