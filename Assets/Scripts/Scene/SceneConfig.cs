using UnityEngine;

/*场景配置文件，专门用来配置场景中需要用到的参数以及文件路径*/
public class SceneConfig : MonoBehaviour 
{
    //场景的BGM文件名
    public string MusicPath;
    //场景的刷怪点：位置信息，怪物信息

    //场景中的事件

	void Start () 
    {
        //延迟0.5s播放新场景的BGM
        Invoke("PlayMusic", 0.5f);
	}
	
	//播放场景背景音乐方法
    void PlayMusic()
    {

    }
}
