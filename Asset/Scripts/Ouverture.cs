using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ouverture : MonoBehaviour
{
    public GameObject cloture;

    // Start is called before the first frame update
    void Start()
    {
        //ajout des events
        LevierActiver.ouverture += OuvrirCloture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnDestroy()
    {
        //enleve les events connecter
        LevierActiver.ouverture -= OuvrirCloture;
    }
    void OuvrirCloture(){
        cloture.transform.position = new Vector3(cloture.transform.position.x,-5,cloture.transform.position.z);
    }
}
