using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameplayManager _gameplayManager;

    private bool _isDragging = false;
    private Vector3 _initialMousePosition;
    private Vector3 _dragDirection;
    private Vector3 _currentMousePosition;
    private RaycastHit _initialHit;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _isDragging == false)
        {
            InitDrag();
        }
        else if (Input.GetMouseButton(0) && _isDragging)
        {
            _currentMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            EndDrag();
        }
    }

    private void InitDrag()
    {
        _initialMousePosition = Input.mousePosition;

        Ray initRay = Camera.main.ScreenPointToRay(_initialMousePosition);
        Physics.Raycast(initRay, out _initialHit);

        if (_initialHit.collider.gameObject.TryGetComponent(out SpotController spot))
        {
            if (spot != null)
            {
                if (_gameplayManager.CurrentSpot != spot)
                {
                    _gameplayManager.CurrentSpot = spot;
                    _gameplayManager.CurrentTile = spot.CurrentTile;
                }

                _isDragging = true;
            }
        }
    }

    private void EndDrag()
    {
        _isDragging = false;

        Ray rayCurrentMousePos = Camera.main.ScreenPointToRay(_currentMousePosition);
        Physics.Raycast(rayCurrentMousePos, out RaycastHit currentHit);

        _dragDirection = (currentHit.point - _initialHit.point).normalized;
        _dragDirection = ConvertToFourDirection(_dragDirection);

        _gameplayManager.MoveSpot(_dragDirection);
    }

    private Vector3 ConvertToFourDirection(Vector3 direction)
    {
        float absX = Mathf.Abs(direction.x);
        float absZ = Mathf.Abs(direction.z);
        
        return new Vector3(absX > absZ ? Mathf.Round(direction.x) : 0, 0, absZ > absX ? Mathf.Round(direction.z) : 0);
    }
}