using UnityEngine;

public class LadderTrigger : MonoBehaviour
{
    public GameObject wallCollider;
    public GameObject rightWallCollider;
    public GameObject leftWallCollider;
    public GameObject player;
    public Transform groundPosition; 
    public Transform platformPosition;

    public int platformSortingOrder = 6;
    public int groundSortingOrder = 3;

    private bool isPlayerOnPlatform = false;
    private BoxCollider2D boxCollider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isPlayerOnPlatform)
            {
                wallCollider.transform.position = platformPosition.position;
                boxCollider = rightWallCollider.GetComponent<BoxCollider2D>();
                boxCollider.size = new Vector2(0.17f, 5.2f);
                boxCollider = leftWallCollider.GetComponent<BoxCollider2D>();
                boxCollider.size = new Vector2(0.17f, 5.2f);
                player.GetComponent<SpriteRenderer>().sortingOrder = platformSortingOrder;
                isPlayerOnPlatform = true;
            }
            else
            {
                player.GetComponent<SpriteRenderer>().sortingOrder = groundSortingOrder;
                boxCollider = rightWallCollider.GetComponent<BoxCollider2D>();
                boxCollider.size = new Vector2(0.17f, 5.2f);
                boxCollider = leftWallCollider.GetComponent<BoxCollider2D>();
                boxCollider.size = new Vector2(0.17f, 5.2f);
                wallCollider.transform.position = groundPosition.position;
                isPlayerOnPlatform = false;
            }
        }
    }
}
