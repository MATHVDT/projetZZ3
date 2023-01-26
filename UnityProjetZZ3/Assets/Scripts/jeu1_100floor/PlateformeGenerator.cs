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

        // Set Player d�but game
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

        // Random x => fonction je sais plus quoi pour une bonne r�partition
        //positionGeneration.x = Random.Range(xMinEcran, xMaxEcran);

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
    /// R�cup�re une plateforme disponible dans un pool sp�cifique.
    /// </summary>
    /// <param name="poolIndice">Indice du pool choisi.</param>
    /// <returns>Une plateforme pas activ�, ou null.</returns>
    private GameObject ChoisirPlateformeInPool(int poolIndice)
    {
        // Pour toute les plateformes d'un certain type, dans un pool
        foreach (GameObject p in _poolPlateforme[poolIndice])
        {
            // Check si la plateforme est d�j� active
            if (!p.activeInHierarchy)
            { // Plateforme disponible
                return p;
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
        if (plateforme.activeSelf) // Check s'il est bien desactiv�
            Debug.LogWarning($"Le GameObject {plateforme.name} est d�j� activ�e et est situ� � la position {plateforme.transform.position}.");

        var extremiteCollider = plateforme.GetComponent<ExtremiteBoxCollider2D>();

        plateforme.transform.position = positionActivation;
        plateforme.SetActive(true);
        extremiteCollider.DecalagePositionToDownCollider2D();

        RecentrerPlateforme(extremiteCollider);
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
                GameObject obj = Instantiate(PlateformesPrefabs[p], transform);
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
        GeneratePlateforme(positionGeneration: positionZoneHautEcran);

        Vector3 posActivativation = Vector3.zero;

        // Activation des autres plateformes pour remplir la hauteur de l'�cran
        for (int i = 0; i < nbPlateformesEcran; i++)
        {
            GeneratePlateforme();
        }
    }

    /// <summary>
    /// Mise en place d'une plateforme simple au centre de l'�cran avec le Player dessus.
    /// </summary>
    private void SetCenterPlayerPlateformeAtStart()
    {
        var transformPlayer = GameObject.Find("Player").transform;
        var transformContourCarte = GameObject.Find("ContourCarte").transform;

        //var listPlateforme = _poolPlateforme[0]
        GameObject plateformeCenter = FindPlateformeAtCenter(transformContourCarte);
        if (plateformeCenter == null)
        {
            Debug.LogWarning("Pas de plateforme au centre de l'�cran trouv�e");
        }

        // Re-centrage de la plateforme au centre de l'�cran sur les x.
        float xCenter = (_xMaxEcran + _xMinEcran) / 2;
        var centerPlateformPosition = new Vector3(xCenter, plateformeCenter.transform.position.y, plateformeCenter.transform.position.z);

        // Changement par une plateforme simple
        plateformeCenter.SetActive(false); // Desactivation de l'ancienne
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
