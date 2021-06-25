using System;

namespace Examples.Assemblies.Plugins
{
    public class PlugExecutor : MarshalByRefObject, IPlugable
    {
        public void DoAction()
        {
            //Put it in a variable to make it easier to rewrite later.
            var count = 1L;
            Console.WriteLine($"★{typeof(PlugExecutor).Name}.{nameof(DoAction)} is Called: {nameof(count)} = {count}.");

            return;
        }

        public object GetValue(string target)
        {
            throw new NotImplementedException();
        }

        public T GetValue<T>(string target)
        {
            throw new NotImplementedException();
        }

        public void ThrownAction()
        {
            try
            {
                throw new Exception($"Inner thrown Exception.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"★{typeof(PlugExecutor).Name}.{nameof(ThrownAction)} is catched: {ex.Message}.");
            }
            throw new Exception($"Outer thrown Exception.");
        }

    }
}
