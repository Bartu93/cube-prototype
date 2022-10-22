using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Slider comboSlider;
    public float comboTimeAmount;
    public bool decreasingComboTime;
    public int comboAmount;
    public TMP_Text comboText;
    public GameObject starPrefab;
    public Transform starTextTransform;
    public TMP_Text starAmountText;
    public int starAmount;


    private static UIManager _ins;
    public static UIManager ins
    {
        get
        {
            return _ins;
        }
    }

    void Awake()
    {
        if (_ins == null)
            _ins = this;
    }

    private void Start()
    {
        comboSlider.value = comboSlider.minValue;
        comboSlider.maxValue = comboTimeAmount;
        comboText.text = "x" + " " + comboAmount.ToString();
        comboAmount = 1;
        starAmount = 0;
        starAmountText.text = starAmount.ToString();

    }

    public void OnCubesMatched()
    {
        comboSlider.value = comboTimeAmount;
        starAmount += comboAmount;
        for (int i = 0; i < comboAmount; i++)
        {
            InstantiateStars();
        }
        starAmountText.text = starAmount.ToString();
        comboAmount++;
        comboText.text = "x" + " " + comboAmount.ToString();
        decreasingComboTime = true;
    }

    void Update()
    {
        if (decreasingComboTime == true)
        {
            comboSlider.value = comboSlider.value - Time.deltaTime;
            if (comboSlider.value <= comboSlider.minValue)
            {
                decreasingComboTime = false;
                comboAmount = 1;
                comboText.text = "x" + " " + comboAmount.ToString();
            }
        }
    }

    void InstantiateStars()
    {
        Vector3 starSpawnPos = Random.insideUnitSphere * 30f + comboSlider.transform.position;
        starSpawnPos.z = 0;
        Instantiate(starPrefab, starSpawnPos, Quaternion.identity,gameObject.transform);
    }

    public void UndoPowerUpButtonPressed()
    {
        Deck.ins.UndoPowerUp();
    }

    public void HintPowerUpButtonPressed()
    {
        Deck.ins.HintPowerUp();
    }
}
