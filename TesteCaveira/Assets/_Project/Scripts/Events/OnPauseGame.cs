using Coimbra.Services.Events;

namespace _Project.Scripts.Events
{
	public sealed partial class OnPauseGame : IEvent
	{
        public bool IsPaused;
	}
}