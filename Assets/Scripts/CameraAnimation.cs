using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation : MonoBehaviour 
{
    public bool isMoving = false;
    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;
    private Vector3 initPosition, endPosition;
    private List<Renderer> renderers;
    private List<int> blockingMaterials;
    public float transparency;
    public float fadeSpeed;

    private void Start() 
    {
        renderers = new List<Renderer>();
        blockingMaterials = new List<int>();
    }

    void Update()
    {
        if(isMoving)
        {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(initPosition, endPosition, fracJourney);
            if(fracJourney >= 1)
                isMoving = false;
        }
        else
        {
            GameObject player = GameObject.Find("Player");
            RaycastHit[] hits;
            hits = Physics.RaycastAll(transform.position, (player.transform.position - transform.position), Vector3.Distance(player.transform.position, transform.position));

            blockingMaterials.Clear();

            for(int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                if(hit.collider.tag == "Player")
                    break;
                
                Renderer rend = hit.transform.GetComponent<Renderer>();

                if(rend)
                {
                    if(renderers.Contains(rend))
                    {
                        blockingMaterials.Add(renderers.IndexOf(rend));
                    }
                    else
                    {
                        renderers.Add(rend);
                        blockingMaterials.Add(renderers.IndexOf(rend));

                        SetMaterialTransparent(rend.material);
                        MakeTransparent(renderers[i], true);
                    }
                }
            }

            UpdateTransparencies();
        }
    }

    private void UpdateTransparencies()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            if(blockingMaterials.Contains(i))
            {
                MakeTransparent(renderers[i], false);
            }
            else
            {
                if(RemoveTransparency(renderers[i]))
                {
                    SetMaterialOpaque(renderers[i].material);
                    Color color = renderers[i].material.color;
                    color.a = 1F;
                    renderers[i].material.color = color;
                    renderers.RemoveAt(i);
                }
            }
        }
    }

    private void MakeTransparent(Renderer rend, bool set)
    {
        Color tempColor = rend.material.color;
        
        if(tempColor.a <= 0.5F)
            return;
        
        if(set)
            tempColor.a = 1F;
        else
            tempColor.a = tempColor.a - Time.deltaTime * fadeSpeed;
        
        rend.material.color = tempColor;
    }

    private bool RemoveTransparency(Renderer rend)
    {
        if(rend.material.color == null)
            return false;

        Color tempColor = rend.material.color;
        float newTempColor = tempColor.a + Time.deltaTime * fadeSpeed;

        if(newTempColor >= 1F)
            return true;
        
        tempColor.a = newTempColor;
        rend.material.color = tempColor;

        return false;
    }

    public void MoveCamera(Vector3 deltaMove)
    {
        startTime = Time.time;
        initPosition = transform.position;
        endPosition = initPosition + deltaMove;
        journeyLength = Vector3.Distance(initPosition, endPosition);
        isMoving = true;
    }

    private void SetMaterialTransparent(Material m)
    {
        {
            m.SetFloat("_Mode", 2);
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            m.SetInt("_ZWrite", 0);
            m.DisableKeyword("_ALPHATEST_ON");
            m.EnableKeyword("_ALPHABLEND_ON");
            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            m.renderQueue = 3000;
        }
    }


    private void SetMaterialOpaque(Material m)
    {
            m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            m.SetInt("_ZWrite", 1);
            m.DisableKeyword("_ALPHATEST_ON");
            m.DisableKeyword("_ALPHABLEND_ON");
            m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            m.renderQueue = -1;
    }
}
