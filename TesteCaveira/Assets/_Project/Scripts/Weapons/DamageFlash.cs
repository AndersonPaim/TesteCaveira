using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Weapons
{
    public class DamageFlash : MonoBehaviour
    {
        [SerializeField] private List<Renderer> _meshRenderers;
        [SerializeField] private Material _flashMaterial;
        [SerializeField] private int _flashTime;
    
        private List<Material> _defaultMaterials = new List<Material>();

        public async UniTask Flash()
        {
            SetDefaultMaterials();
            await FlashDelay();
        }

        private void SetDefaultMaterials()
        {
            _defaultMaterials.Clear();

            foreach (Renderer t in _meshRenderers)
            {
                _defaultMaterials.Add(t.GetComponent<SkinnedMeshRenderer>().materials[0]);
            }
        }

        private async UniTask FlashDelay()
        {
            foreach(Renderer mesh in _meshRenderers)
            {
                mesh.material = _flashMaterial;
            }

            await UniTask.Delay(_flashTime);

            for(int i = 0; i < _meshRenderers.Count; i++)
            {
                _meshRenderers[i].material = _defaultMaterials[i];
            }
        }
    }
}