using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Mort : MonoBehaviour
{
    public TextMeshProUGUI texteMort;
    // Start is called before the first frame update
    void Start()
    {
        //ajoute les events 
        Minotaure.joueurMort += Perdu;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //lorsque l'object est detruit
    void OnDestroy()
    {
        //enleve les events connecter
        Minotaure.joueurMort -= Perdu;
    }
    
    //une fois que le joueur gagne cette function est invoké
    public void Perdu()
    {
        texteMort.text = "Vous êtes mort";
        StartCoroutine("RetourAuMenu");
    }

    private IEnumerator RetourAuMenu()
    {
        float tempPasser = 0;
        while (tempPasser < 1)
        {
            tempPasser += Time.deltaTime;
            yield return null;      // Attend la prochaine frame avant de continuer l'ex�cution de la fonction
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene( SceneManager.GetActiveScene().name);
    }
}
