using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
  public class SelectionController : MonoBehaviour
  {
    [SerializeField] readonly List<GameObject> _selected = new List<GameObject>();

    void Start()
    {
    }

    void Update()
    {

      if ( Input.GetMouseButtonDown (0)){
         RaycastHit hit;
         Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         if ( Physics.Raycast (ray,out hit,100.0f)) {
           StartCoroutine(ScaleMe(hit.transform.parent));
           Debug.Log("You selected the " + hit.transform.parent.name);
         }
       }


    }

    IEnumerator ScaleMe(Transform objTr) {
        objTr.localScale *= 1.2f;
        yield return new WaitForSeconds(0.5f);
        objTr.localScale /= 1.2f;
    }
  }
}