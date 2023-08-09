using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Sortie : MonoBehaviour
{
    public TextMeshProUGUI texteVictoire;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Joueur"))
        {
            Gagner();
        }
    }
    
    //une fois que le joueur gagne cette function est invoké
    public void Gagner()
    {
        texteVictoire.text = "Vous avez gagné !";
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
