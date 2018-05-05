using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttackList : MonoBehaviour {

    public static AutoAttackList instance;

    public List<AutoAttackListItem> itemList = new List<AutoAttackListItem>();

    [SerializeField] GameObject itemPrefab;
    [SerializeField] UIGrid grid;

    private void Awake()
    {
        instance = this;
    }

    public void Refresh()
    {
        GeneralController.instance.listScroll.transform.localPosition = new Vector3(-9, 515, 0);
        GeneralController.instance.listScroll.GetComponent<UIPanel>().clipOffset = new Vector3(0, -400, 0);
        grid.Reposition();
    }

    public void CreateItem(MotionController mc)
    {
        AutoAttackListItem newItem = Instantiate(itemPrefab, transform).GetComponent<AutoAttackListItem>();
        newItem.SetContent(mc);
        newItem.transform.SetParent(transform);
        itemList.Add(newItem);

        Refresh();
        newItem.OnDestruct += grid.Reposition;
    }
}
