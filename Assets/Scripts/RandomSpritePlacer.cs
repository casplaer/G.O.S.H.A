using UnityEngine;
using System.Collections.Generic;

public class RandomSpritePlacer : MonoBehaviour
{
    public GameObject[] sprites; // массив ваших спрайтов
    public Vector2 topLeftCorner; // верхний левый угол комнаты
    public Vector2 bottomRightCorner; // нижний правый угол комнаты

    private const float spriteSize = 32.0f; // размер спрайта в пиксел€х

    [ContextMenu("Place Sprites")]
    public void PlaceSprites()
    {
        // ќчистка текущих дочерних объектов
        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        // ѕолучаем размеры комнаты в единицах мира
        float roomWidth = bottomRightCorner.x - topLeftCorner.x;
        float roomHeight = topLeftCorner.y - bottomRightCorner.y;

        //  оличество спрайтов по ширине и высоте
        int spritesInRow = Mathf.CeilToInt(roomWidth / spriteSize);
        int spritesInColumn = Mathf.CeilToInt(roomHeight / spriteSize);

        // —оздаем список дл€ отслеживани€ зан€тых позиций
        List<Vector2> usedPositions = new List<Vector2>();

        // «аполн€ем комнату спрайтами
        for (int y = 0; y < spritesInColumn; y++)
        {
            for (int x = 0; x < spritesInRow; x++)
            {
                Vector2 newPos = new Vector2(
                    topLeftCorner.x + x * spriteSize,
                    topLeftCorner.y - y * spriteSize
                );

                // ƒобавл€ем позицию в список зан€тых
                usedPositions.Add(newPos);

                // —оздаем спрайт в новой позиции
                int spriteIndex = Random.Range(0, sprites.Length);
                GameObject newSprite = Instantiate(sprites[spriteIndex], newPos, Quaternion.identity);
                newSprite.transform.parent = transform; // делаем новый спрайт дочерним объектом
            }
        }
    }
}
