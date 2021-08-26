using UnityEngine;

public class PokemonCollectionMenu : MonoBehaviour
{
    public AppController appController;

    public void Start()
    {
        appController = GameObject.Find("AppController").GetComponent<AppController>();
    }

    public void CollectionType(int collectionType)
    {
        appController.collectionType = collectionType;

        switch (collectionType)
        {
            case 1: appController.collectionName = "Caught Normal Pokemon"; break;
            case 2: appController.collectionName = "Caught Shiny Pokemon"; break;
            case 3: appController.collectionName = "Caught Shiny Pokemon"; break;
            default: appController.collectionName = "Caught Normal Pokemon"; break;
        }
    }
    
}
