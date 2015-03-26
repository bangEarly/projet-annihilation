using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class SlideShow : MonoBehaviour
{
    //public Texture2D[] slides = new Texture2D[9];  //this is removed, no need to set the size its auto detected

    public string Extention = "jpg"; //extension you looking for
    public float FramePerSec = 25f;

    public string MyPath = @"c:\Unity\YourGame\Assets\Texures\DirWhereYourJPGFilesAre";
    private float _changeTime = 0.04f;
    private int _currentSlide;
    private int _scaledHeight;
    private int _scaledWidth;
    private int _screenAspectRatio;
    private int _screenHeight;
    private int _screenWidth;
    // directory where all the *.jpg files are that need to be animated

    private Texture2D[] _slides;
    private int _textureAspectRatio;
    private int _textureHeight;
    private int _textureWidth;
    private float _timeSinceLast = 1.0f;
    private float _xPosition;
    private float _yPosition;

    private void Start()
    {
        Debug.Log("Finding files....");
        GetFiles(); //new added function

        if (_slides != null)
        {
            //calc the time to change from fps
            _changeTime = 1/FramePerSec;
            Debug.Log("FPS change time is: " + _changeTime);

            GetComponent<GUITexture>().texture = _slides[_currentSlide];
            _currentSlide++;
        }
        else
        {
            Debug.Log("Set reading directory and file type please");
        }
    }

    private void Awake() // THE WORKIN'ONE WOOHA !
    {
        _textureHeight = GetComponent<GUITexture>().texture.height;
        _textureWidth = GetComponent<GUITexture>().texture.width;
        _screenHeight = Screen.height;
        _screenWidth = Screen.width;

        _xPosition = (_screenWidth - _textureWidth)/2.0f;
        _yPosition = (_screenHeight - _textureHeight)/2.0f;
        _scaledWidth = Convert.ToInt32(_screenWidth - 2.0f*_xPosition);
        _scaledHeight = Convert.ToInt32(_screenHeight - 2.0f*_yPosition);
        GetComponent<GUITexture>().pixelInset = new Rect(_xPosition, _yPosition, _scaledWidth, _scaledHeight);
    }

    /*
    private void Awake()
    {
        _textureHeight = guiTexture.texture.height;
        _textureWidth = guiTexture.texture.width;
        _screenHeight = Screen.height;
        _screenWidth = Screen.width;

        _screenAspectRatio = (_screenWidth/_screenHeight);
        _textureAspectRatio = (_textureWidth/_textureHeight);

        if (_textureAspectRatio != _screenAspectRatio)
        {
            // The scaled size is based on the height
            _scaledHeight = _screenHeight;
            _scaledWidth = (_screenHeight*_textureAspectRatio);
        }
        else
        {
            // The scaled size is based on the width
            _scaledWidth = _screenWidth;
            _scaledHeight = (_scaledWidth/_textureAspectRatio);
        }
        _xPosition = _screenWidth/2 - (_scaledWidth/2);
        guiTexture.pixelInset = new Rect(_xPosition, _scaledHeight - _scaledHeight, _scaledWidth, _scaledHeight);
    }
    */

    private void Update()
    {
        //guiTexture.pixelInset = new Rect(_xPosition, _scaledHeight - _scaledHeight, _scaledWidth, _scaledHeight);
        if (_slides == null) return;
        if (_timeSinceLast > _changeTime && _currentSlide < _slides.Length)
        {
            GetComponent<GUITexture>().texture = _slides[_currentSlide];
            _timeSinceLast = 0.0f;
            _currentSlide++;
        }
        _timeSinceLast += Time.deltaTime;

        if (_currentSlide != _slides.Length) return;
        _currentSlide = 0;
        Scene1toScene2.Timer = Scene1toScene2.Total;
    }

    internal void GetFiles()
    {
        if (Directory.Exists(MyPath))
        {
            var dir = new DirectoryInfo(MyPath);
            Debug.Log("Looking for files in dir: " + MyPath);

            FileInfo[] info = dir.GetFiles("*." + Extention);

            // Get number of files, and set the length for the texture2d array
            int totalFiles = info.Length;
            _slides = new Texture2D[totalFiles];

            int i = 0;

            //Read all found files
            foreach (string filePath in info.Select(f => f.Directory + "/" + f.Name))
            {
                Debug.Log("[" + i + "] file found: " + filePath);

                byte[] bytes = File.ReadAllBytes(filePath);
                var tex = new Texture2D(1, 1);

                tex.LoadImage(bytes);
                _slides[i] = tex;

                i++;
            }
        }
        else
        {
            Debug.Log("Directory DOES NOT exist! ");
        }
    }
}