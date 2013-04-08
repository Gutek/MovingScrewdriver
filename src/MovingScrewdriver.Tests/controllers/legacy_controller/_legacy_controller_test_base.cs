using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web.Routing;
using FakeItEasy;
using MovingScrewdriver.Web.Controllers.Legacy;

namespace MovingScrewdriver.Tests.controllers.legacy_controller
{
    public class legacy_controller_test_base : raven_db_controller_test_base
    {
        protected readonly LegacyController _controller;
        protected NameValueCollection _queryStrings;

        public legacy_controller_test_base()
        {
            _controller = new LegacyController { CurrentSession = Store.OpenSession() };
            _queryStrings = new NameValueCollection();

            A.CallTo(() => _requestBase.QueryString).Returns(_queryStrings);

            _controllerContext = new ControllerContext(_httpContext, new RouteData(), _controller);
            _controller.ControllerContext = _controllerContext;
        }

        public void execute(Action action)
        {
            action();
            _controller.CurrentSession.SaveChanges();
        }
    }
}