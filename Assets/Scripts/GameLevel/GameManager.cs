using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    /* [SerializeField] inspector k�sm�nda g�r�nmesini ve hierarchydeki nesneleri s�r�kleyip bu k�sma b�rakma i�lemlerine olanak sa�l�yor. */

    [SerializeField]
    private GameObject karePrefab;

    [SerializeField]
    private Transform karelerPanel;

    [SerializeField]
    private TextMeshProUGUI soruText;


    private GameObject[] karelerDizisi = new GameObject[25];

    [SerializeField]
    private Transform soruPaneli;

    [SerializeField]
    private Sprite[] kareSprites;//Karelerin i�erisinde bulunan resimleri dizi �eklinde olu�turdum.

    [SerializeField]
    private GameObject sonucPaneli;

    [SerializeField] //inspector k�sm�nda g�r�nmesini ve hierarchydeki nesneleri s�r�kleyip bu k�sma b�rakma i�lemlerine olanak sa�l�yor.
    AudioSource audioSource;



    List<int> bolumDegerleriListesi= new List<int>();

    int bolunenSayi,bolenSayi;
    int kacinciSoru;
    int dogruSonuc;
    int butonDegeri;
    bool butonaBasilsinmi; //de�er belirtmedi�in i�in false �eklindedir
    int kalanHak; //HalanHakManager Script dosyas�ndan Kalan haklara ula�aca��z
    string sorununZorlukDerecesi;

    KalanHakManager kalanHakManager;//bir scriptten ba�ka bir scripte ula�ma 
    PuanManager puanManager;
    GameObject gecerliKare;

    public AudioClip butonSesi;//Hangi ses dosyas�n� oynataca��m�z k�s�m 
    private void Awake()
    {
        kalanHak= 3;

        audioSource = GetComponent<AudioSource>();//GameManager'�n ba�l� oldu�u nesne i�erindeki Audio Source ula�mak.

        sonucPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;  //Sonuc paneli ba�lang��ta g�r�nmez

        kalanHakManager = Object.FindObjectOfType<KalanHakManager>();
        puanManager=Object.FindObjectOfType<PuanManager>();//Object.FindObjectOfType<PuanManager>()==PuanManager ismine sahip Script dosyas�n� bul 
        kalanHakManager.KalanHaklar�KontrolEt(kalanHak); //3 tane hak aktif

    }
    void Start()
    {
        butonaBasilsinmi= false;
        soruPaneli.GetComponent<RectTransform>().localScale = Vector3.zero; //Soru panelinin ebatlar�n� 0 yapt�k.
        kareleriolustur();
    }

    public void kareleriolustur() //Kareleri �o�altmak 
    {
        for (int i= 0; i< 25; i++)
        {
            GameObject kare = Instantiate(karePrefab, karelerPanel); //Instantiate= �rneklendir, �o�alt  Instantiate(�o�altaca��m�z nesne )

            kare.transform.GetChild(1).GetComponent<Image>().sprite = kareSprites[Random.Range(0,kareSprites.Length)];//Image componentini bularak resmi de�i�tiriyor
            //Karelerde bulunan Button �zelli�indeki OnClick eklemek
            kare.transform.GetComponent<Button>().onClick.AddListener(() => ButonaBasildi());


            karelerDizisi[i] = kare;
        }
        BolumDegerleriniTexteYazdir();
        StartCoroutine(DoFadeRoutine());
        Invoke("SoruPaneliniAc",2f);
    }

    void ButonaBasildi()//Bas�lan karenin de�erini okur.
    { 
        if(butonaBasilsinmi)
        {

            audioSource.PlayOneShot(butonSesi);//Unity'nin �zel Fonksiyonu PlayOneShot = 1 kez �al��t�r

            //buton de�eri int de�er di�eri ise String de�er onun i�in "Parse" ile int'a d�n��t�rme i�lemi yap�yoruz.
            butonDegeri = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            
            //Debug.Log(butonDegeri);

            gecerliKare = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject; //ge�erlikare �uan t�klanan kare = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject
            SonucuKontrolEt();
        }

        
    }

    void SonucuKontrolEt()
    {
        if (butonDegeri == dogruSonuc)
        {
            gecerliKare.transform.GetChild(1).GetComponent<Image>().enabled = true; //do�ru ise enabled(aktifle�tirme)= a��k(true) hale gelecek ve resimin g�z�kmesini sa�layacak
            gecerliKare.transform.GetChild(0).GetComponent< TextMeshProUGUI >().text=""; //resmin alt�ndaki yaz�y� siler
            gecerliKare.transform.GetComponent<Button>().interactable = false;//tekrar o kareye bas�lmas�n� engelledik.   interactable= etkile�imli


            puanManager.PuaniArtir(sorununZorlukDerecesi);
            bolumDegerleriListesi.RemoveAt(kacinciSoru); //Ayn� soru tekrarlanmas�n diye bolumDegerleriListesi'ndeki kacinciSoru(sorulan soru yani) listeden ��kart�yoruz.
           
           
            if(bolumDegerleriListesi.Count>0)
            {
                SoruPaneliniAc();

            }
            else
            {
                OyunBitti();
            }
          
        }
        else
        {
            kalanHak--;
            kalanHakManager.KalanHaklar�KontrolEt(kalanHak);
        }
        if(kalanHak==0)
        {
            OyunBitti();
        }
    }

    void OyunBitti()
    {
        butonaBasilsinmi = false; //butona bas�lmas�n� inaktif yapt�k.
        sonucPaneli.GetComponent<RectTransform>().DOScale(1, 0.5f);
      
    }


    IEnumerator DoFadeRoutine()
    {
        foreach(var kare in karelerDizisi)
        {
            kare.GetComponent<CanvasGroup>().DOFade(1,0.2f);
            yield return new WaitForSeconds(0.07f);

        }
    }

    void BolumDegerleriniTexteYazdir()//ekrandaki k�rm�z� kutucuklar�n fonksiyonu 
    {


        foreach (var kare in karelerDizisi)
        {
            int rastgeleDeger = Random.Range(1, 13);
            bolumDegerleriListesi.Add(rastgeleDeger);//25 tane de�erin herbirini bolumDegerleriListesi i�erisine atma i�lemi

            /*kare.transform.GetChild(0).GetComponent<Text>().text = rastgeleDeger.ToString(); UI -TEXT- SE�ERSEN E�ER BU KODU KULLAN ANCAK 
            TEXT-TEXTMESHPRO SE�ENE�� SE��L�YSE A�A�IDAK� KODU KULLAN *bu k�s�mda hata ald�n ��nk�*    */
            kare.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = rastgeleDeger.ToString();

         
            //olu�turdu�umuz kare Prefab nesnesindeki button i�erindeki text GetChild(0) 0. seviyede olan nesne
        }
       // Debug.Log(bolumDegerleriListesi[0]);
    }
   void SoruPaneliniAc()
    {
        SoruySor();
        butonaBasilsinmi = true; //t�m kartlar geldi�inde butona basma aktif oluyor.
        soruPaneli.GetComponent<RectTransform>().DOScale(1,0.5f);
        
    }

    void SoruySor()
    {
        bolenSayi = Random.Range(2, 11);//2-10'a kadar olan de�er 
        kacinciSoru=Random.Range(0, bolumDegerleriListesi.Count);
        dogruSonuc = bolumDegerleriListesi[kacinciSoru];
        bolunenSayi = bolenSayi * dogruSonuc;


        if (bolunenSayi <= 40)
        {
            sorununZorlukDerecesi = "kolay";
        }
        else if (bolunenSayi > 40 && bolunenSayi <= 80)
        {
            sorununZorlukDerecesi = "orta";
        }
        else
            sorununZorlukDerecesi = "zor";

        soruText.text=bolunenSayi.ToString()+ ":" +bolenSayi.ToString();
    }
}
