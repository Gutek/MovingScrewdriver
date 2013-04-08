using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MovingScrewdriver.Web.Extensions;
using MovingScrewdriver.Web.ViewModels;

namespace MovingScrewdriver.Web.Controllers.Aside
{
    public partial class AsideController : AbstractController
    {
        [ChildActionOnly]
         public ActionResult RecentComments()
         {
             var commentsTuples = CurrentSession.QueryForRecentComments(q => q.Take(5));

             var result = new List<RecentCommentViewModel>();
             foreach (var commentsTuple in commentsTuples)
             {
                 var recentCommentViewModel = Mapper.Map<RecentCommentViewModel>(commentsTuple.Item1);
                 
                 Mapper.Map(commentsTuple.Item2, 
                     recentCommentViewModel, 
                     commentsTuple.Item2.GetType(), 
                     typeof(RecentCommentViewModel));

                 result.Add(recentCommentViewModel);
             }

             return PartialView("recent_comments", result);
         }
    }
}