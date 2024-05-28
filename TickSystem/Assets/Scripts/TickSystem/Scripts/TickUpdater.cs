using UnityEngine;

namespace TickSystem
{
	[DisallowMultipleComponent]
	public class TickUpdater : MonoBehaviour
	{
		/// <summary>
		/// Singleton instance of the TickUpdater.
		/// </summary>
		public static TickUpdater Instance { get; private set; }
		
		/// <summary>
		/// The TickerAsset currently being managed by the TickUpdater.
		/// </summary>
		public TickerAsset Ticker { get; private set; }
		
		/// <summary>
		/// Initializes the singleton instance of the TickUpdater.
		/// </summary>
		/// <remarks>
		/// See TickUpdater.Instance for result of initialization.
		/// </remarks>
		public static void InitializeInstance()
		{
			if (Instance != null) return;
			Instance = new GameObject("Tick System Updater").AddComponent<TickUpdater>();
			DontDestroyOnLoad(Instance.gameObject);
		}

		/// <summary>
		/// Sets the TickerAsset for the TickUpdater to manage.
		/// </summary>
		/// <param name="tickerAsset">The TickerAsset to set.</param>
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