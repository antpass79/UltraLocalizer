using Globe.TranslationServer.Porting.UltraDBDLL.UltraDBConcept.Models;
using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Porting.UltraDBDLL.XmlManager
{
    public class TuplaComparerForUpdate : IEqualityComparer<ConceptTupla>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(ConceptTupla x, ConceptTupla y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.ComponentNamespace == y.ComponentNamespace && x.InternalNamespace == y.InternalNamespace && x.ConceptId == y.ConceptId && x.ContextId == y.ContextId;

        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.
        public int GetHashCode(ConceptTupla tupla)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(tupla, null)) return 0;

            //Get hash code for the  field if it is not null.
            int hashComponentNamespace = tupla.ComponentNamespace == null ? 0 : tupla.ComponentNamespace.GetHashCode();

            //Get hash code for the  field if it is not null.
            int hashInternalNamespace = tupla.InternalNamespace == null ? 0 : tupla.InternalNamespace.GetHashCode();

            //Get hash code for the  field if it is not null.
            int hashConceptId = tupla.ConceptId == null ? 0 : tupla.ConceptId.GetHashCode();

            //Get hash code for the  field if it is not null.
            int hashContextId = tupla.ContextId == null ? 0 : tupla.ContextId.GetHashCode();

            //Calculate the hash code for the product.
            return hashComponentNamespace ^ hashInternalNamespace ^ hashConceptId ^ hashContextId;
        }
    }
}
