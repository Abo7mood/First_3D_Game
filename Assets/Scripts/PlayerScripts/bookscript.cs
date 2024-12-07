using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bookscript : MonoBehaviour
{
    public GameObject book;
    public GameObject vfx;
    public GameObject fakebook;
    public GameObject fakebook1;
    // Start is called before the first frame update
    private void Awake()
    {
        fakebook1.SetActive(false);
        fakebook.SetActive(false);
        vfx.SetActive(false);
        book.SetActive(false);
    }
   
   
    public void playertriggerenter()
    {
        fakebook.SetActive(true);
    }
    public void playertriggerenter2()
    {
        vfx.SetActive(true);
        book.SetActive(true);
        fakebook.SetActive(false);
    }
    public void playertriggerexit()
    {
        vfx.SetActive(false);
        book.SetActive(false);
        fakebook1.SetActive(true);
    }
    public void playertriggerexit2()
    {
        fakebook1.SetActive(false);
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playertriggerenter();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playertriggerexit();
        }
    }
}
