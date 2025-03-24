using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dummy : MonoBehaviour
{
    [SerializeField] int _maxHP = 100;
    [SerializeField] int _currentHP = 100;
    [SerializeField] Image _healthBar;
    [SerializeField] Image _healthBarBG;
    [SerializeField] private RectTransform _hitImage;
    [SerializeField] private GameObject _dummyPrefab;

    [SerializeField] private Collider _headCollider;
    [SerializeField] private Collider _bodyCollider;

    private Coroutine _coroutine;

    public void TakeDamage(int damage)
    {
        _currentHP -= damage;
        _healthBar.fillAmount = (float)_currentHP / _maxHP;
        if (_currentHP <= 0)
        {
            //Destroy the dummy with coroutine
            StartCoroutine(DestroyDummy());

        }
        ShowHit();
    }
    private void Update()
    {
        //HPBar follow the camera
        _healthBar.transform.LookAt(Camera.main.transform);
        _healthBarBG.transform.LookAt(Camera.main.transform);
    }
    private IEnumerator DestroyDummy()
    {
        //Play the death animation (not have)
        yield return new WaitForSeconds(0.5f);
        Dummy newDummy = Instantiate(_dummyPrefab, transform.position, transform.rotation).GetComponent<Dummy>();
        newDummy._currentHP = newDummy._maxHP;
        newDummy._healthBar.fillAmount = 1;
        newDummy._hitImage = this._hitImage;
        yield return new WaitForSeconds(0.01f);
        Destroy(gameObject);
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

    public bool IsHeadshot(Collider collider)
    {
        return collider == _headCollider;
    }
}
