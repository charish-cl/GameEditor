using DG.Tweening;
using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        float distanceTravelled;

        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
            }
        
            var paths= pathCreator.path.localPoints;
            Debug.Log(paths.Length);
            transform.DOPath(paths,speed)
                .SetSpeedBased(true)
                .SetLoops(-1,LoopType.Restart);
        }

        void Update()
        {
            // if (pathCreator != null)
            // {
            //     distanceTravelled += speed * Time.deltaTime;
            //     transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            //     var rotationAtDistance= pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            //     Debug.Log(rotationAtDistance.eulerAngles);
            //     transform.rotation = Quaternion.Euler(new Vector3(0,0,-rotationAtDistance.eulerAngles.x));
            // }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }
    }
}