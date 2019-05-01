using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelSquareSort : MonoBehaviour
{
    // 150 total squares for 15X10
    // 1024 total squares for 32X32
    public Transform UndergroundTiles;

    public int mapXSize;
    public int mapZSize;

    private int xTilePos = 0;
    private int zTilePos = 0;

    private int childCount = 0;

    private bool timeToSort = false;

    private void Update()
    {
        if (timeToSort == true)
        {
            return;
        }

        childCount = this.transform.childCount;

        RaycastHit hit = new RaycastHit();

        foreach (Transform child in this.transform)
        {

            if (Physics.Raycast(child.transform.position, Vector3.up, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.tag == "GameBoardSquare")
                {
                    child.SetParent(UndergroundTiles);
                }
            }
        }

        if(childCount <= 150)
        {
            StartCoroutine(SortChildren());
            timeToSort = true;
        }
    }

    private IEnumerator SortChildren()
    {
        int childTracker = 0;

        while (childTracker < this.transform.childCount)
        {

            foreach (Transform child in this.transform)
            {
                Vector3 Distance = this.transform.position - child.transform.position;

                float distanceInX = Mathf.Abs(Distance.x);
                float distanceInZ = Mathf.Abs(Distance.z);

                if (Mathf.Abs(distanceInX - xTilePos) < 1 && Mathf.Abs(distanceInZ - zTilePos) < 1)
                {

                    child.SetSiblingIndex((xTilePos * mapZSize) + zTilePos);

                    zTilePos++;

                    if (zTilePos >= mapZSize)
                    {
                        zTilePos = 0;
                        xTilePos++;
                    }
                }
            }

            childTracker++;

            yield return null;
        }

        Destroy(this);
    }
}
