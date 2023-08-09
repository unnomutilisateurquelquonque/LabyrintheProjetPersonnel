using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	// Start is called before the first frame update

	public Button helloWorld;
    public Button commencerPartie;
    public Button quitter;
    public GameObject menu;

    public delegate void PartieCommencer();
    public static event PartieCommencer commencerLaPartie;

	void Start()
	{
		Button helloWorldBoutton = helloWorld.GetComponent<Button>();
        Button commencerBoutton = commencerPartie.GetComponent<Button>();
        Button quitterBoutton = quitter.GetComponent<Button>();
		helloWorldBoutton.onClick.AddListener(HelloWorld);
        commencerBoutton.onClick.AddListener(CommencerPartie);
        quitterBoutton.onClick.AddListener(QuitterJeu);
	}

    //Affiche un message log "hello world"
	void HelloWorld()
	{
        Debug.Log("helloWorld");
	}

    //Ferme le menu et commence le jeu
	void CommencerPartie()
	{
        menu.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        commencerLaPartie?.Invoke();
	}
    void QuitterJeu()
    {
        //dans l'éditeur
        UnityEditor.EditorApplication.isPlaying = false;
        //Dans l'app
        Application.Quit();
    }
}

