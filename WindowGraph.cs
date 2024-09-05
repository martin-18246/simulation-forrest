using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] private float xGraphMaximum = 1500f; // maximum x position on the screen
    [SerializeField] private float yGraphMaximum = 500f; // maximum y position on the screen

    [SerializeField] private float distanceAxisX = 100f;
    [SerializeField] private float distanceAxisY = 100f;


    public List<Snapshot> snapshots = new List<Snapshot>();


    [SerializeField] private float snapshotInterval = 0.5f;
    [SerializeField] private float startTime;
    [SerializeField] private float lastSnapshotTime;
    [SerializeField] private float offset = 5;

    private List<GameObject> graphShapes;

    private RectTransform graphContainer;


    public bool showNumRabbits = false;
    public bool showNumFoxes = false;
    public bool showNumFood = false;
    public bool showAverageSpeedRabbits = false;
    public bool showAverageSpeedFoxes = false;

    public Color black = new Color(0, 0, 0, 0.8f);
    public Color red = new Color(1, 0.2f, 0.2f, 0.8f);
    public Color blue = new Color(0.2f, 0.2f, 1, 0.8f);
    public Color green = new Color(0.2f, 1, 0.2f, 0.8f);
    public Color cyan = new Color(0, 1f, 1f, 0.8f);
    public Color yellow = new Color(1, 0.5f, 0.5f);

    public float maximumValueInVisibleLists = 50;


    private void Awake()
    {

        graphShapes = new List<GameObject>();
        lastSnapshotTime = -snapshotInterval;
        snapshots = new List<Snapshot>();
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();


        startTime = Time.time;

    }

    private void RestartContainer()
    {
        foreach (var item in graphShapes)
        {
            Destroy(item);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) showNumRabbits = showNumRabbits ? false : true;
        if (Input.GetKeyDown(KeyCode.Alpha1)) showNumFoxes = showNumFoxes ? false : true;
        if (Input.GetKeyDown(KeyCode.Alpha2)) showNumFood = showNumFood ? false : true;
        if (Input.GetKeyDown(KeyCode.Alpha3)) showAverageSpeedRabbits = showAverageSpeedRabbits ? false : true;
        if (Input.GetKeyDown(KeyCode.Alpha4)) showAverageSpeedFoxes = showAverageSpeedFoxes ? false : true;


        if (Time.time - lastSnapshotTime > snapshotInterval)
        {
            Snapshot snapshot = DataCollector.MakeSnapshot();
            snapshots.Add(snapshot);
            lastSnapshotTime = Time.time;
            RestartContainer();

            ShowVisibleGraphs();
        }

        if (Time.time - startTime > 20) snapshotInterval = 5f;

    }

    private void ShowVisibleGraphs()
    {
        maximumValueInVisibleLists = 1;

        if (showNumFood) ShowGraph(cyan, InfoPoint.NumFood);
        if (showNumRabbits) ShowGraph(black, InfoPoint.NumRabbits);
        if (showNumFoxes) ShowGraph(red, InfoPoint.NumFoxes);
        if (showAverageSpeedRabbits) ShowGraph(green, InfoPoint.AverageSpeedRabbit);
        if (showAverageSpeedFoxes) ShowGraph(blue, InfoPoint.AverageSpeedFox);
        yGraphMaximum = maximumValueInVisibleLists;

        if (showNumFood || showNumRabbits || showNumFoxes || showAverageSpeedFoxes || showAverageSpeedRabbits)
        {
            CreateDotConnection(Vector2.zero, new Vector2(distanceAxisX, 0), black, offset);
            CreateDotConnection(Vector2.zero, new Vector2(0, distanceAxisY), black, offset);
        }

    }



    private void ShowGraph(Color color, InfoPoint infoPoint)
    {
        float graphHeight = graphContainer.sizeDelta.y;

        float xSize = 50f;

        Vector2? lastPositionObject = null;

        for (int i = 0; i < snapshots.Count; i++)
        {
            if (snapshots.Count * xSize > xGraphMaximum) xSize = xGraphMaximum / snapshots.Count;


            float xPosition = xSize + i * xSize;
            float traitValue = snapshots[i].GetTrait(infoPoint);
            if (traitValue > maximumValueInVisibleLists) { maximumValueInVisibleLists = traitValue; }; 
            float yPosition = (snapshots[i].GetTrait(infoPoint) / yGraphMaximum) * graphHeight;

            Vector2 positionObject = new Vector2(xPosition, yPosition);

            if (lastPositionObject != null)
            {
                CreateDotConnection(lastPositionObject.Value, positionObject, color, 0);
            }

            lastPositionObject = positionObject;
        }

    }


    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB, Color color, float offset1)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        graphShapes.Add(gameObject);

        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = color;

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 direction = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);


        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + direction * distance * 0.5f + offset1 * new Vector2(1, 0);
        rectTransform.localEulerAngles = new Vector3(0, 0, Utilities.GetAngleFromVectorFloat(direction));

    }
}
