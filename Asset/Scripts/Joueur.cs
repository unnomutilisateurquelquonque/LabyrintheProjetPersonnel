using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Joueur : MonoBehaviour
{
    //public Camera minimap;

    //vitesse de rotation de la cam
    public float vitesseRotationCamera;

    //vitesse de movement du joueur
    public float vitesse;

    //sensibilité de la souris
    public float sensibiliteSouris = 100f;

    //courir
    private float vitesseCourir;
    private bool courir = false;

    /*
    //audio pour music
    public AudioClip music;
    public AudioClip musicMenu;
    public AudioClip victoire;
    public AudioClip perdre;
    public AudioSource source;
    */

    /*
    //audio pour les effects
    public AudioClip collecter;
    public AudioSource effect;
    */

    // Contr�les
    [Header("Controles")]
    public KeyCode toucheAvancer = KeyCode.W;
    public KeyCode toucheGauche = KeyCode.A;
    public KeyCode toucheReculer = KeyCode.S;
    public KeyCode toucheDroite = KeyCode.D;
    public KeyCode toucheTablette = KeyCode.F;
    public KeyCode toucheCourrir = KeyCode.RightShift;

    private bool jeuEncour = false;

    public delegate void ChangerInfoTab();
    public static event ChangerInfoTab changerInfo;


    // Bornes
    [Header("Bornes")]
    public float angleMaxCamX;


    // Start is called before the first frame update
    void Start()
    {
        //ajoute les events 
        vitesseCourir = vitesse * 2;
        //source.clip = musicMenu;
        //source.loop = true;
        //source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        //if (jeuEncour == true) {
            Mouvement();
            courir = false;
        //}
    }


    void OnDestroy()
    {

    }

    //tout les mouvement effectuer par le joueur
    public void Mouvement()
    {
        //changer vitesse
        if (Input.GetKey(toucheCourrir)) 
        {
            courir = true;
        }
        // la camera tourne selon la direction de la souri
        float rotationX = Input.GetAxis("Mouse Y");
        float rotationY = Input.GetAxis("Mouse X");
        Camera camJoueur = GetComponent<Camera>();
        Rigidbody joueur = GetComponent<Rigidbody>();
        camJoueur.transform.Rotate(0f, rotationY * sensibiliteSouris * vitesseRotationCamera * Time.deltaTime, 0f);
        camJoueur.transform.Rotate(-1 * rotationX * sensibiliteSouris * vitesseRotationCamera * Time.deltaTime, 0f, 0f);
        float angleCamX = camJoueur.transform.localRotation.eulerAngles.x;
        float angleCamY = camJoueur.transform.localRotation.eulerAngles.y;
        camJoueur.transform.localRotation = Quaternion.Euler(angleCamX, angleCamY, 0f);

        // repositionement cam si hors limite
        // On positionne la tête dans l'intervalle demandée en X
        if (angleCamX > angleMaxCamX && angleCamX < 179f)
        {
            joueur.transform.localRotation = Quaternion.Euler(angleMaxCamX, angleCamY, 0f);
        }
        // Les angles négatifs sont traités de façon positive par eulerAngles (toujours sur l'intervalle 0 - 360)
        else if (angleCamX < 360f - angleMaxCamX && angleCamX > 181f)
        {
            joueur.transform.localRotation = Quaternion.Euler(-angleMaxCamX, angleCamY, 0f);
        }
        //gérer avancer et reculer
        if (Input.GetKey(toucheAvancer))
        {
            if (courir == true) 
            {
                joueur.velocity = transform.forward * vitesseCourir;
            }
            else
            {
                joueur.velocity = transform.forward * vitesse;
            }
        }
        else if (Input.GetKey(toucheReculer)) 
        {
            joueur.velocity = -transform.forward * vitesse;
        }
        //gérer gauche et droite
        if (Input.GetKey(toucheGauche))
        {
            joueur.velocity = -transform.right * vitesse;
        }
        else if (Input.GetKey(toucheDroite))
        {
            joueur.velocity = transform.right * vitesse;
        }

    }

    void Initialiser()
    {
        jeuEncour = true;
        /*
        source.clip = music;
        source.loop = true;
        source.Play();
        */
        Cursor.lockState = CursorLockMode.Locked;
    }
    /*
    void Collecter() 
    {
        effect.clip = collecter;
        source.loop = false;
        effect.Play();
    }*/
}