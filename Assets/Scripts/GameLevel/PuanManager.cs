using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PuanManager : MonoBehaviour
{
    private int toplamPuan;
    private int puanArtis;

    [SerializeField]
    private TextMeshProUGUI puanText;

    void Start()
    {
        puanText.text=toplamPuan.ToString();//Oyun �al���r �al��maz toplam puan=0 de�eri ekranda g�z�ks�n

    }
    public void PuaniArtir(string zorlukSeviyesi)
    {
        switch(zorlukSeviyesi)
        {
            case "kolay":
                puanArtis= 5;
                break;
            case "orta":
                puanArtis= 10;
                break;
            case "zor":
                puanArtis= 20;
                break;

        }
        toplamPuan += puanArtis;
        puanText.text = toplamPuan.ToString();

    }



}
