using Coimbra.Services.Events;

namespace Event
{
	public sealed partial class OnMusicVolumeUpdate : IEvent
	{
		public float Volume;
	}
}