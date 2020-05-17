using UnityEngine;

public class MyCamera : MonoBehaviour
{
    public GameObject Target;//锁定的目标
    public float ZoomSpeed;//镜头缩放速率
    public float MovingSpeed;//镜头移动速率
    public float Rotatespeed;//镜头旋转速率

    public float distance;//距离目标的距离
    public float offsetDistance=2f;//y轴上的偏移量
    public float ViewAngle;//镜头俯视的角度
    void Start()
    {
        ResetView();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX, deltaY;
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        if (Target)
        {
            if (Input.GetAxis("Mouse ScrollWheel")!=0)//滚轮
            {
                distance += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
            }
            if (Input.GetMouseButton(0))//鼠标左键旋转视野
            {
                deltaX = x * Rotatespeed;
                deltaY = y * Rotatespeed;
                transform.Rotate(0, deltaX, 0, Space.World);
                transform.Rotate(-deltaY, 0, 0);
            }
            else//松开左键回到原来位置
            {
                transform.rotation = Quaternion.Slerp
                    (transform.rotation, Quaternion.Euler(ViewAngle, Target.transform.rotation.eulerAngles.y, 0), Time.deltaTime * 2);
            }
            if (Input.GetMouseButton(1))
            {
                deltaX = x * Rotatespeed;
                deltaY = y * Rotatespeed;
                transform.Rotate(0, deltaX, 0, Space.World);
                transform.Rotate(-deltaY, 0, 0);

                Target.transform.Rotate(0, deltaX, 0);
            }
            
            Vector3 temp = transform.rotation * new Vector3(0, 0, -distance) + Target.transform.position;
            temp.y += offsetDistance;
            transform.position = temp;

        }

    }
    void ResetView()
    {
        if (Target)
        {
            transform.rotation = Quaternion.Euler(ViewAngle, Target.transform.rotation.eulerAngles.y, 0);
            Vector3 temp = transform.rotation * new Vector3(0, 0, -distance) + Target.transform.position;
            temp.y += offsetDistance;
            transform.position = temp;
        }
    }
}
