using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{
    public Captor[] captors;

    public float speed;
    public float range;

    bool firstTime = true;

    float[] firstNeurs = { 1, 1, 1, 1, 1 };
    float[] secondNeurs = new float[5];
    float[] thirdNeurs = new float[5];
    float[] lastNeurs = new float[3];

    float[] firstSyn;
    float[] secondSyn;
    float[] thirdSyn;

    float[] values;

    private void Start()
    {
        values = new float[secondNeurs.Length + thirdNeurs.Length + lastNeurs.Length];
        SetBlackBoxNeurons();
        SetBlackBoxSyn();

        StartCoroutine(FirstTimeEnabled());
        DoAntThings();
    }

    private void Update()
    {
        if (!firstTime)
        {
            //AntThings();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoAntThings();
        }
    }

    void DoAntThings()
    {
        values = new float[secondNeurs.Length + thirdNeurs.Length + lastNeurs.Length];

        foreach (Captor captor in captors)
        {
            float rate = 0;
            if (captor.GetHits(range).Length > 0)
            {
                RaycastHit hit = captor.GetNearestHit(range);
                rate = 1 - captor.GetDistance(hit.point) / range;
            }

            firstNeurs[(int)captor.GetSide()] = rate;
        }

        for (int i = 0; i < firstNeurs.Length; i++)
        {
            for (int ia = 0; ia < secondNeurs.Length; ia++)
            {
                values[ia] += firstNeurs[i] * firstSyn[i * secondNeurs.Length + ia] + secondNeurs[ia];
                Debug.LogError(values[ia]);
            }
        }

        for (int i = 0; i < secondNeurs.Length; i++)
        {
            for (int ia = 0; ia < thirdNeurs.Length; ia++)
            {
                values[ia + secondNeurs.Length] += values[i] * secondSyn[i * thirdNeurs.Length + ia] + thirdNeurs[ia];
                Debug.LogError(values[ia + secondNeurs.Length]);
            }
        }

        for (int i = 0; i < thirdNeurs.Length; i++)
        {
            for (int ia = 0; ia < lastNeurs.Length; ia++)
            {
                values[ia + thirdNeurs.Length + secondNeurs.Length] += values[i + secondNeurs.Length] * thirdSyn[i * lastNeurs.Length + ia] + lastNeurs[ia];
                Debug.LogError(values[ia + thirdNeurs.Length + secondNeurs.Length] + " ind " + ia + thirdNeurs.Length + secondNeurs.Length + "/" + values.Length);
            }
        }

        for (int i = secondNeurs.Length + thirdNeurs.Length; i < values.Length; i++)
        {
            Debug.Log("==========");
            Debug.LogError(values[i]);
            values[i] = 1 / (1 + Mathf.Pow(1.1f, -values[i]));
            Debug.LogError(values[i]);
        }
    }

    void SetBlackBoxSyn()
    {
        firstSyn = new float[firstNeurs.Length * secondNeurs.Length];
        secondSyn = new float[secondNeurs.Length * thirdNeurs.Length];
        thirdSyn = new float[thirdNeurs.Length * lastNeurs.Length];

        for (int i = 0; i < firstSyn.Length; i++)
        {
            firstSyn[i] = Random.Range(0, 200) / 100f;
            Debug.Log(firstSyn[i]);
        }

        for (int i = 0; i < secondSyn.Length; i++)
        {
            secondSyn[i] = Random.Range(0, 200) / 100f;
            Debug.Log(secondSyn[i]);
        }

        for (int i = 0; i < thirdSyn.Length; i++)
        {
            thirdSyn[i] = Random.Range(0, 200) / 100f;
            Debug.Log(thirdSyn[i]);
        }
    }

    void SetBlackBoxNeurons()
    {
        for (int i = 0; i < secondNeurs.Length; i++)
        {
            secondNeurs[i] = Random.Range(-100, 100);
            Debug.Log(secondNeurs[i]);
        }

        for (int i = 0; i < thirdNeurs.Length; i++)
        {
            thirdNeurs[i] = Random.Range(-100, 100);
            Debug.Log(thirdNeurs[i]);
        }

        for (int i = 0; i < lastNeurs.Length; i++)
        {
            lastNeurs[i] = Random.Range(-100, 100);
            Debug.Log(lastNeurs[i]);
        }
    }

    IEnumerator FirstTimeEnabled()
    {
        yield return new WaitForSeconds(1);
        firstTime = false;
    }

    void MoveRight(float move)
    {
        transform.Translate(Vector3.right * move * Time.deltaTime, Space.Self);
    }

    void MoveForward(float move)
    {
        transform.Translate(Vector3.forward * move * Time.deltaTime, Space.Self);
    }

    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < captors.Length; i++)
        {
            Gizmos.color = Color.HSVToRGB(i * (360 / captors.Length) / 360f, 1, 1);
            Gizmos.DrawRay(captors[i].GetTransform().position, captors[i].GetTransform().TransformDirection(Vector3.forward) * range);
        }
    }
}