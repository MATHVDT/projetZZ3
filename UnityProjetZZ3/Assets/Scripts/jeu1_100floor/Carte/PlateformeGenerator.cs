using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// Script gérant la génération des plateformes en continue.
/// Instanciation de toutes les plateformes et gestion avec des pools d'objects.
/// Mise en place des plateformes sur tout l'écran au lancement du jeu.
/// </summary>
public class PlateformeGenerator : MonoBehaviour
{
    // Paramètres des pools de plateformes
    const int NB_PLATEFORMES = 5;
    const int POOL_SIZE = 10;

    // Prefab des plateformes stockées dedans (directement depuis l'inspector)
    public GameObject[] PlateformesPrefabs = new GameObject[NB_PLATEFORMES];

    private GameObject[][] _poolPlateforme; // Pools de plateformes
    private GameObject _lastPlateforme = null; // Référence sur la dernière plateforme activée

    // Variable de calculs
    private float _hauteurPlayer = 0;
    private float _distanceEntrePlateformes = 0;
    private float _ratioHauteurPlayer = 1.0f;

    // Bornes des x pour l'activation des plateformes
    private float _xMinEcran;
    private float _xMaxEcran;

    // La vitesse doit dépendre de la taille de l'écran
    // Multiplier par Jeu.transform.scale
    public float VitesseCarte; // Vitesse de montée des plateformes

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

        // Set Player au centre et sur une plateforme simple au début de la game
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

        // Activation de la plateforme à la position définie
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
        List<int> poolDispo = new List<int> { 0, 1, 2, 3, 4 }; // 5 types de plateformes

        // Tant que l'on a pas tester tous les pools de plateformes
        while (nbPoolTest < NB_PLATEFORMES)
        {
            // Choix d'un type de plateforme
            choixPlateformes = poolDispo[Random.Range(0, poolDispo.Count)];
            //choixPlateformes = 1; // TODO a changer pour forcer le spwan de plateforme que d'un type

            // Récupération d'une plateforme disponible dans le pool choisi (choixPlateformes)
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
    /// Récupère une plateforme disponible dans un pool spécifique.
    /// </summary>
    /// <param name="poolIndice">Indice du pool choisi.</param>
    /// <returns>Une plateforme pas activé, ou null.</returns>
    private GameObject ChoisirPlateformeInPool(int poolIndice)
    {
        // Pour toute les plateformes d'un certain type, dans un pool
        foreach (GameObject p in _poolPlateforme[poolIndice])
        {
            // Check si la plateforme est déjà activée
            if (!p.activeInHierarchy)
            {
                return p; // Plateforme disponible
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
        if (plateforme.activeSelf) // Check s'elle est bien desactivée
            Debug.LogWarning($"Le GameObject {plateforme.name} est déjà activée et est situé à la position {plateforme.transform.position}.");

        var extremiteCollider = plateforme.GetComponent<ExtremiteBoxCollider2D>();
        //var extremiteSpriteRenderer = plateforme.GetComponent<ExtremiteSpriteRenderer>();

        plateforme.transform.position = positionActivation;
        plateforme.SetActive(true);
        // Re set la vitesse des plateformes (normalement pas utile)
        plateforme.GetComponent<MovePlateforme>().Speed = VitesseCarte;

        // Décalage plateforme sur les Y pour qu'avec la plateforme précédente,
        // la distance entre les 2 soient bien _distanceEntrePlateformes (en tenant compte des BoxCollider)
        extremiteCollider.DecalagePositionToDownCollider2D();

        // Recentre la plateforme pour qu'elle ne sorte pas de l'écran
        RecentrerPlateforme(extremiteCollider);
    }

    /// <summary>
    /// Recentre les plateformes pour qu'elles soient toujours dans la carte et ne débordent pas.
    /// </summary>
    /// <param name="extremiteCollider">Script qui de checker les positions des extremités de la plateforme.</param>
    private void RecentrerPlateforme(ExtremiteBoxCollider2D extremiteCollider)
    {
        int varDeSecu = 0; // Sert à vérifier que l'on reste pas dans la boucle

        // Tant que le coté gauche du collider est en dehors de la map, on décale la plateforme vers la droite
        while (varDeSecu < 100 && extremiteCollider.GetPositionLeftCollider2D().x < _xMinEcran)
        {
            //Debug.Log($"{plateforme.name} {extremiteCollider.GetPositionLeftCollider2D().x} < {_xMinEcran} et varDeSecu:{varDeSecu}");
            ++varDeSecu;
            extremiteCollider.DecalagePositionToRightCollider2D();
        }

        varDeSecu = 0;
        // Tant que le coté droit du collider est en dehors de la map, on décale la plateforme vers la gauche
        while (varDeSecu < 100 && extremiteCollider.GetPositionRightCollider2D().x > _xMaxEcran)
        {
            //Debug.Log($"{plateforme.name} {extremiteCollider.GetPositionRightCollider2D().x} > {_xMaxEcran} et varDeSecu:{varDeSecu}");
            ++varDeSecu;
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
            _poolPlateforme[p] = new GameObject[POOL_SIZE]; // Instanciation du pool de plateformes vide

            // Instantiation de toutes les plateformes d'un type
            for (int k = 0; k < POOL_SIZE; ++k)
            {
                GameObject obj = Instantiate(PlateformesPrefabs[p], transform); // Instanciation de l'objet plateforme
                obj.name = obj.name + k.ToString(); // Rennomage de la plateforme
                obj.GetComponent<MovePlateforme>().Speed = VitesseCarte; // Parametrage de la vitesse 
                obj.SetActive(false); // Désactivation de la plateforme
                _poolPlateforme[p][k] = obj; // Ajout de la plateforme à son pool
            }
        }
    }


    /***************************** METHODE START ************************************/

    /// <summary>
    /// Génération des plateformes sur toutes la hauteur de la carte au start.
    /// </summary>
    private void GeneratePlateformeStart()
    {
        const string nameObjectHautEcran = "ZoneHautEcran";
        const string nameObjectBasEcran = "ZoneBasEcran";

        // Récupération des extremités verticales de la carte
        var positionZoneHautEcran = GameObject.Find(nameObjectHautEcran).GetComponent<Transform>().position;
        var positionZoneBasEcran = GameObject.Find(nameObjectBasEcran).GetComponent<Transform>().position;

        // Calcul de nombre de plateformes nécessaire pour remplir l'écran
        float hauteurCarte = positionZoneHautEcran.y - positionZoneBasEcran.y;
        float nbPlateformesEcran = hauteurCarte / 1.5f / _distanceEntrePlateformes;

        // Activation de la première plateforme en haut de l'écran
        GeneratePlateforme(positionGeneration: positionZoneHautEcran);

        // Activation des autres plateformes pour remplir la hauteur de l'écran
        for (int i = 0; i < nbPlateformesEcran; i++)
        { GeneratePlateforme(); }
    }

    /// <summary>
    /// Mise en place d'une plateforme simple au centre de l'écran avec le Player dessus.
    /// </summary>
    private void SetCenterPlayerPlateformeAtStart()
    {
        var transformPlayer = GameObject.Find("Player").transform;
        var transformContourCarte = GameObject.Find("ContourCarte").transform;

        GameObject plateformeCenter = FindPlateformeAtCenter(transformContourCarte);
        if (plateformeCenter == null)
        { Debug.LogWarning("Pas de plateforme au centre de l'écran trouvée"); }

        // Re-centrage de la plateforme au centre de l'écran sur les x.
        float xCenter = (_xMaxEcran + _xMinEcran) / 2;
        var centerPlateformPosition = new Vector3(xCenter, plateformeCenter.transform.position.y, plateformeCenter.transform.position.z);

        // Changement par une plateforme simple
        plateformeCenter.SetActive(false); // Desactivation de l'ancienne plateforme
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
