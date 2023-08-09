using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Minotaure : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    //public Animation animation;
    public Animator animateur;
    public GameObject tableau;
    private bool[,] tableauPieceAccessible;
    private float espacement;
    private Vector3 positionInitial;
    private int grilleGrandeur;
    public GameObject joueur;
    private RaycastHit coup;
    public GameObject ligneDeVue;
    private bool activer = false;
    public int positionX;
    public int positionZ;
    private bool courrir = false;

    public delegate void Perdre();
    public static event Perdre joueurMort;


    void Start() 
    {
        //ajoute les events 
        Menu.commencerLaPartie += Activation;
        Activationlevier.activation += ChangerVitesse;
    }

    // Update is called once per frame
    void Update()
    {
        if (activer == true)
        {
            if(courrir){

            }
            ligneDeVue.transform.transform.LookAt(joueur.transform);
            if (Physics.Raycast (ligneDeVue.transform.position, ligneDeVue.transform.forward,out coup,50) && coup.transform.tag == "Joueur")
            {
                //le minotaure voit le joueur et le poursuit
                //animation.Play();
                agent.SetDestination(joueur.transform.position);
                StartCoroutine("Pursuite");
            }
            else if(agent.hasPath == false)
            {
                //le minotaure ne voit pas le joueur
                int xAleatoire = Random.Range(0,grilleGrandeur);
                int zAleatoire = Random.Range(0,grilleGrandeur);
                if(tableauPieceAccessible[xAleatoire,zAleatoire]==true){
                    Vector3 position = new Vector3(xAleatoire * -espacement, 0f, zAleatoire * espacement)+positionInitial;
                    //animation.Play();
                    agent.SetDestination(position);
                }
            }
        }
    }

    //lorsque l'object est detruit
    void OnDestroy()
    {
        //enleve les events connecter
        Menu.commencerLaPartie -= Activation;
        Activationlevier.activation -= ChangerVitesse;
    }

    //lors d'une collision
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.tag == "Joueur")
        {
            joueurMort?.Invoke();
            activer = false;
            agent.ResetPath();
            animateur.Play("Attaquer");
        }
    }

    //active le minotaure
    public void Activation()
    {
        activer = true;
        tableauPieceAccessible = tableau.GetComponent<GenerationDuLabyrithe>().tableauPieceAccessible;
        positionInitial = tableau.GetComponent<GenerationDuLabyrithe>().positionInitial;
        espacement = tableau.GetComponent<GenerationDuLabyrithe>().espacement;
        grilleGrandeur = tableau.GetComponent<GenerationDuLabyrithe>().grilleGrandeur;
    }

    private IEnumerator Pursuite()
    {
        float tempPasser = 0;
        while (tempPasser < 4)
        {
            tempPasser += Time.deltaTime;
            yield return null;      // Attend la prochaine frame avant de continuer l'ex�cution de la fonction
        }
        agent.ResetPath();
    }
    private void ChangerVitesse(){
        agent.speed = agent.speed + 0.5f;
        if(agent.speed >= 3)
        {
            animateur.Play("Courrir");
            courrir=true;
        }
    }
}
