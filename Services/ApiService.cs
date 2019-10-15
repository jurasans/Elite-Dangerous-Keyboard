using System;
using EliteAPI;
using EliteAPI.Events;
using EliteAPI.Status;
using Somfic.Logging;

namespace EliteKeyboard
{
	public class ApiService : IApiService
	{
		private readonly IEliteDangerousAPI api;

		public ApiService(IEliteDangerousAPI api, string path)
		{
			this.api = api;
			api.ChangeJournal(new System.IO.DirectoryInfo(path));
			api.Start();
		}
		public GameStatus Status => api.Status;

		public void RegisterHeatWarning(Action<HeatWarningInfo> heatWarning)
		{
			if (heatWarning != null)
			{
				api.Events.HeatWarningEvent += (o, i) => heatWarning(i);
			}

		}
		public void RegisterHardPointsAction(Action<StatusEvent> hardpointEvent)
		{
			if (hardpointEvent != null)
			{
				api.Events.StatusHardpointsEvent += (o, i) => hardpointEvent(i);

			}
		}
		public void RegisterShieldEvent(Action<StatusEvent> shieldEvent)
		{
			if (shieldEvent != null)
			{
				api.Events.StatusShieldsEvent += (o, i) => shieldEvent(i);

			}

		}

		public void Dispose()
		{
			api.Stop();
		}
	}
}
