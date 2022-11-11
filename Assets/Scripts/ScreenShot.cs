using UnityEngine;
using System.Collections;
using System.IO;
using System;
namespace ApocalipseZ
{


    public class ScreenShot : MonoBehaviour
    {
        private InputManager inputManager;
        private string _caminho;
        void Start()
        {
            inputManager = GameController.Instance.InputManager;
            _caminho = Application.dataPath + "/capturas/";

            if (!Directory.Exists(_caminho))
            {
                Directory.CreateDirectory(_caminho);
            }
        }


        void Update()
        {
            if (inputManager.GetPrint())
            {
                string nomeImagem = _caminho + DateTime.Now.Ticks.ToString() + ".png";
                //O recurso Application.Cap...está obsoleta na versão 2017 da Unity.
                ScreenCapture.CaptureScreenshot(nomeImagem, 6);//Unity < 2017
                //ScreenCapture.CaptureScreenshot(nomeImagem, 2); //Unity >= 2017
            }
        }
    }
}