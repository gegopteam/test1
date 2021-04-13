using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;
/// <summary>
/// 拖拽的Item
/// </summary>
public class ScrollableCell : MonoBehaviour
{

    protected ScrollableAreaController controller = null;//controller   
    protected float cellHeight;
    protected float cellWidth;
    protected bool deactivateIfNull = true;//是否在为null的时候禁用
    protected ScrollableCell parentCell;

    protected System.Object dataObject = null;
    public System.Object DataObject
    {
        get { return dataObject; }
        set
        {
            dataObject = value;
            ConfigureCell();//配置Cell数据
        }
    }

    private int dataIndex;//索引
    public int DataIndex
    {
        get { return dataIndex; }
    }
    //初始化
    public virtual void Init(ScrollableAreaController controller, System.Object data, int index, float cellHeight = 0f, float cellWidth = 0f, ScrollableCell parentCell = null)
    {
        this.controller = controller;
        this.dataObject = data;
        this.dataIndex = index;
        this.cellHeight = cellHeight;
        this.cellWidth = cellWidth;
        this.parentCell = parentCell;

        if (deactivateIfNull)
        {
            if (data == null)
                this.gameObject.SetActive(false);
            else
                this.gameObject.SetActive(true);
        }
    }
    //配置数据
    public void ConfigureCell()
    {
        ConfigureCellData();
    }
    public virtual void ConfigureCellData()
    { }
}
