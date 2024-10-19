using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIPather : MonoBehaviour {

    [SerializeField] LayerMask obstacleMask;
    [SerializeField] LayerMask staticObstacleMask;

    List<int>[] visibleNodes;
    
    bool[] usedNodes;

    // Start is called before the first frame update
    void Start(){
        visibleNodes = new List<int>[transform.childCount];
        usedNodes = new bool[visibleNodes.Length];
        for (int i = 0; i < visibleNodes.Length; i++) {
            visibleNodes[i] = new List<int> ();
        }

        for (int i = 0; i < transform.childCount; i++) {
            for (int j = i+1; j < transform.childCount; j++) {
                Vector2 pos1 = transform.GetChild (i).position, pos2 = transform.GetChild (j).position;
                if (Physics2D.Raycast (pos1, pos2 - pos1, Vector2.Distance (pos1, pos2), staticObstacleMask).collider == null) {
                    visibleNodes[i].Add (j);
                    visibleNodes[j].Add (i);
                    Debug.Log ($"{i} {j} | {visibleNodes[i].Count} {visibleNodes[j].Count}");
                }
            }
        }

        
    }

    public LayerMask GetObstacleMask () { return obstacleMask; }

    // Update is called once per frame
    void Update() {
        
    }

    public Vector2 FindPath (Vector2 start, Vector2 goal, int maxDepth) {
        /*Vector2 retDir = Vector2.zero;
        float lowestScore = 0;
        bool foundGoal = false;
    
        if (Physics2D.Raycast (start, (goal - start).normalized, Vector2.Distance (start, goal), obstacleMask).collider == null)
            return (goal - start).normalized;
    
        for (int i = 0; i < transform.childCount; i++) {
            Vector2 nodePos = transform.GetChild (i).position;
            float dist = Vector2.Distance (start, nodePos);
    
            if (Physics2D.Raycast (start, (nodePos - start).normalized, dist, obstacleMask).collider != null)
                continue;
    
            (float currentScore, bool leadsToGoal) = ScoreNode (i, goal, maxDepth);
    
            if ((foundGoal == leadsToGoal && lowestScore > currentScore) || (!foundGoal && leadsToGoal)) {
                //Debug.Log ($"executed {currentScore}");
                lowestScore = currentScore;
                foundGoal = leadsToGoal;
                retDir = (nodePos - start).normalized;
            }
        }
        
        return retDir;*/
    
        (_, _, int i) = ShortestPath (start, goal, maxDepth);
        switch (i) {
            case -2: //error
                return Vector2.zero;
    
            case -1: //a direct path to the goal exists
                return (goal - start).normalized;
    
            default:
            //Debug.Log ("a" + i);
                return ((Vector2)transform.GetChild (i).position - start).normalized;
        }
    }
    
    // distance = travel distance, complete path = whether the path leads to the goal or just somewhere close to it.
    // i = index of first node. (-1 for straight to the goal, -2 for error)
    public (float distance, bool completePath, int i) ShortestPath (Vector2 start, Vector2 goal, int recDepth) {
        for (int i = 0; i < usedNodes.Length; i++) {
            usedNodes[i] = false;
        }
        float dist = Vector2.Distance (start, goal);
        if (Physics2D.Raycast (start, (goal - start).normalized, dist, obstacleMask).collider == null) {
            return (dist, true, -1);
        }
        int ind = -2;
        dist = float.MaxValue;
        bool foundGoal = false;
        for (int i = 0; i < transform.childCount; i++) {
            float nodeDist = Vector2.Distance (start, transform.GetChild (i).position);
            if (Physics2D.Raycast (start, ((Vector2)transform.GetChild(i).position-start).normalized, nodeDist, obstacleMask).collider != null)
                continue;
    
            bool completePath;
            float pathDist;
            (pathDist, completePath) = ScoreNode (i, goal, recDepth);
            pathDist += nodeDist;
            if ((completePath == foundGoal && dist > pathDist) || (!foundGoal && completePath)) {
                dist = pathDist;
                foundGoal = completePath;
                ind = i;
            }
        }
    
        return (dist, foundGoal, ind);
    }


    // scores a path on the following (distance, and whether it leads directly to the player)
    (float, bool) ScoreNode (int node, Vector2 end, int maxDepth) {
        Vector2 startPosition = transform.GetChild (node).position;
        float distanceToGoal = Vector2.Distance (end, startPosition);
		bool foundGoal = Physics2D.Raycast (
			startPosition,
			(end - startPosition),
			distanceToGoal,
			obstacleMask
		).collider == null;
        if (maxDepth == 0 || foundGoal) {

            return (distanceToGoal, foundGoal);
        }

        float lowestScore = float.MaxValue;
        for (int j = 0; j < visibleNodes[node].Count; j++) {
            int i = visibleNodes[node][j];
            // skip if node is the current node
            if (i == node) continue;

            // skip if the node is behind a wall
            Vector2 nodePos = transform.GetChild (i).position;
            float distanceToNode = Vector2.Distance (startPosition, nodePos);


			if (Physics2D.Raycast (startPosition, (nodePos - startPosition), distanceToNode).collider != null)
                continue;

            // call score on all nodes and add distance to the node to the score
            (float pathDist, bool leadsToPlayer) = ScoreNode (i, end, maxDepth-1);
            pathDist += distanceToNode;

            // track the lowest score
            if ((foundGoal == leadsToPlayer && lowestScore > pathDist) || (!foundGoal && leadsToPlayer)) {
                lowestScore = pathDist;
                foundGoal = leadsToPlayer;
            }
        }

        return (lowestScore, foundGoal);
    }
}

