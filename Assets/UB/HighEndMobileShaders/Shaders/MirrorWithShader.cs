
//#define DEBUG_RENDER

using UnityEngine;
using System.Collections;
using System.Linq;

[ExecuteInEditMode]
public class MirrorWithShader : UBEffectBase
{
    public bool IsDirect = false;
    public bool UseCameraClipPlane = false;
    public Vector3 Normal = Vector3.up;
    [Range(1, 100)]
    public int Iterations = 3;
    public float CustomFloatParam2 = 3;
    public Shader Shader;
    private Material _material;
    public bool DisablePixelLights = true;
    public int TextureSize = 256;
    public float ClipPlaneOffset = 0f;
    public Camera Camera;

    public LayerMask ReflectLayers = -1;


    //private Camera CamReference;
    //private Camera ReverseCamera;
    //private GameObject ReverseCameraHolder;

    private Hashtable _reflectionCameras = new Hashtable(); // Camera -> Camera table

    //private GameObject m_copyCamObject = null;
    //private GameObject m_copyCam = null;
    private RenderTexture _reflectionTexture1;
    private RenderTexture _reflectionTexture2;
    private RenderTexture _reflectionTexture3;

    private int _oldReflectionTextureSize;


    //private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    //{
    //    Vector2 diference = vec2 - vec1;
    //    float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
    //    return Vector2.Angle(Vector2.right, diference) * sign;
    //}

    public static Quaternion QuaternionFromMatrix(Matrix4x4 m) { return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1)); }
    public static Vector4 PosToV4(Vector3 v) { return new Vector4(v.x, v.y, v.z, 1.0f); }
    public static Vector3 ToV3(Vector4 v) { return new Vector3(v.x, v.y, v.z); }

    public static Vector3 ZeroV3 = new Vector3(0.0f, 0.0f, 0.0f);
    public static Vector3 OneV3 = new Vector3(1.0f, 1.0f, 1.0f);

    public void OnWillRenderObject()
    {
        var rend = GetComponent<Renderer>();
        RenderMe(rend.sharedMaterials.Where(a => a != null).ToArray());
    }

    void RenderMe(Material[] materials)
    {
        // Safeguard from recursive draws       
        if (InsideRendering)
            return;
        InsideRendering = true;

        //use current camera if not assigned
        if (Camera == null)
        {
            Camera = Camera.current;
        }

        //if render is for same camera, is direct(must), pixelLights, same resolution,same mask, same offset; 
        //do not render it again for same frame
        var renderToTexture = true;
        var uniqueName = Camera.GetInstanceID() + "_" + IsDirect + "_" + DisablePixelLights + "_" +
                         TextureSize + "_" + ClipPlaneOffset + "_" + ReflectLayers.value;
        
        if (_material)
        {
            DestroyImmediate(_material);
            _material = null;
        }
        if (Shader)
        {
            _material = new Material(Shader);
            _material.hideFlags = HideFlags.HideAndDontSave;

            if (_material.HasProperty("_CustomFloatParam2"))
            {
                _material.SetFloat("_CustomFloatParam2", CustomFloatParam2);
            }
        }

        // Optionally disable pixel lights for reflection
        int oldPixelLightCount = QualitySettings.pixelLightCount;
        if (DisablePixelLights)
            QualitySettings.pixelLightCount = 0;

        //var mainCamOldPosition = m_Camera.transform.position;
        //var mainCamOldRotation = m_Camera.transform.rotation;
        //Camera cam = m_Camera;

        ////setup
        Camera reflectionCamera;
        CreateMirrorObjects(Camera, out reflectionCamera);

        UpdateCameraModes(Camera, reflectionCamera);
        
        reflectionCamera.cullingMask = ~(1 << 4) & ReflectLayers.value; // never render water layer
        reflectionCamera.targetTexture = _reflectionTexture1;

        if (IsDirect)
        {
            try
            {
                AlreadyRendered.Remove(uniqueName + "_" + (Time.frameCount - 1));
#if DEBUG_RENDER
                Debug.Log("Removed previous entry " + uniqueName + "_" + (Time.frameCount - 1));
#endif
            }
            finally
            {
            }
            try
            {
                RenderTexture myOut;
                if (AlreadyRendered.TryGetValue(uniqueName + "_" + Time.frameCount, out myOut))
                {
#if DEBUG_RENDER
                    Debug.Log("Render already made for this frame skipping " + uniqueName + "_" + Time.frameCount);
#endif
                    _reflectionTexture1 = myOut;
                    renderToTexture = false;
                }
            }
            finally
            {
            }
        }

        if (UseCameraClipPlane && IsDirect)
        {
            Vector4 clipPlaneWorldSpace = new Vector4(Normal.x, Normal.y, Normal.z, 
                Vector3.Dot(transform.position, Normal)-ClipPlaneOffset);
            Vector4 clipPlaneCameraSpace = Matrix4x4.Transpose(Matrix4x4.Inverse(Camera.worldToCameraMatrix)) * clipPlaneWorldSpace;

            // Update projection based on new clip plane
            // Note: http://aras-p.info/texts/obliqueortho.html and http://www.terathon.com/lengyel/Lengyel-Oblique.pdf
            reflectionCamera.projectionMatrix = Camera.CalculateObliqueMatrix(clipPlaneCameraSpace);

        }

        if (!IsDirect)
        {
            // find out the reflection plane: position and normal in world space
            Vector3 pos = transform.position;
            Vector3 normal = Normal;

            float d = -Vector3.Dot(normal, pos) - ClipPlaneOffset;
            Vector4 reflectionPlane = new Vector4(normal.x, normal.y, normal.z, d);

            // Render reflection
            // Reflect camera around reflection plane
            Matrix4x4 reflection = Matrix4x4.zero;
            CalculateReflectionMatrix(ref reflection, reflectionPlane);
            Vector3 oldpos = Camera.transform.position;
            Vector3 newpos = reflection.MultiplyPoint(oldpos);
            reflectionCamera.worldToCameraMatrix = Camera.worldToCameraMatrix * reflection;

            Vector4 clipPlane = CameraSpacePlane(reflectionCamera, pos, normal, 1.0f);
            //Matrix4x4 projection = cam.projectionMatrix;
            Matrix4x4 projection = Camera.CalculateObliqueMatrix(clipPlane);
            reflectionCamera.projectionMatrix = projection;

            GL.invertCulling = true;
            reflectionCamera.transform.position = newpos;
            Vector3 euler = Camera.transform.eulerAngles;
            reflectionCamera.transform.eulerAngles = new Vector3(0, euler.y, euler.z);
#if DEBUG_RENDER
            Debug.Log("Rendering for mirror... frame: " + Time.frameCount);
#endif
            reflectionCamera.Render();
            reflectionCamera.transform.position = oldpos;
            GL.invertCulling = false;
        }
        else
        {
            if (renderToTexture)
            {
#if DEBUG_RENDER
                Debug.Log("Rendering for direct... " + uniqueName + "_" + Time.frameCount);
#endif
                reflectionCamera.transform.position = Camera.transform.position;
                reflectionCamera.transform.rotation = Camera.transform.rotation;

                reflectionCamera.Render();
            }
        }

        //hold texture1 unchanged!!
        Graphics.Blit(_reflectionTexture1, _reflectionTexture2);

        if (Shader != null && _material != null)
        {
            for (int i = 1; i <= Iterations; i++)
            {
                if (_material.HasProperty("_CustomFloatParam1"))
                    _material.SetFloat("_CustomFloatParam1", i);
                if (i % 2 == 1) //a little hack to copy textures in order from 1 to 2 than 2 to 1 and so :)
                {
                    Graphics.Blit(_reflectionTexture2, _reflectionTexture3, _material);
                }
                else
                {
                    Graphics.Blit(_reflectionTexture3, _reflectionTexture2, _material);
                }
            }
        }

        foreach (Material mat in materials)
        {
            if (mat.HasProperty("_ReflectionTex"))
            {
                if (Shader)
                {
                    //Debug.Log("setting ref text for " + GetInstanceID());
                    if (Iterations % 2 == 1) //again a hack:)
                    {
                        mat.SetTexture("_ReflectionTex", _reflectionTexture3);
                    }
                    else
                    {
                        mat.SetTexture("_ReflectionTex", _reflectionTexture2);
                    }
                }
                else
                {
                    mat.SetTexture("_ReflectionTex", _reflectionTexture1);
                }
            }
            if (mat.HasProperty("_IsReverse"))
            {
                mat.SetFloat("_IsReverse", IsDirect ? 1 : 0);
            }
        }

        // Restore pixel light count
        if (DisablePixelLights)
            QualitySettings.pixelLightCount = oldPixelLightCount;

        if (IsDirect)
        {
            try
            {
                RenderTexture myOut;
                if (!AlreadyRendered.TryGetValue(uniqueName + "_" + Time.frameCount, out myOut))
                {
                    AlreadyRendered.Add(uniqueName + "_" + Time.frameCount, _reflectionTexture1);
#if DEBUG_RENDER
                    Debug.Log("Added to renderlist: " + uniqueName + "_" + Time.frameCount);
#endif
                }
                myOut = null;
            }
            finally
            {
            }
        }

        InsideRendering = false;
    }


    // Cleanup all the objects we possibly have created
    void OnDisable()
    {
        if (_reflectionTexture1)
        {
            DestroyImmediate(_reflectionTexture1);
            _reflectionTexture1 = null;
        }
        if (_reflectionTexture2)
        {
            DestroyImmediate(_reflectionTexture2);
            _reflectionTexture2 = null;
        }
        if (_reflectionTexture3)
        {
            DestroyImmediate(_reflectionTexture3);
            _reflectionTexture3 = null;
        }
        foreach (DictionaryEntry kvp in _reflectionCameras)
            DestroyImmediate(((Camera)kvp.Value).gameObject);
        _reflectionCameras.Clear();
    }


    private void UpdateCameraModes(Camera src, Camera dest)
    {
        if (dest == null)
            return;
        // set camera to clear the same way as current camera
        dest.clearFlags = src.clearFlags;//CameraClearFlags.SolidColor; // src.clearFlags;
        dest.backgroundColor = src.backgroundColor;
        if (src.clearFlags == CameraClearFlags.Skybox)
        {
            Skybox sky = src.GetComponent(typeof(Skybox)) as Skybox;
            Skybox mysky = dest.GetComponent(typeof(Skybox)) as Skybox;
            if (!sky || !sky.material)
            {
                mysky.enabled = false;
            }
            else
            {
                mysky.enabled = true;
                mysky.material = sky.material;
            }
        }
        // update other values to match current camera.
        // even if we are supplying custom camera&projection matrices,
        // some of values are used elsewhere (e.g. skybox uses far plane)
        dest.farClipPlane = src.farClipPlane;
        dest.nearClipPlane = src.nearClipPlane;
        dest.orthographic = src.orthographic;
        dest.fieldOfView = src.fieldOfView;
        dest.aspect = src.aspect;
        dest.orthographicSize = src.orthographicSize;
        dest.renderingPath = src.renderingPath;
    }

    // On-demand create any objects we need
    private void CreateMirrorObjects(Camera currentCamera, out Camera reflectionCamera)
    {
        //reflectionCamera = null;

        // Reflection render texture
        if (!_reflectionTexture1 || !_reflectionTexture2 || !_reflectionTexture3 ||
            _oldReflectionTextureSize != TextureSize)
        {
            if (_reflectionTexture1)
                DestroyImmediate(_reflectionTexture1);
            _reflectionTexture1 = new RenderTexture(TextureSize, TextureSize, 24);
            _reflectionTexture1.name = "__MirrorReflection1" + GetInstanceID();
            _reflectionTexture1.isPowerOfTwo = true;
            _reflectionTexture1.hideFlags = HideFlags.DontSave;

            if (_reflectionTexture2)
                DestroyImmediate(_reflectionTexture2);
            _reflectionTexture2 = new RenderTexture(TextureSize, TextureSize, 24);
            _reflectionTexture2.name = "__MirrorReflection2" + GetInstanceID();
            _reflectionTexture2.isPowerOfTwo = true;
            _reflectionTexture2.hideFlags = HideFlags.DontSave;

            if (_reflectionTexture3)
                DestroyImmediate(_reflectionTexture3);
            _reflectionTexture3 = new RenderTexture(TextureSize, TextureSize, 24);
            _reflectionTexture3.name = "__MirrorReflection3" + GetInstanceID();
            _reflectionTexture3.isPowerOfTwo = true;
            _reflectionTexture3.hideFlags = HideFlags.DontSave;

            _oldReflectionTextureSize = TextureSize;
        }

        // Camera for reflection
        reflectionCamera = _reflectionCameras[currentCamera] as Camera;
        if (!reflectionCamera) // catch both not-in-dictionary and in-dictionary-but-deleted-GO
        {
            GameObject go = new GameObject("Mirror Refl Camera id" + GetInstanceID() + " for " + currentCamera.GetInstanceID(), typeof(Camera), typeof(Skybox));
            reflectionCamera = go.GetComponent<Camera>();
            reflectionCamera.enabled = false;
            reflectionCamera.transform.position = transform.position;
            reflectionCamera.transform.rotation = transform.rotation;
            reflectionCamera.gameObject.AddComponent<FlareLayer>();
            go.hideFlags = HideFlags.HideAndDontSave;
            _reflectionCameras[currentCamera] = reflectionCamera;
        }
    }


    // Given position/normal of the plane, calculates plane in camera space.
    private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
    {
        Vector3 offsetPos = pos + normal * ClipPlaneOffset;
        Matrix4x4 m = cam.worldToCameraMatrix;
        Vector3 cpos = m.MultiplyPoint(offsetPos);
        Vector3 cnormal = m.MultiplyVector(normal).normalized * sideSign;
        return new Vector4(cnormal.x, cnormal.y, cnormal.z, -Vector3.Dot(cpos, cnormal));
    }

    // Calculates reflection matrix around the given plane
    private void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
    {
        reflectionMat.m00 = (1F - 2F * plane[0] * plane[0]);
        reflectionMat.m01 = (-2F * plane[0] * plane[1]);
        reflectionMat.m02 = (-2F * plane[0] * plane[2]);
        reflectionMat.m03 = (-2F * plane[3] * plane[0]);

        reflectionMat.m10 = (-2F * plane[1] * plane[0]);
        reflectionMat.m11 = (1F - 2F * plane[1] * plane[1]);
        reflectionMat.m12 = (-2F * plane[1] * plane[2]);
        reflectionMat.m13 = (-2F * plane[3] * plane[1]);

        reflectionMat.m20 = (-2F * plane[2] * plane[0]);
        reflectionMat.m21 = (-2F * plane[2] * plane[1]);
        reflectionMat.m22 = (1F - 2F * plane[2] * plane[2]);
        reflectionMat.m23 = (-2F * plane[3] * plane[2]);

        reflectionMat.m30 = 0F;
        reflectionMat.m31 = 0F;
        reflectionMat.m32 = 0F;
        reflectionMat.m33 = 1F;
    }
}
