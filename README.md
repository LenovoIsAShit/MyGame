这是我的第一个项目，目前正在维护更新中，主要有单人模式和多人联机模式(低优先)，
玩家模型资源来自Unity免费商店，其他都是自己拼的，这个游戏demo会长期更新:smile::grinning_face_with_big_eyes:

[2025/2/9 上传视频]<br>https://www.bilibili.com/video/BV1gFN7evEFr/?spm_id_from=333.1387.homepage.video_card.click&vd_source=142e89f3ee527e4138ae6596aa56e322<br>
<br>//////////////////////////////////////////////////////   日志   ///////////////////////////////////////////////////////<br><br>


  [2024/12/18] <br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;完成单人模式和多人联机基本结构<br>
  
  [2024/12/22]<br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;增加血条系统，以及血条增减动态动画<br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;增加敌人，以及敌人AI行为树<br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;不使用WLAN网卡进入多人联机不会再报错，但是不会搜索到房间<br>
![enemy](https://github.com/user-attachments/assets/99f54f88-ddb6-4061-b7bb-59f5ed96b002)

  [2025/1/2]<br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;拓展行为树，在已经有日常巡逻状态的行为树上，增加基于A*的追踪玩家状态

  
  [2025/1/4]<br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;增加基于工厂模式和多态设计的子弹类型<br>
  ![57BD1EFF92EA95EDC5FFC4FD8E30F8CE](https://github.com/user-attachments/assets/65b4ceba-2e19-47da-bce3-7db73a8c0618)

  [2025/1/10]<br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;增加子弹爆炸效果和脚底方向指针，现在点击鼠标左键向该方向发射子弹<br>
  ![401903288-8dcba040-f805-498f-918f-5d031e61891b](https://github.com/user-attachments/assets/d31016e9-efdc-490c-8fc3-48c4217c0a43)
  ![401903297-1727ba93-ee21-44ce-83ed-b4f074e6efeb](https://github.com/user-attachments/assets/7e536cd1-3238-4701-a3e2-c8bd84e22462)<br>

  [2025/1/27]<br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;新增UImanager基于单例模式管理所有UI界面<br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;新增对象池管理，回收包括子弹，敌人等对象便于重复使用<br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;新增背包系统，包括"物品栏"，“子弹库”，展示当前使用主子弹框和其他三个道具框<br>
  ![406748397-88709c50-9a17-4ca1-b475-86cd2778d64c](https://github.com/user-attachments/assets/060ab5e3-9a57-4b34-8c88-7e6458b37a5c)


  [2025/1/27]<br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;新增关卡流程控制和关卡过度动画(杀光当前所有怪物触发)<br>

  [2025/2/7]<br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;新增怪物生成过度动画<br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;更改AssetBundle资源管理方式，增加ABmanager脚本管理图片，模型，预制件等资源<br>

  [2025/2/9]<br>
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;加入了一个超级酷的BOSS战<br>
  ![4844E3849E68D1D793D978A7DAA249C0](https://github.com/user-attachments/assets/03a658c2-6e4f-4803-9377-1ae2446ce22c)



