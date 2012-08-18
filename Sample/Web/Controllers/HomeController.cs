using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IronPython.Hosting;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private static dynamic PygmentScope;
        private static IEnumerable<SelectListItem> Lexers;
        private static IEnumerable<SelectListItem> Styles;

        public ActionResult Index()
        {

            return View(new SyntaxHighlighterModel { Styles = GetStyles(), Languages = GetLexers() });
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Index(SyntaxHighlighterModel model)
        {
            EnsurePygmentScope();

            var styles = GetStyles();
            var lanaguages = GetLexers();

            styles.Single(x => x.Value == model.Style).Selected = true;
            lanaguages.Single(x => x.Value == model.Language).Selected = true;
            //  var result = SyntaxHighlighterHelper.Highlight(model.Code, "CSharp", "Default");

            model.Styles = styles;
            model.Languages = lanaguages;
            model.HighlightedCode = PygmentScope.generate_html(model.Code, model.Language, model.Style);
            return View(model);
        }

        private void EnsurePygmentScope()
        {
            if (PygmentScope == null)
            {
                var ipy = Python.CreateRuntime();
                var engine = ipy.GetEngine("IronPython");
                engine.SetSearchPaths(new string[] { Server.MapPath("~/App_Data") });
                PygmentScope = ipy.UseFile("pygments_helper.py");

            }
        }

        private IEnumerable<SelectListItem> GetLexers()
        {
            if (Lexers == null)
            {
                EnsurePygmentScope();

                dynamic lexers = PygmentScope.get_lexers();

                IList<SelectListItem> lexerItems = new List<SelectListItem>();
                foreach (dynamic lexer in lexers())
                {
                    dynamic lookup = lexer[1];
                    lexerItems.Add(new SelectListItem
                    {
                        Text = lexer[0],
                        Value = lookup[0]
                    });
                }

                Lexers = lexerItems.OrderBy(x => x.Text).AsEnumerable();
            }

            return Lexers;
        }

        private IEnumerable<SelectListItem> GetStyles()
        {
            if (Styles == null)
            {
                EnsurePygmentScope();

                dynamic styles = PygmentScope.get_styles();

                IList<SelectListItem> styleItems = new List<SelectListItem>();
                foreach (string style in styles())
                {
                    styleItems.Add(new SelectListItem { Text = style, Value = style });
                }

                Styles = styleItems.OrderBy(x => x.Text).AsEnumerable(); ;
            }

            return Styles;
        }

    }
}
