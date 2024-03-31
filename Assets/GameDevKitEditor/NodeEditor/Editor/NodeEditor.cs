

namespace GameDevKit.NodeEditor
{
  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

  public class NodeEditor : EditorWindow
  {
      private  List<Node> nodes; //节点列表
      public  List<Connection> connections; //连接列表
      
      public ConnectionPoint SelectingPoint; //记录正在选择的点，用于判断之后是否会发生连接
      public bool isRemoveConnectionMode; //标志着是否进入移除连线模式

      private Vector2 GridOffset; //画布网格的偏移

      //记录一些GUIStyle
      GUIStyle style_Node; //节点的GUI风格
      GUIStyle style_Node_select; //节点在选择情况下的GUI风格
      GUIStyle style_Point; //连接点的GUI风格


      private float scale = 1f; //缩放比例


      [MenuItem("Window/My Node Editor")]
      private static void OpenWindow()
      {
          NodeEditor window = GetWindow<NodeEditor>();
      }


      private void OnEnable()
      {
          //设置窗口的标题：
          titleContent = new GUIContent("My Node Editor");

          //创建节点列表对象
          nodes = new List<Node>();
          //创建连接列表对象
          connections = new List<Connection>();

          // //初始化一些GUIStyle
          style_Node = new GUIStyle();
          style_Node.SetNormalBackground(Color.grey);
          //alignment：	文本对齐方式
          style_Node.alignment = TextAnchor.MiddleCenter; //中心

          //节点在选择状态下的风格：
          style_Node_select = new GUIStyle();
          style_Node_select.SetNormalBackground(Color.green);
          style_Node_select.alignment = TextAnchor.MiddleCenter; //中心
          //fontStyle：   要使用的字体样式
          style_Node_select.fontStyle = FontStyle.Bold; //粗体

          //连接点的风格：
          style_Point = new GUIStyle();
          //normal：       正常显示组件时的渲染设置
          style_Point.SetNormalBackground(Color.gray);
          //active：       按下控件时的渲染设置。
          style_Point.SetActiveBackground(Color.green);
          //hover：        鼠标悬停在控件上时的渲染设置。
          style_Point.SetHoverBackground(Color.red);

      }



      //EditorWindow的接口OnGUI：绘制控件调用的接口
      private void OnGUI()
      {
          
          DrawToolBar();
          
          ScaleWithMouseWheel(Event.current); //处理鼠标滚轮缩放
          //绘制背景画布网格
          DrawGrid(20*scale, 0.2f, Color.gray);
          DrawGrid(100*scale, 0.4f, Color.gray);

          //绘制节点和连线
          DrawNodes();
          DrawConnections();


          //处理事件
          ProcessNodeEvents(Event.current); //先处理节点的
          ProcessEvents(Event.current); //再处理自身的
          //绘制待连接线
          DrawPendingConnection(Event.current);

          if (GUI.changed) //若GUI发生变化则重新绘制  
              Repaint();
      }






      #region 绘制方法


      private int toolbarSelection = 0;
      private Vector2 toolBarSize = new Vector2(200, 50);

      private void DrawToolBar()
      {
          //右侧
          Rect toolbarRect = new Rect(position.width - toolBarSize.x, 0, toolBarSize.x, toolBarSize.y);
          GUILayout.BeginArea(toolbarRect);
          if (GUILayout.Button("Save"))
          {
              SaveCurrentGraph();
          }

          GUILayout.EndArea();
      }

      public string path = "Assets/Config/DataBase/";
      public string Name = "NewNodeGraph";

      private void SaveCurrentGraph()
      {
          // FFile.CreatDirectoryIfEmpty(path);
          // var data = ScriptableObject.CreateInstance<NodeDataBase>();
          // data.Create(path, Name);
      }

      //绘制所有节点
      private void DrawNodes()
      {
          for (int i = 0; i < nodes.Count; i++)
              nodes[i].Draw();
      }

      //绘制所有的连线
      private void DrawConnections()
      {
          for (int i = 0; i < connections.Count; i++)
              connections[i].Draw();
      }

      //绘制待连接线
      private void DrawPendingConnection(Event e)
      {
          if (SelectingPoint != null) //如果已经选择了一个连接点，则画出待连接的线
          {
              //贝塞尔曲线的起点，根据已选则点的方向做判断：
              Vector3 startPosition = (SelectingPoint.type == ConnectionPointType.In)
                  ? SelectingPoint.rect.center
                  : e.mousePosition;
              Vector3 endPosition = (SelectingPoint.type == ConnectionPointType.In)
                  ? e.mousePosition
                  : SelectingPoint.rect.center;

              Handles.DrawBezier( //绘制通过给定切线的起点和终点的纹理化贝塞尔曲线
                  startPosition,
                  endPosition,
                  startPosition + Vector3.left * 50f, //startTangent	贝塞尔曲线的起始切线。
                  endPosition - Vector3.left * 50f, //endTangent	贝塞尔曲线的终点切线。
                  Color.white, //color	    要用于贝塞尔曲线的颜色。
                  null, //texture	要用于绘制贝塞尔曲线的纹理。
                  2f //width	    贝塞尔曲线的宽度。
              );

              GUI.changed = true;
          }
      }

      //绘制画布网格
      //gridSpacing：  格子间距
      //gridOpacity：  网格线不透明度
      //gridColor：    网格线颜色
      private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
      {
          //宽度分段
          int widthDivs = Mathf.CeilToInt( position.width/scale/ gridSpacing);
          //高度分段
          int heightDivs = Mathf.CeilToInt(position.height/scale / gridSpacing);

          Handles.BeginGUI(); //在 3D Handle GUI 内开始一个 2D GUI 块。
          {
              //设置颜色：
              Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

              //单格的偏移，算是GridOffset的除余
              Vector3 gridOffset = new Vector3(GridOffset.x % gridSpacing, GridOffset.y % gridSpacing, 0);

              //绘制所有的竖线
              for (int i = 0; i < widthDivs; i++)
              {
                  Handles.DrawLine(
                      new Vector3(gridSpacing * i, 0 - gridSpacing, 0) + gridOffset, //起点
                      new Vector3(gridSpacing * i, position.height + gridSpacing, 0f) + gridOffset); //终点
              }

              //绘制所有的横线
              for (int j = 0; j < heightDivs; j++)
              {
                  Handles.DrawLine(
                      new Vector3(0 - gridSpacing, gridSpacing * j, 0) + gridOffset, //起点
                      new Vector3(position.width + gridSpacing, gridSpacing * j, 0f) + gridOffset); //终点
              }

              //重设颜色
              Handles.color = Color.white;
          }
          Handles.EndGUI(); //结束一个 2D GUI 块并返回到 3D Handle GUI。
      }

      #endregion

      //处理事件
      private void ScaleWithMouseWheel(Event e)
      {
          if (e.control && e.type == EventType.ScrollWheel)
          {
              // 缩放比例范围为 0 - 2，其中 1 表示原始大小
              scale -= e.delta.y * 0.1f;
              scale = Mathf.Clamp(scale, 0.1f, 2f);
              Repaint(); // 重新绘制窗口，以便应用缩放变化。
          }

          // 应用缩放变化至 GUI 矩阵
          GUI.matrix = Matrix4x4.Scale(new Vector3(scale, scale, 1f));
      }

      private void ProcessEvents(Event e)
      {
          switch (e.type) //根据事件类型做判断
          {
              case EventType.MouseDown: //按下鼠标键
                  if (e.button == 1) //鼠标右键
                  {
                      //触发菜单
                      RightMouseMenu(e.mousePosition);
                  }

                  if (e.button == 0) //按下鼠标左键
                  {
                      SelectingPoint = null; //清空当前所选的连接点
                  }

                  break;
              case EventType.KeyDown: //按下键盘
                  if (e.keyCode == KeyCode.Y) //是Y键
                  {
                      isRemoveConnectionMode = true; //进入移除连线模式
                      GUI.changed = true; //提示需要刷新GUI
                  }

                  break;
              case EventType.KeyUp: //松开键盘
                  if (e.keyCode == KeyCode.Y) //是Y键
                  {
                      isRemoveConnectionMode = false; //离开移除连线模式
                      GUI.changed = true; //提示需要刷新GUI
                  }

                  break;
              case EventType.MouseDrag: //鼠标拖拽
                  if (e.button == 0) //鼠标左键
                  {
                      DragAllNodes(e.delta); //拖拽所有节点（拖拽画布）
                      GridOffset += e.delta; //增加画布网格的偏移
                      GUI.changed = true; //提示需要刷新GUI
                  }

                  break;
          }
      }

      //处理所有节点的事件
      private void ProcessNodeEvents(Event e)
      {
          //降序处理所有节点的事件（之所以降序是因为后画的节点将显示在更上层）
          for (int i = nodes.Count - 1; i >= 0; i--)
          {
              //处理每个节点的事件并看是否发生了拖拽
              bool DragHappend = nodes[i].ProcessEvents(e);
              //若发生了拖拽则提示GUI发生变化
              if (DragHappend)
                  GUI.changed = true;
          }
      }

      //鼠标右键菜单：
      private void RightMouseMenu(Vector2 mousePosition)
      {
          //创建菜单对象
          GenericMenu genericMenu = new GenericMenu();
          //菜单加一项 Add node
          genericMenu.AddItem(new GUIContent("Add node"), false, () => AddNode(mousePosition));
          //显示菜单
          genericMenu.ShowAsContext();
      }


      //处理添加节点
      private void AddNode(Vector2 nodePosition)
      {
          nodes.Add(new Node(nodePosition, this, style_Node, style_Node_select, style_Point));
      }

      //处理移除节点
      public void RemoveNode(Node node)
      {
          //收集“待删除连接列表”
          List<Connection> connectionsToRemove = new List<Connection>();

          //遍历所有的连接，若连接的入点或出点是属于要删除的节点的，则将其添加到“待删除连接列表”中
          for (int i = 0; i < connections.Count; i++)
          {
              if (connections[i].inPoint == node.inPoint || connections[i].outPoint == node.outPoint)
                  connectionsToRemove.Add(connections[i]);
          }

          //删除“待删除连接列表”中所有连接
          for (int i = 0; i < connectionsToRemove.Count; i++)
              connections.Remove(connectionsToRemove[i]);

          connectionsToRemove = null;

          //移除节点
          nodes.Remove(node);
      }

      //拖拽所有节点（拖拽画布）
      private void DragAllNodes(Vector2 delta)
      {
          for (int i = 0; i < nodes.Count; i++)
              nodes[i].ProcessDrag(delta);
      }


      public void Connect(ConnectionPoint point, ConnectionPoint other)
      {
          connections.Add(new Connection(point, other, this));
      }

      // public void Load(NodeDataBase nodeDataBase)
      // {
      //     throw new System.NotImplementedException();
      // }
  }

}