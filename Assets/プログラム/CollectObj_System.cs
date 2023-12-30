using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectObj_System : MonoBehaviour
{
    public enum CollectObj_Select { junk, ore }
    public CollectObj_Select cs;
    public string collect_text = "çÃéÊÇµÇ‹Ç∑Ç©";
    public float collect_time = 1;

    public int collect_item_id;
    public int collect_item_num;
    public void CollectObj_function()
    {
        switch (cs)
        {
            case CollectObj_Select.junk:
                Destroy(gameObject);
                break; 
            case CollectObj_Select.ore: 
                break;
        }
    }
}
