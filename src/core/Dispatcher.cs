using System;
using USBUnlocker.Utilities;

namespace USBUnlocker.Core
{
    public class Dispatcher
    {
        private readonly FeatureRegistry _registry;
        private readonly AppConfig _config;
        
        public Dispatcher(FeatureRegistry registry, AppConfig config)
        {
            _registry = registry;
            _config = config;
        }
        
        public void Dispatch(string input)
        {
            if (string.IsNullOrEmpty(input))
                return;
            
            _registry.Dispatch(input);
        }
    }
}
