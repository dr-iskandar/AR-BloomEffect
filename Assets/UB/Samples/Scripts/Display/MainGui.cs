using UnityEngine;
using System.Collections;
//using UnityStandardAssets.CinematicEffects;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class MainGui : Gui {

    //public Camera Camera;

    public Light Light;


    public GameObject NormalFloor;
    public GameObject MirrorFloor;
    public GameObject MirrorBlurFloor;
    public GameObject MirrorBlurTexFloor;
    public GameObject MirrorRefFloor;
    public GameObject MirrorRefTexFloor;
    public GameObject MirrorTexFloorSurf;
    public GameObject MirrorBlurTexFloorSurf;
    public GameObject MirrorRefTexFloorSurf;
    public GameObject MirrorAlphaTexFloorSurf;
    public GameObject RefractionGrabPass;
    public GameObject Refraction;
    public GameObject Blur;
    public GameObject BlurGaus;
    public GameObject Mosaic;
    public GameObject CRT;
    public GameObject Portal;
    public GameObject Water;
    public GameObject Ghost;
    public GameObject Car;
    public GameObject Heli;

    public void Start()
    {
        Application.targetFrameRate = 300;
        ActivateDefault();
    }

    public void Off()
    {
        NormalFloor.SetActive(false);
        MirrorFloor.SetActive(false);
        MirrorBlurFloor.SetActive(false);
        MirrorBlurTexFloor.SetActive(false);
        MirrorRefFloor.SetActive(false);
        MirrorRefTexFloor.SetActive(false);
        MirrorTexFloorSurf.SetActive(false);
        MirrorBlurTexFloorSurf.SetActive(false);
        MirrorRefTexFloorSurf.SetActive(false);
        MirrorAlphaTexFloorSurf.SetActive(false);
        Refraction.SetActive(false);
        RefractionGrabPass.SetActive(false);
        Blur.SetActive(false);
        BlurGaus.SetActive(false);
        Mosaic.SetActive(false);
        CRT.SetActive(false);
        Portal.SetActive(false);
        Water.SetActive(false);
        Ghost.SetActive(false);
        Car.SetActive(true);
        Heli.SetActive(false);
    }

    public void ActivateShadows()
    {
        if (Light.shadows == LightShadows.None)
            Light.shadows = LightShadows.Hard;
        else
            Light.shadows = LightShadows.None;
    }

    public void ActivateDefault()
    {
        Off();
        NormalFloor.SetActive(true);
    }

    public void ActivateMirror()
    {
        if (MirrorFloor.activeSelf)
        {
            Off();
            MirrorBlurFloor.SetActive(true);
        } 
        else if(MirrorBlurFloor.activeSelf)
        {
            Off();
            MirrorBlurTexFloor.SetActive(true);
        }
        else if(MirrorBlurTexFloor.activeSelf)
        {
            Off();
            MirrorRefFloor.SetActive(true);
        }
        else if(MirrorRefFloor.activeSelf)
        {
            Off();
            MirrorRefTexFloor.SetActive(true);
        }
        else if(MirrorRefTexFloor.activeSelf)
        {
            Off();
            MirrorTexFloorSurf.SetActive(true);
        }
        else if(MirrorTexFloorSurf.activeSelf)
        {
            Off();
            MirrorBlurTexFloorSurf.SetActive(true);
        }
        else if(MirrorBlurTexFloorSurf.activeSelf)
        {
            Off();
            MirrorRefTexFloorSurf.SetActive(true);
        }
        else if(MirrorRefTexFloorSurf.activeSelf)
        {
            Off();
            MirrorAlphaTexFloorSurf.SetActive(true);
        }
        else
        {
            Off();
            MirrorFloor.SetActive(true);
        }

        NormalFloor.SetActive(false);
    }

    public void ActivateRefractionGrabPass()
    {
        ActivateDefault();
        RefractionGrabPass.SetActive(true);
    }

    public void ActivateRefraction()
    {
        ActivateDefault();
        Refraction.SetActive(true);
    }

    public void ActivateBlur()
    {
        ActivateDefault();
        Blur.SetActive(true);
    }

    public void ActivateBlurGaus()
    {
        ActivateDefault();
        BlurGaus.SetActive(true);
    }

    public void ActivateDistortion()
    {
        ActivateDefault();
        Car.SetActive(false);
        Heli.SetActive(true);
    }

    public void ActivateMosaic()
    {
        ActivateDefault();
        Mosaic.SetActive(true);
    }

    public void ActivateCRT()
    {
        ActivateDefault();
        CRT.SetActive(true);
    }

    public void ActivatePortal()
    {
        ActivateDefault();
        Portal.SetActive(true);
    }

    public void ActivateWater()
    {
        Off();
        Water.SetActive(true);
    }

    public void ActivateGhost()
    {
        Off();
        Ghost.SetActive(true);
        NormalFloor.SetActive(true);
    }

    public void LoadShaderScene(){
        SceneManager.LoadScene("Shaders");
    }

    public void LoadEffectsScene(){
        SceneManager.LoadScene("PostEffects");
    }
}

