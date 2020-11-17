using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IPointerDownHandler
{

    Vector3 newPosition;
    Vector3 newPosition2;
    Vector3 newPosition3;
    RaycastHit hitInfo;
    RaycastHit hitInfo2;
    RaycastHit hitInfo3;
    [SerializeField]
    GameObject pieceUp, pieceRight, pieceDown, pieceLeft, pieceUpUp, pieceRightRight, pieceDownDown, pieceLeftLeft;
    [SerializeField]
    GameManager gm;
    bool canPlayAudio = true;
    bool canMove;
    [SerializeField]
    bool tryMovingInThrees;
    public bool CanMove { get; set; }

    private void FixedUpdate()
    {

        if (canMove && gm.StartButtonPressed)
        {
            if (canPlayAudio && canMove)
                gm.PlaySound();

            transform.position = Vector3.MoveTowards(transform.position, newPosition, 20f);
            
            //FOR DOUBLE MOVES
            if (pieceUp != null)
            {
                pieceUp.transform.position = Vector3.MoveTowards(pieceUp.transform.position, newPosition2, 20f);
                if (pieceUp.transform.position == newPosition2)
                {
                    pieceUp = null;
                }
            }

            if (pieceRight != null)
            {
                pieceRight.transform.position = Vector3.MoveTowards(pieceRight.transform.position, newPosition2, 20f);
                if (pieceRight.transform.position == newPosition2)
                {
                    pieceRight = null;
                }
            }

            if (pieceDown != null)
            {
                pieceDown.transform.position = Vector3.MoveTowards(pieceDown.transform.position, newPosition2, 20f);
                if (pieceDown.transform.position == newPosition2)
                {
                    pieceDown = null;
                }
            }

            if (pieceLeft != null)
            {
                pieceLeft.transform.position = Vector3.MoveTowards(pieceLeft.transform.position, newPosition2, 20f);
                if (pieceLeft.transform.position == newPosition2)
                {
                    pieceLeft = null;
                }
            }
            //FOR DOUBLE MOVES

            //FOR TRIPLE MOVES
            if (pieceUpUp != null)
            {
                pieceUpUp.transform.position = Vector3.MoveTowards(pieceUpUp.transform.position, newPosition3, 20f);
                if (pieceUpUp.transform.position == newPosition3)
                {
                    pieceUpUp = null;
                }
            }

            if (pieceRightRight != null)
            {
                pieceRightRight.transform.position = Vector3.MoveTowards(pieceRightRight.transform.position, newPosition3, 20f);
                if (pieceRightRight.transform.position == newPosition3)
                {
                    pieceRightRight = null;
                }
            }

            if (pieceDownDown != null)
            {
                pieceDownDown.transform.position = Vector3.MoveTowards(pieceDownDown.transform.position, newPosition3, 20f);
                if (pieceDownDown.transform.position == newPosition3)
                {
                    pieceDownDown = null;
                }
            }

            if (pieceLeftLeft != null)
            {
                pieceLeftLeft.transform.position = Vector3.MoveTowards(pieceLeftLeft.transform.position, newPosition3, 20f);
                if (pieceLeftLeft.transform.position == newPosition3)
                {
                    pieceLeftLeft = null;
                }
            }
            //FOR TRIPLE MOVES

            canPlayAudio = false;
            if (transform.position == newPosition) { }
            gm.CheckForWin();
        }

        if (transform.position == newPosition)
        {
            canMove = false;
            canPlayAudio = true;
        }

    }

    void CalculateNewPosition()
    {
        MoveInOnes();

        if (canMove == false && tryMovingInThrees == false)//newPosition == transform.position)
        {
            MoveInTwos();
        }
        
        if (canMove == false && tryMovingInThrees == true)
        {
            MoveInThrees();
            tryMovingInThrees = false;
        }
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        CalculateNewPosition();
    }


    void MoveInOnes()
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
            //newPosition = transform.position;
            canMove = false;
        }
    }

    void MoveInTwos()
    {
        if (Physics.Raycast(transform.position, Vector3.up, out hitInfo, 150f) &&
            hitInfo.collider.tag == "Piece" &&
            Physics.Raycast(hitInfo.collider.transform.position, Vector3.up, out hitInfo2, 150f) && hitInfo2.collider.tag == "Cell" && gm.StartButtonPressed)
        {
            newPosition = hitInfo.collider.transform.position;
            newPosition2 = hitInfo2.collider.transform.position;
            pieceUp = hitInfo.collider.gameObject;
            canMove = true;
        }
        else if (Physics.Raycast(transform.position, Vector3.right, out hitInfo, 150f) &&
            hitInfo.collider.tag == "Piece" &&
            Physics.Raycast(hitInfo.collider.transform.position, Vector3.right, out hitInfo2, 150f) && hitInfo2.collider.tag == "Cell" && gm.StartButtonPressed)
        {
            newPosition = hitInfo.collider.transform.position;
            newPosition2 = hitInfo2.collider.transform.position;
            pieceRight = hitInfo.collider.gameObject;
            canMove = true;
        }

        else if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 150f) &&
            hitInfo.collider.tag == "Piece" &&
            Physics.Raycast(hitInfo.collider.transform.position, Vector3.down, out hitInfo2, 150f) && hitInfo2.collider.tag == "Cell" && gm.StartButtonPressed)
        {
            newPosition = hitInfo.collider.transform.position;
            newPosition2 = hitInfo2.collider.transform.position;
            pieceRight = hitInfo.collider.gameObject;
            canMove = true;
        }

        else if (Physics.Raycast(transform.position, Vector3.left, out hitInfo, 150f) &&
            hitInfo.collider.tag == "Piece" &&
            Physics.Raycast(hitInfo.collider.transform.position, Vector3.left, out hitInfo2, 150f) && hitInfo2.collider.tag == "Cell" && gm.StartButtonPressed)
        {
            newPosition = hitInfo.collider.transform.position;
            newPosition2 = hitInfo2.collider.transform.position;
            pieceRight = hitInfo.collider.gameObject;
            canMove = true;
        }
        else
        {
            newPosition = transform.position;
            canMove = false;
            tryMovingInThrees = true;
        }
    }

    void MoveInThrees()
    {
        if (Physics.Raycast(transform.position, Vector3.up, out hitInfo, 150f) &&
            hitInfo.collider.tag == "Piece" &&
            Physics.Raycast(hitInfo.collider.transform.position, Vector3.up, out hitInfo2, 150f) && hitInfo2.collider.tag == "Piece" &&
            Physics.Raycast(hitInfo2.collider.transform.position, Vector3.up, out hitInfo3, 150f) && hitInfo3.collider.tag == "Cell" && 
            gm.StartButtonPressed)
        {
            newPosition = hitInfo.collider.transform.position;
            newPosition2 = hitInfo2.collider.transform.position;
            newPosition3 = hitInfo3.collider.transform.position;
            pieceUp = hitInfo.collider.gameObject;
            pieceUpUp = hitInfo2.collider.gameObject;
            canMove = true;
        }
        else if (Physics.Raycast(transform.position, Vector3.right, out hitInfo, 150f) &&
            hitInfo.collider.tag == "Piece" &&
            Physics.Raycast(hitInfo.collider.transform.position, Vector3.right, out hitInfo2, 150f) && hitInfo2.collider.tag == "Piece" &&
            Physics.Raycast(hitInfo2.collider.transform.position, Vector3.right, out hitInfo3, 150f) && hitInfo3.collider.tag == "Cell" &&
            gm.StartButtonPressed)
        {
            newPosition = hitInfo.collider.transform.position;
            newPosition2 = hitInfo2.collider.transform.position;
            newPosition3 = hitInfo3.collider.transform.position;
            pieceRight = hitInfo.collider.gameObject;
            pieceRightRight = hitInfo2.collider.gameObject;
            canMove = true;
        }

        else if (Physics.Raycast(transform.position, Vector3.down, out hitInfo, 150f) &&
            hitInfo.collider.tag == "Piece" &&
            Physics.Raycast(hitInfo.collider.transform.position, Vector3.down, out hitInfo2, 150f) && hitInfo2.collider.tag == "Piece" &&
            Physics.Raycast(hitInfo2.collider.transform.position, Vector3.down, out hitInfo3, 150f) && hitInfo3.collider.tag == "Cell" &&
            gm.StartButtonPressed)
        {
            newPosition = hitInfo.collider.transform.position;
            newPosition2 = hitInfo2.collider.transform.position;
            newPosition3 = hitInfo3.collider.transform.position;
            pieceDown = hitInfo.collider.gameObject;
            pieceDownDown = hitInfo2.collider.gameObject;
            canMove = true;
        }

        else if (Physics.Raycast(transform.position, Vector3.left, out hitInfo, 150f) &&
            hitInfo.collider.tag == "Piece" &&
            Physics.Raycast(hitInfo.collider.transform.position, Vector3.left, out hitInfo2, 150f) && hitInfo2.collider.tag == "Piece" &&
            Physics.Raycast(hitInfo2.collider.transform.position, Vector3.left, out hitInfo3, 150f) && hitInfo3.collider.tag == "Cell" &&
            gm.StartButtonPressed)
        {
            newPosition = hitInfo.collider.transform.position;
            newPosition2 = hitInfo2.collider.transform.position;
            newPosition3 = hitInfo3.collider.transform.position;
            pieceLeft = hitInfo.collider.gameObject;
            pieceLeftLeft = hitInfo2.collider.gameObject;
            canMove = true;
        }
    }
}
