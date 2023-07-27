 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Sahne yüklemesi için gereken kütüphane

public class SonucManager : MonoBehaviour
{
  

    /* Bunlarý Butona Aktarmam lazým. Bunun için de "yenidenBaslaBtn" ve "anaMenuyeDon" Butonlarýna ONCLICK özelliði ekleyerek sonucPanel'ini aKtarýyorum 
    buradan ilgili script içerisne yazdýðýn(yani bu script dosyasý) seçiyorsun ve ilgili fonksiyonu aktararak click özelliðini aktifleþtirmiþ olunuyor.*/
    public void OyunaYenidenBasla()
    {
        SceneManager.LoadScene("gameLevel");// ! Unity'deki Sahne Ekranýnýn ismi ! 
    }

    public void AnaMenuyeDon()
    {
        SceneManager.LoadScene("menuLevel");
    }

}
