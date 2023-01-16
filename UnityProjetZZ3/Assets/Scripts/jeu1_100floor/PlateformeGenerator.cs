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

    public float _hauteurPlayer;

    public float xMinEcran;
    public float xMaxEcran;

    private ContourCarte _contourCarte;

    public Vector3 DefaultPositionGeneration;


    public bool Button_generate;

    public Transform TargetInstanciation;
    public float _vitesseCarte;

    float timeSinceLastCall = 0f; // variable pour stocker le temps écoulé depuis le dernier appel de la méthode
    float callInterval = 2f; // intervalle de temps entre chaque appel de la méthode, ici 1 seconde

    // Start is called before the first frame update
    void Start()
    {
        // Set up les pool des plateformes
        CreatePlateformesPool();

        DefaultPositionGeneration = transform.position;

        _hauteurPlayer = GameObject.Find("Player").GetComponent<SpriteRenderer>().bounds.size.y;
        //Debug.Log("hauteur player : " + _hauteurPlayer);

        _contourCarte = gameObject.GetComponentInChildren<ContourCarte>();
    }

    // Update is called once per frame
    void Update()
    {

        timeSinceLastCall += Time.deltaTime; // ajoute le temps écoulé depuis le dernier frame à la variable timeSinceLastCall

        if (timeSinceLastCall >= callInterval) // si le temps écoulé est supérieur ou égal à l'intervalle de temps défini
        {
            timeSinceLastCall -= callInterval; // soustraction de l'intervalle de temps au temps écoulé pour maintenir la synchronisation
                                               //GenerateTest(); // appel de la méthode
                                               //GeneratePlateforme();
        }
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
        targetPosition.x = Random.Range(xMinEcran, xMaxEcran);

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

    /// <summary>
    /// Génère une plateforme pour la continuité de la carte.
    /// </summary>
    public void GeneratePlateforme()
    {
        Vector3 positionGeneration = DefaultPositionGeneration;

        // Random x => fonction je sais plus quoi pour une bonne répartition
        positionGeneration.x = Random.Range(xMinEcran, xMaxEcran);

        // Le choix du type de plateforme doit dépendre de la fonction je sais plus quoi
        ActiveFromPosition(ChoisirPlateformeInPool(), positionGeneration);
    }

    /// <summary>
    /// Récupère une plateforme disponible dans les pool.
    /// </summary>
    /// <returns>Plateforme disponible.</returns>
    /// <exception cref="System.Exception">Pas de plateformes disponibles.</exception>
    private GameObject ChoisirPlateformeInPool()
    {
        int choixPlateformes; int nbPoolTest = 0;

        while (nbPoolTest < NB_PLATEFORMES)
        {
            // Choix d'un type de plateforme
            choixPlateformes = Random.Range(0, NB_PLATEFORMES);

            // Pour toute les plateformes d'un certain type, dans un pool
            foreach (GameObject p in _poolPlateforme[choixPlateformes])
            {
                // Check si la plateforme est déjà active
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
    /// Active un GameObject sur une position donnée. 
    /// </summary>
    /// <param name="plateforme">Plateforme desactivée.</param>
    /// <param name="positionActivation">Position de l'activation de la plateforme.</param>
    private void ActiveFromPosition(GameObject plateforme, Vector3 positionActivation)
    {
        if (plateforme.activeSelf) // Check s'il est bien desactivé
            Debug.LogWarning($"Le GameObject {plateforme.name} est déjà activée et est situé à la position {plateforme.transform.position}.");

        plateforme.transform.position = positionActivation;
        plateforme.SetActive(true);
    }

    /// <summary>
    /// Construit les pool des différentes plateformes. (Instatiation des différents éléments des pool)
    /// </summary>
    private void CreatePlateformesPool()
    {
        // Création des différents pools pour les plateformes
        _poolPlateforme = new GameObject[NB_PLATEFORMES][];

        // Instiation des pools des différentes type de plateformes
        for (int p = 0; p < NB_PLATEFORMES; ++p)
        {
            _poolPlateforme[p] = new GameObject[POOL_SIZE];

            // Instantiation de toutes les plateformes d'un type
            for (int k = 0; k < POOL_SIZE; ++k)
            {
                GameObject obj = Instantiate(PlateformesPrefabs[p], TargetInstanciation);
                obj.GetComponent<MovePlateforme>().Speed = _vitesseCarte;
                obj.SetActive(false);
                _poolPlateforme[p][k] = obj;
            }
        }
    }


    private void GeneratePlateformeStart()
    {
        Vector3 posActivativation = DefaultPositionGeneration;




    }
}
