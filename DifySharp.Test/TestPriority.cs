using Xunit.Abstractions;
using Xunit.Sdk;

// using XUnit.Project.Attributes;

namespace DifySharp.Test;

/// <summary>
/// specify priority of test case
/// </summary>
/// <param name="priority"></param>
[AttributeUsage(AttributeTargets.Method)]
public class TestPriorityAttribute(int priority) : Attribute
{
    public int Priority { get; private set; } = priority;
}

public class PriorityOrderer : ITestCaseOrderer
{
    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(
        IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
    {
        string assemblyName  = typeof(TestPriorityAttribute).AssemblyQualifiedName!;
        var    sortedMethods = new SortedDictionary<int, List<TTestCase>>();
        foreach (var testCase in testCases)
        {
            var priority = testCase.TestMethod.Method
                .GetCustomAttributes(assemblyName)
                .FirstOrDefault()
                ?.GetNamedArgument<int>(nameof(TestPriorityAttribute.Priority)) ?? 0;

            GetOrCreate(sortedMethods, priority).Add(testCase);
        }

        foreach (var testCase in
                 sortedMethods.Keys.SelectMany(
                     priority => sortedMethods[priority].OrderBy(
                         testCase => testCase.TestMethod.Method.Name)))
        {
            yield return testCase;
        }
    }

    private static TValue GetOrCreate<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key)
        where TKey : struct
        where TValue : new() =>
        dictionary.TryGetValue(key, out TValue? result)
            ? result
            : (dictionary[key] = new TValue());
}