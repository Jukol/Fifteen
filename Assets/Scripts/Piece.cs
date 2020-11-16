using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IPointerDownHandler
{

    Vector3 newPosition;
    Vector3 newPos;
    RaycastHit hitInfo;
    [SerializeField]
    GameManager gm;
    bool canPlayAudio = true;
    bool canMove;
    public bool CanMove { get; set; }

    private void FixedUpdate()
    {
       
        if (canMove && gm.StartButtonPressed)
        {
            if (canPlayAudio && canMove)
                gm.PlaySound();
            
            transform.position = Vector3.MoveTowards(transform.position, newPos, 20f);
            canPlayAudio = false;
            if (transform.position == newPos) { }
                gm.CheckForWin();
        }
            
        if (transform.position == newPos)
        {
            canMove = false;
            canPlayAudio = true;
        }
            
    }

    Vector3 CalculateNewPosition()
    {
        if (Physics.Raycast(transform.position, Vector3.up, out hitInfo, 150f) && hitInfo.collider.tag == "Cell" && gm.StartButtonPressed)
        {
            newPosition = hitInfo.collider.transform.position;
            canMove = true;
        }
            
        else if (Physics.Raycast(transform.position, Vector3.right, out hitInfo, 150f) && hitInfo.collider.tag == "Cell" && gm.StartButtonPressed)
        {
            newPosition = hitInfo.collider.transform.position;
            canMove = true;
        }
        else if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 150f) && hitInfo.collider.tag == "Cell" && gm.StartButtonPressed)
        {
            newPosition = hitInfo.collider.transform.position;
            canMove = true;
        }
        else if (Physics.Raycast(transform.position, Vector3.left, out hitInfo, 150f) && hitInfo.collider.tag == "Cell" && gm.StartButtonPressed)
        {
            newPosition = hitInfo.collider.transform.position;
            canMove = true;
        }
        else
        {
            newPosition = transform.position;
            canMove = false;
        }
            
        return newPosition;
    }

    
    public void OnPointerDown(PointerEventData eventData)
    { 
        newPos = CalculateNewPosition();
    }
}
