using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private List<Renderer> _meshRenderers;
    [SerializeField] private Material _flashMaterial;
    [SerializeField] private int _flashTime;
    private List<Material> _defaultMaterials = new List<Material>();

    public async UniTask Flash()
    {
        await FlashDelay();
    }

    private void OnEnable()
    {
        SetDefaultMaterials();
    }

    private void SetDefaultMaterials()
    {
        for(int i = 0; i < _meshRenderers.Count; i++)
        {
            _defaultMaterials.Add(_meshRenderers[i].GetComponent<SkinnedMeshRenderer>().materials[0]);
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