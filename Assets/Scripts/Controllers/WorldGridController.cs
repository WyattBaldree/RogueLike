using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using static GameController;

public class WorldGridController : MonoBehaviour
{
    [SerializeField]
    private WorldGridInstance worldGridInstanceClass;

    private WorldGridInstance[,] worldGridArray;

    public void Initialize()
    {
        Assert.IsNotNull(worldGridInstanceClass, "The Floor Inventory Class has not been set.");

        Vector2Int screenRes = GetGameController().ScreenResInUnits;

        // create all item lists
        worldGridArray = new WorldGridInstance[screenRes.x, screenRes.y];

        for (int i = 0; i < screenRes.x; i++)
        {
            for (int j = 0; j < screenRes.y; j++)
            {
                worldGridArray[i, j] = (WorldGridInstance)Instantiate(worldGridInstanceClass, new Vector3(i, j), Quaternion.identity, transform);
            }
        }
    }
}
