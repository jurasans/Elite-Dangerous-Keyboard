using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RGB.NET.Core;

namespace EliteKeyboard
{
	public class LedService : ILedService, IDisposable
	{
		List<IRGBDeviceProvider> providers = new List<IRGBDeviceProvider>();
		private readonly RGBSurface surface;

		public LedService(RGBSurface surface)
		{
			this.surface = surface;
		}
		public void LoadDevices(IRGBDeviceProvider provider)
		{
			providers.Add(provider);
		}

		protected void LoadDevices(RGBSurface surface, IRGBDeviceProvider deviceProvider)
		{
			surface.LoadDevices(deviceProvider, RGBDeviceType.Keyboard | RGBDeviceType.LedMatrix
											  | RGBDeviceType.Mousepad | RGBDeviceType.LedStripe
											  | RGBDeviceType.Mouse | RGBDeviceType.Headset
											  | RGBDeviceType.HeadsetStand);
		}
		public void InitializeDevices()
		{
			for (int i = 0; i < providers.Count; i++)
			{
				LoadDevices(surface, providers[i]);
			}

		}

		public void Blink(IEnumerable<Led> leds = null)
		{
			PlayBlinkAnimation(5, 200, leds ?? surface.Leds);
		}
		public void Wave(Direction direction, IEnumerable<Led> leds = null)
		{
			Wave(direction, 5, 10, 1, leds ?? surface.Leds);
		}
		private async void PlayBlinkAnimation(int times, int delayMs, IEnumerable<Led> leds)
		{
			for (int i = 0; i < times; i++)
			{

				foreach (var led in leds)
				{
					led.Color = new Color(0, 0, 0);
				}
				surface.Update(true);

				await Task.Delay(delayMs);

				foreach (var led in leds)
				{
					led.Color = new Color(255, 255, 255);
				}
				surface.Update(true);
				await Task.Delay(delayMs);
			}

		}

		private async void Wave(Direction direction, int times, int rate, int size, IEnumerable<Led> leds = null)
		{

		}
		public void Dispose()
		{
			surface.Dispose();
			for (int i = 0; i < providers.Count; i++)
			{
				providers[i].Dispose();
			}
		}

	}
}
