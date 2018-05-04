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

    public void CreateItem(MotionController mc)
    {
        AutoAttackListItem newItem = Instantiate(itemPrefab, transform).GetComponent<AutoAttackListItem>();
        newItem.SetContent(mc);
        newItem.transform.SetParent(transform);
        itemList.Add(newItem);

        grid.Reposition();
        newItem.OnDestruct += grid.Reposition;
    }
}
