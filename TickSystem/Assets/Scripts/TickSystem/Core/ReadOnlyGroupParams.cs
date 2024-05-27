
namespace TickSystem.Core.Experimental
{
	public readonly struct ReadOnlyGroupParams
	{
		public string name => _params.name;
		public float interval => _params.interval;
		public bool active => _params.active;
		public bool useRealTime => _params.useRealTime;
		
		private readonly GroupParams _params;
		
		public ReadOnlyGroupParams(ref GroupParams parameters)
		{
			_params = parameters;
		}
	}
}