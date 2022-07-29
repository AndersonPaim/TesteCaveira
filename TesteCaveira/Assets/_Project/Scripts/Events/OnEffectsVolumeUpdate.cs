using Coimbra.Services.Events;

namespace Event
{
	public sealed partial class OnEffectsVolumeUpdate : IEvent
	{
		public float Volume;
	}
}