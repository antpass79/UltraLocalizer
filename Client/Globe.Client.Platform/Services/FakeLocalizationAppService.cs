using Globe.Client.Platform.Assets.Localization;
using Globe.Client.Platform.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Globe.Client.Platform.Services
{
    public class FakeLocalizationAppService : ILocalizationAppService
    {
        #region Data Members

        private readonly string LANGUAGE_EN = "en";
        private readonly string LANGUAGE_IT = "it";
        readonly LocalizedDictionary _english = new LocalizedDictionary();
        readonly LocalizedDictionary _italian = new LocalizedDictionary();
        readonly Dictionary<string, LocalizedDictionary> _languages = new Dictionary<string, LocalizedDictionary>();

        LocalizedDictionary _current;

        #endregion

        #region Constructors

        public FakeLocalizationAppService()
        {
            FillEnglishValues();
            FillItalianValues();

            _languages.Add(LANGUAGE_EN, _english);
            _languages.Add(LANGUAGE_IT, _italian);

            _selectedLanguage = LANGUAGE_EN;
            _current = _languages[LANGUAGE_EN];
        }

        #endregion

        #region Properties

        private string _selectedLanguage;
        public string SelectedLanguage { get => _selectedLanguage; }

        #endregion

        #region Public Functions

        async public Task<LocalizedDictionary> LoadAsync(string language)
        {
            if (_languages.ContainsKey(language))
                _selectedLanguage = language;
            else
                _selectedLanguage = LANGUAGE_EN;

            _current = _languages[_selectedLanguage];

            return await Task.FromResult(_current);
        }

        public string Resolve(string key)
        {
            return _current[key];
        }

        #endregion

        #region Private Functions

        private void FillItalianValues()
        {
            _italian.Add(nameof(LanguageKeys.UltraLocalizer), LanguageKeys.UltraLocalizer);

            _italian.Add(nameof(LanguageKeys.Concept), "Concetto");
            _italian.Add(nameof(LanguageKeys.ComponentNamespace), "Spazio nomi del componente");
            _italian.Add(nameof(LanguageKeys.InternalNamespace), "Spazio nomi interno");
            _italian.Add(nameof(LanguageKeys.Software_developer_comment), "Commento dello sviluppatore");
            _italian.Add(nameof(LanguageKeys.Master_translator_comment), "Commento del traduttore master");
            _italian.Add(nameof(LanguageKeys.Ignore_Translation), "Ignora la traduzione");
            _italian.Add(nameof(LanguageKeys.Searching_Options_for_selected_String), "Opzioni di ricerca per la stringa selezionata");
            _italian.Add(nameof(LanguageKeys.Show_Custom_Filters), "Mostra filtri per esportazione Custom");
            _italian.Add(nameof(LanguageKeys.Choose_Exportation_Mode), "Selezionare la modalitá di esportazione:");
            _italian.Add(nameof(LanguageKeys.By_Concept_English_language_only), "Cerca il concetto per Inglese");
            _italian.Add(nameof(LanguageKeys.By_Value_Current_language_only), "Cerca il valore per linguaggio corrente");
            _italian.Add(nameof(LanguageKeys.No_Filters), "Nessun filtro");
            _italian.Add(nameof(LanguageKeys.Filter_by_Context), "Filtro per contesto");
            _italian.Add(nameof(LanguageKeys.Filter_by_String_Type), "Filtro per contesto");
            _italian.Add(nameof(LanguageKeys.Select_all_languages), "Seleziona tutte le lingue");
            _italian.Add(nameof(LanguageKeys.Select_all_components), "Seleziona tutti i componenti");

            _italian.Add(nameof(LanguageKeys.Add_selected_item), "Aggiungi la selezione");
            _italian.Add(nameof(LanguageKeys.Add_selected_group), "Aggiungi il gruppo");
            _italian.Add(nameof(LanguageKeys.Add_all_items), "Aggiungi tutto");
            _italian.Add(nameof(LanguageKeys.Remove_all_items), "Rimuovi tutto");
            _italian.Add(nameof(LanguageKeys.Remove_the_selection), "Rimuovi la selezione");

            _italian.Add(nameof(LanguageKeys.Get_new_concepts), "Ottieni nuovi concetti");
            _italian.Add(nameof(LanguageKeys.No_concepts_found), "Filtro per contesto");

            _italian.Add(nameof(LanguageKeys.ShowFilters), "Mostra Filtri");
            _italian.Add(nameof(LanguageKeys.HideFilters), "Nascondi Filtri");
            _italian.Add(nameof(LanguageKeys.Options), "Opzioni");
            _italian.Add(nameof(LanguageKeys.Translation), "Traduzione");
            _italian.Add(nameof(LanguageKeys.Merge), "Unisci");
            _italian.Add(nameof(LanguageKeys.Load), "Carica");
            _italian.Add(nameof(LanguageKeys.Save), "Salva");
            _italian.Add(nameof(LanguageKeys.Open), "Apri");
            _italian.Add(nameof(LanguageKeys.Close), "Chiudi");
            _italian.Add(nameof(LanguageKeys.AutoMerge), "Unisci");
            _italian.Add(nameof(LanguageKeys.Cancel), "Annulla");
            _italian.Add(nameof(LanguageKeys.UserName), "Nome Utente");
            _italian.Add(nameof(LanguageKeys.UserLogin), "Login Utente");
            _italian.Add(nameof(LanguageKeys.Password), "Password");
            _italian.Add(nameof(LanguageKeys.Login), "Accedi");
            _italian.Add(nameof(LanguageKeys.Logout), "Esci");
            _italian.Add(nameof(LanguageKeys.Search), "Cerca");
            _italian.Add(nameof(LanguageKeys.NoComponents), "Nessun componente trovato");

            _italian.Add(nameof(LanguageKeys.Page_Home), "Home");
            _italian.Add(nameof(LanguageKeys.Page_Joblist_Management), "Gestione liste di lavoro");
            _italian.Add(nameof(LanguageKeys.Page_Current_Job), "Lavoro corrente");
            _italian.Add(nameof(LanguageKeys.Page_Concept_Management), "Gestione Concetti");
            _italian.Add(nameof(LanguageKeys.Page_JobList_Status), "Stato delle liste di lavoro");

            _italian.Add(nameof(LanguageKeys.Please_enter_your_credentials), "Inserisci i dettagli");

            _italian.Add(nameof(LanguageKeys.Operation_successfully_completed), "Operazione eseguita con successo");
            _italian.Add(nameof(LanguageKeys.Error_during_server_communication), "Errore durante la comunicazione con il server");
            _italian.Add(nameof(LanguageKeys.Strings_from_file_system), "Stringhe caricate da file system");
            _italian.Add(nameof(LanguageKeys.Impossible_to_retrieve_strings), "Impossibile recuperare le stringhe");
            _italian.Add(nameof(LanguageKeys.Strings_saved_in_file_system), "Stringhe salvate nel file siystem");
            _italian.Add(nameof(LanguageKeys.Impossible_to_save_strings), "Impossibile salvare le stringhe");
            _italian.Add(nameof(LanguageKeys.HomeIntro_1), "Il Localizzatore fornisce una interfaccia utente per tradurre tutte le parole in linguaggi differenti");
            _italian.Add(nameof(LanguageKeys.HomeIntro_2), "Il traduttore puo' lavorare offline");
            _italian.Add(nameof(LanguageKeys.Is_logged), "è connesso");
            _italian.Add(nameof(LanguageKeys.Items_Count), "Numero di Oggetti: ");
            _italian.Add(nameof(LanguageKeys.Filters), "Filtri");
            _italian.Add(nameof(LanguageKeys.Language), "Linguaggio");
            _italian.Add(nameof(LanguageKeys.Components), "Componenti");
            _italian.Add(nameof(LanguageKeys.Check_New_Concepts), "Cerca Nuovi Concetti");
            _italian.Add(nameof(LanguageKeys.Save_Joblist), "Salva Joblist");
            _italian.Add(nameof(LanguageKeys.Export_to_XML), "Esporta in XML");
            _italian.Add(nameof(LanguageKeys.FilterBy), "Filtri applicati");
            _italian.Add(nameof(LanguageKeys.JobListName), "Lista di lavoro");
            _italian.Add(nameof(LanguageKeys.KeepThis), "Prendi questo");
            _italian.Add(nameof(LanguageKeys.Full), "Standard");
            _italian.Add(nameof(LanguageKeys.Custom), "Personalizzata");
            _italian.Add(nameof(LanguageKeys.Export), "Esporta");

            _italian.Add(nameof(LanguageKeys.English), "Inglese");
            _italian.Add(nameof(LanguageKeys.French), "Francese");
            _italian.Add(nameof(LanguageKeys.Italian), "Italiano");
            _italian.Add(nameof(LanguageKeys.German), "Tedesco");
            _italian.Add(nameof(LanguageKeys.Spanish), "Spagnolo");
            _italian.Add(nameof(LanguageKeys.Chinese), "Cinese");
            _italian.Add(nameof(LanguageKeys.Russian), "Russo");
            _italian.Add(nameof(LanguageKeys.Portuguese), "Portoghese");

            _italian.Add(nameof(LanguageKeys.Error_during_searching_new_concepts), "Errore durante la ricerca di nuovi Concepts");
            _italian.Add(nameof(LanguageKeys.Error_during_building_groups), "Errore durante la costruzione della struttura ad albero dei concetti non tradotti");
            _italian.Add(nameof(LanguageKeys.Error_during_filters_initialization), "Errore durante inizializzazione dei filtri");
            _italian.Add(nameof(LanguageKeys.Error_during_dialog_opening), "Errore durante apertura della nuova finestra");
            _italian.Add(nameof(LanguageKeys.Error_performing_search_action), "Errore durante l'azione di Cerca");
        }

        private void FillEnglishValues()
        {
            _english.Add(nameof(LanguageKeys.UltraLocalizer), LanguageKeys.UltraLocalizer);

            _english.Add(nameof(LanguageKeys.Concept), "Concept");
            _english.Add(nameof(LanguageKeys.ComponentNamespace), "Component Namespace");
            _english.Add(nameof(LanguageKeys.InternalNamespace), "Internal Namespace");
            _english.Add(nameof(LanguageKeys.Software_developer_comment), "Software developer comment");
            _english.Add(nameof(LanguageKeys.Master_translator_comment), "Master translator comment");
            _english.Add(nameof(LanguageKeys.Ignore_Translation), "Ignore translation");
            _english.Add(nameof(LanguageKeys.Searching_Options_for_selected_String), "Searching options for selected string");
            _english.Add(nameof(LanguageKeys.Show_Custom_Filters), "Show filters of custom exportation");
            _english.Add(nameof(LanguageKeys.Choose_Exportation_Mode), "Choose exportation mode:");
            _english.Add(nameof(LanguageKeys.By_Concept_English_language_only), "By Concept English language only");
            _english.Add(nameof(LanguageKeys.By_Value_Current_language_only), "By Value Current language only");
            _english.Add(nameof(LanguageKeys.No_Filters), "No Filters");
            _english.Add(nameof(LanguageKeys.Filter_by_Context), "Filter by Context");
            _english.Add(nameof(LanguageKeys.Filter_by_String_Type), "Filter by String Type");
            _english.Add(nameof(LanguageKeys.Select_all_languages), "Select all languages");
            _english.Add(nameof(LanguageKeys.Select_all_components), "Select all components");

            _english.Add(nameof(LanguageKeys.Add_selected_item), "Add selected item");
            _english.Add(nameof(LanguageKeys.Add_selected_group), "Add the group");
            _english.Add(nameof(LanguageKeys.Add_all_items), "Add all");
            _english.Add(nameof(LanguageKeys.Remove_all_items), "Remove all");
            _english.Add(nameof(LanguageKeys.Remove_the_selection), "Remove the selection");

            _english.Add(nameof(LanguageKeys.Get_new_concepts), "Get new concepts");
            _english.Add(nameof(LanguageKeys.No_concepts_found), "No concepts found");

            _english.Add(nameof(LanguageKeys.ShowFilters), "Show Filters");
            _english.Add(nameof(LanguageKeys.HideFilters), "Hide Filters");
            _english.Add(nameof(LanguageKeys.Options), LanguageKeys.Options);
            _english.Add(nameof(LanguageKeys.Translation), LanguageKeys.Translation);
            _english.Add(nameof(LanguageKeys.Merge), LanguageKeys.Merge);
            _english.Add(nameof(LanguageKeys.Load), LanguageKeys.Load);
            _english.Add(nameof(LanguageKeys.Save), LanguageKeys.Save);
            _english.Add(nameof(LanguageKeys.Open), LanguageKeys.Open);
            _english.Add(nameof(LanguageKeys.Close), LanguageKeys.Close);
            _english.Add(nameof(LanguageKeys.AutoMerge), "Auto Merge");
            _english.Add(nameof(LanguageKeys.Cancel), LanguageKeys.Cancel);
            _english.Add(nameof(LanguageKeys.UserName), "User Name");
            _english.Add(nameof(LanguageKeys.UserLogin), "User Login");
            _english.Add(nameof(LanguageKeys.Password), "Password");
            _english.Add(nameof(LanguageKeys.Login), LanguageKeys.Login);
            _english.Add(nameof(LanguageKeys.Logout), LanguageKeys.Logout);
            _english.Add(nameof(LanguageKeys.Search), LanguageKeys.Search);
            _english.Add(nameof(LanguageKeys.NoComponents), "Any component found");

            _english.Add(nameof(LanguageKeys.Page_Home), "Home");
            _english.Add(nameof(LanguageKeys.Page_Joblist_Management), "Joblist Management");
            _english.Add(nameof(LanguageKeys.Page_Current_Job), "Current Job");
            _english.Add(nameof(LanguageKeys.Page_Concept_Management), "Concept Management");
            _english.Add(nameof(LanguageKeys.Page_JobList_Status), "Joblist Status");

            _english.Add(nameof(LanguageKeys.Please_enter_your_credentials), "Please enter your credentials");

            _english.Add(nameof(LanguageKeys.Operation_successfully_completed), "Operation successfully completed");
            _english.Add(nameof(LanguageKeys.Error_during_server_communication), "Error during server communication");
            _english.Add(nameof(LanguageKeys.Strings_from_file_system), "Strings from file system");
            _english.Add(nameof(LanguageKeys.Impossible_to_retrieve_strings), "Impossible to retrieve strings");
            _english.Add(nameof(LanguageKeys.Strings_saved_in_file_system), "Strings saved in file system");
            _english.Add(nameof(LanguageKeys.Impossible_to_save_strings), "Impossible to save strings");
            _english.Add(nameof(LanguageKeys.HomeIntro_1), "The Ultra Localizer provides an User Interface to translate all strings in different languages.");
            _english.Add(nameof(LanguageKeys.HomeIntro_2), "The translator can work without network connection, thanks to the offline mode.");
            _english.Add(nameof(LanguageKeys.Download), "Check the last available application version.");
            _english.Add(nameof(LanguageKeys.Is_logged), "is logged");
            _english.Add(nameof(LanguageKeys.Items_Count), "Items Count: ");
            _english.Add(nameof(LanguageKeys.Filters), "Filters");
            _english.Add(nameof(LanguageKeys.Language), "Language");
            _english.Add(nameof(LanguageKeys.Components), "Components");
            _english.Add(nameof(LanguageKeys.Check_New_Concepts), "Check New Concepts");
            _english.Add(nameof(LanguageKeys.Save_Joblist), "Save Joblist");
            _english.Add(nameof(LanguageKeys.Export_to_XML), "Export to XML");
            _english.Add(nameof(LanguageKeys.FilterBy), "Filter by");
            _english.Add(nameof(LanguageKeys.JobListName), "Job list");
            _english.Add(nameof(LanguageKeys.KeepThis), "Keep this");
            _english.Add(nameof(LanguageKeys.Full), "Full");
            _english.Add(nameof(LanguageKeys.Custom), "Custom");
            _english.Add(nameof(LanguageKeys.Export), "Export");

            _english.Add(nameof(LanguageKeys.English), "English");
            _english.Add(nameof(LanguageKeys.French), "French");
            _english.Add(nameof(LanguageKeys.Italian), "Italian");
            _english.Add(nameof(LanguageKeys.German), "German");
            _english.Add(nameof(LanguageKeys.Spanish), "Spanish");
            _english.Add(nameof(LanguageKeys.Chinese), "Chinese");
            _english.Add(nameof(LanguageKeys.Russian), "Russian");
            _english.Add(nameof(LanguageKeys.Portuguese), "Portuguese");

            _english.Add(nameof(LanguageKeys.Error_during_searching_new_concepts), "Error during searching new concepts");
            _english.Add(nameof(LanguageKeys.Error_during_building_groups), "Error during building groups");
            _english.Add(nameof(LanguageKeys.Error_during_filters_initialization), "Error during filters initialization");
            _english.Add(nameof(LanguageKeys.Error_during_dialog_opening), "Error during dialog opening");
            _english.Add(nameof(LanguageKeys.Error_performing_search_action), "Error performing search action");
        }

        #endregion
    }
}
