using System;
using System.Linq;
using AutoMapper;
using MovingScrewdriver.Web.Infrastructure.AutoMapper;
using Xunit;

namespace MovingScrewdriver.Tests.auto_mapper
{
    public class auto_mapper_tests
    {
        [Fact]
        public void assert_configuration_valid()
        {
            var assembly = typeof(DefaultMappingProfile).Assembly;

            var profiles = assembly.GetTypes().Where(x => x.BaseType == typeof(Profile) && x.IsAbstract == false).ToList();

            foreach (var profile in profiles)
            {
                var instance = (Profile)Activator.CreateInstance(profile);
                Mapper.AddProfile(instance);
            }
            
            Mapper.AssertConfigurationIsValid();
        } 
    }
}