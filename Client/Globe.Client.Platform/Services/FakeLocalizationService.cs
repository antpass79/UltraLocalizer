using Globe.Client.Platform.Assets.Localization;
using System.Collections.Generic;

namespace Globe.Client.Platform.Services
{
    public class FakeLocalizationService : ILocalizationService
    {
        Dictionary<string, string> _strings = new Dictionary<string, string>();

        public FakeLocalizationService()
        {
            _strings.Add(nameof(English.Operation_successfully_completed), English.Operation_successfully_completed);
            _strings.Add(nameof(English.Error_during_server_communication), English.Error_during_server_communication);
            _strings.Add(nameof(English.Strings_from_file_system), English.Strings_from_file_system);
            _strings.Add(nameof(English.Impossible_to_retrieve_strings), English.Impossible_to_retrieve_strings);
            _strings.Add(nameof(English.Strings_saved_in_file_system), English.Strings_saved_in_file_system);
            _strings.Add(nameof(English.Impossible_to_save_strings), English.Impossible_to_save_strings);
        }

        public string Resolve(string key)
        {
            if (!_strings.ContainsKey(key))
                return $"{key} MISSED";

            return _strings[key];
        }
    }
}
