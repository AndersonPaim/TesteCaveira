using Coimbra.Services.Events;

namespace _Project.Scripts.Events
{
	public sealed partial class OnMusicVolumeUpdate : IEvent
	{
		public float Volume;
	}
}