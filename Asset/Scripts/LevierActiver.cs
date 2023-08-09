using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevierActiver : MonoBehaviour
{
    private int nblevier = 0;
    public TextMeshProUGUI textePointRestant;

    public delegate void OuvrirLaSortie();
    public static event OuvrirLaSortie ouverture;

    // Start is called before the first frame update
    void Start()
    {
        textePointRestant.text = "0";
        //ajoute les events 
        Activationlevier.activation += ChangerText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        //enleve les events connecter
        Activationlevier.activation -= ChangerText;
    }

    void ChangerText()
    {
        nblevier++;
        textePointRestant.text = nblevier.ToString();
        if(nblevier == 8){
        ouverture?.Invoke();
        }
    }
}
