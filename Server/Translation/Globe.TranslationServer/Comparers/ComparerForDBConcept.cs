using Globe.TranslationServer.Models;
using System;
using System.Collections.Generic;

namespace Globe.TranslationServer.Comparers
{
    internal class ComparerForDBConcept : IEqualityComparer<ConceptSearch>
    {
        // Products are equal if their names and product numbers are equal.
        public bool Equals(ConceptSearch x, ConceptSearch y)
        {

            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.StringId == y.StringId;

        }

        // If Equals() returns true for a pair of objects 
        // then GetHashCode() must return the same value for these objects.
        public int GetHashCode(ConceptSearch tupla)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(tupla, null)) return 0;

            //Get hash code for the  field if it is not null.
            int hashIDString = tupla.StringId.GetHashCode();

            //Calculate the hash code for the product.
            return hashIDString;
        }
    }
}