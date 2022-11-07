using UnityEngine;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour, IPointerDownHandler
{
    public int MyNumber => myNumber;
    public bool Movable { get => movable; set => movable = value; }
    public int[] PiecePositionOnGrid { get => piecePositionOnGrid; set => piecePositionOnGrid = value; }
    public string LevelFromEmpty { set => levelFromEmpty = value; }
    
    [SerializeField] private bool movable;
    [SerializeField] int myNumber;
    [SerializeField] private int[] piecePositionOnGrid = new int[2];
    [SerializeField] private string levelFromEmpty;

    private Transform _emptyPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (movable && GameManager.Instance.ShufflePressed && GameManager.Instance.CanClick) 
            GameManager.Instance.Move(gameObject, piecePositionOnGrid, levelFromEmpty);
    }
}
