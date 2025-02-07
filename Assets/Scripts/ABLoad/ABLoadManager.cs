using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AB包管理器-》只能对应一个主包
/// 
/// 
/// </summary>

public class ABLoadManager : Singleton<ABLoadManager>
{
    public Dictionary<string, AssetBundle> dic;
    //加载主包后，可以通过字典保存ab包
    AssetBundle mainAB = null;
    //主包
    AssetBundleManifest mainfest = null;
    //主包的依赖
    public string MainAB;
    //主包名



    private void Awake()
    {
        instance = this.GetComponent<ABLoadManager>();
        dic = new Dictionary<string, AssetBundle>();
    }

    private void Start()
    {
        InitSource();
    }


    /// <summary>
    /// streamingAssetsPath路径
    /// </summary>
    string sap
    {
        get { return Application.streamingAssetsPath + "/"; }
    }
    

    /// <summary>
    /// 加载ab包
    /// </summary>
    /// <param name="abname"></param>
    public void Load(string abname)
    {
        if(mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(sap + MainAB);
            mainfest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //加载依赖和主包

        
        string[] strs = mainfest.GetAllDependencies(abname);
        //获得要加载的ab包的依赖

        for(int i= 0; i < strs.Length; i++)
        {
            if (!dic.ContainsKey(strs[i]))
            {
                dic.Add(strs[i], AssetBundle.LoadFromFile(sap + strs[i]));
            }
        }
        //依次加载依赖

        if (!dic.ContainsKey(abname))
        {
            dic.Add(abname, AssetBundle.LoadFromFile(sap + abname));
        }
        //加载目标ab包
    }


    /// <summary>
    /// 获取某个实例
    /// </summary>
    /// <param name="abname"></param>
    /// <param name="objname"></param>
    /// <returns></returns>
    public GameObject Get(string abname,string objname)
    {
        Load(abname);

        return Instantiate(dic[abname].LoadAsset<GameObject>(objname));
    }


    /// <summary>
    /// 获取图片
    /// </summary>
    /// <param name="abname"></param>
    /// <param name="objname"></param>
    /// <returns></returns>
    public Sprite GetImage(string abname, string objname)
    {
        Load(abname);

        return dic[abname].LoadAsset<Sprite>(objname);
    }


    /// <summary>
    /// 卸载ab包
    /// </summary>
    /// <param name="abname"></param>
    public void UnLoad(string abname)
    {
        if (dic.ContainsKey(abname))
        {
            dic[abname].Unload(false);
            dic.Remove(abname);
        }
        //卸载ab包
    }


    /// <summary>
    /// 加载需要的图片，材质等资源
    /// </summary>
    public void InitSource()
    {
        Load("image");
        Load("material");
        Load("font");
        Load("model");
    }

}
