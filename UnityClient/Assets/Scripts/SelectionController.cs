using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts
{
  public class SelectionController : MonoBehaviour
  {
    [SerializeField] readonly List<GameObject> _selected = new List<GameObject>();

    void Start()
    {
    }

    void Update()
    {
      if (Input.GetMouseButtonDown(0))
      {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100.0f))
        {
          var island = hit.transform.parent.parent;
          StartCoroutine(Enlarge(island, new Vector3(1.5f, 1.5f, 1.5f), 0.25f));
          Debug.Log("You selected the " + hit.transform.parent.name);
        }
      }
    }

    IEnumerator Enlarge(Transform target, Vector3 toScale, float duration)
    {
      var elapsedTime = 0f;
      var startingScale = target.localScale;
      var currentScale = new Vector3();
      while (elapsedTime < duration)
      {
        currentScale.x = Mathf.Lerp(startingScale.x, toScale.x, Easing.Circular.easeOut(elapsedTime/duration));
        currentScale.y = Mathf.Lerp(startingScale.y, toScale.y, Easing.Circular.easeOut(elapsedTime/duration));
        currentScale.z = Mathf.Lerp(startingScale.z, toScale.z, Easing.Circular.easeOut(elapsedTime/duration));

        target.localScale = currentScale;

        elapsedTime += Time.deltaTime;
        yield return new WaitForEndOfFrame();
      }
    }

    IEnumerator Shrink(Transform objTr)
    {
      objTr.localScale /= 1.2f;
      yield return new WaitForEndOfFrame();
    }
  }
}