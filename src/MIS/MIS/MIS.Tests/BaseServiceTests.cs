namespace MIS.Tests
{
    using System.Reflection;

    using NUnit.Framework;

    using Services.Mapping;
    using Services.Models;

    using ViewModels.View.AdministratorManage;

    [TestFixture]
    public abstract class BaseServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            this.RegisterMappings();
        }


        private void RegisterMappings()
        {
            AutoMapperConfig.RegisterMappings(typeof(AdministratorShowUserViewModel).GetTypeInfo().Assembly,
                typeof(CompanyServiceModel).GetTypeInfo().Assembly);
        }
    }
}