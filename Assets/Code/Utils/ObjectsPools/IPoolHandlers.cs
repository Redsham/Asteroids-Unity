namespace Utils.ObjectsPools
{
    public interface IPoolInitializer
    {
        void OnPoolInitialize();
    }
    public interface IPoolGetHandler
    {
        void OnPoolGet();
    }
    public interface IPoolReturnHandler
    {
        void OnPoolReturn();
    }
    public interface IPoollable
    {
        bool IsUsing { get; set; }
    }
}