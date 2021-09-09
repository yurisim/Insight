using Insight.Core.Models;
using NUnit.Framework;
using System.Collections.Generic;
using Insight.Core.Helpers;
using FluentAssertions;

namespace Insight.Core.UnitTests.nUnit.HelpersTest
{
	[TestFixture]
	public class DataCalculationTests
	{

		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.GetReadinessPercentageTestCases))]
		public void GetReadinessPercentageTest(TestCaseObject testCaseParameters)
		{
			//arrange
			var (input, expectedOverallPercent) = testCaseParameters;

			//act
			decimal OverallPercent = DataCalculation.GetReadinessPercentage(input);

			//assert
			OverallPercent.Should().Be(expectedOverallPercent);
		}

		[TestCaseSource(typeof(TestCasesObjects), nameof(TestCasesObjects.GetReadinessPercentageByCatagoryTestCases))]
		public void GetReadinessPercentageByCatagoryTest(TestCaseObject testCaseParameters)
		{
			//arrange
			var (input, expectedMedicalPercent, expectedPersonnelPercent, expectedTraninPercent) = testCaseParameters;

			//act
			var result = DataCalculation.GetReadinessPerctageByCategory(input);
			var (OverallMedicalPercent, OverallPersonnelPercent, OverallTrainingPercent) = result;

			//assert
			OverallMedicalPercent.Should().Be(expectedMedicalPercent);
			OverallPersonnelPercent.Should().Be(expectedPersonnelPercent);
			OverallTrainingPercent.Should().Be(expectedTraninPercent);
		}

		private class TestCasesObjects
		{
			public static object[] GetReadinessPercentageTestCases =
			{
				new TestCaseObject(
					input : new List<Person>{
						new Person
						{
							Medical = new Medical { OverallStatus = Status.Current, },
							Personnel = new Personnel { OverallStatus = Status.Current, },
							Training = new Training { OverallStatus = Status.Current, }
						}
					},
					expectedOverallPercent : 1m
				),

				new TestCaseObject(
					input : new List<Person>{
						new Person
						{
							Medical = new Medical { OverallStatus = Status.Upcoming, },
							Personnel = new Personnel { OverallStatus = Status.Upcoming, },
							Training = new Training { OverallStatus = Status.Upcoming, }
						}
					},
					expectedOverallPercent : 1m
				),

				new TestCaseObject(
					input : new List<Person>{
						new Person
						{
							Medical = new Medical { OverallStatus = Status.Overdue, },
							Personnel = new Personnel { OverallStatus = Status.Current, },
							Training = new Training { OverallStatus = Status.Current, }
						}
					},
					expectedOverallPercent : 0m
				),

				new TestCaseObject(
					input : new List<Person>{
						new Person
						{
							FirstName = "first",
							LastName = "last",
							Medical = new Medical { OverallStatus = Status.Unknown, },
							Personnel = new Personnel { OverallStatus = Status.Current, },
							Training = new Training { OverallStatus = Status.Current, }
						}
					},
					expectedOverallPercent : 0m
				),

				new TestCaseObject(
					input : new List<Person>{
						new Person
						{
							Medical = new Medical { OverallStatus = Status.Unknown, },
							Personnel = new Personnel { OverallStatus = Status.Unknown, },
							Training = new Training { OverallStatus = Status.Unknown, }
						},
					},
					expectedOverallPercent : 0m
				),

				new TestCaseObject(
					input : new List<Person>{
						new Person
						{
							Personnel = new Personnel { OverallStatus = Status.Unknown, },
							Training = new Training { OverallStatus = Status.Unknown, }
						},
					},
					expectedOverallPercent : 0m
				),

				new TestCaseObject(
					input : new List<Person>{
						new Person
						{
							Medical = new Medical { OverallStatus = Status.Unknown, },
							Personnel = new Personnel { OverallStatus = Status.Unknown, },
							Training = new Training { OverallStatus = Status.Unknown, }
						},
						new Person
						{
							Medical = new Medical { OverallStatus = Status.Unknown, },
							Personnel = new Personnel { OverallStatus = Status.Unknown, },
							Training = new Training { OverallStatus = Status.Unknown, }
						}
					},
					expectedOverallPercent : 0m
				),

				new TestCaseObject(
					input : new List<Person>{
						new Person
						{
							Medical = new Medical { OverallStatus = Status.Unknown, },
							Personnel = new Personnel { OverallStatus = Status.Unknown, },
							Training = new Training { OverallStatus = Status.Unknown, }
						},
						new Person
						{
							Medical = new Medical { OverallStatus = Status.Current, },
							Personnel = new Personnel { OverallStatus = Status.Current, },
							Training = new Training { OverallStatus = Status.Current, }
						}
					},
					expectedOverallPercent : 0.5m
				),

				new TestCaseObject(
					input : new List<Person>{
						new Person
						{
							Medical = new Medical { OverallStatus = Status.Upcoming, },
							Personnel = new Personnel { OverallStatus = Status.Upcoming, },
							Training = new Training { OverallStatus = Status.Upcoming, }
						},
						new Person
						{
							Medical = new Medical { OverallStatus = Status.Current, },
							Personnel = new Personnel { OverallStatus = Status.Current, },
							Training = new Training { OverallStatus = Status.Current, }
						}
					},
					expectedOverallPercent : 1m
				),

				new TestCaseObject(
					input : new List<Person>{
						new Person
						{

						},
						new Person
						{
							Medical = new Medical { OverallStatus = Status.Current, },
							Personnel = new Personnel { OverallStatus = Status.Current, },
							Training = new Training { OverallStatus = Status.Current, }
						}
					},
					expectedOverallPercent : .5m
				),

				new TestCaseObject(
					input : new List<Person>{
						new Person
						{
						},
					},
					expectedOverallPercent : 0m
				),

				new TestCaseObject(
					input : new List<Person>{

					},
					expectedOverallPercent : 1m
				),

				new TestCaseObject(
					input : null,
					expectedOverallPercent : 1m
				),

			};

			public static object[] GetReadinessPercentageByCatagoryTestCases =
			{
				new TestCaseObject(
					input : new List<Person>{
						new Person
						{
							FirstName = "first",
							LastName = "last",
							Medical = new Medical { OverallStatus = Status.Current, },
							Personnel = new Personnel { OverallStatus = Status.Current, },
							Training = new Training { OverallStatus = Status.Current, }
						}
					},
					expectedMedicalPercent : 1m,
					expectedPersonnelPercent : 1m,
					expectedTraninPercent : 1m
				),

				new TestCaseObject(
					input : new List<Person>{
						new Person
						{
							FirstName = "first",
							LastName = "last",
							Medical = new Medical { OverallStatus = Status.Upcoming, },
							Personnel = new Personnel { OverallStatus = Status.Upcoming, },
							Training = new Training { OverallStatus = Status.Upcoming, }
						}
					},
					expectedMedicalPercent : 1m,
					expectedPersonnelPercent : 1m,
					expectedTraninPercent : 1m
				),

				new TestCaseObject(
					input : new List<Person>{
						new Person
						{
							FirstName = "first",
							LastName = "last",
							Medical = new Medical { OverallStatus = Status.Overdue, },
							Personnel = new Personnel { OverallStatus = Status.Overdue, },
							Training = new Training { OverallStatus = Status.Overdue, }
						}
					},
					expectedMedicalPercent : 0m,
					expectedPersonnelPercent : 0m,
					expectedTraninPercent : 0m
				),

				new TestCaseObject(
					input : new List<Person>{
						new Person
						{
							FirstName = "first",
							LastName = "last",
							Medical = new Medical { OverallStatus = Status.Current, },
							Personnel = new Personnel { OverallStatus = Status.Current, },
							Training = new Training { OverallStatus = Status.Current, }
						},
						new Person
						{
							FirstName = "first",
							LastName = "last",
							Medical = new Medical { OverallStatus = Status.Unknown },
							Personnel = new Personnel { OverallStatus = Status.Unknown, },
							Training = new Training { OverallStatus = Status.Unknown, }
						}
					},
					expectedMedicalPercent : 0.5m,
					expectedPersonnelPercent : 0.5m,
					expectedTraninPercent : 0.5m
				),

				new TestCaseObject(
					input : new List<Person>{
						new Person
						{
							FirstName = "first",
							LastName = "last",
							Medical = new Medical { OverallStatus = Status.Current, },
							Personnel = new Personnel { OverallStatus = Status.Unknown, },
							Training = new Training { OverallStatus = Status.Upcoming, }
						},
						new Person
						{
							FirstName = "first",
							LastName = "last",
							Medical = new Medical { OverallStatus = Status.Unknown },
							Personnel = new Personnel { OverallStatus = Status.Current, },
							Training = new Training { OverallStatus = Status.Unknown, }
						},
						new Person
						{
							FirstName = "first",
							LastName = "last",
							Medical = new Medical { OverallStatus = Status.Unknown },
							Personnel = new Personnel { OverallStatus = Status.Overdue, },
							Training = new Training { OverallStatus = Status.Current, }
						},
						new Person
						{
							FirstName = "first",
							LastName = "last",
							Medical = new Medical { OverallStatus = Status.Unknown },
							Personnel = new Personnel { OverallStatus = Status.Unknown, },
							Training = new Training { OverallStatus = Status.Overdue, }
						}
					},
					expectedMedicalPercent : 0.25m,
					expectedPersonnelPercent : 0.25m,
					expectedTraninPercent : 0.5m
				),
			};
		}

		public class TestCaseObject
		{
			private readonly IList<Person> _input;
			private readonly decimal _expectedMedicalPercent;
			private readonly decimal _expectedPersonnelPercent;
			private readonly decimal _expectedTraninPercent;
			private readonly decimal _expectedOverallPercent;

			public TestCaseObject(List<Person> input, decimal expectedMedicalPercent, decimal expectedPersonnelPercent, decimal expectedTraninPercent)
			{
				_input = input;
				_expectedMedicalPercent = expectedMedicalPercent;
				_expectedPersonnelPercent = expectedPersonnelPercent;
				_expectedTraninPercent = expectedTraninPercent;
			}

			public void Deconstruct(out IList<Person> input, out decimal expectedMedicalPercent, out decimal expectedPersonnelPercent, out decimal expectedTraninPercent)
			{
				input = _input;
				expectedMedicalPercent = _expectedMedicalPercent;
				expectedPersonnelPercent = _expectedPersonnelPercent;
				expectedTraninPercent = _expectedTraninPercent;
			}

			public TestCaseObject(List<Person> input, decimal expectedOverallPercent)
			{
				_input = input;
				_expectedOverallPercent = expectedOverallPercent;
			}

			public void Deconstruct(out IList<Person> input, out decimal expectedOverallPercent)
			{
				input = _input;
				expectedOverallPercent = _expectedOverallPercent;
			}

		}
	}
}
