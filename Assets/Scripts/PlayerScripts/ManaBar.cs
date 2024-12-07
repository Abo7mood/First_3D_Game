using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    [SerializeField] private float TiredTime = 2f;
    [SerializeField] private float ReginTime = 2f;
    private float BarmaskWidth;
    public bool Tire;
    public bool CanRun = true;
    //public RectTransform EdgeTransform1;
    //public RectTransform EdgeTransform2;
    //public RectTransform EdgeTransform3;
    PlayerMovement playerMovement;
    private Mana mana;
    public Image barImage;
    public RawImage coolImage;
    // Start is called before the first frame update
    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        mana = new Mana();


        //BarmaskWidth=EdgeTransform1.sizeDelta.x;



    }
    IEnumerator ManaRegin()
    {
        yield return new WaitForSeconds(ReginTime);
        if (barImage.fillAmount < .01)
        {

            Tire = true;
            yield return new WaitForSeconds(1f);
            Tire = false;

        }

    }
    IEnumerator ManaRegin2()
    {
        yield return new WaitForSeconds(ReginTime);
        if (barImage.fillAmount < .075)
        {
            CanRun = false;

            yield return new WaitForSeconds(TiredTime);
            CanRun = true;
        }

    }
    void manareg()
    {
    }
    private void Update()
    {

        StartCoroutine(ManaRegin());
        StartCoroutine(ManaRegin2());


        if (Tire == false && playerMovement.isSprint == false)
        {
            mana.manaAmount += mana.manaReganAmount * Time.deltaTime;
        }
        else


       

        mana.Update();

        barImage.fillAmount = mana.GetManaNormolized();

        Rect uvRect = coolImage.uvRect;
        uvRect.x += 0.2f * Time.deltaTime;
        coolImage.uvRect = uvRect;

        //Vector2 barmasksizedelta = EdgeTransform1.sizeDelta;
        //barmasksizedelta.x = barImage.fillAmount * BarmaskWidth;
        //EdgeTransform1.sizeDelta = barmasksizedelta;

        //EdgeTransform1.anchoredPosition = new Vector2(mana.GetManaNormolized() * BarmaskWidth, 0);

    }
    public void SpendMana(float Spend)
    {
        mana.TrySpendMana(Spend);
    }
}
public class Mana
{
    public const int Mana_Max = 100;

    public float manaAmount;
    public float manaReganAmount;

    public Mana()
    {
        manaAmount = 0;
        manaReganAmount = 20f;
    }

    public void Update() => manaAmount = Mathf.Clamp(manaAmount, 0, Mana_Max);

    public void TrySpendMana(float amount)
    {
        if (manaAmount >= amount)
        {
            manaAmount -= amount;
        }
    }
    public float GetManaNormolized() { return manaAmount / Mana_Max; }
}