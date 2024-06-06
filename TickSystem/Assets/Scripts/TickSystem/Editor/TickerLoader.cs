using UnityEditor;
using UnityEngine;

namespace TickSystem.Editor
{
	/// <summary>
	/// This editor class is required to initialize any <see cref="TickerAsset"/> instances during each play mode session.
	///
	/// Since they are scriptable objects,
	/// they only initialize when the editor starts/stops,
	/// not when entering/exiting play mode.
	///
	/// However, packaged builds will not have this issue.
	/// </summary>
	public static class TickerLoader
	{
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		public static void LoadTickerAssets()
		{
			// Event though this is an editor class, it's still a good idea to check if we're in the editor.
			#if UNITY_EDITOR && false
			
			string[] guids = AssetDatabase.FindAssets($"t:{nameof(TickerAsset)}", null);
			if (guids.Length == 0)
			{
				Debug.Log($"No {nameof(TickerAsset)} assets found in project.");
				return;
			}
			
			foreach (string guid in guids)
			{
				var asset = AssetDatabase.LoadAssetAtPath<TickerAsset>(AssetDatabase.GUIDToAssetPath(guid));
				if (asset != null)
				{
					asset.Register();
					asset.GetTicker(); // for-instantiation tickers
				}
			}
			
			#endif
		}
	}
}