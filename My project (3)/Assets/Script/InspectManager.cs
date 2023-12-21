using System.Collections;
using UnityEngine;

public class InspectManager : MonoBehaviour
{
    public float distance;
    public Transform playerSocket;

    Vector3 originalPos;
    bool onInspect = false;
    GameObject inspected;

    public PlayerController playerScript;

    private void Update()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, fwd, out hit, distance))
        {
            if (hit.transform.tag == "Object" && !onInspect)
            {
                 if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    inspected = hit.transform.gameObject;
                    originalPos = hit.transform.position;
                    onInspect = true;

                    StartCoroutine(pickupItem());
                }
            }                           
        }

        if (onInspect)
        {
            inspected.transform.position = Vector3.Lerp(inspected.transform.position, playerSocket.position, 0.2f);
            playerSocket.Rotate(new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0) * Time.deltaTime * 125f);
        }else if(inspected != null)
        {
            inspected.transform.SetParent(null);
            inspected.transform.position = Vector3.Lerp(inspected.transform.position, originalPos, 0.2f);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && onInspect)
        {
            StartCoroutine(dropItem());
            onInspect = false;
        }
    }

    IEnumerator pickupItem()
    {
        playerScript.enabled = false;
        yield return new WaitForSeconds(0.2f);
        inspected.transform.SetParent(playerSocket);                    
    }

    IEnumerator dropItem()
    {
        inspected.transform.rotation = Quaternion.identity;
        yield return new WaitForSeconds(0.2f);
        playerScript.enabled = true;
    }
}
