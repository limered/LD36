namespace Assets.Systems
{
    public interface IGameSystem
    {
        void Init();
        void RegisterComponent(ILogicComponent component);
    }
}