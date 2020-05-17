using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterScript : MonoBehaviour
{
    public void OnBtnClick()
    {
        SceneController.Instance.LoadScene(SceneType.Demo1);
    }
}
