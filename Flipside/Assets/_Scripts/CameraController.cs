using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float smoothSpeed = 0.125f;
    public float maxY = 5f;
    public float minY = -5f;

    public string ballTag = "Ball";
    private Transform ball;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        transform.position = pos;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (ball == null)
        {
            GameObject ballObj = GameObject.FindGameObjectWithTag(ballTag);
            if (ballObj != null)
            {
                ball = ballObj.transform;
            }
            else return;
        }
        float targetY = Mathf.Clamp(ball.position.y, minY, maxY);

        Vector3 desiredPosition = new Vector3(transform.position.x, targetY, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 0.2f);
    }
}
