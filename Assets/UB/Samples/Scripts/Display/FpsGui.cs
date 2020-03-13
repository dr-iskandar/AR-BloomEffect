using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class FpsGui : Gui
{
    public Text Text; //to change ui

    int frameCount = 0;
    float dt = 0.0f;
    float fps = 0.0f;
    float updateRate = 4.0f;  // 4 updates per sec.

    string groupName = Guid.NewGuid().ToString(); //just to group thread actions

    void Start()
    {
        Execute(() =>
        {
            CallGui();
        }, groupName, 500, false);
    }

    void CallGui()
    {
        Text.text = string.Format("{0:N1} fps", fps);

        Execute(() =>
        {
            CallGui();
        }, groupName, 500, false);
    }


    protected override void Update()
    {
        base.Update();

        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0f/updateRate)
        {
            fps = frameCount / dt ;
            //Debug.Log("fps:" + fps);
            frameCount = 0;
            dt -= 1.0f/updateRate;
        }
    }
}