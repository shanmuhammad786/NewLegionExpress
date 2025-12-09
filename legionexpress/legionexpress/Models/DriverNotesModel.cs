using System;
using System.Collections.Generic;

namespace legionexpress.Models
{
	public class DriverNotesRequestModel
	{
        public int id { get; set; }
        public string notes { get; set; }

    }

    public class UpdateDriverNotesResponse
    {
        public UpdateDriverNotesResult Result { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError { get; set; }
    }

    public class UpdateDriverNotesResult
    {
    }
}

