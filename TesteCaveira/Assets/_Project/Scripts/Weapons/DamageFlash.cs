using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private List<Renderer> _meshRenderer;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _flashMaterial;

    public async UniTask Flash()
    {
        await FlashDelay();
    }

    private async UniTask FlashDelay()
    {
        foreach(Renderer mesh in _meshRenderer)
        {
            mesh.material = _flashMaterial;
        }

        await UniTask.Delay(125);

        foreach(Renderer mesh in _meshRenderer)
        {
            mesh.material = _defaultMaterial;
        }
    }
}