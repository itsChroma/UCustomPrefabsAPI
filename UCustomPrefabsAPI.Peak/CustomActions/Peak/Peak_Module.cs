using UCustomPrefabsAPI.Peak.CustomActions;
namespace UCustomPrefabsAPI.Peak
{
    public abstract class Peak_Module
    {
        public Peak_CustomHelper instance;
        public virtual void Init() { }
        public virtual void Update() { }
        public virtual void Reset() { }
        public virtual void Destroy() { }
    }
}
