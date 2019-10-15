using System;
using System.Collections.Generic;
using RGB.NET.Core;

namespace EliteKeyboard
{
	public interface ILedService : IDisposable
	{
		void InitializeDevices();
		void LoadDevices(IRGBDeviceProvider provider);
		void Blink(IEnumerable<Led> leds = null);
		void Wave(Direction direction, IEnumerable<Led> leds = null);
	}
}