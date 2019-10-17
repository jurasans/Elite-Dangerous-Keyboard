using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Ninject;
using Ninject.Modules;
using RGB.NET.Core;
using RGB.NET.Groups;

namespace EliteKeyboard
{
	class Program
	{
		static void Main(string[] args)
		{
			Dictionary<string, NinjectModule> manufecturers = new Dictionary<string, NinjectModule>
			{
				{ "corsair" , new CorsairInjector() }
			};
#if DEBUG
			args = new[] { "t" };
#endif
			if (args[0] == "help")
			{
				Console.WriteLine("-t = test led implementation");
				Console.WriteLine("-r = run elite dangerous keyboard software.");
				Console.WriteLine("ManufecturerA,ManufecturerB = list of manufecturers seperated by comma");
				Console.WriteLine("-currently supports :");
				Console.WriteLine("-corsair :");
			}
			if (args[0] == "t")
			{
				RunLedTest();
			}
			if (args[0] == ("r"))
			{
				try
				{
					var ManufecturersInjectors = args[1].Split(',').Select(manufecturer => manufecturers[manufecturer]).ToList();
					RunProgram(ManufecturersInjectors);
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}


		}
		private static void RunProgram(List<NinjectModule> injectors)
		{
			injectors.Add(new MainLedInjector());
			injectors.Add(new ApiInjector());

			var kernel = new StandardKernel(injectors.ToArray());
			using (var ledService = kernel.Get<ILedService>())
			using (var apiService = kernel.Get<IApiService>())

			using (var corsair = kernel.Get<CorsairDeviceHandler>())
			{
				Console.Write("ok go away now");
			}
		}
		private static void RunLedTest()
		{
			var kernel = new StandardKernel(new MainLedInjector());
			using (var service = kernel.Get<ILedService>())
			{
				var surface = kernel.Get<RGBSurface>();
				var provider = kernel.Get<IRGBDeviceProvider>();
				service.LoadDevices(provider);
				service.InitializeDevices();

				ILedGroup lightBar = new ListLedGroup(surface.Leds.Where(led => led.LedRectangle.Location.Y <= 6));


				ConsoleKeyInfo key;
				do
				{
					key = Console.ReadKey();
					switch (key.Key)
					{
						case ConsoleKey.NumPad0:
							service.Blink();
							break;
						case ConsoleKey.NumPad1:
							service.Blink(lightBar.GetLeds());
							break;
						case ConsoleKey.NumPad2:
							service.Wave(Direction.DownTop);
							break;
						case ConsoleKey.NumPad3:
							service.Wave(Direction.LeftRight);
							break;
						case ConsoleKey.NumPad4:
							service.Wave(Direction.TopDown);
							break;
						case ConsoleKey.NumPad5:
							service.Wave(Direction.RightLeft);
							break;
						default:
							break;
					}
				}
				while (key.Key != ConsoleKey.Escape);
			}
		}
	}
}
