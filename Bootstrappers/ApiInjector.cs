using EliteAPI;

namespace EliteKeyboard
{
	public class ApiInjector : Ninject.Modules.NinjectModule
	{
		public override void Load()
		{
			Bind<string>().ToConstant(Properties.Settings.Default.Properties["Setting_path"].DefaultValue.ToString());
			Bind<IEliteDangerousAPI>().To<EliteDangerousAPI>().InSingletonScope();
			Bind<IApiService>().To<ApiService>().InSingletonScope();
		}
	}
}
