﻿using System;
using Facility.Definition.Fsd;
using Xunit;

namespace Facility.Definition.UnitTests
{
	internal static class TestUtility
	{
		public static void ThrowsServiceDefinitionException(Action action, ServiceTextPosition position)
		{
			try
			{
				action();
				Assert.True(false);
			}
			catch (ServiceDefinitionException exception)
			{
				Assert.Same(position, exception.Position);
			}
		}

		public static void ThrowsServiceDefinitionException(Func<object> func, ServiceTextPosition position)
		{
			ThrowsServiceDefinitionException(
				() =>
				{
					func();
				}, position);
		}

		public static ServiceInfo ParseTestApi(string text)
		{
			return new FsdParser().ParseDefinition(new ServiceTextSource("TestApi.fsd", text)).Service;
		}

		public static ServiceDefinitionException ParseInvalidTestApi(string text)
		{
			try
			{
				ParseTestApi(text);
				throw new InvalidOperationException("Parse did not fail.");
			}
			catch (ServiceDefinitionException exception)
			{
				return exception;
			}
		}

		public static string[] GenerateFsd(ServiceInfo service)
		{
			var generator = new FsdGenerator { GeneratorName = "TestUtility" };
			return generator.GenerateOutput(new ServiceDefinitionInfo(service)).Text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
		}
	}
}