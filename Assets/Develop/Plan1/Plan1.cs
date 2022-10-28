using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Plan1 : MonoBehaviour
{
    public GameObject Tip;
    public Animator MuYuAnimator;
    private float _downTime;
    private Coroutine _loopPlayKnock;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Tip.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        playKnock();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            playKnock();
            _downTime = Time.time;
            if(_loopPlayKnock!=null)
            {
                StopCoroutine(_loopPlayKnock);
                _loopPlayKnock=null;
            }
        }

        if(Input.GetMouseButton(0) && Time.time-_downTime>1 && _loopPlayKnock==null)
        {
            _loopPlayKnock = StartCoroutine(loopPlayKnock());
        }
    }

    private IEnumerator loopPlayKnock()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f/2);
            playKnock();
        }
    }

    private void playKnock()
    {
        MuYuAnimator.gameObject.SetActive(false);
        MuYuAnimator.gameObject.SetActive(true);
        AudioManager.I.PlayKnock();
        var tip = GameObject.Instantiate(Tip,Tip.transform.parent);
        tip.SetActive(true);
        StartCoroutine(delayDestroy(tip));
    }

    private IEnumerator delayDestroy(GameObject tip)
    {
        yield return new WaitForSeconds(1f);
        GameObject.Destroy(tip);
    }

    public void OnPlayOM(bool isOn)
    {
        AudioManager.I.OnPlayOM(isOn);
    }
}
/*
沙弥(初入 草庵)
比丘(和尚 小庙 开始塑像1个)
主持(人多之后管事的 寺 塑佛 菩萨 罗汉 天尊)
方丈(精神领袖)
长老(退隐潜修)
佛(最高级)
*/
