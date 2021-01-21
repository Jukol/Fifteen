using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField]
    Animator anim;
    public void MoveIn()
    {
        anim.SetTrigger("In");
    }

    public void MoveOut()
    {
        anim.SetTrigger("Out");
        //StartCoroutine(PauseAndCloseSettings());
    }

    IEnumerator PauseAndCloseSettings()
    {
        yield return new WaitForSeconds(1.0f);
        this.gameObject.SetActive(false);
    }
}
