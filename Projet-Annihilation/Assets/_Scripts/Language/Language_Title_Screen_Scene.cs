using System;
using UnityEngine;
using UnityEngine.UI;

public class Language_Title_Screen_Scene : MonoBehaviour
{
    public Text Object0; //solo
    public Text Object1; //multi
    public Text Object2; //options
    public Text Object3; //credits
    public Text Object4; //quit
    public Text Object5; //titre

    // Use this for initialization
    private void Start()
    {
    }

    void Awake()
    {
        Object0.text = global::Language.get_word(0);
        Object1.text = global::Language.get_word(1);
        Object2.text = global::Language.get_word(2);
        Object3.text = global::Language.get_word(3);
        Object4.text = global::Language.get_word(4);
        Object5.text = global::Language.get_word(5);
    }


    // Update is called once per frame
    private void Update()
    {
    }
}