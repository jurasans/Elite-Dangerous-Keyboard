using System;
using EliteAPI.Events;
using EliteAPI.Status;

namespace EliteKeyboard
{
	public interface IApiService : IDisposable
	{
		void RegisterHardPointsAction(Action<StatusEvent> hardpointEvent);
		void RegisterHeatWarning(Action<HeatWarningInfo> heatWarning);
		void RegisterShieldEvent(Action<StatusEvent> shieldEvent);
		GameStatus Status { get; }
	}
}