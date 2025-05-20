using UnityEngine;

namespace Swift_Blade.Pool
{
    public class FollowParticle<T> : ParticlePoolAble<T> where T : ParticlePoolAble<T>
    {
        private Transform followTransform;
        private float yOffset;

        public void SetFollowTransform(Transform followTransform,bool useYOffset = true)
        {
            this.followTransform = followTransform;
            if (followTransform != null && useYOffset)
            {
                yOffset = 0;
                yOffset = transform.position.y - followTransform.position.y;
            }
            else if (useYOffset == false)
            {
                yOffset = 0;
            }
        }

        protected override void Update()
        {
            base.Update();

            if (followTransform != null)
            {
                Vector3 pos = new Vector3
                (
                    followTransform.position.x,
                    followTransform.position.y + yOffset,
                    followTransform.position.z
                );
                transform.position = pos;
            }
            
        }
    }
}