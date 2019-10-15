using System;
using RGB.NET.Core;
using RGB.NET.Devices.Corsair;

namespace EliteKeyboard
{
	public class MainLedInjector : Ninject.Modules.NinjectModule
	{
		public override void Load()
		{
			Bind<ILedService, IDisposable>().To<LedService>().InSingletonScope();
			Bind<RGBSurface>().ToMethod((c) => RGBSurface.Instance).InSingletonScope();
			Bind<IRGBDeviceProvider>().ToMethod((c) => CorsairDeviceProvider.Instance).InSingletonScope();
			Bind<CorsairDeviceHandler>().ToSelf().InSingletonScope();
		}
	}
}
