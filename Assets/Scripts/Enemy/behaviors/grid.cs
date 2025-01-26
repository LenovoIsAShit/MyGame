using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������ʼ������̬����(A*)��������·��
/// 
/// 
/// ����ȫ�ַ������������
/// </summary>
public class grid : MonoBehaviour
{
    static bool[,] grids;
    //��������
    static GameObject[] obstacles;
    //�ϰ���
    static float minx, miny, maxx, maxy;
    //����λ��
    static int lenx, leny;
    //�����С
    Transform edges;
    //�����־

    void Start()
    {
        Init();
    }

    void FixedUpdate()
    {
        refresh_obs_and_enemies();
    }

    void Init()
    {
        int INF = 99999999;
        minx = INF;
        miny = INF;
        maxx = 0;
        maxy = 0;


        edges = GameObject.FindGameObjectWithTag("EdgePoints").transform;
        obstacles = new GameObject[edges.childCount];
        for(int i = 0; i < edges.childCount-1; i++)
        {
            obstacles[i]= edges.GetChild(i).gameObject;
            minx = Mathf.Min(obstacles[i].transform.position.x, minx);
            miny = Mathf.Min(obstacles[i].transform.position.z, miny);
            maxx = Mathf.Max(obstacles[i].transform.position.x, maxx);
            maxy = Mathf.Max(obstacles[i].transform.position.z, maxy);
        }
        //��ʼ���ϰ���

        lenx = (int)(maxx - minx);
        leny = (int)(maxy - miny);
        grids = new bool[lenx, leny];

        refresh_obs_and_enemies();
    }

    void refresh_obs_and_enemies()
    {
        for (int dx = 0; dx < lenx; dx++)
        {
            for (int dy = 0; dy < leny; dy++)
            {
                grids[dx, dy] = false;
            }
        }//Ĭ��false

        Transform cubes = edges.GetChild(4);
        int[] dlen = { -1, 0, 1 };
        //��Χ����ƫ����

        for (int i = 0; i < cubes.childCount; i++)
        {
            int tx = (int)(cubes.GetChild(i).transform.position.x - minx);
            int ty = (int)(cubes.GetChild(i).transform.position.z - miny);

            for (int j = 0; j < dlen.Length; j++)
            {
                for (int k = 0; k < dlen.Length; k++)
                {
                    Set_obstacle(tx + dlen[j], ty + dlen[k]);
                    //���⴦�� 
                }
            }
        }

        var res = EnemyController.Get_enemies();
        for (int i = 0; i < res.Count; i++)
        {
            int tx = (int)(res[i].transform.position.x - minx);
            int ty = (int)(res[i].transform.position.z - miny);
            Set_obstacle(tx, ty);//��ʾ�˴�����ͨ��
        }
    }
    //ˢ��grids

    void Set_obstacle(int tx,int ty)
    {
        if (tx < 0 || tx >= lenx || ty < 0 || ty >= leny)
        {
            return;
        }
        grids[tx, ty] = true;
    }

    public static List<Vector3> Astar(Vector3 st,Vector3 ed)
    {
        int inf = 9999999;

        List<Vector3Int> open_list = new List<Vector3Int>();
        List<Vector3> close_list = new List<Vector3>();

        Vector3Int r_st = new Vector3Int((int)Mathf.Max((st.x - minx), 0), 0, (int)Mathf.Max((st.z - miny), 0));
        if (grids[r_st.x, r_st.z] == true) grids[r_st.x, r_st.z] = false;
        Vector3Int r_ed = new Vector3Int((int)(ed.x - minx), 0, (int)(ed.z - miny));
        //ӳ�䵽���������

        float[,] G = new float[lenx, leny];
        float[,] H = new float[lenx, leny];
        bool[,] selected = new bool[lenx, leny];
        bool[,] oepn_selected = new bool[lenx, leny];//open list�Ƿ��Ѿ�����
        Vector3Int[,] fa = new Vector3Int[lenx, leny];//��¼����

        for(int i = 0; i < lenx; i++)
        {
            for(int j = 0; j < leny; j++)
            {
                H[i, j] = Mathf.Abs(r_ed.x - i) + Mathf.Abs(r_ed.z - j);
                G[i, j] = inf;
                selected[i, j] = false;
                oepn_selected[i, j] = false;
            }
        }
        //��ʼ������

        open_list.Add(r_st);
        G[r_st.x, r_st.z] = 0;
        //�����Ž�ȥ

        int[] dlen = { -1, 0, 1 };

        while (true)
        {
            float minf = inf;
            int remx = -1, remy = -1;
            for (int i = 0; i < open_list.Count; i++)
            {
                int tx = open_list[i].x, ty = open_list[i].z;
                if (selected[tx, ty] == false&& minf > G[tx, ty] + H[tx, ty])
                {
                    minf = G[tx, ty] + H[tx, ty];
                    remx = tx;
                    remy = ty;
                }
            }

            if (remx == -1)
            {
                return new List<Vector3>();
            }
            //˵���޷�����

            selected[remx,remy]=true;
            close_list.Add(new Vector3(remx+minx, st.y, remy+miny));
            //�Ѵ˵����close list

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int tx = remx + dlen[i], ty = remy + dlen[j];
                    if (dlen[i] == 0 && dlen[j] == 0) continue;
                    //���������������
                    if (tx < 0 || tx >= lenx || ty < 0 || ty >= leny) continue;
                    //��������
                    if (grids[tx, ty] == true) continue;
                    //�ϰ�������


                    if (selected[tx, ty] == false)
                    {
                        if (dlen[i] != 0 || dlen[j] != 0)
                        {
                            if(G[tx, ty]> G[remx, remy] + 1.414f)
                            {
                                G[tx, ty] = G[remx, remy] + 1.414f;
                                fa[tx, ty] = new Vector3Int(remx, 0, remy);
                            }
                        }
                        else
                        {
                            if (G[tx, ty] > G[remx, remy] + 1f)
                            {
                                G[tx, ty] = G[remx, remy] + 1f;
                                fa[tx, ty] = new Vector3Int(remx, 0, remy);
                            }
                        }

                        if (oepn_selected[tx, ty] == false)
                        {
                            open_list.Add(new Vector3Int(tx, (int)st.y, ty));
                            oepn_selected[tx, ty] = true;
                        }
                    }
                    //ûѡ���Ÿ���
                }
            }
            //������Χ�ڵ�

            if (Mathf.Abs(remx - r_ed.x) <= 1 && Mathf.Abs(remy - r_ed.z) <= 1)
            {
                fa[r_ed.x, r_ed.z] = new Vector3Int(remx, 0, remy);
                //���ý���·��
                break;
            }
        }

        List<Vector3> res = new List<Vector3>();
        int sx = r_ed.x, sy = r_ed.z;
        while (sx != r_st.x || sy != r_st.z)
        {
            res.Add(new Vector3(sx + minx, st.y, sy + miny));
            int tx = sx, ty = sy;
            sx = fa[tx, ty].x;
            sy = fa[tx, ty].z;

            if (sx == tx && sy == ty) break;
        }
        res.Reverse();//��ת

        return res;
    }
    //��������·����

    public static bool Edge_check(Vector3 pos)
    {
        if (pos.x < minx || pos.x > maxx || pos.z < miny || pos.z > maxy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //����Ƿ��ڷ�Χ��
}
