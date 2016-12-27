﻿using System.Linq;
using NUnit.Framework;
using Shouldly;

namespace Facility.Definition.UnitTests.Fsd
{
	public sealed class ServiceTests
	{
		[Test]
		public void EmptyServiceDefinition()
		{
			var service = TestUtility.ParseTestApi("service TestApi{}");

			service.Name.ShouldBe("TestApi");
			service.Attributes.Count.ShouldBe(0);
			service.ErrorSets.Count.ShouldBe(0);
			service.Enums.Count.ShouldBe(0);
			service.Dtos.Count.ShouldBe(0);
			service.Methods.Count.ShouldBe(0);
			service.Summary.ShouldBe("");
			service.Remarks.Count.ShouldBe(0);

			TestUtility.GenerateFsd(service).ShouldBe(new[]
			{
				"// DO NOT EDIT: generated by TestUtility",
				"",
				"service TestApi",
				"{",
				"}",
				"",
			});
		}

		[Test]
		public void EmptyServiceDefinitionWithWhitespace()
		{
			var service = TestUtility.ParseTestApi(" \r\n\t service \r\n\t TestApi \r\n\t { \r\n\t } \r\n\t ");

			service.Name.ShouldBe("TestApi");

			TestUtility.GenerateFsd(service).ShouldBe(new[]
			{
				"// DO NOT EDIT: generated by TestUtility",
				"",
				"service TestApi",
				"{",
				"}",
				"",
			});
		}

		[Test]
		public void BlankServiceDefinition()
		{
			TestUtility.ParseInvalidTestApi("").Message.ShouldBe("TestApi.fsd(1,1): expected '[' or 'service'");
		}

		[Test]
		public void WhitespaceServiceDefinition()
		{
			TestUtility.ParseInvalidTestApi(" \r\n\t ").Message.ShouldBe("TestApi.fsd(2,3): expected '[' or 'service'");
		}

		[Test]
		public void MissingServiceName()
		{
			TestUtility.ParseInvalidTestApi("service{}").Message.ShouldBe("TestApi.fsd(1,8): expected service name");
		}

		[Test]
		public void MissingServiceBody()
		{
			TestUtility.ParseInvalidTestApi("service TestApi").Message.ShouldBe("TestApi.fsd(1,16): expected '{'");
		}

		[Test]
		public void MissingEndBrace()
		{
			TestUtility.ParseInvalidTestApi("service TestApi {").Message.ShouldBe("TestApi.fsd(1,18): expected '}' or '[' or 'data' or 'enum' or 'errors' or 'method'");
		}

		[Test]
		public void DuplicatedService()
		{
			TestUtility.ParseInvalidTestApi("service TestApi{} service TestApi{}").Message.ShouldBe("TestApi.fsd(1,19): expected end");
		}

		[Test]
		public void ServiceSummary()
		{
			var service = TestUtility.ParseTestApi("/// test\n/// summary\nservice TestApi{}");

			service.Summary.ShouldBe("test summary");

			TestUtility.GenerateFsd(service).ShouldBe(new[]
			{
				"// DO NOT EDIT: generated by TestUtility",
				"",
				"/// test summary",
				"service TestApi",
				"{",
				"}",
				"",
			});
		}

		[Test]
		public void ServiceRemarks()
		{
			var service = TestUtility.ParseTestApi("service TestApi{}\n# TestApi\ntest\nremarks");

			service.Remarks[1].ShouldBe("remarks");

			TestUtility.GenerateFsd(service).ShouldBe(new[]
			{
				"// DO NOT EDIT: generated by TestUtility",
				"",
				"service TestApi",
				"{",
				"}",
				"",
				"# TestApi",
				"",
				"test",
				"remarks",
				"",
			});
		}

		[Test]
		public void MethodRemarks()
		{
			var service = TestUtility.ParseTestApi("service TestApi { method do {}: {} }\n# do\nremarks");

			service.Methods.Single().Remarks.Single().ShouldBe("remarks");

			TestUtility.GenerateFsd(service).ShouldBe(new[]
			{
				"// DO NOT EDIT: generated by TestUtility",
				"",
				"service TestApi",
				"{",
				"\tmethod do",
				"\t{",
				"\t}:",
				"\t{",
				"\t}",
				"}",
				"",
				"# do",
				"",
				"remarks",
				"",
			});
		}

		[Test]
		public void DuplicatedRemarks()
		{
			TestUtility.ParseInvalidTestApi("service TestApi { method do {}: {} }\n# do\nremarks\n# do\nremarks")
				.Message.ShouldBe("TestApi.fsd(4,1): Duplicate remarks heading: do");
		}

		[Test]
		public void UnmatchedRemarks()
		{
			TestUtility.ParseInvalidTestApi("service TestApi{}\n# TestApi2\ntest\nremarks")
				.Message.ShouldBe("TestApi.fsd(2,1): Unused remarks heading: TestApi2");
		}
	}
}
