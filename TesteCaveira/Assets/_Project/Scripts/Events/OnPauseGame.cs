using Coimbra.Services.Events;

namespace Event
{
	public sealed partial class OnPauseGame : IEvent
	{
        public bool IsPaused;
	}
}