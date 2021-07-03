using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Generator : MonoBehaviour
{
	private static int[] center = new int[2]{0,0};
	private static int SIZE = 125;
	private static int NUMPARTICLES = 100;
	private int simulationTime;
	private int circum;
	private int[,,] barcenter;
	private int[,] parts;
	private GameObject[] partsObjects;
	private bool[,] tree;
	private Color color;
	private float norm2(int[] x, int[] y) { return Mathf.Sqrt(Mathf.Pow(x[0] - y[0],2) + Mathf.Pow(x[1] - y[1],2)); }
	public GameObject particlePrefab;
	private bool flag;

	private float[] proba(int[] particle, int[] center)
	{
		float k = 0.5f;
		float deltax = center[0] - particle[0];
		float deltay = center[1] - particle[1];
		float d = norm2(center, particle);
		float[] r = new float[] { 0.5f + (deltax / d) * k, 0.5f + (deltay / d) * k, 0.5f - (deltax / d) * k, 0.5f - (deltay / d) * k };
		r[0] = r[0] / 2;
		for (int i = 1; i < 4; i++) { r[i] = (2 * r[i - 1] + r[i]) / 2; }
		return r;
	}
	private void updatebaryc(int[] part) {
		int rmax = circum + 50;
		int rmin = circum - 20;
		for (int i = -rmax + SIZE; i < rmax + SIZE; i++)
		{
			for (int j = -rmax + SIZE; j < rmax + SIZE; j++)
			{
				int[] coords = new int[2] { i, j };
				if (norm2(center, coords) > rmin && norm2(center, coords) < rmax) {
					int[] coordsB = new int[2] { barcenter[i, j, 0], barcenter[i, j, 1] };
					float d1 = norm2(coords, coordsB);
					float d2 = norm2(coords, part);
					if (d2 == 0){
						barcenter[i, j,0] = part[0];
						barcenter[i, j,1] = part[1];
					} 
					else if (d1!= 0){
						barcenter[i, j, 0] = (int)((barcenter[i, j, 0] / Mathf.Pow(d1, 2) + part[0] / Mathf.Pow(d2, 2)) / (1 / Mathf.Pow(d1, 2) + 1 / Mathf.Pow(d2, 2)));
						barcenter[i, j, 1] = (int)((barcenter[i, j, 1] / Mathf.Pow(d1, 2) + part[1] / Mathf.Pow(d2, 2)) / (1 / Mathf.Pow(d1, 2) + 1 / Mathf.Pow(d2, 2)));
					}
				}
			}
		}
	}
	
	// Start is called before the first frame update
	void Start()
	{
		flag = true;
		simulationTime = 0;
		circum = 0;
		barcenter = new int[2* SIZE, 2* SIZE, 2];
		parts = new int[100,2];
		partsObjects = new GameObject[NUMPARTICLES];
		tree = new bool[2* SIZE, 2* SIZE];
		tree[SIZE, SIZE] = true;

		for (int i = 0; i < parts.GetLength(0); i++) {
			float theta = (Random.Range(0f, 360f) / 180) * Mathf.PI;
			float radius = Random.Range(circum + 5, circum + 25);
			parts[i,0] = (int)(radius * Mathf.Cos(theta));
			parts[i,1] = (int)(radius * Mathf.Sin(theta));
			partsObjects[i] = Instantiate(particlePrefab, new Vector3(parts[i, 0], 0, parts[i, 1]), Quaternion.identity);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (flag)
		{
			simulationTime ++;
			float hue = simulationTime%300 / 300f;
			color = Color.HSVToRGB(hue, 1f, 0.5f);


			for (int i = 0; i < NUMPARTICLES; i++)
			{
				float r = Random.Range(0f, 1f);
				int[] part = new int[2] { parts[i, 0], parts[i, 1] };
				
				float[] probas = proba(part,
					new int[2] { barcenter[part[0] + SIZE, part[1] + SIZE, 0], barcenter[part[0] + SIZE, part[1] + SIZE, 1] });
				if (r < probas[0]) { part[0] += 1; }
				else if (r < probas[1]) { part[1] += 1; }
				else if (r < probas[2]) { part[0] += -1; }
				else { part[1] += -1; }
				




				if ((int)part[0] >= SIZE-1 || (int)part[0] < -SIZE + 2 || (int)part[1] >= SIZE-1 || (int)part[1] < -SIZE + 2 || norm2(center, part) <= circum - 20)
				{
					float theta = (Random.Range(0f, 360f) / 180) * Mathf.PI;
					float radius = Random.Range(circum + 5, circum + 25);

					part[0] = (int)(radius * Mathf.Cos(theta));

					part[1] = (int)(radius * Mathf.Sin(theta));
				}

				if (tree[SIZE + part[0] + 1, SIZE + part[1]] ||
				tree[SIZE + part[0] - 1, SIZE + part[1]] ||
				tree[SIZE + part[0], SIZE + part[1] + 1] ||
				tree[SIZE + part[0], SIZE + part[1] - 1])
				{
					tree[SIZE + part[0], SIZE + part[1]] = true;
					updatebaryc(part);



					float dist = norm2(center, part);

					if (dist > circum)
					{
						circum = (int)(dist);
						if (circum >= 100) { flag = false; }
					}
					GameObject stablePart = Instantiate(particlePrefab, new Vector3(part[0], 0, part[1]), Quaternion.identity);
					stablePart.GetComponent<Renderer>().material.color = color;


					float theta = (Random.Range(0f, 360f) / 180) * Mathf.PI;
					float radius = Random.Range(circum + 5, circum + 25);

					part[0] = (int)(radius * Mathf.Cos(theta));

					part[1] = (int)(radius * Mathf.Sin(theta));
				}
				parts[i, 0] = part[0];
				parts[i, 1] = part[1];
				partsObjects[i].transform.position = new Vector3(part[0], 0, part[1]);
			}
		}
		
        
    }
}