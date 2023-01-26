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

    public float _vitesseCarte;

    //float timeSinceLastCall = 0f; // variable pour stocker le temps écoulé depuis le dernier appel de la méthode
    //float callInterval = 2f; // intervalle de temps entre chaque appel de la méthode, ici 1 seconde

    // Start is called before the first frame update
    void Start()
    {
        // Set up les pool des plateformes
        CreatePlateformesPool();

        // Récupération de valeur
        _hauteurPlayer = GameObject.Find("Player").GetComponent<BoxCollider2D>().size.y * GameObject.Find("Player").transform.lossyScale.y;
        _distanceEntrePlateformes = _ratioHauteurPlayer * _hauteurPlayer;

        _xMinEcran = GameObject.Find("ContourCarte").GetComponent<ExtremiteBoxCollider2D>().GetPositionLeftCollider2D().x;
        _xMaxEcran = GameObject.Find("ContourCarte").GetComponent<ExtremiteBoxCollider2D>().GetPositionRightCollider2D().x;

        // Génération de la carte
        GeneratePlateformeStart();

        // Set Player début game
        SetCenterPlayerPlateformeAtStart();
    }



    /// <summary>
    /// Génère une plateforme pour la continuité de la carte (ou à la position indiquée en paramètre).
    /// Si le paramètre est non défini, alors il est null et un nouvelle position est calculée pour l'activation de la plateforme.
    /// <paramref name="positionGeneration">Vector3 qui indique la position où l'on souhaite générer la plateforme.</param>
    /// </summary>
    public void GeneratePlateforme(Vector3? positionGeneration = null)
    {

        // Choix de la plateforme
        GameObject plateforme = ChoisirPlateformeInPools();

        // Récupération de la position d'activation
        if (positionGeneration == null)
            positionGeneration = FindPostionActivation(plateforme);

        // Random x => fonction je sais plus quoi pour une bonne répartition
        //positionGeneration.x = Random.Range(xMinEcran, xMaxEcran);

        ActivationPlateformeFromPosition(plateforme, (Vector3)positionGeneration);

        _lastPlateforme = plateforme; // Garder en mémoire la dernière plateforme activée
    }

    /// <summary>
    /// Récupère une plateforme disponible dans les pools.
    /// </summary>
    /// <returns>Plateforme disponible.</returns>
    /// <exception cref="System.Exception">Pas de plateformes disponibles.</exception>
    private GameObject ChoisirPlateformeInPools()
    {
        int choixPlateformes; int nbPoolTest = 0;

        while (nbPoolTest < NB_PLATEFORMES)
        {
            // Choix d'un type de plateforme
            choixPlateformes = Random.Range(1, NB_PLATEFORMES);
            var plateforme = ChoisirPlateformeInPool(choixPlateformes);

            if (plateforme != null)
            {
                return plateforme;
            }

            Debug.LogWarning($"Plus de plateformes de type {choixPlateformes} disponibles.");
            ++nbPoolTest;
        }
        throw new System.Exception("Aucune plateformes disponibles dans les pool.");
    }

    /// <summary>
    /// Récupère une plateforme disponible dans un pool spécifique.
    /// </summary>
    /// <param name="poolIndice">Indice du pool choisi.</param>
    /// <returns>Une plateforme pas activé, ou null.</returns>
    private GameObject ChoisirPlateformeInPool(int poolIndice)
    {
        // Pour toute les plateformes d'un certain type, dans un pool
        foreach (GameObject p in _poolPlateforme[poolIndice])
        {
            // Check si la plateforme est déjà active
            if (!p.activeInHierarchy)
            { // Plateforme disponible
                return p;
            }
        }
        Debug.LogWarning($"Plus de plateformes de type {poolIndice} disponibles.");
        return null;
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
        extremiteCollider.DecalagePositionToDownCollider2D();

        RecentrerPlateforme(extremiteCollider);
    }

    /// <summary>
    /// Recentre les plateformes pour qu'elles soient toujours dans la carte et ne débordent pas.
    /// </summary>
    /// <param name="extremiteCollider">Script qui de checker les positions des extremités de la plateforme.</param>
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
    /// Calcul la position où activer la prochaine plateforme.
    /// En y : l'extremité basse du collider de la précedente plateforme.
    /// En x : une valeur aléatoire entre les bornes min et max de la carte.
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
            // TODO : fonction qui utilise un aléatoire suivant une génération bien
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
                GameObject obj = Instantiate(PlateformesPrefabs[p], transform);
                obj.name = obj.name + k.ToString();
                obj.GetComponent<MovePlateforme>().Speed = _vitesseCarte;
                obj.SetActive(false);
                _poolPlateforme[p][k] = obj;
            }
        }
    }

    /// <summary>
    /// Génération des plateformes sur toutes la hauteur de la carte au start.
    /// </summary>
    private void GeneratePlateformeStart()
    {
        const string nameObjectHautEcran = "ZoneHautEcran";
        const string nameObjectBasEcran = "ZoneBasEcran";

        var positionZoneHautEcran = GameObject.Find(nameObjectHautEcran).GetComponent<Transform>().position;
        var positionZoneBasEcran = GameObject.Find(nameObjectBasEcran).GetComponent<Transform>().position;

        float hauteurCarte = positionZoneHautEcran.y - positionZoneBasEcran.y;
        float nbPlateformesEcran = hauteurCarte / _distanceEntrePlateformes;

        // Activation de la première plateforme en haut de l'écran
        GeneratePlateforme(positionGeneration: positionZoneHautEcran);

        Vector3 posActivativation = Vector3.zero;

        // Activation des autres plateformes pour remplir la hauteur de l'écran
        for (int i = 0; i < nbPlateformesEcran; i++)
        {
            GeneratePlateforme();
        }
    }

    /// <summary>
    /// Mise en place d'une plateforme simple au centre de l'écran avec le Player dessus.
    /// </summary>
    private void SetCenterPlayerPlateformeAtStart()
    {
        var transformPlayer = GameObject.Find("Player").transform;
        var transformContourCarte = GameObject.Find("ContourCarte").transform;

        //var listPlateforme = _poolPlateforme[0]
        GameObject plateformeCenter = FindPlateformeAtCenter(transformContourCarte);
        if (plateformeCenter == null)
        {
            Debug.LogWarning("Pas de plateforme au centre de l'écran trouvée");
        }

        // Re-centrage de la plateforme au centre de l'écran sur les x.
        float xCenter = (_xMaxEcran + _xMinEcran) / 2;
        var centerPlateformPosition = new Vector3(xCenter, plateformeCenter.transform.position.y, plateformeCenter.transform.position.z);

        // Changement par une plateforme simple
        plateformeCenter.SetActive(false); // Desactivation de l'ancienne
        plateformeCenter = ChoisirPlateformeInPool(2);  // Récupéraction d'un plateforme simple disponible
        ActivationPlateformeFromPosition(plateformeCenter, centerPlateformPosition); // Activation de la plateforme simple

        // Récupération du haut de la plateforme
        Vector3 positionHautColliderPlateforme = plateformeCenter.GetComponent<ExtremiteBoxCollider2D>().GetPositionUpCollider2D();

        // Ajustement du player au dessus de la plateforme au centre de l'écran
        transformPlayer.position = positionHautColliderPlateforme;
        transformPlayer.GetComponent<ExtremiteBoxCollider2D>().DecalagePositionToUpCollider2D();
    }

    /// <summary>
    /// Récupère la plateforme activé la plus proche en Y du transform donné.
    /// </summary>
    /// <param name="transformContourCarte">Transform du centre de la Map</param>
    /// <returns>Transform de la plateforme la plus proche (en Y) du transform donnée.</returns>
    private GameObject FindPlateformeAtCenter(Transform transformContourCarte)
    {
        // Dans chaque pool de plateforme
        foreach (var pool in _poolPlateforme)
        {
            // Sur chaque plateforme
            foreach (var plateforme in pool)
            {
                if (!plateforme.activeInHierarchy)
                    continue; // Plateforme pas activée

                if (Mathf.Abs(plateforme.transform.position.y - transformContourCarte.position.y) < _distanceEntrePlateformes)
                { // La plateforme est la plus au centre
                    return plateforme;
                }
            }
        }
        return null;
    }

}
