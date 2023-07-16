using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prefab_sonido : MonoBehaviour
{
    // script Álvaro 

    void Start()
    {
        
    }


    void Update()
    {
        
    }
    void OnEnable()
    {
        StartCoroutine(Bullet_control());
    }
    IEnumerator Bullet_control()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            Debug.Log("TE HACE DAÑO");
            //Destroy(other.gameObject);
            //gameObject.SetActive(false);
        }
    }
}
