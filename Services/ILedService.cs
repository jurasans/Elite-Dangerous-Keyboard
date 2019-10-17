using System;
using System.Collections.Generic;
using RGB.NET.Core;

namespace EliteKeyboard
{
	public interface ILedService : IDisposable
	{
		void InitializeDevices();
		void LoadDevices(IRGBDeviceProvider provider);
		void Blink(IEnumerable<Led> leds = null, int times = 5, int delayMs = 200);
		void Wave(Direction direction, IEnumerable<Led> leds = null, int times = 3, int rate = 50, int size = 1);
	}
}