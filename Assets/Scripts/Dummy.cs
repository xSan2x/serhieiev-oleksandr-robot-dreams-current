using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dummy : MonoBehaviour
{
    [SerializeField] int _maxHP = 100;
    [SerializeField] int _currentHP = 100;
    [SerializeField] Image _healthBar;
    [SerializeField] private RectTransform _hitImage;

    private Coroutine _coroutine;

    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        _healthBar.fillAmount = (float)_currentHP / _maxHP;
        if (_currentHP <= 0)
        {
            Destroy(gameObject);
        }
        ShowHit();
    }

    public void PlayHeadshotSound()
    {
        GetComponent<AudioSource>().Play();
    }

    public void ShowHit()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        _coroutine = StartCoroutine(ShowHitImage());
    }
    private IEnumerator ShowHitImage()
    {
        _hitImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _hitImage.gameObject.SetActive(false);
    }
}
