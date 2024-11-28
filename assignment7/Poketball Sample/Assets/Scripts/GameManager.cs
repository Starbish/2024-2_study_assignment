using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    UIManager MyUIManager;

    public GameObject BallPrefab;   // prefab of Ball

    // Constants for SetupBalls
    public static Vector3 StartPosition = new Vector3(0, 0, -6.35f);
    public static Quaternion StartRotation = Quaternion.Euler(0, 90, 90);
    const float BallRadius = 0.286f;
    const float RowSpacing = 0.02f;

    GameObject PlayerBall;
    GameObject CamObj;

    const float CamSpeed = 3f;

    const float MinPower = 15f;
    const float PowerCoef = 1f;

    void Awake()
    {
        // PlayerBall, CamObj, MyUIManager를 얻어온다.
        // ---------- TODO ---------- 
        if( !(PlayerBall = GameObject.Find("PlayerBall")))
            Debug.LogError("PlayerBall을 찾을 수 없음.");

        if( !(CamObj = Camera.main.gameObject) )
            Debug.LogError("Camera를 찾을 수 없음.");

        if( !(MyUIManager = FindObjectOfType<UIManager>()))
            Debug.LogError("UIManager를 찾을 수 없음.");

        // -------------------- 
    }

    void Start()
    {
        SetupBalls();
    }

    // Update is called once per frame
    void Update()
    {
        // 좌클릭시 raycast하여 클릭 위치로 ShootBallTo 한다.
        // ---------- TODO ----------
        if(Input.GetMouseButtonDown(0))
        {
            // 카메라의 포지션을 기준으로 Angle을 direction vector로 계산해야 할 줄 알았는데
            // transform.forward 가 이 기능을 하는 듯?
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                //Vector3 towards = new Vector3(hit.point.x, 0.0f, hit.point.z);
                ShootBallTo(hit.point);
                Debug.Log(hit.point);
            }
        }


        // -------------------- 
    }

    void LateUpdate()
    {
        CamMove();
    }

    void SetupBalls()
    {
        // 15개의 공을 삼각형 형태로 배치한다.
        // 가장 앞쪽 공의 위치는 StartPosition이며, 공의 Rotation은 StartRotation이다.
        // 각 공은 RowSpacing만큼의 간격을 가진다.
        // 각 공의 이름은 {index}이며, 아래 함수로 index에 맞는 Material을 적용시킨다.
        // Obj.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/ball_1");
        // ---------- TODO ----------
        GameObject[] balls = new GameObject[15];
        for(int i = 1; i <= 5; i++)
        {
            for(int j = 1; j <= i; j++)
            {
                // 공 인덱스 1 ~ 15
                int index = (i - 1) * i/2 + (j - 1);

                // 각 공에 적절한 위치를 계산한다
                float startValue = -(i - 1) / 2.0f + (j - 1);
                Vector3 height = - PlayerBall.transform.forward * (i-1) * (2 * BallRadius + RowSpacing);
                Vector3 width = PlayerBall.transform.right * startValue * (2 * BallRadius + RowSpacing);

                // 최종 위치를 계산한다
                Vector3 position = StartPosition + height + width;
                
                // 프리팹으로 생성
                balls[index] = Instantiate(BallPrefab, position, StartRotation);

                // 이름
                balls[index].name = $"{index+1}";

                // 텍스쳐
                balls[index].GetComponent<MeshRenderer>().material = Resources.Load<Material>($"Materials/ball_{index+1}");

                Debug.Log("볼이 생성됨");
            }
        }
        // -------------------- 
    }

    void CamMove()
    {
        // CamObj는 PlayerBall을 CamSpeed의 속도로 따라간다.
        // ---------- TODO ---------- 
        if (PlayerBall == null) return; // PlayerBall이 설정되지 않은 경우 종료

        // 현재 카메라 위치
        Vector3 currentPosition = CamObj.transform.position;

        // PlayerBall의 목표 위치
        Vector3 targetPosition = PlayerBall.transform.position;

        // 위치를 CamSpeed 속도로 이동
        CamObj.transform.position = Vector3.Lerp(currentPosition, targetPosition, CamSpeed * Time.deltaTime);
        CamObj.transform.position = new Vector3(CamObj.transform.position.x, 15.0f, CamObj.transform.position.z);
        // -------------------- 
    }

    float CalcPower(Vector3 displacement)
    {
        return MinPower + displacement.magnitude * PowerCoef;
    }

    void ShootBallTo(Vector3 targetPos)
    {
        // targetPos의 위치로 공을 발사한다.
        // 힘은 CalcPower 함수로 계산하고, y축 방향 힘은 0으로 한다.
        // ForceMode.Impulse를 사용한다.
        // ---------- TODO ---------- 
        Rigidbody rb = PlayerBall.GetComponent<Rigidbody>();
        Vector3 direction = targetPos - PlayerBall.transform.position;
        direction = new Vector3(direction.x, 0.0f, direction.z);
        Vector3 final = direction.normalized;
        rb.AddForce(final * CalcPower(final), ForceMode.Impulse);
        // -------------------- 
    }
    
    // When ball falls
    public void Fall(string ballName)
    {
        // "{ballName} falls"을 1초간 띄운다.
        // ---------- TODO ---------- 
        MyUIManager.DisplayText($"{ballName} falls", 1.0f);
        // -------------------- 
    }
}
