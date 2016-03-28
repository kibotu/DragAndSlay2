using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controls
{
  public class OvrInput : MonoBehaviour
  {

    [SerializeField] TextMesh _text;

    void Start()
    {
      OVRTouchpad.Create();
      OVRTouchpad.TouchHandler += HandleTouchHandler;
    }

    private void HandleTouchHandler(object sender, EventArgs e)
    {
      var touchArgs = (OVRTouchpad.TouchArgs) e;
      if (touchArgs.TouchType == OVRTouchpad.TouchEvent.SingleTap)
      {
        //TODO: Insert code here to handle a single tap.  Note that there are other TouchTypes you can check for like directional swipes, but double tap is not currently implemented I believe.
        Debug.Log("Received SingleTap");
        _text.text = "singletap";
      }
      else
      {
        Debug.Log("Received " + e);
        _text.text = touchArgs.TouchType.ToString();
      }
    }
  }
}