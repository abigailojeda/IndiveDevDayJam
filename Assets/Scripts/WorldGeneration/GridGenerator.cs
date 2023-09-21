using UnityEngine;
using System.Collections.Generic;
using System;

public class GridGenerator : MonoBehaviour
{
    public static Action finishedGeneratingGrid;

    public GameObject gridSquarePrefab;
    public int rows = 5;
    public int columns = 5;
    public float spacing = 1.1f; // Adjust this value to control spacing
    public Color pathColor = Color.cyan;
    public Color t1Color = Color.green;
    public Color t2Color = Color.yellow;
    public Color t3Color = Color.red;
    public Color t4Color = Color.blue;
    public int desiredPathLength = 20; // Desired path length
    public int minDistanceFromBorder = 1; // Desired distance from the border
    [Range(0, 1)]
    public float straightPathBias = 0.3f; // Bias towards straight paths

    private GameObject[,] grid;
    public List<GameObject> pathElements = new List<GameObject>();
    public List<GameObject> tier1Elements = new List<GameObject>();
    public List<GameObject> tier2Elements = new List<GameObject>();
    public List<GameObject> tier3Elements = new List<GameObject>();
    public List<GameObject> tier4Elements = new List<GameObject>();

    void Start()
    {
        GenerateGrid();
        GeneratePath();
        FindTierElements();
        RotateTiersToPath();
        finishedGeneratingGrid?.Invoke();
    }

    void FindTierElements()
    {
        // Call the function for different distances
        FindElementsAtDistance(4, tier4Elements, t4Color);
        FindElementsAtDistance(3, tier3Elements, t3Color);
        FindElementsAtDistance(2, tier2Elements, t2Color);
        FindElementsAtDistance(1, tier1Elements, t1Color);

        // Clean up the arrays to remove elements present in lower tiers
        RemoveElementsInLowerTiers(tier4Elements, tier3Elements, tier2Elements, tier1Elements);
        RemoveElementsInLowerTiers(tier3Elements, tier2Elements, tier1Elements);
        RemoveElementsInLowerTiers(tier2Elements, tier1Elements);

        // Now, your tier1Elements, tier2Elements, tier3Elements, and tier4Elements lists contain elements at different distances from the path.
    }

    void GenerateGrid()
    {
        grid = new GameObject[rows, columns];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 position = new Vector3(i * spacing, 0, j * spacing); // Apply spacing
                GameObject gridSquare = Instantiate(gridSquarePrefab, position, Quaternion.identity, transform);

                // Get the MeshRenderer component
                MeshRenderer meshRenderer = gridSquare.GetComponent<MeshRenderer>();

                // Assign the material to the MeshRenderer
                meshRenderer.material = new Material(meshRenderer.material);

                grid[i, j] = gridSquare;
            }
        }
    }

    void GeneratePath()
    {
        int maxPathLength = desiredPathLength; // Desired path length

        while (pathElements.Count < maxPathLength)
        {
            // Clear existing path elements and start fresh
            pathElements.Clear();

            MarkRandomPath();
            ContinuePath();
        }
    }

    void MarkRandomPath()
    {
        // Calculate the middle row and column of the grid
        int middleRow = rows / 2;
        int middleColumn = columns / 2;

        // Mark the middle element as the starting point of the path
        MarkAsPath(middleRow, middleColumn);

        /* int side = Random.Range(0, 4); // 0: top, 1: right, 2: bottom, 3: left

        switch (side)
        {
            case 0: // Top
                int randomColumn = Random.Range(0, columns);
                MarkAsPath(0, randomColumn);
                break;
            case 1: // Right
                int randomRow = Random.Range(0, rows);
                MarkAsPath(randomRow, columns - 1);
                break;
            case 2: // Bottom
                int randomColumn2 = Random.Range(0, columns);
                MarkAsPath(rows - 1, randomColumn2);
                break;
            case 3: // Left
                int randomRow2 = Random.Range(0, rows);
                MarkAsPath(randomRow2, 0);
                break;
        } */
    }

    void ContinuePath()
    {
        while (pathElements.Count < desiredPathLength)
        {
            List<GameObject> availableAdjacentElements = new List<GameObject>();

            // Find the most recent path element
            GameObject currentPathElement = pathElements[pathElements.Count - 1];

            int currentRow = -1;
            int currentCol = -1;

            // Find the current element's row and column in the grid
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (grid[i, j] == currentPathElement)
                    {
                        currentRow = i;
                        currentCol = j;
                        break;
                    }
                }
            }

            // Check and add adjacent elements that are not part of the path and are at least 5 tiles away from any border
            if (currentRow > minDistanceFromBorder && !pathElements.Contains(grid[currentRow - 1, currentCol]))
            {
                if (CountAdjacentPaths(currentRow - 1, currentCol) <= 1)
                {
                    availableAdjacentElements.Add(grid[currentRow - 1, currentCol]); // Up
                }
            }
            if (currentRow < rows - minDistanceFromBorder - 1 && !pathElements.Contains(grid[currentRow + 1, currentCol]))
            {
                if (CountAdjacentPaths(currentRow + 1, currentCol) <= 1)
                {
                    availableAdjacentElements.Add(grid[currentRow + 1, currentCol]); // Down
                }
            }
            if (currentCol > minDistanceFromBorder && !pathElements.Contains(grid[currentRow, currentCol - 1]))
            {
                if (CountAdjacentPaths(currentRow, currentCol - 1) <= 1)
                {
                    availableAdjacentElements.Add(grid[currentRow, currentCol - 1]); // Left
                }
            }
            if (currentCol < columns - minDistanceFromBorder - 1 && !pathElements.Contains(grid[currentRow, currentCol + 1]))
            {
                if (CountAdjacentPaths(currentRow, currentCol + 1) <= 1)
                {
                    availableAdjacentElements.Add(grid[currentRow, currentCol + 1]); // Right
                }
            }

            if (availableAdjacentElements.Count == 0)
            {
                break; // No more available adjacent elements
            }

            // Bias towards straight paths
            float randomBias = UnityEngine.Random.Range(0f, 1f);
            float straightBiasThreshold = straightPathBias;
            GameObject nextPathElement;

            if (randomBias < straightBiasThreshold)
            {
                // Select an available adjacent element in the same direction as the previous element
                nextPathElement = GetSameDirectionElement(availableAdjacentElements, currentPathElement);
            }
            else
            {
                // Randomly select an available adjacent element
                int randomIndex = UnityEngine.Random.Range(0, availableAdjacentElements.Count);
                nextPathElement = availableAdjacentElements[randomIndex];
            }

            // Mark it as part of the path
            MarkAsPath(nextPathElement);
        }
    }

    int CountAdjacentPaths(int row, int col)
    {
        int count = 0;

        if (row > 0 && pathElements.Contains(grid[row - 1, col])) count++; // Up
        if (row < rows - 1 && pathElements.Contains(grid[row + 1, col])) count++; // Down
        if (col > 0 && pathElements.Contains(grid[row, col - 1])) count++; // Left
        if (col < columns - 1 && pathElements.Contains(grid[row, col + 1])) count++; // Right

        return count;
    }

    void MarkAsPath(int row, int column)
    {
        GameObject element = grid[row, column];
        element.GetComponent<MeshRenderer>().material.color = pathColor;
        pathElements.Add(element);
    }

    void MarkAsPath(GameObject element)
    {
        element.GetComponent<MeshRenderer>().material.color = pathColor;
        pathElements.Add(element);
    }

     GameObject GetSameDirectionElement(List<GameObject> adjacentElements, GameObject currentElement)
    {
        // Find the direction of the previous path element
        int currentRow = -1;
        int currentCol = -1;

        // Find the current element's row and column in the grid
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (grid[i, j] == currentElement)
                {
                    currentRow = i;
                    currentCol = j;
                    break;
                }
            }
        }

        GameObject sameDirectionElement = null;

        // Check if there's an adjacent element in the same direction
        if (currentRow > 0 && adjacentElements.Contains(grid[currentRow - 1, currentCol]))
        {
            sameDirectionElement = grid[currentRow - 1, currentCol]; // Up
        }
        else if (currentRow < rows - 1 && adjacentElements.Contains(grid[currentRow + 1, currentCol]))
        {
            sameDirectionElement = grid[currentRow + 1, currentCol]; // Down
        }
        else if (currentCol > 0 && adjacentElements.Contains(grid[currentRow, currentCol - 1]))
        {
            sameDirectionElement = grid[currentRow, currentCol - 1]; // Left
        }
        else if (currentCol < columns - 1 && adjacentElements.Contains(grid[currentRow, currentCol + 1]))
        {
            sameDirectionElement = grid[currentRow, currentCol + 1]; // Right
        }

        return sameDirectionElement;
    }

     void FindElementsAtDistance(int distance, List<GameObject> elementsList, Color color)
    {
        foreach (GameObject pathElement in pathElements)
        {
            int pathElementRow = -1;
            int pathElementCol = -1;

            // Find the row and column of the path element in the grid
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (grid[i, j] == pathElement)
                    {
                        pathElementRow = i;
                        pathElementCol = j;
                        break;
                    }
                }
            }

            if (pathElementRow == -1 || pathElementCol == -1)
            {
                continue; // Skip if the path element was not found in the grid
            }

            // Check non-path elements at the specified distance from the path element
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    GameObject currentElement = grid[i, j];

                    if (!pathElements.Contains(currentElement))
                    {
                        int currentDistance = Mathf.Abs(i - pathElementRow) + Mathf.Abs(j - pathElementCol);

                        if (currentDistance == distance)
                        {
                            if(!elementsList.Contains(currentElement))
                            {
                                elementsList.Add(currentElement);
                            }
                            currentElement.GetComponent<MeshRenderer>().material.color = color;
                        }
                    }
                }
            }
        }
    }

    void RemoveElementsInLowerTiers(List<GameObject> source, params List<GameObject>[] lowerTiers)
    {
        foreach (var lowerTier in lowerTiers)
        {
            source.RemoveAll(element => lowerTier.Contains(element));
        }
    }

    void RotateTiersToPath()
    {
        foreach (GameObject element in tier1Elements)
        {
            RotateToClosestPath(element);
        }

        foreach (GameObject element in tier2Elements)
        {
            RotateToClosestPath(element);
        }

        foreach (GameObject element in tier3Elements)
        {
            RotateToClosestPath(element);
        }

        foreach (GameObject element in tier4Elements)
        {
            RotateToClosestPath(element);
        }
    }

    void RotateToClosestPath(GameObject target)
    {
        if (pathElements.Count == 0)
        {
            Debug.LogError("No path squares assigned to the script.");
            return;
        }

        GameObject closestPathSquare = pathElements[0];
        float closestDistance = Vector3.Distance(target.transform.position, closestPathSquare.transform.position);

        // Find the closest path square
        foreach (GameObject pathSquare in pathElements)
        {
            float distance = Vector3.Distance(target.transform.position, pathSquare.transform.position);
            if (distance < closestDistance)
            {
                closestPathSquare = pathSquare;
                closestDistance = distance;
            }
        }

        // Calculate the rotation angle
        Vector3 lookDirection = closestPathSquare.transform.position - target.transform.position;
        lookDirection.y = 0; // Lock rotation to the x-z plane
        Quaternion rotation = Quaternion.LookRotation(lookDirection);

        // Calculate the rotation angle to make it 90 degrees
        float angle = Mathf.Round(rotation.eulerAngles.y / 90.0f) * 90.0f;
        rotation = Quaternion.Euler(0, angle, 0);

        // Apply the rotation
        target.transform.rotation = rotation;
    }
}
