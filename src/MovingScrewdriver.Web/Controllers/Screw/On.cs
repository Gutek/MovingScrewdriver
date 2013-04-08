using System;
using System.Web.Mvc;
using MovingScrewdriver.Web.Models;
using MovingScrewdriver.Web.ViewModels;
using Raven.Client;

namespace MovingScrewdriver.Web.Controllers.Screw
{
    public partial class ScrewController : AbstractController
    {
         public ActionResult On()
         {
             var configExists = AssertConfigurationIsNeeded();

             if (configExists.IsNullOrEmpty())
             {
                 return View("on", ScrewdriverConfig.Create());
             }

             return Redirect(configExists);
         }

        [HttpPost]
        public ActionResult On(ScrewdriverConfig model)
        {
            var result = new AjaxResult();

            var configExists = AssertConfigurationIsNeeded();
            if (configExists.IsNotNullOrEmpty())
            {
                result.Success = false;
                result.Message = "Konfiguracja jest już stworzona, zaraz zostaniesz przeniesiony na stronę domową";
                result.RedirectUrl = Url.Action("AllPosts", "Posts");

                return Json(result);
            }

            if (!ModelState.IsValid)
            {
                result.Success = false;
                result.Message = "Nie wszystkie dane zostały poprawnie wypełnine, popraw błędy walidacyjne i spróbuj ponownie.";
                result.Errors = ValidationErrors;

                return Json(result);
            }

            model.Id = "Blog/Config";
            CurrentSession.Store(model);

            result.Success = true;
            result.Message = "Gratulacje utworzenia swojego nowego bloga! za kilka chwil zostaniesz przeniesiony na stronę z informacją o koncie użytkownika";
            //result.RedirectUrl = Url.Action("Done");

            return Json(result);
        }

        private string AssertConfigurationIsNeeded()
        {
            ScrewdriverConfig bc;

            // Bypass the aggressive caching to force loading the BlogConfig object,
            // otherwise we might get a null BlogConfig even though a valid one exists
            using (CurrentSession.Advanced.DocumentStore.DisableAggressiveCaching())
            {
                bc = CurrentSession.Load<ScrewdriverConfig>("Blog/Config");
            }

            if (bc != null)
            {
                return Url.Action("AllPosts", "Posts");
            }

            return null;
        }
    }
}