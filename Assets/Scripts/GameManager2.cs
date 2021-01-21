using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class GameManager2 : MonoBehaviour
{
    private static GameManager2 _instance;
    public static GameManager2 Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("GameManager2 instance is NULL.");
            }

            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }

    [SerializeField] private List<Transform> _cells;    //to populate positions in Inspector
    private Transform[,] _grid = new Transform[4, 4];   //to create a 2D array of positions

    [SerializeField] private List<GameObject> _pieces;  //to populate pieces in Inspector
    private GameObject[,] _arrayOfPieces = new GameObject[4, 4]; //to create a 2D array of pieces
    private Piece3[,] _piece3 = new Piece3[4, 4];

    [SerializeField] private GameObject _pieceSet; //parent for pieces on Canvas

    [SerializeField] private float _speed; //piece movement speed during play
    int[] positionOfEmpty = new int[2];

    //int[] shuffledCellNumbers = new int[16];

    private bool _shufflePressed; //to allow moving of pieces in Piece3
    public bool CanClick { get; set; } //to prevent moving before animation is over
    public bool ShufflePressed //to prevent moving before shuffle is pressed
    {
        get { return _shufflePressed; }
    }

    [SerializeField] AudioClip knockSound;

    [SerializeField] private bool solvable;

    

    public int moves { get; set; }

    [SerializeField] private AudioSource _music;
    public AudioSource Music
    {
        get
        {
            if (_music == null)
            {
                Debug.LogError("Audio Source is NULL.");
            }

            return _music;
        }
    }
    
    public bool soundFX { get; set; }

    private void Start()
    {
        ListToGrid(_cells, _grid); //populating _grid with positions from _cells list
        ListToGrid(_pieces, _arrayOfPieces); //populating 2D array with pieces from _pieces list

        for (int row = 0; row < _arrayOfPieces.GetLength(0); row++)
        {
            for (int column = 0; column < _arrayOfPieces.GetLength(1); column++)
            {
                _arrayOfPieces[row, column] = Instantiate(_arrayOfPieces[row, column], _grid[row, column].position, Quaternion.identity, _pieceSet.transform); //arranging pieces on board
                _piece3[row, column] = _arrayOfPieces[row, column].GetComponent<Piece3>(); //getting Piece3 component of each piece on the array
                //_piece3[row, column].PiecePositionOnGrid = PositionOnGrid(_arrayOfPieces[row, column], _grid); //assigning grid position for each piece
            }
        }

        CanClick = true;
    }

    public void ListToGrid<T>(List<T> list, T[,] grid)
    {
        for (int row = 0; row < grid.GetLength(0); row++)
        {
            for (int column = 0; column < grid.GetLength(1); column++)
            {
                switch (row)
                {
                    case 0:
                        grid[row, column] = list[column];           //-x-|-0-|-0-|-0-
                        break;
                    case 1:
                        grid[row, column] = list[column + 4];       //-x-|-0-|-0-|-0-
                        break;                                      //-x-|-0-|-0-|-0-
                    case 2:
                        grid[row, column] = list[column + 8];       //-x-|-0-|-0-|-0-
                        break;                                      //-x-|-0-|-0-|-0-
                                                                    //-x-|-0-|-0-|-0-
                    
                    case 3:                                         //-x-|-0-|-0-|-0-
                        grid[row, column] = list[column + 12];      //-x-|-0-|-0-|-0-
                        break;                                      //-x-|-0-|-0-|-0-
                                                                    //-x-|-0-|-0-|-0-
                    default:
                        return;
                }
            }
        }
    }

    public void Shuffle()
    {
        List<Transform> shuffledCells = _cells;
        Transform[,] shuffledGrid = new Transform[4, 4];

        for (int i = 0; i < shuffledCells.Count; i++)
        {
            Transform obj = shuffledCells[i];
            int random = UnityEngine.Random.Range(0, i);
            shuffledCells[i] = shuffledCells[random];
            shuffledCells[random] = obj;
        }

        ListToGrid(shuffledCells, shuffledGrid);
        StartCoroutine(ShuffleProcedure(shuffledGrid));
        moves = 0;
        _shufflePressed = true;
    }

    public int[] PositionOnGrid(GameObject sourceItem, Transform[,] targetGrid)
    {
        int[] position = new int[2];

        for (int row = 0; row < targetGrid.GetLength(0); row++)
        {
            for (int column = 0; column < targetGrid.GetLength(1); column++)
            {
                if (sourceItem.transform.position == targetGrid[row, column].transform.position)
                {
                    position[0] = row;
                    position[1] = column;
                }
            }
        }

        return position;
    }

    private Piece3[] GridToArray()
    {
        Piece3[] newArray = new Piece3[16];
        for (int row = 0; row < _piece3.GetLength(0); row++)
        {
            for (int column = 0; column < _piece3.GetLength(1); column++)
            {
                if (_piece3[row, column].PiecePositionOnGrid[0] == 0 && _piece3[row, column].PiecePositionOnGrid[1] == 0)
                {
                    newArray[0] = _piece3[row, column];
                }
                else if (_piece3[row, column].PiecePositionOnGrid[0] == 0 && _piece3[row, column].PiecePositionOnGrid[1] == 1)
                {
                    newArray[1] = _piece3[row, column];
                }
                else if (_piece3[row, column].PiecePositionOnGrid[0] == 0 && _piece3[row, column].PiecePositionOnGrid[1] == 2)
                {
                    newArray[2] = _piece3[row, column];
                }
                else if (_piece3[row, column].PiecePositionOnGrid[0] == 0 && _piece3[row, column].PiecePositionOnGrid[1] == 3)
                {
                    newArray[3] = _piece3[row, column];
                }
                else if (_piece3[row, column].PiecePositionOnGrid[0] == 1 && _piece3[row, column].PiecePositionOnGrid[1] == 0)
                {
                    newArray[4] = _piece3[row, column];
                }
                else if (_piece3[row, column].PiecePositionOnGrid[0] == 1 && _piece3[row, column].PiecePositionOnGrid[1] == 1)
                {
                    newArray[5] = _piece3[row, column];
                }
                else if (_piece3[row, column].PiecePositionOnGrid[0] == 1 && _piece3[row, column].PiecePositionOnGrid[1] == 2)
                {
                    newArray[6] = _piece3[row, column];
                }
                else if (_piece3[row, column].PiecePositionOnGrid[0] == 1 && _piece3[row, column].PiecePositionOnGrid[1] == 3)
                {
                    newArray[7] = _piece3[row, column];
                }
                else if (_piece3[row, column].PiecePositionOnGrid[0] == 2 && _piece3[row, column].PiecePositionOnGrid[1] == 0)
                {
                    newArray[8] = _piece3[row, column];
                }
                else if (_piece3[row, column].PiecePositionOnGrid[0] == 2 && _piece3[row, column].PiecePositionOnGrid[1] == 1)
                {
                    newArray[9] = _piece3[row, column];
                }
                else if (_piece3[row, column].PiecePositionOnGrid[0] == 2 && _piece3[row, column].PiecePositionOnGrid[1] == 2)
                {
                    newArray[10] = _piece3[row, column];
                }
                else if (_piece3[row, column].PiecePositionOnGrid[0] == 2 && _piece3[row, column].PiecePositionOnGrid[1] == 3)
                {
                    newArray[11] = _piece3[row, column];
                }
                else if (_piece3[row, column].PiecePositionOnGrid[0] == 3 && _piece3[row, column].PiecePositionOnGrid[1] == 0)
                {
                    newArray[12] = _piece3[row, column];
                }
                else if (_piece3[row, column].PiecePositionOnGrid[0] == 3 && _piece3[row, column].PiecePositionOnGrid[1] == 1)
                {
                    newArray[13] = _piece3[row, column];
                }
                else if (_piece3[row, column].PiecePositionOnGrid[0] == 3 && _piece3[row, column].PiecePositionOnGrid[1] == 2)
                {
                    newArray[14] = _piece3[row, column];
                }
                else if (_piece3[row, column].PiecePositionOnGrid[0] == 3 && _piece3[row, column].PiecePositionOnGrid[1] == 3)
                {
                    newArray[15] = _piece3[row, column];
                }
            }
        }

        return newArray;
    } //populating shuffled 1D array for solvability method

    public void MovablePieces()
    {
        for (int row = 0; row < _arrayOfPieces.GetLength(0); row++)
        {
            for (int column = 0; column < _arrayOfPieces.GetLength(0); column++)
            {
                if (_piece3[row, column].PiecePositionOnGrid[0] == _piece3[3, 3].PiecePositionOnGrid[0])// piece and empty are in the same row
                {
                    _piece3[row, column].Movable = true;

                    if (_piece3[row, column].PiecePositionOnGrid[1] - _piece3[3, 3].PiecePositionOnGrid[1] == 1) //difference in columns
                    {
                        _piece3[row, column].LevelFromEmpty = "on the right 1";
                    }
                    else if (_piece3[row, column].PiecePositionOnGrid[1] - _piece3[3, 3].PiecePositionOnGrid[1] == -1)
                    {
                        _piece3[row, column].LevelFromEmpty = "on the left 1";
                    }
                    else if (_piece3[row, column].PiecePositionOnGrid[1] - _piece3[3, 3].PiecePositionOnGrid[1] == 2)
                    {
                        _piece3[row, column].LevelFromEmpty = "on the right 2";
                    }
                    else if (_piece3[row, column].PiecePositionOnGrid[1] - _piece3[3, 3].PiecePositionOnGrid[1] == -2)
                    {
                        _piece3[row, column].LevelFromEmpty = "on the left 2";
                    }
                    else if (_piece3[row, column].PiecePositionOnGrid[1] - _piece3[3, 3].PiecePositionOnGrid[1] == 3)
                    {
                        _piece3[row, column].LevelFromEmpty = "on the right 3";
                    }
                    else if (_piece3[row, column].PiecePositionOnGrid[1] - _piece3[3, 3].PiecePositionOnGrid[1] == -3)
                    {
                        _piece3[row, column].LevelFromEmpty = "on the left 3";
                    }
                }
                else if (_piece3[row, column].PiecePositionOnGrid[1] == _piece3[3, 3].PiecePositionOnGrid[1])
                {
                    _piece3[row, column].Movable = true;

                    if (_piece3[row, column].PiecePositionOnGrid[0] - _piece3[3, 3].PiecePositionOnGrid[0] == 1)
                    {
                        _piece3[row, column].LevelFromEmpty = "below 1";
                    }
                    else if (_piece3[row, column].PiecePositionOnGrid[0] - _piece3[3, 3].PiecePositionOnGrid[0] == -1)
                    {
                        _piece3[row, column].LevelFromEmpty = "above 1";
                    }
                    else if (_piece3[row, column].PiecePositionOnGrid[0] - _piece3[3, 3].PiecePositionOnGrid[0] == 2)
                    {
                        _piece3[row, column].LevelFromEmpty = "below 2";
                    }
                    else if (_piece3[row, column].PiecePositionOnGrid[0] - _piece3[3, 3].PiecePositionOnGrid[0] == -2)
                    {
                        _piece3[row, column].LevelFromEmpty = "above 2";
                    }
                    else if (_piece3[row, column].PiecePositionOnGrid[0] - _piece3[3, 3].PiecePositionOnGrid[0] == 3)
                    {
                        _piece3[row, column].LevelFromEmpty = "below 3";
                    }
                    else if (_piece3[row, column].PiecePositionOnGrid[0] - _piece3[3, 3].PiecePositionOnGrid[0] == -3)
                    {
                        _piece3[row, column].LevelFromEmpty = "above 3";
                    }
                }
                else
                {
                    _arrayOfPieces[row, column].GetComponent<Piece3>().Movable = false;
                    _piece3[row, column].LevelFromEmpty = "";
                }
            }
        }
    }

    public void Move(GameObject thisPiece, int[] positionOnGrid, string levelFromEmpty)
    {
        GameObject pieceBelow = _arrayOfPieces[PieceBelowPosition(positionOnGrid)[0], PieceBelowPosition(positionOnGrid)[1]];
        GameObject pieceAbove = _arrayOfPieces[PieceAbovePosition(positionOnGrid)[0], PieceAbovePosition(positionOnGrid)[1]];
        GameObject pieceOnTheRight = _arrayOfPieces[PieceOnTheRightPosition(positionOnGrid)[0], PieceOnTheRightPosition(positionOnGrid)[1]];
        GameObject pieceOnTheLeft = _arrayOfPieces[PieceOnTheLeftPosition(positionOnGrid)[0], PieceOnTheLeftPosition(positionOnGrid)[1]];

        GameObject pieceBelowBelow = _arrayOfPieces[PieceBelowBelowPosition(positionOnGrid)[0], PieceBelowBelowPosition(positionOnGrid)[1]];
        GameObject pieceAboveAbove = _arrayOfPieces[PieceAboveAbovePosition(positionOnGrid)[0], PieceAboveAbovePosition(positionOnGrid)[1]];
        GameObject pieceOnTheRightRight = _arrayOfPieces[PieceOnTheRightRightPosition(positionOnGrid)[0], PieceOnTheRightRightPosition(positionOnGrid)[1]];
        GameObject pieceOnTheLeftLeft = _arrayOfPieces[PieceOnTheLeftLeftPosition(positionOnGrid)[0], PieceOnTheLeftLeftPosition(positionOnGrid)[1]];



        if (levelFromEmpty == "above 1" || levelFromEmpty == "on the right 1" || levelFromEmpty == "below 1" || levelFromEmpty == "on the left 1")
        {
            Vector3 tempPosition = thisPiece.transform.position;
            StartCoroutine(DoTweenProcedure1(thisPiece, _arrayOfPieces[3, 3].transform.position));
            _arrayOfPieces[3, 3].transform.position = tempPosition;
        }

        else if (levelFromEmpty == "above 2")
        {
            Vector3 down = _grid[positionOnGrid[0] + 1, positionOnGrid[1]].position;
            Vector3 down2 = _grid[positionOnGrid[0] + 2, positionOnGrid[1]].position;
            Vector3 tempPosition = thisPiece.transform.position;
            StartCoroutine(DoTweenProcedure2(
                thisPiece, down,
                pieceBelow, down2));
            _arrayOfPieces[3, 3].transform.position = tempPosition;
        }
        else if (levelFromEmpty == "below 2")
        {
            Vector3 up = _grid[positionOnGrid[0] - 1, positionOnGrid[1]].position;
            Vector3 up2 = _grid[positionOnGrid[0] - 2, positionOnGrid[1]].position;
            Vector3 tempPosition = thisPiece.transform.position;
            StartCoroutine(DoTweenProcedure2(
                thisPiece, up,
                pieceAbove, up2));
            _arrayOfPieces[3, 3].transform.position = tempPosition;
        }
        else if (levelFromEmpty == "on the right 2")
        {
            Vector3 left = _grid[positionOnGrid[0], positionOnGrid[1] - 1].position;
            Vector3 left2 = _grid[positionOnGrid[0], positionOnGrid[1] - 2].position;
            Vector3 tempPosition = thisPiece.transform.position;
            StartCoroutine(DoTweenProcedure2(
                thisPiece, left,
                pieceOnTheLeft, left2));
            _arrayOfPieces[3, 3].transform.position = tempPosition;
        }
        else if (levelFromEmpty == "on the left 2")
        {
            Vector3 right = _grid[positionOnGrid[0], positionOnGrid[1] + 1].position;
            Vector3 right2 = _grid[positionOnGrid[0], positionOnGrid[1] + 2].position;
            Vector3 tempPosition = thisPiece.transform.position;
            StartCoroutine(DoTweenProcedure2(
                thisPiece, right,
                pieceOnTheRight, right2));
            _arrayOfPieces[3, 3].transform.position = tempPosition;
        }

        else if (levelFromEmpty == "above 3")
        {
            Vector3 down = _grid[positionOnGrid[0] + 1, positionOnGrid[1]].position;
            Vector3 down2 = _grid[positionOnGrid[0] + 2, positionOnGrid[1]].position;
            Vector3 down3 = _grid[positionOnGrid[0] + 3, positionOnGrid[1]].position;
            Vector3 tempPosition = thisPiece.transform.position;
            StartCoroutine(DoTweenProcedure3(
                thisPiece, down,
                pieceBelow, down2,
                pieceBelowBelow, down3));
            _arrayOfPieces[3, 3].transform.position = tempPosition;
        }
        else if (levelFromEmpty == "below 3")
        {
            Vector3 up = _grid[positionOnGrid[0] - 1, positionOnGrid[1]].position;
            Vector3 up2 = _grid[positionOnGrid[0] - 2, positionOnGrid[1]].position;
            Vector3 up3 = _grid[positionOnGrid[0] - 3, positionOnGrid[1]].position;
            Vector3 tempPosition = thisPiece.transform.position;
            StartCoroutine(DoTweenProcedure3(
                thisPiece, up,
                pieceAbove, up2,
                pieceAboveAbove, up3));
            _arrayOfPieces[3, 3].transform.position = tempPosition;
        }
        else if (levelFromEmpty == "on the right 3")
        {
            Vector3 left = _grid[positionOnGrid[0], positionOnGrid[1] - 1].position;
            Vector3 left2 = _grid[positionOnGrid[0], positionOnGrid[1] - 2].position;
            Vector3 left3 = _grid[positionOnGrid[0], positionOnGrid[1] - 3].position;
            Vector3 tempPosition = thisPiece.transform.position;
            StartCoroutine(DoTweenProcedure3(
                thisPiece, left,
                pieceOnTheLeft, left2,
                pieceOnTheLeftLeft, left3));
            _arrayOfPieces[3, 3].transform.position = tempPosition;
        }
        else if (levelFromEmpty == "on the left 3")
        {
            Vector3 right = _grid[positionOnGrid[0], positionOnGrid[1] + 1].position;
            Vector3 right2 = _grid[positionOnGrid[0], positionOnGrid[1] + 2].position;
            Vector3 right3 = _grid[positionOnGrid[0], positionOnGrid[1] + 3].position;
            Vector3 tempPosition = thisPiece.transform.position;
            StartCoroutine(DoTweenProcedure3(
                thisPiece, right,
                pieceOnTheRight, right2,
                pieceOnTheRightRight, right3));
            _arrayOfPieces[3, 3].transform.position = tempPosition;
        }

        moves++;
        UIManager.Instance.UpdateMovesText(moves);
    }

    private void UpdatePositions()
    {
        for (int row = 0; row < _arrayOfPieces.GetLength(0); row++)
        {
            for (int column = 0; column < _arrayOfPieces.GetLength(1); column++)
            {
                _piece3[row, column].PiecePositionOnGrid = PositionOnGrid(_arrayOfPieces[row, column], _grid);
            }
        }
    }

    private int[] PieceBelowPosition(int[] positionOnGrid)
    {
        int[] position = new int[2];

        for (int row = 0; row < _piece3.GetLength(0); row++)
        {
            for (int column = 0; column < _piece3.GetLength(1); column++)
            {
                if (_piece3[row, column].PiecePositionOnGrid[0] == positionOnGrid[0] + 1 && _piece3[row, column].PiecePositionOnGrid[1] == positionOnGrid[1])
                {
                    position[0] = row;
                    position[1] = column;
                }
            }
        }

        return position;
    }

    private int[] PieceAbovePosition(int[] positionOnGrid)
    {
        int[] position = new int[2];

        for (int row = 0; row < _piece3.GetLength(0); row++)
        {
            for (int column = 0; column < _piece3.GetLength(1); column++)
            {
                if (_piece3[row, column].PiecePositionOnGrid[0] == positionOnGrid[0] - 1 && _piece3[row, column].PiecePositionOnGrid[1] == positionOnGrid[1])
                {
                    position[0] = row;
                    position[1] = column;
                }
            }
        }

        return position;
    }

    private int[] PieceOnTheLeftPosition(int[] positionOnGrid)
    {
        int[] position = new int[2];

        for (int row = 0; row < _piece3.GetLength(0); row++)
        {
            for (int column = 0; column < _piece3.GetLength(1); column++)
            {
                if (_piece3[row, column].PiecePositionOnGrid[0] == positionOnGrid[0] && _piece3[row, column].PiecePositionOnGrid[1] == positionOnGrid[1] - 1)
                {
                    position[0] = row;
                    position[1] = column;
                }
            }
        }

        return position;
    }

    private int[] PieceOnTheRightPosition(int[] positionOnGrid)
    {
        int[] position = new int[2];

        for (int row = 0; row < _piece3.GetLength(0); row++)
        {
            for (int column = 0; column < _piece3.GetLength(1); column++)
            {
                if (_piece3[row, column].PiecePositionOnGrid[0] == positionOnGrid[0] && _piece3[row, column].PiecePositionOnGrid[1] == positionOnGrid[1] + 1)
                {
                    position[0] = row;
                    position[1] = column;
                }
            }
        }

        return position;
    }


    private int[] PieceBelowBelowPosition(int[] positionOnGrid)
    {
        int[] position = new int[2];

        for (int row = 0; row < _piece3.GetLength(0); row++)
        {
            for (int column = 0; column < _piece3.GetLength(1); column++)
            {
                if (_piece3[row, column].PiecePositionOnGrid[0] == positionOnGrid[0] + 2 && _piece3[row, column].PiecePositionOnGrid[1] == positionOnGrid[1])
                {
                    position[0] = row;
                    position[1] = column;
                }
            }
        }

        return position;
    }

    private int[] PieceAboveAbovePosition(int[] positionOnGrid)
    {
        int[] position = new int[2];

        for (int row = 0; row < _piece3.GetLength(0); row++)
        {
            for (int column = 0; column < _piece3.GetLength(1); column++)
            {
                if (_piece3[row, column].PiecePositionOnGrid[0] == positionOnGrid[0] - 2 && _piece3[row, column].PiecePositionOnGrid[1] == positionOnGrid[1])
                {
                    position[0] = row;
                    position[1] = column;
                }
            }
        }

        return position;
    }

    private int[] PieceOnTheLeftLeftPosition(int[] positionOnGrid)
    {
        int[] position = new int[2];

        for (int row = 0; row < _piece3.GetLength(0); row++)
        {
            for (int column = 0; column < _piece3.GetLength(1); column++)
            {
                if (_piece3[row, column].PiecePositionOnGrid[0] == positionOnGrid[0] && _piece3[row, column].PiecePositionOnGrid[1] == positionOnGrid[1] - 2)
                {
                    position[0] = row;
                    position[1] = column;
                }
            }
        }

        return position;
    }

    private int[] PieceOnTheRightRightPosition(int[] positionOnGrid)
    {
        int[] position = new int[2];

        for (int row = 0; row < _piece3.GetLength(0); row++)
        {
            for (int column = 0; column < _piece3.GetLength(1); column++)
            {
                if (_piece3[row, column].PiecePositionOnGrid[0] == positionOnGrid[0] && _piece3[row, column].PiecePositionOnGrid[1] == positionOnGrid[1] + 2)
                {
                    position[0] = row;
                    position[1] = column;
                }
            }
        }

        return position;
    }

    private void CheckForWin()
    {
        int winScore = 0;

        for (int row = 0; row < _arrayOfPieces.GetLength(0); row++)
        {
            for (int column = 0; column < _arrayOfPieces.GetLength(1); column++)
            {
                if (_arrayOfPieces[row, column].transform.position == _grid[row, column].position)
                {
                    winScore++;
                    if (winScore == 16)
                    {
                        UIManager.Instance.StopTimer();
                        Settings.Instance.HighScore();
                    }
                }
            }

        }
    }

    IEnumerator DoTweenProcedure1(GameObject obj, Vector3 targetPosition)
    {
        CanClick = false;
        Tween myTween = obj.transform.DOMove(targetPosition, _speed);
        yield return myTween.WaitForCompletion();
        if (soundFX)
        {
            AudioSource.PlayClipAtPoint(knockSound, Camera.main.transform.position, 1f);
        }
        UpdatePositions();
        MovablePieces();
        CanClick = true;
        CheckForWin();
    }

    IEnumerator DoTweenProcedure2(
        GameObject obj1, Vector3 targetPosition1,
        GameObject obj2, Vector3 targetPosition2)
    {
        CanClick = false;
        Tween myTween1 = obj1.transform.DOMove(targetPosition1, _speed);
        Tween myTween2 = obj2.transform.DOMove(targetPosition2, _speed);
        yield return myTween2.WaitForCompletion();
        if (soundFX)
        {
            AudioSource.PlayClipAtPoint(knockSound, Camera.main.transform.position, 1f);
        }
        UpdatePositions();
        MovablePieces();
        CanClick = true;
        CheckForWin();
    }

    IEnumerator DoTweenProcedure3(
        GameObject obj1, Vector3 targetPosition1,
        GameObject obj2, Vector3 targetPosition2,
        GameObject obj3, Vector3 targetPosition3)
    {
        CanClick = false;
        Tween myTween = obj1.transform.DOMove(targetPosition1, _speed);
        Tween myTween2 = obj2.transform.DOMove(targetPosition2, _speed);
        Tween myTween3 = obj3.transform.DOMove(targetPosition3, _speed);
        yield return myTween3.WaitForCompletion();
        if (soundFX)
        {
            AudioSource.PlayClipAtPoint(knockSound, Camera.main.transform.position, 1f);
        }
        UpdatePositions();
        MovablePieces();
        CanClick = true;
        CheckForWin();
    }

    IEnumerator ShuffleProcedure(Transform[,] shuffledGrid)//animating shuffling
    {  
        Tween goodTween = null;

        // Moving pieces

        for (int row = 0; row < shuffledGrid.GetLength(0); row++)
        {
            for (int column = 0; column < shuffledGrid.GetLength(1); column++)
            {
                goodTween = _arrayOfPieces[row, column].transform.DOMove(shuffledGrid[row, column].position, 1);
            }
        }

        yield return goodTween.WaitForCompletion();

        
        
        // Getting piece positions on grid

        for (int row = 0; row < shuffledGrid.GetLength(0); row++)
        {
            for (int column = 0; column < shuffledGrid.GetLength(1); column++)
            {
                _piece3[row, column].PiecePositionOnGrid = PositionOnGrid(_arrayOfPieces[row, column], _grid);
            }
        }

        
        int[] newArrayNumbers = new int[16];

        for (int i = 0; i < GridToArray().Length; i++)
        {

            newArrayNumbers[i] = GridToArray()[i].MyNumber; //seeing what piece is on each index of shuffled array 
            //!!! Try to move this out of coroutine!!!
        }

        Solvability(newArrayNumbers);

        if (solvable)
        {
            UIManager.Instance.StartTimer();
        }
        else
        {
            UIManager.Instance.ShowUnsolvablePanel();
        }

        MovablePieces();

    }

    private bool Solvability(int[] list)
    {
        //count inversions

        int inversionCount = 0;
        for (int i = 0; i < list.Length - 1; i++)
        {
            for (int j = i + 1; j < list.Length; j++)
            {
                if (list[i] != 0)
                {
                    if (list[j] != 0)
                    {
                        if (list[i] > list[j])
                        {
                            inversionCount++;
                        }
                    }
                }
            }
            
        }

        Debug.Log("Inversion count: " + inversionCount);

        //find row of empty

        positionOfEmpty = PositionOnGrid(_arrayOfPieces[3, 3], _grid);

        int rowOfEmpty = positionOfEmpty[0] + 1;

        Debug.Log("Row of empty: " + rowOfEmpty);

        if (rowOfEmpty == 1 || rowOfEmpty == 3)
        {
            if (inversionCount % 2 != 0)
            {
                solvable = true;
                Debug.Log("Puzzle solvable");
            }
            else if (inversionCount % 2 == 0)
            {
                solvable = false;
                Debug.Log("Puzzle NOT solvable");
            }
        }
        else if (rowOfEmpty == 2 || rowOfEmpty == 4)
        {
            if (inversionCount % 2 == 0)
            {
                solvable = true;
                Debug.Log("Puzzle solvable");
            }
            else if (inversionCount % 2 != 0)
            {
                solvable = false;
                Debug.Log("Puzzle NOT solvable");
            }
        }

        return solvable;
    }

    public void QuitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }


}
