using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Models
{
    public class SyntaxHighlighterModel
    {
        public string Language { get; set; }

        public string Style { get; set; }

        public string Code { get; set; }

        public string HighlightedCode { get; set; }

        public IEnumerable<SelectListItem> Styles { get; set; }

        public IEnumerable<SelectListItem> Languages { get; set; }
    }
}