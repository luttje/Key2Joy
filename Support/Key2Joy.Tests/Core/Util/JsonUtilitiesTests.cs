using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Key2Joy.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Key2Joy.Tests.Core.Util;

public class Person
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public Person Child { get; set; }

    public List<Person> Children { get; set; } = new();
}

[TestClass]
public class JsonUtilitiesTests
{
    [TestMethod]
    public void PopulateObject_PopulatesExistingObject()
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var person = new Person
        {
            FirstName = "Abc",
            Child = new Person
            {
                FirstName = "Foo"
            },
            Children =
            {
                new Person { FirstName = "Xyz" }
            }
        };

        var json = /*lang=json,strict*/ @"{ ""lastName"": ""Def"", ""child"": { ""lastName"": ""Bar"" }, ""children"": [ { ""lastName"": ""mno"" } ] }";
        JsonUtilities.PopulateObject(json, person, options);

        Assert.AreEqual("Abc", person.FirstName);
        Assert.AreEqual("Def", person.LastName);

        Assert.IsNull(person.Child.FirstName);
        Assert.AreEqual("Bar", person.Child.LastName);

        Assert.IsNull(person.Children.First().FirstName);
        Assert.AreEqual("mno", person.Children.First().LastName);
    }
}
