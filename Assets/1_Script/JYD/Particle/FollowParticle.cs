using UnityEngine;

namespace Swift_Blade.Pool
{
    public class FollowParticle<T> : ParticlePoolAble<T> where T : ParticlePoolAble<T>
    {
        private Transform followTransform;
        private float yOffset;

        public void SetFollowTransform(Transform followTransform)
        {
            this.followTransform = followTransform;
            if (followTransform != null)
            {
                yOffset = transform.position.y - followTransform.position.y;
            }
        }

        protected override void Update()
        {
            base.Update();

            if (followTransform != null)
            {
                Vector3 pos = new Vector3(
                    followTransform.position.x,
                    followTransform.position.y + yOffset,
                    followTransform.position.z
                );
                transform.position = pos;
            }
            
        }
    }
}