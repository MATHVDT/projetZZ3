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
    public GameObject _lastPlateforme = null;


    public float _hauteurPlayer = 0;
    private float _hauteurMaxPlateforme = 0;
    private float _ratioHauteurPlayer = 1.0f;
    private float _ratioHauteurMaxPlateforme = 0.8f;


    public float _xMinEcran;
    public float _xMaxEcran;



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

        _hauteurPlayer = 1.1f * GameObject.Find("Player").GetComponent<BoxCollider2D>().size.y * GameObject.Find("Player").transform.lossyScale.y;

        _xMinEcran = GameObject.Find("ContourCarte").GetComponent<ExtremiteBoxCollider2D>().GetPositionLeftCollider2D().x;
        _xMaxEcran = GameObject.Find("ContourCarte").GetComponent<ExtremiteBoxCollider2D>().GetPositionRightCollider2D().x;

        GeneratePlateformeStart();
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

    /// <summary>
    /// Génère une plateforme pour la continuité de la carte.
    /// </summary>
    public void GeneratePlateforme()
    {

        // Choix de la plateforme
        GameObject plateforme = ChoisirPlateformeInPool();

        // Récupération de la position d'activation
        Vector3 positionGeneration = FindPostionActivation(plateforme);

        // Random x => fonction je sais plus quoi pour une bonne répartition
        //positionGeneration.x = Random.Range(xMinEcran, xMaxEcran);

        ActivationPlateformeFromPosition(plateforme, positionGeneration);


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
            choixPlateformes = Random.Range(1, NB_PLATEFORMES);

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
    private void ActivationPlateformeFromPosition(GameObject plateforme, Vector3 positionActivation)
    {
        if (plateforme.activeSelf) // Check s'il est bien desactivé
            Debug.LogWarning($"Le GameObject {plateforme.name} est déjà activée et est situé à la position {plateforme.transform.position}.");

        var extremiteCollider = plateforme.GetComponent<ExtremiteBoxCollider2D>();

        plateforme.transform.position = positionActivation;
        plateforme.SetActive(true);
        extremiteCollider.DecalagePositionToUpCollider2D();

        int i = 0;
        while (i < 100 && extremiteCollider.GetPositionLeftCollider2D().x < _xMinEcran)
        {
            //Debug.Log($"{plateforme.name} {extremiteCollider.GetPositionLeftCollider2D().x} < {_xMinEcran} et i:{i++}");
            extremiteCollider.DecalagePositionToRightCollider2D();
        }
        i = 0;
        while (i < 100 && extremiteCollider.GetPositionRightCollider2D().x > _xMaxEcran)
        {
            //Debug.Log($"{plateforme.name} {extremiteCollider.GetPositionRightCollider2D().x} > {_xMaxEcran} et i:{i++}");
            extremiteCollider.DecalagePositionToLeftCollider2D();
        }

        //if (extremiteCollider.GetPositionRightCollider2D().x > _xMaxEcran)
        //{
        //    extremiteCollider.DecalageLeftCollider2D();
        //}

        _lastPlateforme = plateforme;
    }


    private Vector3 FindPostionActivation(GameObject plateformeAActiver)
    {
        Vector3 positionActivation = Vector3.zero;

        if (_lastPlateforme != null)
        {
            // Find Y position
            Vector3 extremiteBasse = _lastPlateforme.GetComponent<ExtremiteBoxCollider2D>().GetPositionDownCollider2D();
            positionActivation = extremiteBasse + _hauteurPlayer * Vector3.down;

            // Find X position
            float x = Random.Range(_xMinEcran, _xMaxEcran);
            Debug.Log($"val rand x : {x}");
            positionActivation += x * Vector3.right;
        }
        return positionActivation;
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
                obj.name = obj.name + k.ToString();
                obj.GetComponent<MovePlateforme>().Speed = _vitesseCarte;
                obj.SetActive(false);
                _poolPlateforme[p][k] = obj;
            }
        }

        // Récupération de la hauteur Max d'un plateforme
        foreach (var p in PlateformesPrefabs)
        {
            float hauteur = p.GetComponent<SpriteRenderer>().bounds.size.y;
            _hauteurMaxPlateforme = Mathf.Max(_hauteurMaxPlateforme, hauteur);
        }
    }


    private void GeneratePlateformeStart()
    {
        const string nameObjectLimiteHauteEcran = "ZoneHautEcran";
        Vector3 posActivativation = DefaultPositionGeneration;


        var collider = GameObject.Find(nameObjectLimiteHauteEcran).GetComponent<BoxCollider2D>();

        float objectHeight = collider.size.y;        // récupération de la hauteur de l'objet

        // Récupération de la position haute du contour de l'objet
        float yMaxEcran = (collider.transform.position.y + collider.offset.y + (objectHeight / 2));

        float h_y = yMaxEcran - DefaultPositionGeneration.y;
        float hauteurPlateformeSpace = _ratioHauteurMaxPlateforme * _hauteurMaxPlateforme + _ratioHauteurPlayer * _hauteurPlayer;
        //float nBPlateformes = h_y / hauteurPlateformeSpace;
        float nBPlateformes = h_y / _hauteurPlayer;


        ActivationPlateformeFromPosition(ChoisirPlateformeInPool(), new Vector3(0, 4.5f, 0));

        for (int i = 0; i < 10; i++)
        {
            var g = ChoisirPlateformeInPool();
            posActivativation = FindPostionActivation(g);
            ActivationPlateformeFromPosition(g, posActivativation);
        }

    }
}
