 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Sahne y�klemesi i�in gereken k�t�phane

public class SonucManager : MonoBehaviour
{
  

    /* Bunlar� Butona Aktarmam laz�m. Bunun i�in de "yenidenBaslaBtn" ve "anaMenuyeDon" Butonlar�na ONCLICK �zelli�i ekleyerek sonucPanel'ini aKtar�yorum 
    buradan ilgili script i�erisne yazd���n(yani bu script dosyas�) se�iyorsun ve ilgili fonksiyonu aktararak click �zelli�ini aktifle�tirmi� olunuyor.*/
    public void OyunaYenidenBasla()
    {
        SceneManager.LoadScene("gameLevel");// ! Unity'deki Sahne Ekran�n�n ismi ! 
    }

    public void AnaMenuyeDon()
    {
        SceneManager.LoadScene("menuLevel");
    }

}
