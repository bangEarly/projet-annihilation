using UnityEngine;

public class MyUnitySingleton : MonoBehaviour
{
    private static MyUnitySingleton instance;

    public static MyUnitySingleton Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // any other methods you need
}