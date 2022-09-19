using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    [Tooltip("Yery�z� olu�turulurken kullan�lan g�rsel do�ru �al��mazsa bu se�ene�i kullan�n. (�rnek olarak 'imageTestGround' g�rselini kullanabilirsiniz.)")]
    [SerializeField] private bool highAccuracy;
    [Header("IMAGES")]
    [Tooltip("Kullan�lacak g�rseli ekleyiniz.")]
    [SerializeField] private Texture2D groundImage;
    [Tooltip("Kullan�lacak g�rseli ekleyiniz.")]
    [SerializeField] private Texture2D objectsImage;

    [Header("OBJECTS")]
    [Tooltip("Yery�z�n� olu�turacak nesne.")]
    [SerializeField] private GameObject groundObject;
    [Tooltip("Renge g�re olu�turulacak nesneleri ekleyiniz.   (Nesne s�ras� groundColors[] dizisindeki rengiyle ayn� s�rada olmal�.)")]
    [SerializeField] private GameObject[] placeableObjects;
    [Header("COLORS")]
    [Tooltip("G�rsel �zerinde kulland���n�z renkleri giriniz.")]
    [SerializeField] private Color[] groundColors;
    [Tooltip("G�rsel �zerinde kulland���n�z renkleri giriniz.    (Renk s�ras� placeableObjects[] dizisindeki nesnesiyle ayn� s�rada olmal�.)")]
    [SerializeField] private Color[] objectsColors;


    private void Start()
    {
        /* SECILEN GORSELIN PIXELLERININ RGBA KODLARINI ALIR */
        Color[] pixGround = groundImage.GetPixels();
        Color[] pixObjects = objectsImage.GetPixels();

        /* GORSELIN BOYUTLARINI TUTAN INT DEGERLERI */
        int worldX = groundImage.width;
        int worldZ = groundImage.height;

        /* GORSELIN PIKSELLERI UZERINDEN NESNELERIN OLUSTURULACAGI KONUMLARI TUTAR */
        Vector3[] spawnPositions = new Vector3[pixGround.Length];
        /* ORIJIN ORTA NOKTASI OLARAK AYARLANARAK NESNELERIN OLUSTURULACAGI BASLANGIC KONUMU ALINIR */
        Vector3 startingSpawnPosition = new Vector3(-Mathf.Round(worldX * 0.5f), 0, -Mathf.Round(worldZ * 0.5f));

        /* SIRADAKI OLUSTURULACAK NESNENIN KONUMUNU TUTAR. ILK NESNE BASLANGIC NOKTASINDA OLUSTURULACAGI ICIN DEFAULT OLARAK startingSpawnPosition AYARLIDIR */
        Vector3 currentSpawnPos = startingSpawnPosition;

        /*
                YER YUZEYININ OLUSTURULDUGU KISIM
        */

        /* SAYAC */
        int counter = 0;

        /* OLUSTURULAN NESNENIN YUKSEKLIGINE GORE AYARLANACAK SCALE */
        Vector3 scale = new Vector3(1, 1, 1);

        /* X VE Z KONUMLARI GEZILIR VE NESNELER YERLESTIRILIR */
        for (int z = 0; z < worldZ; z++)
        {
            for (int x = 0; x < worldX; x++)
            {
                /* ISLEM YAPILACAK KONUM BELIRLENIR */
                spawnPositions[counter] = currentSpawnPos;
                currentSpawnPos.x++;

                /* ISLEM YAPILACAK COLOR SECILIR */
                Color cGround = pixGround[counter]; 
                Color cObjects = pixObjects[counter];


                /* OLUSTURULACAK GROUND YUKSEKLIGI SECILIR VE OLUSTURULUR */
                
                Debug.Log(cGround);

                for (int k = 0; k < groundColors.Length; k++)
                {
                    /* YERYUZU GORSELINDE PIKSELLER COK HIZLI DEGISIYORSA 'highAccuracy' BOOL'unu 'true' YAPMAK DOGRU SONUCU ORTAYA CIKARTIYOR. */
                    if( highAccuracy )
                    {
                        if (cGround == groundColors[k])
                        {
                            /* DEGISTIRILEN SCALE'E GORE YUKSEKLIK AYARI */
                            spawnPositions[counter].y += (k * 0.5f);
                            GameObject grnd = Instantiate(groundObject, spawnPositions[counter], Quaternion.identity);
                            scale.y += k;
                            grnd.transform.localScale = scale;
                            scale.y -= k;

                            /* OLUSTURULACAK NESNE SECILIR VE OLUSTURULUR */
                            for (int j = 0; j < objectsColors.Length; j++)
                            {
                                if (cObjects.Equals(objectsColors[j]))
                                {
                                    /* DEGISTIRILEN SCALE'E GORE YUKSEKLIK AYARI */
                                    spawnPositions[counter].y += (k * 0.5f) + 1;
                                    GameObject obj = Instantiate(placeableObjects[j], spawnPositions[counter], Quaternion.identity);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    else
                    {
                        if (cGround.Equals(groundColors[k]))
                        {

                            /* DEGISTIRILEN SCALE'E GORE YUKSEKLIK AYARI */
                            spawnPositions[counter].y += (k * 0.5f);
                            GameObject grnd = Instantiate(groundObject, spawnPositions[counter], Quaternion.identity);
                            scale.y += k;
                            grnd.transform.localScale = scale;
                            scale.y -= k;

                            /* OLUSTURULACAK NESNE SECILIR VE OLUSTURULUR */
                            for (int j = 0; j < objectsColors.Length; j++)
                            {
                                if (cObjects.Equals(objectsColors[j]))
                                {
                                    /* DEGISTIRILEN SCALE'E GORE YUKSEKLIK AYARI */
                                    spawnPositions[counter].y += (k * 0.5f) + 1;
                                    GameObject obj = Instantiate(placeableObjects[j], spawnPositions[counter], Quaternion.identity);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                    
                }

                counter++;
            }

            currentSpawnPos.x = startingSpawnPosition.x;
            currentSpawnPos.z++;
        }

    }
}


