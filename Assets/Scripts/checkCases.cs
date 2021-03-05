using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkCases : MonoBehaviour
{

    public bool validé = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseEnter()
    {
        GameManager.Instance.goCase = this.gameObject;

        GameManager.Instance.MouseEnterCases.Add(gameObject);
        validé = true;

    }

    public void OnMouseExit()
    {
        
    }

}
