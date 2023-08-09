using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.AI;
using Unity.AI.Navigation;

public class GenerationDuLabyrithe: MonoBehaviour
{
    public NavMeshSurface surface;
    public Vector3 positionInitial;
    public float espacement;
    public int grilleGrandeur;
    public GameObject joueur;
    public GameObject minotaure;
    public GameObject levier;
    public GameObject corridorGaucheDroite;
    public GameObject corridorHautBas;
    public GameObject tMurDroite;
    public GameObject tMurBas;
    public GameObject tMurGauche;
    public GameObject tMurHaut;
    public GameObject x;
    public GameObject coinBasVersGauche;
    public GameObject coinGaucheVersHaut;
    public GameObject coinHautVersDroite;
    public GameObject coinDroitVersBas;
    public GameObject Plein;
    public GameObject culDeSacDroite;
    public GameObject culDeSacBas;
    public GameObject culDeSacGauche;
    public GameObject culDeSacHaut;
    public GameObject[,] tableauPiece;
    public List<GameObject> possibiliteRestante;
    public bool[,] tableauPieceAccessible;
    private Vector3 position;
    private bool encourDeChangement = false;
    public bool beta;

    // Start is called before the first frame update
    void Start()
    {
        Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
        RenderSettings.fogColor = Color.black;
        RenderSettings.fog = true;
        tableauPiece= new GameObject[grilleGrandeur,grilleGrandeur];
        PlacerTuileInitial();
        //loop qui permet de rendre toute les zones accesible
        //je loop enleverZone car je ne veut pas enlever toute les zones inaccessibles et certaine zone inaccessibles sont à l'intériur ou à l'arrière d'autre zone
        //version beta
        if(beta){
            for(int k = 0; k<7; k++)
            {
                Generation();
                tableauPieceAccessible = new bool[grilleGrandeur,grilleGrandeur];
                VerifierSiAccessible(5,0);
                if(k < 6 )
                {

                    EnleverZoneInacessible();
                }
            }
        }
        else{
        //version stable
        Generation();
        tableauPieceAccessible = new bool[grilleGrandeur,grilleGrandeur];
        VerifierSiAccessible(5,0);
        }
        //Parcourir();
        GenerationDesLeviers();
        UnityEngine.AI.NavMesh.RemoveAllNavMeshData();
        surface.BuildNavMesh();
        GenereEnemi();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //génére les tuiles manquante dans le tableau et s'occupe des vérification des bordures
    void Generation()
    {
        EffaceEtRemplirLaListe();
        for (int i = 0; i < grilleGrandeur; i++) 
        {
            for (int j = 0; j < grilleGrandeur; j++)
            {
                if(tableauPiece[j,i] == null){
                    //Bordure du bas
                    if(i==0)
                    {
                        for(int k = 0; k<possibiliteRestante.Count; k++)
                        {
                            if(possibiliteRestante[k].gameObject.GetComponent<Mur>().murSud != true){
                                possibiliteRestante.RemoveAt(k);
                                //pour annuler la reduction de la liste lord de son parcour
                                k=k-1;
                            }
                        }
                    }
                    //Bordure du haut
                    else if(i==grilleGrandeur-1)
                    {
                        for(int k = 0; k<possibiliteRestante.Count; k++)
                        {
                            if(possibiliteRestante[k].gameObject.GetComponent<Mur>().murNord != true)
                            {
                                possibiliteRestante.RemoveAt(k);
                                //pour annuler la reduction de la liste lord de son parcour
                                k=k-1;
                            }
                        }
                    }
                    //Bordure a droite
                    if(j==0)
                    {
                        for(int k = 0; k<possibiliteRestante.Count; k++)
                        {
                            if(possibiliteRestante[k].gameObject.GetComponent<Mur>().murEst != true)
                            {
                                possibiliteRestante.RemoveAt(k);
                                //pour annuler la reduction de la liste lord de son parcour
                                k=k-1;
                            }
                        }
                    }
                    //Bordure a gauche
                    if(j==grilleGrandeur-1)
                    {
                        for(int k = 0; k<possibiliteRestante.Count; k++)
                        {
                            if(possibiliteRestante[k].gameObject.GetComponent<Mur>().murWest != true)
                            {
                                possibiliteRestante.RemoveAt(k);
                                //pour annuler la reduction de la liste lord de son parcour
                                k=k-1;
                            }
                        }
                    }
                    VerifierTuile(j,i);
                    int nombreAleatoire = Random.Range(0,possibiliteRestante.Count);
                    GameObject piece = Instantiate(possibiliteRestante[nombreAleatoire]);
                    tableauPiece[j,i] = piece;
                    position = new Vector3(j * -espacement, 0f, i * espacement);
                    piece.transform.position = position + positionInitial;
                    EffaceEtRemplirLaListe();
                }
            }
        }
    }

    // i et j sont les coordonné de de la tuile à vérifier
    // s'occupe des limitation avec les autres murs et celle supprimer par la fonction EnleverZoneInacessible
    void VerifierTuile(int j, int i)
    {
        //vérifie que la tuile n'est pas une bordure
        if(j != 0)
        {
            //verifie si la tuile à droite est vide
            if(tableauPiece[j-1,i] != null)
            {
                //tuile a droite
                if(tableauPiece[j-1,i].GetComponent<Mur>().murWest == true)
                {
                    for(int k = 0; k<possibiliteRestante.Count; k++)
                    {
                        //Cherche un mur
                        if(possibiliteRestante[k].gameObject.GetComponent<Mur>().murEst != true)
                        {
                            possibiliteRestante.RemoveAt(k);
                            //pour annuler la reduction de la liste lord de son parcour
                            k=k-1;
                        }   
                    }
                }    
                else
                {
                    //Cherche un trou
                    for(int k = 0; k<possibiliteRestante.Count; k++)
                    {
                        if(possibiliteRestante[k].gameObject.GetComponent<Mur>().murEst == true)
                        {
                            possibiliteRestante.RemoveAt(k);
                            //pour annuler la reduction de la liste lord de son parcour
                            k=k-1;
                        }
                    }   
                }
            }
            else if(encourDeChangement){
                    //Cherche un trou
                    for(int k = 0; k<possibiliteRestante.Count; k++)
                    {
                        if(possibiliteRestante[k].gameObject.GetComponent<Mur>().murEst == true)
                        {
                            possibiliteRestante.RemoveAt(k);
                            //pour annuler la reduction de la liste lord de son parcour
                            k=k-1;
                        }
                    } 
            }
        }
        //vérifie que la tuile n'est pas une bordure
        if(j != grilleGrandeur-1)
        {
            //verifie si la tuile à gauche est vide
            if(tableauPiece[j+1,i] != null)
            {
                //tuile a gauche      
                if(tableauPiece[j+1,i].GetComponent<Mur>().murEst == true)
                {
                    for(int k = 0; k<possibiliteRestante.Count; k++)
                    {
                        //Cherche un mur
                        if(possibiliteRestante[k].gameObject.GetComponent<Mur>().murWest != true)
                        {
                            possibiliteRestante.RemoveAt(k);
                            //pour annuler la reduction de la liste lord de son parcour
                            k=k-1;
                        }   
                    }
                }
                else
                {
                    for(int k = 0; k<possibiliteRestante.Count; k++)
                    {
                        //Cherche un trou
                        if(possibiliteRestante[k].gameObject.GetComponent<Mur>().murWest == true)
                        {
                            possibiliteRestante.RemoveAt(k);
                            //pour annuler la reduction de la liste lord de son parcour
                            k=k-1;
                        }
                    }   
                }
            }
            else if(encourDeChangement){
                    //Cherche un trou
                    for(int k = 0; k<possibiliteRestante.Count; k++)
                    {
                        if(possibiliteRestante[k].gameObject.GetComponent<Mur>().murWest == true)
                        {
                            possibiliteRestante.RemoveAt(k);
                            //pour annuler la reduction de la liste lord de son parcour
                            k=k-1;
                        }
                    } 
            }
        }
        //vérifie que la tuile n'est pas une bordure
        if(i != grilleGrandeur-1)
        {
            //verifie si la tuile au dessus est vide
            if(tableauPiece[j,i+1] != null)
            {
                //tuile au dessus
                if(tableauPiece[j,i+1].GetComponent<Mur>().murSud == true)
                {
                    for(int k = 0; k<possibiliteRestante.Count; k++)
                    {
                        //Cherche un mur
                        if(possibiliteRestante[k].gameObject.GetComponent<Mur>().murNord != true)
                        {
                            possibiliteRestante.RemoveAt(k);
                            //pour annuler la reduction de la liste lord de son parcour
                            k=k-1;
                        }
                    }   
                }
                else
                {
                    for(int k = 0; k<possibiliteRestante.Count; k++)
                    {
                        //Cherche un trou
                        if(possibiliteRestante[k].gameObject.GetComponent<Mur>().murNord == true)
                        {
                            possibiliteRestante.RemoveAt(k);
                            //pour annuler la reduction de la liste lord de son parcour
                            k=k-1;
                        }
                    }   
                }
            }
            else if(encourDeChangement){
                    //Cherche un trou
                    for(int k = 0; k<possibiliteRestante.Count; k++)
                    {
                        if(possibiliteRestante[k].gameObject.GetComponent<Mur>().murNord == true)
                        {
                            possibiliteRestante.RemoveAt(k);
                            //pour annuler la reduction de la liste lord de son parcour
                            k=k-1;
                        }
                    } 
            }
        }
        if(i != 0)
        {
            //verifie si la tuile en dessous est vide
            if(tableauPiece[j,i-1] != null)
            {
                //tuile en dessous
                if(tableauPiece[j,i-1].GetComponent<Mur>().murNord == true)
                {
                    //Cherche un mur
                    for(int k = 0; k<possibiliteRestante.Count; k++)
                    {
                        if(possibiliteRestante[k].gameObject.GetComponent<Mur>().murSud != true)
                        {
                            possibiliteRestante.RemoveAt(k);
                            //pour annuler la reduction de la liste lord de son parcour
                            k=k-1;
                        }
                    }   
                }
                else
                {
                    //Cherche un trou
                    for(int k = 0; k<possibiliteRestante.Count; k++)
                    {
                        if(possibiliteRestante[k].gameObject.GetComponent<Mur>().murSud == true)
                        {
                            possibiliteRestante.RemoveAt(k);
                            //pour annuler la reduction de la liste lord de son parcour
                            k=k-1;
                        }
                    }   
                }
            }
            else if(encourDeChangement){
                    //Cherche un trou
                    for(int k = 0; k<possibiliteRestante.Count; k++)
                    {
                        if(possibiliteRestante[k].gameObject.GetComponent<Mur>().murSud == true)
                        {
                            possibiliteRestante.RemoveAt(k);
                            //pour annuler la reduction de la liste lord de son parcour
                            k=k-1;
                        }
                    } 
            }
        }
    }

    //remplace les tuiles à l'entrer part une tuile sans mur
    void PlacerTuileInitial()
    {
        //les 3 tuiles à l'entrer
        //millieu
        Destroy(tableauPiece[5,0]);
        GameObject pieceEntrer1 = Instantiate(x);
        tableauPiece[5,0] = pieceEntrer1;
        position = new Vector3(5 * -espacement, 0f, 0 * espacement);
        pieceEntrer1.transform.position = position + positionInitial;
        //gauche
        Destroy(tableauPiece[7,0]);
        GameObject pieceEntrer2 = Instantiate(x);
        tableauPiece[7,0] = pieceEntrer2;
        position = new Vector3(7 * -espacement, 0f, 0 * espacement);
        pieceEntrer2.transform.position = position + positionInitial;
        //droite
        Destroy(tableauPiece[3,0]);
        GameObject pieceEntrer3 = Instantiate(x);
        tableauPiece[3,0] = pieceEntrer3;
        position = new Vector3(3 * -espacement, 0f, 0 * espacement);
        pieceEntrer3.transform.position = position + positionInitial;      
    }

    //On efface et remplie le tableau pour la prochaine tuile
    void EffaceEtRemplirLaListe()
    {
        possibiliteRestante.Clear();
        possibiliteRestante.Add(corridorGaucheDroite);
        possibiliteRestante.Add(corridorHautBas);
        possibiliteRestante.Add(tMurDroite);
        possibiliteRestante.Add(tMurBas);
        possibiliteRestante.Add(tMurGauche);
        possibiliteRestante.Add(tMurHaut);
        possibiliteRestante.Add(x);
        possibiliteRestante.Add(coinBasVersGauche);
        possibiliteRestante.Add(coinGaucheVersHaut);
        possibiliteRestante.Add(coinHautVersDroite);
        possibiliteRestante.Add(coinDroitVersBas);
        possibiliteRestante.Add(Plein);
        possibiliteRestante.Add(culDeSacDroite);
        possibiliteRestante.Add(culDeSacBas);
        possibiliteRestante.Add(culDeSacGauche);
        possibiliteRestante.Add(culDeSacHaut);
    }

    //génère les leviers à des endroit aléatoire et accessible par le joueur
    void GenerationDesLeviers()
    {
        for(int k = 0;k < 8; k++)
            {
            int xAleatoire = Random.Range(0,grilleGrandeur);
            int zAleatoire = Random.Range(0,grilleGrandeur);
            //vérifie si la tuile est pleine. On ne veut pas un levier dans un mur
            if(tableauPiece[xAleatoire,zAleatoire].name == "Plein(Clone)")
            {
                k--;
            }
            //vérifie si la tuile est accessible. On ne veut pas une partie impossible a terminer.
            else if(tableauPieceAccessible[xAleatoire,zAleatoire] == false)
            {
                k--;
            }
            else if(tableauPiece[xAleatoire,zAleatoire].GetComponent<Mur>().murNord == true)
            {
                    //Génération du levier
                    GameObject levierIns = Instantiate(levier);
                    position = new Vector3(xAleatoire * -espacement, 2f, zAleatoire * espacement+2.45f);
                    levierIns.transform.position = position + positionInitial;
                    //permet de s'assuré que les leviers ne se génére pas au même endroit
                    tableauPieceAccessible[xAleatoire,zAleatoire] = false;
            }
            else if(tableauPiece[xAleatoire,zAleatoire].GetComponent<Mur>().murEst == true)
            {
                    //Génération du levier
                    GameObject levierIns = Instantiate(levier);
                    position = new Vector3(xAleatoire * -espacement + 2.45f, 2f, zAleatoire * espacement);
                    levierIns.transform.position = position + positionInitial;
                    levierIns.transform.Rotate(0.0f, 90.0f, 0.0f, Space.Self);
                    //permet de s'assuré que les leviers ne se génére pas au même endroit
                    tableauPieceAccessible[xAleatoire,zAleatoire] = false;
            }
            else if(tableauPiece[xAleatoire,zAleatoire].GetComponent<Mur>().murSud == true)
            {
                    //Génération du levier
                    GameObject levierIns = Instantiate(levier);
                    position = new Vector3(xAleatoire * -espacement, 2f, zAleatoire * espacement-2.45f);
                    levierIns.transform.position = position + positionInitial;
                    levierIns.transform.Rotate( 0.0f, 180.0f, 0.0f, Space.Self);
                    //permet de s'assuré que les leviers ne se génére pas au même endroit
                    tableauPieceAccessible[xAleatoire,zAleatoire] = false;
            }
            else if(tableauPiece[xAleatoire,zAleatoire].GetComponent<Mur>().murWest == true)
            {
                    //Génération du levier
                    GameObject levierIns = Instantiate(levier);
                    position = new Vector3(xAleatoire * -espacement - 2.45f, 2f, zAleatoire * espacement);
                    levierIns.transform.position = position + positionInitial;
                    levierIns.transform.Rotate( 0.0f, 270.0f, 0.0f, Space.Self);
                    //permet de s'assuré que les leviers ne se génére pas au même endroit
                    tableauPieceAccessible[xAleatoire,zAleatoire] = false;
            }
            else
            {
                //on cherche une nouvelle tuile car la tuile n'a pas de mur pour le levier
                k--;
            }
        }
    }
    
    //vérifie si les tuile sont accessible ou non
    void VerifierSiAccessible(int j,int i)
    {
        tableauPieceAccessible[j,i] = true;
        if(j+1 < grilleGrandeur-1)
        {
            if(tableauPiece[j,i].GetComponent<Mur>().murWest != true && tableauPieceAccessible[j+1,i] == false)
            {
                VerifierSiAccessible(j+1,i);
            }
        }
        if(j-1 >= 0 )
        {
            if(tableauPiece[j,i].GetComponent<Mur>().murEst != true && tableauPieceAccessible[j-1,i] == false)
            {
                VerifierSiAccessible(j-1,i);
            }
        }
        if(i+1 < grilleGrandeur-1)
        {
            if(tableauPiece[j,i].GetComponent<Mur>().murNord != true && tableauPieceAccessible[j,i+1] == false)
            {
                VerifierSiAccessible(j,i+1);
            }
        }
        if(i-1 >= 0 )
        {
            if(tableauPiece[j,i].GetComponent<Mur>().murSud != true && tableauPieceAccessible[j,i-1] == false)
            {
                VerifierSiAccessible(j,i-1);
            }
        }
    }

    //affiche le nombre de piece accessible
    void Parcourir()
    {
        int nbPieceAccessible = 0;
        for (int i = 0; i < grilleGrandeur; i++) 
        {
            for (int j = 0; j < grilleGrandeur; j++)
            {
                if(tableauPieceAccessible[j,i] == true){
                    nbPieceAccessible++;
                }
            }
        }
        Debug.Log("Il y a " + nbPieceAccessible + " piece accessible dans le labyrinthe");
    }

    //Choisie une tuile aléatoire pour le minotaure
    void GenereEnemi(){
        bool enemyPlacer = false;
        while(enemyPlacer == false)
        {
            int xAleatoire = Random.Range(0,grilleGrandeur);
            int zAleatoire = Random.Range(0,grilleGrandeur);
            //vérifie si la tuile est accessible. On ne veut pas une partie trop facile à gagner.
            if(tableauPieceAccessible[xAleatoire,zAleatoire] == true && tableauPiece[xAleatoire,zAleatoire].name != "Plein(Clone)")
            {
                if(zAleatoire < 5 && tableauPieceAccessible[xAleatoire,5] == true){
                    zAleatoire = 5;
                }
                position = new Vector3(xAleatoire * -espacement, 2f, zAleatoire * espacement);
                minotaure.GetComponent<Minotaure>().positionX = xAleatoire;
                minotaure.GetComponent<Minotaure>().positionZ = zAleatoire;
                minotaure.GetComponent<Minotaure>().joueur = joueur;
                minotaure.transform.position = position + positionInitial;
                //active les composante qui block la transformation de la position
                minotaure.GetComponent<Collider>().enabled = true;
                minotaure.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
                enemyPlacer = true;
            }
        }

    }

    //Enleve des tuiles qui cause le labyrinthe à être plus petit
    void EnleverZoneInacessible(){
        for (int i = 0; i < grilleGrandeur; i++) 
        {
            for (int j = 0; j < grilleGrandeur; j++)
            {   
                encourDeChangement = false;
                // 33.33% du temp le mur sera detruit on ne veut pas que les zone innacessible soit tous des tuiles X
                int nbAleatoire = Random.Range(1,4);
                if(tableauPieceAccessible[j,i] == false && nbAleatoire == 3 && tableauPiece[j,i].name != "Plein(Clone)")
                {
                    if(j+1 < grilleGrandeur-1)
                    {
                        if(tableauPieceAccessible[j+1,i] == true)
                        {
                            //supression des tuiles problématique
                            Destroy(tableauPiece[j,i]);
                            tableauPiece[j,i] = null;
                            Destroy(tableauPiece[j+1,i]);
                            tableauPiece[j+1,i] = null;
                            encourDeChangement = true;
                        }
                    }
                    if(j-1 >= 0 )
                    {
                        if(tableauPieceAccessible[j-1,i] == true && encourDeChangement == false)
                        {
                            //supression des tuiles problématique
                            Destroy(tableauPiece[j,i]);
                            tableauPiece[j,i] = null;
                            Destroy(tableauPiece[j-1,i]);
                            tableauPiece[j-1,i] = null;
                            encourDeChangement = true;
                        }
                    }
                    if(i+1 < grilleGrandeur-1)
                    {
                        if(tableauPieceAccessible[j,i+1] == true  && encourDeChangement == false)
                        {
                            //supression des tuiles problématique
                            Destroy(tableauPiece[j,i]);
                            tableauPiece[j,i] = null;
                            Destroy(tableauPiece[j,i+1]);
                            tableauPiece[j,i+1] = null;
                            encourDeChangement = true;
                        }
                    }
                    if(i-1 >= 0  && encourDeChangement == false)
                    {
                        if(tableauPieceAccessible[j,i-1] == true )
                        {
                            //supression des tuiles problématique
                            Destroy(tableauPiece[j,i]);
                            tableauPiece[j,i] = null;
                            Destroy(tableauPiece[j,i-1]);
                            tableauPiece[j,i-1] = null;
                            encourDeChangement = true;
                        }
                    }
                }
            }
        }
        //permet de remplie les tuiles qui vont vers le vide de généré un passage entre les tuiles
        encourDeChangement = true;
    }
}
