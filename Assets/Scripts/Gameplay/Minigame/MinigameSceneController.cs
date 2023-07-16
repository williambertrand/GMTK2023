using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameSceneController : MonoBehaviour
{
    public static MinigameSceneController Instance;
    public BeatScroller beatScroller;
    public HumanPrefabInfo[] humanPrefabs;
    public BaitInfo[] baitPrefabs;

    public Transform fisherSpawnPosition;

    public Transform poleTip;
    public Transform victoryPosition;
    
    public ButtonController leftButton;
    public ButtonController rightButton;

    private void Awake()
    {
        Instance = this;
    }

    public void Init(HumanType type, SongLevel songLevel, BaitType baitType){
        var human = GameObject.Instantiate(MinigameSceneController.Instance.humanPrefabs.Where(x => x.type == type).First().prefab);
        human.transform.position = MinigameSceneController.Instance.fisherSpawnPosition.position;
        MinigameSceneController.Instance.beatScroller.fisherman = human;
        MinigameSceneController.Instance.beatScroller.songLevel = songLevel;
        human.GetComponent<FishermanController>().fishingPolePosition = MinigameSceneController.Instance.poleTip;
        human.GetComponent<FishermanController>().victoryPosition = MinigameSceneController.Instance.victoryPosition;

        
        var bait = GameObject.Instantiate(MinigameSceneController.Instance.baitPrefabs.Where(x => x.type == baitType).First().prefab);
        bait.transform.parent = human.transform;
        bait.transform.position = human.GetComponent<FishermanController>().baitPosition.position;
    }

    void Start(){
        // MinigameSceneController.Instance.Init(HumanType.GoofyOrange, SongLevel.Normal, BaitType.HAMBURGER);
    }

    public void Finish(bool hasWon){
        SceneManager.GetSceneByName("GamePlayFinal").GetRootGameObjects().Where(x => x.gameObject.name == "GameManager").First().GetComponent<GamePlayManager>().ReturnFromMinigame(hasWon);
    }

}

[System.Serializable]
public struct HumanPrefabInfo
{
    public GameObject prefab;
    public HumanType type;
}

public enum HumanType
{
    GoofyOrange
}

[System.Serializable]
public struct BaitInfo
{
    public GameObject prefab;
    public BaitType type;
}
