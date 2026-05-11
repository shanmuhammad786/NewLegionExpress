using System;
using System.Collections.Generic;
using Xamarin.Forms.PlatformConfiguration;

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

    public class refuseCollectionRequestModel
    {
        public int id { get; set; }
    }

    public class AcceptCollectionRequestListModel
    {
        public List<int> ids { get; set; }
    }
}

