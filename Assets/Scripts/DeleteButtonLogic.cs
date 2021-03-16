using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButtonLogic : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Card;

    public GetTokensData canvasData;
    public void delete(){
        var placementMetadata = Card.GetComponentInChildren<cardMetadata>(false);
        var spriteAnimation = Card.GetComponentInChildren<cardAnimation>(false);
        canvasData.placedTokens[placementMetadata.slug][placementMetadata.tokenID] = false;
        spriteAnimation.status = "delete";
    }
    void Start()
    {
        gameObject.SetActive(false);
    }

}
