using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechElevator.TagHelpers
{
    public class RepeatTagHelper : TagHelper
    {

        public int Count { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {

            Guid guid = Guid.NewGuid();
            output.TagName = "div";
            output.Attributes.Add("id", $"id-{guid}");
            output.Attributes.Add("class", "my-repeatable-class");

            for(var i = 0; i < Count; ++i )
            {
                output.Content.AppendHtml(await output.GetChildContentAsync(useCachedResult: false));
            }

        }
    }
}
