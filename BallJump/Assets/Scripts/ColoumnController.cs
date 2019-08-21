using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoumnController : MonoBehaviour
{
    private Vector2 lastTapPos;
    private Vector3 startRotation;

    public Transform topTransform;
    public Transform finishTransform;
    public GameObject platePrefab;

    public List<Stage> allStages = new List<Stage>();
    private float coloumnLength;
    private List<GameObject> spawnedLevels = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        startRotation = transform.localEulerAngles;
        coloumnLength = topTransform.localPosition.y - (finishTransform.localPosition.y + 0.1f);
        LoadStage(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 curTapPos = Input.mousePosition;
            if (lastTapPos == Vector2.zero)
            {
                lastTapPos = curTapPos;
            }
            float delta = lastTapPos.x - curTapPos.x;
            lastTapPos = curTapPos;

            transform.Rotate(Vector3.up * delta);
        }
        if (Input.GetMouseButtonUp(0))
        {
            lastTapPos = Vector2.zero;
        }
    }

    public void LoadStage(int stageNumber)
    {
        Stage stage = allStages[Mathf.Clamp(stageNumber, 0, allStages.Count - 1)];
        if (stage == null)
        {
            Debug.LogError("No stage " + stageNumber + "found in list of stages.");
            return;
        }
        //change camer BGC
        Camera.main.backgroundColor = allStages[stageNumber].backGroundColor;
        //change ball color
        FindObjectOfType<BallController>().GetComponent<Renderer>().material.color = allStages[stageNumber].ballColor;
        //reset rotation of coloumn
        transform.localEulerAngles = startRotation;

        foreach (GameObject go in spawnedLevels)
            Destroy(go);

        //create new lvl and go
        float levelLenght = coloumnLength / stage.levels.Count;
        float spawnPosY = topTransform.localPosition.y;

        for (int i = 0; i < stage.levels.Count; i++)
        {
            spawnPosY -= levelLenght;
            GameObject level = Instantiate(platePrefab, transform);
            Debug.Log("Levels spawned");
            level.transform.localPosition = new Vector3(0, spawnPosY, 0);
            spawnedLevels.Add(level);

            #region gaps
            int partsToDisable = 12 - stage.levels[i].partCount;
            List<GameObject> disabledParts = new List<GameObject>();

            while (disabledParts.Count < partsToDisable)
            {
                GameObject randomPart = level.transform.GetChild(Random.Range(0, level.transform.childCount)).gameObject;
                if (!disabledParts.Contains(randomPart))
                {
                    randomPart.SetActive(false);
                    disabledParts.Add(randomPart);
                }
            }
            #endregion

            List<GameObject> leftParts = new List<GameObject>();
            foreach(Transform t in level.transform)
            {
                t.GetComponent<Renderer>().material.color = allStages[stageNumber].triangleColor;
                if (t.gameObject.activeInHierarchy)
                    leftParts.Add(t.gameObject);
            }

            #region deathparts
            List<GameObject> deathParts = new List<GameObject>();

            while (deathParts.Count < stage.levels[i].deathPartCount)
            {
                GameObject randomPart = leftParts[(Random.Range(0, leftParts.Count))];
                if (!deathParts.Contains(randomPart))
                {
                    randomPart.gameObject.AddComponent<DeathPart>();
                    deathParts.Add(randomPart);
                }
            }
            #endregion
        }
    }


}
