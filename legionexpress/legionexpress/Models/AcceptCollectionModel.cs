using System;
using System.Collections.Generic;

namespace legionexpress.Models
{
	public class AcceptCollectionRequestModel
	{
        public int id { get; set; }
    }

    public class AcceptCollectionResponse
    {
        public AcceptCollectionResult Result { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError { get; set; }
    }

    public class AcceptCollectionResult
    {
    }
}

