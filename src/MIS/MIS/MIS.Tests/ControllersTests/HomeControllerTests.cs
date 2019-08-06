namespace MIS.Tests.ControllersTests
{
    using MyTested.AspNetCore.Mvc;

    using NUnit.Framework;

    using ViewModels.Input.Home;

    using WebApp.Controllers;

    public class HomeControllerTests : BaseControllerTests
    {
        [Test]
        public void IndexShouldReturnView()
            => MyMvc.Controller<HomeController>()
                          .Calling(x => x.Index())
                          .ShouldReturn()
                          .View();

        [Test]
        public void PrivacyShouldReturnView()
            => MyMvc.Controller<HomeController>()
                    .Calling(x => x.Privacy())
                    .ShouldReturn()
                    .View();

        [Test]
        public void ContactShouldReturnView()
            => MyMvc.Controller<HomeController>()
                    .Calling(x => x.Contact())
                    .ShouldReturn()
                    .View();

        [Test]
        public void ContactShouldSendEmailAndRedirectToIndex()
            => MyMvc.Controller<HomeController>()
                    .Calling(x => x.Contact(new ContactCreateInputModel()
                    {
                        Description = "some description",
                        Email = "validEmail@abv.bg",
                        Name = "validName",
                        Subject = "validSubject"
                    }))
                    .ShouldHave()
                    .ValidModelState()
                    .AndAlso()
                    .ShouldHave()
                    .TempData(1)
                    .AndAlso()
                    .ShouldReturn()
                    .RedirectToAction("Index");

        [Test]
        public void ContactShouldReturn()
            => MyMvc.Controller<HomeController>()
                    .Calling(x => x.Contact(new ContactCreateInputModel()
                    {
                        Description = null,
                        Email = "invalidEmail",
                        Name = "validName",
                        Subject = "validSubject"
                    }))
                    .ShouldHave()
                    .InvalidModelState()
                    .AndAlso()
                    .ShouldHave()
                    .NoTempData()
                    .AndAlso()
                    .ShouldReturn()
                    .View(new ContactCreateInputModel()
                    {
                        Description = null,
                        Email = "invalidEmail",
                        Name = "validName",
                        Subject = "validSubject"
                    });

        [Test]
        public void ErrorShouldReturnView()
            => MyMvc.Controller<HomeController>()
                    .Calling(x => x.Error())
                    .ShouldReturn()
                    .View();
    }
}