using UnityEngine;
using UnityEngine.UI;

public class Language : MonoBehaviour
{
    public static string[,] stringTab = new string[2,256]; //tableau des traductions (ici, 2 langues, 256 mots possibles)
    public GameObject Object1;
    public static string LanguageStr = "fr";

    // Use this for initialization
    private void Start()
    {
        DontDestroyOnLoad(Object1);

        stringTab[0, 0] = "Singleplayer";
        stringTab[1, 0] = "Solo";

        stringTab[0, 1] = "Multiplayer";
        stringTab[1, 1] = "Multijoueur";

        stringTab[0, 2] = "Options";
        stringTab[1, 2] = "Options";

        stringTab[0, 3] = "Credits";
        stringTab[1, 3] = "Credits";

        stringTab[0, 4] = "Quit";
        stringTab[1, 4] = "Quitter";

        stringTab[0, 5] = "Project Annihilation";
        stringTab[1, 5] = "Projet Annihilation";
    }

    public static string get_word(int wordId)
    {
        switch (LanguageStr)
        {
            case "eng":
                return stringTab[0, wordId];
                break;
            case "fr":
                return stringTab[1, wordId];
                break;
            default:
                return "error";
                //return global::DontDestroyOnLoad.stringTab[0, wordId];
                break;
        }
    }

    private void Awake()
    {

    }

}