using Globe.TranslationServer.Entities;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept;
using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Globe.TranslationServer.Porting.UltraDBDLL.XmlManager
{
    public class XmlManager
    {
        #region Data Members

        private object _xmlProcessedLock = new object();
        private object _xmlProcessingLock = new object();
        private object _errorsNoticeLock = new object();
        private object _keyTuplasLock = new object();
        private object _keyCommentsLock = new object();
        private object _xmlDataLock = new object();

        private string _directory = string.Empty;
        private string _extension = ".definition.xml";

        private int _interlock = 0;

        //lista che contiene tutte le triple component/internal/conceptId della tabella conceptTable (non ci sono triple ripetute)
        private IEnumerable<ConceptTupla> _dbTuplasForInsert;
        private IEnumerable<ConceptTupla> _dbTuplasForConceptList;
        //lista che contiene tutte le triple component/internal/conceptId e i contextId ad esse associati, (ci sono triple ripetute, per ogni diverso contextId associato alla tripla)
        IEnumerable<ConceptTupla> _dbTuplasForConcept2ContextList;
        //private List<ConceptTupla> _dbTuplas;

        private readonly LocalizationContext context;

        #endregion

        #region Constructors

        public XmlManager(LocalizationContext context)
        {
            this.context = context;
            this.CurrentLogManager = new LogManager();
        }

        #endregion

        #region Properties

        public List<int> ConceptList { get; set; }
        public List<int> Concept2ContextList { get; set; }
        public bool ChangesFound { get; set; }
        public int InsertedCount { get; set; }
        public int UpdatedCount { get; set; }
        public LogManager CurrentLogManager { get; set; }

        public List<string> XmlProcessed { get; set; }

        public List<string> XmlProcessing { get; set; }

        private int _xmlFound;
        public int XmlFound { get { return _xmlFound; } }

        public List<string> ErrorsNotice { get; set; }

        /// <summary>
        /// Elenco di tutte le stringhe lette da xml, ordinate secondo la chiave composta dalla tripla 
        /// ComponentNamespace, InternalNamespace, ConceptID. Ad ogni chiave, corrisponde una sottolista con almeno un elemento
        /// relativo alle stringhe associate
        /// </summary>
        public Dictionary<string, IEnumerable<string>> KeyTuplas { get; set; }

        public Dictionary<string, string> KeyComments { get; set; }

        /// <summary>
        /// Elenco di tutti gli xml letti, memorizzati come oggetti di tipo LocalizationResource
        /// </summary>
        public List<LocalizationResource> XmlData { get; set; }

        /// <summary>
        /// Indica la cartella dove pescare i file xml
        /// </summary>
        public string XmlDirectory
        {
            get { return _directory; }
            set { _directory = value; }
        }

        /// <summary>
        /// Indica l'estensione dei file da caricare.
        /// Se non indicata, il valore di default é .def.xml
        /// </summary>
        public string FileExtension
        {
            get { return _extension; }
            set { _extension = value; }
        }

        public AutoResetEvent Completed;
        public AutoResetEvent CompletedLists;
        #endregion



        #region Ctor

        //public XmlManager()
        //{

        //}

        #endregion

        #region metodi pubblici

        /// <summary>
        /// Carica gli Xml e aggiorna il DB in modo sequenziale
        /// </summary>
        public void LoadXmlAndFillDB()
        {
            InitializeDataStruct();
            GetXml();
        }



        public string GetUserString(string queryComponent,
                                    string queryInternal,
                                    string queryConcept,
                                    string queryContext)
        {
            var z = from x in XmlData
                    where x.ComponentNamespace == queryComponent
                    from S in x.LocalizationSection
                    where S.InternalNamespace == queryInternal
                    from a in S.Concept
                    where a.Id == queryConcept
                    from b in a.String
                    where b.Context == queryContext
                    select b.TypedValue;
            if (z.Count() > 0)
            {
                string s = z.FirstOrDefault().ToString();
                return s;
            }
            return null;
        }
        /// <summary>
        /// Carica gli Xml e aggiorna il DB, parallelizzando se possibile le operazioni
        /// </summary>
        public void LoadXmlAndFillDBWithThread()
        {
            InitializeDataStruct();
            _interlock = 0;
            ThreadPool.QueueUserWorkItem(new WaitCallback(GetXml), null);
        }


        /// <summary>
        /// Carica gli Xml SENZA aggiornare il DB, in modo sequenziale
        /// </summary>
        public void LoadXmlOnly()
        {
            InitializeDataStruct();
            GetXmlOnly();
        }

        /// <summary>
        /// Carica gli Xml SENZA aggiornare il DB, parallelizzando ove possibile le operazioni
        /// </summary>
        public void LoadXmlOnlyWithThread()
        {
            InitializeDataStruct();
            _interlock = 0;
            ThreadPool.QueueUserWorkItem(new WaitCallback(GetXmlOnly), null);
        }


        public void FillDB(Dictionary<string, IEnumerable<string>> keyTuplas)
        {
            if (keyTuplas == null || keyTuplas.Count() == 0)
                return;
            Completed.Reset();

            WriteDB(keyTuplas);
        }



        /// <summary>
        /// Dati i tre indici di ricerca, ritorna la chiave con cui accedere il valore nella lista KeyTuplas
        /// </summary>
        /// <param name="ComponentNamespace"></param>
        /// <param name="InternalNamespace"></param>
        /// <param name="ConceptID"></param>
        /// <returns></returns>
        public string GetKey(string ComponentNamespace, string InternalNamespace, string ConceptID)
        {
            return string.Format("{0}|{1}|{2}", ComponentNamespace, InternalNamespace, ConceptID);
        }

        /// <summary>
        /// Data la chiave, restituisce i tre indici di ricerca
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ComponentNamespace"></param>
        /// <param name="InternalNamespace"></param>
        /// <param name="ConceptID"></param>
        public void GetIndexes(string key, out string ComponentNamespace, out string InternalNamespace, out string ConceptID)
        {
            try
            {
                string[] str = key.Split('|');
                ComponentNamespace = str[0];
                InternalNamespace = (str[1] == string.Empty) ? null : str[1];
                ConceptID = str[2];
            }
            catch (Exception ex)
            {
                CurrentLogManager.Log(ex);
                throw new Exception("invalid key");
            }
        }

        #endregion

        #region metodi privati

        private void InitializeDataStruct()
        {
            XmlData = new List<LocalizationResource>();
            KeyTuplas = new Dictionary<string, IEnumerable<string>>();
            KeyComments = new Dictionary<string, string>();
            Completed = new AutoResetEvent(false);
            XmlProcessing = new List<string>();
            XmlProcessed = new List<string>();
            ErrorsNotice = new List<string>();
        }

        private void Initialize(out string[] files)
        {
            UltraDBConcept.UltraDBConcept concept = new UltraDBConcept.UltraDBConcept(context);
            //leggo dal db tutte le tuple della Concept

            UltraDBConcept2Context conceptToContext = new UltraDBConcept2Context(context);
            //eseguo le query
            _dbTuplasForInsert = concept.GetAllConcepts();


            DateTime timeStart = DateTime.Now;

            files = Directory.GetFiles(_directory, "*" + _extension);
            _xmlFound = files.Length;
            CurrentLogManager.Log(string.Format("{0} xml files found", _xmlFound));

        }

        #region GetXmlEDB

        private void GetXml()
        {
            string[] files;
            Initialize(out files);
            if (files == null || files.Length == 0)
            {
                //rilascio
                Completed.Set();
                if (OnCompleted != null)
                {
                    OnCompleted(this, null);
                }

                return;
            }


            foreach (string file in files)
            {
                if (!File.Exists(file))
                {
                    continue;
                }
                LoadXmlAndWriteDB(file);
            }


            Completed.Set();
            if (OnCompleted != null)
            {
                OnCompleted(this, null);
            }

        }

        private void GetXml(object state)
        {
            string[] files;
            Initialize(out files);
            if (files == null || files.Length == 0)
            {
                //rilascio
                Completed.Set();
                if (OnCompleted != null)
                {
                    OnCompleted(this, null);
                }

                return;
            }



            foreach (string file in files)
            {
                if (!File.Exists(file))
                {
                    //Logger.Error("Impossibile trovare xml in " + file);
                    continue;
                }
                System.Threading.Interlocked.Increment(ref _interlock);
                ThreadPool.QueueUserWorkItem(new WaitCallback(LoadXmlAndWriteDB), file);
            }
            while (_interlock > 0)
            {
                Thread.Sleep(50);
            }

            Completed.Set();
            if (OnCompleted != null)
            {
                OnCompleted(this, null);
            }

        }

        private void LoadXmlAndWriteDB(object state)
        {

            List<ConceptTupla> localKeyTuplasForInsert = new List<ConceptTupla>();
            List<ConceptTupla> localKeyTuplasForUpdate = new List<ConceptTupla>();
            IEnumerable<ConceptTupla> dbTuplasForUpdate;
            UltraDBConcept.UltraDBConcept concept = new UltraDBConcept.UltraDBConcept(context);
            UltraDBConcept2Context conceptToContext = new UltraDBConcept2Context(context);
            bool toSkip = false;
            string file = string.Empty;


            try
            {
                file = state as string;
                DateTime timeStart2 = DateTime.Now;

                lock (_xmlProcessingLock)
                {
                    XmlProcessing.Add(file.Trim());
                }

                #region Carico l'xml
                if (File.Exists(file))
                {
                    CurrentLogManager.Log(string.Format("{0} xml file loaded", file));
                    //recupero l'xml
                    LocalizationResource res = LocalizationResource.Load(file);

                    //aggiungo l'xml nella struttura che li terrà in memoria
                    lock (_xmlDataLock)
                    {
                        XmlData.Add(res);
                    }

                    //leggo i campi dell'xml
                    var DBData = res.LocalizationSection.Select(r => new { ComponentNamespace = res.ComponentNamespace, InternalNamespace = r.InternalNamespace, ConceptID = r.Concept.Select(t => new { Id = t.Id, Comments = t.Comments, str = t.String.Select(st => st.Context) }) });

                    //Preparo la struttura letta da xml per effettuare le operazioni insiemistiche con le strutture lette da DB
                    foreach (var item in DBData)
                    {
                        //tmpInternalNamespace = (item.InternalNamespace == null) ? DBNull.Value.ToString() : item.InternalNamespace;

                        foreach (var element in item.ConceptID)
                        {
                            string tmp = GetKey(item.ComponentNamespace, item.InternalNamespace, element.Id);
                            string com = null;
                            if (element.Comments != null)
                                com = element.Comments.TypedValue;
                            try
                            {
                                //inserisco la tupla nell'elenco globale di tutti gli xml letti
                                lock (_keyTuplasLock)
                                {
                                    KeyTuplas.Add(tmp, element.str);
                                }

                                lock (_keyCommentsLock)
                                {
                                    KeyComments.Add(tmp, com);
                                }

                            }
                            catch (Exception ex)
                            {
                                CurrentLogManager.Log(ex);

                                //segnalo eventuale ripetizione delle 3 chiavi presente in due xml distinti
                                lock (_errorsNoticeLock)
                                {
                                    ErrorsNotice.Add(string.Format("La tripla {0} nel file {1} risulta già presente in un xml precedentemente processato e pertanto viene scartata", tmp, file));
                                }
                                toSkip = true;
                            }

                            if (!toSkip)
                            {
                                try
                                {
                                    //riempo la struttura per l'insert
                                    localKeyTuplasForInsert.Add(new ConceptTupla { ComponentNamespace = item.ComponentNamespace, InternalNamespace = item.InternalNamespace, ConceptId = element.Id, Strings = element.str });
                                    //riempo la struttura per l'update
                                    foreach (string s in element.str)
                                    {
                                        localKeyTuplasForUpdate.Add(new ConceptTupla { ComponentNamespace = item.ComponentNamespace, InternalNamespace = item.InternalNamespace, ConceptId = element.Id, ContextId = UltraDBGlobal.UltraDBGlobal.GetContextId(s.Trim()).ToString() });
                                    }
                                }
                                catch (Exception)
                                {
                                    //nop
                                }
                            }
                            toSkip = false;

                        }
                    }
                    #endregion

                    #region INSERIMENTO NUOVE TUPLE NEL DB
                    //eseguo la differenza tra le triple lette nell'xml e quelle presenti nel DB. 
                    //l'insieme ottenuto va inserito nel db sia nella tabella Concept che in quella Context2Concept (per la parte relativa alle stringhe)
                    var tuplaToInsert = localKeyTuplasForInsert.Except(_dbTuplasForInsert, new TuplaComparerForInsert());

                    ////inserisco nel DB le nuove tuple sia nella Concept che nella Concept2Context
                    foreach (var item in tuplaToInsert)
                    {
                        //inserisco la tripla
                        int conceptId = concept.InsertNewConcept(item.ComponentNamespace, item.InternalNamespace, item.ConceptId, false, null);
                        CurrentLogManager.Log(string.Format("Concept {0} has been inserted with Component Namespace {1} and Internal Namespace {2} ", item.ConceptId, item.ComponentNamespace, item.InternalNamespace));
                        //per ogni tripla inserita, inserisco la entry nella concept2context
                        foreach (string s in item.Strings)
                        {
                            conceptToContext.InsertNewConcept2Context(conceptId, UltraDBGlobal.UltraDBGlobal.GetContextId(s.Trim()));
                        }
                    }
                    #endregion

                    #region UPDATE TUPLE DB

                    dbTuplasForUpdate = concept.GetAllConceptsAndContext();

                    var tuplaToUpdate = localKeyTuplasForUpdate.Except(dbTuplasForUpdate, new TuplaComparerForUpdate());


                    //inserisco nella Concept2Context le stringhe nuove associate a Concept già presenti
                    foreach (var item in tuplaToUpdate)
                    {
                        //ricavo l'ID
                        var id = dbTuplasForUpdate.Where(s => s.ComponentNamespace == item.ComponentNamespace && s.InternalNamespace == item.InternalNamespace && s.ConceptId == item.ConceptId).Select(s => s).FirstOrDefault().Id;
                        conceptToContext.InsertNewConcept2Context(Convert.ToInt32(id), Convert.ToInt32(item.ContextId.Trim()));

                        CurrentLogManager.Log(string.Format("Concept {0} has been updated with the context {1} ", item.ConceptId, UltraDBGlobal.UltraDBGlobal.GetContextName(Convert.ToInt32(item.ContextId.Trim()))));
                    }

                    #endregion
                }



                //Console.WriteLine(string.Format("Caricamento di {0} avvenuto in {1} ms", file, DateTime.Now.Subtract(timeStart2).TotalMilliseconds));

            }
            catch (Exception ex)
            {
                lock (_errorsNoticeLock)
                {
                    ErrorsNotice.Add(ex.Message);
                }
            }
            finally
            {
                Interlocked.Decrement(ref _interlock);

                lock (_xmlProcessedLock)
                {
                    XmlProcessed.Add(file.Trim());
                }

                lock (_xmlProcessingLock)
                {
                    XmlProcessing.Remove(file.Trim());
                }
            }
        }

        #endregion

        #region GetXMLONLY

        private void GetXmlOnly()
        {
            string[] files;
            Initialize(out files);
            if (files == null || files.Length == 0)
            {
                //rilascio
                Completed.Set();
                if (OnCompleted != null)
                {
                    OnCompleted(this, null);
                }

                return;
            }

            foreach (string file in files)
            {
                if (!File.Exists(file))
                {
                    continue;
                }
                LoadXml(file);
            }

            Completed.Set();
            if (OnCompleted != null)
            {
                OnCompleted(this, null);
            }
        }

        private void GetXmlOnly(object state)
        {
            string[] files;
            Initialize(out files);
            if (files == null || files.Length == 0)
            {
                //rilascio
                Completed.Set();
                if (OnCompleted != null)
                {
                    OnCompleted(this, null);
                }

                return;
            }

            foreach (string file in files)
            {
                if (!File.Exists(file))
                {
                    //Logger.Error("Impossibile trovare xml in " + file);
                    continue;
                }
                System.Threading.Interlocked.Increment(ref _interlock);
                try
                {
                    bool b = ThreadPool.QueueUserWorkItem(new WaitCallback(LoadXml), file);
                    //Trace.TraceInformation("Thread {0} inserted {1}", file, b);
                }
                catch (System.Exception ex)
                {
                    Trace.TraceInformation("Thread {0} exception {1}", file, ex.Message);
                }

            }
            while (_interlock > 0)
            {
                Thread.Sleep(50);
            }

            Completed.Set();
            if (OnCompleted != null)
            {
                OnCompleted(this, null);
            }

        }

        private void LoadXml(object state)
        {
            List<ConceptTupla> localKeyTuplasForInsert = new List<ConceptTupla>();
            List<ConceptTupla> localKeyTuplasForUpdate = new List<ConceptTupla>();
            string file = string.Empty;

            try
            {
                file = state as string;
                DateTime timeStart2 = DateTime.Now;

                lock (_xmlProcessingLock)
                {
                    XmlProcessing.Add(file.Trim());
                }
                if (File.Exists(file))
                {
                    CurrentLogManager.Log(string.Format("{0} xml file loaded", file));
                    //recupero l'xml
                    LocalizationResource res = LocalizationResource.Load(file);

                    //aggiungo l'xml nella struttura che li terrà in memoria
                    lock (_xmlDataLock)
                    {
                        XmlData.Add(res);
                    }

                    //leggo i campi dell'xml
                    var DBData = res.LocalizationSection.Select(r => new { ComponentNamespace = res.ComponentNamespace, InternalNamespace = r.InternalNamespace, ConceptID = r.Concept.Select(t => new { Id = t.Id, Comments = t.Comments, str = t.String.Select(st => st.Context) }) });

                    //Preparo la struttura letta da xml per effettuare le operazioni insiemistiche con le strutture lette da DB
                    foreach (var item in DBData)
                    {


                        foreach (var element in item.ConceptID)
                        {
                            string tmp = GetKey(item.ComponentNamespace, item.InternalNamespace, element.Id);
                            string com = null;
                            if (element.Comments != null)
                                com = element.Comments.TypedValue;
                            try
                            {
                                //inserisco la tupla nell'elenco globale di tutti gli xml letti
                                lock (_keyTuplasLock)
                                {
                                    KeyTuplas.Add(tmp, element.str);
                                }

                                lock (_keyCommentsLock)
                                {
                                    KeyComments.Add(tmp, com);
                                }

                            }
                            catch (Exception ex)
                            {
                                CurrentLogManager.Log(ex);
                                //segnalo eventuale ripetizione delle 3 chiavi presente in due xml distinti
                                lock (_errorsNoticeLock)
                                {
                                    ErrorsNotice.Add(string.Format("La tripla {0} nel file {1} risulta già presente in un xml precedentemente processato e pertanto viene scartata", tmp, file));
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CurrentLogManager.Log(ex);
                lock (_errorsNoticeLock)
                {
                    ErrorsNotice.Add(ex.Message);
                }
            }
            finally
            {
                Interlocked.Decrement(ref _interlock);

                lock (_xmlProcessedLock)
                {
                    XmlProcessed.Add(file.Trim());
                }

                lock (_xmlProcessingLock)
                {
                    XmlProcessing.Remove(file.Trim());
                }
            }

        }


        #endregion

        #region SetDBOnly

        private void WriteDB(Dictionary<string, IEnumerable<string>> keyTupla)
        {
            string CNamespace, INamespace, ConceptId;
            try
            {
                List<ConceptTupla> localKeyTuplasForInsert = new List<ConceptTupla>();
                List<ConceptTupla> localKeyTuplasForUpdate = new List<ConceptTupla>();
                IEnumerable<ConceptTupla> dbTuplasForUpdate;
                var concept = new UltraDBConcept.UltraDBConcept(context);
                UltraDBConcept2Context conceptToContext = new UltraDBConcept2Context(context);
                foreach (string str in keyTupla.Keys.Select(r => r))
                {
                    string cNamespace, iNamespace, conceptId;
                    IEnumerable<string> strs = keyTupla[str];

                    try
                    {
                        GetIndexes(str, out cNamespace, out iNamespace, out conceptId);
                    }
                    catch (Exception ex)
                    {
                        lock (_errorsNoticeLock)
                        {
                            ErrorsNotice.Add(ex.Message);
                        }
                        continue;
                    }

                    //riempo la struttura per l'insert
                    localKeyTuplasForInsert.Add(new ConceptTupla { ComponentNamespace = cNamespace, InternalNamespace = iNamespace, ConceptId = conceptId, Strings = strs });

                    foreach (string s in strs)
                    {
                        localKeyTuplasForUpdate.Add(new ConceptTupla { ComponentNamespace = cNamespace, InternalNamespace = iNamespace, ConceptId = conceptId, ContextId = UltraDBGlobal.UltraDBGlobal.GetContextId(s.Trim()).ToString() });
                    }
                }

                #region INSERIMENTO NUOVE TUPLE NEL DB
                //eseguo la differenza tra le triple lette nell'xml e quelle presenti nel DB. 
                //l'insieme ottenuto va inserito nel db sia nella tabella Concept che in quella Context2Concept (per la parte relativa alle stringhe)
                var tuplaToInsert = localKeyTuplasForInsert.Except(_dbTuplasForInsert, new TuplaComparerForInsert());
                ChangesFound = (tuplaToInsert.Count() > 0);
                InsertedCount = tuplaToInsert.Count();

                ////inserisco nel DB le nuove tuple sia nella Concept che nella Concept2Context
                foreach (var item in tuplaToInsert)
                {
                    //inserisco la tripla
                    CNamespace = item.ComponentNamespace;
                    INamespace = item.InternalNamespace;
                    ConceptId = item.ConceptId;
                    int conceptId = concept.InsertNewConcept(item.ComponentNamespace, item.InternalNamespace, item.ConceptId, false, null);
                    CurrentLogManager.Log(string.Format("Concept {0} has been inserted with Component Namespace {1} and Internal Namespace {2} ", item.ConceptId, item.ComponentNamespace, item.InternalNamespace));

                    //per ogni tripla inserita, inserisco la entry nella concept2context
                    foreach (string s in item.Strings)
                    {
                        conceptToContext.InsertNewConcept2Context(conceptId, UltraDBGlobal.UltraDBGlobal.GetContextId(s.Trim()));
                    }
                }
                #endregion

                #region UPDATE TUPLE DB

                dbTuplasForUpdate = concept.GetAllConceptsAndContext();

                var tuplaToUpdate = localKeyTuplasForUpdate.Except(dbTuplasForUpdate, new TuplaComparerForUpdate());
                ChangesFound = ChangesFound || (tuplaToUpdate.Count() > 0);
                UpdatedCount = tuplaToUpdate.Count();
                //inserisco nella Concept2Context le stringhe nuove associate a Concept già presenti
                foreach (var item in tuplaToUpdate)
                {
                    //ricavo l'ID
                    try
                    {
                        var id = dbTuplasForUpdate.Where(s => s.ComponentNamespace == item.ComponentNamespace && s.InternalNamespace == item.InternalNamespace && s.ConceptId == item.ConceptId).Select(s => s).FirstOrDefault().Id;
                        conceptToContext.InsertNewConcept2Context(Convert.ToInt32(id), Convert.ToInt32(item.ContextId.Trim()));

                        CurrentLogManager.Log(string.Format("Concept {0} has been updated with the context {1} ", item.ConceptId, UltraDBGlobal.UltraDBGlobal.GetContextName(Convert.ToInt32(item.ContextId.Trim()))));
                    }
                    catch (System.Exception ex)
                    {
                        CurrentLogManager.Log(ex);

                        string err = string.Format("Broken Concept at Component={0} Internal={1} Concept={2} Context= {3} ",
                                                    item.ComponentNamespace,
                                                    item.InternalNamespace,
                                                    item.ConceptId,
                                                    UltraDBGlobal.UltraDBGlobal.GetContextName(Convert.ToInt32(item.ContextId.Trim())));
                        CurrentLogManager.Log(err);
                        ErrorsNotice.Add(err);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                lock (_errorsNoticeLock)
                {
                    ErrorsNotice.Add(ex.Message);
                }
            }
            finally
            {
                //rilascio
                Completed.Set();
                if (OnCompleted != null)
                {
                    OnCompleted(this, null);
                }
            }
        }

        #endregion

        #endregion

        #region eventi

        public event EventHandler OnCompleted;
        public event EventHandler OnCompletedLists;

        #endregion

        #region Esportazione liste

        List<int> _conceptList;
        List<int> _conceptToContextList;

        public void GetLists()
        {
            List<ConceptTupla> localKeyTuplasForInsert = new List<ConceptTupla>();
            List<ConceptTupla> localKeyTuplasForUpdate = new List<ConceptTupla>();

            List<int> localC2CList = new List<int>();

            _conceptList = new List<int>();
            _conceptToContextList = new List<int>();

            ConceptList = new List<int>();
            Concept2ContextList = new List<int>();

            var concept = new UltraDBConcept.UltraDBConcept(context);
            //leggo dal db tutte le tuple della Concept
            _dbTuplasForConceptList = concept.GetFullConcepts();
            _dbTuplasForConcept2ContextList = concept.GetFullConceptsAndContext();
            CompletedLists = new AutoResetEvent(false);

            string[] files = Directory.GetFiles(_directory, "*" + _extension);

            if (files == null || files.Length == 0)
            {
                //rilascio
                CompletedLists.Set();
                if (OnCompleted != null)
                {
                    OnCompleted(this, null);
                }

                return;
            }


            foreach (string file in files)
            {
                if (!File.Exists(file))
                {
                    continue;
                }
                LoadLists(file, localKeyTuplasForInsert, localKeyTuplasForUpdate);
            }

            #region RICERCA TUPLE CHE SONO NELLA TABELLA CONCEPT DEL DB MA NON NELL'XML
            //eseguo la differenza tra le triple lette da DB e quelle presenti nell'xml. 
            //l'insieme ottenuto va inserito nella lista conceptList 
            var tuplaToConceptList = _dbTuplasForConceptList.Except(localKeyTuplasForInsert, new TuplaComparerForInsert());

            ////inserisco nel DB le nuove tuple sia nella Concept che nella Concept2Context
            foreach (var item in tuplaToConceptList)
            {
                _conceptList.Add(Convert.ToInt32(item.Id));
                //data la conceptList devo ricavare la relativa lista dei C2C connessa
                localC2CList.AddRange(concept.GetAllConcepts2ContextIDFromConcept(Convert.ToInt32(item.Id)));
            }
            #endregion



            #region RICERCA TUPLE CHE SONO NELLA CONCEPT2CONTEXT MA NON NELL'XML

            var TuplaToConcept2ContextList = _dbTuplasForConcept2ContextList.Except(localKeyTuplasForUpdate, new TuplaComparerForUpdate());

            //inserisco nella Concept2Context le stringhe nuove associate a Concept già presenti
            foreach (var item in TuplaToConcept2ContextList)
            {
                _conceptToContextList.Add(Convert.ToInt32(item.IdConcept2Context));

            }

            //dalla lista ricavata, tolgo quei c2c raggiungibili tramite la lista dei concepts

            var tempList = _conceptToContextList.Except(localC2CList);

            foreach (var item in tempList)
            {
                Concept2ContextList.Add(Convert.ToInt32(item));

            }
            #endregion

            ConceptList = _conceptList;


            CompletedLists.Set();
            if (OnCompletedLists != null)
            {
                OnCompletedLists(this, null);
            }

            return;
        }






        private void LoadLists(string state, List<ConceptTupla> localKeyTuplasForInsert, List<ConceptTupla> localKeyTuplasForUpdate)
        {


            string file = string.Empty;

            try
            {
                file = state;

                #region Carico l'xml
                if (File.Exists(file))
                {

                    //recupero l'xml
                    LocalizationResource res = LocalizationResource.Load(file);


                    //leggo i campi dell'xml
                    var DBData = res.LocalizationSection.Select(r => new { ComponentNamespace = res.ComponentNamespace, InternalNamespace = r.InternalNamespace, ConceptID = r.Concept.Select(t => new { Id = t.Id, Comments = t.Comments, str = t.String.Select(st => st.Context) }) });

                    //Preparo la struttura letta da xml per effettuare le operazioni insiemistiche con le strutture lette da DB
                    foreach (var item in DBData)
                    {

                        foreach (var element in item.ConceptID)
                        {
                            try
                            {
                                //riempo la struttura per l'insert
                                localKeyTuplasForInsert.Add(new ConceptTupla { ComponentNamespace = item.ComponentNamespace, InternalNamespace = item.InternalNamespace, ConceptId = element.Id, Strings = element.str });
                                //riempo la struttura per l'update
                                foreach (string s in element.str)
                                {
                                    localKeyTuplasForUpdate.Add(new ConceptTupla { ComponentNamespace = item.ComponentNamespace, InternalNamespace = item.InternalNamespace, ConceptId = element.Id, ContextId = UltraDBGlobal.UltraDBGlobal.GetContextId(s.Trim()).ToString() });
                                }
                            }
                            catch (Exception e)
                            {
                                CurrentLogManager.Log(e);
                                //nop
                            }
                        }
                    }

                    #endregion
                }

            }
            catch (Exception ex)
            {
                CurrentLogManager.Log(ex);
            }
        }
        #endregion
    }
}
