using Application.ApplicationDto.Command;
using Application.ApplicationDto.Command.Responses;
using Common;
using Common.ResultInterfaces;
using Domain.Models;
using Radzen;
using System.Globalization;

namespace Presentation.Server.Components.Pages.AdminPages
{
    public partial class AddResource
    {
        List<string> resourceTypes = new List<string>() { "Hytte", "Campingvognplads", "Teltplads" };
        TextInfo textInfo = new CultureInfo("da-DK", false).TextInfo;

        class ResourceModel
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public decimal BasePrice { get; set; } = 0;
            public int Location { get; set; } = 1;
            public string? Description { get; set; }
        }

        ResourceModel resourceModel = new ResourceModel();

        private async Task CreateResource(ResourceModel resourceModel, DialogService ds)
        {
            UICreateResourceDto dto = Mapper.Map<UICreateResourceDto>(resourceModel);
            IResult<CreateResourceUIResponseDto> result = await _createResourceCommand.CreateResourceAsync(dto);

            ds.Close(true);

            if (result.IsSucces())
            {
                await SuccessDialog();
            }
            else if (result.IsError())
            {
                var errorMessage = result.GetError().Exception;
                await ErrorDialog(errorMessage);
            }

        }
    }
}
