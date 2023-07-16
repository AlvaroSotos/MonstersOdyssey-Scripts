using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataSaver : MonoBehaviour
{

    // script Álvaro 
    // Start is called before the first frame update
    public int what = 0;
    public GameObject kayn;
     public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/MyGameData.dat");

        MyData puro_data = new MyData();
        puro_data.stats.current_life = 100;
        puro_data.stats.name = "luisja";
        //puro_data.stats.position = kayn.transform.position;
        puro_data.stats.x = kayn.transform.position.x;
        puro_data.stats.y = kayn.transform.position.y;
        puro_data.stats.z = kayn.transform.position.z;
        //puro_data.stats.rotation = kayn.transform.rotation;


        bf.Serialize(file, puro_data);
        file.Close();
    }

     public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/MyGameData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = File.Open(Application.persistentDataPath + "/MyGameData.dat", FileMode.Open);
            MyData puro_data = bf.Deserialize(fs) as MyData;
            fs.Close();

            if (puro_data != null)
            {
                kayn.GetComponent<CharacterController>().enabled = false;
                Vector3 hola = new Vector3(puro_data.stats.x, puro_data.stats.y, puro_data.stats.z);
                kayn.transform.position = hola;
                kayn.GetComponent<CharacterController>().enabled = true;

                //kayn.transform.position = puro_data.stats.position;
                //kayn.transform.rotation = puro_data.stats.rotation;

                Debug.Log("Vida actual: " + puro_data.stats.current_life);
                Debug.Log("Nombre: " + puro_data.stats.name);
            }
        }
    }
}
