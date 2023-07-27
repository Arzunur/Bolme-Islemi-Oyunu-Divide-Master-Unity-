using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement; //Sahneler arasý geçiþ.Sahne yöneticisi 

public class menuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject startBtn, exitBtn;

    void Start()
    {
        FadeOut();
    }

    
  void FadeOut()//solma
    {
        startBtn.GetComponent<CanvasGroup>().DOFade(1,0.8f); //bitis degeri,süre   
        exitBtn.GetComponent<CanvasGroup>().DOFade(1,0.8f).SetDelay(0.6f);
    }

    public void ExitGame() //Uygulamadan Çýkma þlemi
    {
        Application.Quit(); 
    }

    public void StartGameLevel()
    {
        SceneManager.LoadScene("gameLevel");//Hangi ekrana geçeceði ekraný belirtiyoruz.
    }
}
