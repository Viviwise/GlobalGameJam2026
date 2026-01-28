using UnityEngine;
using UnityEngine.UI;

public class SwitchLightListener : MonoBehaviour
{
  public SpriteRenderer lightPanel;

  void Start()
  {
      lightPanel.enabled = false;
      lightPanel = GetComponent<SpriteRenderer>();
  }
  public void LightUp()
  {
      lightPanel.enabled = true;
  }
}
