using Coimbra.Services.Events;

namespace _Project.Scripts.Events
{
	public sealed partial class OnEffectsVolumeUpdate : IEvent
	{
		public float Volume;
	}
}