using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private const float EXPLODE_TIME = 3f;
    private const float EXPLODE_SCALE = 3f;
    private const float EXPLODE_DURATION = 0.1f;

    private float time = 0;

    public void Setup(float time, Vector3 position)
    {
        this.time = time;
        transform.position = position;
    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;
        if (time > EXPLODE_TIME)
        {
            var ratio = (time - EXPLODE_TIME) / EXPLODE_DURATION;
            transform.localScale = Vector3.one * EXPLODE_SCALE * ratio;
            if (time - EXPLODE_TIME > EXPLODE_DURATION)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            var scale = 0.5f + (1f + Mathf.Sin(time * 8f)) / 2f;
            transform.localScale = Vector3.one * scale;
        }
    }
}