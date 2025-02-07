using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AB��������-��ֻ�ܶ�Ӧһ������
/// 
/// 
/// </summary>

public class ABLoadManager : Singleton<ABLoadManager>
{
    public Dictionary<string, AssetBundle> dic;
    //���������󣬿���ͨ���ֵ䱣��ab��
    AssetBundle mainAB = null;
    //����
    AssetBundleManifest mainfest = null;
    //����������
    public string MainAB;
    //������



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
    /// streamingAssetsPath·��
    /// </summary>
    string sap
    {
        get { return Application.streamingAssetsPath + "/"; }
    }
    

    /// <summary>
    /// ����ab��
    /// </summary>
    /// <param name="abname"></param>
    public void Load(string abname)
    {
        if(mainAB == null)
        {
            mainAB = AssetBundle.LoadFromFile(sap + MainAB);
            mainfest = mainAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //��������������

        
        string[] strs = mainfest.GetAllDependencies(abname);
        //���Ҫ���ص�ab��������

        for(int i= 0; i < strs.Length; i++)
        {
            if (!dic.ContainsKey(strs[i]))
            {
                dic.Add(strs[i], AssetBundle.LoadFromFile(sap + strs[i]));
            }
        }
        //���μ�������

        if (!dic.ContainsKey(abname))
        {
            dic.Add(abname, AssetBundle.LoadFromFile(sap + abname));
        }
        //����Ŀ��ab��
    }


    /// <summary>
    /// ��ȡĳ��ʵ��
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
    /// ��ȡͼƬ
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
    /// ж��ab��
    /// </summary>
    /// <param name="abname"></param>
    public void UnLoad(string abname)
    {
        if (dic.ContainsKey(abname))
        {
            dic[abname].Unload(false);
            dic.Remove(abname);
        }
        //ж��ab��
    }


    /// <summary>
    /// ������Ҫ��ͼƬ�����ʵ���Դ
    /// </summary>
    public void InitSource()
    {
        Load("image");
        Load("material");
        Load("font");
        Load("model");
    }

}
