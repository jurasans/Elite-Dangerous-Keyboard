namespace EliteKeyboard
{
	public class CorsairInjector : Ninject.Modules.NinjectModule
	{
		public override void Load()
		{
			Bind<CorsairDeviceHandler>().ToSelf();
		}
	}
}
