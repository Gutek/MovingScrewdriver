using MovingScrewdriver.Web.Infrastructure;
using Xunit;

namespace MovingScrewdriver.Tests.infrastructure
{
    public class slug_converter_tests
    {
         [Fact]
         public void should_removes_normal_characters()
         {
             var result = SlugConverter.TitleToSlug("some: testing & for me");
             Assert.Equal("some-testing-for-me", result);

             result = SlugConverter.TitleToSlug("some-more Testing For Me");
             Assert.Equal("some-more-testing-for-me", result);

             result = SlugConverter.TitleToSlug("some~more Testing For Me");
             Assert.Equal("some-more-testing-for-me", result);

             result = SlugConverter.TitleToSlug("some.more Testing For Me");
             Assert.Equal("some-more-testing-for-me", result);

             result = SlugConverter.TitleToSlug("some'more Testing For Me");
             Assert.Equal("somemore-testing-for-me", result);

             result = SlugConverter.TitleToSlug("some\"more Testing For Me");
             Assert.Equal("somemore-testing-for-me", result);

             result = SlugConverter.TitleToSlug("some[more Testing For Me");
             Assert.Equal("some-more-testing-for-me", result);

             result = SlugConverter.TitleToSlug("some}more Testing For Me");
             Assert.Equal("some-more-testing-for-me", result);
         }

        [Fact]
        public void should_convert_polish_characters()
        {
            var result = SlugConverter.TitleToSlug("ąćęłńóśżź");
            Assert.Equal("acelnoszz", result);
        }
    }
}