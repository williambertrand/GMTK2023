using UnityEngine;

public class SpriteVisual : MonoBehaviour
{
    public Human human;
    [SerializeField] private float _rotXFactor = 1; 

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x * _rotXFactor, Camera.main.transform.rotation.eulerAngles.y, 0f);
    }

    public void ResumeWalking(){
        human.ResumeWalking();
    }
}
