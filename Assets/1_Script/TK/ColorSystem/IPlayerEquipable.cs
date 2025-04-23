using UnityEngine;

namespace Swift_Blade
{
    public interface IPlayerEquipable
    {
        public ColorType GetColor { get; }
        public Sprite GetSprite { get; }
        public string DisplayName { get; }
    }
}
