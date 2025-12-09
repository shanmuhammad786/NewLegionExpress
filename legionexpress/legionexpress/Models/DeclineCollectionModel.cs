using System;
using System.Collections.Generic;

namespace legionexpress.Models
{
	public class DeclineCollectionRequestModel
	{
        public int id { get; set; }
        public string declineNotes { get; set; }
    }

    public class DeclineCollectionResponse
    {
        public DeclineCollectionResult Result { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError { get; set; }
    }

    public class DeclineCollectionResult
    {
    }
}

