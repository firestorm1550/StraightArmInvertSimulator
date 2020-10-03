using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[ExecuteAlways]
[RequireComponent(typeof(GridLayoutGroup))]
public class GLGCellResizer : MonoBehaviour
{
    private GridLayoutGroup glg;

    private void Awake()
    {
        glg = GetComponent<GridLayoutGroup>();
    }
    
    void Update()
    {
        if (glg.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
        {
            float usableWidth = ((RectTransform) glg.transform).rect.width - glg.padding.horizontal;
            float cellWidth = (usableWidth - glg.spacing.x * (glg.constraintCount - 1)) / glg.constraintCount; 
            glg.cellSize = new Vector2(cellWidth, glg.cellSize.y);
        }
        if(glg.constraint == GridLayoutGroup.Constraint.FixedRowCount)
            throw new NotImplementedException();
    }
}
