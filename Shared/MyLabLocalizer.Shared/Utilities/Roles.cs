namespace MyLabLocalizer.Shared.Utilities
{
    public static class Roles
    {
        public readonly static string All = string.Empty;
        public readonly static string Admin = nameof(Admin);
        public readonly static string SuperUser = nameof(SuperUser);
        public readonly static string MasterTranslator = nameof(MasterTranslator);
        public readonly static string Group_Administrators =
            $"{Admin}," +
            $"{SuperUser}," +
            $"{MasterTranslator}";
        public readonly static string TranslatorIT = nameof(TranslatorIT);
        public readonly static string TranslatorDE = nameof(TranslatorDE);
        public readonly static string TranslatorEN = nameof(TranslatorEN);
        public readonly static string TranslatorPT = nameof(TranslatorPT);
        public readonly static string TranslatorRU = nameof(TranslatorRU);
        public readonly static string TranslatorES = nameof(TranslatorES);
        public readonly static string TranslatorFR = nameof(TranslatorFR);
        public readonly static string Group_Translators =
            $"{TranslatorIT}," +
            $"{TranslatorDE}," +
            $"{TranslatorEN}," +
            $"{TranslatorPT}," +
            $"{TranslatorRU}," +
            $"{TranslatorES}," +
            $"{TranslatorFR}";
        public readonly static string Group_All =
            $"{Admin}," +
            $"{SuperUser}," +
            $"{MasterTranslator}," +
            $"{Group_Translators}";
    }
}
