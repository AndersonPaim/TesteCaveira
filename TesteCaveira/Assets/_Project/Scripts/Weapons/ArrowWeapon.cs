using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Weapons
{
    public class ArrowWeapon : WeaponBase
    {
        [SerializeField] private int _destroyDelay;
        private Rigidbody _rb;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _rb = GetComponent<Rigidbody>();
        }

        protected async void OnTriggerEnter(Collider other)
        {
            if(CanDoDamage(other.gameObject))
            {
                DoDamage(other.gameObject);
                await DisableObjectASync(0);
            }
            else
            {
                _rb.isKinematic = true;
                await DisableObjectASync(_destroyDelay);
            }
        }

        private async UniTask DisableObjectASync(int time)
        {
            await UniTask.Delay(time * 1000);
            _rb.isKinematic = false;
            gameObject.SetActive(false);
        }
    }
}