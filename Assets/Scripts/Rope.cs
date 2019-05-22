using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
	public Transform[] segments;
	public float totalLength = 1.0f;
	public float currentLength { get; private set; }

	private float lastTotalLength = 0.0f; //DEBUG

    void Start()
    {
		Resize(totalLength);
    }
	
    void Update()
    {
		if (lastTotalLength != totalLength) //DEBUG
		{
			Resize(totalLength);
			lastTotalLength = totalLength;
		}
    }

	public void Resize(float newLength)
	{
		currentLength = newLength;

		float segmentLength = totalLength / segments.Length;

		int fullSegmentsCount = (int)(currentLength / segmentLength);

		float lastSegmentLength = currentLength - fullSegmentsCount * segmentLength;

		for (int i = 0; i < fullSegmentsCount; i++)
		{
			segments[i].localScale = new Vector3(segmentLength, 1.0f, 1.0f);
		}

		if (fullSegmentsCount < segments.Length)
		{
			segments[fullSegmentsCount].localScale = new Vector3(lastSegmentLength, 1.0f, 1.0f);
		}

		for (int i = fullSegmentsCount + 1; i < segments.Length; i++)
		{
			segments[i].localScale = new Vector3(0.0f, 1.0f, 1.0f);
		}
	}
}
