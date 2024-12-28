﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by Reqnroll (https://www.reqnroll.net/).
//      Reqnroll Version:2.0.0.0
//      Reqnroll Generator Version:2.0.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace OpenEugene.Specs.LittleHelpBook.Features
{
    using Reqnroll;
    using System;
    using System.Linq;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "2.0.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public partial class AdvancedSearchFiltersFeature : object, Xunit.IClassFixture<AdvancedSearchFiltersFeature.FixtureData>, Xunit.IAsyncLifetime
    {
        
        private static global::Reqnroll.ITestRunner testRunner;
        
        private static string[] featureTags = ((string[])(null));
        
        private Xunit.Abstractions.ITestOutputHelper _testOutputHelper;
        
#line 1 "AdvancedSearch.feature"
#line hidden
        
        public AdvancedSearchFiltersFeature(AdvancedSearchFiltersFeature.FixtureData fixtureData, Xunit.Abstractions.ITestOutputHelper testOutputHelper)
        {
            this._testOutputHelper = testOutputHelper;
        }
        
        public static async System.Threading.Tasks.Task FeatureSetupAsync()
        {
            testRunner = global::Reqnroll.TestRunnerManager.GetTestRunnerForAssembly(null, global::Reqnroll.xUnit.ReqnrollPlugin.XUnitParallelWorkerTracker.Instance.GetWorkerId());
            global::Reqnroll.FeatureInfo featureInfo = new global::Reqnroll.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Features", "Advanced Search Filters", "As a front-room staff member, \r\nI want to use advanced search filters, \r\nSo that " +
                    "I can narrow down the search results to the most relevant services for clients.", global::Reqnroll.ProgrammingLanguage.CSharp, featureTags);
            await testRunner.OnFeatureStartAsync(featureInfo);
        }
        
        public static async System.Threading.Tasks.Task FeatureTearDownAsync()
        {
            string testWorkerId = testRunner.TestWorkerId;
            await testRunner.OnFeatureEndAsync();
            testRunner = null;
            global::Reqnroll.xUnit.ReqnrollPlugin.XUnitParallelWorkerTracker.Instance.ReleaseWorker(testWorkerId);
        }
        
        public async System.Threading.Tasks.Task TestInitializeAsync()
        {
        }
        
        public async System.Threading.Tasks.Task TestTearDownAsync()
        {
            await testRunner.OnScenarioEndAsync();
        }
        
        public void ScenarioInitialize(global::Reqnroll.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioInitialize(scenarioInfo);
            testRunner.ScenarioContext.ScenarioContainer.RegisterInstanceAs<Xunit.Abstractions.ITestOutputHelper>(_testOutputHelper);
        }
        
        public async System.Threading.Tasks.Task ScenarioStartAsync()
        {
            await testRunner.OnScenarioStartAsync();
        }
        
        public async System.Threading.Tasks.Task ScenarioCleanupAsync()
        {
            await testRunner.CollectScenarioErrorsAsync();
        }
        
        async System.Threading.Tasks.Task Xunit.IAsyncLifetime.InitializeAsync()
        {
            await this.TestInitializeAsync();
        }
        
        async System.Threading.Tasks.Task Xunit.IAsyncLifetime.DisposeAsync()
        {
            await this.TestTearDownAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Using Category Filters Given I am on the search page of the safety-net service di" +
            "rectory, When I apply filters for specific categories and sub-categories, Then I" +
            " should see services that match the selected categories and sub-categories.")]
        [Xunit.TraitAttribute("FeatureTitle", "Advanced Search Filters")]
        [Xunit.TraitAttribute("Description", "Using Category Filters Given I am on the search page of the safety-net service di" +
            "rectory, When I apply filters for specific categories and sub-categories, Then I" +
            " should see services that match the selected categories and sub-categories.")]
        public async System.Threading.Tasks.Task UsingCategoryFiltersGivenIAmOnTheSearchPageOfTheSafety_NetServiceDirectoryWhenIApplyFiltersForSpecificCategoriesAndSub_CategoriesThenIShouldSeeServicesThatMatchTheSelectedCategoriesAndSub_Categories_()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Using Category Filters Given I am on the search page of the safety-net service di" +
                    "rectory, When I apply filters for specific categories and sub-categories, Then I" +
                    " should see services that match the selected categories and sub-categories.", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 6
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 7
await testRunner.GivenAsync("the directory has a comprehensive list of categories and sub-categories,", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 8
await testRunner.WhenAsync("I select specific categories and sub-categories,", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 9
await testRunner.ThenAsync("the search results should reflect services under those categories and sub-categor" +
                        "ies.", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Combining Multiple Filters Given I have selected a category, When I apply additio" +
            "nal filters like location, eligibility, and service type, Then I should see serv" +
            "ices that match all the applied filters.")]
        [Xunit.TraitAttribute("FeatureTitle", "Advanced Search Filters")]
        [Xunit.TraitAttribute("Description", "Combining Multiple Filters Given I have selected a category, When I apply additio" +
            "nal filters like location, eligibility, and service type, Then I should see serv" +
            "ices that match all the applied filters.")]
        public async System.Threading.Tasks.Task CombiningMultipleFiltersGivenIHaveSelectedACategoryWhenIApplyAdditionalFiltersLikeLocationEligibilityAndServiceTypeThenIShouldSeeServicesThatMatchAllTheAppliedFilters_()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Combining Multiple Filters Given I have selected a category, When I apply additio" +
                    "nal filters like location, eligibility, and service type, Then I should see serv" +
                    "ices that match all the applied filters.", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 11
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 12
await testRunner.GivenAsync("a variety of filters are available,", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 13
await testRunner.WhenAsync("I combine multiple filters for a search,", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 14
await testRunner.ThenAsync("the directory should provide a list of services that meet all the criteria.", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [Xunit.SkippableFactAttribute(DisplayName="Saving Filtered Searches Given I have applied a set of filters for a search, When" +
            " I save this filtered search for future use, Then I should be able to access thi" +
            "s search quickly without reapplying the filters.")]
        [Xunit.TraitAttribute("FeatureTitle", "Advanced Search Filters")]
        [Xunit.TraitAttribute("Description", "Saving Filtered Searches Given I have applied a set of filters for a search, When" +
            " I save this filtered search for future use, Then I should be able to access thi" +
            "s search quickly without reapplying the filters.")]
        public async System.Threading.Tasks.Task SavingFilteredSearchesGivenIHaveAppliedASetOfFiltersForASearchWhenISaveThisFilteredSearchForFutureUseThenIShouldBeAbleToAccessThisSearchQuicklyWithoutReapplyingTheFilters_()
        {
            string[] tagsOfScenario = ((string[])(null));
            System.Collections.Specialized.OrderedDictionary argumentsOfScenario = new System.Collections.Specialized.OrderedDictionary();
            global::Reqnroll.ScenarioInfo scenarioInfo = new global::Reqnroll.ScenarioInfo("Saving Filtered Searches Given I have applied a set of filters for a search, When" +
                    " I save this filtered search for future use, Then I should be able to access thi" +
                    "s search quickly without reapplying the filters.", null, tagsOfScenario, argumentsOfScenario, featureTags);
#line 16
this.ScenarioInitialize(scenarioInfo);
#line hidden
            if ((global::Reqnroll.TagHelper.ContainsIgnoreTag(scenarioInfo.CombinedTags) || global::Reqnroll.TagHelper.ContainsIgnoreTag(featureTags)))
            {
                testRunner.SkipScenario();
            }
            else
            {
                await this.ScenarioStartAsync();
#line 17
await testRunner.GivenAsync("a set of applied filters,", ((string)(null)), ((global::Reqnroll.Table)(null)), "Given ");
#line hidden
#line 18
await testRunner.WhenAsync("I choose to save the search,", ((string)(null)), ((global::Reqnroll.Table)(null)), "When ");
#line hidden
#line 19
await testRunner.ThenAsync("the directory should allow me to retrieve the same filtered results later.", ((string)(null)), ((global::Reqnroll.Table)(null)), "Then ");
#line hidden
            }
            await this.ScenarioCleanupAsync();
        }
        
        [System.CodeDom.Compiler.GeneratedCodeAttribute("Reqnroll", "2.0.0.0")]
        [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
        public class FixtureData : object, Xunit.IAsyncLifetime
        {
            
            async System.Threading.Tasks.Task Xunit.IAsyncLifetime.InitializeAsync()
            {
                await AdvancedSearchFiltersFeature.FeatureSetupAsync();
            }
            
            async System.Threading.Tasks.Task Xunit.IAsyncLifetime.DisposeAsync()
            {
                await AdvancedSearchFiltersFeature.FeatureTearDownAsync();
            }
        }
    }
}
#pragma warning restore
#endregion
