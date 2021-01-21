using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Piece3 : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private bool _movable;
    private Transform _emptyPosition;

    [SerializeField] int _myNumber;
    public int MyNumber
    {
        get
        {
            return _myNumber;
        }
    }

    private GameObject thisPiece;

    public Transform EmptyPosition
    {
        get { return _emptyPosition; }
        set { _emptyPosition = value; }
    }
    public bool Movable
    {
        get { return _movable; }
        set { _movable = value; }
    }

    [SerializeField] private int[] _piecePositionOnGrid = new int[2];
    public int[] PiecePositionOnGrid
    {
        get { return _piecePositionOnGrid; }
        set { _piecePositionOnGrid = value; }
    }

    [SerializeField]
    private string _levelFromEmpty;
    public string LevelFromEmpty
    {
        get { return _levelFromEmpty; }
        set { _levelFromEmpty = value; }
    }

    private void Start()
    {
        thisPiece = this.gameObject;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_movable && GameManager2.Instance.ShufflePressed && GameManager2.Instance.CanClick)
        {
            //Vector3 tempPosition = this.transform.position;
            //transform.position = _emptyPosition.position;
            //GameManager2.Instance.MoveEmpty(tempPosition);

            GameManager2.Instance.Move(thisPiece, _piecePositionOnGrid, _levelFromEmpty);
        }
            
    }
}
