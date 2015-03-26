using UnityEngine;
using UnityEngine.UI;

public class Opacity : MonoBehaviour
{
    public RawImage Logo;
    public GameObject BlackScreen;
    private static float _logoOpacity;
    private static float _blackScreenOpacity;
    // Use this for initialization
    private void Start()
    {
    }

    private void Awake()
    {
        _logoOpacity = Logo.color.a;
        _blackScreenOpacity = BlackScreen.GetComponent<GUITexture>().color.a;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        Logo.color = new Color(_logoOpacity, Logo.color.r, Logo.color.g, Logo.color.b);
        BlackScreen.GetComponent<GUIText>().color = new Color(_blackScreenOpacity, BlackScreen.GetComponent<GUIText>().color.r,
            BlackScreen.GetComponent<GUIText>().color.g, BlackScreen.GetComponent<GUIText>().color.b);
    }
}