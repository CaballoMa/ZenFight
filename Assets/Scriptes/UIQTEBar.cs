using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIQTEBar : MonoBehaviour
{
    [SerializeField] private float barVal;
    [SerializeField] private float validVal;
    [SerializeField] private float validExcuteVal;
    [SerializeField] private float speed;
    [SerializeField] public bool isMoving;
    [SerializeField] public bool isExcuteMoving;
    [SerializeField] private float sliderVal;
    [SerializeField] private float sliderExcuteVal;

    [SerializeField] private GameObject QTEObj;
    [SerializeField] private GameObject QTEExcuteBar;
    [SerializeField] private RectTransform pointTransform;
    [SerializeField] private RectTransform pointExcuteTransform;
    [SerializeField] private RectTransform validRangeTransform;
    [SerializeField] private RectTransform validExcuteRangeTransform;
    [SerializeField] private RectTransform qteBarTransform;

    [SerializeField] private float startPointPosRectX;
    [SerializeField] private float endPointPosRectX;

    [SerializeField] private float validLeft;
    [SerializeField] private float validRight;

    void Start()
    {
        isMoving = false;
        sliderVal = 0f;
        sliderExcuteVal = 0f;
        validLeft = barVal/2 - validVal/2;
        validRight = barVal/2 + validVal/2;
        validExcuteVal = validVal;
        //startPointPosRectX = qteBarTransform.position.x - qteBarTransform.localScale.x/2 * qteBarTransform.rect.width;
        //endPointPosRectX = qteBarTransform.position.x + qteBarTransform.localScale.x / 2 * qteBarTransform.rect.width;
        startPointPosRectX = -200f;
        endPointPosRectX = 200f;
        float validUIScaleX = (validVal/barVal) * qteBarTransform.localScale.x * qteBarTransform.rect.width/ validRangeTransform.rect.width;
        validRangeTransform.localScale = new Vector3(validUIScaleX, validRangeTransform.localScale.y, validRangeTransform.localScale.z);
        validExcuteRangeTransform.localScale = new Vector3(validUIScaleX, validRangeTransform.localScale.y, validRangeTransform.localScale.z);
        QTEObj.SetActive(false);
        QTEExcuteBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving)
        {
            sliderVal += Time.deltaTime * speed;
            if(sliderVal > barVal)
            {
                isMoving = false;
                sliderVal = 0f;
            }
        }

        if (isExcuteMoving)
        {
            sliderExcuteVal += Time.deltaTime * speed;
            if (sliderExcuteVal > barVal)
            {
                isExcuteMoving = false;
                sliderExcuteVal = 0f;
            }
        }

        pointTransform.anchoredPosition3D = new Vector3(startPointPosRectX + (sliderVal / barVal) * (endPointPosRectX - startPointPosRectX), pointTransform.anchoredPosition3D.y, pointTransform.anchoredPosition3D.z);
        pointExcuteTransform.anchoredPosition3D = new Vector3(startPointPosRectX + (sliderExcuteVal / barVal) * (endPointPosRectX - startPointPosRectX), pointExcuteTransform.anchoredPosition3D.y, pointExcuteTransform.anchoredPosition3D.z);
    }

    public bool CheckHit()
    {
        isMoving = false;
        if(sliderVal >= validLeft && sliderVal <= validRight)
        {
            sliderVal = 0;
            return true;
        }
        else
        {
            sliderVal = 0;
            return false;
        }
    }

    public bool CheckExcuteHit()
    {
        isExcuteMoving = false;
        if (sliderExcuteVal >= validLeft && sliderExcuteVal <= validRight)
        {
            sliderExcuteVal = 0;
            Debug.Log("Excute Success!");
            return true;
        }
        else
        {
            sliderExcuteVal = 0;
            Debug.Log("Excute Fail!");
            ExtendExcuteValidRange(1.5f);
            return false;
        }
    }

    private void ExtendExcuteValidRange(float time)
    {
        validExcuteVal *= time;
        if (validExcuteVal < 0) validExcuteVal = 0.01f;
        if(validExcuteVal > barVal) validExcuteVal = barVal;
        float validUIScaleX = (validExcuteVal / barVal) * qteBarTransform.localScale.x * qteBarTransform.rect.width / validRangeTransform.rect.width;
        validExcuteRangeTransform.localScale = new Vector3(validUIScaleX, validRangeTransform.localScale.y, validRangeTransform.localScale.z);
    }
}
