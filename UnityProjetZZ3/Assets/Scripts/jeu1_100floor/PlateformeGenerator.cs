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

    float timeSinceLastCall = 0f; // variable pour stocker le temps �coul� depuis le dernier appel de la m�thode
    float callInterval = 2f; // intervalle de temps entre chaque appel de la m�thode, ici 1 seconde

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

        timeSinceLastCall += Time.deltaTime; // ajoute le temps �coul� depuis le dernier frame � la variable timeSinceLastCall

        if (timeSinceLastCall >= callInterval) // si le temps �coul� est sup�rieur ou �gal � l'intervalle de temps d�fini
        {
            timeSinceLastCall -= callInterval; // soustraction de l'intervalle de temps au temps �coul� pour maintenir la synchronisation
                                               //GenerateTest(); // appel de la m�thode
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
                GameObject obj = Instantiate(PlateformesPrefabs[p], TargetInstanciation);
                obj.name = obj.name + k.ToString();
                obj.GetComponent<MovePlateforme>().Speed = _vitesseCarte;
                obj.SetActive(false);
                _poolPlateforme[p][k] = obj;
            }
        }

        // R�cup�ration de la hauteur Max d'un plateforme
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

        float objectHeight = collider.size.y;        // r�cup�ration de la hauteur de l'objet

        // R�cup�ration de la position haute du contour de l'objet
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
