using UnityEngine;

namespace Swift_Blade.Pool
{
    
    public class FollowParticle<T> : ParticlePoolAble<T> where T : ParticlePoolAble<T>
    {
        private Transform followTransform;

        public void SetFollowTransform(Transform followTransform)
        {
            this.followTransform = followTransform;
        }
        
        protected override void Update()
        {
            
            base.Update();
            if (followTransform)
            {
                Vector3 pos = new Vector3(followTransform.position.x,transform.position.y
                    ,followTransform.position.z);
                transform.position = pos;
            }
        }
        
    }
    
}