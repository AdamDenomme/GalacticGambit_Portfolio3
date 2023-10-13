using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Navigation : MonoBehaviour
{
    [SerializeField] Image neutralWaypoint;

    [SerializeField] List<navigationMenuColumn> columns;
    [SerializeField] List<Image> waypointsSpawned;
    public int currentWaypointIndex = 0;
    bool updateWayPoint;
    // Start is called before the first frame update
    void Start()
    {
        updateWayPoint = true;
        levelGeneration.instance.nextEncounter();
    }

    // Update is called once per frame
    void Update()
    {
        if(updateWayPoint)
        {
            waypointsSpawned[currentWaypointIndex].color = new Color(120, 151, 255, 255);
            updateWayPoint = false;
        }
    }

    public void nextWaypoint()
    {
        currentWaypointIndex++;
        updateWayPoint = true;
    }

    public void generateNavigationMenu()
    {
        levelGeneration.instance.selectLevel();

        for(int i = 0; i <= levelGeneration.instance.encounters.Count-1; i++)
        {
            int height = Random.Range(0, 6);

            columns[i].options[height].gameObject.SetActive(true);
            waypointsSpawned.Add(columns[i].options[height]);

        }

        for(int i = 0; i <= waypointsSpawned.Count; i++)
        {
            GameObject line = new GameObject("Line");
            line.transform.parent = transform;

            RectTransform rectTransform = line.AddComponent<RectTransform>();
            Image image = line.AddComponent<Image>();

            Vector3 direction = waypointsSpawned[i + 1].transform.position - waypointsSpawned[i].transform.position;
            float distance = direction.magnitude;
            direction.Normalize();

            rectTransform.sizeDelta = new Vector2(distance, 2f);
            rectTransform.position = waypointsSpawned[i].transform.position + direction * distance * 0.5f;
            rectTransform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            image.color = Color.green;
        }
    }
}
