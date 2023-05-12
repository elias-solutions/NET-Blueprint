using BIT.NET.Backend.Blueprint.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspNetCore.Simple.MsTest.Sdk;
using BIT.NET.Backend.Blueprint.Integration.SimpleSdk.Tests.Environments;
using ObjectsComparer;

namespace BIT.NET.Backend.Blueprint.Integration.SimpleSdk.Tests.Api.Persons.V1
{
    [TestClass]
    [TestCategory("User API V1")]
    public class PersonsControllerTest : MsTestBase
    {
        private const string Route = "api/v1/persons";

        [TestMethod]
        public async Task PersonsController_Ok()
        {
            var addedPerson = await Client.AssertPostAsync<PersonDto>(Route, "AddPersonRequest.json", "AddedPersonResponse.json", FilterDifferences);
            await Client.AssertGetAsync<PersonDto>($"{Route}/{addedPerson.Id}", "GetPersonResponse.json", FilterDifferences);
            await Client.AssertGetAsync<IEnumerable<PersonDto>>(Route, "GetPersonsResponse.json", FilterDifferences);
            await Client.AssertDeleteAsync($"{Route}/{addedPerson.Id}");
            await Client.AssertGetAsync<IEnumerable<PersonDto>>(Route, "[]");
        }

        private IEnumerable<Difference> FilterDifferences(IEnumerable<Difference> diffs)
        {
            foreach (var difference in diffs)
            {
                if (difference.MemberPath.Contains(nameof(PersonDto.Id)) || 
                    difference.MemberPath.Contains(nameof(PersonDto.Created)) || 
                    difference.MemberPath.Contains(nameof(PersonDto.CreatedBy)))
                {
                    continue;
                }

                yield return difference;
            }
        }
    }
}