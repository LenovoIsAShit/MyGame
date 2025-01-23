using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 负责网格初始化，静态方法(A*)计算最优路径
/// 
/// 
/// 属于全局方法，无需挂载
/// </summary>
public class grid : MonoBehaviour
{
    static bool[,] grids;
    //网格数组
    static GameObject[] obstacles;
    //障碍物
    static float minx, miny, maxx, maxy;
    //网格位置
    static int lenx, leny;
    //网格大小
    Transform edges;
    //区域标志

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
        //初始化障碍物

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
        }//默认false

        Transform cubes = edges.GetChild(4);
        int[] dlen = { -1, 0, 1 };
        //周围网格偏移量

        for (int i = 0; i < cubes.childCount; i++)
        {
            int tx = (int)(cubes.GetChild(i).transform.position.x - minx);
            int ty = (int)(cubes.GetChild(i).transform.position.z - miny);

            for (int j = 0; j < dlen.Length; j++)
            {
                for (int k = 0; k < dlen.Length; k++)
                {
                    Set_obstacle(tx + dlen[j], ty + dlen[k]);
                    //特殊处理 
                }
            }
        }

        var res = EnemyController.Get_enemies();
        for (int i = 0; i < res.Count; i++)
        {
            int tx = (int)(res[i].transform.position.x - minx);
            int ty = (int)(res[i].transform.position.z - miny);
            Set_obstacle(tx, ty);//表示此处不可通过
        }
    }
    //刷新grids

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
        //映射到网格的坐标

        float[,] G = new float[lenx, leny];
        float[,] H = new float[lenx, leny];
        bool[,] selected = new bool[lenx, leny];
        bool[,] oepn_selected = new bool[lenx, leny];//open list是否已经存在
        Vector3Int[,] fa = new Vector3Int[lenx, leny];//记录父亲

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
        //初始化网格

        open_list.Add(r_st);
        G[r_st.x, r_st.z] = 0;
        //把起点放进去

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
            //说明无法到达

            selected[remx,remy]=true;
            close_list.Add(new Vector3(remx+minx, st.y, remy+miny));
            //把此点加入close list

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int tx = remx + dlen[i], ty = remy + dlen[j];
                    if (dlen[i] == 0 && dlen[j] == 0) continue;
                    //如果是自身则跳过
                    if (tx < 0 || tx >= lenx || ty < 0 || ty >= leny) continue;
                    //外界点跳过
                    if (grids[tx, ty] == true) continue;
                    //障碍点跳过


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
                    //没选过才更新
                }
            }
            //处理周围节点

            if (Mathf.Abs(remx - r_ed.x) <= 1 && Mathf.Abs(remy - r_ed.z) <= 1)
            {
                fa[r_ed.x, r_ed.z] = new Vector3Int(remx, 0, remy);
                //设置结束路径
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
        res.Reverse();//倒转

        return res;
    }
    //返回所有路径点

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
    //检测是否在范围内
}
