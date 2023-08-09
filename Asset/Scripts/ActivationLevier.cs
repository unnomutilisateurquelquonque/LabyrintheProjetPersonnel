using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activationlevier : MonoBehaviour
{
    public GameObject levier;
    public GameObject lumiere;
    private bool active = false;

    public delegate void ActiverLevier();
    public static event ActiverLevier activation;

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
        if (other.CompareTag("Joueur") && active == false)
        {
            Activer();
        }
    }
    
    // Update is called once per frame
    void Activer()
    {
        // la position
        float positionX = levier.transform.position.x;
        float positionY = levier.transform.position.y;
        float positionZ = levier.transform.position.z;

        //une seule des valeurs est n'est pas zero alors le levier sera activé seulement dans la rotation actuel
        levier.transform.Rotate( 90.0f, 0.0f, 0.0f, Space.Self);
        //levier.transform.rotate = new Quaternion.Euler(-rotationX,-rotationY,0);
        levier.transform.position = new Vector3(positionX,positionY+0.2f,positionZ);
        lumiere.GetComponent<Light>().color = Color.green;
        active = true;
        activation?.Invoke();
    }
}
