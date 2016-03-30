using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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
          if (!_isEnlarging)
            StartCoroutine(Wiggle(island, new Vector3(1.5f, 1.5f, 1.5f), .25f));
          Debug.Log("You selected the " + hit.transform.parent.name);
        }
      }
    }

    private bool _isEnlarging = false;

    private IEnumerator Wiggle(Transform target, Vector3 toScale, float duration)
    {
      _isEnlarging = true;

      var elapsedTime = 0f;
      var startingScale = target.localScale;
      var currentScale = new Vector3();
      while (elapsedTime < duration)
      {
        currentScale.x = Mathf.Lerp(startingScale.x, toScale.x, Easing.Wiggle.easeIn(elapsedTime/duration/2));
        currentScale.y = 1.0f/currentScale.x;
        currentScale.z = 1.0f/currentScale.y;

        target.localScale = currentScale;

        elapsedTime += Time.deltaTime;
        yield return new WaitForEndOfFrame();
      }

      toScale = startingScale;
      elapsedTime = 0f;
      currentScale = new Vector3();
      while (elapsedTime < duration)
      {
        currentScale.x = Mathf.Lerp(startingScale.x, toScale.x, Easing.Wiggle.easeIn(elapsedTime/duration/2));
        currentScale.y = 1.0f/currentScale.x;
        currentScale.z = 1.0f/currentScale.y;

        target.localScale = currentScale;

        elapsedTime += Time.deltaTime;
        yield return new WaitForEndOfFrame();
      }

      target.localScale = startingScale;

      _isEnlarging = false;
    }
  }
}