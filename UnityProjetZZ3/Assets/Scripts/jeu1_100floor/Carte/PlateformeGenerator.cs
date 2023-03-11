using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// Script g�rant la g�n�ration des plateformes en continue.
/// Instanciation de toutes les plateformes et gestion avec des pools d'objects.
/// Mise en place des plateformes sur tout l'�cran au lancement du jeu.
/// </summary>
public class PlateformeGenerator : MonoBehaviour
{
    // Param�tres des pools de plateformes
    const int NB_PLATEFORMES = 5;
    const int POOL_SIZE = 10;

    // Prefab des plateformes stock�es dedans (directement depuis l'inspector)
    public GameObject[] PlateformesPrefabs = new GameObject[NB_PLATEFORMES];

    private GameObject[][] _poolPlateforme; // Pools de plateformes
    private GameObject _lastPlateforme = null; // R�f�rence sur la derni�re plateforme activ�e

    // Variable de calculs
    private float _hauteurPlayer = 0;
    private float _distanceEntrePlateformes = 0;
    private float _ratioHauteurPlayer = 1.0f;

    // Bornes des x pour l'activation des plateformes
    private float _xMinEcran;
    private float _xMaxEcran;

    // La vitesse doit d�pendre de la taille de l'�cran
    // Multiplier par Jeu.transform.scale
    public float VitesseCarte; // Vitesse de mont�e des plateformes

    // Start is called before the first frame update
    void Start()
    {
        // Set up les pool des plateformes
        CreatePlateformesPool();

        // R�cup�ration de valeur
        _hauteurPlayer = GameObject.Find("Player").GetComponent<BoxCollider2D>().size.y * GameObject.Find("Player").transform.lossyScale.y;
        _distanceEntrePlateformes = _ratioHauteurPlayer * _hauteurPlayer;

        _xMinEcran = GameObject.Find("ContourCarte").GetComponent<ExtremiteBoxCollider2D>().GetPositionLeftCollider2D().x;
        _xMaxEcran = GameObject.Find("ContourCarte").GetComponent<ExtremiteBoxCollider2D>().GetPositionRightCollider2D().x;

        // G�n�ration de la carte
        GeneratePlateformeStart();

        // Set Player au centre et sur une plateforme simple au d�but de la game
        SetCenterPlayerPlateformeAtStart();
    }



    /// <summary>
    /// G�n�re une plateforme pour la continuit� de la carte (ou � la position indiqu�e en param�tre).
    /// Si le param�tre est non d�fini, alors il est null et un nouvelle position est calcul�e pour l'activation de la plateforme.
    /// <paramref name="positionGeneration">Vector3 qui indique la position o� l'on souhaite g�n�rer la plateforme.</param>
    /// </summary>
    public void GeneratePlateforme(Vector3? positionGeneration = null)
    {

        // Choix de la plateforme
        GameObject plateforme = ChoisirPlateformeInPools();

        // R�cup�ration de la position d'activation
        if (positionGeneration == null)
            positionGeneration = FindPostionActivation(plateforme);

        // Activation de la plateforme � la position d�finie
        ActivationPlateformeFromPosition(plateforme, (Vector3)positionGeneration);

        _lastPlateforme = plateforme; // Garder en m�moire la derni�re plateforme activ�e
    }

    /// <summary>
    /// R�cup�re une plateforme disponible dans les pools.
    /// </summary>
    /// <returns>Plateforme disponible.</returns>
    /// <exception cref="System.Exception">Pas de plateformes disponibles.</exception>
    private GameObject ChoisirPlateformeInPools()
    {
        int choixPlateformes; int nbPoolTest = 0;
        List<int> poolDispo = new List<int> { 0, 1, 2, 3, 4 }; // 5 types de plateformes

        // Tant que l'on a pas tester tous les pools de plateformes
        while (nbPoolTest < NB_PLATEFORMES)
        {
            // Choix d'un type de plateforme
            choixPlateformes = poolDispo[Random.Range(0, poolDispo.Count)];
            //choixPlateformes = 1; // TODO a changer pour forcer le spwan de plateforme que d'un type

            // R�cup�ration d'une plateforme disponible dans le pool choisi (choixPlateformes)
            var plateforme = ChoisirPlateformeInPool(choixPlateformes);

            // Si la plateforme est ok => on la retourn
            if (plateforme != null) { return plateforme; }

            Debug.LogWarning($"Plus de plateformes de type {choixPlateformes} disponibles.");
            poolDispo.Remove(choixPlateformes);
            ++nbPoolTest;
        }
        throw new System.Exception("Aucune plateformes disponibles dans les pool.");
    }

    /// <summary>
    /// R�cup�re une plateforme disponible dans un pool sp�cifique.
    /// </summary>
    /// <param name="poolIndice">Indice du pool choisi.</param>
    /// <returns>Une plateforme pas activ�, ou null.</returns>
    private GameObject ChoisirPlateformeInPool(int poolIndice)
    {
        // Pour toute les plateformes d'un certain type, dans un pool
        foreach (GameObject p in _poolPlateforme[poolIndice])
        {
            // Check si la plateforme est d�j� activ�e
            if (!p.activeInHierarchy)
            {
                return p; // Plateforme disponible
            }
        }
        Debug.LogWarning($"Plus de plateformes de type {poolIndice} disponibles.");
        return null;
    }

    /// <summary>
    /// Active un GameObject sur une position donn�e. 
    /// </summary>
    /// <param name="plateforme">Plateforme desactiv�e.</param>
    /// <param name="positionActivation">Position de l'activation de la plateforme.</param>
    private void ActivationPlateformeFromPosition(GameObject plateforme, Vector3 positionActivation)
    {
        if (plateforme.activeSelf) // Check s'elle est bien desactiv�e
            Debug.LogWarning($"Le GameObject {plateforme.name} est d�j� activ�e et est situ� � la position {plateforme.transform.position}.");

        var extremiteCollider = plateforme.GetComponent<ExtremiteBoxCollider2D>();
        //var extremiteSpriteRenderer = plateforme.GetComponent<ExtremiteSpriteRenderer>();

        plateforme.transform.position = positionActivation;
        plateforme.SetActive(true);
        // Re set la vitesse des plateformes (normalement pas utile)
        plateforme.GetComponent<MovePlateforme>().Speed = VitesseCarte;

        // D�calage plateforme sur les Y pour qu'avec la plateforme pr�c�dente,
        // la distance entre les 2 soient bien _distanceEntrePlateformes (en tenant compte des BoxCollider)
        extremiteCollider.DecalagePositionToDownCollider2D();

        // Recentre la plateforme pour qu'elle ne sorte pas de l'�cran
        RecentrerPlateforme(extremiteCollider);
    }

    /// <summary>
    /// Recentre les plateformes pour qu'elles soient toujours dans la carte et ne d�bordent pas.
    /// </summary>
    /// <param name="extremiteCollider">Script qui de checker les positions des extremit�s de la plateforme.</param>
    private void RecentrerPlateforme(ExtremiteBoxCollider2D extremiteCollider)
    {
        int varDeSecu = 0; // Sert � v�rifier que l'on reste pas dans la boucle

        // Tant que le cot� gauche du collider est en dehors de la map, on d�cale la plateforme vers la droite
        while (varDeSecu < 100 && extremiteCollider.GetPositionLeftCollider2D().x < _xMinEcran)
        {
            //Debug.Log($"{plateforme.name} {extremiteCollider.GetPositionLeftCollider2D().x} < {_xMinEcran} et varDeSecu:{varDeSecu}");
            ++varDeSecu;
            extremiteCollider.DecalagePositionToRightCollider2D();
        }

        varDeSecu = 0;
        // Tant que le cot� droit du collider est en dehors de la map, on d�cale la plateforme vers la gauche
        while (varDeSecu < 100 && extremiteCollider.GetPositionRightCollider2D().x > _xMaxEcran)
        {
            //Debug.Log($"{plateforme.name} {extremiteCollider.GetPositionRightCollider2D().x} > {_xMaxEcran} et varDeSecu:{varDeSecu}");
            ++varDeSecu;
            extremiteCollider.DecalagePositionToLeftCollider2D();
        }
    }


    /// <summary>
    /// Calcul la position o� activer la prochaine plateforme.
    /// En y : l'extremit� basse du collider de la pr�cedente plateforme.
    /// En x : une valeur al�atoire entre les bornes min et max de la carte.
    /// </summary>
    /// <param name="plateformeAActiver"></param>
    /// <returns></returns>
    private Vector3 FindPostionActivation(GameObject plateformeAActiver)
    {
        Vector3 positionActivation = Vector3.zero;

        if (_lastPlateforme != null)
        {
            // Find Y position
            Vector3 extremiteBasse = _lastPlateforme.GetComponent<ExtremiteBoxCollider2D>().GetPositionDownCollider2D();
            positionActivation = extremiteBasse + _distanceEntrePlateformes * Vector3.down;

            // Find X position
            float x = Random.Range(_xMinEcran, _xMaxEcran);
            // TODO : fonction qui utilise un al�atoire suivant une g�n�ration bien
            positionActivation += x * Vector3.right;
        }
        return positionActivation;
    }

    /// <summary>
    /// Construit les pool des diff�rentes plateformes. (Instatiation des diff�rents �l�ments des pool)
    /// </summary>
    private void CreatePlateformesPool()
    {
        // Cr�ation des diff�rents pools pour les plateformes
        _poolPlateforme = new GameObject[NB_PLATEFORMES][];

        // Instiation des pools des diff�rentes type de plateformes
        for (int p = 0; p < NB_PLATEFORMES; ++p)
        {
            _poolPlateforme[p] = new GameObject[POOL_SIZE]; // Instanciation du pool de plateformes vide

            // Instantiation de toutes les plateformes d'un type
            for (int k = 0; k < POOL_SIZE; ++k)
            {
                GameObject obj = Instantiate(PlateformesPrefabs[p], transform); // Instanciation de l'objet plateforme
                obj.name = obj.name + k.ToString(); // Rennomage de la plateforme
                obj.GetComponent<MovePlateforme>().Speed = VitesseCarte; // Parametrage de la vitesse 
                obj.SetActive(false); // D�sactivation de la plateforme
                _poolPlateforme[p][k] = obj; // Ajout de la plateforme � son pool
            }
        }
    }


    /***************************** METHODE START ************************************/

    /// <summary>
    /// G�n�ration des plateformes sur toutes la hauteur de la carte au start.
    /// </summary>
    private void GeneratePlateformeStart()
    {
        const string nameObjectHautEcran = "ZoneHautEcran";
        const string nameObjectBasEcran = "ZoneBasEcran";

        // R�cup�ration des extremit�s verticales de la carte
        var positionZoneHautEcran = GameObject.Find(nameObjectHautEcran).GetComponent<Transform>().position;
        var positionZoneBasEcran = GameObject.Find(nameObjectBasEcran).GetComponent<Transform>().position;

        // Calcul de nombre de plateformes n�cessaire pour remplir l'�cran
        float hauteurCarte = positionZoneHautEcran.y - positionZoneBasEcran.y;
        float nbPlateformesEcran = hauteurCarte / 1.5f / _distanceEntrePlateformes;

        // Activation de la premi�re plateforme en haut de l'�cran
        GeneratePlateforme(positionGeneration: positionZoneHautEcran);

        // Activation des autres plateformes pour remplir la hauteur de l'�cran
        for (int i = 0; i < nbPlateformesEcran; i++)
        { GeneratePlateforme(); }
    }

    /// <summary>
    /// Mise en place d'une plateforme simple au centre de l'�cran avec le Player dessus.
    /// </summary>
    private void SetCenterPlayerPlateformeAtStart()
    {
        var transformPlayer = GameObject.Find("Player").transform;
        var transformContourCarte = GameObject.Find("ContourCarte").transform;

        GameObject plateformeCenter = FindPlateformeAtCenter(transformContourCarte);
        if (plateformeCenter == null)
        { Debug.LogWarning("Pas de plateforme au centre de l'�cran trouv�e"); }

        // Re-centrage de la plateforme au centre de l'�cran sur les x.
        float xCenter = (_xMaxEcran + _xMinEcran) / 2;
        var centerPlateformPosition = new Vector3(xCenter, plateformeCenter.transform.position.y, plateformeCenter.transform.position.z);

        // Changement par une plateforme simple
        plateformeCenter.SetActive(false); // Desactivation de l'ancienne plateforme
        plateformeCenter = ChoisirPlateformeInPool(2);  // R�cup�raction d'un plateforme simple disponible
        ActivationPlateformeFromPosition(plateformeCenter, centerPlateformPosition); // Activation de la plateforme simple

        // R�cup�ration du haut de la plateforme
        Vector3 positionHautColliderPlateforme = plateformeCenter.GetComponent<ExtremiteBoxCollider2D>().GetPositionUpCollider2D();

        // Ajustement du player au dessus de la plateforme au centre de l'�cran
        transformPlayer.position = positionHautColliderPlateforme;
        transformPlayer.GetComponent<ExtremiteBoxCollider2D>().DecalagePositionToUpCollider2D();
    }

    /// <summary>
    /// R�cup�re la plateforme activ� la plus proche en Y du transform donn�.
    /// </summary>
    /// <param name="transformContourCarte">Transform du centre de la Map</param>
    /// <returns>Transform de la plateforme la plus proche (en Y) du transform donn�e.</returns>
    private GameObject FindPlateformeAtCenter(Transform transformContourCarte)
    {
        // Dans chaque pool de plateforme
        foreach (var pool in _poolPlateforme)
        {
            // Sur chaque plateforme
            foreach (var plateforme in pool)
            {
                if (!plateforme.activeInHierarchy)
                    continue; // Plateforme pas activ�e

                if (Mathf.Abs(plateforme.transform.position.y - transformContourCarte.position.y) < _distanceEntrePlateformes)
                { // La plateforme est la plus au centre
                    return plateforme;
                }
            }
        }
        return null;
    }

}
