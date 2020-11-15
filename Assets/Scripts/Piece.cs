using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IPointerDownHandler
{

    bool canMove;
    Vector3 newPosition;
    Vector3 newPos;
    RaycastHit hitInfo;
    [SerializeField]
    GameManager gm;

    private void Update()
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, 10f);
            if (transform.position == newPos)
                gm.CheckForWin();
        }
            
        if (transform.position == newPos)
            canMove = false;
    }

    Vector3 CalculateNewPosition()
    {
        if (Physics.Raycast(transform.position, Vector3.up, out hitInfo, 70f) && hitInfo.collider.tag == "Cell")
            newPosition = hitInfo.collider.transform.position;
        else if (Physics.Raycast(transform.position, Vector3.right, out hitInfo, 70f) && hitInfo.collider.tag == "Cell")
            newPosition = hitInfo.collider.transform.position;
        else if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 70f) && hitInfo.collider.tag == "Cell")
            newPosition = hitInfo.collider.transform.position;
        else if (Physics.Raycast(transform.position, Vector3.left, out hitInfo, 70f) && hitInfo.collider.tag == "Cell")
            newPosition = hitInfo.collider.transform.position;
        else
            newPosition = transform.position;
        return newPosition;
    }

    
    public void OnPointerDown(PointerEventData eventData)
    {
        canMove = true;
        newPos = CalculateNewPosition();
    }

}
