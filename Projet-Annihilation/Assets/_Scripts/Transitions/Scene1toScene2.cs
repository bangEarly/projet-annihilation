using System;
using UnityEngine;
using UnityEngine.UI;

public class Scene1toScene2 : MonoBehaviour
{
    public static float Timer;
    public static float Total;
    public Text Str;

    // Use this for initialization
    private void Start()
    {
        Timer = 0.0f;
        Total = 15.0f;
    }


    // Update is called once per frame
    private void Update()
    {
        Timer += Time.deltaTime;
        Str.text = Convert.ToString(Total - Timer);
        if (Timer > Total - 0.00001f)
            Application.LoadLevel(1);
    }
}