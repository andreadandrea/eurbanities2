using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathList : List<Transform>
{
    public List<PathList> parents = new List<PathList>();

    public float GetCost()
    {
        return Count + parents.Sum(parent => parent.Count);
    }
    
    public PathList() {}

    public PathList(PathList newParent)
    {
        parents.AddRange(newParent.parents);
        parents.Add(newParent);
    }
}