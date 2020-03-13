
//#define DEBUG_RENDER

using UnityEngine;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;

[ExecuteInEditMode]
public class UBRefractionsPE : UBEffectBase
{
    public float ReflDistort = 0.05f;
    public Texture2D BumpMap;
    public float TileX = 1f;
    public float TileY = 1f;
    public Shader Shader;
    private Material _material;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_material)
        {
            DestroyImmediate(_material);
            _material = null;
        }
        if (Shader)
        {
            _material = new Material(Shader);
            _material.hideFlags = HideFlags.HideAndDontSave;

            if (_material.HasProperty("_ReflDistort"))
            {
                _material.SetFloat("_ReflDistort", ReflDistort);
            }
            if (_material.HasProperty("_BumpMap"))
            {
                _material.SetTexture("_BumpMap", BumpMap);
            }
            if (_material.HasProperty("_TileX"))
            {
                _material.SetFloat("_TileX", TileX);
            }
            if (_material.HasProperty("_TileY"))
            {
                _material.SetFloat("_TileY", TileY);
            }
        }

        if (Shader != null && _material != null)
        {
            Graphics.Blit(source, destination, _material);
        }
    }
}
