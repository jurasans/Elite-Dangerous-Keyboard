using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RGB.NET.Core;

namespace EliteKeyboard
{
	public class LedService : ILedService, IDisposable
	{
		List<IRGBDeviceProvider> providers = new List<IRGBDeviceProvider>();
		private Dictionary<int, Led[]> yOrientedLeds;
		private Dictionary<int, Led[]> xOrientedLeds;
		private int[] yKeyCollection;
		private int[] xKeyCollection;
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

		public void Blink(IEnumerable<Led> leds = null, int times = 5, int delayMs = 200)
		{
			PlayBlinkAnimation(times, delayMs, leds ?? surface.Leds);
		}
		public void Wave(Direction direction, IEnumerable<Led> leds = null, int times = 3, int rate = 1000, int size = 1)
		{
			PlayWaveAnimation(direction, leds ?? surface.Leds, times, rate, size);
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
		public async Task FadeKey(Led led, Color start, Color end, int fadeTimeMs = 50)
		{
			double fadetime = fadeTimeMs;
			while (fadeTimeMs >= 0)
			{
				var lerpedColorR = Lerp(start.R, end.R, fadetime - (fadetime - fadeTimeMs) / fadetime);
				var lerpedColorG = Lerp(start.G, end.G, fadetime - (fadetime - fadeTimeMs) / fadetime);
				var lerpedColorB = Lerp(start.B, end.B, fadetime - (fadetime - fadeTimeMs) / fadetime);

				Console.WriteLine(lerpedColorR);
				led.Color = start.AddRGB(lerpedColorR, lerpedColorG, lerpedColorB);
				await Task.Delay(5);
				fadeTimeMs-=5;
				surface.Update();

			}
		}
		double Lerp(double firstFloat, double secondFloat, double by)
		{
			return firstFloat * (1 - by) + secondFloat * by;
		}
		private async void PlayWaveAnimation(Direction direction = Direction.TopDown, IEnumerable<Led> leds = null, int times = 3, int rate = 50, int size = 1)
		{
			CreateLookups(leds);
			for (int waveIndex = 0; waveIndex < times; waveIndex++)
			{
				switch (direction)
				{
					case Direction.TopDown:
						for (int groupIndex = 0; groupIndex < yKeyCollection.Length; groupIndex++)
						{
							var ledsToChange = yOrientedLeds[yKeyCollection[groupIndex]];
							for (int ledIndex = 0; ledIndex < ledsToChange.Length; ledIndex++)
							{
								FadeKey(ledsToChange[ledIndex], new Color(0, 0, 0), new Color(255, 255, 255), 500);
							}
							surface.Update();
							await Task.Delay(rate);
						}
						break;
					case Direction.DownTop:
						break;
					case Direction.LeftRight:
						break;
					case Direction.RightLeft:
						break;
					default:
						break;
				}

			}


		}

		private void CreateLookups(IEnumerable<Led> leds)
		{
			yOrientedLeds = leds.GroupBy(led => (int)led.LedRectangle.Location.Y).ToDictionary(key => key.Key, value => value.ToArray());
			xOrientedLeds = leds.GroupBy(led => (int)led.LedRectangle.Location.X).ToDictionary(key => key.Key, value => value.ToArray());
			yKeyCollection = yOrientedLeds.Keys.OrderBy(key => key).ToArray();
			xKeyCollection = xOrientedLeds.Keys.OrderBy(key => key).ToArray();
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
