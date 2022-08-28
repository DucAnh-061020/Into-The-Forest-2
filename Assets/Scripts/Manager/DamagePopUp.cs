using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    private TextMeshPro textMeshPro;
    private float TTL;
    private const float DISAPPER_TIMER = 1f;
    private Color TextColor;
    private Vector3 moveVector;
    private static int SortOrder;
    private float decreaseScale = .5f;
    private float increaseScale = .5f;

    private void Awake()
    {
        textMeshPro = transform.GetComponent<TextMeshPro>();
    }

    public void Setup(float damageAmount, Color color = default)
    {
        textMeshPro.color = color;
        textMeshPro.SetText(damageAmount.ToString());
        TextColor = textMeshPro.color;
        TTL = DISAPPER_TIMER;
        moveVector = new Vector3(0f, 1f) * 15f;
        SortOrder++;
        textMeshPro.sortingOrder = SortOrder;
    }

    public static DamagePopUp Create(Vector3 pos,
                                     float damageAmount,
                                     Color color = default)
    {
        GameObject damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, pos, Quaternion.identity);
        DamagePopUp damagePopUp = damagePopupTransform.GetComponent<DamagePopUp>();
        damagePopUp.Setup(damageAmount,color);

        return damagePopUp;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;
        if (TTL > DISAPPER_TIMER / 2)
        {
            transform.localScale += Vector3.one * increaseScale * Time.deltaTime;
        }
        else
        {
            transform.localScale -= Vector3.one * decreaseScale * Time.deltaTime;
        }

        TTL -= Time.deltaTime;
        if (TTL < 0)
        {
            float despawnSpeed = 3f;
            TextColor.a -= despawnSpeed * Time.deltaTime;
            textMeshPro.color = TextColor;
            if (TextColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
