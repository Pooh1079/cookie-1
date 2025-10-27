using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 targetPosition;
    private bool isMoving;
    public GameObject moveIndicator;
    private SpriteRenderer sr;

    void Start()
    {
        targetPosition = transform.position;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        // блокируем движение только если игрок реально строит
        if (BuildManager.instance != null && BuildManager.instance.isBuilding)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition = new Vector2(clickPos.x, clickPos.y);
            isMoving = true;

            if (moveIndicator != null)
                Instantiate(moveIndicator, targetPosition, Quaternion.identity);
        }

        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
            if (sr != null)
            {
                if (direction.x > 0.05f) sr.flipX = false;
                else if (direction.x < -0.05f) sr.flipX = true;
            }

            if (Vector2.Distance(transform.position, targetPosition) < 0.05f)
                isMoving = false;
        }
    }
}

