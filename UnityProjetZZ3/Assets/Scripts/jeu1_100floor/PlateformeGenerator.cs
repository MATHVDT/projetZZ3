using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateformeGenerator : MonoBehaviour
{
    const int NB_PLATEFORMES = 5;
    const int POOL_SIZE = 20;

    public GameObject[] PlateformesPrefabs = new GameObject[NB_PLATEFORMES];

    private GameObject[][] _poolPlateforme;

    public float xMinEcran;
    public float xMaxEcran;

    public Transform TargetInstanciation;
    public float _vitesseCarte;

    float timeSinceLastCall = 0f; // variable pour stocker le temps �coul� depuis le dernier appel de la m�thode
    float callInterval = 2f; // intervalle de temps entre chaque appel de la m�thode, ici 1 seconde

    // Start is called before the first frame update
    void Start()
    {

        //PoolingPlateforme = new List<GameObject>(GameObject.FindGameObjectsWithTag("Plateforme"));
        //GameObject.FindObjectOfType()

        _poolPlateforme = new GameObject[NB_PLATEFORMES][];

        // Instiation des pools des diff�rentes type de plateformes
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

    // Update is called once per frame
    void Update()
    {
        timeSinceLastCall += Time.deltaTime; // ajoute le temps �coul� depuis le dernier frame � la variable timeSinceLastCall

        if (timeSinceLastCall >= callInterval) // si le temps �coul� est sup�rieur ou �gal � l'intervalle de temps d�fini
        {
            timeSinceLastCall -= callInterval; // soustraction de l'intervalle de temps au temps �coul� pour maintenir la synchronisation
            Generate(); // appel de la m�thode
        }
    }

    public void Generate()
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
}
