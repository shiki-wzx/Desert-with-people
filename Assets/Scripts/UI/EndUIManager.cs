using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class EndUIManager : MonoBehaviour {
    [SerializeField] private Image blackMask;
    [SerializeField] private GameObject ed1, ed2, credit;


    private IEnumerator ToLight() {
        const float speed = .5f;
        var color = blackMask.color;
        while(color.a > 1e-5) {
            color.a -= speed * Time.deltaTime;
            color.a = Mathf.Clamp01(color.a);
            blackMask.color = color;
            yield return null;
        }
    }


    private IEnumerator ToDark() {
        const float speed = .5f;
        var color = blackMask.color;
        while(1 - color.a > 1e-5) {
            color.a += speed * Time.deltaTime;
            color.a = Mathf.Clamp01(color.a);
            blackMask.color = color;
            yield return null;
        }
    }


    private IEnumerator PlayED() {
        yield return new WaitForSeconds(1);
        ed1.SetActive(true);
        yield return StartCoroutine(ToLight());
        yield return new WaitForSeconds(6);
        yield return StartCoroutine(ToDark());
        ed1.SetActive(false);

        yield return new WaitForSeconds(1);
        ed2.SetActive(true);
        yield return StartCoroutine(ToLight());
        yield return new WaitForSeconds(6);
        yield return StartCoroutine(ToDark());
        ed2.SetActive(false);

        yield return new WaitForSeconds(1);
        credit.SetActive(true);
        StartCoroutine(ToLight());
    }


    private void Start() {
        credit.GetComponentInChildren<Button>().onClick.AddListener(Application.Quit);

        ed2.SetActive(false);
        credit.SetActive(false);
        blackMask.color = Color.black;
        StartCoroutine(PlayED());
    }
}
