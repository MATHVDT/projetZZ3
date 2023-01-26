using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateformeGenerator : MonoBehaviour
{
    const int NB_PLATEFORMES = 5;
    const int POOL_SIZE = 10;

    public GameObject[] PlateformesPrefabs = new GameObject[NB_PLATEFORMES];

    private GameObject[][] _poolPlateforme;
    private GameObject _lastPlateforme = null;

    private float _hauteurPlayer = 0;
    private float _distanceEntrePlateformes = 0;
    public float _ratioHauteurPlayer = 1.0f;

    private float _xMinEcran;
    private float _xMaxEcran;


    public bool Button_generate;

    public float _vitesseCarte;

    //float timeSinceLastCall = 0f; // variable pour stocker le temps �coul� depuis le dernier appel de la m�thode
    //float callInterval = 2f; // intervalle de temps entre chaque appel de la m�thode, ici 1 seconde

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
    }

    /*
    void Update()
    {

        //timeSinceLastCall += Time.deltaTime; // ajoute le temps �coul� depuis le dernier frame � la variable timeSinceLastCall

        //if (timeSinceLastCall >= callInterval) // si le temps �coul� est sup�rieur ou �gal � l'intervalle de temps d�fini
        //{
        //    timeSinceLastCall -= callInterval; // soustraction de l'intervalle de temps au temps �coul� pour maintenir la synchronisation
        //                                       //GenerateTest(); // appel de la m�thode
        //                                       //GeneratePlateforme();
        //}
    }
    private void FixedUpdate()
    {
        if (Button_generate)
        { // Btn de test pour generer une plateforme
            GeneratePlateforme();
            Button_generate = false;
        }
    }
    public void GenerateTest()
    {
        int choixPlateformes = Random.Range(0, NB_PLATEFORMES);
        Vector3 targetPosition = TargetInstanciation.transform.position;
        targetPosition.x = Random.Range(_xMinEcran, _xMaxEcran);

        bool objAvailable = false;

        for (int k = 0; k < POOL_SIZE; ++k)
        {
            GameObject obj = _poolPlateforme[choixPlateformes][k];
            if (!obj.activeInHierarchy)
            {
                objAvailable = true;
                obj.transform.position = targetPosition;
                obj.SetActive(true);
                break;
            }
        }

        if (!objAvailable)
        {
            Debug.Log($"Manque de plateforme type {choixPlateformes}.");
        }
    }
    */

    /// <summary>
    /// G�n�re une plateforme pour la continuit� de la carte.
    /// </summary>
    public void GeneratePlateforme()
    {

        // Choix de la plateforme
        GameObject plateforme = ChoisirPlateformeInPool();

        // R�cup�ration de la position d'activation
        Vector3 positionGeneration = FindPostionActivation(plateforme);

        // Random x => fonction je sais plus quoi pour une bonne r�partition
        //positionGeneration.x = Random.Range(xMinEcran, xMaxEcran);

        ActivationPlateformeFromPosition(plateforme, positionGeneration);


    }

    /// <summary>
    /// R�cup�re une plateforme disponible dans les pool.
    /// </summary>
    /// <returns>Plateforme disponible.</returns>
    /// <exception cref="System.Exception">Pas de plateformes disponibles.</exception>
    private GameObject ChoisirPlateformeInPool()
    {
        int choixPlateformes; int nbPoolTest = 0;

        while (nbPoolTest < NB_PLATEFORMES)
        {
            // Choix d'un type de plateforme
            choixPlateformes = Random.Range(1, NB_PLATEFORMES);

            // Pour toute les plateformes d'un certain type, dans un pool
            foreach (GameObject p in _poolPlateforme[choixPlateformes])
            {
                // Check si la plateforme est d�j� active
                if (!p.activeInHierarchy)
                { // Plateforme disponible
                    return p;
                }
            }

            Debug.LogWarning($"Plus de plateformes de type {choixPlateformes} disponibles.");
            ++nbPoolTest;
        }
        throw new System.Exception("Aucune plateformes disponibles dans les pool.");
    }

    /// <summary>
    /// Active un GameObject sur une position donn�e. 
    /// </summary>
    /// <param name="plateforme">Plateforme desactiv�e.</param>
    /// <param name="positionActivation">Position de l'activation de la plateforme.</param>
    private void ActivationPlateformeFromPosition(GameObject plateforme, Vector3 positionActivation)
    {
        if (plateforme.activeSelf) // Check s'il est bien desactiv�
            Debug.LogWarning($"Le GameObject {plateforme.name} est d�j� activ�e et est situ� � la position {plateforme.transform.position}.");

        var extremiteCollider = plateforme.GetComponent<ExtremiteBoxCollider2D>();

        plateforme.transform.position = positionActivation;
        plateforme.SetActive(true);
        extremiteCollider.DecalagePositionToUpCollider2D();

        RecentrerPlateforme(extremiteCollider);

        _lastPlateforme = plateforme; // Garder en m�moire la derni�re plateforme activ�e
    }

    /// <summary>
    /// Recentre les plateformes pour qu'elles soient toujours dans la carte et ne d�bordent pas.
    /// </summary>
    /// <param name="extremiteCollider">Script qui de checker les positions des extremit�s de la plateforme.</param>
    private void RecentrerPlateforme(ExtremiteBoxCollider2D extremiteCollider)
    {
        int varDeSecu = 0;
        while (varDeSecu < 100 && extremiteCollider.GetPositionLeftCollider2D().x < _xMinEcran)
        {
            ++varDeSecu;
            //Debug.Log($"{plateforme.name} {extremiteCollider.GetPositionLeftCollider2D().x} < {_xMinEcran} et varDeSecu:{varDeSecu}");
            extremiteCollider.DecalagePositionToRightCollider2D();
        }
        varDeSecu = 0;
        while (varDeSecu < 100 && extremiteCollider.GetPositionRightCollider2D().x > _xMaxEcran)
        {
            ++varDeSecu;
            //Debug.Log($"{plateforme.name} {extremiteCollider.GetPositionRightCollider2D().x} > {_xMaxEcran} et varDeSecu:{varDeSecu}");
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
            _poolPlateforme[p] = new GameObject[POOL_SIZE];

            // Instantiation de toutes les plateformes d'un type
            for (int k = 0; k < POOL_SIZE; ++k)
            {
                GameObject obj = Instantiate(PlateformesPrefabs[p]);
                obj.name = obj.name + k.ToString();
                obj.GetComponent<MovePlateforme>().Speed = _vitesseCarte;
                obj.SetActive(false);
                _poolPlateforme[p][k] = obj;
            }
        }
    }

    /// <summary>
    /// G�n�ration des plateformes sur toutes la hauteur de la carte au start.
    /// </summary>
    private void GeneratePlateformeStart()
    {
        const string nameObjectHautEcran = "ZoneHautEcran";
        const string nameObjectBasEcran = "ZoneBasEcran";

        var positionZoneHautEcran = GameObject.Find(nameObjectHautEcran).GetComponent<Transform>().position;
        var positionZoneBasEcran = GameObject.Find(nameObjectBasEcran).GetComponent<Transform>().position;

        float hauteurCarte = positionZoneHautEcran.y - positionZoneBasEcran.y;
        float nbPlateformesEcran = hauteurCarte / _distanceEntrePlateformes;

        // Activation de la premi�re plateforme en haut de l'�cran
        ActivationPlateformeFromPosition(ChoisirPlateformeInPool(), positionZoneHautEcran);

        Vector3 posActivativation = Vector3.zero;

        // Activation des autres plateformes pour remplir la hauteur de l'�cran
        for (int i = 0; i < nbPlateformesEcran; i++)
        {
            var g = ChoisirPlateformeInPool();
            posActivativation = FindPostionActivation(g);
            ActivationPlateformeFromPosition(g, posActivativation);
        }
    }
}
