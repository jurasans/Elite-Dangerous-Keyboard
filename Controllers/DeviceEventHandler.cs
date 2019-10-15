using System;
using Ninject;
using RGB.NET.Core;

namespace EliteKeyboard
{
	public abstract class DeviceEventHandler : IInitializable, IDisposable
	{
		protected readonly IApiService api;
		protected readonly ILedService surface;
		protected IRGBDeviceProvider Provider { get; }

		public DeviceEventHandler(IApiService api, ILedService surface)
		{
			this.api = api;
			this.surface = surface;
			surface.LoadDevices(Provider);

		}

		public void Initialize()
		{
			if (Provider == null)
			{
				Console.Error.WriteLine("no provider for device handler");
				return;
			}
			api.RegisterHardPointsAction(HardpointsEvent);
			api.RegisterShieldEvent(ShieldsEvent);
			api.RegisterHeatWarning(HeatWarningEvent);



		}

		public abstract void ShieldsEvent(EliteAPI.Events.StatusEvent e);
		public abstract void HardpointsEvent(EliteAPI.Events.StatusEvent e);
		public abstract void HeatWarningEvent(EliteAPI.Events.HeatWarningInfo e);

		public void Dispose()
		{
		}
	}
}
