using EliteAPI.Events;

namespace EliteKeyboard
{
	public class CorsairDeviceHandler : DeviceEventHandler
	{
		public CorsairDeviceHandler(IApiService api, ILedService surface) : base(api, surface)
		{
		}

		public override void ShieldsEvent(StatusEvent e)
		{

		}

		public override void HardpointsEvent(StatusEvent e)
		{

		}

		public override void HeatWarningEvent(HeatWarningInfo e)
		{

		}
	}
}
