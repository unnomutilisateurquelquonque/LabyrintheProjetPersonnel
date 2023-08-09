using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatonSecret : MonoBehaviour
{
    public GameObject chatonSecret;
    private bool active;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Joueur") && active == false)
        {
            Chaton();
        }
    }
    
    // Update is called once per frame
    void Chaton()
    {
        active = true;
        // la position
        float positionX = chatonSecret.transform.position.x;
        float positionY = chatonSecret.transform.position.y;
        float positionZ = chatonSecret.transform.position.z;
        //déplacement
        Vector3 positionViser = new Vector3(positionX+6.5f,positionY,positionZ);
        // fortement inspiré de https://gamedevbeginner.com/the-right-way-to-lerp-in-unity-with-examples/
        StartCoroutine(LerpPosition(positionViser, 5));
    }


    IEnumerator LerpPosition(Vector3 positionAViser, float duration)
    {
        float temp = 0;
        Vector3 positionInitial = chatonSecret.transform.position;
        while (temp < duration)
        {
            chatonSecret.transform.position = Vector3.Lerp(positionInitial,positionAViser, temp / duration);
            temp += Time.deltaTime;
            yield return null;
        }
        chatonSecret.transform.position = positionAViser;
    }
}
