using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    /* [SerializeField] inspector kýsmýnda görünmesini ve hierarchydeki nesneleri sürükleyip bu kýsma býrakma iþlemlerine olanak saðlýyor. */

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
    private Sprite[] kareSprites;//Karelerin içerisinde bulunan resimleri dizi þeklinde oluþturdum.

    [SerializeField]
    private GameObject sonucPaneli;

    [SerializeField] //inspector kýsmýnda görünmesini ve hierarchydeki nesneleri sürükleyip bu kýsma býrakma iþlemlerine olanak saðlýyor.
    AudioSource audioSource;



    List<int> bolumDegerleriListesi= new List<int>();

    int bolunenSayi,bolenSayi;
    int kacinciSoru;
    int dogruSonuc;
    int butonDegeri;
    bool butonaBasilsinmi; //deðer belirtmediðin için false þeklindedir
    int kalanHak; //HalanHakManager Script dosyasýndan Kalan haklara ulaþacaðýz
    string sorununZorlukDerecesi;

    KalanHakManager kalanHakManager;//bir scriptten baþka bir scripte ulaþma 
    PuanManager puanManager;
    GameObject gecerliKare;

    public AudioClip butonSesi;//Hangi ses dosyasýný oynatacaðýmýz kýsým 
    private void Awake()
    {
        kalanHak= 3;

        audioSource = GetComponent<AudioSource>();//GameManager'ýn baðlý olduðu nesne içerindeki Audio Source ulaþmak.

        sonucPaneli.GetComponent<RectTransform>().localScale = Vector3.zero;  //Sonuc paneli baþlangýçta görünmez

        kalanHakManager = Object.FindObjectOfType<KalanHakManager>();
        puanManager=Object.FindObjectOfType<PuanManager>();//Object.FindObjectOfType<PuanManager>()==PuanManager ismine sahip Script dosyasýný bul 
        kalanHakManager.KalanHaklarýKontrolEt(kalanHak); //3 tane hak aktif

    }
    void Start()
    {
        butonaBasilsinmi= false;
        soruPaneli.GetComponent<RectTransform>().localScale = Vector3.zero; //Soru panelinin ebatlarýný 0 yaptýk.
        kareleriolustur();
    }

    public void kareleriolustur() //Kareleri Çoðaltmak 
    {
        for (int i= 0; i< 25; i++)
        {
            GameObject kare = Instantiate(karePrefab, karelerPanel); //Instantiate= Örneklendir, Çoðalt  Instantiate(çoðaltacaðýmýz nesne )

            kare.transform.GetChild(1).GetComponent<Image>().sprite = kareSprites[Random.Range(0,kareSprites.Length)];//Image componentini bularak resmi deðiþtiriyor
            //Karelerde bulunan Button Özelliðindeki OnClick eklemek
            kare.transform.GetComponent<Button>().onClick.AddListener(() => ButonaBasildi());


            karelerDizisi[i] = kare;
        }
        BolumDegerleriniTexteYazdir();
        StartCoroutine(DoFadeRoutine());
        Invoke("SoruPaneliniAc",2f);
    }

    void ButonaBasildi()//Basýlan karenin deðerini okur.
    { 
        if(butonaBasilsinmi)
        {

            audioSource.PlayOneShot(butonSesi);//Unity'nin Özel Fonksiyonu PlayOneShot = 1 kez çalýþtýr

            //buton deðeri int deðer diðeri ise String deðer onun için "Parse" ile int'a dönüþtürme iþlemi yapýyoruz.
            butonDegeri = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            
            //Debug.Log(butonDegeri);

            gecerliKare = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject; //geçerlikare þuan týklanan kare = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject
            SonucuKontrolEt();
        }

        
    }

    void SonucuKontrolEt()
    {
        if (butonDegeri == dogruSonuc)
        {
            gecerliKare.transform.GetChild(1).GetComponent<Image>().enabled = true; //doðru ise enabled(aktifleþtirme)= açýk(true) hale gelecek ve resimin gözükmesini saðlayacak
            gecerliKare.transform.GetChild(0).GetComponent< TextMeshProUGUI >().text=""; //resmin altýndaki yazýyý siler
            gecerliKare.transform.GetComponent<Button>().interactable = false;//tekrar o kareye basýlmasýný engelledik.   interactable= etkileþimli


            puanManager.PuaniArtir(sorununZorlukDerecesi);
            bolumDegerleriListesi.RemoveAt(kacinciSoru); //Ayný soru tekrarlanmasýn diye bolumDegerleriListesi'ndeki kacinciSoru(sorulan soru yani) listeden çýkartýyoruz.
           
           
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
            kalanHakManager.KalanHaklarýKontrolEt(kalanHak);
        }
        if(kalanHak==0)
        {
            OyunBitti();
        }
    }

    void OyunBitti()
    {
        butonaBasilsinmi = false; //butona basýlmasýný inaktif yaptýk.
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

    void BolumDegerleriniTexteYazdir()//ekrandaki kýrmýzý kutucuklarýn fonksiyonu 
    {


        foreach (var kare in karelerDizisi)
        {
            int rastgeleDeger = Random.Range(1, 13);
            bolumDegerleriListesi.Add(rastgeleDeger);//25 tane deðerin herbirini bolumDegerleriListesi içerisine atma iþlemi

            /*kare.transform.GetChild(0).GetComponent<Text>().text = rastgeleDeger.ToString(); UI -TEXT- SEÇERSEN EÐER BU KODU KULLAN ANCAK 
            TEXT-TEXTMESHPRO SEÇENEÐÝ SEÇÝLÝYSE AÞAÐIDAKÝ KODU KULLAN *bu kýsýmda hata aldýn çünkü*    */
            kare.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = rastgeleDeger.ToString();

         
            //oluþturduðumuz kare Prefab nesnesindeki button içerindeki text GetChild(0) 0. seviyede olan nesne
        }
       // Debug.Log(bolumDegerleriListesi[0]);
    }
   void SoruPaneliniAc()
    {
        SoruySor();
        butonaBasilsinmi = true; //tüm kartlar geldiðinde butona basma aktif oluyor.
        soruPaneli.GetComponent<RectTransform>().DOScale(1,0.5f);
        
    }

    void SoruySor()
    {
        bolenSayi = Random.Range(2, 11);//2-10'a kadar olan deðer 
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
