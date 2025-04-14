using UnityEngine;

namespace Swift_Blade
{
    public class WeaponOrbParticle : MonoBehaviour
    {
        [SerializeField] private ParticleSystem ps;
        [SerializeField] private ParticleSystem ps1;
        [SerializeField] private ParticleSystem ps2;

        [SerializeField] private ParticleSystem ps_model;

        [SerializeField] private WeaponSO def;
        private void Awake()
        {
            Setup(def);
        }
        public void Setup(WeaponSO weapon)
        {
            Color color = ColorUtils.GetColorRGBUnity(weapon.ColorType);
            SetParticleColor(ps, color);
            SetParticleColor(ps1, color);
            SetParticleColor(ps2, color);

            //todo : this doesn't work
            ParticleSystem.ShapeModule shapeModule = ps_model.shape;
            shapeModule.mesh = null;// weapon.PreviewMesh;

        }
        private void SetParticleColor(ParticleSystem particleSystem, Color color)
        {
            ParticleSystem.MainModule mainModule = particleSystem.main;
            mainModule.startColor = color;
        }
        private void SetParticleColor(ParticleSystem.MainModule targetMainModule, Color color)
        {
            ParticleSystem.MainModule mainModule = targetMainModule;
            mainModule.startColor = color;
        }

    }
}
