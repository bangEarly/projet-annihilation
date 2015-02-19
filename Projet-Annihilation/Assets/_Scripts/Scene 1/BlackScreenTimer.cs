using UnityEngine;
using UnityEngine.UI;

public class BlackScreenTimer : MonoBehaviour
{
    public static float Timer;
    public bool Hiding;
    public Text T42;
    // Use this for initialization
    private void Start()
    {
        Hiding = true;
    }

    private void Awake()
    {
        Timer = 0.0f;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Timer < 5.0f)
            Timer += Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (Timer < 5.0f)
            T42.color = Hiding
                ? new Color(0f, 0f, 0f, (T42.color.a - (Timer/10))*1.0f)
                : new Color(0f, 0f, 0f, (T42.color.a + (Timer/10))*1.0f);
    }

    public void Reverse()
    {
        Hiding = !Hiding;
        Timer = 0.0f;
    }
}