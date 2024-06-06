using UnityEngine;
using System.Collections.Generic;

public class RandomSpritePlacer : MonoBehaviour
{
    public GameObject[] sprites; // ������ ����� ��������
    public Vector2 topLeftCorner; // ������� ����� ���� �������
    public Vector2 bottomRightCorner; // ������ ������ ���� �������

    private const float spriteSize = 32.0f; // ������ ������� � ��������

    [ContextMenu("Place Sprites")]
    public void PlaceSprites()
    {
        // ������� ������� �������� ��������
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        // �������� ������� ������� � �������� ����
        float roomWidth = bottomRightCorner.x - topLeftCorner.x;
        float roomHeight = topLeftCorner.y - bottomRightCorner.y;

        // ���������� �������� �� ������ � ������
        int spritesInRow = Mathf.CeilToInt(roomWidth / spriteSize);
        int spritesInColumn = Mathf.CeilToInt(roomHeight / spriteSize);

        // ������� ������ ��� ������������ ������� �������
        List<Vector2> usedPositions = new List<Vector2>();

        // ��������� ������� ���������
        for (int y = 0; y < spritesInColumn; y++)
        {
            for (int x = 0; x < spritesInRow; x++)
            {
                Vector2 newPos = new Vector2(
                    topLeftCorner.x + x * spriteSize,
                    topLeftCorner.y - y * spriteSize
                );

                // ��������� ������� � ������ �������
                usedPositions.Add(newPos);

                // ������� ������ � ����� �������
                int spriteIndex = Random.Range(0, sprites.Length);
                GameObject newSprite = Instantiate(sprites[spriteIndex], newPos, Quaternion.identity);
                newSprite.transform.parent = transform; // ������ ����� ������ �������� ��������
            }
        }
    }
}
