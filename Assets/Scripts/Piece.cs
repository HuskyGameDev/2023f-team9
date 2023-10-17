using UnityEngine;
using UnityEngine.InputSystem;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int position { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public int rotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    //GO OVER WITH TEAM!!!
    private float lockTime;

    private InputAction moveAction;
    private InputAction downAction;
    private InputAction rotateAction;

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;
        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;

        moveAction = GameManager.Instance.inputActions.Dropper.Move;
        rotateAction = GameManager.Instance.inputActions.Dropper.Rotate;
        downAction = GameManager.Instance.inputActions.Dropper.Down;

        this.cells ??= new Vector3Int[data.cells.Length];

        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }

        //Setup actions
        PlayerInput playerInput = GetComponent<PlayerInput>();
        this.moveAction = playerInput.actions.FindAction("Block Move");
        this.downAction = playerInput.actions.FindAction("Block Down");
        this.rotateAction = playerInput.actions.FindAction("Block Rotate");
    }

    private void Update()
    {
        //Clears previous position
        this.board.Clear(this);

        this.lockTime += Time.deltaTime;

        //Rotation controls
        if (rotateAction.triggered)
        {
            if (rotateAction.ReadValue<float>() > 0)
            {
                Rotate(1);
            }
            else
            {
                Rotate(-1);
            }
        }

        //Left and Right controls
        if (moveAction.triggered)
        {
            if (moveAction.ReadValue<float>() > 0)
            {
                Move(Vector2Int.right);
            }
            else
            {
                Move(Vector2Int.left);
            }
        }

        //Move down (testing stuff)
        if (downAction.triggered)
        {
            Move(Vector2Int.down);
        }

        if (Time.time >= this.stepTime)
        {
            Step();
        }

        this.board.Set(this);
    }

    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay;

        Move(Vector2Int.down);

        if (this.lockTime >= this.lockDelay)
        {
            Lock();
        }
    }

    private void Lock()
    {
        this.board.Set(this);
        this.board.ClearLines();
        this.board.SpawnPiece();
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsValidPosition(this, newPosition);

        if (valid)
        {
            this.position = newPosition;
            //GO OVER WITH TEAM!!!
            this.lockTime = 0f;
        }

        return valid;
    }

    private void Rotate(int direction)
    {
        int originalRotation = this.rotationIndex;
        this.rotationIndex = Wrap(this.rotationIndex + direction, 0, 4);


        ApplyRotationMatrix(direction);
        if (!TestWallKicks(this.rotationIndex, direction))
        {
            this.rotationIndex = originalRotation;
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            Vector3 cell = this.cells[i];

            int x, y;
            switch (this.data.tetromino)
            {
                case Tetromino.I_Block:
                case Tetromino.O_Block:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));

                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }


    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < this.data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }
        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;
        if (rotationDirection < 0)
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));
    }

    //This one is hard to explain but basically it wraps the numbers in a matrix. If we have 0, 1, 2, 3, and we want to shift 1
    //it won't give us 0, 0, 1, 2, instead it will give us 3, 0, 1, 2. Important for position of tiles in the cells
    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
}
