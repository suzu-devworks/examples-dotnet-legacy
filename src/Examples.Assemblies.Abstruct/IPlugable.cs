namespace Examples.Assemblies
{
    public interface IPlugable
    {
        void DoAction();

        object GetValue(string target);

        T GetValue<T>(string target);

        void ThrownAction();
    }
}
